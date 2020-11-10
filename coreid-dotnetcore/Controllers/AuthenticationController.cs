using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Assently.Samples.DotnetCore.Models;
using Assently.Samples.DotnetCore.Models.CoreId;
using Assently.Samples.DotnetCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Assently.Samples.DotnetCore.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthorizationRequestTokenService _tokenService;
        private readonly IdentityTokenService _identityTokenService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationController(AuthorizationRequestTokenService tokenService, IdentityTokenService identityTokenService,
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _tokenService = tokenService;
            _identityTokenService = identityTokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/Home")
        {
            // The authToken has an expiration,
            // if the user stays on this page too long before logging in
            // token can become stale since it is generated on page-load
            var authToken = _tokenService.CreateToken();

            HttpContext.Session.SetString("authToken", authToken);

            return View("Login", new LoginViewModel(authToken, returnUrl));
        }

        /// <summary>
        /// The OidcReturn is a cross-origin request. It will not have access to the session/cookies.
        /// You can either handle this in a way that does not require session at this step.
        /// Or, like this example, use the data as parameters and let the user issue the request from the same site.
        ///
        /// The appropriate implementation of this entirely depends on your application.
        /// <param name="id">transactionId</param>
        /// <param name="status">RresultType</param>
        /// <param name="errorMessage">Optional errorMessage</param>
        /// <param name="identityToken">Identity token if successful identification</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult OidcReturn(string id, string status, string errorMessage, string identityToken)
        {
            return View("OidcReturn", new AuthenticationResult
                {
                    Type = status,
                    Success = status == ResultType.Authenticated,
                    Token = identityToken,
                    ErrorMessage = errorMessage,
                    TransactionId = id
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<LoginResult> Login(AuthenticationResult data)
        {
            if (data.Success && data.Type == ResultType.Authenticated && !string.IsNullOrWhiteSpace(data.Token))
            {
                var originalToken = HttpContext.Session.GetString("authToken");

                try
                {
                    // The identity token can ONLY be trusted once the signature has been validated.
                    var validatedIdentityToken = _identityTokenService.ValidateToken(data.Token, originalToken);

                    #region Integrate with your user management

                    // TODO: Implement this function
                    var userIdentifier = MapTokenToIdentifier(validatedIdentityToken);

                    var user = await _userManager.FindByNameAsync(userIdentifier);

                    if (user == null)
                    {
                        user = new IdentityUser {UserName = userIdentifier};
                        await _userManager.CreateAsync(user);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    #endregion

                    if (Url.IsLocalUrl(data.ReturnUrl))
                        return new LoginResult(data.ReturnUrl);

                    return new LoginResult(Url.Action("Index", "Home"));
                }
                catch (NotImplementedException e)
                {
                    return new LoginResult(false, Url.Action("Login"), e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return new LoginResult(false, Url.Action("Login"), "Unable to log in");
        }

        private string MapTokenToIdentifier(JwtSecurityToken identityToken)
        {
            // The identity token contains identification data in a common format
            // How the identification data should be mapped to a user depends on the application.
            //
            // The "sub"/"subject"-claim is not guaranteed to be unique.
            //
            // If your integration supports multiple countries, the nationalIds may conflict across countries.
            //
            // Certain identifications do not result in unique identifiers.
            throw new NotImplementedException("Implement appropriate mapping of token to a unique identifier in AuthenticationController#MapTokenToIdentifier()");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}