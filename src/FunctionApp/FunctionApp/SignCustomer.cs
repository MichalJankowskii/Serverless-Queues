namespace FunctionApp
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using FluentValidation.Results;
    using FunctionApp.Models;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.WindowsAzure.Storage.Table;
    using SendGrid.Helpers.Mail;
    using Twilio.Rest.Api.V2010.Account;
    using Twilio.Types;
    using Validators;

    public static class SignCustomer
    {
        [FunctionName("SignCustomer")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")]
            Customer customer,
            [Table("customers", Connection = "registrationstorage_STORAGE")]
            CloudTable customersTable,
            [TwilioSms(
                AccountSidSetting = "twilioAccountSid",
                AuthTokenSetting = "twilioAuthToken",
                From = "!!!!!!!!!!!!!!!!!!!!!!!!ENTER_FROM_PHONE_NUMBER - PROVIDED BY TWILIO!!!!!!!!!!!!!!!!!!!!!!!!",
                Body = "New customer {Name} {Surname}!")]
            out CreateMessageOptions smsMessageOptions,
            [SendGrid(
                ApiKey = "sendGridApiKey",
                To = "{Email}",
                Subject = "Thank you!",
                Text = "Hi {Name}, Thank you for registering!!!!",
                From = "!!!!!!!!!!!!!!!!!!!!!!!!ENTER_FROM_EMAIL_ADDRESS!!!!!!!!!!!!!!!!!!!!!!!!"
            )]
            out SendGridMessage emailMessage)
        {
            smsMessageOptions = new CreateMessageOptions(new PhoneNumber("!!!!!!!!!!!!!!!!!!!!!!!!ENTER_YOUR_PHONE_NUMBER!!!!!!!!!!!!!!!!!!!!!!!!"));
            emailMessage = new SendGridMessage();

            var validator = new CustomerValidator();
            ValidationResult results = validator.Validate(customer);

            if (results.IsValid == false)
            {
                string validationMessage = "Validation failed: " +
                                           results.Errors.Select(error => error.ErrorMessage)
                                               .Aggregate((a, b) => a + "; " + b);
                var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.ReasonPhrase = validationMessage;
                return responseMessage;
            }

            customer.PartitionKey = "AzureTest";
            customer.RowKey = Guid.NewGuid().ToString();

            TableOperation insertOperation = TableOperation.Insert(customer);
            customersTable.ExecuteAsync(insertOperation).Wait();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
