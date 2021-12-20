using Assently.Client;
using Assently.Client.Models;
using Assently.Client.Models.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Assently.ClientAPI.TestUI.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public string? Name { get; set; }
        [BindProperty]
        public string? Email { get; set; }
        [BindProperty]
        public string? PhoneNumber { get; set; }
        [BindProperty]
        public string? PersonalNumber { get; set; }
        [BindProperty]
        public string? Description { get; set; }
        public string? Message { get; set; }


        private readonly IConfiguration _configuration;
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            Message = string.Empty;
        }

        public IActionResult OnPost()
        {
            var apiResponse = CreateCaseFromTemplate();
            if (string.IsNullOrWhiteSpace(apiResponse.ErrorMessage) && !string.IsNullOrWhiteSpace(apiResponse.URL))
            {
                return Redirect(apiResponse.URL);
            }
            else
            {
                Message = apiResponse.ErrorMessage;
                return Page();
            }

        }

        private CreateAndSendCaseResponse CreateCaseFromTemplate()
        {
            CreateAndSendCaseResponse createAndSendCaseResponse = new CreateAndSendCaseResponse();


            if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Email))
            {
                try
                {
                    var client = new AssentlyClient(_configuration.GetConnectionString("WebURI"), _configuration.GetConnectionString("APIKey"), _configuration.GetConnectionString("APISecret"));

                    var newCaseID = Guid.NewGuid();
                    client.CreateCaseFromTemplate(Guid.Parse(_configuration["TemplateId"]), newCaseID);

                    var @case = client.GetCase(newCaseID);

                    @case.Name = $"{Name}/{newCaseID}";
                    @case.Parties[0].Name = Name;
                    @case.Parties[0].EmailAddress = Email;
                    @case.Parties[0].MobilePhone = PhoneNumber;
                    @case.Parties[0].SocialSecurityNumber = PersonalNumber;
                    @case.Documents[0].FormFields["ContractDescription"] = Description;


                    client.UpdateCase(@case);
                    client.SendCase(newCaseID);

                    createAndSendCaseResponse.URL = @case.Parties[0].PartyUrl;
                }
                catch (Exception ex)
                {
                    createAndSendCaseResponse.ErrorMessage = ex.Message;
                }
            }
            else
            {
                createAndSendCaseResponse.ErrorMessage = "Please enter your Name and Email, to create contract";

            }
            return createAndSendCaseResponse;

        }
    }

    class CreateAndSendCaseResponse {
        public string? ErrorMessage { get; set; }

        [DataType(DataType.Url)]
        public string? URL { get; set; }

    }
}