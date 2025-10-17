using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum HeaderPurpose
{
    none,//기본값. 사용하지 않습니다.

    //Common
    Authorization,
    ContentType,
    Accept,

    //For Gemini
    XGoogleApiKey,
}
[Serializable]
public struct HeaderSetting
{
    public HeaderPurpose headerPurpose;
    public string name;
    public string value;

    public HeaderSetting(HeaderPurpose purpose, string name, string value)
    {
        this.headerPurpose = purpose;
        this.name = name;
        this.value = value;
    }
    public HeaderSetting(HeaderPurpose purpose, string name)
    {
        this.headerPurpose = purpose;
        this.name = name;
        value = default;
    }
}
[Serializable]
public class URLSetting<TEnum> where TEnum : Enum
{
    public TEnum purpose;
    public string url;
    public string description; // 단순히 설명을 위함. Asset Inspector에서 확인 가능.
}


public abstract class APIConfigBase<TEnum, TUrlSetting> : ScriptableObject where TEnum : Enum where TUrlSetting : URLSetting<TEnum>
{
    [Serializable]
    public enum APIType
    {
        none,//기본값. 사용하지 않습니다.
        StableDiffusion,
        GeminiLLM
    }

    [Header("API Information")]
    public APIType apiType;
    
    [Header("Setting")]
    [SerializeField] protected List<HeaderSetting> headers;
    Dictionary<HeaderPurpose, HeaderSetting> _headerCache = new();

    protected abstract List<TUrlSetting> Urls { get; }
    Dictionary<TEnum, TUrlSetting> _urlCache = new();

    public HeaderSetting GetHeader(HeaderPurpose purpose)
    {
        if(purpose == HeaderPurpose.none)
        {
            Debug.LogError("HeaderPurpose가 none으로 설정되어 있습니다. 올바른 HeaderPurpose를 선택해주세요.");
            return default;
        }

        if (_headerCache.TryGetValue(purpose, out var header))
        {
            return header;
        }

        return default;
    }


    protected virtual void OnEnable()
    {
        if (apiType == APIType.none)
            Debug.LogError("APIType이 none으로 설정되어 있습니다. 올바른 APIType을 선택해주세요.");

        _headerCache.Clear();
        foreach(var header in headers)
        {
            if (!_headerCache.ContainsKey(header.headerPurpose))
                _headerCache.Add(header.headerPurpose, header);
            else
            {
                Debug.LogWarning($"중복된 HeaderPurpose가 있습니다: {header.headerPurpose}. 가장 처음 설정된 값이 사용됩니다.");
            }
        }

        _urlCache.Clear();
        foreach (var urlSetting in Urls) // 자식이 제공한 리스트를 사용
        {
            if (!_urlCache.ContainsKey(urlSetting.purpose))
                _urlCache.Add(urlSetting.purpose, urlSetting);
        }
    }

    public virtual TUrlSetting GetUrl(TEnum purpose)
    {
        if (purpose.Equals(default(TEnum)))
        {
            Debug.LogError("URLPurpose가 none으로 설정되어 있습니다. 올바른 URLPurpose를 선택해주세요.");
            throw new NullReferenceException();
        }
        if (_urlCache.TryGetValue(purpose, out var urlSetting))
        {
            return urlSetting;
        }
        Debug.LogError($"{typeof(TEnum).Name} {purpose}에 해당하는 URL이 없습니다.");
        throw new KeyNotFoundException();
    }
}
