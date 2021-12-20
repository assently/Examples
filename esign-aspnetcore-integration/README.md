# E-Sign Integration sample, for demonstration purposes only
_For ASP.NET Core Web Application_

This sample is configured for test-environment, and for demonstration purposes only.

This application shows how you can use Assently.Client 6.0.1 nuget (Assently API services) to create a case from a template 

The appropriate approach will entirely depend on your application.

Read the guide at https://test.assently.com/api

### How to run

You will need to register and receive credentials from https://test.assently.com/, verify your email address and have e-sign test-credentials to perform test E-Sign Integration.

Start by creating a document template with one party in  https://test.assently.com/ with the pdf named Test-PDF in the project and activate the template setting to Enable PDF form fields.
Take the Template id from the URL which is https://test.assently.com/a/templates/edit/"TemplateId", you created and update the TemplateId in appsettings.json
   
    "TemplateId": "", // targeted template id to create a case
    

    Update ConnectionStrings values in appsettings.json, with your credentials. You can find those in https://test.assently.com/a/account/api by clicking Enable API Access , after you register

    "WebURI": "https://test.assently.com", // API hostname
    "APIKey": "", // user API key
    "APISecret": "" // user API Secret


Note that this demonstration application is build in .net 6.0 version which will require visual studio 2022 to run this  application

_For demonstration purposes only. Do not use as a template for your own app._