using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlManager : ManagerBase<UrlManager>
{
    [Header("ScriptableObjects")]
    [SerializeField] APIConfigBase<GeminiRequestPurpose, URLSettingForGemini> gemini;
    [SerializeField] APIConfigBase<StableDiffusionRequestPurpose, URLSettingForStableDiffusion> stableDiffusion;

    public APIConfigBase<GeminiRequestPurpose, URLSettingForGemini> Gemini => gemini;
    public APIConfigBase<StableDiffusionRequestPurpose, URLSettingForStableDiffusion> StableDiffusion => stableDiffusion;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}
