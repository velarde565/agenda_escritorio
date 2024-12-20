using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AgendaEscritorio.service
{
    /// <summary>
    /// Clase que maneja la conexión cliente-servidor y la gestión de información del usuario.
    /// </summary>
    public partial class Client
    {
        // Conexión TCP para comunicarse con el servidor
        private TcpClient client;  // Objeto TcpClient para gestionar la conexión al servidor
        private StreamWriter writer;  // Escribiente para enviar datos al servidor
        private StreamReader reader;  // Lector para recibir datos del servidor
        private CryptographyService cryptoService;



        // Dirección y puerto del servidor
        private const string ServerAddress = "localhost";  // Dirección del servidor
        private const int Port = 12522;  // Puerto para la conexión

        // Campos que almacenan la información del usuario
        private string sessionToken;  // Token de sesión para autenticar al usuario
        private string username;  // Nombre de usuario
        private string username2;  // Nombre de usuario adicional o secundario
        private string role;  // Rol del usuario (por ejemplo, Administrador, Usuario)
        private string fullName;  // Nombre completo del usuario
        private string birthDate;  // Fecha de nacimiento del usuario
        private string otherData;  // Otros datos adicionales del usuario

        


        /// <summary>
        /// Propiedad que indica si el usuario tiene permisos de administrador.
        /// Solo tiene un setter privado, se asigna internamente.
        /// </summary>
        public char IsAdmin { get; private set; }

        // Propiedades de solo lectura para acceder a la información del usuario
        public string SessionToken => sessionToken;  // Propiedad de solo lectura para el token de sesión
        public string Username => username;  // Propiedad de solo lectura para el nombre de usuario
        public string Role => role;  // Propiedad de solo lectura para el rol del usuario
        public string FullName => fullName;  // Propiedad de solo lectura para el nombre completo
        public string BirthDate => birthDate;  // Propiedad de solo lectura para la fecha de nacimiento
        public string OtherData => otherData;  // Propiedad de solo lectura para otros datos adicionales

        /// <summary>
        /// Establece el token de sesión para el usuario.
        /// </summary>
        /// <param name="token">El token de sesión a establecer.</param>
        public void SetSessionToken(string token) => sessionToken = token;

        /// <summary>
        /// Obtiene el token de sesión actual.
        /// </summary>
        /// <returns>El token de sesión.</returns>
        public string GetSessionToken() => sessionToken;


        /// <summary>
        /// Constructor de la clase <see cref="Client"/>.
        /// Inicializa el servicio de criptografía para ser utilizado en las operaciones relacionadas con la seguridad.
        /// </summary>
        public Client()
        {
            cryptoService = new CryptographyService(); // Inicializa el servicio de criptografía
        }



        /// <summary>
        /// Envía la clave pública del cliente al servidor de forma asincrónica.
        /// Establece la comunicación con el servidor para enviar la clave pública y procesar la respuesta recibida.
        /// </summary>
        /// <returns>Una tarea asincrónica que representa el envío de la clave pública y el procesamiento de la respuesta del servidor.</returns>
        public async Task SendClientPublicKeyAsync()
        {
            try
            {
                if (writer == null)
                {
                    MessageBox.Show("No estás conectado al servidor.");
                    return;
                }

                // Paso 1: Obtener y enviar la clave pública del cliente al servidor.
                string publicKey = cryptoService.GetPublicKey();
                string packet = "301" + publicKey.Length.ToString("D4") + publicKey;

                await writer.WriteLineAsync(packet); // Enviar la clave pública al servidor
                await writer.FlushAsync();

                // Paso 2: Leer la respuesta del servidor tras enviar la clave pública.
                string response = await reader.ReadLineAsync();

                if (response.StartsWith("302"))
                {
                    // Éxito: El servidor ha confirmado recibir la clave pública.
                    MessageBox.Show("Clave pública enviada correctamente. El servidor ha respondido con 302.");

                    // Enviar nuestro propio 302 de vuelta al servidor.
                    await writer.WriteLineAsync("302");
                    await writer.FlushAsync();

                    // Paso 3: Leer la respuesta del servidor después de nuestro 302.
                    string nextResponse = await reader.ReadLineAsync();

                    if (nextResponse.StartsWith("304"))
                    {
                        // Procesar la clave AES encriptada.
                        await ProcessEncryptedAESKeyAsync(nextResponse);
                    }
                    else
                    {
                        MessageBox.Show($"Respuesta inesperada del servidor: {nextResponse}");
                    }
                }
                else if (response.StartsWith("303"))
                {
                    // Error: El servidor no pudo procesar la clave pública.
                    MessageBox.Show("Error al enviar la clave pública. El servidor ha respondido con 303.");
                }
                else
                {
                    // Respuesta inesperada del servidor.
                    MessageBox.Show($"Respuesta inesperada del servidor: {response}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar la clave pública: {ex.Message}");
            }
        }




        /// <summary>
        /// Procesa la clave AES encriptada recibida en la respuesta.
        /// Extrae la longitud de la clave, la desencripta usando la clave privada del cliente,
        /// y luego la almacena en el servicio de criptografía.
        /// </summary>
        /// <param name="response">La respuesta que contiene la clave AES encriptada en formato Base64.</param>
        /// <returns>Una tarea asincrónica que representa el proceso de desencriptación y almacenamiento de la clave AES.</returns>
        private async Task ProcessEncryptedAESKeyAsync(string response)
        {
            try
            {
                // Extraer los 4 bytes del offset (longitud de la clave AES encriptada).
                int keyLength = int.Parse(response.Substring(3, 4)); // Offset desde el cuarto carácter.

                // Extraer la clave AES encriptada.
                string encryptedAESKeyBase64 = response.Substring(7, keyLength); // Desde el índice 7.
                byte[] encryptedAESKey = Convert.FromBase64String(encryptedAESKeyBase64); // Convertir el Base64 a bytes.

                // Desencriptar la clave AES usando la clave privada del cliente.
                byte[] decryptedAESKey = cryptoService.DecryptDataWithPrivateKey(encryptedAESKey);

                // Delegar el almacenamiento de la clave AES al CryptographyService
                cryptoService.SetAESKey(decryptedAESKey);

                MessageBox.Show("Clave AES procesada y almacenada correctamente en CryptographyService.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la clave AES encriptada: {ex.Message}");
            }
        }










        /// <summary>
        /// Establece una conexión asincrónica con el servidor.
        /// Si ya está conectado, no intenta reconectar.
        /// </summary>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        public async Task ConnectAsync()
        {
            try
            {
                // Verifica si ya hay una conexión activa
                if (client != null && client.Connected)
                {
                    MessageBox.Show("Ya estás conectado al servidor.");
                    return; // Evita volver a conectar si ya está conectado
                }

                // Crea una nueva conexión al servidor
                client = new TcpClient();
                await client.ConnectAsync(ServerAddress, Port);  // Conecta al servidor en la dirección y puerto definidos

                // Configura los objetos StreamWriter y StreamReader para gestionar la comunicación
                writer = new StreamWriter(client.GetStream()) { AutoFlush = true };  // StreamWriter para enviar datos
                reader = new StreamReader(client.GetStream());  // StreamReader para recibir datos

                MessageBox.Show("¡Conectado al servidor!");  // Mensaje de confirmación
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción y muestra un mensaje de error
                MessageBox.Show($"Error al conectar con el servidor: {ex.Message}");
            }
        }




        /// <summary>
        /// Asynchronously sends a custom packet to the server.
        /// </summary>
        /// <param name="customPacket">The custom packet to send.</param>
        /// <returns>Returns true if the packet was sent successfully, false otherwise.</returns>
        public async Task<bool> SendCustomPacketAsync(string customPacket)
        {
            try
            {
                // Validar que el paquete no esté vacío
                if (string.IsNullOrWhiteSpace(customPacket))
                {
                    MessageBox.Show("El paquete no puede estar vacío. Por favor, introduce un paquete válido.");
                    return false;
                }

                // Agregar un carácter de fin (`\n`) si no está presente
                if (!customPacket.EndsWith("\n"))
                {
                    customPacket += "\n";
                }

                // Convertir el paquete a un arreglo de bytes
                byte[] packetBytes = Encoding.UTF8.GetBytes(customPacket);

                // Verificar que la clave AES ya esté inicializada
                if (cryptoService.GetAESKey() == null)
                {
                    MessageBox.Show("Clave AES no disponible. Asegúrese de haber recibido y desencriptado la clave del servidor.");
                    return false;
                }

                // Encriptar el paquete con AES
                byte[] encryptedPacket = cryptoService.EncryptDataWithAES(packetBytes);
                string encryptedPacketBase64 = Convert.ToBase64String(encryptedPacket);
                MessageBox.Show($"Encrypted Custom Packet: {encryptedPacketBase64}");

                // Enviar el paquete encriptado al servidor
                await writer.WriteLineAsync(encryptedPacketBase64);
                await writer.FlushAsync();

                // Esperar la respuesta del servidor
                string response = await reader.ReadLineAsync();
                if (response == null)
                {
                    MessageBox.Show("El servidor no envió ninguna respuesta.");
                    return false;
                }

                MessageBox.Show($"Server Response (Encrypted): {response}");

                // Convertir la respuesta en base64 a un arreglo de bytes
                byte[] encryptedResponse = Convert.FromBase64String(response);

                // Desencriptar la respuesta
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir los bytes desencriptados a un string
                string decryptedResponseString = Encoding.UTF8.GetString(decryptedResponse);
                MessageBox.Show($"Server Response (Decrypted): {decryptedResponseString}");

                // Aquí podrías procesar la respuesta si es necesario
                return true;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra durante el envío del paquete
                MessageBox.Show($"Error sending custom packet: {ex.Message}");
                return false;
            }
        }




        /// <summary>
        /// Asynchronously sends a login request to the server with the provided username and password.
        /// </summary>
        /// <param name="username">The username for login.</param>
        /// <param name="password">The password for login.</param>
        /// <returns>Returns true if the login was successful, false otherwise.</returns>
        public async Task<bool> SendLoginAsync(string username, string password)
        {
            this.username = username; // Guardar el nombre de usuario para usarlo después
            this.username2 = username; // Asignar el nombre de usuario a una segunda variable (posiblemente para respaldo o registro)

            try
            {
                // Construir el paquete de login
                string loginPacket = ProtocolHelper.ConstructLoginPacket(username, password);
                MessageBox.Show($"Login Packet (Original): {loginPacket}");

                // Convertir el paquete de login a un arreglo de bytes
                byte[] loginPacketBytes = Encoding.UTF8.GetBytes(loginPacket);

                // Verificar que la clave AES ya esté inicializada
                if (cryptoService.GetAESKey() == null)
                {
                    MessageBox.Show("Clave AES no disponible. Asegúrese de haber recibido y desencriptado la clave del servidor.");
                    return false;
                }

                // Encriptar el paquete de login con AES
                byte[] encryptedLoginPacket = cryptoService.EncryptDataWithAES(loginPacketBytes);
                string encryptedLoginPacketBase64 = Convert.ToBase64String(encryptedLoginPacket);
                MessageBox.Show($"Encrypted Login Packet: {encryptedLoginPacketBase64}");

                // Enviar el paquete encriptado al servidor
                await writer.WriteLineAsync(encryptedLoginPacketBase64);
                await writer.FlushAsync();

                // Esperar la respuesta del servidor
                string response = await reader.ReadLineAsync();
                MessageBox.Show($"Server Response (Encrypted): {response}");

                // Convertir la respuesta en base64 a un arreglo de bytes
                byte[] encryptedResponse = Convert.FromBase64String(response);

                // Desencriptar la respuesta
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir los bytes desencriptados a un string
                string decryptedResponseString = Encoding.UTF8.GetString(decryptedResponse);
                MessageBox.Show($"Server Response (Decrypted): {decryptedResponseString}");

                // Procesar la respuesta desencriptada del login
                return await ProcessLoginResponseAsync(decryptedResponseString);
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra durante el proceso de login
                MessageBox.Show($"Error sending login: {ex.Message}");
                return false; // Retornar false si ocurre un error
            }
        }









        /// <summary>
        /// Processes the login response received from the server.
        /// </summary>
        /// <param name="response">The response message from the server.</param>
        /// <returns>Returns true if login is successful, false otherwise.</returns>
        private async Task<bool> ProcessLoginResponseAsync(string response)
        {
            // Split the response into its components using a helper method
            var parts = ProtocolHelper.LecturaPartesPaquete(response);
            int action = parts.Item1; // Extract the action code from the response
            string message = parts.Item2; // Extract the message from the response

            switch (action)
            {
                case 103: // Successful login
                    if (message.Length >= 2)
                    {
                        string token = DataExtractor.ExtractToken(message); // Extract the token from the message
                        SetSessionToken(token); // Store the session token
                        MessageBox.Show($"Login successful! Token: {token}");

                        // Request user data after successful login
                        await RequestUserDataAsync(sessionToken, username, username2);
                        return true; // Return true to indicate successful login
                    }
                    else
                    {
                        MessageBox.Show("Error: Could not extract token.");
                        return false;
                    }

                case 107: // Login successful but server is insecure
                    if (message.Length >= 2)
                    {
                        string token = DataExtractor.ExtractToken(message); // Extract the token from the message
                        SetSessionToken(token); // Store the session token
                        MessageBox.Show($"Login successful, but the server is not secure. You must change the password. Token: {token}");
                        return true; // Return true to indicate successful login with restrictions
                    }
                    else
                    {
                        MessageBox.Show("Error: Could not extract token.");
                        return false;
                    }

                case 9: // Login error
                    MessageBox.Show($"Login error: {message}");
                    return false;

                default: // Unexpected response
                    MessageBox.Show($"Unexpected response: {response}");
                    return false;
            }
        }


        /// <summary>
        /// Asynchronously sends a help request to the server.
        /// </summary>
        /// <returns>Returns true if the request was successful, false otherwise.</returns>
        public async Task<bool> SendHelpRequestAsync()
        {
            try
            {
                // Mensaje de ayuda que será enviado (en este caso "104")
                string helpMessage = "104";
                MessageBox.Show($"Help Request (Original): {helpMessage}");

                // Convertir el mensaje a un arreglo de bytes
                byte[] helpMessageBytes = Encoding.UTF8.GetBytes(helpMessage);

                // Verificar que la clave AES ya esté inicializada
                if (cryptoService.GetAESKey() == null)
                {
                    MessageBox.Show("Clave AES no disponible. Asegúrese de haber recibido y desencriptado la clave del servidor.");
                    return false;
                }

                // Encriptar el mensaje con AES
                byte[] encryptedHelpMessage = cryptoService.EncryptDataWithAES(helpMessageBytes);
                string encryptedHelpMessageBase64 = Convert.ToBase64String(encryptedHelpMessage);
                MessageBox.Show($"Encrypted Help Request: {encryptedHelpMessageBase64}");

                // Enviar el mensaje encriptado al servidor
                await writer.WriteLineAsync(encryptedHelpMessageBase64);
                await writer.FlushAsync();

                // Esperar la respuesta del servidor
                string response = await reader.ReadLineAsync();
                MessageBox.Show($"Server Response (Encrypted): {response}");

                // Convertir la respuesta en base64 a un arreglo de bytes
                byte[] encryptedResponse = Convert.FromBase64String(response);

                // Desencriptar la respuesta
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir los bytes desencriptados a un string
                string decryptedResponseString = Encoding.UTF8.GetString(decryptedResponse);
                MessageBox.Show($"Server Response (Decrypted): {decryptedResponseString}");

                // Procesar la respuesta (puedes añadir lógica adicional aquí si es necesario)
                return true;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra durante el envío
                MessageBox.Show($"Error sending help request: {ex.Message}");
                return false;
            }
        }






        /// <summary>
        /// Asynchronously requests the user data from the server using the session token and username.
        /// The request and response are encrypted using AES.
        /// </summary>
        /// <param name="sessionToken">The session token to authenticate the request.</param>
        /// <param name="username">The username for which to request data.</param>
        /// <param name="nombre2">A secondary username (used for additional processing or validation).</param>
        public async Task RequestUserDataAsync(string sessionToken, string username, string nombre2)
        {
            try
            {
                // Construir el paquete de solicitud de datos del usuario
                string requestPacket = ProtocolHelper.ConstructUserDataRequestPacket(sessionToken, username, nombre2);
                MessageBox.Show($"User Data Request Packet (Original): {requestPacket}");

                // Convertir el paquete de solicitud a un arreglo de bytes
                byte[] requestPacketBytes = Encoding.UTF8.GetBytes(requestPacket);

                // Verificar que la clave AES ya esté inicializada
                if (cryptoService.GetAESKey() == null)
                {
                    MessageBox.Show("Clave AES no disponible. Asegúrese de haber recibido y desencriptado la clave del servidor.");
                    return;
                }

                // Encriptar el paquete de solicitud con AES
                byte[] encryptedRequestPacket = cryptoService.EncryptDataWithAES(requestPacketBytes);
                string encryptedRequestPacketBase64 = Convert.ToBase64String(encryptedRequestPacket);
                MessageBox.Show($"Encrypted User Data Request Packet: {encryptedRequestPacketBase64}");

                // Enviar el paquete encriptado al servidor
                await writer.WriteLineAsync(encryptedRequestPacketBase64);
                await writer.FlushAsync();

                // Esperar la respuesta del servidor (en formato Base64 y encriptada con AES)
                string encryptedResponse = await reader.ReadLineAsync();
                MessageBox.Show($"Server Response (Encrypted): {encryptedResponse}");

                // Convertir la respuesta del servidor (Base64) a un arreglo de bytes
                byte[] encryptedResponseBytes = Convert.FromBase64String(encryptedResponse);

                // Desencriptar la respuesta con AES
                byte[] decryptedResponseBytes = cryptoService.DecryptDataWithAES(encryptedResponseBytes);

                // Convertir los bytes desencriptados a un string
                string decryptedResponse = Encoding.UTF8.GetString(decryptedResponseBytes);
                MessageBox.Show($"Server Response (Decrypted): {decryptedResponse}");

                // Procesar la respuesta desencriptada
                ProcessUserDataResponse(decryptedResponse);
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra durante el proceso de solicitud de datos
                MessageBox.Show($"Error requesting user data: {ex.Message}");
            }
        }



        /// <summary>
        /// Processes the user data response received from the server.
        /// </summary>
        /// <param name="response">The response message containing user data.</param>
        private void ProcessUserDataResponse(string response)
        {
            // Check if the response is too short to contain valid data
            if (response.Length < 3)
            {
                MessageBox.Show("Response too short.");
                return;
            }

            // Extract the protocol and action from the response
            char protocol = response[0];
            int action = int.Parse(response.Substring(1, 2));

            // Verify that the protocol and action match the expected values
            if (protocol != '2' || action != 15)
            {
                MessageBox.Show($"Unexpected protocol or action: {protocol}, {action}");
                return;
            }

            // Extract user data starting from index 3
            int index = 3;
            sessionToken = DataExtractor.ExtractData(response, ref index);
            username = DataExtractor.ExtractData(response, ref index);
            role = DataExtractor.ExtractData(response, ref index);
            fullName = DataExtractor.ExtractData(response, ref index);
            birthDate = DataExtractor.ExtractData(response, ref index);
            otherData = DataExtractor.ExtractOtherData(response, ref index);
            IsAdmin = response[index]; // Store the admin status value

            // Show the extracted information in a message box
            MessageBox.Show($"Token: {sessionToken}\nUsername: {username}\nRole: {role}\nFull Name: {fullName}\nBirth Date: {birthDate}\nOther Data: {otherData}\nIs Admin: {IsAdmin}");
        }

        /// <summary>
        /// Sends a logout request to the server with AES encryption.
        /// </summary>
        public async Task SendLogoutAsync()
        {
            try
            {
                // Construct the logout packet using the session token and username
                string logoutPacket = ProtocolHelper.ConstructLogoutPacket(sessionToken, username);
                MessageBox.Show($"Logout Packet Before Encryption: {logoutPacket}");

                // Check if the writer is initialized before sending the packet
                if (writer == null)
                {
                    MessageBox.Show("Error: writer is not initialized.");
                    return;
                }

                // Encrypt the logout packet with AES
                byte[] encryptedLogoutPacket = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(logoutPacket));

                // Convert the encrypted packet to Base64 for transmission
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedLogoutPacket));
                await writer.FlushAsync();

                // Wait for the server's encrypted response
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                MessageBox.Show($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decode the Base64 response and decrypt it
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convert the decrypted response back to a string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                MessageBox.Show($"Decrypted Server Response: {response}");

                // Process the logout response
                ProcessLogoutResponse(response);
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the logout process
                MessageBox.Show($"Error during logout: {ex.Message}");
            }
        }


        /// <summary>
        /// Processes the logout response received from the server.
        /// </summary>
        /// <param name="response">The response message from the server after logout request.</param>
        private void ProcessLogoutResponse(string response)
        {
            // Split the response into action and message using a helper function
            var parts = ProtocolHelper.LecturaPartesPaquete(response);
            int action = parts.Item1;
            string message = parts.Item2;

            // Check if the action is 102, indicating a successful logout
            if (action == 102) // Logout confirmation
            {
                MessageBox.Show("Logout successful.");
            }
            else
            {
                // If the action is unexpected, show an error message
                MessageBox.Show($"Unexpected response: {response}");
            }
        }


        /// <summary>
        /// Envía una solicitud de apagado al servidor de forma asincrónica.
        /// Construye un paquete de apagado utilizando el nombre de usuario, la contraseña y el token de sesión,
        /// luego encripta el paquete y lo envía al servidor. Después procesa la respuesta encriptada recibida del servidor.
        /// </summary>
        /// <param name="username">El nombre de usuario que solicita el apagado del servidor.</param>
        /// <param name="password">La contraseña asociada al nombre de usuario para la autenticación.</param>
        /// <param name="sessionToken">El token de sesión válido para autenticar la solicitud.</param>
        /// <returns>Una tarea asincrónica que representa el envío de la solicitud de apagado y el procesamiento de la respuesta del servidor.</returns>
        public async Task SendShutdownRequestAsync(string username, string password, string sessionToken)
        {
            try
            {
                // Construir el paquete de apagado del servidor utilizando el token de sesión, nombre de usuario y la contraseña
                string shutdownPacket = ProtocolHelper.ConstructShutdownPacket(sessionToken, username, password);
                MessageBox.Show($"Shutdown Packet Before Encryption: {shutdownPacket}");

                // Verificar que el writer está inicializado
                if (writer == null)
                {
                    MessageBox.Show("Error: writer no está inicializado.");
                    return;
                }

                // Encriptar el paquete de apagado con AES
                byte[] encryptedShutdownPacket = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(shutdownPacket));

                // Convertir el paquete encriptado a Base64 para transmitirlo
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedShutdownPacket));
                await writer.FlushAsync();

                // Esperar la respuesta del servidor (encriptada)
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                MessageBox.Show($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar y desencriptar la respuesta
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada a string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                MessageBox.Show($"Decrypted Server Response: {response}");

            }
            catch (Exception ex)
            {
                // Manejar cualquier error
                MessageBox.Show($"Error durante la solicitud de apagado: {ex.Message}");
            }
        }




        /// <summary>
        /// Asynchronously sends a request to change the user's full name on the server. The request and response are encrypted using AES.
        /// </summary>
        /// <param name="sessionToken">The session token for authentication.</param>
        /// <param name="usernameToChange">The username whose full name is to be changed.</param>
        /// <param name="newFullName">The new full name for the user.</param>
        /// <returns>Returns a Task representing the asynchronous operation.</returns>
        public async Task RequestChangeFullNameAsync(string sessionToken, string usernameToChange, string newFullName)
        {
            try
            {
                // Construir el paquete para solicitar el cambio de nombre completo
                string requestPacket = ProtocolHelper.ConstructChangeFullNamePacket(sessionToken, usernameToChange, newFullName, username);
                MessageBox.Show($"Change Full Name Request Packet (Original): {requestPacket}");

                // Convertir el paquete a un arreglo de bytes
                byte[] requestPacketBytes = Encoding.UTF8.GetBytes(requestPacket);

                // Verificar que la clave AES ya esté inicializada
                if (cryptoService.GetAESKey() == null)
                {
                    MessageBox.Show("Clave AES no disponible. Asegúrese de haber recibido y desencriptado la clave del servidor.");
                    return;
                }

                // Encriptar el paquete de solicitud con AES
                byte[] encryptedRequestPacket = cryptoService.EncryptDataWithAES(requestPacketBytes);
                string encryptedRequestPacketBase64 = Convert.ToBase64String(encryptedRequestPacket);
                MessageBox.Show($"Encrypted Change Full Name Request Packet: {encryptedRequestPacketBase64}");

                // Enviar el paquete encriptado al servidor
                await writer.WriteLineAsync(encryptedRequestPacketBase64);
                await writer.FlushAsync();

                // Esperar la respuesta del servidor (en formato Base64 y encriptada con AES)
                string encryptedResponse = await reader.ReadLineAsync();
                MessageBox.Show($"Server Response (Encrypted): {encryptedResponse}");

                // Convertir la respuesta del servidor (Base64) a un arreglo de bytes
                byte[] encryptedResponseBytes = Convert.FromBase64String(encryptedResponse);

                // Desencriptar la respuesta con AES
                byte[] decryptedResponseBytes = cryptoService.DecryptDataWithAES(encryptedResponseBytes);

                // Convertir los bytes desencriptados a un string
                string decryptedResponse = Encoding.UTF8.GetString(decryptedResponseBytes);
                MessageBox.Show($"Server Response (Decrypted): {decryptedResponse}");

                // Procesar la respuesta desencriptada
                ProcessChangeFullNameResponse(decryptedResponse);
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que ocurra durante la solicitud de cambio de nombre completo
                MessageBox.Show($"Error al solicitar el cambio de nombre completo: {ex.Message}");
            }
        }




        /// <summary>
        /// Procesa la respuesta recibida al intentar cambiar el nombre completo de un usuario.
        /// </summary>
        /// <param name="response">La respuesta recibida del servidor, como una cadena de texto.</param>
        private void ProcessChangeFullNameResponse(string response)
        {
            // Muestra la respuesta recibida en un cuadro de mensaje
            MessageBox.Show(response);

            // Verifica que la respuesta tenga al menos 3 caracteres
            if (response.Length < 3)
            {
                MessageBox.Show("Response too short.");
                return; // Si la respuesta es demasiado corta, se sale del método
            }

            // Extrae el protocolo (primer carácter de la respuesta)
            char protocol = response[0];

            // Extrae la acción (los siguientes dos caracteres de la respuesta)
            int action = int.Parse(response.Substring(1, 2));

            // Verifica que el protocolo sea '2' y la acción sea '20' (indicando un cambio de nombre exitoso)
            if (protocol != '2' || action != 20)
            {
                // Muestra un mensaje de error si el protocolo o la acción no coinciden con los esperados
                MessageBox.Show($"Unexpected protocol or action: {protocol}, {action}");
                return; // Si el protocolo o acción no son los esperados, se sale del método
            }

            // Inicializa el índice para extraer el token
            int index = 3;

            // Extrae el token de la respuesta usando un helper de extracción de datos
            string Token = DataExtractor.ExtractData(response, ref index);

            // Verifica si el token extraído coincide con el token de sesión actual
            if (Token == SessionToken)
            {
                // Muestra un mensaje indicando que el cambio de nombre fue confirmado
                MessageBox.Show("Confirmacion cambio de nombre");
            }
        }



        /// <summary>
        /// Solicita la adición de un permiso a un usuario específico. Los datos de la solicitud y respuesta están cifrados con AES.
        /// </summary>
        /// <param name="sessionToken">El token de sesión para validar la solicitud.</param>
        /// <param name="username">El nombre de usuario al que se le va a agregar el permiso.</param>
        /// <param name="roleToAdd">El nombre del rol que se va a agregar al usuario.</param>
        /// <param name="permissions">Los permisos que se van a asignar al rol.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task RequestAddPermissionAsync(string sessionToken, string username, string roleToAdd, string permissions)
        {
            try
            {
                // Construir el paquete de solicitud
                string requestPacket = ProtocolHelper.ConstructAddPermissionPacket(sessionToken, username, roleToAdd, permissions);
                MessageBox.Show($"Add Permission Request Packet (Original): {requestPacket}");

                // Convertir el paquete a bytes
                byte[] requestPacketBytes = Encoding.UTF8.GetBytes(requestPacket);

                // Verificar que la clave AES esté inicializada
                if (cryptoService.GetAESKey() == null)
                {
                    MessageBox.Show("Clave AES no disponible. Asegúrese de haber recibido y desencriptado la clave del servidor.");
                    return;
                }

                // Cifrar el paquete con AES
                byte[] encryptedRequestPacket = cryptoService.EncryptDataWithAES(requestPacketBytes);
                string encryptedRequestPacketBase64 = Convert.ToBase64String(encryptedRequestPacket);
                MessageBox.Show($"Encrypted Add Permission Request Packet: {encryptedRequestPacketBase64}");

                // Enviar el paquete cifrado al servidor
                await writer.WriteLineAsync(encryptedRequestPacketBase64);
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponse = await reader.ReadLineAsync();
                MessageBox.Show($"Server Response (Encrypted): {encryptedResponse}");

                // Convertir la respuesta del servidor (Base64) a bytes
                byte[] encryptedResponseBytes = Convert.FromBase64String(encryptedResponse);

                // Desencriptar la respuesta
                byte[] decryptedResponseBytes = cryptoService.DecryptDataWithAES(encryptedResponseBytes);

                // Convertir los bytes desencriptados a un string
                string decryptedResponse = Encoding.UTF8.GetString(decryptedResponseBytes);
                MessageBox.Show($"Server Response (Decrypted): {decryptedResponse}");

                // Procesar la respuesta desencriptada
                ProcessAddPermissionResponse(decryptedResponse);
            }
            catch (Exception ex)
            {
                // Manejar errores
                MessageBox.Show($"Error al solicitar la adición de permiso: {ex.Message}");
            }
        }

        /// <summary>
        /// Procesa la respuesta del servidor tras la solicitud de adición de permiso.
        /// </summary>
        /// <param name="response">La respuesta desencriptada recibida del servidor.</param>
        private void ProcessAddPermissionResponse(string response)
        {
            try
            {
                // Mostrar la respuesta completa
                MessageBox.Show(response);

                // Validar la longitud mínima de la respuesta
                if (response.Length < 3)
                {
                    MessageBox.Show("Respuesta demasiado corta.");
                    return;
                }

                // Extraer el protocolo y la acción
                char protocol = response[0];
                int action = int.Parse(response.Substring(1, 2));

                // Validar protocolo y acción esperados
                if (protocol != '2' || action != 9)
                {
                    MessageBox.Show($"Protocolo o acción inesperada: {protocol}, {action}");
                    return;
                }

                // Extraer el token desde la respuesta
                int index = 3;
                string token = DataExtractor.ExtractData(response, ref index);

                // Validar el token con el de la sesión
                if (token == SessionToken)
                {
                    MessageBox.Show("Permiso agregado exitosamente.");
                }
                else
                {
                    MessageBox.Show("Token incorrecto o acción no permitida.");
                }
            }
            catch (Exception ex)
            {
                // Manejar errores en el procesamiento
                MessageBox.Show($"Error al procesar la respuesta de adición de permiso: {ex.Message}");
            }
        }

        /// <summary>
        /// Solicita la eliminación de un rol para un usuario de forma asincrónica.
        /// Construye un paquete de solicitud para eliminar el rol, lo encripta con AES y lo envía al servidor.
        /// Luego, procesa la respuesta del servidor para determinar el éxito o fracaso de la operación.
        /// </summary>
        /// <param name="sessionToken">El token de sesión válido para autenticar la solicitud.</param>
        /// <param name="username">El nombre de usuario al que se le va a eliminar el rol.</param>
        /// <param name="rolAEliminar">El nombre del rol que se va a eliminar.</param>
        /// <returns>Una tarea asincrónica que representa la solicitud de eliminación del rol y el procesamiento de la respuesta del servidor.
        /// Devuelve <c>true</c> si el rol fue eliminado correctamente, o <c>false</c> en caso contrario.</returns>
        public async Task<bool> RequestDeleteRoleAsync(string sessionToken, string username, string rolAEliminar)
        {
            try
            {
                // Construir el paquete de solicitud
                string requestPacket = ProtocolHelper.ConstructDeleteRolePacket(sessionToken, username, rolAEliminar);
                MessageBox.Show($"Delete Role Request Packet (Original): {requestPacket}");

                // Convertir el paquete a bytes
                byte[] requestPacketBytes = Encoding.UTF8.GetBytes(requestPacket);

                // Verificar que la clave AES esté inicializada
                if (cryptoService.GetAESKey() == null)
                {
                    MessageBox.Show("Clave AES no disponible. Asegúrese de haber recibido y desencriptado la clave del servidor.");
                    return false;
                }

                // Cifrar el paquete con AES
                byte[] encryptedRequestPacket = cryptoService.EncryptDataWithAES(requestPacketBytes);
                string encryptedRequestPacketBase64 = Convert.ToBase64String(encryptedRequestPacket);
                MessageBox.Show($"Encrypted Delete Role Request Packet: {encryptedRequestPacketBase64}");

                // Enviar el paquete cifrado al servidor
                await writer.WriteLineAsync(encryptedRequestPacketBase64);
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponse = await reader.ReadLineAsync();
                MessageBox.Show($"Server Response (Encrypted): {encryptedResponse}");

                // Convertir la respuesta del servidor (Base64) a bytes
                byte[] encryptedResponseBytes = Convert.FromBase64String(encryptedResponse);

                // Desencriptar la respuesta
                byte[] decryptedResponseBytes = cryptoService.DecryptDataWithAES(encryptedResponseBytes);

                // Convertir los bytes desencriptados a un string
                string decryptedResponse = Encoding.UTF8.GetString(decryptedResponseBytes);
                MessageBox.Show($"Server Response (Decrypted): {decryptedResponse}");

                // Procesar la respuesta desencriptada
                return ProcessDeleteRoleResponse(decryptedResponse);
            }
            catch (Exception ex)
            {
                // Manejar errores
                MessageBox.Show($"Error al solicitar la eliminación del rol: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Procesa la respuesta recibida del servidor después de solicitar la eliminación de un rol.
        /// Valida el protocolo, la acción, y el token recibido, asegurándose de que corresponda con la sesión actual.
        /// </summary>
        /// <param name="response">La respuesta en texto del servidor, que incluye el protocolo, la acción y el token.</param>
        /// <returns>Devuelve <c>true</c> si la eliminación del rol fue exitosa, o <c>false</c> en caso de error o discrepancia en los datos.</returns>
        private bool ProcessDeleteRoleResponse(string response)
        {
            try
            {
                // Mostrar la respuesta completa
                MessageBox.Show(response);

                // Validar la longitud mínima de la respuesta
                if (response.Length < 3)
                {
                    MessageBox.Show("Respuesta demasiado corta.");
                    return false;
                }

                // Extraer el protocolo y la acción
                char protocol = response[0];
                int action = int.Parse(response.Substring(1, 2));

                // Validar protocolo y acción esperados
                if (protocol != '2' || action != 11)
                {
                    MessageBox.Show($"Protocolo o acción inesperada: {protocol}, {action}");
                    return false;
                }

                // Extraer el token desde la respuesta
                int index = 3;
                string token = DataExtractor.ExtractData(response, ref index);

                // Validar el token con el de la sesión
                if (token == SessionToken)
                {
                    MessageBox.Show("Rol eliminado exitosamente.");
                    return true;
                }
                else
                {
                    MessageBox.Show("Token incorrecto o acción no permitida.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Manejar errores en el procesamiento
                MessageBox.Show($"Error al procesar la respuesta de eliminación del rol: {ex.Message}");
                return false;
            }
        }




        /// <summary>
        /// Solicita al servidor los permisos de un usuario específico. Los datos se envían y reciben cifrados con AES.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario conectado.</param>
        /// <param name="username">El nombre de usuario al que se le desean obtener los permisos.</param>
        /// <returns>Un diccionario con los permisos del usuario.</returns>
        public async Task<Dictionary<string, string>> RequestGetPermissionsAsync(string sessionToken, string username)
        {
            try
            {
                // Construir el paquete de solicitud utilizando el protocolo adecuado
                string requestPacket = ProtocolHelper.ConstructGetPermissionsPacket(sessionToken, username);
                MessageBox.Show($"Get Permissions Request Packet (Original): {requestPacket}");

                // Convertir el paquete a bytes
                byte[] requestPacketBytes = Encoding.UTF8.GetBytes(requestPacket);

                // Verificar que la clave AES esté inicializada
                if (cryptoService.GetAESKey() == null)
                {
                    MessageBox.Show("Clave AES no disponible. Asegúrese de haber recibido y desencriptado la clave del servidor.");
                    return null;
                }

                // Cifrar el paquete con AES
                byte[] encryptedRequestPacket = cryptoService.EncryptDataWithAES(requestPacketBytes);
                string encryptedRequestPacketBase64 = Convert.ToBase64String(encryptedRequestPacket);
                MessageBox.Show($"Encrypted Get Permissions Request Packet: {encryptedRequestPacketBase64}");

                // Enviar el paquete cifrado al servidor
                await writer.WriteLineAsync(encryptedRequestPacketBase64);
                await writer.FlushAsync();

                // Lista para almacenar todas las respuestas relevantes del servidor
                List<string> responses = new List<string>();

                bool isReceivingPermissions = false; // Indica si estamos en el rango de permisos
                string encryptedResponse;

                // Leer respuestas del servidor cifradas hasta encontrar acción 43 (fin de la lista)
                while ((encryptedResponse = await reader.ReadLineAsync()) != null)
                {
                    // Convertir la respuesta cifrada del servidor (Base64) a bytes
                    byte[] encryptedResponseBytes = Convert.FromBase64String(encryptedResponse);

                    // Desencriptar la respuesta
                    byte[] decryptedResponseBytes = cryptoService.DecryptDataWithAES(encryptedResponseBytes);

                    // Convertir los bytes desencriptados a un string
                    string decryptedResponse = Encoding.UTF8.GetString(decryptedResponseBytes);
                    MessageBox.Show($"Server Response (Decrypted): {decryptedResponse}");

                    // Extraer protocolo y acción de la respuesta
                    if (decryptedResponse.Length < 3)
                        continue; // Ignorar respuestas inválidas

                    char protocol = decryptedResponse[0];
                    int action = int.Parse(decryptedResponse.Substring(1, 2));

                    // Verificar si es el protocolo esperado
                    if (protocol != '2')
                        continue; // Ignorar respuestas de otros protocolos

                    // Manejar las acciones 42 y 43
                    if (action == 42)
                    {
                        // Inicio de transmisión de permisos
                        isReceivingPermissions = true;
                        continue;
                    }
                    else if (action == 43)
                    {
                        // Fin de transmisión de permisos
                        isReceivingPermissions = false;
                        break; // Salir del bucle
                    }

                    // Almacenar respuestas solo si estamos recibiendo permisos
                    if (isReceivingPermissions)
                    {
                        responses.Add(decryptedResponse);
                    }
                }

                // Mostrar mensaje indicando que todas las respuestas fueron recibidas
                MessageBox.Show("Respuesta del servidor: permisos recibidos correctamente.");

                // Concatenar todas las respuestas en un solo mensaje
                string allResponses = string.Join(Environment.NewLine, responses);

                // Mostrar todas las respuestas recibidas en un MessageBox
                MessageBox.Show($"Paquetes recibidos:\n{allResponses}");

                // Procesar todas las respuestas y devolver un diccionario con los permisos
                return ProcessGetPermissionsResponse(responses);
            }
            catch (Exception ex)
            {
                // En caso de error, lanzar una excepción con un mensaje detallado
                throw new Exception($"Error al solicitar los permisos del rol: {ex.Message}");
            }
        }



        private Dictionary<string, string> rolesAndPermissions = new Dictionary<string, string>();

        /// <summary>
        /// Procesa una lista de respuestas del servidor y extrae los permisos de los roles.
        /// </summary>
        /// <param name="responses">Lista de respuestas del servidor que contienen información de permisos.</param>
        /// <returns>Un diccionario con los roles y sus permisos.</returns>
        private Dictionary<string, string> ProcessGetPermissionsResponse(List<string> responses)
        {
            // Limpiar el diccionario antes de usarlo para almacenar los nuevos datos
            rolesAndPermissions.Clear();

            try
            {
                // Iterar sobre cada respuesta recibida
                foreach (var response in responses)
                {
                    // Verificar que la respuesta tiene un mínimo de longitud suficiente para ser procesada
                    if (response.Length < 3)
                        throw new Exception("Respuesta demasiado corta.");

                    // Extraer el protocolo (primer carácter) y la acción (siguientes dos caracteres)
                    char protocol = response[0];
                    int action = int.Parse(response.Substring(1, 2));

                    // Verificar que el protocolo y la acción son los esperados
                    if (protocol != '2' || action != 17) // Acción 17: Consultar permisos
                        throw new Exception($"Protocolo o acción inesperada: {protocol}, {action}");

                    // Inicializar el índice para comenzar a extraer datos después del protocolo y la acción
                    int index = 3;

                    // Extraer el offset del token y luego el token en sí
                    int tokenOffset = int.Parse(response.Substring(index, 2)); // Leer el offset del token
                    index += 2;
                    string token = response.Substring(index, tokenOffset); // Extraer el token
                    index += tokenOffset;

                    // Verificar si el token coincide con el de la sesión actual
                    if (token != SessionToken)
                        throw new Exception("Token incorrecto o acción no permitida.");

                    // Extraer el nombre del rol
                    int roleNameOffsetLength = char.IsDigit(response[index + 1]) ? 2 : 1; // Verificar si el segundo carácter es un dígito
                    int roleNameOffset = int.Parse(response.Substring(index, roleNameOffsetLength)); // Obtener el offset del nombre del rol
                    index += roleNameOffsetLength;
                    string roleName = response.Substring(index, roleNameOffset); // Extraer el nombre del rol
                    index += roleNameOffset;

                    // Extraer los permisos (longitud fija de 7 caracteres)
                    int permissionsLength = 7;
                    if (response.Length < index + permissionsLength)
                        throw new Exception("La longitud de los permisos es inválida.");
                    string permissions = response.Substring(index, permissionsLength); // Extraer los permisos
                    index += permissionsLength;

                    // Almacenar el nombre del rol y sus permisos en el diccionario
                    if (!rolesAndPermissions.ContainsKey(roleName))
                    {
                        rolesAndPermissions.Add(roleName, permissions);
                    }
                    else
                    {
                        rolesAndPermissions[roleName] = permissions;
                    }
                }

                return rolesAndPermissions;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al procesar la respuesta: {ex.Message}");
            }
        }



        /// <summary>
        /// Envía una solicitud al servidor para editar los permisos de un usuario. Los datos se envían y reciben cifrados con AES.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario conectado.</param>
        /// <param name="username">El nombre de usuario cuyo rol y permisos se van a editar.</param>
        /// <param name="rol">El rol que se asignará al usuario.</param>
        /// <param name="permisos">El arreglo de permisos (booleanos) que se asignarán al rol del usuario.</param>
        /// <returns>Un código de respuesta del servidor (éxito o error).</returns>
        public async Task<int> SendEditPermissionsAsync(string sessionToken, string username, string rol, bool[] permisos)
        {
            try
            {
                // Construir el paquete de permisos utilizando el Helper
                string paquete = ProtocolHelper.ConstructEditPermissionsPacket(sessionToken, username, rol, permisos);
                MessageBox.Show($"Edit Permissions Request Packet (Original): {paquete}");

                // Convertir el paquete a bytes
                byte[] paqueteBytes = Encoding.UTF8.GetBytes(paquete);

                // Verificar que la clave AES esté inicializada
                if (cryptoService.GetAESKey() == null)
                {
                    MessageBox.Show("Clave AES no disponible. Asegúrese de haber recibido y desencriptado la clave del servidor.");
                    return -1;
                }

                // Cifrar el paquete con AES
                byte[] paqueteCifrado = cryptoService.EncryptDataWithAES(paqueteBytes);
                string paqueteCifradoBase64 = Convert.ToBase64String(paqueteCifrado);
                MessageBox.Show($"Encrypted Edit Permissions Packet: {paqueteCifradoBase64}");

                // Enviar el paquete cifrado al servidor de forma asíncrona
                await writer.WriteLineAsync(paqueteCifradoBase64);
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponse = await reader.ReadLineAsync();

                // Convertir la respuesta cifrada del servidor (Base64) a bytes
                byte[] encryptedResponseBytes = Convert.FromBase64String(encryptedResponse);

                // Desencriptar la respuesta
                byte[] decryptedResponseBytes = cryptoService.DecryptDataWithAES(encryptedResponseBytes);

                // Convertir los bytes desencriptados a un string
                string decryptedResponse = Encoding.UTF8.GetString(decryptedResponseBytes);
                MessageBox.Show($"Server Response (Decrypted): {decryptedResponse}");

                // Verificar si la respuesta del servidor es un código numérico válido
                if (int.TryParse(decryptedResponse, out int responseCode))
                {
                    return responseCode;  // Retorna el código de respuesta obtenido del servidor
                }
                else
                {
                    // Si la respuesta no es válida (no se puede convertir a entero)
                    MessageBox.Show("Error: Invalid server response.");
                    return -1;  // Indica error en la respuesta del servidor
                }
            }
            catch (Exception ex)
            {
                // Si ocurre algún error durante el envío o procesamiento
                MessageBox.Show($"Error sending permissions: {ex.Message}");
                return -1;  // Error en el envío de la solicitud
            }
        }




        /// <summary>
        /// Envía una solicitud al servidor para activar o desactivar el modo gestión. Los datos se envían y reciben cifrados con AES.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario conectado.</param>
        /// <param name="username">El nombre del usuario que activa o desactiva el modo gestión.</param>
        /// <param name="activarModoGestion">Indica si se debe activar o desactivar el modo gestión (true = activar, false = desactivar).</param>
        /// <returns>Un código de respuesta del servidor (éxito o error).</returns>
        public async Task<int> SendModoGestionAsync(string sessionToken, string username)
        {
            try
            {
                // Construir el paquete de solicitud utilizando el Helper
                string paquete = ProtocolHelper.ConstructModoGestionPacket(sessionToken, username);
                MessageBox.Show($"Modo Gestion Request Packet (Original): {paquete}");

                // Convertir el paquete a bytes
                byte[] paqueteBytes = Encoding.UTF8.GetBytes(paquete);

                // Verificar que la clave AES esté inicializada
                if (cryptoService.GetAESKey() == null)
                {
                    MessageBox.Show("Clave AES no disponible. Asegúrese de haber recibido y desencriptado la clave del servidor.");
                    return -1;
                }

                // Cifrar el paquete con AES
                byte[] paqueteCifrado = cryptoService.EncryptDataWithAES(paqueteBytes);
                string paqueteCifradoBase64 = Convert.ToBase64String(paqueteCifrado);
                MessageBox.Show($"Encrypted Modo Gestion Packet: {paqueteCifradoBase64}");

                // Enviar el paquete cifrado al servidor de forma asíncrona
                await writer.WriteLineAsync(paqueteCifradoBase64);
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponse = await reader.ReadLineAsync();

                // Convertir la respuesta cifrada del servidor (Base64) a bytes
                byte[] encryptedResponseBytes = Convert.FromBase64String(encryptedResponse);

                // Desencriptar la respuesta
                byte[] decryptedResponseBytes = cryptoService.DecryptDataWithAES(encryptedResponseBytes);

                // Convertir los bytes desencriptados a un string
                string decryptedResponse = Encoding.UTF8.GetString(decryptedResponseBytes);
                MessageBox.Show($"Server Response (Decrypted): {decryptedResponse}");

                // Verificar si la respuesta del servidor es un código numérico válido
                if (int.TryParse(decryptedResponse, out int responseCode))
                {
                    return responseCode;  // Retorna el código de respuesta obtenido del servidor
                }
                else
                {
                    // Si la respuesta no es válida (no se puede convertir a entero)
                    MessageBox.Show("Error: Invalid server response.");
                    return -1;  // Indica error en la respuesta del servidor
                }
            }
            catch (Exception ex)
            {
                // Si ocurre algún error durante el envío o procesamiento
                MessageBox.Show($"Error sending mode gestion request: {ex.Message}");
                return -1;  // Error en el envío de la solicitud
            }
        }


        /// <summary>
        /// Envía una solicitud asincrónica al servidor para cambiar otros datos de un usuario específico.
        /// Los datos se envían y reciben cifrados con AES.
        /// </summary>
        /// <param name="sessionToken">El token de sesión que identifica al usuario conectado.</param>
        /// <param name="usernameToChange">El nombre de usuario cuyo "otros datos" serán modificados.</param>
        /// <param name="newOtherData">Los nuevos datos que se desean asignar al usuario.</param>
        public async Task RequestChangeOtherDataAsync(string sessionToken, string usernameToChange, string newOtherData)
        {
            try
            {
                // Construir el paquete de solicitud utilizando el helper
                string requestPacket = ProtocolHelper.ConstructChangeOtherDataPacket(sessionToken, usernameToChange, username, newOtherData);
                MessageBox.Show($"Change Other Data Request Packet (Original): {requestPacket}");

                // Convertir el paquete a bytes
                byte[] requestPacketBytes = Encoding.UTF8.GetBytes(requestPacket);

                // Verificar que la clave AES esté inicializada
                if (cryptoService.GetAESKey() == null)
                {
                    MessageBox.Show("Clave AES no disponible. Asegúrese de haber recibido y desencriptado la clave del servidor.");
                    return;
                }

                // Cifrar el paquete con AES
                byte[] encryptedRequestPacket = cryptoService.EncryptDataWithAES(requestPacketBytes);
                string encryptedRequestPacketBase64 = Convert.ToBase64String(encryptedRequestPacket);
                MessageBox.Show($"Encrypted Change Other Data Packet: {encryptedRequestPacketBase64}");

                // Enviar el paquete cifrado al servidor de forma asíncrona
                await writer.WriteLineAsync(encryptedRequestPacketBase64);
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponse = await reader.ReadLineAsync();

                // Convertir la respuesta cifrada del servidor (Base64) a bytes
                byte[] encryptedResponseBytes = Convert.FromBase64String(encryptedResponse);

                // Desencriptar la respuesta
                byte[] decryptedResponseBytes = cryptoService.DecryptDataWithAES(encryptedResponseBytes);

                // Convertir los bytes desencriptados a un string
                string decryptedResponse = Encoding.UTF8.GetString(decryptedResponseBytes);
                MessageBox.Show($"Server Response (Decrypted): {decryptedResponse}");

                // Procesar la respuesta
                ProcessChangeOtherDataResponse(decryptedResponse);
            }
            catch (Exception ex)
            {
                // Si ocurre algún error durante el envío o procesamiento
                MessageBox.Show($"Error al solicitar el cambio de otros datos: {ex.Message}");
            }
        }



        /// <summary>
        /// Procesa la respuesta del servidor después de solicitar el cambio de otros datos de un usuario.
        /// </summary>
        /// <param name="response">La respuesta recibida del servidor que indica el resultado de la solicitud.</param>
        private void ProcessChangeOtherDataResponse(string response)
        {
            // Verificar si la respuesta es un error específico (Error 112)
            if (response == "112")
            {
                // Mostrar mensaje si el error indica falta de permisos
                MessageBox.Show("Error 112: No tienes permisos para cambiar el nombre de este usuario.");
                return;
            }

            // Si no es un error, mostrar mensaje de éxito
            MessageBox.Show("Nombre completo cambiado con éxito.");
        }



        /// <summary>
        /// Solicita el cambio de la fecha de nacimiento de un usuario al servidor.
        /// Los datos se envían y reciben cifrados con AES.
        /// </summary>
        /// <param name="sessionToken">El token de sesión actual para validar la solicitud.</param>
        /// <param name="usernameToChange">El nombre de usuario al que se le cambiará la fecha de nacimiento.</param>
        /// <param name="newBirthDate">La nueva fecha de nacimiento que se desea asignar.</param>
        /// <param name="connectedUsername">El nombre de usuario conectado que realiza la solicitud, usado para validar permisos.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        public async Task RequestChangeBirthDateAsync(string sessionToken, string usernameToChange, string newBirthDate, string connectedUsername)
        {
            try
            {
                // Construir el paquete de solicitud utilizando el helper
                string requestPacket = ProtocolHelper.ConstructChangeBirthDatePacket(sessionToken, usernameToChange, newBirthDate, connectedUsername);
                MessageBox.Show($"Change Birth Date Request Packet (Original): {requestPacket}");

                // Convertir el paquete a bytes
                byte[] requestPacketBytes = Encoding.UTF8.GetBytes(requestPacket);

                // Verificar que la clave AES esté inicializada
                if (cryptoService.GetAESKey() == null)
                {
                    MessageBox.Show("Clave AES no disponible. Asegúrese de haber recibido y desencriptado la clave del servidor.");
                    return;
                }

                // Cifrar el paquete con AES
                byte[] encryptedRequestPacket = cryptoService.EncryptDataWithAES(requestPacketBytes);
                string encryptedRequestPacketBase64 = Convert.ToBase64String(encryptedRequestPacket);
                MessageBox.Show($"Encrypted Change Birth Date Packet: {encryptedRequestPacketBase64}");

                // Enviar el paquete cifrado al servidor de forma asíncrona
                await writer.WriteLineAsync(encryptedRequestPacketBase64);
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponse = await reader.ReadLineAsync();

                // Convertir la respuesta cifrada del servidor (Base64) a bytes
                byte[] encryptedResponseBytes = Convert.FromBase64String(encryptedResponse);

                // Desencriptar la respuesta
                byte[] decryptedResponseBytes = cryptoService.DecryptDataWithAES(encryptedResponseBytes);

                // Convertir los bytes desencriptados a un string
                string decryptedResponse = Encoding.UTF8.GetString(decryptedResponseBytes);
                MessageBox.Show($"Server Response (Decrypted): {decryptedResponse}");

                // Procesar la respuesta
                ProcessChangeBirthDateResponse(decryptedResponse);
            }
            catch (Exception ex)
            {
                // Si ocurre algún error durante el envío o procesamiento
                MessageBox.Show($"Error al solicitar el cambio de fecha de nacimiento: {ex.Message}");
            }
        }




        /// <summary>
        /// Procesa la respuesta del servidor después de solicitar un cambio de fecha de nacimiento.
        /// </summary>
        /// <param name="response">La respuesta del servidor que contiene el código de resultado.</param>
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



        /// <summary>
        /// Solicita el cambio de contraseña de un usuario específico con cifrado AES.
        /// </summary>
        /// <param name="sessionToken">El token de sesión que autentica la solicitud.</param>
        /// <param name="usernameToEdit">El nombre de usuario cuya contraseña se desea cambiar.</param>
        /// <param name="currentPassword">La contraseña actual del usuario.</param>
        /// <param name="newPassword">La nueva contraseña para el usuario.</param>
        /// <param name="connectedUsername">El nombre de usuario conectado que está realizando la solicitud (debe ser un administrador o el propio usuario).</param>
        public async Task RequestChangePasswordAsync(string sessionToken, string usernameToEdit, string currentPassword, string newPassword, string connectedUsername)
        {
            try
            {
                // Construir el paquete de solicitud utilizando el helper
                string requestPacket = ProtocolHelper.ConstructChangePasswordPacket(sessionToken, usernameToEdit, currentPassword, newPassword, connectedUsername);
                MessageBox.Show($"Password Change Request Packet (Original): {requestPacket}");

                // Convertir el paquete a bytes
                byte[] requestPacketBytes = Encoding.UTF8.GetBytes(requestPacket);

                // Verificar que la clave AES esté inicializada
                if (cryptoService.GetAESKey() == null)
                {
                    MessageBox.Show("Clave AES no disponible. Asegúrese de haber recibido y desencriptado la clave del servidor.");
                    return;
                }

                // Cifrar el paquete con AES
                byte[] encryptedRequestPacket = cryptoService.EncryptDataWithAES(requestPacketBytes);
                string encryptedRequestPacketBase64 = Convert.ToBase64String(encryptedRequestPacket);
                MessageBox.Show($"Encrypted Password Change Request Packet: {encryptedRequestPacketBase64}");

                // Enviar el paquete cifrado al servidor de forma asíncrona
                await writer.WriteLineAsync(encryptedRequestPacketBase64);
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponse = await reader.ReadLineAsync();

                // Convertir la respuesta cifrada del servidor (Base64) a bytes
                byte[] encryptedResponseBytes = Convert.FromBase64String(encryptedResponse);

                // Desencriptar la respuesta
                byte[] decryptedResponseBytes = cryptoService.DecryptDataWithAES(encryptedResponseBytes);

                // Convertir los bytes desencriptados a un string
                string decryptedResponse = Encoding.UTF8.GetString(decryptedResponseBytes);
                MessageBox.Show($"Server Response (Decrypted): {decryptedResponse}");

                // Procesar la respuesta
                ProcessChangeFullPassword(decryptedResponse);
            }
            catch (Exception ex)
            {
                // Si ocurre algún error durante el envío o procesamiento
                MessageBox.Show($"Error al solicitar el cambio de contraseña: {ex.Message}");
            }
        }

        /// <summary>
        /// Procesa la respuesta del servidor después de una solicitud de cambio de contraseña.
        /// </summary>
        /// <param name="response">La respuesta recibida del servidor, que contiene información sobre el resultado del cambio de contraseña.</param>
        private void ProcessChangeFullPassword(string response)
        {
            MessageBox.Show(response);

            // Verificar si la longitud de la respuesta es válida
            if (response.Length < 3)
            {
                MessageBox.Show("Response too short.");
                return;
            }

            // Extraer protocolo y acción de la respuesta
            char protocol = response[0];
            int action = int.Parse(response.Substring(1, 2));

            // Mostrar el valor de protocol y luego el valor de action
            MessageBox.Show($"Protocol: {protocol}");
            MessageBox.Show($"Action: {action}");

            // Verificar que el protocolo y la acción sean los esperados
            if (protocol != '1' || action != 04)
            {
                MessageBox.Show($"Unexpected protocol or action: {protocol}, {action}");
                return;
            }

            // Extraer el token de la respuesta
            int index = 3;
            string Token = DataExtractor.ExtractData(response, ref index);

            // Verificar si el token coincide con el token de sesión actual
            if (Token == SessionToken)
            {
                MessageBox.Show("Confirmación de cambio de contraseña");
            }
        }


        /// <summary>
        /// Solicita la eliminación de un usuario en el servidor.
        /// </summary>
        /// <param name="sessionToken">El token de sesión actual del usuario conectado, usado para autenticar la solicitud.</param>
        /// <param name="usernameToDelete">El nombre de usuario del usuario que se desea eliminar.</param>
        /// <param name="connectedUsername">El nombre de usuario del usuario conectado, usado para verificar si tiene permisos para eliminar al otro usuario.</param>
        public async Task RequestDeleteUserAsync(string sessionToken, string usernameToDelete, string connectedUsername)
        {
            try
            {
                // Construir el paquete de solicitud de eliminación usando ProtocolHelper
                string requestPacket = ProtocolHelper.ConstructDeleteUserPacket(sessionToken, usernameToDelete, connectedUsername);

                // Cifrar el paquete con AES antes de enviarlo
                byte[] encryptedRequest = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(requestPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedRequest));
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar la respuesta Base64 y desencriptarla con AES
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada de vuelta a un string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                Console.WriteLine($"Decrypted Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessDeleteUserResponse(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al solicitar la eliminación del usuario: {ex.Message}");
            }
        }

        /// <summary>
        /// Procesa la respuesta del servidor después de solicitar la eliminación de un usuario.
        /// </summary>
        /// <param name="response">La respuesta recibida del servidor, que contiene el código de estado y posible mensaje de error o éxito.</param>
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

        /// <summary>
        /// Solicita la lista de usuarios al servidor de manera asincrónica.
        /// Envía una solicitud cifrada al servidor, recibe y procesa las respuestas,
        /// y finalmente retorna la lista de usuarios encontrados.
        /// </summary>
        /// <param name="sessionToken">El token de sesión válido para autenticar la solicitud.</param>
        /// <param name="username">El nombre de usuario del que se solicita la lista de usuarios.</param>
        /// <returns>Una tarea asincrónica que representa la solicitud y procesamiento de los usuarios.
        /// Devuelve una lista de cadenas que contiene la información de los usuarios, o una lista vacía si no se encuentran usuarios.</returns>
        public async Task<List<string>> RequestShowUsersAsync(string sessionToken, string username)
        {
            try
            {
                // Construir el paquete de solicitud para mostrar los usuarios
                string requestPacket = ProtocolHelper.ConstructShowUsersPacket(sessionToken, username);

                // Cifrar el paquete con AES
                byte[] encryptedRequest = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(requestPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedRequest));
                await writer.FlushAsync();

                // Lista para almacenar la información formateada de los usuarios
                var usersList = new List<string>();
                bool endOfUsers = false; // Bandera para identificar el fin del envío

                while (!endOfUsers)
                {
                    // Leer la respuesta cifrada del servidor
                    string encryptedResponseBase64 = await reader.ReadLineAsync();
                    Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                    // Decodificar la respuesta Base64 y desencriptarla con AES
                    byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                    byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                    // Convertir la respuesta desencriptada de vuelta a un string
                    string response = Encoding.UTF8.GetString(decryptedResponse);
                    Console.WriteLine($"Decrypted Server Response: {response}");

                    // Procesar la respuesta del servidor
                    if (response.StartsWith("240")) // Inicio de transmisión
                    {
                        continue;
                    }
                    else if (response.StartsWith("241")) // Fin de transmisión
                    {
                        endOfUsers = true;
                        continue;
                    }
                    else if (response.StartsWith("239")) // Información de usuario
                    {
                        string userInfo = ProcessShowUsersResponse(response);
                        usersList.Add(userInfo);
                    }
                    else if (response.StartsWith("9")) // Protocolo 9: errores
                    {
                        string errorCode = response.Substring(1, 4);
                        switch (errorCode)
                        {
                            case "1410": // No hay usuarios
                                return usersList; // Retornar lista vacía
                            default:
                                throw new Exception($"Error desconocido: {errorCode}");
                        }
                    }
                    else
                    {
                        throw new Exception($"Respuesta inesperada del servidor: {response}");
                    }
                }

                // Mostrar los usuarios recolectados en un MessageBox
                if (usersList.Count > 0)
                {
                    string message = string.Join("\n", usersList);
                    MessageBox.Show(message, "Lista de Usuarios", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No se encontraron usuarios.", "Lista de Usuarios", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                return usersList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la solicitud de mostrar usuarios: {ex.Message}");
            }
        }


        /// <summary>
        /// Procesa la respuesta del servidor para mostrar la información de un usuario en formato legible.
        /// Extrae los datos de un paquete de tipo 239 (información de usuario) y los formatea para su visualización.
        /// </summary>
        /// <param name="response">La respuesta del servidor que contiene los datos del usuario en un formato codificado.</param>
        /// <returns>Una cadena formateada con la información del usuario, incluyendo el nombre de usuario, el nombre completo y el rol.</returns>
        private string ProcessShowUsersResponse(string response)
        {
            try
            {
                // Extraer los datos del paquete 239
                int currentIndex = 3; // Saltar el identificador "239"

                // Leer el offset y token (se ignora)
                int tokenLength = int.Parse(response.Substring(currentIndex, 2));
                currentIndex += 2 + tokenLength;

                // Leer el offset y nombre de usuario
                int usernameLength = int.Parse(response.Substring(currentIndex, 2));
                currentIndex += 2;
                string username = response.Substring(currentIndex, usernameLength);
                currentIndex += usernameLength;

                // Leer el offset y nombre completo
                int fullNameLength = int.Parse(response.Substring(currentIndex, 2));
                currentIndex += 2;
                string fullName = response.Substring(currentIndex, fullNameLength);
                currentIndex += fullNameLength;

                // Leer el offset y rol/permisos
                int roleLength = int.Parse(response.Substring(currentIndex, 2));
                currentIndex += 2;
                string role = response.Substring(currentIndex, roleLength);

                // Construir la línea formateada
                string userInfo = $"Usuario: {username}, Nombre completo: {fullName}, Rol: {role}";
                Console.WriteLine(userInfo); // Para depuración

                return userInfo;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la respuesta del usuario: {ex.Message}");
                throw new Exception($"Error al procesar la respuesta del usuario: {ex.Message}");
            }
        }






        /// <summary>
        /// Solicita la creación de un nuevo usuario al servidor.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario conectado, usado para autenticar la solicitud.</param>
        /// <param name="username">El nombre de usuario que se desea crear.</param>
        /// <param name="nombreUsuario">El nombre de usuario (nombre de acceso) para la nueva cuenta.</param>
        /// <param name="password">La contraseña asociada al nuevo usuario.</param>
        /// <param name="nombreCompleto">El nombre completo del nuevo usuario.</param>
        /// <param name="fechaNacimiento">La fecha de nacimiento del nuevo usuario.</param>
        /// <param name="otrosDatos">Cualquier otro dato adicional relevante para el usuario.</param>
        /// <param name="rolPermisos">El rol y los permisos que tendrá el nuevo usuario.</param>
        public async Task RequestCreateUserAsync(string sessionToken, string username, string nombreUsuario, string password, string nombreCompleto, string fechaNacimiento, string otrosDatos, string rolPermisos, string apodo)
        {
            try
            {
                // Construir el paquete de solicitud de creación usando ProtocolHelper
                string requestPacket = ProtocolHelper.ConstructCreateUserPacket(sessionToken, username, nombreUsuario, password, nombreCompleto, fechaNacimiento, otrosDatos, rolPermisos, apodo);

                // Cifrar el paquete con AES
                byte[] encryptedRequest = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(requestPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedRequest));
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar la respuesta Base64 y desencriptarla con AES
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada de vuelta a un string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                Console.WriteLine($"Decrypted Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessCreateUserResponse(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al solicitar la creación del usuario: {ex.Message}");
            }
        }




        /// <summary>
        /// Procesa la respuesta del servidor después de intentar crear un usuario.
        /// </summary>
        /// <param name="response">La respuesta recibida del servidor que contiene el estado de la solicitud de creación de usuario.</param>
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




        /// <summary>
        /// Solicita al servidor la agenda de un usuario o de un grupo, dependiendo de si la agenda es grupal o individual.
        /// La solicitud es enviada al servidor encriptada, y la respuesta es procesada posteriormente.
        /// </summary>
        /// <param name="sessionToken">El token de sesión que autentica la solicitud del cliente.</param>
        /// <param name="username">El nombre de usuario que solicita la agenda.</param>
        /// <param name="isGrupal">Indica si la agenda solicitada es para un grupo o para un usuario individual.</param>
        /// <param name="groupName">El nombre del grupo, si la agenda solicitada es para un grupo. Este parámetro es opcional.</param>
        public async Task RequestShowAgendaAsync(string sessionToken, string username, bool isGrupal, string groupName = "")
        {
            try
            {
                // Construir el paquete de solicitud de agenda
                string agendaPacket = ProtocolHelper.ConstructAgendaPacket(sessionToken, username, isGrupal, groupName);

                // Cifrar el paquete con AES
                byte[] encryptedAgenda = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(agendaPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedAgenda));
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar la respuesta Base64 y desencriptarla con AES
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada de vuelta a un string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                Console.WriteLine($"Decrypted Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessShowAgendaResponse(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la solicitud para mostrar la agenda: {ex.Message}");
            }
        }


        /// <summary>
        /// Procesa la respuesta del servidor relacionada con la solicitud de mostrar la agenda.
        /// Dependiendo del código recibido en la respuesta, se identifica el estado de la muestra de la agenda,
        /// incluyendo el inicio, los días del mes y el fin de la muestra de la agenda.
        /// </summary>
        /// <param name="response">La respuesta recibida del servidor, que indica el estado de la solicitud de la agenda.</param>
        private void ProcessShowAgendaResponse(string response)
        {
            if (response.StartsWith("401"))
            {
                // Confirmación del inicio de la muestra de la agenda
                Console.WriteLine("Comienzo de la muestra de la agenda.");
            }
            else if (response.StartsWith("402"))
            {
                // Procesar los días del mes recibidos
                Console.WriteLine("Días del mes recibidos.");
            }
            else if (response.StartsWith("403"))
            {
                // Confirmación del fin de la muestra de la agenda
                Console.WriteLine("Fin de la muestra de la agenda.");
            }
            else
            {
                throw new Exception("Respuesta desconocida del servidor.");
            }
        }


        /// <summary>
        /// Solicita al servidor avanzar un mes en la agenda.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario conectado.</param>
        public async Task RequestAdvanceMonthAsync(string sessionToken, string username)
        {
            try
            {
                // Construir el paquete para avanzar un mes
                string advanceMonthPacket = ProtocolHelper.ConstructAdvanceMonthPacket(sessionToken, username);

                // Cifrar el paquete con AES
                byte[] encryptedPacket = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(advanceMonthPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedPacket));
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar la respuesta Base64 y desencriptarla con AES
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada de vuelta a un string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                Console.WriteLine($"Decrypted Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessAdvanceMonthResponse(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la solicitud para avanzar un mes en la agenda: {ex.Message}");
            }
        }

        /// <summary>
        /// Procesa la respuesta del servidor tras solicitar avanzar un mes en la agenda.
        /// </summary>
        /// <param name="response">Respuesta desencriptada del servidor.</param>
        private void ProcessAdvanceMonthResponse(string response)
        {
            if (response.StartsWith("402"))
            {
                // Confirmación del cambio exitoso de mes
                Console.WriteLine("Mes avanzado con éxito.");
            }
            else if (response.StartsWith("1604"))
            {
                // Error: límite del año alcanzado
                Console.WriteLine("Error: No se puede avanzar más allá del límite del año permitido (3000).");
            }
            else if (response.StartsWith("105"))
            {
                // Error: usuario inválido
                Console.WriteLine("Error: Usuario inválido.");
            }
            else if (response.StartsWith("1601"))
            {
                // Error: datos corruptos en el servidor
                Console.WriteLine("Error: Datos corruptos en el servidor.");
            }
            else
            {
                throw new Exception("Respuesta desconocida del servidor.");
            }
        }



        /// <summary>
        /// Solicita al servidor retroceder un mes en la agenda.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario conectado.</param>
        public async Task RequestGoBackMonthAsync(string sessionToken, string username)
        {
            try
            {
                // Construir el paquete para retroceder un mes
                string goBackMonthPacket = ProtocolHelper.ConstructGoBackMonthPacket(sessionToken, username);

                // Cifrar el paquete con AES
                byte[] encryptedPacket = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(goBackMonthPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedPacket));
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar la respuesta Base64 y desencriptarla con AES
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada de vuelta a un string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                Console.WriteLine($"Decrypted Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessGoBackMonthResponse(response);
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si algo falla
                MessageBox.Show($"Error en la solicitud para retroceder un mes en la agenda: {ex.Message}");
            }
        }


        /// <summary>
        /// Procesa la respuesta del servidor relacionada con la solicitud de retroceder al mes anterior en la agenda.
        /// Dependiendo del código recibido en la respuesta, se maneja el error correspondiente o se confirma el inicio de la muestra de la agenda.
        /// </summary>
        /// <param name="response">La respuesta recibida del servidor que indica el resultado de la solicitud para retroceder de mes.</param>
        private void ProcessGoBackMonthResponse(string response)
        {
            if (response.StartsWith("1604"))
            {
                // Error: No se puede retroceder más allá de enero de 2000
                MessageBox.Show("No puedes retroceder más allá de enero de 2000.");
            }
            else if (response.StartsWith("105"))
            {
                // Error: Usuario no válido
                MessageBox.Show("El usuario no es válido.");
            }
            else if (response.StartsWith("1601"))
            {
                // Error: Datos corruptos
                MessageBox.Show("Los datos de días del servidor están corruptos.");
            }
            else if (response.StartsWith("401"))
            {
                // Confirmación del inicio de la muestra de la agenda
                Console.WriteLine("Comienzo de la muestra de la agenda.");
            }
            else
            {
                throw new Exception("Respuesta desconocida del servidor.");
            }
        }





        /// <summary>
        /// Solicita al servidor avanzar un año en la agenda.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario conectado.</param>
        public async Task RequestAdvanceYearAsync(string sessionToken, string username)
        {
            try
            {
                // Construir el paquete para avanzar un año
                string advanceYearPacket = ProtocolHelper.ConstructAdvanceYearPacket(sessionToken, username);

                // Cifrar el paquete con AES
                byte[] encryptedPacket = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(advanceYearPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedPacket));
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar la respuesta Base64 y desencriptarla con AES
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada de vuelta a un string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                Console.WriteLine($"Decrypted Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessAdvanceYearResponse(response);
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si algo falla
                MessageBox.Show($"Error en la solicitud para avanzar un año en la agenda: {ex.Message}");
            }
        }



        /// <summary>
        /// Procesa la respuesta del servidor relacionada con la solicitud de avanzar al siguiente año en la agenda.
        /// Dependiendo del código recibido en la respuesta, se maneja el error correspondiente o se confirma el avance exitoso del año.
        /// </summary>
        /// <param name="response">La respuesta recibida del servidor que indica el resultado de la solicitud para avanzar de año.</param>
        private void ProcessAdvanceYearResponse(string response)
        {
            if (response.StartsWith("402"))
            {
                // Confirmación del cambio exitoso de año
                Console.WriteLine("Año avanzado con éxito.");
            }
            else if (response.StartsWith("1604"))
            {
                // Error: No se puede avanzar más allá del año 3000
                MessageBox.Show("No puedes avanzar más allá del año 3000.");
            }
            else if (response.StartsWith("105"))
            {
                // Error: Usuario no válido
                MessageBox.Show("El usuario no es válido.");
            }
            else if (response.StartsWith("1601"))
            {
                // Error: Datos corruptos
                MessageBox.Show("Los datos de días del servidor están corruptos.");
            }
            else
            {
                throw new Exception("Respuesta desconocida del servidor.");
            }
        }





        /// <summary>
        /// Solicita al servidor retroceder un año en la agenda.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario conectado.</param>
        public async Task RequestGoBackYearAsync(string sessionToken, string username)
        {
            try
            {
                // Construir el paquete para retroceder un año
                string goBackYearPacket = ProtocolHelper.ConstructGoBackYearPacket(sessionToken, username);

                // Cifrar el paquete con AES
                byte[] encryptedPacket = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(goBackYearPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedPacket));
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar la respuesta Base64 y desencriptarla con AES
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada de vuelta a un string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                Console.WriteLine($"Decrypted Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessGoBackYearResponse(response);
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si algo falla
                MessageBox.Show($"Error en la solicitud para retroceder un año en la agenda: {ex.Message}");
            }
        }




        /// <summary>
        /// Procesa la respuesta del servidor relacionada con la solicitud de retroceder un año en la agenda.
        /// Dependiendo del código recibido en la respuesta, se maneja el error correspondiente o se confirma el retroceso exitoso del año.
        /// </summary>
        /// <param name="response">La respuesta recibida del servidor que indica el resultado de la solicitud para retroceder de año.</param>
        private void ProcessGoBackYearResponse(string response)
        {
            if (response.StartsWith("1604"))
            {
                // Error: No se puede retroceder más allá de enero de 2000
                MessageBox.Show("No puedes retroceder más allá de enero de 2000.");
            }
            else if (response.StartsWith("105"))
            {
                // Error: Usuario no válido
                MessageBox.Show("El usuario no es válido.");
            }
            else if (response.StartsWith("1601"))
            {
                // Error: Datos corruptos
                MessageBox.Show("Los datos de días del servidor están corruptos.");
            }
            else if (response.StartsWith("401"))
            {
                // Confirmación del cambio exitoso de año
                Console.WriteLine("Año retrocedido con éxito.");
            }
            else
            {
                throw new Exception("Respuesta desconocida del servidor.");
            }
        }





        /// <summary>
        /// Envía una solicitud asíncrona para crear un nuevo evento (día) en el servidor.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario para autenticar la solicitud.</param>
        /// <param name="fecha">La fecha del evento que se va a crear.</param>
        /// <param name="contenido">El contenido o descripción del evento.</param>
        /// <param name="tags">Etiquetas asociadas al evento.</param>
        /// <param name="esGrupal">Indica si el evento es grupal (true) o individual (false).</param>
        /// <param name="nombreGrupo">El nombre del grupo al que pertenece el evento (si es grupal).</param>
        public async Task RequestCreateDayAsync(string sessionToken, string username, string fecha, string contenido, string tags, bool esGrupal, string nombreGrupo)
        {
            try
            {
                // Construir el paquete de solicitud
                string requestPacket = ProtocolHelper.ConstructCreateDayPacket(sessionToken, username, fecha, contenido, tags, esGrupal, nombreGrupo);

                // Cifrar el paquete con AES
                byte[] encryptedRequest = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(requestPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedRequest));
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar la respuesta Base64 y desencriptarla con AES
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada de vuelta a un string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                Console.WriteLine($"Decrypted Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessCreateDayResponse(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la solicitud de creación del evento: {ex.Message}");
            }
        }








        /// <summary>
        /// Procesa la respuesta recibida del servidor tras solicitar la creación de un evento (día).
        /// </summary>
        /// <param name="response">La respuesta del servidor que contiene el estado de la solicitud.</param>
        private void ProcessCreateDayResponse(string response)
        {
            if (response.Length < 3)
            {
                MessageBox.Show("Respuesta demasiado corta.");
                return;
            }

            // Extraer el protocolo y la acción de la respuesta
            char protocol = response[0];
            int action = int.Parse(response.Substring(1, 2));

            // Verificar que el protocolo y la acción sean los esperados
            if (protocol != '2' || action != 6)
            {
                MessageBox.Show($"Protocolo o acción inesperados: {protocol}, {action}");
                return;
            }

            int index = 3;
            // Extraer el código de error de la respuesta
            int errorCode = int.Parse(response.Substring(index, 4));
            index += 4;

            // Procesar la respuesta según el código de error
            switch (errorCode)
            {
                case 0:
                    MessageBox.Show("Evento creado exitosamente.");
                    break;
                case 1104:
                    MessageBox.Show("Formato de fecha incorrecto.");
                    break;
                case 1605:
                    MessageBox.Show("El objeto día no existe.");
                    break;
                case 1602:
                    MessageBox.Show("El día ya ha sido asignado.");
                    break;
                case 1502:
                    MessageBox.Show("Demasiados tags (máximo 10).");
                    break;
                case 1404:
                    MessageBox.Show("No perteneces al grupo especificado.");
                    break;
                default:
                    MessageBox.Show($"Error inesperado: {errorCode}");
                    break;
            }
        }



        /// <summary>
        /// Solicita la creación de un nuevo grupo en el servidor, enviando el paquete con los datos necesarios para la creación.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que realiza la solicitud.</param>
        /// <param name="username">El nombre de usuario del solicitante.</param>
        /// <param name="nombreGrupo">El nombre del grupo que se desea crear.</param>
        /// <returns>Una tarea asincrónica que representa la operación de la solicitud de creación de grupo.</returns>
        public async Task RequestCreateGroupAsync(string sessionToken, string username, string nombreGrupo)
        {
            try
            {
                // Construir el paquete de solicitud
                string requestPacket = ProtocolHelper.ConstructCreateGroupPacket(sessionToken, username, nombreGrupo);

                // Cifrar el paquete con AES
                byte[] encryptedRequest = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(requestPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedRequest));
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar la respuesta Base64 y desencriptarla con AES
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada de vuelta a un string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                Console.WriteLine($"Decrypted Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessCreateGroupResponse(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la solicitud de creación del grupo: {ex.Message}");
            }
        }



        /// <summary>
        /// Procesa la respuesta del servidor para la solicitud de creación de grupo.
        /// </summary>
        /// <param name="response">La respuesta del servidor que contiene el estado de la creación del grupo.</param>
        /// <exception cref="Exception">Lanza excepciones con mensajes específicos en caso de errores o respuestas inesperadas del servidor.</exception>
        private void ProcessCreateGroupResponse(string response)
        {
            if (response.StartsWith("226"))
            {
                // Grupo creado exitosamente
                Console.WriteLine("Grupo creado correctamente.");
            }
            else if (response.StartsWith("9"))
            {
                // Procesar errores específicos según el protocolo
                string errorCode = response.Substring(1, 4); // Asumiendo que el código de error está en esta posición
                switch (errorCode)
                {
                    case "112":
                        throw new Exception("El usuario no tiene permisos para crear un grupo.");
                    case "1406":
                        throw new Exception("El usuario ya tiene el número máximo de grupos permitidos.");
                    case "1401":
                        throw new Exception("El grupo con el nombre dado ya existe.");
                    default:
                        throw new Exception("Error desconocido al crear el grupo.");
                }
            }
            else
            {
                throw new Exception("Respuesta desconocida del servidor.");
            }
        }



        /// <summary>
        /// Solicita la eliminación de un grupo al servidor.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario.</param>
        /// <param name="username">El nombre de usuario que solicita la eliminación del grupo.</param>
        /// <param name="nombreGrupo">El nombre del grupo que se desea eliminar.</param>
        /// <returns>Una tarea asincrónica que representa la solicitud de eliminación de grupo.</returns>
        /// <exception cref="Exception">Lanza excepciones si ocurre un error durante la solicitud o el procesamiento de la respuesta.</exception>
        public async Task RequestDeleteGroupAsync(string sessionToken, string username, string nombreGrupo)
        {
            try
            {
                // Construir el paquete de solicitud
                string requestPacket = ProtocolHelper.ConstructDeleteGroupPacket(sessionToken, username, nombreGrupo);

                // Cifrar el paquete con AES
                byte[] encryptedRequest = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(requestPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedRequest));
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar la respuesta Base64 y desencriptarla con AES
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada de vuelta a un string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                Console.WriteLine($"Decrypted Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessDeleteGroupResponse(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la solicitud de eliminación del grupo: {ex.Message}");
            }
        }



        /// <summary>
        /// Procesa la respuesta del servidor al intentar eliminar un grupo.
        /// </summary>
        /// <param name="response">La respuesta recibida del servidor en formato de cadena.</param>
        /// <exception cref="Exception">Lanza excepciones específicas dependiendo de la respuesta del servidor.
        /// El error puede indicar problemas de permisos, inexistencia del grupo o respuestas desconocidas del servidor.</exception>
        private void ProcessDeleteGroupResponse(string response)
        {
            if (response.StartsWith("218"))
            {
                // Confirmación de eliminación para el usuario que lo solicita
                Console.WriteLine("Grupo eliminado correctamente.");
            }
            else if (response.StartsWith("9"))
            {
                // Procesar errores específicos según el protocolo
                string errorCode = response.Substring(1, 4); // Asumiendo que el código de error está en esta posición
                switch (errorCode)
                {
                    case "112":
                        throw new Exception("El usuario no tiene permisos para eliminar este grupo.");
                    case "1402":
                        throw new Exception("El grupo no existe.");
                    default:
                        throw new Exception("Error desconocido al eliminar el grupo.");
                }
            }
            else
            {
                throw new Exception("Respuesta desconocida del servidor.");
            }
        }



        /// <summary>
        /// Realiza una solicitud asincrónica al servidor para eliminar la agenda de un grupo específico.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario para la autenticación.</param>
        /// <param name="username">El nombre de usuario que solicita la eliminación de la agenda grupal.</param>
        /// <param name="nombreGrupo">El nombre del grupo cuya agenda se desea eliminar.</param>
        /// <returns>Una tarea asincrónica que representa la operación de solicitud de eliminación de la agenda grupal.</returns>
        /// <exception cref="Exception">Lanza excepciones en caso de errores durante la solicitud, como errores de cifrado o respuestas inesperadas del servidor.</exception>
        public async Task RequestDeleteGroupAgendaAsync(string sessionToken, string username, string nombreGrupo)
        {
            try
            {
                // Construir el paquete de solicitud
                string requestPacket = ProtocolHelper.ConstructDeleteGroupAgendaPacket(sessionToken, username, nombreGrupo);

                // Cifrar el paquete con AES
                byte[] encryptedRequest = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(requestPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedRequest));
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar la respuesta Base64 y desencriptarla con AES
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada de vuelta a un string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                Console.WriteLine($"Decrypted Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessDeleteGroupAgendaResponse(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la solicitud de eliminación de la agenda grupal: {ex.Message}");
            }
        }



        /// <summary>
        /// Procesa la respuesta del servidor para la solicitud de eliminación de la agenda de un grupo.
        /// </summary>
        /// <param name="response">La respuesta del servidor que se va a procesar.</param>
        /// <exception cref="Exception">Lanza excepciones en caso de respuestas no esperadas o errores específicos del servidor, como falta de permisos o grupo inexistente.</exception>
        private void ProcessDeleteGroupAgendaResponse(string response)
        {
            if (response.StartsWith("218"))
            {
                // Confirmación de eliminación para el usuario que lo solicita
                Console.WriteLine("Agenda grupal eliminada correctamente.");
            }
            else if (response.StartsWith("9"))
            {
                // Procesar errores específicos según el protocolo
                string errorCode = response.Substring(1, 4); // Asumiendo que el código de error está en esta posición
                switch (errorCode)
                {
                    case "112":
                        throw new Exception("El usuario no tiene permisos para eliminar esta agenda grupal.");
                    case "1404":
                        throw new Exception("El grupo no existe.");
                    default:
                        throw new Exception("Error desconocido al eliminar la agenda grupal.");
                }
            }
            else
            {
                throw new Exception("Respuesta desconocida del servidor.");
            }
        }



        /// <summary>
        /// Envía una solicitud para invitar a un usuario a un grupo.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que realiza la solicitud.</param>
        /// <param name="username">El nombre de usuario del solicitante que envía la invitación.</param>
        /// <param name="nombreGrupo">El nombre del grupo al que se desea invitar al usuario.</param>
        /// <param name="sobrenombreUsuario">El sobrenombre del usuario que se va a invitar al grupo.</param>
        /// <exception cref="Exception">Lanza una excepción si ocurre un error en la solicitud o si la respuesta del servidor es inesperada.</exception>
        public async Task RequestInviteUserToGroupAsync(string sessionToken, string username, string nombreGrupo, string sobrenombreUsuario)
        {
            try
            {
                // Construir el paquete de solicitud
                string requestPacket = ProtocolHelper.ConstructInviteUserPacket(sessionToken, username, nombreGrupo, sobrenombreUsuario);

                // Cifrar el paquete con AES
                byte[] encryptedRequest = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(requestPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedRequest));
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar la respuesta Base64 y desencriptarla con AES
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada de vuelta a un string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                Console.WriteLine($"Decrypted Server Response: {response}");

                // Procesar la respuesta del servidor
                ProcessInviteUserResponse(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la solicitud de invitación al grupo: {ex.Message}");
            }
        }




        /// <summary>
        /// Procesa la respuesta del servidor después de intentar invitar a un usuario a un grupo.
        /// </summary>
        /// <param name="response">La respuesta recibida del servidor, que contiene el resultado de la solicitud de invitación.</param>
        /// <exception cref="Exception">Lanza una excepción si la respuesta del servidor indica un error o si la respuesta es inesperada.</exception>
        private void ProcessInviteUserResponse(string response)
        {
            if (response.StartsWith("228"))
            {
                // Confirmación de invitación exitosa
                Console.WriteLine("Usuario invitado correctamente al grupo.");
            }
            else if (response.StartsWith("9"))
            {
                // Procesar errores específicos según el protocolo
                string errorCode = response.Substring(1, 4);
                switch (errorCode)
                {
                    case "112":
                        throw new Exception("El usuario no tiene permisos para invitar a este grupo.");
                    case "1404":
                        throw new Exception("El grupo no existe.");
                    case "1603":
                        throw new Exception("El sobrenombre del usuario no corresponde a ningún usuario registrado.");
                    case "1407":
                        throw new Exception("El usuario ya pertenece al grupo.");
                    default:
                        throw new Exception("Error desconocido al invitar al usuario.");
                }
            }
            else
            {
                throw new Exception("Respuesta desconocida del servidor.");
            }
        }










        /// <summary>
        /// Solicita al servidor la lista de grupos que el usuario posee.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que realiza la solicitud.</param>
        /// <param name="username">El nombre de usuario del propietario de los grupos.</param>
        /// <returns>Una lista de nombres de los grupos que el usuario posee.</returns>
        /// <exception cref="Exception">Lanza una excepción si ocurre un error durante la solicitud o el procesamiento de la respuesta del servidor.</exception>
        public async Task<List<string>> RequestViewOwnedGroupsAsync(string sessionToken, string username)
        {
            try
            {
                // Construir el paquete de solicitud
                string requestPacket = ProtocolHelper.ConstructViewOwnedGroupsPacket(sessionToken, username);

                // Cifrar el paquete con AES
                byte[] encryptedRequest = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(requestPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedRequest));
                await writer.FlushAsync();

                // Lista para almacenar los grupos propios
                var grupos = new List<string>();
                bool endOfGroups = false; // Bandera para identificar fin de envío

                while (!endOfGroups)
                {
                    // Leer la respuesta cifrada del servidor
                    string encryptedResponseBase64 = await reader.ReadLineAsync();
                    Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                    // Decodificar la respuesta Base64 y desencriptarla con AES
                    byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                    byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                    // Convertir la respuesta desencriptada de vuelta a un string
                    string response = Encoding.UTF8.GetString(decryptedResponse);
                    Console.WriteLine($"Decrypted Server Response: {response}");

                    // Procesar la respuesta
                    if (response.StartsWith("244")) // Acción 44: Inicio de envío
                    {
                        continue;
                    }
                    else if (response.StartsWith("245")) // Acción 45: Fin de envío
                    {
                        endOfGroups = true;
                        continue;
                    }
                    else if (response.StartsWith("210")) // Acción 10: Datos del grupo
                    {
                        string groupName = ProcessViewOwnedGroupsResponse(response);
                        grupos.Add(groupName);
                    }
                    else if (response.StartsWith("9")) // Protocolo 9: errores
                    {
                        string errorCode = response.Substring(1, 4); // Extraer código de error
                        switch (errorCode)
                        {
                            case "1410": // No hay grupos propios
                                return grupos; // Retornar lista vacía
                            default:
                                throw new Exception($"Error desconocido: {errorCode}");
                        }
                    }
                    else
                    {
                        throw new Exception($"Respuesta inesperada del servidor: {response}");
                    }
                }

                return grupos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la solicitud de grupos propios: {ex.Message}");
            }
        }



        /// <summary>
        /// Procesa la respuesta del servidor para extraer el nombre de un grupo al que el usuario posee acceso.
        /// </summary>
        /// <param name="response">La respuesta del servidor que contiene los detalles del grupo.</param>
        /// <returns>El nombre del grupo extraído de la respuesta del servidor.</returns>
        /// <exception cref="Exception">Lanza una excepción si el formato de la respuesta no es el esperado o si hay errores en la validación de los datos.</exception>
        private string ProcessViewOwnedGroupsResponse(string response)
        {
            try
            {
                // Extraer datos según el formato del protocolo
                char protocol = response[0];
                int action = int.Parse(response.Substring(1, 2));

                // Verificar protocolo y acción
                if (protocol != '2' || action != 10)
                {
                    MessageBox.Show($"Protocolo o acción inesperada: {protocol}, {action}");
                    throw new Exception($"Protocolo o acción inesperada: {protocol}, {action}");
                }

                int index = 3; // Comenzar después del protocolo y la acción
                int tokenOffset = int.Parse(response.Substring(index, 2));
                index += 2;

                string token = response.Substring(index, tokenOffset);
                index += tokenOffset;

                // Verificar el token de sesión
                if (token != SessionToken)
                {
                    MessageBox.Show("Token incorrecto en la respuesta.");
                    throw new Exception("Token incorrecto en la respuesta.");
                }

                // Leer el offset del nombre del grupo
                int groupNameOffset = int.Parse(response.Substring(index, 1)); // Solo un carácter para el offset
                index += 1;

                // Extraer el nombre del grupo
                string groupName = response.Substring(index, groupNameOffset);

                return groupName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la respuesta del grupo: {ex.Message}");
                throw new Exception($"Error al procesar la respuesta del grupo: {ex.Message}");
            }
        }



        /// <summary>
        /// Solicita al servidor los grupos de los cuales el usuario es miembro y devuelve una lista con los nombres de esos grupos.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que autentica la solicitud.</param>
        /// <param name="username">El nombre de usuario del solicitante.</param>
        /// <returns>Una lista de nombres de grupos a los que el usuario pertenece.</returns>
        /// <exception cref="Exception">Lanza una excepción si ocurre un error durante el proceso de solicitud o si la respuesta del servidor es inesperada.</exception>
        public async Task<List<string>> RequestViewMembershipGroupsAsync(string sessionToken, string username)
        {
            try
            {
                // Construir el paquete de solicitud
                string requestPacket = ProtocolHelper.ConstructViewMembershipGroupsPacket(sessionToken, username);

                // Cifrar el paquete con AES
                byte[] encryptedRequest = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(requestPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedRequest));
                await writer.FlushAsync();

                // Lista para almacenar los grupos de membresía
                var grupos = new List<string>();
                bool endOfGroups = false; // Bandera para identificar fin de envío

                while (!endOfGroups)
                {
                    // Leer la respuesta cifrada del servidor
                    string encryptedResponseBase64 = await reader.ReadLineAsync();
                    Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                    // Decodificar la respuesta Base64 y desencriptarla con AES
                    byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                    byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                    // Convertir la respuesta desencriptada de vuelta a un string
                    string response = Encoding.UTF8.GetString(decryptedResponse);
                    Console.WriteLine($"Decrypted Server Response: {response}");

                    // Procesar la respuesta
                    if (response.StartsWith("244")) // Acción 44: Inicio de envío
                    {
                        continue;
                    }
                    else if (response.StartsWith("245")) // Acción 45: Fin de envío
                    {
                        endOfGroups = true;
                        continue;
                    }
                    else if (response.StartsWith("210")) // Acción 10: Datos del grupo
                    {
                        string groupName = ProcessViewGroupResponse(response);
                        grupos.Add(groupName);
                    }
                    else if (response.StartsWith("9")) // Protocolo 9: errores
                    {
                        string errorCode = response.Substring(1, 4); // Extraer código de error
                        switch (errorCode)
                        {
                            case "1410": // No hay grupos de membresía
                                return grupos; // Retornar lista vacía
                            default:
                                throw new Exception($"Error desconocido: {errorCode}");
                        }
                    }
                    else
                    {
                        throw new Exception($"Respuesta inesperada del servidor: {response}");
                    }
                }

                return grupos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la solicitud de grupos de membresía: {ex.Message}");
            }
        }





        /// <summary>
        /// Solicita al servidor la lista de todos los grupos y devuelve una lista con los nombres de esos grupos.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que autentica la solicitud.</param>
        /// <param name="username">El nombre de usuario del solicitante.</param>
        /// <returns>Una lista de nombres de todos los grupos disponibles.</returns>
        /// <exception cref="Exception">Lanza una excepción si ocurre un error durante el proceso de solicitud o si la respuesta del servidor es inesperada.</exception>
        public async Task<List<string>> RequestViewAllGroupsAsync(string sessionToken, string username)
        {
            try
            {
                // Construir el paquete de solicitud
                string requestPacket = ProtocolHelper.ConstructViewAllGroupsPacket(sessionToken, username);

                // Cifrar el paquete con AES
                byte[] encryptedRequest = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(requestPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedRequest));
                await writer.FlushAsync();

                // Lista para almacenar los grupos
                var grupos = new List<string>();
                bool endOfGroups = false; // Bandera para identificar fin de envío

                while (!endOfGroups)
                {
                    // Leer la respuesta cifrada del servidor
                    string encryptedResponseBase64 = await reader.ReadLineAsync();
                    Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                    // Decodificar la respuesta Base64 y desencriptarla con AES
                    byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                    byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                    // Convertir la respuesta desencriptada de vuelta a un string
                    string response = Encoding.UTF8.GetString(decryptedResponse);
                    Console.WriteLine($"Decrypted Server Response: {response}");

                    // Procesar la respuesta
                    if (response.StartsWith("244")) // Acción 44: Inicio de envío
                    {
                        continue;
                    }
                    else if (response.StartsWith("245")) // Acción 45: Fin de envío
                    {
                        endOfGroups = true;
                        continue;
                    }
                    else if (response.StartsWith("210")) // Acción 10: Datos del grupo
                    {
                        string groupName = ProcessViewGroupResponse(response);
                        grupos.Add(groupName);
                    }
                    else if (response.StartsWith("9")) // Protocolo 9: errores
                    {
                        string errorCode = response.Substring(1, 4); // Extraer código de error
                        switch (errorCode)
                        {
                            case "1410": // No hay grupos
                                return grupos; // Retornar lista vacía
                            default:
                                throw new Exception($"Error desconocido: {errorCode}");
                        }
                    }
                    else
                    {
                        throw new Exception($"Respuesta inesperada del servidor: {response}");
                    }
                }

                return grupos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la solicitud de todos los grupos: {ex.Message}");
            }
        }





        /// <summary>
        /// Procesa la respuesta del servidor para extraer el nombre de un grupo según el formato del protocolo.
        /// </summary>
        /// <param name="response">La respuesta del servidor en formato de protocolo.</param>
        /// <returns>El nombre del grupo extraído de la respuesta.</returns>
        /// <exception cref="Exception">Lanza una excepción si ocurre un error en el formato de la respuesta o si el token de sesión no coincide.</exception>
        private string ProcessViewGroupResponse(string response)
        {
            try
            {
                // Extraer datos según el formato del protocolo
                int index = 3; // Comenzar después del protocolo y la acción
                int tokenOffset = int.Parse(response.Substring(index, 2));

                index += 2;
                string token = response.Substring(index, tokenOffset);

                index += tokenOffset;

                // Verificar el token de sesión
                if (token != SessionToken)
                {
                    throw new Exception("Token incorrecto en la respuesta.");
                }

                // Leer el offset del nombre del grupo
                int groupNameOffset = int.Parse(response.Substring(index, 1));

                index += 1;

                // Extraer el nombre del grupo
                string groupName = response.Substring(index, groupNameOffset);

                return groupName;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al procesar la respuesta del grupo: {ex.Message}");
            }
        }



        /// <summary>
        /// Envía una solicitud al servidor para actualizar la información "Sobre" del usuario.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario.</param>
        /// <param name="username">El nombre de usuario asociado a la solicitud.</param>
        /// <param name="infoSobre">La nueva información "Sobre" que se desea actualizar.</param>
        /// <returns>Una tarea asincrónica que maneja el proceso de solicitud y respuesta del servidor.</returns>
        /// <exception cref="Exception">Lanza una excepción si ocurre un error durante el proceso de cifrado, transmisión o respuesta del servidor.</exception>
        public async Task SendServerInfoUpdateRequestAsync(string sessionToken, string username, string infoSobre)
        {
            try
            {
                // Construir el paquete de actualización de "Sobre" usando el token de sesión, nombre de usuario e información
                string infoSobrePacket = ProtocolHelper.ConstructInfoSobrePacket(sessionToken, username, infoSobre);
                MessageBox.Show($"Info 'Sobre' Packet Before Encryption: {infoSobrePacket}");

                // Verificar que el writer está inicializado
                if (writer == null)
                {
                    MessageBox.Show("Error: writer no está inicializado.");
                    return;
                }

                // Encriptar el paquete con AES
                byte[] encryptedInfoSobrePacket = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(infoSobrePacket));

                // Convertir el paquete encriptado a Base64 para transmitirlo
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedInfoSobrePacket));
                await writer.FlushAsync();

                // Esperar la respuesta encriptada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                MessageBox.Show($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar y desencriptar la respuesta
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada a string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                MessageBox.Show($"Decrypted Server Response: {response}");

                // Verificar si la respuesta del servidor es "OK" o cualquier otro mensaje esperado
                if (response == "OK")
                {
                    MessageBox.Show("La información 'Sobre' se ha actualizado correctamente.");
                }
                else
                {
                    MessageBox.Show($"Error al actualizar la información 'Sobre': {response}");
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier error
                MessageBox.Show($"Error during info sobre request: {ex.Message}");
            }
        }





        /// <summary>
        /// Envía una solicitud al servidor para mostrar los datos del mes actual para el usuario especificado.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario.</param>
        /// <param name="username">El nombre de usuario asociado a la solicitud.</param>
        /// <returns>Una tarea asincrónica que realiza el proceso de solicitud y procesamiento de la respuesta del servidor.</returns>
        /// <exception cref="Exception">Lanza una excepción si ocurre un error durante la construcción, cifrado, transmisión, o procesamiento de la respuesta del servidor.</exception>
        public async Task RequestShowMonthAsync(string sessionToken, string username)
        {
            try
            {
                // Construir el paquete de solicitud (Acción 01: Mostrar mes)
                string requestPacket = ProtocolHelper.ConstructShowMonthPacket(sessionToken, username);

                // Cifrar el paquete con AES
                byte[] encryptedRequest = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(requestPacket));

                // Convertir el paquete cifrado a Base64 para enviarlo como texto
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedRequest));
                await writer.FlushAsync();

                // Leer la respuesta cifrada del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                Console.WriteLine($"Encrypted Server Response: {encryptedResponseBase64}");

                // Decodificar la respuesta Base64 y desencriptarla con AES
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);

                // Convertir la respuesta desencriptada de vuelta a un string
                string response = Encoding.UTF8.GetString(decryptedResponse);
                Console.WriteLine($"Decrypted Server Response: {response}");

                // Procesar la respuesta
                await ProcessShowMonthResponse(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la solicitud para mostrar el mes: {ex.Message}");
            }
        }


        private async Task ProcessShowMonthResponse(string response)
        {

        }




        /// <summary>
        /// Envía una solicitud al servidor para insertar un nuevo tag en una fecha específica del usuario.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario.</param>
        /// <param name="username">El nombre de usuario asociado a la solicitud.</param>
        /// <param name="fecha">La fecha en la que se desea insertar el tag, en el formato esperado por el servidor.</param>
        /// <param name="nuevoTag">El contenido del tag que se desea insertar.</param>
        /// <returns>Una tarea asincrónica que realiza el proceso de inserción del tag.</returns>
        /// <exception cref="Exception">Lanza una excepción si ocurre un error durante el cifrado, envío, recepción o procesamiento de la respuesta del servidor.</exception>
        public async Task InsertarTagAsync(string sessionToken, string username, string fecha, string nuevoTag)
        {
            // Lógica para enviar la solicitud al servidor
            // Construcción del paquete para insertar el tag según el protocolo

            string packet = ProtocolHelper.ConstructInsertTagPacket(sessionToken, username, fecha, nuevoTag);

            // Cifrado y envío del paquete al servidor (similar a los otros métodos de solicitud)
            byte[] encryptedPacket = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(packet));
            await writer.WriteLineAsync(Convert.ToBase64String(encryptedPacket));
            await writer.FlushAsync();

            // Leer y procesar la respuesta del servidor
            string encryptedResponseBase64 = await reader.ReadLineAsync();
            byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
            byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);
            string response = Encoding.UTF8.GetString(decryptedResponse);

            // Procesar la respuesta
            ProcessInsertTagResponse(response);
        }





        /// <summary>
        /// Procesa la respuesta del servidor tras intentar insertar un tag en la agenda.
        /// </summary>
        /// <param name="response">La respuesta del servidor en formato de string.</param>
        /// <remarks>
        /// Este método maneja diferentes códigos de respuesta del servidor, mostrando mensajes apropiados al usuario 
        /// según el resultado de la operación. Incluye casos de éxito, errores específicos y errores desconocidos.
        /// </remarks>
        private void ProcessInsertTagResponse(string response)
        {
            switch (response)
            {
                case "425":
                    // Confirmación de tag insertado con éxito
                    MessageBox.Show("Tag insertado correctamente.");
                    break;

                case "230":
                    // Notificación para miembros del grupo
                    MessageBox.Show("El tag ha sido insertado. Notificación enviada a los miembros del grupo.");
                    break;

                case "1104":
                    MessageBox.Show("Error: Fecha con formato incorrecto.");
                    break;

                case "9":
                    MessageBox.Show("Error: No se pudo obtener el objeto de fecha.");
                    break;

                case "1605":
                    MessageBox.Show("Error: Día no preinicializado.");
                    break;

                case "1606":
                    MessageBox.Show("Error: Día no creado.");
                    break;

                case "1601":
                    MessageBox.Show("Error: Objeto de agenda corrupto.");
                    break;

                case "1402":
                    MessageBox.Show("Error: No perteneces al grupo.");
                    break;

                case "1404":
                    MessageBox.Show("Error: El grupo no existe.");
                    break;

                case "1403":
                    MessageBox.Show("Error: No tienes el bloqueo de edición.");
                    break;

                case "1505":
                    MessageBox.Show("Error: El tag ya existe.");
                    break;

                case "1502":
                    MessageBox.Show("Error: Ya hay 10 tags creados previamente.");
                    break;

                default:
                    MessageBox.Show($"Error desconocido: {response}");
                    break;
            }
        }




        /// <summary>
        /// Solicita la eliminación de un tag en el servidor para una fecha específica.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario actual.</param>
        /// <param name="username">Nombre de usuario del cliente.</param>
        /// <param name="fecha">Fecha asociada al tag que se desea eliminar.</param>
        /// <param name="tagAEliminar">Nombre del tag que se eliminará.</param>
        /// <returns>Una tarea asincrónica que representa la operación de eliminación del tag.</returns>
        /// <remarks>
        /// Este método construye el paquete de eliminación de tag según el protocolo definido, lo cifra con AES, lo envía al servidor,
        /// y posteriormente procesa la respuesta del servidor para determinar el resultado de la operación.
        /// </remarks>
        public async Task EliminarTagAsync(string sessionToken, string username, string fecha, string tagAEliminar)
        {
            // Construcción del paquete para eliminar el tag
            string packet = ProtocolHelper.ConstructEliminarTagPacket(sessionToken, username, fecha, tagAEliminar);

            // Cifrado y envío del paquete al servidor
            byte[] encryptedPacket = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(packet));
            await writer.WriteLineAsync(Convert.ToBase64String(encryptedPacket));
            await writer.FlushAsync();

            // Leer y procesar la respuesta del servidor
            string encryptedResponseBase64 = await reader.ReadLineAsync();
            byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
            byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);
            string response = Encoding.UTF8.GetString(decryptedResponse);

            // Procesar la respuesta
            ProcessEliminarTagResponse(response);
        }






        /// <summary>
        /// Procesa la respuesta del servidor tras intentar eliminar un tag.
        /// </summary>
        /// <param name="response">Respuesta del servidor tras la solicitud de eliminación del tag.</param>
        /// <remarks>
        /// El método interpreta los códigos de respuesta del servidor y muestra un mensaje correspondiente al usuario.
        /// Maneja confirmaciones exitosas, notificaciones de grupo y diversos errores relacionados con la operación.
        /// </remarks>
        private void ProcessEliminarTagResponse(string response)
        {
            if (response.StartsWith("426"))
            {
                // Confirmación de tag eliminado con éxito
                MessageBox.Show("Tag eliminado correctamente.");
            }
            else if (response.StartsWith("230"))
            {
                // Notificación para miembros del grupo
                MessageBox.Show("El tag ha sido eliminado. Notificación enviada a los miembros del grupo.");
            }
            else
            {
                // Manejar errores
                switch (response)
                {
                    case "1104":
                        MessageBox.Show("Error: Fecha con formato incorrecto.");
                        break;
                    case "9":
                        MessageBox.Show("Error: No se pudo obtener el objeto de fecha.");
                        break;
                    case "1605":
                        MessageBox.Show("Error: Día no preinicializado.");
                        break;
                    case "1606":
                        MessageBox.Show("Error: Día no creado.");
                        break;
                    case "1601":
                        MessageBox.Show("Error: Objeto de agenda corrupto.");
                        break;
                    case "1402":
                        MessageBox.Show("Error: No perteneces al grupo.");
                        break;
                    case "1404":
                        MessageBox.Show("Error: El grupo no existe.");
                        break;
                    case "1403":
                        MessageBox.Show("Error: No tienes el bloqueo de edición.");
                        break;
                    case "1504":
                        MessageBox.Show("Error: El tag no existe.");
                        break;
                    default:
                        MessageBox.Show("Error desconocido.");
                        break;
                }
            }
        }



        /// <summary>
        /// Realiza una búsqueda de un tag en el servidor.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario.</param>
        /// <param name="username">Nombre de usuario que realiza la búsqueda.</param>
        /// <param name="grupal">Indica si la búsqueda es en un contexto grupal.</param>
        /// <param name="tag">Nombre del tag a buscar.</param>
        /// <param name="groupName">Nombre del grupo (opcional, requerido si es una búsqueda grupal).</param>
        /// <remarks>
        /// Este método cifra el paquete de búsqueda, lo envía al servidor y procesa la respuesta recibida.
        /// En caso de error, muestra un mensaje apropiado.
        /// </remarks>
        public async Task SearchTagAsync(string sessionToken, string username, bool grupal, string tag, string groupName = null)
        {
            try
            {
                // Construir el paquete
                string packet = ProtocolHelper.ConstructSearchTagPacket(sessionToken, username, grupal, tag, groupName);

                // Cifrar y enviar el paquete al servidor
                byte[] encryptedPacket = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(packet));
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedPacket));
                await writer.FlushAsync();

                // Leer y procesar la respuesta del servidor
                string encryptedResponseBase64 = await reader.ReadLineAsync();
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);
                string response = Encoding.UTF8.GetString(decryptedResponse);

                // Procesar la respuesta
                ProcessSearchTagResponse(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }




        /// <summary>
        /// Procesa la respuesta del servidor para la búsqueda de tags.
        /// </summary>
        /// <param name="response">Respuesta completa recibida del servidor.</param>
        private void ProcessSearchTagResponse(string response)
        {
            // Lista para almacenar los días coincidentes
            List<string> matchingDays = new List<string>();

            // Dividir la respuesta en líneas
            string[] lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            // Variable de control
            bool readingResults = false;

            foreach (string line in lines)
            {
                // Identificar el tipo de respuesta
                if (line.StartsWith("1601"))
                {
                    MessageBox.Show("Error: El mostreo está corrupto.");
                    return;
                }
                else if (line.StartsWith("105"))
                {
                    MessageBox.Show("Error: El usuario no existe.");
                    return;
                }
                else if (line.StartsWith("1402"))
                {
                    MessageBox.Show("Error: No perteneces al grupo.");
                    return;
                }
                else if (line.StartsWith("407"))
                {
                    MessageBox.Show("No hay resultados: No se encontraron coincidencias.");
                    return;
                }
                else if (line.StartsWith("405"))
                {
                    // Inicio de resultados
                    readingResults = true;
                    continue;
                }
                else if (line.StartsWith("404"))
                {
                    // Fin de resultados
                    readingResults = false;
                    break;
                }
                else if (readingResults)
                {
                    // Día coincidente encontrado
                    matchingDays.Add(line);
                }
                else
                {
                    // Manejar líneas inesperadas
                    MessageBox.Show($"Respuesta desconocida: {line}");
                    return;
                }
            }

            // Mostrar los resultados al usuario
            if (matchingDays.Count > 0)
            {
                string results = string.Join("\n", matchingDays);
                MessageBox.Show($"Días coincidentes:\n{results}");
            }
            else
            {
                MessageBox.Show("No se encontraron días coincidentes.");
            }
        }




        /// <summary>
        /// Modifica un evento en el servidor.
        /// </summary>
        /// <param name="sessionToken">Token de sesión para la autenticación.</param>
        /// <param name="username">Nombre de usuario para identificar al solicitante.</param>
        /// <param name="fecha">Fecha del evento a modificar.</param>
        /// <param name="nuevoContenido">Nuevo contenido o descripción del evento.</param>
        /// <param name="esGrupal">Indica si el evento es grupal (true) o no (false).</param>
        public async Task ModificarEventoAsync(string sessionToken, string username, string fecha, string nuevoContenido, bool esGrupal)
        {
            try
            {
                // Construcción del paquete para modificar el evento, con los parámetros proporcionados
                string packet = ProtocolHelper.ConstructModificarEventoPacket(sessionToken, username, fecha, nuevoContenido, esGrupal);

                // Cifrado del paquete antes de enviarlo al servidor
                byte[] encryptedPacket = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(packet));

                // Enviar el paquete cifrado al servidor
                await writer.WriteLineAsync(Convert.ToBase64String(encryptedPacket));
                await writer.FlushAsync();

                // Leer la respuesta del servidor, que estará cifrada
                string encryptedResponseBase64 = await reader.ReadLineAsync();

                // Desencriptar la respuesta
                byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);
                byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);
                string response = Encoding.UTF8.GetString(decryptedResponse);

                // Procesar la respuesta recibida del servidor
                ProcessModificarEventoResponse(response);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                MessageBox.Show($"Error al modificar el evento: {ex.Message}");
            }
        }



        /// <summary>
        /// Procesa la respuesta recibida del servidor después de intentar modificar un evento.
        /// </summary>
        /// <param name="response">Respuesta del servidor tras la solicitud de modificación del evento.</param>
        private void ProcessModificarEventoResponse(string response)
        {
            // Verificar si la respuesta indica que la modificación fue exitosa
            if (response.StartsWith("426"))
            {
                // Confirmación de evento modificado con éxito
                MessageBox.Show("Evento modificado correctamente.");
            }
            // Verificar si la respuesta indica que los miembros del grupo fueron notificados
            else if (response.StartsWith("230"))
            {
                // Notificación para miembros del grupo
                MessageBox.Show("El evento ha sido modificado. Notificación enviada a los miembros del grupo.");
            }
            else
            {
                // Manejar errores específicos basados en la respuesta recibida
                switch (response)
                {
                    case "1104":
                        MessageBox.Show("Error: Fecha con formato incorrecto.");
                        break;
                    case "9":
                        MessageBox.Show("Error: No se pudo obtener el objeto de fecha.");
                        break;
                    case "1605":
                        MessageBox.Show("Error: Día no preinicializado.");
                        break;
                    case "1606":
                        MessageBox.Show("Error: Día no creado.");
                        break;
                    case "1601":
                        MessageBox.Show("Error: Objeto de agenda corrupto.");
                        break;
                    case "1402":
                        MessageBox.Show("Error: No perteneces al grupo.");
                        break;
                    case "1404":
                        MessageBox.Show("Error: El grupo no existe.");
                        break;
                    case "1403":
                        MessageBox.Show("Error: No tienes el bloqueo de edición.");
                        break;
                    default:
                        // Caso para errores desconocidos
                        MessageBox.Show("Error desconocido.");
                        break;
                }
            }
        }




        /// <summary>
        /// Realiza el bloqueo de edición de un día específico en el servidor.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario.</param>
        /// <param name="username">Nombre de usuario que realiza la solicitud.</param>
        /// <param name="fecha">Fecha del día que se desea bloquear.</param>
        /// <param name="nombreGrupo">Nombre del grupo al que pertenece el usuario.</param>
        public async Task BloquearDiaEdicionAsync(string sessionToken, string username, string fecha, string nombreGrupo)
        {
            // Construcción del paquete para bloquear el día en el servidor
            string packet = ProtocolHelper.ConstructBloquearDiaEdicionPacket(sessionToken, username, fecha, nombreGrupo);

            // Cifrado del paquete utilizando AES para garantizar la seguridad de la comunicación
            byte[] encryptedPacket = cryptoService.EncryptDataWithAES(Encoding.UTF8.GetBytes(packet));

            // Envío del paquete cifrado al servidor
            await writer.WriteLineAsync(Convert.ToBase64String(encryptedPacket));
            await writer.FlushAsync();

            // Leer la respuesta cifrada del servidor
            string encryptedResponseBase64 = await reader.ReadLineAsync();
            byte[] encryptedResponse = Convert.FromBase64String(encryptedResponseBase64);

            // Desencriptar la respuesta con AES
            byte[] decryptedResponse = cryptoService.DecryptDataWithAES(encryptedResponse);
            string response = Encoding.UTF8.GetString(decryptedResponse);

            // Procesar la respuesta del servidor
            ProcessBloquearDiaEdicionResponse(response);
        }






        /// <summary>
        /// Procesa la respuesta recibida del servidor después de intentar bloquear la edición de un día.
        /// </summary>
        /// <param name="response">Respuesta completa recibida del servidor.</param>
        private void ProcessBloquearDiaEdicionResponse(string response)
        {
            // Verificar si la respuesta es una confirmación exitosa de bloqueo de edición
            if (response.StartsWith("426"))
            {
                // Mostrar mensaje de éxito
                MessageBox.Show("El día ha sido bloqueado para edición.");
            }
            // Verificar si la respuesta indica que se envió una notificación a los miembros del grupo
            else if (response.StartsWith("230"))
            {
                // Mostrar mensaje de éxito con notificación
                MessageBox.Show("El día ha sido bloqueado para edición. Notificación enviada a los miembros del grupo.");
            }
            else
            {
                // Manejar diferentes errores basados en el código de respuesta
                switch (response)
                {
                    case "1104":
                        // Error: Fecha con formato incorrecto
                        MessageBox.Show("Error: Fecha con formato incorrecto.");
                        break;

                    case "1606":
                        // Error: El día no ha sido creado
                        MessageBox.Show("Error: Día no creado.");
                        break;

                    case "1404":
                        // Error: El grupo no existe
                        MessageBox.Show("Error: El grupo no existe.");
                        break;

                    case "1403":
                        // Error: El día ya está bloqueado para edición
                        MessageBox.Show("Error: El día ya está bloqueado para edición.");
                        break;

                    case "1601":
                        // Error: El objeto de agenda está corrupto
                        MessageBox.Show("Error: Objeto de agenda corrupto.");
                        break;

                    default:
                        // Mensaje de error desconocido
                        MessageBox.Show("Error desconocido.");
                        break;
                }
            }
        }




        /// <summary>
        /// Cierra la conexión con el servidor, liberando los recursos asociados.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                // Cerrar el escritor si está activo
                if (writer != null)
                {
                    writer.Close();  // Cierra el StreamWriter
                    writer = null;   // Asigna null al escritor para liberar recursos
                }

                // Cerrar el lector si está activo
                if (reader != null)
                {
                    reader.Close();  // Cierra el StreamReader
                    reader = null;   // Asigna null al lector para liberar recursos
                }

                // Cerrar el cliente (conexión TCP) si está activo
                if (client != null)
                {
                    client.Close();  // Cierra la conexión TCP
                    client = null;   // Asigna null al cliente para liberar recursos
                }
            }
            catch (Exception ex)
            {
                // Si ocurre un error durante la desconexión, mostrar el mensaje de error
                MessageBox.Show($"Error during disconnection: {ex.Message}");
            }
        }

    }
}
