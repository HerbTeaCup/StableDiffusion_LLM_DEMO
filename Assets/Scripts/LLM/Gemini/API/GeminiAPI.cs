using GeminiLLM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GeminiRequestPurpose
{
    None,//None�� ������� ����. �⺻
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
            Debug.LogError("HeaderPurpose�� none���� �����Ǿ� �ֽ��ϴ�. �ùٸ� HeaderPurpose�� �������ּ���.");
            return default;
        }

        HeaderSetting headerSetting;

        if (_headerCache.ContainsKey(purpose))
        {
            headerSetting = _headerCache[purpose];
        }
        else
        {
            Debug.LogError($"HeaderPurpose '{purpose}'�� �ش��ϴ� HeaderSetting�� �������� �ʽ��ϴ�.");
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
