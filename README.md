# PaymentGateway API

David Milne 23/03/2021

## Project Structure

 - API project contains controllers and general startup configuration
 - Common project contains domain and dto objects, mappers, some logging enums and constants.
 - Infrastructure project contains monitoring classes and repositories
 - Service project contains business logic

## How to use
Run project locally. Appsettings.json should have the value "BankHttpRepository": "MockedBankHttpRepository" to use the mocked bank repository.
**CreatePaymentRequest**
https://localhost:44342/api/CreatePayment
POST
Content-Type: application/json
DebugExpectedSuccess: true/false

    {
      "cardHolderName": "Mr Card Holder" Any string is valid
      "cardNumber": "4444444444444448" Must pass Luhn check, you can use this number to test.
      "expiryMonth": "12" - must be a numeric string 1-12
      "expiryYear": "2025" - must be a numeric 4 digit string, month and year must be in the future
      "amount": 99.00 - must be > 0
      "currencyCode": "GBP" - must be a 3 character string
      "cvv": "123" - must be a numeric string of 3-4 digits
    }
In order to test failure with the acquirer, you can supply the DebugExpectedSuccess header with the value false. It will default to true if not supplied.

**Note**. Card validation is carried out using the luhn check locally, any further validation needs to be done at the Accquirer level.

**RetrievePaymentDetailsRequest**
https://localhost:44342/api/RetrievePayment
POST
Content-Type: application/json
DebugExpectedSuccess: Success/Failure/TransactionIdNotRecognised

    {
      "transactionId": "string", Any string is valid
    }
In order to test failure with the acquirer, you can supply the DebugExpectedStatus header. Permitted values are Success , Failure, TransactionIdNotRecognised. It defaults to success if not set

## Logging
I have added some simple metrics and error logging using Prometheus. Obviously this is not wired up to go anywhere but can be viewed using the standard https://localhost:44342/metrics. 

## Major Improvements 
**Authentication**
No authentication is carried out, this would need to be taken care of to validate the origin of the request

**Encryption**
The card data arrives with only the protection of https. This is not acceptable for real data. I looked into a few ways that I might be able to implement this and I ran out of time. I did think of having a pre-auth request that could generate a new set of keys and return a session id and the public key. It could then be encrypted at the client side and the session id passed back in the CreatePaymentRequest so the application could decrypt with the private key. I believe this is feasible but it doesn't add for great usability, no PaymentGateway I have ever integrated into required this extra step!

**The use of the mocked repository**
I don't like the use of a hard-coded string to determine what repo to use however it does feel like a simple and pragmatic solution. There is probably a better way to do this! In addition it could be nice to switch programatically to use this based on request type i.e. the supply of debug headers.

**Currency Code Validation**
This would be better if we had an external service of Currency ISO codes. I assume this approach would work with a live acquirer as they would validate the code however it would be better to have more robust validation in a test instance for integration.
> Written with [StackEdit](https://stackedit.io/).

