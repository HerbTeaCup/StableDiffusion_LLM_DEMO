using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StableDiffusionRequestPurpose
{
    none,//기본값. 사용하지 않습니다.
    Progress,
    Upscaler,
    LatentUpscaler,
    SDModels,
    Loras,
    Options,
    Txt2Img,
    Samplers,
    Schedulers,
    Ping,
}

[System.Serializable]
public class StableDiffusionURLSetting : URLSetting<StableDiffusionRequestPurpose> { }

[CreateAssetMenu(fileName = "StableDiffusionAPI", menuName = "API/StableDiffusionAPI", order = 1)]
public class StableDiffusionAPI : APIConfigBase<StableDiffusionRequestPurpose, StableDiffusionURLSetting>
{
    [SerializeField] List<StableDiffusionURLSetting> urls;
    protected override List<StableDiffusionURLSetting> Urls => urls;
    protected override void OnEnable()
    {
        base.OnEnable();

        apiType = APIType.StableDiffusion;
    }
}
