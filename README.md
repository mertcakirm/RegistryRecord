# 🗂️ RegistryRecord Backend (.NET Core + MySQL)

This project is the **backend API** for the RegistryRecord system, developed with **ASP.NET Core** and **MySQL**.  
It provides endpoints for managing users, records, and document files associated with individuals based on their registry numbers.

---

## 🚀 Features

- **Registry Number Lookup**
  - Retrieve all information and related documents for a person using their registry number.

- **Document Management**
  - Upload, update, and delete document files.
  - All files are stored physically under the `wwwroot` folder for easy access.

- **Image & File Handling**
  - Images and documents are saved under `wwwroot/uploads` (or a similar directory).
  - Accessible through static file serving via ASP.NET Core middleware.

- **User Management**
  - Simple authentication and role structure (Admin, User).
  - Admins can add, edit, or remove user records.

- **API Design**
  - RESTful API structure.
  - Clean, consistent JSON responses.
  - Compatible with any frontend (React, Angular, Vue, etc.).

---

## 🧱 Tech Stack

- **ASP.NET Core 8**
- **Entity Framework Core**
- **MySQL**
- **JWT Authentication**
- **Dependency Injection**

---

## ⚙️ Setup & Run

1. **Clone the repository**
   ```bash
   git clone https://github.com/mertcakirm/RegistryRecord-Backend.git
   cd RegistryRecord-Backend
