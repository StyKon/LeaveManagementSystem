namespace LeaveManagementSystem.DATA.Common
{
    public class EntityResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }


        public static EntityResult<T> SuccessResult(T data, string message = "Operation successful.")
        {
            return new EntityResult<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }


        public static EntityResult<T> FailureResult(string message)
        {
            return new EntityResult<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }
}
