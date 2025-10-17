using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


//�� ������Ʈ�� �Ϲ����� Slider�� ������ ���� �����Ƿ� BaseSlider�� ��ӹ��� ����
//���� �ʿ��ϴٸ� BaseBar�� ���� �� ���
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
