using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class FileManager
{
    public enum ImageFormat
    {
        PNG,
        JPG
    }

    /// <summary>
    /// ���� ��¥ ���� ������ �����ϰ� �� ��θ� ��ȯ
    /// </summary>
    string GetOrCreateDateFolder()
    {
        string today = System.DateTime.Now.ToString("yyyy-MM-dd");
        string folderPath = Path.Combine(Application.persistentDataPath, today);    //�̹��� ���� �����Ϸ��� �� ��ιۿ� ������ ���� ����

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("���� ������: " + folderPath);
        }

        return folderPath;
    }

    /// <summary>
    /// ������ ��¥ ���� �ȿ� ����
    /// </summary>
    public void SaveFileInDateFolder(string filename, byte[] data, ImageFormat format = ImageFormat.PNG)
    {
        switch (format)
        {
            case ImageFormat.PNG:
                if (!filename.EndsWith(".png"))
                {
                    filename += ".png";
                }
                break;
            case ImageFormat.JPG:
                if (!filename.EndsWith(".jpg"))
                {
                    filename += ".jpg";
                }
                break;
        }

        string folder = GetOrCreateDateFolder();
        string fullPath = Path.Combine(folder, filename);

        File.WriteAllBytes(fullPath, data);
        Debug.Log("���� �����: " + fullPath);
    }
}

