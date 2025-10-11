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

public class SDManager : MonoBehaviour, IAsyncElement
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
        GameManager.asyncManager.asyncElements.Add(this);
    }

    public async Task Init()
    {
        config = await GetRequestAsync<Config>(sDurls.optionAPI);

        if (GameManager.sdManager.config.samples_save == false || GameManager.sdManager.config.save_images_add_number == false)
        {
            GameManager.sdManager.config.samples_save = true;
            GameManager.sdManager.config.save_images_add_number = true;
            //GameManager.sdManager.config.outdir_img2img_samples
            await PostRequestAsync<Config>(sDurls.optionAPI, GameManager.sdManager.config);
        }
    }
}
