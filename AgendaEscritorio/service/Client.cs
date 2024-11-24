using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
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
        /// Asynchronously sends a login request to the server with the provided username and password.
        /// </summary>
        /// <param name="username">The username for login.</param>
        /// <param name="password">The password for login.</param>
        /// <returns>Returns true if the login was successful, false otherwise.</returns>
        public async Task<bool> SendLoginAsync(string username, string password)
        {
            this.username = username; // Save the username for later use
            this.username2 = username; // Assign the username to a secondary variable (possibly for backup or logging)

            try
            {
                // Construct the login packet using a helper method
                string loginPacket = ProtocolHelper.ConstructLoginPacket(username, password);
                Console.WriteLine($"Login Packet Sent: {loginPacket}");

                // Send the login packet to the server asynchronously
                await writer.WriteLineAsync(loginPacket);
                await writer.FlushAsync();

                // Wait for the server's response
                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                // Process the login response from the server
                return await ProcessLoginResponseAsync(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the login process
                MessageBox.Show($"Error sending login: {ex.Message}");
                return false; // Return false if an error occurs
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
                case 103:
                    // If the action is 103 (successful login), extract the session token
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
                        // If the token could not be extracted, show an error
                        MessageBox.Show("Error: Could not extract token.");
                        return false; // Return false if the token extraction failed
                    }
                case 9:
                    // If the action code is 9, it indicates a login error
                    MessageBox.Show($"Login error: {message}");
                    return false; // Return false for login error
                default:
                    // If the action is not recognized, show an unexpected response error
                    MessageBox.Show($"Unexpected response: {response}");
                    return false; // Return false for unexpected responses
            }
        }

        /// <summary>
        /// Asynchronously requests the user data from the server using the session token and username.
        /// </summary>
        /// <param name="sessionToken">The session token to authenticate the request.</param>
        /// <param name="username">The username for which to request data.</param>
        /// <param name="nombre2">A secondary username (used for additional processing or validation).</param>
        public async Task RequestUserDataAsync(string sessionToken, string username, string nombre2)
        {
            try
            {
                // Construct the user data request packet
                string requestPacket = ProtocolHelper.ConstructUserDataRequestPacket(sessionToken, username, nombre2);

                // Send the request packet to the server asynchronously
                await writer.WriteLineAsync(requestPacket);
                await writer.FlushAsync();

                // Wait for the server's response
                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                // Process the user data response from the server
                ProcessUserDataResponse(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the user data request process
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
        /// Sends a logout request to the server.
        /// </summary>
        public async Task SendLogoutAsync()
        {
            try
            {
                // Construct the logout packet using the session token and username
                string logoutPacket = ProtocolHelper.ConstructLogoutPacket(sessionToken, username);
                MessageBox.Show($"Logout Packet Sent: {logoutPacket}");

                // Check if the writer is initialized before sending the packet
                if (writer == null)
                {
                    MessageBox.Show("Error: writer is not initialized.");
                    return;
                }

                // Send the logout packet to the server asynchronously
                await writer.WriteLineAsync(logoutPacket);
                await writer.FlushAsync();

                // Wait for the server's response
                string response = await reader.ReadLineAsync();
                MessageBox.Show($"Server Response: {response}");

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
        /// Asynchronously sends a request to change the user's full name on the server.
        /// </summary>
        /// <param name="sessionToken">The session token for authentication.</param>
        /// <param name="usernameToChange">The username whose full name is to be changed.</param>
        /// <param name="newFullName">The new full name for the user.</param>
        /// <returns>Returns a Task representing the asynchronous operation.</returns>
        public async Task RequestChangeFullNameAsync(string sessionToken, string usernameToChange, string newFullName)
        {
            try
            {
                // Construct the packet for changing the full name
                string requestPacket = ProtocolHelper.ConstructChangeFullNamePacket(sessionToken, usernameToChange, newFullName, username);

                // Send the request packet to the server asynchronously
                await writer.WriteLineAsync(requestPacket);
                await writer.FlushAsync();

                // Wait for the server's response
                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                // Process the response from the server
                ProcessChangeFullNameResponse(response);
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the request for changing the full name
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
        /// Solicita la adición de un permiso a un usuario específico.
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
                // Construir el paquete de solicitud utilizando un helper de protocolo
                string requestPacket = ProtocolHelper.ConstructAddPermissionPacket(sessionToken, username, roleToAdd, permissions);

                // Enviar la solicitud al servidor utilizando el escritor asíncrono
                await writer.WriteLineAsync(requestPacket);
                await writer.FlushAsync();

                // Leer la respuesta del servidor
                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                // Procesar la respuesta recibida del servidor
                ProcessAddPermissionResponse(response);
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar un mensaje con la excepción capturada
                MessageBox.Show($"Error al solicitar la adición de permiso: {ex.Message}");
            }
        }




        /// <summary>
        /// Procesa la respuesta del servidor tras la solicitud de adición de permiso.
        /// </summary>
        /// <param name="response">La respuesta recibida del servidor.</param>
        private void ProcessAddPermissionResponse(string response)
        {
            // Mostrar la respuesta completa en un MessageBox
            MessageBox.Show(response);

            // Verificar si la respuesta tiene una longitud mínima (3 caracteres)
            if (response.Length < 3)
            {
                MessageBox.Show("Respuesta demasiado corta.");
                return;
            }

            // Extraer el protocolo y la acción de la respuesta
            char protocol = response[0];
            int action = int.Parse(response.Substring(1, 2));

            // Verificar si el protocolo es '2' y la acción es '9' (Afegeix permís)
            if (protocol != '2' || action != 9)
            {
                MessageBox.Show($"Protocolo o acción inesperada: {protocol}, {action}");
                return;
            }

            // Extraer el token de la respuesta comenzando desde el índice 3
            int index = 3;
            string token = DataExtractor.ExtractData(response, ref index);

            // Verificar si el token extraído coincide con el token de la sesión
            if (token == SessionToken)
            {
                // Si el token es correcto, mostrar mensaje de éxito
                MessageBox.Show("Permiso agregado exitosamente.");
            }
            else
            {
                // Si el token no coincide, mostrar mensaje de error
                MessageBox.Show("Token incorrecto o acción no permitida.");
            }
        }




        /// <summary>
        /// Solicita al servidor los permisos de un usuario específico.
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

                // Enviar la solicitud al servidor
                await writer.WriteLineAsync(requestPacket);
                await writer.FlushAsync();

                // Lista para almacenar todas las respuestas del servidor
                List<string> responses = new List<string>();

                // Establecer un tiempo máximo de espera para recibir respuestas (5 segundos)
                int timeout = 5000; // 5 segundos (ajustar según sea necesario)

                // Crear una tarea que se complete tras el timeout
                var timeoutTask = Task.Delay(timeout);

                // Leer las respuestas del servidor y agregarlas a la lista
                string response;
                while (true)
                {
                    // Esperar a que se complete la lectura o el timeout, lo que ocurra primero
                    var readTask = reader.ReadLineAsync();
                    var completedTask = await Task.WhenAny(readTask, timeoutTask);

                    if (completedTask == readTask && readTask.Result != null)
                    {
                        // Si se recibe una respuesta, agregarla a la lista
                        response = readTask.Result;
                        responses.Add(response);
                    }
                    else
                    {
                        // Si el timeout ocurre, salir del bucle
                        break;
                    }
                }

                // Mostrar mensaje indicando que todas las respuestas fueron recibidas
                MessageBox.Show("Respuesta del servidor: todo bien");

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
                // Verificar si el offset del nombre del rol tiene 1 o 2 dígitos
                int roleNameOffsetLength = char.IsDigit(response[index + 1]) ? 2 : 1; // Verificar si el segundo carácter es un dígito
                int roleNameOffset = int.Parse(response.Substring(index, roleNameOffsetLength)); // Obtener el offset del nombre del rol
                index += roleNameOffsetLength;
                string roleName = response.Substring(index, roleNameOffset); // Extraer el nombre del rol
                index += roleNameOffset;

                // Extraer los permisos (longitud fija de 7 caracteres según el ejemplo proporcionado)
                int permissionsLength = 7;
                if (response.Length < index + permissionsLength)
                    throw new Exception("La longitud de los permisos es inválida.");
                string permissions = response.Substring(index, permissionsLength); // Extraer los permisos
                index += permissionsLength;

                // Almacenar el nombre del rol y sus permisos en el diccionario
                if (!rolesAndPermissions.ContainsKey(roleName))
                {
                    // Si el rol no está presente, agregarlo con los permisos
                    rolesAndPermissions.Add(roleName, permissions);
                }
                else
                {
                    // Si el rol ya existe, actualizar los permisos
                    rolesAndPermissions[roleName] = permissions;
                }
            }

            // Retornar el diccionario con todos los roles y sus permisos procesados
            return rolesAndPermissions;
        }




        /// <summary>
        /// Envia una solicitud al servidor para editar los permisos de un usuario.
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
                // Esta función crea el paquete de solicitud para editar permisos
                string paquete = ProtocolHelper.ConstructEditPermissionsPacket(sessionToken, username, rol, permisos);
                Console.WriteLine($"Sending Edit Permissions Packet: {paquete}");

                // Enviar el paquete al servidor de forma asíncrona
                // Se utiliza 'WriteLineAsync' para enviar el paquete y 'FlushAsync' para asegurar que se envíe
                await writer.WriteLineAsync(paquete);
                await writer.FlushAsync();

                // Leer la respuesta del servidor
                // 'ReadLineAsync' lee la respuesta una vez que el servidor la envía
                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                // Verificar si la respuesta del servidor es un código numérico válido
                // Si la respuesta se puede convertir a un número entero, se devuelve ese código
                if (int.TryParse(response, out int responseCode))
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
        /// Envia una solicitud al servidor para activar o desactivar el modo gestión.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario conectado.</param>
        /// <param name="username">El nombre del usuario que activa o desactiva el modo gestión.</param>
        /// <param name="activarModoGestion">Indica si se debe activar o desactivar el modo gestión (true = activar, false = desactivar).</param>
        /// <returns>Un código de respuesta del servidor (éxito o error).</returns>
        public async Task<int> SendModoGestionAsync(string sessionToken, string username, bool activarModoGestion)
        {
            try
            {
                // Construir el paquete de solicitud utilizando el Helper
                // Esta función crea el paquete necesario para activar o desactivar el modo gestión
                string paquete = ProtocolHelper.ConstructModoGestionPacket(sessionToken, username, activarModoGestion);
                Console.WriteLine($"Sending Modo Gestion Packet: {paquete}");

                // Enviar el paquete al servidor de manera asíncrona
                await writer.WriteLineAsync(paquete);
                await writer.FlushAsync();

                // Leer la respuesta del servidor
                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                // Verificar si la respuesta es un código numérico
                if (int.TryParse(response, out int responseCode))
                {
                    return responseCode;  // Retorna el código de respuesta (éxito o error)
                }
                else
                {
                    // Si la respuesta no es válida (no es un número entero)
                    MessageBox.Show("Error: Respuesta del servidor inválida.");
                    return -1;  // Error en la respuesta
                }
            }
            catch (Exception ex)
            {
                // Si ocurre algún error durante el envío o procesamiento
                MessageBox.Show($"Error al enviar el paquete para activar/desactivar el modo gestión: {ex.Message}");
                return -1;  // Error en el envío de la solicitud
            }
        }






        /// <summary>
        /// Envía una solicitud asincrónica al servidor para cambiar otros datos de un usuario específico.
        /// </summary>
        /// <param name="sessionToken">El token de sesión que identifica al usuario conectado.</param>
        /// <param name="usernameToChange">El nombre de usuario cuyo "otros datos" serán modificados.</param>
        /// <param name="newOtherData">Los nuevos datos que se desean asignar al usuario.</param>
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
                // Mostrar un mensaje de error si ocurre alguna excepción
                MessageBox.Show($"Error al sol·licitar el canvi d'altres dades: {ex.Message}");
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
        /// Solicita el cambio de contraseña de un usuario específico.
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
                MessageBox.Show("Confirmacion cambio de contraseña");
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
        public async Task RequestCreateUserAsync(string sessionToken, string username, string nombreUsuario, string password, string nombreCompleto, string fechaNacimiento, string otrosDatos, string rolPermisos)
        {
            try
            {
                // Construir el paquete de solicitud de creación usando ProtocolHelper
                string requestPacket = ProtocolHelper.ConstructCreateUserPacket(sessionToken, username, nombreUsuario, password, nombreCompleto, fechaNacimiento, otrosDatos, rolPermisos);

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
        /// Envía una solicitud asíncrona para crear un nuevo evento (día) en el servidor.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario para autenticar la solicitud.</param>
        /// <param name="fecha">La fecha del evento que se va a crear.</param>
        /// <param name="contenido">El contenido o descripción del evento.</param>
        /// <param name="tags">Etiquetas asociadas al evento.</param>
        /// <param name="esGrupal">Indica si el evento es grupal (true) o individual (false).</param>
        /// <param name="nombreGrupo">El nombre del grupo al que pertenece el evento (si es grupal).</param>
        public async Task RequestCreateDayAsync(string sessionToken, string fecha, string contenido, string tags, bool esGrupal, string nombreGrupo)
        {
            try
            {
                // Construir el paquete de solicitud
                string requestPacket = ProtocolHelper.ConstructCreateDayPacket(sessionToken, fecha, contenido, tags, esGrupal, nombreGrupo);

                // Enviar la solicitud
                await writer.WriteLineAsync(requestPacket);
                await writer.FlushAsync();

                // Leer la respuesta
                string response = await reader.ReadLineAsync();
                Console.WriteLine($"Server Response: {response}");

                // Procesar la respuesta
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
