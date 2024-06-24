# Job Portal Application

This project is a Job Portal application built using ASP.NET Web API and MVC.
It allows users to manage jobs, companies, and job categories through a RESTful API 
and provides a web interface to interact with these resources.

## Features
 - Manage jobs: Create, Read, Update, and Delete job data.
 - Company Management: Manage company linked to jobs.
 - Category Management: Manage category linked to jobs.
 - Relational Data: Handle relationships between job, company, and category.

## Running the Project

1. **Set Target Framework:**
   - Open Project > JobPortal Properties > Change target framework to 4.7.1 and then back to 4.7.2.

2. **Ensure App_Data Folder:**
   - Make sure there is an `App_Data` folder in the project directory. If not, create one (Right-click solution > View in File Explorer).

3. **Update Database:**
   - Open Tools > Nuget Package Manager > Package Manager Console.
   - Run `Update-Database` command to apply migrations and create the database.

4. **Verify Database Creation:**
   - Check that the database is created using SQL Server Object Explorer (View > SQL Server Object Explorer > MSSQLLocalDb).

## Common Issues and Resolutions

- **Could not attach .mdf database:**
  - Ensure the `App_Data` folder exists in the project directory.

- **'Type' cannot be null error:**
  - Update Entity Framework to the latest version (e.g., 6.4.4) using NuGet Package Manager.

- **System Exception: Exception has been thrown by the target of an invocation:**
  - Clone the project repository to the local drive instead of cloud-based storage like OneDrive.

- **Could not find a part to the path ../bin/roslyn/csc.exe:**
  - Change the target framework to 4.7.1 and then back to 4.7.2 to resolve.

- **Project Failed to build. System.Web.Http does not have a reference to serialize...:**
  - Add a reference to System.Web.Extensions in Solution Explorer > References.

## Using the API

### List of API Endpoints

#### job managemnet ####

- **List Jobs:**
  ```bash
  curl https://localhost:44320/api/JobData/ListJobs

- **Find Job by ID:**
  ```bash
  curl https://localhost:44320/api/JobData/FindJob/{id}

- **List Jobs for Company:**
  ```bash
  curl https://localhost:44320/api/JobData/ListJobsForCompany/{id}

- **List Jobs for Category:**
  ```bash
  curl https://localhost:44320/api/JobData/ListJobsForCategory/{id}

- **Add a Job:**
  ```bash
  curl -H "Content-Type:application/json" -d @job.json https://localhost:44320/api/JobData/AddJob

- **Update a Job:**
  ```bash
  curl -H "Content-Type:application/json" -d @job.json https://localhost:44320/api/JobData/UpdateJob/{id}

- **Delete a Job:**
  ```bash
  curl -d "" https://localhost:44320/api/JobData/DeleteJob/{id}

#### company managemnet ####
- **List companies:**
  ```bash
  curl https://localhost:44320/api/CompanyData/ListCompanies

- **Find Company by ID:**
  ```bash
  curl https://localhost:44320/api/CompanyData/FindCompany/{id}

- **Add a Company:**
  ```bash
  curl -H "Content-Type:application/json" -d @job.json https://localhost:44320/api/CompanyData/AddCompany

- **Update a Company:**
  ```bash
  curl -H "Content-Type:application/json" -d @job.json https://localhost:44320/api/CompanyData/UpdateCompany/{id}

- **Delete a Company:**
  ```bash
  curl -d "" https://localhost:44320/api/CompanyData/DeleteCompany/{id}

#### company managemnet ####
- **List Category:**
  ```bash
  curl https://localhost:44320/api/CategoryData/ListCategories

- **Find Company by ID:**
  ```bash
  curl https://localhost:44320/api/CategoryData/FindCategory/{id}

- **Add a Company:**
  ```bash
  curl -H "Content-Type:application/json" -d @job.json https://localhost:44320/api/CategoryData/AddCategory

- **Update a Company:**
  ```bash
  curl -H "Content-Type:application/json" -d @job.json https://localhost:44320/api/CategoryData/UpdateCategory/{id}

- **Delete a Company:**
  ```bash
  curl -d "https://localhost:44320/api/CategoryData/DeleteCategory/{id}"

##Database Relationships
 - job: A recipe has many companies and many categories.
 = company: Each company belongs to a job.
 = category: Each category belongs to a job.
  
