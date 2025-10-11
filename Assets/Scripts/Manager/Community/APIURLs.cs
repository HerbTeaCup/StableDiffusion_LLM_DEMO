using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Communication
{
    public static SDURLS sDurls = new ();
    public static OllamaURLS ollamaUrls = new();


    public class SDURLS
    {
        readonly public string nameHeader = "Accept";
        readonly public string valueHeader = "application/json";

        static readonly string BasicLocalAdress = "http://127.0.0.1:7860/";
        readonly public string progressAPI = BasicLocalAdress + "sdapi/v1/progress";
        readonly public string upscalerAPI = BasicLocalAdress + "sdapi/v1/upscalers";
        readonly public string latentupscalerAPI = BasicLocalAdress + "sdapi/v1/latent-upscale-modes";
        readonly public string sd_modelsAPI = BasicLocalAdress + "sdapi/v1/sd-models";
        readonly public string lorasAPI = BasicLocalAdress + "sdapi/v1/loras";
        readonly public string optionAPI = BasicLocalAdress + "sdapi/v1/options";
        readonly public string txt2ImageAPI = BasicLocalAdress + "sdapi/v1/txt2img";
        readonly public string samplerAPI = BasicLocalAdress + "sdapi/v1/samplers";
        readonly public string schedulerAPI = BasicLocalAdress + "sdapi/v1/schedulers";

        readonly public string pingAPI = BasicLocalAdress + "internal/ping";
    }

    public class OllamaURLS
    {
        static readonly string BasicLocalAddress = "http://localhost:11434/api/";
        readonly public string chatAPI = BasicLocalAddress + "chat";
        readonly public string generateAPI = BasicLocalAddress + "generate";
    }
}