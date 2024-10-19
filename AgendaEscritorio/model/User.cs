using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaEscritorio.model
{
    public class User
    {
        public string Username { get; set; } = string.Empty; // Inicializa como cadena vacía
        public string PasswordHash { get; set; } = string.Empty; // Asegúrate de tener esta propiedad
        public string Role { get; set; } = string.Empty; // Inicializa como cadena vacía

        public User(string username, string passwordHash, string role)
        {
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }
    }
}
