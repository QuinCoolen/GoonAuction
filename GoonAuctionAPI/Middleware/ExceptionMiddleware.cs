using GoonAuctionBLL.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleExceptionAsync(context, 400, ex.Message);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(context, 404, ex.Message);
        }
        catch (Exception)
        {
            await HandleExceptionAsync(context, 500, "An unexpected error occurred.");
        }
    }

    private Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(new { error = message });
    }
}
