namespace TrustZoneAPI.Services
{
    public class ResponseResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public List<string>? Errors { get; set; }
        public int StatusCode { get; set; }

        public static implicit operator ResponseResult<T>(T value) => Success(value);

        // ✅ نجاح مع بيانات
        public static ResponseResult<T> Success(T data) => new ResponseResult<T>
        {
            IsSuccess = true,
            Data = data,
            StatusCode = 200
        };
        public static ResponseResult<T> Created(T data) => new ResponseResult<T>
        {
            IsSuccess = true,
            Data = data,
            StatusCode = 201
        };
        public static ResponseResult<T> Created() => new ResponseResult<T>
        {
            IsSuccess = true,
            StatusCode = 201
        };

        // ✅ نجاح بدون بيانات
        public static ResponseResult<T> Success() => new ResponseResult<T>
        {
            IsSuccess = true,
            StatusCode = 200
        };
        public static ResponseResult<T> NotFound() => new ResponseResult<T>
        {
            IsSuccess = false,
            StatusCode = 404
        };
        public static ResponseResult<T> NotFound(string message) => new ResponseResult<T>
        {
            IsSuccess = false,
            ErrorMessage = message,
            StatusCode = 404
        };


        // ❌ خطأ مع رسالة وخاصية StatusCode
        public static ResponseResult<T> Error(string message, int statusCode) => new ResponseResult<T>
        {
            IsSuccess = false,
            ErrorMessage = message,
            StatusCode = statusCode
        };

        // ❌ خطأ مع قائمة من الأخطاء
        public static ResponseResult<T> Error(List<string> errors, int statusCode = 400) => new ResponseResult<T>
        {
            IsSuccess = false,
            Errors = errors,
            StatusCode = statusCode
        };

        // ❌ خطأ من Exception
        public static ResponseResult<T> FromException(Exception ex, int statusCode = 500) => new ResponseResult<T>
        {
            IsSuccess = false,
            ErrorMessage = ex.Message,
            StatusCode = statusCode
        };

        // ❌ خطأ ناتج عن أخطاء التحقق
        public static ResponseResult<T> FromValidationErrors(IEnumerable<string> errors, int statusCode = 400) => new ResponseResult<T>
        {
            IsSuccess = false,
            Errors = errors.ToList(),
            StatusCode = statusCode
        };
    }

    public class ResponseResult : ResponseResult<object>
    {
        public static ResponseResult Success() => new ResponseResult { IsSuccess = true, StatusCode = 200 };
        public static ResponseResult Created() => new ResponseResult { IsSuccess = true, StatusCode = 201 };
        public static ResponseResult NoContent() => new ResponseResult { IsSuccess = true, StatusCode = 204 };
        public static ResponseResult NotFound() => new ResponseResult { IsSuccess = false, StatusCode = 404, ErrorMessage = "" };
        public static ResponseResult NotFound(string message) => new ResponseResult { ErrorMessage = message, StatusCode = 404 };
        public static ResponseResult Error(string message, int statusCode) => new ResponseResult { IsSuccess = false, ErrorMessage = message, StatusCode = statusCode };
        public static ResponseResult Error(List<string> errors, int statusCode = 400) => new ResponseResult { IsSuccess = false, Errors = errors, StatusCode = statusCode };
        public static ResponseResult FromException(Exception ex, int statusCode = 500) => new ResponseResult { IsSuccess = false, ErrorMessage = ex.Message, StatusCode = statusCode };
        public static ResponseResult FromException(string Message, int statusCode = 500) => new ResponseResult { IsSuccess = false, ErrorMessage = Message, StatusCode = statusCode };
    }
}
