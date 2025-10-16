using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static SDsetting;
using static Communication;

public abstract class Generate : MonoBehaviour
{
    public static bool generating = false;

    public async Task ModelListAsync()
    {
        //TODO : 임시. 지금 에셋을 못만들어서 임시로 이렇게.
        var result = await GetRequestAsync<SDmodel[]>(sDurls.sd_modelsAPI, Communication.StalbeDiffusionBasicHeader);
        GameManager.sdManager.checkpoints = result;
    }

    //img2img는 만약 구현한다면 오버로딩할 거임
    /// <summary>
    /// 이미지 자체를 생성하고 반환하는 메소드.
    /// </summary>
    /// <param name="txt2ImageBody">요청 내용(프로폼트, 해상도, 샘플러 등등)이 담겨있는 요청 객체</param>
    /// <returns>image[], info </returns>
    protected async Task<ResponseParam.Txt2ImageOutBody> GenerateImage(RequestParams.Txt2ImageInBody txt2ImageBody)
    {
        if (generating)
        {
            Debug.Log("Already generating");
            return null;
        }

        string targetUrl = sDurls.txt2ImageAPI;
        generating = true;

        if (GameManager.sdManager.checkpoints == null)
            await ModelListAsync();
        if (GameManager.sdManager.config == null)
            GameManager.sdManager.config = await GetRequestAsync<Config>(sDurls.optionAPI, Communication.StalbeDiffusionBasicHeader);

        ResponseParam.Txt2ImageOutBody json =
            await PostRequestAsync<RequestParams.Txt2ImageInBody, ResponseParam.Txt2ImageOutBody>(targetUrl, txt2ImageBody);

        if(json == null || json.images == null)
        {
            generating = false;
            Debug.Log("Failed to generate image");
            return null;
        }

        generating = false;
        return json;
    }

    /// <summary>
    /// GameManager의 txt2ImageBody를 사용하여 이미지를 생성하는 메소드.
    /// </summary>
    /// <returns>이미지 정보 반환</returns>
    protected async Task<ResponseParam.Txt2ImageOutBody> GenerateImage()
    {
        if (generating)
        {
            Debug.Log("Already generating");
            return null;
        }

        string targetUrl = sDurls.txt2ImageAPI;
        generating = true;

        if (GameManager.sdManager.checkpoints == null)
            await ModelListAsync();
        if (GameManager.sdManager.config == null)
        {
            HeaderSetting header = new HeaderSetting(HeaderPurpose.Accept, sDurls.nameHeader, sDurls.valueHeader);

            GameManager.sdManager.config = await GetRequestAsync<Config>(sDurls.optionAPI, header);
        }

        ResponseParam.Txt2ImageOutBody json =
            await PostRequestAsync<RequestParams.Txt2ImageInBody, ResponseParam.Txt2ImageOutBody>(targetUrl, GameManager.sdManager.txt2ImageBody);

        if (json == null || json.images == null)
        {
            generating = false;
            Debug.Log("Failed to generate image");
            return null;
        }

        generating = false;
        return json;
    }
}
