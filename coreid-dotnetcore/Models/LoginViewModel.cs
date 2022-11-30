using Assently.Samples.DotnetCore.Models.CoreId;

namespace Assently.Samples.DotnetCore.Models
{
    public class LoginViewModel
    {
        public LoginViewModel(string token, string returnUrl)
        {
            Token = token;
            ReturnUrl = returnUrl;
        }

        public string ReturnUrl { get; }
        public string Mode => "auth";
        public string DefaultProvider => Provider.SeBankId;
        public string DefaultLocation => Location.Sweden;
        public string Language => Assently.Samples.DotnetCore.Models.CoreId.Language.Swedish;
        /// <summary>
        /// Enable more providers in client by adding them to this collection
        /// They must be in sync with the 'aud' claims in AuthTokenService.cs
        /// </summary>
        public string[] AllowedIdProviders => new [] { Provider.SeBankId, Provider.DkMitId, Provider.NoBankIdOidc, Provider.FiTupas };
        
        public string Token { get; }
    }
}