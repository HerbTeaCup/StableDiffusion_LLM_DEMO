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
    /// 이 값은 너무 중요함. 변경시에 반드시 다시 서버의 값을 다시 받아올것.
    /// </summary>
    public Config config { get; set; } //유령객체 생성 방지 프로퍼티 선언
    public RequestParams.Txt2ImageInBody txt2ImageBody = new(); //재활용을 위한 txt2ImageBody 객체 생성

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
