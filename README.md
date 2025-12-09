# Library Management System

A simple Library Management System built with ASP.NET Core Web Api that allows users to manage books, perform CRUD operations, and search for books.

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)

## Features

- **CRUD Operations**: Create, Read, Update, and Delete books.
- **Search Functionality**: Search for books by title, author, or genre.
- **Data Validation**: Ensures book information is accurate and consistent.
- **In-Memory Caching**: Utilizes in-memory caching for better performance on frequent read operations.

## Architecture

The project follows a **layered architecture** consisting of:

- **Controllers**: Handle incoming API requests and direct them to the appropriate services.
- **Service Layer**: Contains business logic and validations, managing the application’s core functionality.
- **Repository Layer**: Responsible for data access, managing interactions with the database.

This architecture helps maintain clean separation of concerns, enhancing the maintainability and scalability of the application.

## Technologies Used

- ASP.NET Core Web Api 8.0
- Entity Framework Core
- SQL Server (or any other database of your choice)
- Swagger (for API documentation)

## Installation

1. **Create the Database**:
   - Before running the application, ensure that your SQL Server is set up and a new database is created for the library management system.
   You can create the database using SQL Server Management Studio or any other database management tool.

2. **Clone the repository**:
   ```bash
   git clone https://github.com/hasandex/Library.Management.Demo.git

3. Configure the Database Connection:
   Update the database connection string in appsettings.json to point to your created database.

4. Restore the dependencies:
   dotnet restore

5. Run the application:
   dotnet run

## Usage
Once the application is running, you can access it via http://localhost:5108. You will see the main page where you can:

Add new books.
View the list of available books.
Update existing book information.
Delete books from the library.
Search for books using the search feature.

## API Endpoints

Method	Endpoint	Description:
- GET	/api/book/List	                        Get the list of all books.
- GET	/api/book/GetById/{id}	               Get a book by its ID.
- POST	/api/book/Create	                  Add a new book.
- PUT	/api/book/Update	                     Update an existing book.
- DELETE	/api/book/Delete/{id}	            Delete a book by its ID.
- GET	/api/book/List?search="key search"	   Search for books by title, author, or genre.
- GET /api/book/BooksRating                  Display books rating.
