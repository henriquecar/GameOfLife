# Conway's Game of Life API

This is a production-ready REST API in ASP.NET Core for Conway's Game of Life.

## Features
- Upload and persist board state
- Get next state, advance X generations, or resolve to final state
- Board state survives restarts using SQLite
- Includes basic unit tests

## Running
```bash
dotnet restore
dotnet ef database update
dotnet run
```

## Testing
```bash
dotnet test
```

## API Endpoints
- `POST /boards`
- `GET /boards/{id}/next`
- `GET /boards/{id}/advance/{steps}`
- `GET /boards/{id}/final?max=1000`
