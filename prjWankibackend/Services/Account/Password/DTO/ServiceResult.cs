namespace prjWankibackend.Services.Account.Password.DTO
{

    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ServiceResult()
        {
            Success = false;
            Message = string.Empty;
            Data = null;
        }

        public ServiceResult(bool success, string message = "", object data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ServiceResult Ok(string message = "", object data = null)
        {
            return new ServiceResult(true, message, data);
        }

        public static ServiceResult Fail(string message = "", object data = null)
        {
            return new ServiceResult(false, message, data);
        }
    }

}
