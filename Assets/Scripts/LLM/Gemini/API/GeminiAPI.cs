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
    protected override List<URLSettingForGemini> Urls => urls;
    [SerializeField] List<URLSettingForGemini> urls;

    public override HeaderSetting GetHeader(HeaderPurpose purpose)
    {
        if (purpose == HeaderPurpose.none)
        {
            Debug.LogError("HeaderPurpose가 none으로 설정되어 있습니다. 올바른 HeaderPurpose를 선택해주세요.");
            return default;
        }

        HeaderSetting headerSetting;

        if (_headerCache.ContainsKey(purpose))
        {
            headerSetting = _headerCache[purpose];
        }
        else
        {
            Debug.LogError($"HeaderPurpose '{purpose}'에 해당하는 HeaderSetting이 존재하지 않습니다.");
            return default;
        }

        if (purpose == HeaderPurpose.XGoogleApiKey)
        {
            headerSetting.value = APIKeyResister.GeminiKey;
        }

        if (_headerCache.TryGetValue(purpose, out var header))
        {
            return header;
        }

        return default;
    }

    override protected void OnEnable()
    {
        base.OnEnable();
        apiType = APIType.GeminiLLM;
    }
}
