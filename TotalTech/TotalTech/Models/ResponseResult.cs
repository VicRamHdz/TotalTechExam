using System;
namespace TotalTech.Models
{
    public class ResponseResult<T>
    {
        public int StatusCode { get; set; }

        public string Status { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public bool IsSuccess
        {
            get
            {
                if (string.IsNullOrEmpty(Status))
                {
                    return false;
                }
                return Status.ToLower().Equals("success");
            }
        }
    }
}
