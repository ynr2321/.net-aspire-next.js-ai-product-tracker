namespace AspireApp.ApiService.Application.Auth.Models;

public enum AuthResultStatus
{
    Success,
    Conflict,
    ValidationError,
    Unauthorized
}

public record AuthResult(AuthResultStatus Status, string? Message = null, IEnumerable<string>? Errors = null)
{
    public bool Succeeded => Status is AuthResultStatus.Success;

    public static AuthResult Success(string message) => new(AuthResultStatus.Success, message);
    public static AuthResult Conflict(string message) => new(AuthResultStatus.Conflict, message);
    public static AuthResult ValidationError(IEnumerable<string> errors) => new(AuthResultStatus.ValidationError, Errors: errors);
}

public record AuthResult<T>(AuthResultStatus Status, T? Data = default, string? Message = null, IEnumerable<string>? Errors = null)
{
    public bool Succeeded => Status is AuthResultStatus.Success;

    public static AuthResult<T> Success(T data) => new(AuthResultStatus.Success, Data: data);
    public static AuthResult<T> Unauthorized(string message) => new(AuthResultStatus.Unauthorized, Message: message);
}
