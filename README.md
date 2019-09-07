# Payment Gateway

This payment gateway is designed to sit between a merchant and the acquiring bank. A merchant can place a payment request, and query the status of the payment. To help with scaling, upon receiving a payment request we will immediately respond with a unique identifier and pass on the request to be handled by backend processors, which will be responsible for calling the acquiring bank and updating the status of the payment.

This is mostly built against abstracts, so we could replace
* The InMemoryPaymentRepository with a Couchbase implementation or similar
* The InMemoryCommandQueue with a RabbitMQ implementation or similar
* The FakeBankServiceClient with a real bank

# Features
* Built using dotnetcore 2.2
* Containerized using Docker
* Swagger generated API page and C# client
* Telemetry timings and failure rates

# Running
```sh
$docker-compose up -d --build
```
Then navigate to: http://localhost:6125/swagger

# TODO
* Application metrics 
 AppMetrics
* Authentication 
 OpenID?
* Build script / CI 
 github actions?
* Performance testing 
 https://docs.microsoft.com/en-us/aspnet/core/test/load-tests?view=aspnetcore-2.2
 Look into Locust
 Maybe create a Docker file to spin up instances and spam it
 https://medium.com/@taylor.collins/performance-testing-a-net-core-web-api-endpoint-8c859f889fb6
* Encryption 
 More of an infrastructure kind of thing, TLS certificates/checks between different instance?
