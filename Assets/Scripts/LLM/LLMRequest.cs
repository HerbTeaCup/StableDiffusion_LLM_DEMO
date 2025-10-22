using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeminiLLM
{
    /// <summary>
    /// Gemini API ��û�� �ֻ��� ��Ʈ ��ü�Դϴ�.
    /// </summary>
    [Serializable]
    public class GeminiRequest
    {
        // �ý��� ������Ʈ (���� ����)
        [JsonProperty("system_instruction", NullValueHandling = NullValueHandling.Ignore)]
        public SystemInstruction SystemInstruction { get; set; }

        // ����ڿ� ���� �ְ���� ��ȭ ����
        [JsonProperty("contents")]
        public List<Content> Contents { get; set; } = new();

        // ���� �ɼ� (temperature, maxTokens ��) (���� ����)
        [JsonProperty("generationConfig", NullValueHandling = NullValueHandling.Ignore)]
        public GenerationConfig GenerationConfig { get; set; }

        // ���� ���� (���� ����)
        [JsonProperty("safetySettings", NullValueHandling = NullValueHandling.Ignore)]
        public List<SafetySetting> SafetySettings { get; set; }

        // �Լ� ȣ�� ��� (���� ����)
        [JsonProperty("tools", NullValueHandling = NullValueHandling.Ignore)]
        public List<Tool> Tools { get; set; }
    }

    //�ý��� ������Ʈ ����
    [Serializable]
    public class SystemInstruction
    {
        [JsonProperty("parts")]
        public List<Part> Parts { get; set; }
    }

    // ��ȭ�� �� ������ ��Ÿ���� Content ���� (���� + ����)
    [System.Serializable]
    public class Content
    {
        [JsonProperty("role")]
        public string Role { get; set; } // "user" �Ǵ� "model"

        [JsonProperty("parts")]
        public List<Part> Parts { get; set; }
    }

    //���� ����(�ؽ�Ʈ, �̹��� ��)�� ��� ����
    [Serializable]
    public class Part
    {
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
        [JsonProperty("inlineData", NullValueHandling = NullValueHandling.Ignore)]
        public InlineData InlineData { get; set; }
    }

    // Part ���ο� ���Ե� �̹��� ������ ����
    [Serializable]
    public class InlineData
    {
        [JsonProperty("mime_type")]
        public string MimeType { get; set; } // ��: "image/png", "image/jpeg"

        [JsonProperty("data")]
        public string Data { get; set; } // Base64�� ���ڵ��� �̹��� ������
    }

    // ���� �ɼ� �� ����
    [System.Serializable]
    public class GenerationConfig
    {
        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public float? Temperature { get; set; }

        [JsonProperty("topP", NullValueHandling = NullValueHandling.Ignore)]
        public float? TopP { get; set; }

        [JsonProperty("topK", NullValueHandling = NullValueHandling.Ignore)]
        public int? TopK { get; set; }

        [JsonProperty("maxOutputTokens", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxOutputTokens { get; set; }

        [JsonProperty("candidateCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? CandidateCount { get; set; }

        [JsonProperty("stopSequences", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> StopSequences { get; set; }
    }

    // ���� ���� �� ����
    [System.Serializable]
    public class SafetySetting
    {
        [JsonProperty("category")]
        public string Category { get; set; } // ��: "HARM_CATEGORY_HARASSMENT"

        [JsonProperty("threshold")]
        public string Threshold { get; set; } // ��: "BLOCK_NONE"
    }

    // ����(�Լ� ȣ��) �� ���� (��� ���)
    [System.Serializable]
    public class Tool
    {
        [JsonProperty("function_declarations")]
        public List<object> FunctionDeclarations { get; set; } // ���� ������ �� ����, �켱 object��
    }
}

