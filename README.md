# Registration_MVC

An ASP.NET MVC web application for registering users, managing user profiles, and storing profile pictures in SQL Server using stored procedures.

---

## 🚀 Features

- User registration with:
  - First Name, Last Name
  - Date of Birth (DOB)
  - Email & Password
  - Profile Picture Upload
- User listing with:
  - Image preview
  - Edit via modal popup
  - Delete option
- Profile image update support
  - Deletes old image when new one is uploaded
- Passwords are hashed using SHA256
- SQL Server integration via stored procedures
- Clean layout using Bootstrap 5

---

## 🛠️ Technologies

- ASP.NET MVC
- C#
- ADO.NET + SQL Server
- HTML, CSS, Bootstrap 5
- jQuery & AJAX
- Visual Studio

---

## 🗃️ Stored Procedure: `ManageUser`

```sql
CREATE PROCEDURE ManageUser
    @Action VARCHAR(10),
    @UserID INT = NULL,
    @FirstName NVARCHAR(100) = NULL,
    @LastName NVARCHAR(100) = NULL,
    @DOB DATE = NULL,
    @Email NVARCHAR(200) = NULL,
    @ImagePath NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'INSERT'
    BEGIN
        INSERT INTO User_Info (FirstName, LastName, DOB, Email, ImagePath)
        VALUES (@FirstName, @LastName, @DOB, @Email, @ImagePath)
    END

    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE User_Info
        SET FirstName = @FirstName,
            LastName = @LastName,
            DOB = @DOB,
            ImagePath = @ImagePath
        WHERE UserID = @UserID
    END

    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM User_Info WHERE UserID = @UserID
    END

    ELSE IF @Action = 'SELECT'
    BEGIN
        SELECT * FROM User_Info
    END
END
⚠️ Make sure the User_Info table and User_Login table (for email/password) exist in your DB.

🏃 How to Run
Clone or download the repository

Open the solution in Visual Studio

Update the SQL connection string in appsettings.json

Run the app (Ctrl + F5)

Navigate to /Registration/Index to register users

📂 Note
Profile images are stored in wwwroot/uploads

.gitkeep is used to keep the folder tracked in Git

The uploads folder is auto-created at runtime if missing

📝 License
This project is free to use for learning and demo purposes.
2. Go to your GitHub repo → **Add file → Create new file**  
3. Name it: `README.md`  
4. Paste and **Commit** ✅
