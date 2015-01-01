﻿using Newtonsoft.Json;

namespace Sychev.DocumentDB.DataModel
{
    public class GenericResponceNoData
    {
        public GenericResponceNoData(bool isSuccess = true)
        {
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}