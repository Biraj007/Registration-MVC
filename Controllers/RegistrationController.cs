using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Registration_MVC.Models;
using System.Data;

namespace Registration_MVC.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IConfiguration _config;
        public RegistrationController(IConfiguration config) { _config = config; }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(UserModel data, IFormFile ProfilePic)
        {
            if (string.IsNullOrWhiteSpace(data.FirstName) || string.IsNullOrWhiteSpace(data.LastName) ||
                string.IsNullOrWhiteSpace(data.DOB) || string.IsNullOrWhiteSpace(data.Email) || string.IsNullOrWhiteSpace(data.Password))
                return BadRequest("All fields are required.");

            string hashedPassword = HashPassword(data.Password);
            string imagePath = "";

            if (ProfilePic != null && ProfilePic.Length > 0)
            {
                string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePic.FileName);
                string filePath = Path.Combine(uploads, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                ProfilePic.CopyTo(stream);

                imagePath = "/uploads/" + fileName;
            }

            using (SqlConnection con = new(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                SqlCommand cmd = new("ManageUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "INSERT");
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@DOB", DateTime.Parse(data.DOB).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@ImagePath", imagePath);
                cmd.ExecuteNonQuery();

                SqlCommand loginCmd = new(@"INSERT INTO User_Login (Email, Password) VALUES (@Email, @Password)", con);
                loginCmd.Parameters.AddWithValue("@Email", data.Email);
                loginCmd.Parameters.AddWithValue("@Password", hashedPassword);
                loginCmd.ExecuteNonQuery();
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<UserModel> list = new();
            using (SqlConnection con = new(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                SqlCommand cmd = new("ManageUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SELECT");
                using SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new UserModel
                    {
                        UserID = Convert.ToInt32(dr["UserID"]),
                        FirstName = dr["FirstName"].ToString() ?? "",
                        LastName = dr["LastName"].ToString() ?? "",
                        DOB = Convert.ToDateTime(dr["DOB"]).ToString("dd-MM-yyyy"),
                        Email = dr["Email"].ToString() ?? "",
                        ImagePath = dr["ImagePath"].ToString() ?? ""
                    });
                }
            }
            return Json(list);
        }

        [HttpPost]
        public IActionResult Delete(int UserID)
        {
            using (SqlConnection con = new(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                SqlCommand getEmail = new(@"SELECT Email FROM User_Info WHERE UserID = @UserID", con);
                getEmail.Parameters.AddWithValue("@UserID", UserID);
                string email = getEmail.ExecuteScalar() as string ?? string.Empty;

                if (string.IsNullOrEmpty(email)) return NotFound();

                SqlCommand delLogin = new(@"DELETE FROM User_Login WHERE Email = @Email", con);
                delLogin.Parameters.AddWithValue("@Email", email);
                delLogin.ExecuteNonQuery();

                SqlCommand cmd = new("ManageUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.ExecuteNonQuery();
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult Update(UserModel data, IFormFile? ProfilePic)
        {
            if (string.IsNullOrWhiteSpace(data.DOB))
                return BadRequest("Date of Birth is required.");

            string imagePath = data.ImagePath;

            if (ProfilePic != null && ProfilePic.Length > 0)
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePic.FileName);
                string filePath = Path.Combine(uploads, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                ProfilePic.CopyTo(stream);

                imagePath = "/uploads/" + fileName;
            }

            using (SqlConnection con = new(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                SqlCommand cmd = new("ManageUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "UPDATE");
                cmd.Parameters.AddWithValue("@UserID", data.UserID);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@DOB", DateTime.Parse(data.DOB).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(imagePath) ? (object)DBNull.Value : imagePath);

                cmd.ExecuteNonQuery();
            }

            return Ok();
        }


        private static string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            byte[] bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
