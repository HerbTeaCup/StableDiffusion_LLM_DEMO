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
    /// 오늘 날짜 기준 폴더를 생성하고 그 경로를 반환
    /// </summary>
    string GetOrCreateDateFolder()
    {
        string today = System.DateTime.Now.ToString("yyyy-MM-dd");
        string folderPath = Path.Combine(Application.persistentDataPath, today);    //이미지 동적 생성하려면 이 경로밖에 마땅한 곳이 없음

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("폴더 생성됨: " + folderPath);
        }

        return folderPath;
    }

    /// <summary>
    /// 파일을 날짜 폴더 안에 생성
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
        Debug.Log("파일 저장됨: " + fullPath);
    }
}

