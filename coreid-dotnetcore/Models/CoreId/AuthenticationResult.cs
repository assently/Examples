namespace Assently.Samples.DotnetCore.Models.CoreId
{
    public static class ResultType
    {
        /// <summary>
        /// A successful transaction
        /// </summary>
        public const string Authenticated = "authenticated";

        /// <summary>
        /// An unsuccessful transaction
        /// </summary>
        public const string Failed = "failed";

        /// <summary>
        /// User closed or cancelled CoreID client,
        /// can happen after authentication is successful
        /// </summary>
        public const string Cancelled = "cancelled";

        /// <summary>
        /// An error occurred, which usually indicates
        /// a wrong configuration
        /// </summary>
        public const string Error = "error";
    }

    public class AuthenticationResult
    {
        /// <summary>
        /// Always true if authentication succeeded
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Transaction type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Identity token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Provider that user authenticated with
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// TransactionId is present for most of the providers, use this
        /// as a reference if you need to contact Assently Support
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Information about the possible error
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Used by application
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}