CrossCuttingMaster is a .NET project that demonstrates how to implement cross-cutting concerns using MediatR pipeline behaviors.
It ensures consistent handling of logging, validation, exception management, performance tracking, idempotency, audit logging, and transaction management across all requests.
</br></br>

📌 What Each Behavior Does

UnhandledExceptionBehavior → Catches unexpected exceptions and ensures consistent error responses.

LoggingPipelineBehavior → Logs request and response details for observability.

ValidationBehavior → Validates incoming requests using FluentValidation before reaching handlers.

PerformanceBehavior → Tracks execution time of each request for performance monitoring.

IdempotentCachingBehavior → Ensures duplicate requests (same Idempotency-Key) are not processed multiple times.

AuditLogBehavior → Records audit trail for sensitive operations.

TransactionBehavior → Wraps request handling in a database transaction to ensure atomicity.

</br></br>
🚀 Guide: Using CrossCuttingMaster.postman_collection.json
This repository includes a Postman Collection (CrossCuttingMaster.postman_collection.json) for testing the CrossCuttingMaster API.

📥 Import into Postman
Download and install Postman
 if you don’t have it.
Open Postman and click Import (top-left).
Select File and choose CrossCuttingMaster.postman_collection.json.
The collection CrossCuttingMaster will now appear in your sidebar.  
</br></br>

📌 Available Requests

✅ Create Order – Happy Path (200 OK)

POST https://localhost:32770/Order

Body:

{
  "price": 20,
  "userId": 5
}

Expected: Successful order creation (200 OK).    

</br></br>

❌ Create Order – Validation Error (400 Bad Request)

POST https://localhost:32770/Order

Body (missing userId):

{
  "price": 20
}

Expected: Validation failure (400 Bad Request).    

</br></br>

❌ Create Order – Missing Idempotency-Key header (400 Bad Request)

POST https://localhost:32770/Order

Body:

{
  "price": 200,
  "userId": 3
}

Expected: Failure due to missing Idempotency-Key header (400 Bad Request).    

</br></br>

💥 Create Order – Server Error (500 Internal Server Error)

POST https://localhost:32770/Order

Body (special userId: 100 cause server error):

{
  "price": 200,
  "userId": 100
}

Expected: Simulated server failure (500 Internal Server Error).
