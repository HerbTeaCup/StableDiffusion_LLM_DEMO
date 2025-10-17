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
        image.preserveAspect = true; //�̷��� �����ν� ���� width, Height�� ���������� ����

        imageInfo = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>(); //���� Ȯ�强�� ������� ����
    }
    // �̺�Ʈ �ڵ鷯�� ����� ���� �޼���
    public void OnEndEditPropmt(string input)
    {
        //�ܼ��� ������Ʈ�� ������ ����.
        SDManager.Instance.txt2ImageBody.prompt = input;
    }
    public void OnEndEditNegativePropmt(string input)
    {
        //�ܼ��� ������Ʈ�� ������ ����.
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

    //�ݵ�� ���ν����忡�� ����Ǿ�� ��
    async void GenerateStart()
    {
        if (Generate.generating)
        {
            Debug.Log("Already generating");
            return;
        }
        SDsetting.ResponseParam.Txt2ImageOutBody response = await GenerateImage();

        // �̹��� ���ڿ��� ����Ʈ �迭�� ��ȯ
        byte[] imageData = Convert.FromBase64String(response.images[0]);

        //������ �̹��� ����
        try
        {
            //������ ����Ƽ�� Asset������ �����ϰ� �;�����
            //���� �ÿ� Asset������ ��Ű¡ �Ǳ� ������ �������� ������ ���� ����
            FileManager.SaveFileInDateFolder($"{DateTime.Now.ToString("yyyy-MM-dd_HH-m-s")}", imageData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save image: {e.Message}");
        }


        // ���� �ؽ�ó�� ��������Ʈ ����
        if (image.sprite != null)
        {
            DestroyImmediate(image.sprite.texture, true);
            DestroyImmediate(image.sprite, true);
        }

        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
        texture.LoadImage(imageData);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;

        // �̹��� ���� ������Ʈ
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

        _ = Task.Run(Clear); //�񵿱������� �޸� ����
    }

    async Task Clear()
    {
        await Task.Yield(); // ���� �����ӿ��� ���
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
}
