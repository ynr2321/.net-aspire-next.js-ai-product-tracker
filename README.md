WIP distributed app built with **.NET Aspire**:

currently hosted on azure at:
https://frontend.delightfulsmoke-1ef0581f.uksouth.azurecontainerapps.io/

(You can use a fake email to register and get past the login page as long as it is in format fake@email.com)

- ASP.NET Core Web API (.NET 10)
- Next.js frontend
- Docker orchestration

---

## Getting Started :)

### Prerequisites

- .NET 10 SDK  
- Node.js (LTS)  
- Docker  


## ▶ Running the Application Locally

1. Build and restore the solution.

2. Start docker daemon or docker desktop and Run the Aspire App Host project:
   - `dotnet run` or select apphost project in visual studio as startup

4. Open the Aspire Dashboard  
   - Check the console output for the dashboard URL.  
   - Ensure the **API service is NOT running** at this stage.

5. Optional - Run manual database migrations (skip this unless you've off AutoRunPendingEFCoreMigrations in appsettings):
   - In the Aspire Dashboard, select the **Postgres container resource**.
   - Copy the connection string.
   - Paste it into the `DesignTimeDbContextFactory` connection configuration.
   - `dotnet ef database update --project AspireApp.ApiService`


6. Start the API service from the Aspire Dashboard.



---
