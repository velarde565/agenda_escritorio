using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AgendaEscritorio.model;
using AgendaEscritorio.service;

namespace AgendaEscritorio.simulated
{
    public class SimulatedServer
    {
        private TcpListener listener;
        private UserService userService;

        public SimulatedServer()
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
            userService = new UserService(); // Usa el servicio de usuarios que ya tienes
        }

        public async Task StartAsync()
        {
            listener.Start();
            Console.WriteLine("Servidor iniciado...");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                await Task.Run(() => HandleClient(client));
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            if (string.IsNullOrEmpty(request))
            {
                Console.WriteLine("Error: Solicitud vacía");
                client.Close();
                return;
            }

            LoginRequest? loginRequest; // Cambiar a LoginRequest?
            try
            {
                loginRequest = JsonConvert.DeserializeObject<LoginRequest>(request);
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine("Error al deserializar JSON: " + ex.Message);
                LoginResponse errorResponse = new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Solicitud malformada"
                };

                string errorJsonResponse = JsonConvert.SerializeObject(errorResponse);
                byte[] errorResponseData = Encoding.UTF8.GetBytes(errorJsonResponse);
                await stream.WriteAsync(errorResponseData, 0, errorResponseData.Length);
                client.Close();
                return;
            }

            // Asegúrate de manejar el caso donde loginRequest puede ser nulo
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                LoginResponse invalidInputResponse = new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Username o Password no pueden estar vacíos"
                };

                string invalidJsonResponse = JsonConvert.SerializeObject(invalidInputResponse);
                byte[] invalidResponseData = Encoding.UTF8.GetBytes(invalidJsonResponse);
                await stream.WriteAsync(invalidResponseData, 0, invalidResponseData.Length);
                client.Close();
                return;
            }

            // Autenticar usuario
            User? user = userService.Authenticate(loginRequest.Username, loginRequest.Password);
            LoginResponse loginResponse;

            if (user != null)
            {
                loginResponse = new LoginResponse
                {
                    Success = true,
                    Role = user.Role
                };
            }
            else
            {
                loginResponse = new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Credenciales incorrectas"
                };
            }

            // Enviar respuesta
            string finalJsonResponse = JsonConvert.SerializeObject(loginResponse);
            byte[] finalResponseData = Encoding.UTF8.GetBytes(finalJsonResponse);
            await stream.WriteAsync(finalResponseData, 0, finalResponseData.Length);

            client.Close();
        }



    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty; // Inicializa como cadena vacía
        public string Password { get; set; } = string.Empty; // Inicializa como cadena vacía

        // Constructor
        public LoginRequest(string username, string password)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username), "El nombre de usuario no puede ser nulo.");
            Password = password ?? throw new ArgumentNullException(nameof(password), "La contraseña no puede ser nula.");
        }
    }


    public class LoginResponse
{
    public bool Success { get; set; }
    public string Role { get; set; } = string.Empty; // Inicializa como cadena vacía
    public string ErrorMessage { get; set; } = string.Empty;
}

}
