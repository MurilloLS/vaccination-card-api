# 💉 VaccinationCard API

![.NET 8](https://img.shields.io/badge/.NET-8.0-purple)
![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen)

---

## 📑 Table of Contents

1. [Overview](#-overview)
2. [Technologies and Tools](#-technologies-and-tools)
3. [Architecture and Patterns](#-architecture-and-patterns)
4. [Directory Structure](#-directory-structure)
5. [Design and Business Decisions (ADRs)](#-design-and-business-decisions-adrs)
6. [Security and Authentication](#-security-and-authentication)
7. [Getting Started Guide](#-getting-started-guide)
8. [Testing](#-testing)
9. [API Documentation](#-api-documentation)
10. [Database Modeling](#-database-modeling)

---

## 📋 Overview

The **VaccinationCard API** is a robust backend system developed for managing digital vaccination cards. The system allows citizen registration, management of a vaccine catalog (based on the national calendar), and historical recording of applied doses, ensuring data integrity and traceability of immunization records.

The project was built focusing on **Software Quality**, using market practices like Clean Architecture, CQRS, and Automated Testing.

---

## 🚀 Technologies and Tools

The project was developed using **C#** and **.NET 8**. Below are the main libraries and the rationale for their choice:

* **Entity Framework Core + SQLite:** Chosen for portability and local configuration ease, enabling robust persistence without needing to install heavy database servers.
* **MediatR:** Used to implement the **CQRS** pattern (Command Query Responsibility Segregation), completely decoupling Controllers from business logic.
* **FluentValidation:** Implementation of the *Fail-fast* strategy. Business rules (ex: negative age, invalid dose) are validated before even touching the domain.
* **AutoMapper:** To transform between Entities (Domain) and DTOs (API Contracts), avoiding exposure of sensitive data.
* **BCrypt.Net:** Industry standard for password hashing. No password is saved in plain text.
* **xUnit + Moq + FluentAssertions:** Testing stack to ensure business logic (Handlers) works isolated and correctly.
* **Swashbuckle (Swagger):** For interactive documentation and manual API testing.

---

## 🏛️ Architecture and Patterns

The project strictly follows **Clean Architecture**, dividing responsibilities into concentric layers:

1. **Domain (Core):** Contains Entities (`Person`, `Vaccine`, `User`), Repository Interfaces and pure Business Rules. Does not depend on any other layer.
2. **Application (Orchestration):** Contains Use Cases implemented via **CQRS** (`Commands` for writing, `Queries` for reading), DTOs and Validators.
3. **Infrastructure (External World):** Implements data access (`Repositories`, `DbContext`), encryption services and JWT token generation.
4. **Api (Entry):** Contains REST Controllers and Middleware configurations. Only receives the request and delivers it to `MediatR`.

### Patterns Used
* **CQRS:** Clear separation between Read and Write operations, allowing future optimizations and cleaner code.
* **Repository Pattern:** Abstraction of the data layer, facilitating database switching and Mock creation for testing.
* **Dependency Injection:** Extensive use of .NET's native container for inversion of control.

---

## 📂 Directory Structure

```text
VaccinationCardSolution/
├── src/
│   ├── VaccinationCard.Api/           # Controllers, Configuration, Global Middleware
│   ├── VaccinationCard.Application/   # UseCases (CQRS), DTOs, Validators, Interfaces
│   ├── VaccinationCard.Domain/        # Entities, Enums, Custom Exceptions
│   └── VaccinationCard.Infrastructure/# EF Core, Repositories, Services (Auth)
└── tests/
    ├── VaccinationCard.UnitTests/        # Logic Tests (Handlers) with Mocks
    └── VaccinationCard.IntegrationTests/ # E2E Tests (API + In-Memory Database)
```

--- 

## 🧠 Design and Business Decisions (ADRs)

In this section, I document the strategic choices made during development to balance the challenge requirements with software engineering best practices.

### 1. Vaccine Management: Data Seeding vs. Public CRUD
* **The Dilemma:** The challenge requested the "Register a vaccine" functionality. However, in real healthcare systems, vaccines are **Reference Data** standardized by the Ministry of Health. Allowing any user to register vaccines would generate duplication (ex: "Flu", "Gripe", "Influenza") and inconsistency in reports.
* **The Decision:**
    1.  Prioritized data integrity using **Data Seeding** (`DbInitializer`). This ensures the system is born with the official catalog loaded, facilitating immediate testing by the evaluator without manual pre-configuration.
    2.  To strictly meet the functional requirement of the challenge, implemented the management endpoints (`POST`, `PUT`, `DELETE` at `/api/Vaccines`), but protected them via **RBAC (Role-Based Access Control)**. Only users with **ADMIN** profile can alter the catalog, simulating a real Backoffice scenario.

### 2. Category Structure: Normalization vs. Visualization
* **The Problem:** The reference visual interface suggests a "Single Grid" (National Wallet), but clinically, some vaccines (ex: Meningo B) belong to the private network. The dilemma was: simplify the database to have one category or model it correctly?
* **The Decision (Data-Driven Architecture):** Opted to keep the database **normalized and semantic** (Standard *Source of Truth*).
    * **Backend (Truth):** Vaccines are registered in their real categories ("Basic SUS", "Private", etc.) in the database via Seed.
    * **Frontend (Visualization):** The 1:N structure between `VaccineCategory` and `Vaccine` was maintained. This allows the Frontend to treat the "National Wallet" as an aggregating view, displaying essential vaccines regardless of their database category. This approach facilitates future maintenance if new tabs need to be created only via SQL, without code refactoring.

### 3. Deletion Strategy: Verbose Delete
* **The Problem:** The REST standard suggests returning `204 No Content` for successful deletions. However, in critical healthcare systems, the user needs clear feedback about what was just removed to avoid operational errors (ex: deleting the wrong patient record).
* **The Decision:** Implemented **Verbose Delete**. The `DELETE` endpoints return status `200 OK` containing the JSON of the deleted object. This improves **User Experience (UX)**, allowing the Frontend to display precise confirmation messages (ex: *"The BCG vaccine record for Murillo was removed"*).

### 4. Error Handling: Global Exception Handler
* **The Problem:** Validating business rules (ex: "Age cannot be negative", "Vaccine already applied") within Controllers generates repetitive and "dirty" code with multiple `try-catch` blocks.
* **The Decision:** Used the native .NET 8 `IExceptionHandler` middleware.
    * Created a custom exception `DomainException`.
    * The Controller only executes the "happy path". If a rule is violated, the Middleware intercepts the error and standardizes the JSON response as **400 Bad Request** (according to [RFC 7807](https://tools.ietf.org/html/rfc7807)). This keeps Controllers clean and focused only on HTTP orchestration.

### 5. Offensive Security: Blocking Destructive Routes

| Resource | Endpoint | Method | Action | USER Access | ADMIN Access |
| :--- | :--- | :---: | :--- | :---: | :---: |
| **Auth** | `/api/Auth/register` | `POST` | Create User | ✅ Public | ✅ Public |
| | `/api/Auth/login` | `POST` | Get Token | ✅ Public | ✅ Public |
| **Persons** | `/api/Persons` | `POST` | Create Patient | ✅ Yes | ✅ Yes |
| | `/api/Persons` | `GET` | List All | ✅ Yes | ✅ Yes |
| | `/api/Persons/{id}` | `GET` | View Card | ✅ Yes | ✅ Yes |
| | `/api/Persons/{id}` | `PUT` | Edit Patient | ✅ Yes | ✅ Yes |
| | `/api/Persons/{id}` | `DELETE` | Delete Patient | ❌ **Forbidden** | ✅ **Allowed** |
| **Vaccines** | `/api/Vaccines` | `GET` | List Catalog | ✅ Yes | ✅ Yes |
| | `/api/Vaccines` | `POST` | Create Vaccine | ❌ **Forbidden** | ✅ **Allowed** |
| | `/api/Vaccines/{id}` | `PUT` | Correct Name | ❌ **Forbidden** | ✅ **Allowed** |
| | `/api/Vaccines/{id}` | `DELETE` | Delete Vaccine | ❌ **Forbidden** | ✅ **Allowed** |
| **Vaccinations**| `/api/Vaccinations` | `POST` | Apply Dose | ✅ Yes | ✅ Yes |
| | `/api/Vaccinations/{id}`| `GET` | View Detail | ✅ Yes | ✅ Yes |
| | `/api/Vaccinations/{id}`| `PUT` | Correct Dose | ✅ Yes | ✅ Yes |
| | `/api/Vaccinations/{id}`| `DELETE` | Reverse Dose | ❌ **Forbidden** | ✅ **Allowed** |
---

## 🔒 Security and Authentication

The API is protected via **JWT (JSON Web Token)**.

1. **Register/Login:** User sends credentials. API validates password hash (BCrypt) and returns a signed Token.
2. **Access:** Client must send the `Authorization: Bearer <TOKEN>` header in all protected requests.
3. **Swagger:** Documentation has native support (lock button 🔒) to test authenticated endpoints.

---

## ⚡ Getting Started Guide

### 🛠️ Prerequisites

* **[.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)**: Required to compile and run the application.
* **Git**: To clone the repository.

### 🚀 How to Run (Step by Step)

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/MurilloLS/vaccination-card-api.git
    cd VaccinationCardSolution
    ```

2.  **Run the API:**
    The application uses **SQLite**. No external database installation is required.
    When running the command below, the system will automatically:
    * Create the database file (`vaccination_card.db`).
    * Apply migrations (Tables).
    * Populate initial data (SUS Vaccines and Admin User).
    
    ```bash
    dotnet run --project src/VaccinationCard.Api
    ```

3.  **Access Interactive Documentation:**
    Open your browser at the address indicated in the terminal (usually port 5000 or 5205).
    👉 **http://localhost:5205/swagger**

### 🔑 Access Credentials (Automatic Seed)

The system comes pre-configured with a superuser to facilitate evaluation:

| Role | Username | Password | Permissions |
| :--- | :--- | :--- | :--- |
| **ADMIN** | `admin` | `admin123` | Full Access (Includes `DELETE` and catalog management). |
| **USER** | *(Create via API)* | *(Create via API)* | Operational (Register vaccination, Consult). |

---

### 🕵️‍♂️ How to Test Profiles (Security Tutorial)

To validate the **RBAC** (Role-Based Access Control) security system, follow this script in Swagger:

#### 1. Testing "Full Power" (ADMIN)
1.  In Swagger, go to **Auth** -> `POST /api/Auth/login`.
2.  Use the seed credentials (`admin` / `admin123`).
3.  Copy the generated **Token** from the response (long string).
4.  Go to the top of the page, click the **Authorize** button (lock) and paste the token in the format:
    `Bearer <your_token_here>`
5.  Now try to delete a patient (`DELETE /api/Persons/1`).
    * **Result:** ✅ `200 OK` (The operation is allowed).

#### 2. Testing "Restricted Access" (USER)
1.  Create a common user at `POST /api/Auth/register` (ex: `username: "nurse"`).
2.  Login with it and get the new Token.
3.  Change the token in the **Authorize** button (Logout first).
4.  Try to create a new vaccine in the catalog (`POST /api/Vaccines`).
    * **Result:** 🚫 `403 Forbidden` (The system blocks the action, proving security works).

---

## 🧪 Testing

The project has a comprehensive test suite.

### How to Run

```bash
dotnet test
```

### Coverage

* **Unit Tests (`xUnit` + `Moq`):** Cover 100% of *Handlers* (Use Cases). Validate business rules, age calculations, dose validation and domain exceptions, isolating the database.
* **Integration Tests (`WebApplicationFactory`):** Validate the complete flow (HTTP → Auth → In-Memory Database), ensuring the API responds correctly and the JWT token is validated.

---

## 📖 API Documentation

The API is fully documented via **Swagger UI** (accessible at `/swagger`). Below is the detailing of resources, their responsibilities and access levels:

### 🔐 Auth (Authentication)
*Responsible for credential issuance.*
* `POST /api/Auth/register`: Creation of new users in the system.
* `POST /api/Auth/login`: Authentication via credentials. Returns the **Bearer Token** JWT needed to access protected routes.

### 👤 Persons (Patients)
*Management of citizen registration and card visualization.*
* `GET /api/Persons`: Lists all registered citizens.
* `GET /api/Persons/{id}`: **[Highlight]** Returns the complete **Digital Vaccination Card** (Personal data + History of taken doses).
* `POST /api/Persons`: Registers a new citizen.
* `PUT /api/Persons/{id}`: Updates registration data (Name, Age, etc).
* `DELETE /api/Persons/{id}`: 🛡️ **Restricted (Admin)**. Removes the citizen and **cascading deletes** all their vaccination history.

### 💉 Vaccinations (Dose Registration)
*The "Core" of the system. Records the act of vaccination (Link: Person + Vaccine + Dose + Date).*
* `POST /api/Vaccinations`: Registers an applied dose.
    * *Validations:* Prevents future dates, verifies existence of patient/vaccine and validates dose types (D1, D2, R1...).
* `GET /api/Vaccinations/{id}`: Consults details of a specific application record.
* `PUT /api/Vaccinations/{id}`: Allows correction of wrong entries (ex: incorrect date or dose).
* `DELETE /api/Vaccinations/{id}`: 🛡️ **Restricted (Admin)**. Allows reversal/removal of a vaccine application record.

### 🧪 Vaccines (Immunization Catalog)
*Management of Reference Data (Loaded via Seed).*
* `GET /api/Vaccines`: Lists all available vaccines in the catalog to populate the selection grid.
* `POST /api/Vaccines`: 🛡️ **Restricted (Admin)**. Adds new vaccines to the catalog.
* `PUT /api/Vaccines/{id}`: 🛡️ **Restricted (Admin)**. Corrects names or categories of vaccines.
* `DELETE /api/Vaccines/{id}`: 🛡️ **Restricted (Admin)**. Removes vaccines from the catalog.
    * *Security Lock:* The system **prevents** deletion if the vaccine has already been applied to any patient, ensuring historical integrity.

---

# 🗄 Database Modeling

## 🧱 Physical Model (SQL ANSI 2003 – brModelo)

```sql
-- Sql ANSI 2003 - brModelo.

CREATE TABLE PERSON (
id_person INTEGER PRIMARY KEY,
nm_person VARCHAR(150) NOT NULL,
nr_age_person INTEGER NOT NULL,
sg_gender_person CHAR(1) NOT NULL
)

CREATE TABLE VACCINE (
id_vaccine INTEGER PRIMARY KEY,
nm_vaccine VARCHAR(150) NOT NULL,
id_vaccine_category INTEGER NOT NULL
)

CREATE TABLE VACCINE_CATEGORY (
id_vaccine_category INTEGER PRIMARY KEY,
nm_vaccine_category VARCHAR(100) NOT NULL
)

CREATE TABLE USER (
id_user INTEGER PRIMARY KEY,
nm_user VARCHAR(100) NOT NULL UNIQUE,
pwd_hash_user VARCHAR(255) NOT NULL,
sg_role CHAR(5) NOT NULL DEFAULT 'USER'
)

CREATE TABLE VACCINATION (
id_vaccine INTEGER NOT NULL,
id_person INTEGER NOT NULL,
id_vaccination INTEGER PRIMARY KEY,
cd_dose_vaccination CHAR(5) NOT NULL,
dt_application_vaccination DATE NOT NULL,
FOREIGN KEY(id_vaccine) REFERENCES VACCINE (id_vaccine),
FOREIGN KEY(id_person) REFERENCES PERSON (id_person)ON DELETE CASCADE
)

ALTER TABLE VACCINE ADD FOREIGN KEY(id_vaccine_category) REFERENCES VACCINE_CATEGORY (id_vaccine_category)
```

---

## 🔄 History of Evolutions and Recent Decisions (Errata)

This section documents structural changes and technical decisions implemented after the initial version of the project.

### 1. Dynamic Doses Implementation (Database Changed)

* **The Challenge:** Initially, the system did not restrict the number of vaccine doses in the Backend, depending on fixed rules in the Frontend. This generated inconsistencies when creating new vaccines.
* **The Solution:** Evolved to a **Data-Driven** approach.
    * **Backend:** Added the `MaxDoses` property in the `Vaccine` entity, validating values between 1 (single dose) and 5 (complete scheme). The `VACCINE` table received the corresponding column.
    * **Frontend:** Vaccine registration now requires the Admin to define the number of doses. The card interface dynamically adapts, displaying only the dose "slots" pertinent to that vaccine.
    * **Seed:** The `DbInitializer` was updated to load vaccines with their real doses (ex: BCG = 1 dose, Polio = 5 doses), ensuring realism from the first boot.

```sql
CREATE TABLE VACCINE (
id_vaccine INTEGER PRIMARY KEY,
nm_vaccine VARCHAR(150) NOT NULL,
id_vaccine_category INTEGER NOT NULL,
nr_max_doses INTEGER NOT NULL DEFAULT 5 -- New Column
)
```

### 2. Category Link Correction (Return Payload)

* **The Problem:** When creating a vaccine via API, the response DTO returned the category name as `null`, because Entity Framework did not immediately reload the navigation property after insertion.
* **The Solution:**
    * Updated `VaccineDto` to explicitly expose `CategoryId`, ensuring immediate confirmation of the saved link.
    * Adjusted `CreateVaccineHandler` to reload the complete entity (with `Include`) before mapping to the DTO, ensuring the client (Frontend/Swagger) receives the complete and consistent object instantly.

### 3. Test Suite Update

* **Impact:** Changes in the `Vaccine` entity constructor (requiring `MaxDoses`) broke existing unit tests.
* **Action:** All use case tests (`Create`, `Update`, `Delete`, `GetAll`) were refactored to contemplate the new property, maintaining test coverage at 100% and ensuring the new business rule (1-5 doses) is respected.