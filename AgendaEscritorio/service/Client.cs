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
        private string role;
        private string fullName;
        private string birthDate;
        private string otherData;
        public char IsAdmin { get; private set; }  // Solo tiene un setter privado

        public string Username => username;  // Propiedad de solo lectura
        public string Role => role;  // Propiedad de solo lectura
        public string FullName => fullName;  // Propiedad de solo lectura
        public string BirthDate => birthDate;  // Propiedad de solo lectura
        public string OtherData => otherData;  // Propiedad de solo lectura

        public async Task ConnectAsync()
        {
            try
            {
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
                        await RequestUserDataAsync();
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

        public async Task RequestUserDataAsync()
        {
            try
            {
                string requestPacket = ProtocolHelper.ConstructUserDataRequestPacket(sessionToken, username);
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

        public void SetSessionToken(string token) => sessionToken = token;

        public string GetSessionToken() => sessionToken;

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

        public void Disconnect()
        {
            if (client != null)
            {
                writer.Close();
                reader.Close();
                client.Close();
            }
        }
    }
}
