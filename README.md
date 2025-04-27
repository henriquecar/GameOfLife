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

## API Endpoints
- `POST /boards`
- `GET /boards/{id}/next`
- `GET /boards/{id}/advance/{steps}`
- `GET /boards/{id}/final?max=1000`

## Testing
```bash
dotnet test
```

## Development decisions
- Custom mapper strategy using IMapper custom interface to prevent AutoMapper dependency and make the system infrastructure more cheap.
- I'm not validating the minimun or maximun matrix size, I'm supposing they should be raised with product owner.

## Next steps (discuss with PO)
- Matrix auto growing.
- Steps history persistence.
