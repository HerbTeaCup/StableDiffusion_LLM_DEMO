using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GerateImage : Generate
{
    UnityEngine.UI.Image image;
    TMPro.TextMeshProUGUI imageInfo;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        image.preserveAspect = true; //이렇게 함으로써 각종 width, Height에 유동적으로 대응

        imageInfo = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>(); //딱히 확장성을 고려하지 않음
    }
    // 이벤트 핸들러로 사용할 동기 메서드
    public void OnEndEditPropmt(string input)
    {
        //단순히 프로폼트만 수정할 것임.
        SDManager.Instance.txt2ImageBody.prompt = input;
    }
    public void OnEndEditNegativePropmt(string input)
    {
        //단순히 프로폼트만 수정할 것임.
        SDManager.Instance.txt2ImageBody.negative_prompt = input;
    }
    public void OnClick()
    {
        if (AsyncManager.Instance.connected == false)
        {
            Debug.LogError("Disconnected from WebUI...");
            return;
        }

        GenerateStart();
    }

    //반드시 메인스레드에서 실행되어야 함
    async void GenerateStart()
    {
        if (Generate.generating)
        {
            Debug.Log("Already generating");
            return;
        }
        SDsetting.ResponseParam.Txt2ImageOutBody response = await GenerateImage();

        // 이미지 문자열을 바이트 배열로 변환
        byte[] imageData = Convert.FromBase64String(response.images[0]);

        //생성된 이미지 저장
        try
        {
            //원래는 유니티의 Asset폴더에 저장하고 싶었지만
            //빌드 시에 Asset폴더는 패키징 되기 때문에 동적으로 저장할 수가 없음
            FileManager.SaveFileInDateFolder($"{DateTime.Now.ToString("yyyy-MM-dd_HH-m-s")}", imageData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save image: {e.Message}");
        }


        // 기존 텍스처와 스프라이트 해제
        if (image.sprite != null)
        {
            DestroyImmediate(image.sprite.texture, true);
            DestroyImmediate(image.sprite, true);
        }

        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
        texture.LoadImage(imageData);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;

        // 이미지 정보 업데이트
        imageInfo.text =
            $"Prompt: {response.parameters.prompt}\n" +
            $"Negative prompt: {response.parameters.negative_prompt}\n" +
            $"Steps: {response.parameters.steps}, " +
            $"Sampler: {response.parameters.sampler_name}, " +
            $"CFG scale: {response.parameters.cfg_scale}, " +
            $"Seed: {response.parameters.seed}, " +
            $"Size: {texture.width}x{texture.height}, " +
            $"Model: {SDManager.Instance.config.sd_model_checkpoint?.ToString() ?? "Unknown"}\n" +
            $"Denoising strength: {response.parameters.denoising_strength}, ";
        if (response.parameters.enable_hr)
        {
            imageInfo.text +=
                $"HR upscaler: {response.parameters.hr_upscaler}, " +
                $"HR second pass steps: {response.parameters.hr_second_pass_steps}, ";
        }

        _ = Task.Run(Clear); //비동기적으로 메모리 해제
    }

    async Task Clear()
    {
        await Task.Yield(); // 현재 프레임에서 대기
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
}
