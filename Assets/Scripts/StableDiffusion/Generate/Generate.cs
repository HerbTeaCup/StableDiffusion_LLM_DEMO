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

    protected UrlManager urlManager => ManagerResister.GetManager<UrlManager>();

    public async Task ModelListAsync()
    {
        //TODO : �ӽ�. ���� ������ ������ �ӽ÷� �̷���.
        var result = await GetRequestAsync<SDmodel[]>
            (urlManager.StableDiffusion.GetUrl(StableDiffusionRequestPurpose.SDModels), Communication.StalbeDiffusionBasicHeader);
        ManagerResister.GetManager<SDManager>().checkpoints = result;
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

        string targetUrl = urlManager.StableDiffusion.GetUrl(StableDiffusionRequestPurpose.Txt2Img);
        HeaderSetting header = urlManager.StableDiffusion.GetHeader(HeaderPurpose.Accept);
        generating = true;

        if (ManagerResister.GetManager<SDManager>().checkpoints == null)
            await ModelListAsync();
        if (ManagerResister.GetManager<SDManager>().config == null)
            ManagerResister.GetManager<SDManager>().config 
                = await GetRequestAsync<Config>(urlManager.StableDiffusion.GetUrl(StableDiffusionRequestPurpose.Options), Communication.StalbeDiffusionBasicHeader);

        ResponseParam.Txt2ImageOutBody json =
            await PostRequestAsync<RequestParams.Txt2ImageInBody, ResponseParam.Txt2ImageOutBody>(targetUrl, header, ContentType.Json, txt2ImageBody);

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

        string targetUrl = urlManager.StableDiffusion.GetUrl(StableDiffusionRequestPurpose.Txt2Img);
        HeaderSetting header = urlManager.StableDiffusion.GetHeader(HeaderPurpose.Accept);
        generating = true;

        if (ManagerResister.GetManager<SDManager>().checkpoints == null)
            await ModelListAsync();
        if (ManagerResister.GetManager<SDManager>().config == null)
        {
            ManagerResister.GetManager<SDManager>().config 
                = await GetRequestAsync<Config>(urlManager.StableDiffusion.GetUrl(StableDiffusionRequestPurpose.Options), header);
        }

        ResponseParam.Txt2ImageOutBody json =
            await PostRequestAsync<RequestParams.Txt2ImageInBody, ResponseParam.Txt2ImageOutBody>(targetUrl, header, ContentType.Json, ManagerResister.GetManager<SDManager>().txt2ImageBody);

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
