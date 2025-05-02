# Conway's Game of Life API 🧬

A production-ready RESTful API built with ASP.NET Core 9 to simulate **Conway's Game of Life**, including state persistence using SQLite.

---

## 🚀 Getting Started

```bash
dotnet restore
dotnet ef database update
dotnet run
````

Then open Swagger UI:

```
https://localhost:<port>/swagger
```

---

## 🧱 Architecture Overview

- **ASP.NET Core 9** – Modern web framework
- **Entity Framework Core + SQLite** – Lightweight database with restart persistence

---

### ✅ Layers and dependencies

| Layer                             | Depends On →                                      | Purpose                                                                                                                                                                                                                         |
| ---------------                   | --------------------------------                  | ---------------------------------------------                                                                                                                                                                                   |
| `Model`                           | (none)                                            | Defines the core domain objects and value structures of the application. It contains pure business representations, free from persistence or transport concerns. This layer is the most stable and reusable part of the system. |
| `Common`                          | (used by all)                                     | Holds reusable types, validations, interfaces                                                                                                                                                                                   |
| `Application`                     | `Persistence`                                     | Contains business logic and game rules, uses Persistence as a door to persist the objects                                                                                                                                       |
| `Presentation.Api`                | `Application`                                     | Exposes endpoints, handles requests/responses                                                                                                                                                                                   |
| `Persistence.EF`                  | `Persistence`, `Common`                           | Implements data access using EF Core                                                                                                                                                                                            |
| `Persistence`                     | (none, abstract)                                  | Declares contracts for data access                                                                                                                                                                                              |
| `Common.Mapper`                   | `Common`                                          | Converts between domain/DTO models                                                                                                                                                                                              |
| `Tests`                           | `Presentation.Api`, `Application`, `Common`       | Validates functionality                                                                                                                                                                                                         |

![Onion Architecture](onion-architecture.png)

---

## 📬 API Endpoints

| Method | Route                          | Description                            |
| ------ | ------------------------------ | -------------------------------------- |
| POST   | `/boards`                      | Creates a new board                    |
| GET    | `/boards/{id}/next`            | Gets the next generation               |
| GET    | `/boards/{id}/advance/{steps}` | Advances board by `steps` generations  |
| GET    | `/boards/{id}/final?max=1000`  | Resolves until final/stable generation |

---

### 🤝 Interaction Examples

#### 📤 Create a new board

```http
POST /boards
Content-Type: application/json
```

**Request Body**

```json
{
  "initialState": [
    [false, true, false],
    [false, true, false],
    [false, true, false]
  ]
}
```

**Response**

```json
{
  "id": "c1f5e7a2-7d58-41f2-9a4b-4932f2e1d6e1"
}
```

---

#### ⏭️ Get next generation

```http
GET /boards/c1f5e7a2-7d58-41f2-9a4b-4932f2e1d6e1/next
```

**Response**

```json
{
  "initialState": [
    [false, false, false],
    [true,  true,  true],
    [false, false, false]
  ]
}
```

---

#### 🔁 Advance multiple generations

```http
GET /boards/{id}/advance/10
```

Returns the board after 10 steps.

---

#### 🧬 Get final state (until stable)

```http
GET /boards/{id}/final?max=1000
```

Returns final state if reached before 1000 steps.
Returns `400 BadRequest` if not stabilized.

---

## ✅ Validation Rules

* **Minimum board size**: 3x3
* **Maximum board size**: 100x100

Returns `400 BadRequest` if limits are not respected.

---

## 🧪 Unit Testing

Running:

```bash
dotnet test
```

| Tooling                         | Purpose                                    |
| ------------------------------- | ------------------------------------------ |
| **xUnit**                       | Testing framework                          |
| **Mocking**                     | Simulate dependencies (e.g., repositories) |

#### ✅ Covered Scenarios

* Game logic transitions (e.g. still life, blinker, glider)
* Stability detection (final state after N steps)
* Validation of board boundaries and formats
* Future: Integration and persistence tests

---

## 🧭 TODO / Next Steps

### 🧠 Business rules
- [ ] Matrix auto growing.
- [ ] Steps history persistence to prevent cases below:
    - When using `next` the game continues to move forward even if it has already reached the final state, since it is not possible to determine it because we do not save the state history.
    - When using `final` the game continues the movement from the current step, so it is possible to determine the `end step` as a future step from the current one, even if the end state has already occurred in previous steps (during `next` calls). This is also due to the fact that we are not saving the step history.

---

## 📄 License

MIT License © 2025