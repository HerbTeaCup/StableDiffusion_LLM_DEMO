using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Text;
using static SDsetting;
using static Communication;
using System.Threading.Tasks;

public class SDManager : SingletonManager<SDManager>, IAsyncElement
{
    //SD Model
    public SDmodel[] checkpoints;
    public int CheckpointIndex = 0;

    //Sampler
    public SDsampler[] samplerModels;
    public int SamplerModelIndex = 0;

    //Upscaler
    public UpscalerModel[] upscalerModels;
    public int UpscalerModelIndex = 0;

    //scheduler
    public SDscheduler[] schedulerModels;
    public int SchedulerModelIndex = 0;

    /// <summary>
    /// �� ���� �ʹ� �߿���. ����ÿ� �ݵ�� �ٽ� ������ ���� �ٽ� �޾ƿð�.
    /// </summary>
    public Config config { get; set; } //���ɰ�ü ���� ���� ������Ƽ ����
    public RequestParams.Txt2ImageInBody txt2ImageBody = new RequestParams.Txt2ImageInBody(); //��Ȱ���� ���� txt2ImageBody ��ü ����

    private void OnEnable()
    {
        AsyncManager.Instance.asyncElements.Add(this);
    }

    public async Task Init()
    {
        config = await GetRequestAsync<Config>(sDurls.optionAPI, Communication.StalbeDiffusionBasicHeader);

        if (SDManager.Instance.config.samples_save == false || SDManager.Instance.config.save_images_add_number == false)
        {
            SDManager.Instance.config.samples_save = true;
            SDManager.Instance.config.save_images_add_number = true;
            //SDManager.Instance.config.outdir_img2img_samples
            await PostRequestAsync<Config>(sDurls.optionAPI, SDManager.Instance.config);
        }
    }
}
