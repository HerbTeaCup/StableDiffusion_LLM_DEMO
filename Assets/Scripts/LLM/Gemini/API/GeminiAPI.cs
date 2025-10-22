using GeminiLLM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GeminiRequestPurpose
{
    None,//None은 사용하지 않음. 기본
    GenerateContent,
    StreamGeneratedContent,
}

[Serializable]
public class URLSettingForGemini : URLSetting<GeminiRequestPurpose> { }

[CreateAssetMenu(fileName = "GeminiAPI", menuName = "API/GeminiAPI", order = 2)]
public class GeminiAPI : APIConfigBase<GeminiRequestPurpose, URLSettingForGemini>
{
    [SerializeField] List<URLSettingForGemini> urls;

    protected override List<URLSettingForGemini> Urls => urls;

    override protected void OnEnable()
    {
        base.OnEnable();
        apiType = APIType.GeminiLLM;
    }
}
