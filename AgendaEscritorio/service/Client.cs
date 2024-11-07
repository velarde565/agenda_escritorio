using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace AgendaEscritorio.service
{
    public partial class Client
    {
        private TcpClient client;
        private StreamWriter writer;
        private StreamReader reader;
        private const string ServerAddress = "localhost";
        private const int Port = 12522;

        // Campos que almacenan información del usuario
        private string sessionToken;
        private string username;
        private string username2;
        private string role;
        private string fullName;
        private string birthDate;
        private string otherData;
        public char IsAdmin { get; private set; }  // Solo tiene un setter privado

        // Propiedades de solo lectura
        public string SessionToken => sessionToken;  // Propiedad de solo lectura para el token de sesión
        public string Username => username;  // Propiedad de solo lectura para el nombre de usuario
        public string Role => role;  // Propiedad de solo lectura
        public string FullName => fullName;  // Propiedad de solo lectura
        public string BirthDate => birthDate;  // Propiedad de solo lectura
        public string OtherData => otherData;  // Propiedad de solo lectura
        public void SetSessionToken(string token) => sessionToken = token;
        public string GetSessionToken() => sessionToken;


        public async Task ConnectAsync()
        {
            try
            {
                if (client != null && client.Connected)
                {
                    MessageBox.Show("Ya estás conectado al servidor.");
                    return; // Evita volver a conectar si ya está conectado
                }

                client = new TcpClient();
                await client.ConnectAsync(ServerAddress, Port);
                writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                reader = new StreamReader(client.GetStream());
                MessageBox.Show("Connected to server!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to server: {ex.Message}");
            }
        }

        public async Task<bool> SendLoginAsync(string username, string password)
        {
            this.username = username; // Guardar el nombre de usuario
            this.username2 = username;
            try
            {
                string loginPacket = ProtocolHelper.ConstructLoginPacket(username, password);
                Console.WriteLine($"Login Packet Sent: {loginPacket}");

                await writer.WriteLineAsync(loginPacket);
                await writer.FlushAsync();

                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                return await ProcessLoginResponseAsync(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending login: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ProcessLoginResponseAsync(string response)
        {
            var parts = ProtocolHelper.LecturaPartesPaquete(response);
            int action = parts.Item1;
            string message = parts.Item2;

            switch (action)
            {
                case 103:
                    if (message.Length >= 2)
                    {
                        string token = DataExtractor.ExtractToken(message);
                        SetSessionToken(token);
                        MessageBox.Show($"Login successful! Token: {token}");
                        await RequestUserDataAsync(sessionToken, username, username2);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error: Could not extract token.");
                        return false;
                    }
                case 9:
                    MessageBox.Show($"Login error: {message}");
                    return false;
                default:
                    MessageBox.Show($"Unexpected response: {response}");
                    return false;
            }
        }

        public async Task RequestUserDataAsync(string sessionToken, string username, string nombre2)
        {
            try
            {
                string requestPacket = ProtocolHelper.ConstructUserDataRequestPacket(sessionToken, username, nombre2);
                await writer.WriteLineAsync(requestPacket);
                await writer.FlushAsync();

                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");
                ProcessUserDataResponse(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error requesting user data: {ex.Message}");
            }
        }


        private void ProcessUserDataResponse(string response)
        {
            if (response.Length < 3)
            {
                MessageBox.Show("Response too short.");
                return;
            }

            char protocol = response[0];
            int action = int.Parse(response.Substring(1, 2));

            if (protocol != '2' || action != 15)
            {
                MessageBox.Show($"Unexpected protocol or action: {protocol}, {action}");
                return;
            }

            int index = 3;
            sessionToken = DataExtractor.ExtractData(response, ref index);
            username = DataExtractor.ExtractData(response, ref index);
            role = DataExtractor.ExtractData(response, ref index);
            fullName = DataExtractor.ExtractData(response, ref index);
            birthDate = DataExtractor.ExtractData(response, ref index);
            otherData = DataExtractor.ExtractOtherData(response, ref index);
            IsAdmin = response[index]; // Almacenar el valor de administrador

            // Mostrar la información
            MessageBox.Show($"Token: {sessionToken}\nUsername: {username}\nRole: {role}\nFull Name: {fullName}\nBirth Date: {birthDate}\nOther Data: {otherData}\nIs Admin: {IsAdmin}");
        }



       

        public async Task SendLogoutAsync()
        {

            try
            {
                string logoutPacket = ProtocolHelper.ConstructLogoutPacket(sessionToken, username);
                MessageBox.Show($"Logout Packet Sent: {logoutPacket}");

                if (writer == null)
                {
                    MessageBox.Show("Error: writer is not initialized.");
                    return;
                }

                await writer.WriteLineAsync(logoutPacket);
                await writer.FlushAsync();

                string response = await reader.ReadLineAsync();
                MessageBox.Show($"Server Response: {response}");

                ProcessLogoutResponse(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.Message}");
            }
        }

        private void ProcessLogoutResponse(string response)
        {
            var parts = ProtocolHelper.LecturaPartesPaquete(response);
            int action = parts.Item1;
            string message = parts.Item2;

            if (action == 102) // Confirmación de logout
            {
                MessageBox.Show("Logout successful.");
            }
            else
            {
                MessageBox.Show($"Unexpected response: {response}");
            }
        }

        public async Task RequestChangeFullNameAsync(string sessionToken, string usernameToChange, string newFullName)
        {
            try
            {
                // Construir el paquete de solicitud usando ProtocolHelper
                string requestPacket = ProtocolHelper.ConstructChangeFullNamePacket(sessionToken, usernameToChange, newFullName, username);

                // Enviar la solicitud
                await writer.WriteLineAsync(requestPacket);
                await writer.FlushAsync();

                // Leer la respuesta
                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                // Procesar la respuesta
                ProcessChangeFullNameResponse(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al solicitar el cambio de nombre completo: {ex.Message}");
            }
        }


        private void ProcessChangeFullNameResponse(string response)
        {
            MessageBox.Show(response);
            if (response.Length < 3)
            {
                MessageBox.Show("Response too short.");
                return;
            }
            char protocol = response[0];
            int action = int.Parse(response.Substring(1, 2));


            if (protocol != '2' || action != 20)
            {
                MessageBox.Show($"Unexpected protocol or action: {protocol}, {action}");
                return;
            }

            int index = 3;
            string Token = DataExtractor.ExtractData(response, ref index);

            if (Token == SessionToken)
            {
                MessageBox.Show("Confirmacion cambio de nombre");
            }

        }



        public async Task RequestChangeOtherDataAsync(string sessionToken, string usernameToChange, string newOtherData)
        {
            try
            {
                // Construir el paquet de sol·licitud usant ProtocolHelper
                string requestPacket = ProtocolHelper.ConstructChangeOtherDataPacket(sessionToken, usernameToChange, username, newOtherData);

                // Enviar la sol·licitud
                await writer.WriteLineAsync(requestPacket);
                await writer.FlushAsync();

                // Llegir la resposta
                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                // Processar la resposta
                ProcessChangeOtherDataResponse(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al sol·licitar el canvi d'altres dades: {ex.Message}");
            }
        }

        private void ProcessChangeOtherDataResponse(string response)
        {
            if (response == "112")
            {
                MessageBox.Show("Error 112: No tienes permisos para cambiar el nombre de este usuario.");
                return;
            }

            MessageBox.Show("Nombre completo cambiado con éxito.");
        }


        public async Task RequestChangeBirthDateAsync(string sessionToken, string usernameToChange, string newBirthDate, string connectedUsername)
        {
            try
            {
                // Construir el paquete de solicitud usando ProtocolHelper
                string requestPacket = ProtocolHelper.ConstructChangeBirthDatePacket(sessionToken, usernameToChange, newBirthDate, connectedUsername);

                // Enviar la solicitud
                await writer.WriteLineAsync(requestPacket);
                await writer.FlushAsync();

                // Leer la respuesta
                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                // Procesar la respuesta
                ProcessChangeBirthDateResponse(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al solicitar el cambio de fecha de nacimiento: {ex.Message}");
            }
        }

        private void ProcessChangeBirthDateResponse(string response)
        {
            if (response == "112")
            {
                MessageBox.Show("Error 112: No tienes permisos para cambiar la fecha de este usuario.");
                return;
            }
            else if (response == "1105")
            {
                MessageBox.Show("Error 1105: La fecha de nacimiento no puede ser posterior a la fecha actual.");
                return;
            }

            MessageBox.Show("Fecha de nacimiento cambiada con éxito.");
        }


        public async Task RequestChangePasswordAsync(string sessionToken, string usernameToEdit, string currentPassword, string newPassword, string connectedUsername)
        {
            try
            {
                string requestPacket = ProtocolHelper.ConstructChangePasswordPacket(sessionToken, usernameToEdit, currentPassword, newPassword, connectedUsername);

                await writer.WriteLineAsync(requestPacket);
                await writer.FlushAsync();

                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                ProcessChangeFullPassword(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al solicitar el cambio de contraseña: {ex.Message}");
            }
        }



        private void ProcessChangeFullPassword(string response)
        {
            MessageBox.Show(response);
            if (response.Length < 3)
            {
                MessageBox.Show("Response too short.");
                return;
            }
            char protocol = response[0];
            int action = int.Parse(response.Substring(1, 2));

            // Mostrar el valor de protocol y luego el valor de action
            MessageBox.Show($"Protocol: {protocol}");
            MessageBox.Show($"Action: {action}");



            if (protocol != '1' || action != 04)
            {
                MessageBox.Show($"Unexpected protocol or action: {protocol}, {action}");
                return;
            }

            int index = 3;
            string Token = DataExtractor.ExtractData(response, ref index);

            if (Token == SessionToken)
            {
                MessageBox.Show("Confirmacion cambio de contraseña");
            }

        }

        public async Task RequestDeleteUserAsync(string sessionToken, string usernameToDelete, string connectedUsername)
        {
            try
            {
                // Construir el paquete de solicitud de eliminación usando ProtocolHelper
                string requestPacket = ProtocolHelper.ConstructDeleteUserPacket(sessionToken, usernameToDelete, connectedUsername);

                // Enviar la solicitud al servidor
                await writer.WriteLineAsync(requestPacket);
                await writer.FlushAsync();

                // Leer la respuesta del servidor
                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessDeleteUserResponse(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al solicitar la eliminación del usuario: {ex.Message}");
            }
        }

        private void ProcessDeleteUserResponse(string response)
        {
            // Suponiendo que la respuesta del servidor es un código como "224" para eliminación exitosa
            if (response.StartsWith("216"))
            {
                // Confirmación de eliminación de usuario (Protocolo 2, Acción 24)
                MessageBox.Show("El usuario ha sido eliminado exitosamente.");
            }
            else if (response.StartsWith("9")) // Protocolo 9 para errores
            {
                // Extraer el código de error
                string errorCode = response.Substring(1); // Tomar el resto después de "9"

                switch (errorCode)
                {
                    case "112":
                        MessageBox.Show("Error: No autorizado para eliminar el usuario.");
                        break;
                    case "1205":
                        MessageBox.Show("Error: El usuario especificado no existe.");
                        break;
                    default:
                        MessageBox.Show("Error al procesar la solicitud. Detalles: No hay detalles adicionales.");
                        break;
                }
            }
            else
            {
                // Respuesta inesperada del servidor
                MessageBox.Show("Respuesta inesperada del servidor.");
            }
        }

        public async Task RequestCreateUserAsync(string sessionToken,string username, string nombreUsuario, string password, string nombreCompleto, string fechaNacimiento, string otrosDatos, string rolPermisos)
        {
            try
            {
                // Construir el paquete de solicitud de creación usando ProtocolHelper
                string requestPacket = ProtocolHelper.ConstructCreateUserPacket(sessionToken,username, nombreUsuario, password, nombreCompleto, fechaNacimiento, otrosDatos, rolPermisos);

                // Enviar la solicitud al servidor
                await writer.WriteLineAsync(requestPacket);
                await writer.FlushAsync();

                // Leer la respuesta del servidor
                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessCreateUserResponse(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al solicitar la creación del usuario: {ex.Message}");
            }
        }

        private void ProcessCreateUserResponse(string response)
        {
            // Verificar si la respuesta comienza con "223" para confirmación de creación de usuario
            if (response.StartsWith("223"))
            {
                // Confirmación de creación de usuario
                MessageBox.Show("El usuario ha sido creado exitosamente.");
            }
            else if (response.StartsWith("9")) // Protocolo 9 para errores
            {
                // Extraer el código de error
                string errorCode = response.Substring(1); // Tomar el resto después de "9"

                switch (errorCode)
                {
                    case "105":
                        MessageBox.Show("Error: No autorizado para crear el usuario.");
                        break;
                    case "1205":
                        MessageBox.Show("Error: El nombre de usuario ya existe.");
                        break;
                    case "112":
                        MessageBox.Show("Error: No autorizado.");
                        break;
                    default:
                        MessageBox.Show("Error al procesar la solicitud. Detalles: No hay detalles adicionales.");
                        break;
                }
            }
            else
            {
                // Respuesta inesperada del servidor
                MessageBox.Show("Respuesta inesperada del servidor.");
            }
        }









        public void Disconnect()
        {
            try
            {
                if (writer != null)
                {
                    writer.Close();
                    writer = null;
                }
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (client != null)
                {
                    client.Close();
                    client = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during disconnection: {ex.Message}");
            }
        }
    }
}
