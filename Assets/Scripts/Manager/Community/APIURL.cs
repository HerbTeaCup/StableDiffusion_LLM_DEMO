using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "APIURL", menuName = "ScriptableObjects/APIURL", order = 1)]
public class APIURL : ScriptableObject
{
    [Serializable]
    public struct URLs
    {
        public string nameHeader;
        public string address;
    }
    public enum APIType
    {
        StableDiffusion,
        LLM
    }

    [SerializeField] List<URLs> stableDiffusionUrls;
    [SerializeField] List<URLs> llmUrls;

    public Dictionary<(APIType, string), URLs> APIUrls = new Dictionary<(APIType, string), URLs>();

    private void OnEnable()
    {
        APIUrls.Clear();

        foreach (var urls in stableDiffusionUrls)
        {
            APIUrls[(APIType.StableDiffusion, urls.nameHeader)] = urls;
        }
        foreach (var urls in llmUrls)
        {
            APIUrls[(APIType.LLM, urls.nameHeader)] = urls;
        }
    }
}
