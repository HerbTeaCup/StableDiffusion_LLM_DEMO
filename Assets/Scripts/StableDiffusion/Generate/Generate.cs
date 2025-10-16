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
        //TODO : �ӽ�. ���� ������ ������ �ӽ÷� �̷���.
        var result = await GetRequestAsync<SDmodel[]>(sDurls.sd_modelsAPI, Communication.StalbeDiffusionBasicHeader);
        GameManager.sdManager.checkpoints = result;
    }

    //img2img�� ���� �����Ѵٸ� �����ε��� ����
    /// <summary>
    /// �̹��� ��ü�� �����ϰ� ��ȯ�ϴ� �޼ҵ�.
    /// </summary>
    /// <param name="txt2ImageBody">��û ����(������Ʈ, �ػ�, ���÷� ���)�� ����ִ� ��û ��ü</param>
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
    /// GameManager�� txt2ImageBody�� ����Ͽ� �̹����� �����ϴ� �޼ҵ�.
    /// </summary>
    /// <returns>�̹��� ���� ��ȯ</returns>
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
