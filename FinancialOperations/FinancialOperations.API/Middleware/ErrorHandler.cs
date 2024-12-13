using System.Text.Json;

namespace FinancialOperations.API.Middleware
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandler> _logger;

        public ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> log)
        {
            _logger = log;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                _logger.LogError(error, error.Message);
                var response = context.Response;
                response.ContentType = "application/json";

                await response.WriteAsync(new ErrorResponse()
                {
                    StatusCode = response.StatusCode,
                    Message = "Ocorreu um erro inesperado."
                }.ToString());
            }
        }

        public class ErrorResponse
        {
            public static bool Success => false;
            public int StatusCode { get; set; }

            public string Message { get; set; } = string.Empty;
            public int Reason { get; set; }

            public override string ToString() =>
                JsonSerializer.Serialize(this);
        }
    }
}
