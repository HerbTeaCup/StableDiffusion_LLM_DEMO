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

public class SDManager : ManagerBase<SDManager>, IAsyncElement
{
    public override int Order => 10;

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
    public RequestParams.Txt2ImageInBody txt2ImageBody = new(); //��Ȱ���� ���� txt2ImageBody ��ü ����

    UrlManager _urlManager;
    AsyncManager _asyncManager;

    private void Start()
    {
        _asyncManager.AsyncElements.Add(this);
    }

    public async Task Init()
    {
        string optionUrl = _urlManager.StableDiffusion.GetUrl(StableDiffusionRequestPurpose.Options);
        HeaderSetting header = _urlManager.StableDiffusion.GetHeader(HeaderPurpose.Accept);

        config = await GetRequestAsync<Config>(optionUrl, header);

        if (this.config.samples_save == false || ManagerResister.GetManager<SDManager>().config.save_images_add_number == false)
        {
            this.config.samples_save = true;
            this.config.save_images_add_number = true;
            //ManagerResister.GetManager<SDManager>().config.outdir_img2img_samples

            await PostRequestAsync<Config>(optionUrl, header, ContentType.Json, this.config);
        }
    }

    public override void AfterAllManagerInitialized()
    {
        _urlManager = ManagerResister.GetManager<UrlManager>();
        _asyncManager = ManagerResister.GetManager<AsyncManager>();
    }
}
