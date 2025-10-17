using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GeminiRequestPurpose
{
    GenerateContent,
    StreamGeneratedContent,
    CountTokens,
    GetModelInfo,
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
