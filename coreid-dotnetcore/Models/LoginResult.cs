namespace Assently.Samples.DotnetCore.Models
{
    public class LoginResult
    {
        public LoginResult(string redirectUrl)
        {
            Success = true;
            RedirectUrl = redirectUrl;
        }

        public LoginResult(bool success, string redirectUrl, string errorMessage)
        {
            Success = success;
            RedirectUrl = redirectUrl;
            ErrorMessage = errorMessage;
        }
        
        public bool Success { get; }
        public string ErrorMessage { get; }
        public string RedirectUrl { get; }
    }
}