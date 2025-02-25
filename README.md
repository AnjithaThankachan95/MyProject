# Customer Management System

## Overview
This is a .NET 6-based Customer Management System consisting of:
- **Customer Management API** (ASP.NET Core Web API)
- **Customer Management Windows Forms Application** (WinForms) 

The **Windows Forms application** consumes the **API** for performing CRUD operations on customers.

---

## Features
✅ Add a new customer  
✅ View all customers  
✅ Update customer details  
✅ Delete a customer  
✅ API integration with WinForms application  
✅ Uses AutoMapper for DTO mapping  

---

## Technologies Used
- **Backend (API)**: .NET 6, ASP.NET Core Web API, Entity Framework Core, AutoMapper
- **Frontend (WinForms App)**: .NET 6, Windows Forms, HttpClient
- **Database**: In-memory storage (can be extended to SQL Server)

---

## Installation and Setup
### 1️⃣ Clone the Repository
```sh
 git clone https://github.com/your-repo-url.git
```

### 2️⃣ Open the API Project
- Navigate to `CustomerManagementSolution/CustomerManagementAPI`
- Open in **Visual Studio**
- Restore NuGet packages: `dotnet restore`
- Run the API: `dotnet run`

The API will be available at: `https://localhost:7194/api/Customer`

### 3️⃣ Open the Windows Forms App
- Navigate to `CustomerManagementSolution/CustomerWinFormsApp`
- Open in **Visual Studio**
- Build the project
- Run the application

---

## Configuration
Ensure `appsettings.json` exists in both API and WinForms application.

**Example `appsettings.json` for API:**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Example `appsettings.json` for Windows Forms App:**
```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7194/api/Customer"
  }
}
```

---

## API Endpoints
| Method | Endpoint | Description |
|--------|---------|-------------|
| **GET** | `/api/Customer` | Get all customers |
| **GET** | `/api/Customer/{id}` | Get customer by ID |
| **POST** | `/api/Customer` | Add a new customer |
| **PUT** | `/api/Customer/{id}` | Update customer details |
| **DELETE** | `/api/Customer/{id}` | Delete a customer |

---

## Troubleshooting
### 1️⃣ API Not Running?
- Make sure the API is running before launching the Windows Forms app.
- Run the API from the terminal using `dotnet run` inside the API project directory.

### 2️⃣ Configuration File Not Found Error?
- Ensure `appsettings.json` exists in the WinForms project's **output directory** (`bin/Debug/net6.0-windows`).
- Set **Copy to Output Directory** property of `appsettings.json` to `Copy if newer`.

### 3️⃣ Getting a 500 Error?
- Check the API logs in the console.
- Ensure API and WinForms app are using the same **BaseUrl**.

---

## License
MIT License  

---

## Contact
For any issues, feel free to reach out or open an issue in the repository!
