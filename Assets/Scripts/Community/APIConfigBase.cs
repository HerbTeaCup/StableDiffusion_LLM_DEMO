using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum HeaderPurpose
{
    none,//�⺻��. ������� �ʽ��ϴ�.

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
    public string description; // �ܼ��� ������ ����. Asset Inspector���� Ȯ�� ����.
}


public abstract class APIConfigBase<TEnum, TUrlSetting> : ScriptableObject where TEnum : Enum where TUrlSetting : URLSetting<TEnum>
{
    [Serializable]
    public enum APIType
    {
        none,//�⺻��. ������� �ʽ��ϴ�.
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
            Debug.LogError("HeaderPurpose�� none���� �����Ǿ� �ֽ��ϴ�. �ùٸ� HeaderPurpose�� �������ּ���.");
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
            Debug.LogError("APIType�� none���� �����Ǿ� �ֽ��ϴ�. �ùٸ� APIType�� �������ּ���.");

        _headerCache.Clear();
        foreach(var header in headers)
        {
            if (!_headerCache.ContainsKey(header.headerPurpose))
                _headerCache.Add(header.headerPurpose, header);
            else
            {
                Debug.LogWarning($"�ߺ��� HeaderPurpose�� �ֽ��ϴ�: {header.headerPurpose}. ���� ó�� ������ ���� ���˴ϴ�.");
            }
        }

        _urlCache.Clear();
        foreach (var urlSetting in Urls) // �ڽ��� ������ ����Ʈ�� ���
        {
            if (!_urlCache.ContainsKey(urlSetting.purpose))
                _urlCache.Add(urlSetting.purpose, urlSetting);
        }
    }

    public virtual TUrlSetting GetUrl(TEnum purpose)
    {
        if (purpose.Equals(default(TEnum)))
        {
            Debug.LogError("URLPurpose�� none���� �����Ǿ� �ֽ��ϴ�. �ùٸ� URLPurpose�� �������ּ���.");
            throw new NullReferenceException();
        }
        if (_urlCache.TryGetValue(purpose, out var urlSetting))
        {
            return urlSetting;
        }
        Debug.LogError($"{typeof(TEnum).Name} {purpose}�� �ش��ϴ� URL�� �����ϴ�.");
        throw new KeyNotFoundException();
    }
}
