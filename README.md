# Ewallet App MVC

## API Endpoints

Base URL: `/api/v1`

### Users

| Endpoint Path                            | HTTP Method | Description                         | Available |
| ---------------------------------------- | ----------- | ----------------------------------- | --------- |
| `/api/v1/users`                          | GET         | Get all users                       | ✅        |
| `/api/v1/users/{id}`                     | GET         | Get user+wallet detail by user ID   | ✅        |
| `/api/v1/users`                          | POST        | Create user and initialize wallet   | ✅        |
| `/api/v1/users/{id}`                     | PUT         | Update user profile                 |           |
| `/api/v1/users/{id}`                     | DELETE      | Delete user by ID                   |           |
| `/api/v1/topup/`                         | POST        | Top up wallet by wallet ID (body)   | ✅        |
| `/api/v1/topup/{topuprequestId}/confirm` | POST        | Confirm top up by top up request ID | ✅        |
| `/api/v1/transfers`                      | POST        | Transfer balance to another wallet  |           |
| `/api/v1/users/{userId}/transactions`    | GET         | Get user transaction history        | ✅        |

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server
- Visual Studio Code or Visual Studio
- REST Client extension or Swagger UI (optional)

### Clone Repository

```bash
git clone https://github.com/bando9/koda-b1-weekly6
cd Ewallet.Mvc
```

### Database Configuration

Configure the SQL Server connection string in `appsettings.json` or user secrets.

```json
{
  "ConnectionStrings": {
    "Ewalletdb": "Server=YOUR_SERVER;Database=EwalletDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
