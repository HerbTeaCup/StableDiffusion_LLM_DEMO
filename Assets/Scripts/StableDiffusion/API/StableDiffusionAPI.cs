using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StableDiffusionRequestPurpose
{
    none,//기본값. 사용하지 않습니다.
    Progress,
    Upscaler,
    SDModels,
    Loras,
    Options,
    Txt2Img,
    Samplers,
    Schedulers,
    Ping,
}

[System.Serializable]
public class URLSettingForStableDiffusion : URLSetting<StableDiffusionRequestPurpose> { }

[CreateAssetMenu(fileName = "StableDiffusionAPI", menuName = "API/StableDiffusionAPI", order = 1)]
public class StableDiffusionAPI : APIConfigBase<StableDiffusionRequestPurpose, URLSettingForStableDiffusion>
{
    [SerializeField] List<URLSettingForStableDiffusion> urls;
    protected override List<URLSettingForStableDiffusion> Urls => urls;
    protected override void OnEnable()
    {
        base.OnEnable();

        apiType = APIType.StableDiffusion;
    }
}
