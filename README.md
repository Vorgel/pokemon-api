# Pokemon API

This is a backend REST API for managing and retrieving data about Pokémon. It is designed to work as the data source for the frontend application [`pokemon-client`](https://github.com/Vorgel/pokemon-client), built with React.

## Technologies Used

* **.NET 8** – Core backend framework
* **Entity Framework Core** – ORM for database access
* **SQL Server Express** – Relational database

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/your-username/pokemon-api.git
cd pokemon-api
```

### 2. Setup environment variables

This project uses environment variables for sensitive configuration such as the database connection string.
Use the `.env.example` file as a template:

```bash
cp .env.example .env
```

Then, open the newly created `.env` file and provide your own values, especially the connection string:

```dotenv
ConnectionStrings__DefaultConnection=Server=localhost\SQLEXPRESS;Database=PokemonDb;Trusted_Connection=True;TrustServerCertificate=True;
```

> Make sure you have **SQL Server Express** installed and running locally.

### 3. Apply database migrations

Before running the API, make sure the database is up-to-date:

```bash
dotnet ef database update
```

### 4. Provide Pokémon data source

The API requires a source data file named `pokemon.json`. This file is included in the repository. If missing, the API will not function properly.

### 5. Run the application

```bash
dotnet run
```

The API is pre-defined to be set at: 'https://localhost:65176/api' on the frontend side. 
This is only due to quick development of this app and might be changed in the future.

## API Usage

The API exposes endpoints to:

* Get paginated, filtered and sorted lists of Pokémon
* Get unique generations and types (used for filtering in frontend)
* Get detailed Pokémon information

## Frontend

This API is intended to be used with the React frontend [`pokemon-client`](https://github.com/your-username/pokemon-client), which consumes the endpoints to render a UI for browsing, filtering, and viewing Pokémon data.

## License

MIT
