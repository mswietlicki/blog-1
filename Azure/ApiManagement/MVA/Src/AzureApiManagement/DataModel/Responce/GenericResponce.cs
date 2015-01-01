using System.Collections.Generic;

namespace Sychev.AzureApiManagement.Api.Models
{
    public class GenericResponce<T> : GenericResponceNoData
    {
        public List<T> Data { get; set; }

        public GenericResponce()
        {
            Data = new List<T>();
        }

        public GenericResponce(List<T> data)
        {
            Data = data;
        }

        public GenericResponce(T data)
        {
            Data = new List<T> { data };
        }
    }
}