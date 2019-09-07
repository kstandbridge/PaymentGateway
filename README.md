# Payment Gateway

This payment gateway is designed to sit between a merchant and the acquiring bank. A merchant can place a purchase request, and query the status of the purchase. To help with scaling, upon receiving a payment request we will immediately respond with a unique identifier and pass on the request to be handled by backend processors, which will be responsible for calling the acquiring bank.

This is mostly built against abstracts, so we could replace
* The InMemoryPaymentRepository with a Couchbase implementation or similar
* The InMemoryCommandQueue with a RabbitMQ implementation or similar
* The FakeBankServiceClient with a real bank

# Run down of features
* Built using dotnetcore 2.2
* Containerized using Docker
* Swagger generated API page and C# client
* Telemetry timings and failure rates

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