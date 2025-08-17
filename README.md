# 🎬 Movies API

A simple RESTful API for managing **movies and genres** built with **ASP.NET Core 6** and **Entity Framework Core**.

## ✨ Features
- CRUD operations for movies & genres
- Poster image upload (JPG/PNG, max 1MB)
- Get movies by genre
- Swagger API documentation
- Validation & error handling

## ⚙️ Requirements
- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- SQL Server (Express or LocalDB)

## 🚀 Getting Started
1. Clone the repo:
   ```bash
   git clone https://github.com/your-username/movies-api.git
   cd movies-api

## Update connection string in appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=MoviesDB;Trusted_Connection=True;"
}
## Run migrations:
  dotnet ef database update

## Run the app:
  dotnet run

📌 Example Endpoints

GET /api/Genres → Get all genres

POST /api/Movies → Create new movie (with poster)

GET /api/Movies/GetByGenreId?genreId={id} → Movies by genre

🖼️ Image Upload

Allowed formats: JPG, PNG

Max size: 1MB

Sent as form-data with key Poster
