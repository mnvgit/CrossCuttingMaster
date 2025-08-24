CrossCuttingMaster is a modern .NET 8 web application template that demonstrates robust cross-cutting concern handling using MediatR, FluentValidation, and custom middleware. 
The project is designed to help you build scalable, maintainable, and testable APIs by centralizing common concerns such as validation, exception handling, performance monitoring, and audit logging.

•	.NET 8 Web API: Built on the latest .NET platform for maximum performance and long-term support.
•	MediatR Integration: Implements the mediator pattern for clean separation of business logic and infrastructure, supporting request/response, notifications, and pipeline behaviors.
•	FluentValidation: Provides a fluent interface for model validation, integrated seamlessly into the MediatR pipeline.
•	Custom Pipeline Behaviors:
•	Performance Monitoring: Logs warnings for slow requests, with configurable thresholds.
•	Audit Logging: Automatically logs all requests and responses, including errors, for traceability and compliance.
•	Global Exception Handling: Centralized error handling with customizable responses and support for ProblemDetails.
•	Extensible Architecture: Easily add or modify cross-cutting behaviors to suit your project’s needs.





🚀 Guide: Using CrossCuttingMaster.postman_collection.json
This repository includes a Postman Collection (CrossCuttingMaster.postman_collection.json) for testing the CrossCuttingMaster API.

📥 Import into Postman
Download and install Postman
 if you don’t have it.
Open Postman and click Import (top-left).
Select File and choose CrossCuttingMaster.postman_collection.json.
The collection CrossCuttingMaster will now appear in your sidebar.


📌 Available Requests

✅ Create Order – Happy Path (200 OK)
POST https://localhost:32770/Order
Body:
{
  "price": 20,
  "userId": 5
}
Expected: Successful order creation (200 OK).


❌ Create Order – Validation Error (400 Bad Request)
POST https://localhost:32770/Order
Body (missing userId):
{
  "price": 20
}
Expected: Validation failure (400 Bad Request).


💥 Create Order – Server Error (500 Internal Server Error)
POST https://localhost:32770/Order
Body (triggers error case Missing Idempotency-Key header):
{
  "price": 200,
  "userId": 3
}
Expected: Simulated server failure (500 Internal Server Error).
