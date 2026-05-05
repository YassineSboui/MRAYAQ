# MRAYAQ

Brand site for MRAYAQ (Tunisian menswear) built with Vue 3 + Vite and .NET 8 minimal API.

## Structure

- `frontend` — Vue 3 + Vite (TypeScript)
- `backend` — ASP.NET Core .NET 8 minimal API

## Run (dev)

### Backend

```bash
cd backend
# If needed: dotnet dev-certs https --trust
# Set your connection string (recommended: user secrets)
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Default" "Server=YOUR_SERVER;Database=MraYaQ;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True"
# Restore then run
 dotnet restore
 dotnet run
```

The API will run on [https://localhost:5001](https://localhost:5001) by default.

### Frontend

```bash
cd frontend
npm install
npm run dev
```

The site runs on [http://localhost:5173](http://localhost:5173).

## API

- `POST /api/contact`
- `GET /api/categories`
- `GET /api/products?search=&categoryId=&minPrice=&maxPrice=`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`
- `POST /api/orders`
- `GET /api/orders` (latest)
- `GET /api/orders/{id}`

## Database

The contact form writes messages to SQL Server (table `ContactMessages`).
Products, categories, and orders are also stored in SQL Server.
If you prefer environment variables, set:

```bash
MRAYAQ_CONNECTION_STRING=Server=YOUR_SERVER;Database=MraYaQ;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True
```

### Optional env var

If you want a different API base URL, set:

```bash
VITE_API_BASE=https://localhost:5001
```
"# MRAYAQ" 
