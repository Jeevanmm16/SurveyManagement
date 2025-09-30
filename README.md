# Survey Management System

A Survey Management System built with **ASP.NET Core Web API** and **Entity Framework Core**.  
This project demonstrates clean architecture principles, authentication & authorization, and logging.

✨ **Features**
- ✅ User Authentication with JWT
- ✅ Password Hashing with BCrypt
- ✅ Role-based Authorization (Admin & User)
- ✅ Surveys CRUD
- ✅ Questions & Options CRUD
- ✅ Multiple Question Types (Checkbox, Radio, Rating, Text)
- ✅ Users can submit survey responses
- ✅ API versioning support
- ✅ Global Exception Handling
- ✅ Serilog Logging
- ✅ Custom Logging
- ✅ Swagger/OpenAPI documentation

🛠️ **Tech Stack**
- ASP.NET Core 7/8 (Web API)
- Entity Framework Core 7/8 (Database ORM)
- SQL Server (Database)
- ASP.NET Core Identity (User & Role Management)
- JWT Bearer Authentication
- BCrypt.Net (Password Hashing)
- AutoMapper (DTO ↔ Entity mapping)
- Serilog (Logging)
- Custom Logging
- Swagger / Swashbuckle (API Documentation)

📦 **Dependencies**
This project uses the following NuGet packages:
- `AutoMapper` + `AutoMapper.Extensions.Microsoft.DependencyInjection` → DTO to Entity mapping
- `BCrypt.Net-Next` → Password hashing for user authentication
- `Microsoft.AspNetCore.Authentication.JwtBearer` → JWT-based authentication
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` → Identity with EF Core (users, roles, claims)
- `Microsoft.EntityFrameworkCore`, `Design`, `SqlServer`, `Tools` → EF Core with SQL Server support and migrations
- `Microsoft.Extensions.Configuration.*` → Strongly-typed config using appsettings.json
- `Microsoft.AspNetCore.JsonPatch` → Support for PATCH endpoints
- `Microsoft.IdentityModel.Tokens` → Token signing and validation

🚀 **Getting Started**

📌 **Prerequisites**
Make sure you have installed:
- .NET 7/8 SDK
- SQL Server
- Git

📥 **Clone the Repository**
```bash
git clone https://github.com/<your-username>/SurveyManagement.git
cd SurveyManagement
