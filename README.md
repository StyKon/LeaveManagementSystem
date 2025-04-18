# Leave Management System API

An ASP.NET Core Web API project built using Clean Architecture principles to manage employee leave requests. This system includes filtering, pagination, business rule validations, and reporting features, while leveraging generic infrastructure for reusability and scalability.

---

## Features

- Create, update, delete, and view leave requests
- Filtering, sorting, and pagination via query parameters
- Leave rules:
  - Disallow overlapping leave periods
  - Limit annual leave to 20 days per year
  - Require a reason for sick leave
- Leave reporting by employee, year, date range, and department
- Approval endpoint for pending requests
- Generic repository and result wrappers
- Docker support for containerized deployment

---

## Architecture Overview

```
LeaveManagementSystem/
├── Domain/                # Business entities, enums, interfaces
├── Data/                  # EF Core DbContext, migrations, repositories, services, DTOs 
├── WebApi/                # Controllers, Swagger, startup config 
├── Dockerfile             # Defines the application container build
├── docker-compose.yml     # Defines services and configurations for multi-container setup
├── README.md
├── LeaveManagementSystemAPI.postman_collection.json
```

- **Domain**: Contains core business logic including entities (e.g. `LeaveRequest`, `Employee`) and enums (e.g. `LeaveType`, `LeaveStatus`).
- **Data**: Implements repositories, service logic, mappings, DTOs, and data access through Entity Framework Core.
- **WebApi**: Exposes RESTful endpoints, handles HTTP input/output, and configures services via dependency injection.

---

## Design Patterns Used

### Repository Pattern
Abstracts data access logic to promote separation of concerns and testability. Implemented via:
- `IBaseRepository<TEntity>`
- `BaseRepository<TEntity>`
- `ILeaveRequestRepository`, `LeaveRequestRepository`

This helps to centralize and reuse data access logic while keeping the service layer clean.

### Service Layer Pattern
Encapsulates business logic in a dedicated layer (`LeaveRequestService`) which handles:
- Validation (e.g., overlapping leave, sick reason)
- Approval rules
- Filtering and reporting logic

### DTO Pattern
Used to decouple internal models from API input/output contracts. DTOs live in the `Data/Dto` folder and are mapped using AutoMapper.

### Result Wrapper Pattern
Handled by `EntityResult<T>` to standardize service method responses. It simplifies handling success/error flows.

### Pagination Wrapper
Via `PagedResult<T>`, providing a standard format for any paginated API responses.

---

## Generic Infrastructure

### BaseRepository<TEntity>
Location: `Data/Repositories/BaseRepository.cs`

A reusable generic repository that defines the following methods:

- `Task<TEntity> GetByIdAsync(object id)` – Fetch by primary key
- `Task<IEnumerable<TEntity>> GetAllAsync()` – Fetch all records
- `IQueryable<TEntity> GetZ(Expression<Func<TEntity, bool>> filter)` – Query with a predicate expression
- `Task<int> InsertAsync(TEntity entity)` – Insert and save
- `Task<int> UpdateAsync(TEntity entity)` – Attach and update
- `Task<int> DeleteAsync(TEntity entity)` – Attach and remove

### EntityResult<T>
Location: `Data/Common/EntityResult.cs`

A generic result wrapper for returning outcomes from services. Includes:

- `Success` (bool) – Whether the operation succeeded
- `Data` (T) – Optional data object
- `Message` (string) – Success or error message

Helps simplify controller logic and reduce exception-based flow.

### PagedResult<T>
Location: `Data/Common/PagedResult.cs`

A generic response wrapper for paginated endpoints:

- `Items` – Current page of data
- `TotalItems` – Total matching records
- `Page` – Current page number
- `PageSize` – Records per page

---

## Getting Started

### Prerequisites
- .NET 9 SDK
- Docker

### Run Locally
```bash
git clone https://github.com/StyKon/LeaveManagementSystem.git
cd LeaveManagementSystem

dotnet ef database update --project LeaveManagementSystem.DATA --startup-project LeaveManagementSystem.WebApi

dotnet run --project LeaveManagementSystem.WebApi
```
Visit [`https://localhost:5001/swagger/index.html`](https://localhost:5001/swagger/index.html) (or your configured port) to explore the API.

### Run with Docker
```bash
docker-compose up --build
```
Visit [http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html) to explore the API when running inside a Docker container.

### Connection String
Update `appsettings.json` with your connection string if needed:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=LeaveManagementSystem.db"
}
```

---

## API Endpoints

| Method | Route                             | Description                                                                                |
|--------|----------------------------------|--------------------------------------------------------------------------------------------|
| GET    | `/api/leaverequests`             | Get all leave requests                                                                     |
| GET    | `/api/leaverequests/filter`      | Filter and paginate leave requests                                                         |
| GET    | `/api/leaverequests/{id}`        | Get leave request by ID                                                                    |
| POST   | `/api/leaverequests`             | Create a new leave request                                                                 |
| PUT    | `/api/leaverequests/{id}`        | Update a leave request                                                                     |
| DELETE | `/api/leaverequests/{id}`        | Delete a leave request                                                                     |
| POST   | `/api/leaverequests/{id}/approve`| Approve a pending request                                                                  |
| GET    | `/api/leaverequests/report`      | Generate a report of approved leave requests filtered by year, date range, and department  |

---

## Postman Collection

Test all endpoints quickly using the provided Postman collection.

**Steps:**
1. Open Postman
2. Click `Import` > `Upload Files`
3. Select `LeaveManagementSystemAPI.postman_collection.json` from the project root
4. Click `Import` and start testing!

---

## Business Logic Highlights

- **Annual Limit Check**: Employees can’t exceed 20 annual leave days per year.
- **Overlap Check**: Leave periods cannot overlap for the same employee.
- **Sick Leave Reason**: A valid reason is required for sick leave.
- **Approval Restriction**: Only pending leave requests can be approved.

These rules are enforced in the `LeaveRequestService` layer via `ValidateLeaveRequestAsync`.

---

## Improvements to Consider

- Add FluentValidation for richer input validation
- Add unit/integration tests (xUnit, Moq)
- Introduce authentication (JWT-based)
- Use API versioning

---

## Author

**Khalil Frikha**  
📧 Email: khalilfrikha30@gmail.com  
📞 Phone: +21654647661  
🔗 GitHub: [StyKon](https://github.com/StyKon)
