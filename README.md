# Task
Refactor existing solution to production ready state. Please use queues for communication between functions.

# Solution deployment
## Clone / download git repository
`https://github.com/MichalJankowskii/Serverless-Queues.git`
## Generating API Keys
### SendGrid
Please follow manual that is available under the following link: [Sending emails from Azure Functions – SendGrid](https://www.jankowskimichal.pl/en/2018/04/sending-emails-from-azure-functions-sendgrid/)

### Twilio
Please follow manual that is available under the following link: [Sending SMSes from Azure Functions – Twilio](https://www.jankowskimichal.pl/en/2018/04/sending-smses-from-azure-functions-twilio/)


## Provisioning
1. Go to folder `src\Provisioning`
2. Edit `azuredeploy.parameters.json` and update all parameters. Remember that *appName* must be unique for whole Azure cloud
3. Open *PowerShell*
4. Login to Azure:
`Login-AzureRmAccount`
5. Create resource group:
`New-AzureRmResourceGroup -Name AzureLodz -Location "West Europe"`
6. Provision environment:
`New-AzureRmResourceGroupDeployment -Name AzureLodzDeployment -ResourceGroupName AzureLodz -TemplateFile azuredeploy.json -TemplateParameterFile azuredeploy.parameters.json`

## Deployment
1. Open solution and update the following files:
 - `local.settings.json` (only for local debugging)
 - `SignCustomer.cs`
2. Publish it to already created *Function App*. Please note that table will be automatically created before the first usage.


## Check if everything is working correctly.
You can use the following json in body:
```json
{
    "name": "Name",
    "surname": "Surname",
    "country": "UK",
    "email":"noreply@jankowskimichal.pl",
    "birthyear" : 1980
}
```
# You can start your task
Could you please refactor provided solution, that is a proof of concept solution, to shape that would be ready for deployment to production environment.

What would you change in this code?

You can work in groups!!!
