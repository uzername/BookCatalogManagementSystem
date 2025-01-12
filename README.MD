## Description

This is a Book Catalog Management System that allows users to manage and track a collection of books. 
The solution consists of backend and frontend projects. The backend exposes API endpoints for managing books, while the frontend allows users to interact with the catalog through a web interface.

### Backend

- [x] CRUD operations for book entity. 
- [x] Support searching and filtering by title, author, and genre. 
- [x] Pagination and sorting options. 
- [x] Bulk upload functionality to import multiple books from a CSV file.
- [ ] Special: rate limiting to prevent abuse of the API.

### Frontend 
[x] view the list of books in a table format. 
[x] search and filter books by title, author, and genre.
[ ] add new books to the catalog via a form. 
[ ] update book details. 
[ ] delete books from the catalog. 
[ ] Ensure the UI has a responsive design that works seamlessly on both desktop and mobile devices.
[ ] Special: implement real-time updates to the book list, allowing the list to refresh automatically when changes are made. 

### Technologies
- Backend: C#, ASP.NET Core Minimal API
- Frontend: Blazor WebAssembly
- Database: In-memory storage
- Containerization: Docker, Docker Compose
