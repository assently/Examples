using System;
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
        private readonly AuthTokenService _tokenService;
        private readonly IdentityTokenService _identityTokenService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationController(AuthTokenService tokenService, IdentityTokenService identityTokenService,
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

        [HttpPost]
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

                    // You can use Subject as identifier
                    if (string.IsNullOrWhiteSpace(validatedIdentityToken.Subject))
                        throw new Exception("No subject present to use in lookup");

                    var user = await _userManager.FindByNameAsync(validatedIdentityToken.Subject);

                    if (user == null)
                    {
                        user = new IdentityUser {UserName = validatedIdentityToken.Subject};
                        await _userManager.CreateAsync(user);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    #endregion

                    if (Url.IsLocalUrl(data.ReturnUrl))
                        return new LoginResult(data.ReturnUrl);

                    return new LoginResult(Url.Action("Index", "Home"));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return new LoginResult(false, Url.Action("Login"), "Unable to log in");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}