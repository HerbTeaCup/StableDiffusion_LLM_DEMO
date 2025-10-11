using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ollama
{
    public static class Generate
    {
        public class RequestData
        {
            public string model;
            public string prompt;
            public bool stream;

            public RequestData(string model, string prompt, bool stream = false)
            {
                this.model = model;
                this.prompt = prompt;
                this.stream = stream;
            }
        }

        public class ResponseData
        {
            //필수
            public string model;
            public string response;
            public bool done;

            //확장
            //public List<int> context;
            //public long total_duration;
            //public long load_duration;
            //public int prompt_eval_count;
            //public long prompt_eval_duration;
            //public int eval_count;
            //public long eval_duration;
        }
    }
}
