namespace MaintenanceSystem.Helpers
{
    public static class PasswordHasher
    {
        //hashing the password using BCrypt
        public static string Hash(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        // compare the hashed password with the plain password
        public static bool Verify(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
