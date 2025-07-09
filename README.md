# Registration_MVC

A clean and functional ASP.NET MVC web application for user registration, user listing, and profile picture management.

---

## 🚀 Features

- ✍️ User Registration Form:
  - First Name, Last Name
  - Date of Birth (DOB)
  - Email and Password
  - Profile Picture Upload
- 📋 User List View:
  - Displays all registered users in a styled table
  - Shows profile picture
  - Supports Edit (via modal) and Delete actions
- 🖼️ Image Upload & Update:
  - Stored in `wwwroot/uploads`
  - Images can be updated through modal popup
  - Old image gets deleted when new one is uploaded
- 🔐 Password Hashing using SHA256
- 📦 Uses SQL Server with ADO.NET + Stored Procedures
- 🎨 Bootstrap 5 UI + jQuery

---

## 🗂️ Project Structure

Registration_MVC/
├── Controllers/
│ └── RegistrationController.cs
├── Models/
│ ├── RegistrationModel.cs
│ └── UserUpdateViewModel.cs
├── Views/
│ ├── Registration/
│ │ ├── Index.cshtml
│ │ └── List.cshtml
│ └── Shared/
│ ├── _Layout.cshtml
│ ├── _ViewImports.cshtml
│ ├── _ViewStart.cshtml
│ └── _ValidationScriptsPartial.cshtml
├── wwwroot/
│ ├── uploads/ (.gitkeep only, for image storage)
│ └── favicon.ico
├── appsettings.json
├── Program.cs
├── Registration_MVC.csproj
└── .gitignore

yaml
Copy
Edit

---

## 💾 Database Setup

This project uses a stored procedure named `ManageUser` in SQL Server.

### Stored Procedure Parameters:
- `@Action`: `'INSERT'`, `'UPDATE'`, `'SELECT'`, `'DELETE'`
- `@UserID`: User ID (for update/delete)
- `@FirstName`, `@LastName`, `@DOB`, `@Email`, `@ImagePath`

> ⚠️ Make sure the `User_Info` and `User_Login` tables exist in your SQL database.

---

## ⚙️ How to Run

1. Clone or download this repo
2. Open `Registration_MVC.sln` in Visual Studio
3. Update your database connection string in `appsettings.json`
4. Run the project (Ctrl + F5)

---

## 🧪 Example Usage

- Go to `/Registration/Index` → Fill form → Register a user
- Go to `/Registration/List` → View, Edit, or Delete users
- Image uploads saved to `/wwwroot/uploads`

---

## ✅ .gitignore Highlights

- `bin/`, `obj/`, `.vs/` — ignored
- `wwwroot/uploads/*` — ignored except `.gitkeep`
- Keeps your repo clean and light

---

## 📸 Screenshots (optional)

screenshots/
├── registration.png
├── userlist.png

yaml
Copy
Edit

You can add images and reference them here later.

---

## 🛠️ Future Enhancements

- Form validation (client + server side)
- User login / authentication system
- Pagination & search in user list
- Role-based access (admin/user)

---

## 📄 License

This project is open-source and free to use for learning and demonstration purposes.
