using System.Text.Json;

namespace MaintenanceSystem.Helpers
{
    public static class SessionHelper
    {
        public static void SetUser(
       this ISession session,
       int id,
       string fullName,
       string role,
       int? departmentId)
        {
            var userData = new
            {
                Id = id,
                FullName = fullName,
                Role = role,
                DepartmentId = departmentId
            };

            session.SetString(
                "User",
                JsonSerializer.Serialize(userData));
        }

        public static UserSession GetUser(this ISession session)
        {
            var user = session.GetString("User");

            if (user == null)
                return null;

            return JsonSerializer.Deserialize<UserSession>(user);
        }

        public static void ClearUser(this ISession session)
        {
            session.Clear();
        }

        public static bool IsLoggedIn(this ISession session)
        {
            return session.GetString("User") != null;
        }
        // Check if user is Admin
        public static bool IsAdmin(this ISession session)
        {
            var user = session.GetUser();

            return user != null && user.Role == "Admin";
        }

        // Check if user is Technician
        public static bool IsTechnician(this ISession session)
        {
            var user = session.GetUser();

            return user != null && user.Role == "Technician";
        }

        // Check if user is Employee
        public static bool IsEmployee(this ISession session)
        {
            var user = session.GetUser();

            return user != null && user.Role == "Employee";
        }
    }

    public class UserSession
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public int? DepartmentId { get; set; }
    }
}
