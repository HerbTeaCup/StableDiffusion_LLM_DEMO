using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GeminiLLM
{
    /// <summary>
    /// Gemini API 응답의 최상위 루트 객체입니다.
    /// </summary>
    public class GeminiResponse
    {
        [JsonProperty("candidates")]
        public List<Candidate> Candidates { get; set; }

        [JsonProperty("promptFeedback", NullValueHandling = NullValueHandling.Ignore)]
        public PromptFeedback PromptFeedback { get; set; }

        [JsonProperty("usageMetadata")]
        public UsageMetadata UsageMetadata { get; set; }
    }

    public class Candidate
    {
        [JsonProperty("content")]
        public Content Content { get; set; }

        [JsonProperty("finishReason")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FinishReason FinishReason { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("safetyRatings")]
        public List<SafetyRating> SafetyRatings { get; set; }

        [JsonProperty("tokenCount")]
        public int TokenCount { get; set; }
    }

    public class PromptFeedback
    {
        [JsonProperty("blockReason")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BlockReason BlockReason { get; set; }

        [JsonProperty("safetyRatings")]
        public List<SafetyRating> SafetyRatings { get; set; }
    }

    public class SafetyRating
    {
        [JsonProperty("category")]
        [JsonConverter(typeof(StringEnumConverter))]
        public HarmCategory Category { get; set; }

        [JsonProperty("probability")]
        [JsonConverter(typeof(StringEnumConverter))]
        public HarmProbability Probability { get; set; }
    }

    public class UsageMetadata
    {
        [JsonProperty("promptTokenCount")]
        public int PromptTokenCount { get; set; }

        [JsonProperty("candidatesTokenCount")]
        public int CandidatesTokenCount { get; set; }

        [JsonProperty("totalTokenCount")]
        public int TotalTokenCount { get; set; }
    }

    // --- ENUM 정의 ---

    public enum FinishReason
    {
        FINISH_REASON_UNSPECIFIED,
        STOP,
        MAX_TOKENS,
        SAFETY,
        RECITATION,
        OTHER
    }

    public enum BlockReason
    {
        BLOCK_REASON_UNSPECIFIED,
        SAFETY,
        OTHER
    }

    // API 문서의 HARM_CATEGORY_... 와 매칭됩니다.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HarmCategory
    {
        HARM_CATEGORY_UNSPECIFIED,
        HARM_CATEGORY_HARASSMENT,
        HARM_CATEGORY_HATE_SPEECH,
        HARM_CATEGORY_SEXUALLY_EXPLICIT,
        HARM_CATEGORY_DANGEROUS_CONTENT
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum HarmProbability
    {
        HARM_PROBABILITY_UNSPECIFIED,
        NEGLIGIBLE,
        LOW,
        MEDIUM,
        HIGH
    }
}