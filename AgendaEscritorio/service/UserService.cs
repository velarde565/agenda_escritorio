using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using AgendaEscritorio.model;

namespace AgendaEscritorio.service
{
    public class UserService
    {
        private List<User> users = new List<User>();

        public UserService()
        {
            // Usuarios hardcodeados de ejemplo
            users.Add(new User("admin", HashPassword("adminpass"), "admin"));
            users.Add(new User("user", HashPassword("userpass"), "user"));
        }

        // Método para autenticar un usuario
        public User? Authenticate(string username, string password) // Cambiar a User? para permitir nulos
        {
            string passwordHash = HashPassword(password);
            foreach (var user in users)
            {
                if (user.Username == username && user.PasswordHash == passwordHash)
                {
                    return user; // Retorna el usuario si es encontrado
                }
            }
            return null; // Usuario no encontrado o credenciales incorrectas
        }

        // Método para hashear la contraseña usando SHA256
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
