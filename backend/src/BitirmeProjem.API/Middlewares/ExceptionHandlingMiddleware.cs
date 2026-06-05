using System.Net;
using System.Text.Json;

namespace BitirmeProjem.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var lang = context.Request.Headers["Accept-Language"].ToString().StartsWith("en") ? "en" : "tr";

        var (statusCode, message) = exception switch
        {
            InvalidOperationException { Message: "EMAIL_ALREADY_EXISTS" } => (HttpStatusCode.BadRequest, lang == "en"
                ? "This email address is already registered."
                : "Bu email adresi zaten kayıtlı."),
            InvalidOperationException { Message: "FINANCIAL_PROFILE_ALREADY_EXISTS" } => (HttpStatusCode.BadRequest, lang == "en"
                ? "Financial profile already exists."
                : "Finansal profil zaten mevcut."),
            KeyNotFoundException { Message: "FINANCIAL_PROFILE_NOT_FOUND" } => (HttpStatusCode.NotFound, lang == "en"
                ? "Financial profile not found."
                : "Finansal profil bulunamadı."),
            KeyNotFoundException { Message: "RISK_TEST_RESULT_NOT_FOUND" } => (HttpStatusCode.NotFound, lang == "en"
                ? "No risk test result found."
                : "Risk testi sonucu bulunamadı."),
            KeyNotFoundException { Message: "GOAL_NOT_FOUND" } => (HttpStatusCode.NotFound, lang == "en"
                ? "Goal not found."
                : "Hedef bulunamadı."),
            KeyNotFoundException { Message: "USER_NOT_FOUND" } => (HttpStatusCode.NotFound, lang == "en"
                ? "User not found."
                : "Kullanıcı bulunamadı."),
            InvalidOperationException { Message: "INVALID_ANSWER_COUNT" } => (HttpStatusCode.BadRequest, lang == "en"
                ? "You must answer exactly 10 questions."
                : "Tam olarak 10 soruyu cevaplamanız gerekiyor."),
            InvalidOperationException { Message: "INVALID_OPTIONS" } => (HttpStatusCode.BadRequest, lang == "en"
                ? "One or more selected options are invalid."
                : "Seçilen şıklardan bir veya birkaçı geçersiz."),
            UnauthorizedAccessException { Message: "INVALID_CURRENT_PASSWORD" } => (HttpStatusCode.BadRequest, lang == "en"
                ? "Current password is incorrect."
                : "Mevcut şifre hatalı."),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, lang == "en"
                ? "Invalid email or password."
                : "Email veya şifre hatalı."),
            _ => (HttpStatusCode.InternalServerError, lang == "en"
                ? "An unexpected error occurred."
                : "Beklenmeyen bir hata oluştu.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = JsonSerializer.Serialize(new { error = message });
        await context.Response.WriteAsync(response);
    }
}
