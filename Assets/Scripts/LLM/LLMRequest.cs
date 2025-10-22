using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeminiLLM
{
    /// <summary>
    /// Gemini API 요청의 최상위 루트 객체입니다.
    /// </summary>
    [Serializable]
    public class GeminiRequest
    {
        // 시스템 프롬프트 (선택 사항)
        [JsonProperty("system_instruction", NullValueHandling = NullValueHandling.Ignore)]
        public SystemInstruction SystemInstruction { get; set; }

        // 사용자와 모델이 주고받은 대화 내용
        [JsonProperty("contents")]
        public List<Content> Contents { get; set; } = new();

        // 생성 옵션 (temperature, maxTokens 등) (선택 사항)
        [JsonProperty("generationConfig", NullValueHandling = NullValueHandling.Ignore)]
        public GenerationConfig GenerationConfig { get; set; }

        // 안전 설정 (선택 사항)
        [JsonProperty("safetySettings", NullValueHandling = NullValueHandling.Ignore)]
        public List<SafetySetting> SafetySettings { get; set; }

        // 함수 호출 기능 (선택 사항)
        [JsonProperty("tools", NullValueHandling = NullValueHandling.Ignore)]
        public List<Tool> Tools { get; set; }
    }

    //시스템 프롬프트 구조
    [Serializable]
    public class SystemInstruction
    {
        [JsonProperty("parts")]
        public List<Part> Parts { get; set; }
    }

    // 대화의 한 단위를 나타내는 Content 구조 (역할 + 내용)
    [System.Serializable]
    public class Content
    {
        [JsonProperty("role")]
        public string Role { get; set; } // "user" 또는 "model"

        [JsonProperty("parts")]
        public List<Part> Parts { get; set; }
    }

    //실제 내용(텍스트, 이미지 등)을 담는 구조
    [Serializable]
    public class Part
    {
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
        [JsonProperty("inlineData", NullValueHandling = NullValueHandling.Ignore)]
        public InlineData InlineData { get; set; }
    }

    // Part 내부에 포함될 이미지 데이터 구조
    [Serializable]
    public class InlineData
    {
        [JsonProperty("mime_type")]
        public string MimeType { get; set; } // 예: "image/png", "image/jpeg"

        [JsonProperty("data")]
        public string Data { get; set; } // Base64로 인코딩된 이미지 데이터
    }

    // 생성 옵션 상세 구조
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

    // 안전 설정 상세 구조
    [System.Serializable]
    public class SafetySetting
    {
        [JsonProperty("category")]
        public string Category { get; set; } // 예: "HARM_CATEGORY_HARASSMENT"

        [JsonProperty("threshold")]
        public string Threshold { get; set; } // 예: "BLOCK_NONE"
    }

    // 도구(함수 호출) 상세 구조 (고급 기능)
    [System.Serializable]
    public class Tool
    {
        [JsonProperty("function_declarations")]
        public List<object> FunctionDeclarations { get; set; } // 실제 구조는 더 복잡, 우선 object로
    }
}

