## Description

This is a Book Catalog Management System that allows users to manage and track a collection of books. 
The solution consists of backend and frontend projects. The backend exposes API endpoints for managing books, while the frontend allows users to interact with the catalog through a web interface.

### Backend

- [x] CRUD operations for book entity. 
- [x] Support searching and filtering by title, author, and genre. 
- [x] Pagination and sorting options. 
- [x] Bulk upload functionality to import multiple books from a CSV file.
- [x] Special: rate limiting to prevent abuse of the API.

### Frontend 
- [x] view the list of books in a table format. 
- [x] search and filter books by title, author, and genre.
- [x] add new books to the catalog via a form. 
- [x] update book details. 
- [x] delete books from the catalog. 
- [x] Ensure the UI has a responsive design that works seamlessly on both desktop and mobile devices.
- [ ] Special: implement real-time updates to the book list, allowing the list to refresh automatically when changes are made. 

### Technologies
- Backend: C#, ASP.NET Core Minimal API
- Frontend: Blazor WebAssembly
- Database: In-memory storage
- ~~Containerization: Docker, Docker Compose~~ Something went wrong with Docker, but I add those builds

### Run API

Unpack it and then use command:

```
cd path/to/release
dotnet BCMS_backend.dll --urls=http://localhost:5195
```

`path/to/release` should contain BCMS_Backend.dll file. API is available at http://localhost:5195 . Hope you have port 5195 freed

### Run Blazor WebAssembly project

Unfortunately Blazor WebAssembly project is published without dll files, it has only .wasm files which cannot be served by dotnet command. You need a **static webserver** to run this project. Anything that may serve static files. Unfortunately, project won't work if you doubleclick on index.html file. 

I found on internet a static webserver for serving static files, here it is: https://static-web-server.net/download-and-install/ .
I also include it to release.

Use such command to run it:

```
cd path/to/release
static-web-server.exe --root wwwroot --port 7164
```

If you use Node JS  and have installed npm , you may use this: https://www.npmjs.com/package/@node-cli/static-server . 

```
npm install -g @node-cli/static-server
cd path/to/release
static-server -p 7164 wwwroot
```

Setting up npm and node is a separate challenge.

Here `path/to/release` is where you unpacked BCMS_FrontendBlazor

Anyway, a project should be available at http://localhost:7164 .

## Pictures

![screenshot](screenshot.PNG?raw=true "screenshot")