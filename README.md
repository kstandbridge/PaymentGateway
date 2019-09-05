# Payment Gateway

CQRS > Event Driven Workflow
Gateway cannot be the bottleneck

# You order has been recieved
# You order has been processed
# You order has been paid

API to Create payment > returns Id
* FluentValidations
* StoringCard information
* Forwarding payment request to ConcurrentQueue for CreatePaymentProcessor
CreatePaymentProcessor
* Call the acquiring bank to update the process


# Tech to use


# TODO
Maybe check Paypal developer API to see their flow, to better understand flow of a payment gateway

Scaling with
 - https://github.com/HangfireIO/Hangfire

* Application logging
 - Serilog
* Application metrics 
 - AppMetrics
* Containerization
 - Use Docker?
* Authentication 
 - OpenID?
* API client 
 - Use Swagger
* Build script / CI 
 - github actions?
* Performance testing 
 - https://docs.microsoft.com/en-us/aspnet/core/test/load-tests?view=aspnetcore-2.2
 - Look into Locust
 - Maybe create a Docker file to spin up instances and spam it
 - https://medium.com/@taylor.collins/performance-testing-a-net-core-web-api-endpoint-8c859f889fb6
* Encryption 
 - More of an infrastructure kind of thing, TLS certificates/checks between different instance?
* Data storage 
 - Repository pattern
* Anything else you feel may benefit your solution from a technical perspective.