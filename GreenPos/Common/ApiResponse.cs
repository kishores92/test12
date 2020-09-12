using System;

namespace GreenPOS.Common
{
    public class ApiResponse<T>
    {
        public int Code { get; set; } = 0;
        public string Message { get; set; } = "No Records Found";
        public T Data { get; set; }
    }
}
