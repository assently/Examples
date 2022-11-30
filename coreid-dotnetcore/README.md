# CoreID sample, for demonstration purposes only
_For asp.net core_

This sample is configured for test-environment, and for demonstration purposes only.

This application only shows how you can issue authentication request and receive an identity token.
How authentication, sessions or users are handled in this sample application is only for demonstration purposes.

The appropriate approach will entirely depend on your application.

Read the guide at https://docs.assently.com/

### How to run

You will need to register and receive credentials from Assently Support, and have eID test-credentials to perform test-identification.

Start by updating config values in appsettings.Development.json, with your credentials.

    "host": "", // Application hostname
    "customerName": "", // Issuer/customer name, your account Id
    "authSecret": "", // Provided at registration
    "identitySecret": "", // Provided at registration

Application is configured for Swedish BankID by default. 
To enable **different eID-providers** make changes to `LoginViewModel.cs`, and the `aud` claims in `AuthorizationRequestTokenService.cs`.

Implement the mapping function in AuthenticationController

    private string MapTokenToIdentifier(JwtSecurityToken identityToken)

_For demonstration purposes only. Do not use as a template for your own app._
