using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


//이 컴포넌트는 일반적인 Slider의 역할을 하지 않으므로 BaseSlider를 상속받지 않음
//추후 필요하다면 BaseBar를 제작 후 상속
public class ProgressBar : MonoBehaviour
{
    UnityEngine.UI.Slider slider;

    [SerializeField] GameObject progressText;
    TextMeshProUGUI progressValue;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<UnityEngine.UI.Slider>();
        progressValue = progressText.GetComponent<TextMeshProUGUI>();

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatingBar();
    }

    void Init()
    {
        this.slider.value = 0;
        progressText.SetActive(false);
    }

    async void UpdatingBar()
    {
        progressText.SetActive(Generate.generating);

        if (Generate.generating == false)
            return;

        SDsetting.SDProgress value;

        while (Generate.generating)
        {
            value = await GetProgressValue();

            float progress = value.progress;
            float eta = value.eta_relative;

            slider.value = progress;
            progressValue.text = $"{Mathf.Round(progress * 100)}%, ETA = {Mathf.Round(eta)}s";

            await Task.Delay(500);
        }
    }

    async Task<SDsetting.SDProgress> GetProgressValue()
    {
        return await Communication.GetRequestAsync<SDsetting.SDProgress>(ManagerResister.
            GetManager<UrlManager>().StableDiffusion.GetUrl(StableDiffusionRequestPurpose.Progress), Communication.StalbeDiffusionBasicHeader);
    }
}
