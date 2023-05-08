# Currency Exchange 

To use this application you should have installet the following list of softwares:

* Visual studio 2022 
* .NET 7
* Postman
* SQL Server 2019

## Instructions
After you install the requested software above, you will need to create an account at [Api Layer](https://apilayer.com/), then create a key for their service [Exchange Rates Data API](https://apilayer.com/marketplace/exchangerates_data-api?utm_source=apilayermarketplace&utm_medium=featured)
When you get the key add it as a value of the key ExchangeApiConfig.ApiKey in the [appsettings.Development.json](https://github.com/juanarayafonsec/bank-currency-exchange-domain/blob/main/src/Bank.Currency.Exchange.Api/appsettings.Development.json) file inside Bank.Currency.Exchange.Api project

When you are ready with the key, just execute the project Bank.Currency.Exchange.Api and create a user using the endpoint https://localhost:5001/account/api/v1/register, then you can test it using the login endpoint https://localhost:5001/account/api/v1/login

To hit the exchange endpoint https://localhost:5001/exchange/api/v1/, you need to send the JWT token generated in the login request. The JWT expires 8 minutes after its creation 
The exchange endpoint use Baerer token authentication

You can use [this](https://github.com/juanarayafonsec/bank-currency-exchange-domain/blob/main/Postman/postman_collection.json) postman collection to test the project

Enjoy it!
