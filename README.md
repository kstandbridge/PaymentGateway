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
$ docker-compose up -d --build
```
Then navigate to: http://localhost:6125/swagger

# Backlog
* Setup Application metrics using AppMetrics or similar
* Add Authentication possibly using OpenID with IdentityService
* Setup a Build script for CI
* Look into Performance testing wth Locust
