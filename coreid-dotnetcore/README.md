# CoreID sample usage
_For asp.net core_

Authenticate using CoreID in your application.

This sample is configured for test-environment, and for demonstration purposes only.

Read the guide at https://docs.assently.com/

### How to run

You will need to register and receive credentials from Assently Support, and have eID test-credentials to perform test-identification.

Start by updating config values in appsettings.Development.json, with your credentials.

    "host": "", // Application hostname
    "customerName": "", // Issuer/customer name, your account Id
    "authSecret": "", // Provided at registration
    "identitySecret": "", // Provided at registration

Application is configured for Swedish BankID by default. To enable **different eID-providers** make changes to `LoginViewModel.cs`, and the `aud` claims in `AuthTokenService.cs`.

_For demonstration purposes only. Do not use as a template for your own app._
