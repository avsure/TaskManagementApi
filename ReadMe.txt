Task Management API

A Task Management Web API built with .NET 9, demonstrating enterprise-grade patterns like authentication, authorization, DTO mapping, validation, exception handling, logging, caching, API versioning, and file uploads.
This project was built as a learning/demo project to strengthen skills in .NET Core Web API development.


Features:

✅ JWT Authentication & Role-based Authorization (Admin, User, Manager)
✅ Task CRUD Operations (Create, Update, Delete, Assign, Change Status)
✅ User Management (register, login)
✅ File Uploads (attachments for tasks)
✅ Pagination, Filtering & Sorting (for tasks list)
✅ Validation & Error Handling (data annotations, global exception middleware)
✅ API Versioning (v1, v2 routes)
✅ Serilog Logging (Console + File with Correlation IDs)
✅ Caching (Response caching with headers)
✅ Swagger / OpenAPI documentation


Tech Stack:

•	.NET 9 (ASP.NET Core Web API)
•	Entity Framework Core (Code-First + SQL Server)
•	JWT Authentication
•	AutoMapper for DTOs
•	Serilog for structured logging
•	Swagger / OpenAPI

Database Schema:

Users
•	UserId (PK)
•	Username      
•	Email         
•	HashedPassword
•	Role    (Admin/User/Manager) 
•	CreatedAt     
•	UpdatedAt     

Tasks
•	TaskId (PK)     
•	Title           
•	Description     
•	CreatedByUserId (FK Users)
•	AssignedToUserId (nullable FK Users)
•	Status (Low, Medium, High)
•	Priority        
•	CreatedAt       
•	DueDate         
•	UpdatedAt       

Attachments
•	AttachmentId (PK)
•	TaskId (FK Tasks)
•	FileName        
•	FileType        
•	FileSize        
•	FilePath        
•	UploadedByUserId
•	UploadedAt      

Getting Started:

1. Clone Repo:

git clone https://github.com/yourusername/TaskManagementApi.git
cd TaskManagementApi

2. Configure Database:

Update appsettings.json with your SQL Server connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

3.	Run the Project
dotnet run

API will run at:
•	https://localhost:7177/swagger(at your configured port)
•	http://localhost:5154/swagger


API Examples (few curl or Postman samples):

1. POST: 
https://localhost:7109/api/Auth/login
Request Body (JSON):
{
  "username": "",
  "password": ""
}

2. GET: 
https://localhost:7109/api/v1/Tasks?pageNumber=1&pageSize=10
Headers:
Authorization : Bearer <JWT Token from login>

3. POST:
https://localhost:7109/api/v1/Tasks
Request Body (JSON):
{
  "title": "New Task",
  "description": "Task details",
  "assignedToUserId": 2,
  "dueDate": "2025-12-31T23:59:59",
  "status": "Open",
  "priority": "Medium"
}

Logging:

•	Logs are written to console and Logs/log-yyyyMMdd.txt file
•	Each request has a Correlation ID for tracking

What I Learned:

This project helped me practice:
•	Structuring a real-world Web API with best practices
•	Implementing JWT authentication & role-based authorization
•	Handling validation, logging, exception middleware
•	Using DTOs and AutoMapper to separate concerns
•	Implementing API versioning and Swagger documentation
•	Adding file uploads and caching

Next Steps (Future Improvements):

•	Dockerize the API
•	Add Redis distributed caching
•	Implement unit/integration tests
•	Add CI/CD pipeline with GitHub Actions

How to Use This Repo:

Feel free to fork & use this as a starter template for building your own APIs.
If you like the project, ⭐ it on GitHub and connect with me on LinkedIn 😊

