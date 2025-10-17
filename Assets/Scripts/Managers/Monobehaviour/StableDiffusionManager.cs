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
    /// 이 값은 너무 중요함. 변경시에 반드시 다시 서버의 값을 다시 받아올것.
    /// </summary>
    public Config config { get; set; } //유령객체 생성 방지 프로퍼티 선언
    public RequestParams.Txt2ImageInBody txt2ImageBody = new RequestParams.Txt2ImageInBody(); //재활용을 위한 txt2ImageBody 객체 생성

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
