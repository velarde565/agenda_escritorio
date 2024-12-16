using System;
using System.Linq;
using System.Windows;

namespace AgendaEscritorio.service

    /// <summary>
    /// Clase de utilidad para construir los paquetes de datos que se enviarán y recibirán a través de la red en el protocolo de comunicación.
    /// </summary>
{
    public static class ProtocolHelper
    {
        /// <summary>
        /// Genera un paquete para la solicitud de login.
        /// </summary>
        /// <param name="username">Nombre de usuario.</param>
        /// <param name="password">Contraseña.</param>
        /// <returns>El paquete de login en formato string.</returns>
        public static string ConstructLoginPacket(string username, string password)
        {
            // Longitud de los parámetros username y password
            string usernameLengthStr = username.Length.ToString("D2");
            string passwordLengthStr = password.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo
            return $"1{"01"}{usernameLengthStr}{username}{passwordLengthStr}{password}\n";
        }

        /// <summary>
        /// Genera un paquete para solicitar la modificación de los datos de un usuario.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="connectedUsername">Nombre de usuario conectado.</param>
        /// <param name="usernameToChange">Nombre de usuario cuyo dato se quiere cambiar.</param>
        /// <returns>El paquete para modificar los datos de usuario en formato string.</returns>
        public static string ConstructUserDataRequestPacket(string sessionToken, string connectedUsername, string usernameToChange)
        {
            // Longitudes de los parámetros
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string connectedUsernameLengthStr = connectedUsername.Length.ToString("D2");
            string usernameToChangeLengthStr = usernameToChange.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo
            return $"2{"05"}{tokenLengthStr}{sessionToken}{usernameToChangeLengthStr}{usernameToChange}{connectedUsernameLengthStr}{connectedUsername}\n";
        }

        /// <summary>
        /// Genera un paquete para el logout de un usuario.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario conectado.</param>
        /// <returns>El paquete de logout en formato string.</returns>
        public static string ConstructLogoutPacket(string sessionToken, string username)
        {
            // Longitudes de los parámetros
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameLengthStr = username.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo
            return $"1{"02"}{tokenLengthStr}{sessionToken}{usernameLengthStr}{username}\n";
        }

        /// <summary>
        /// Genera un paquete para cambiar el nombre completo de un usuario.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="usernameToChange">Nombre de usuario cuyo nombre completo se va a cambiar.</param>
        /// <param name="newFullName">Nuevo nombre completo.</param>
        /// <param name="connectedUsername">Nombre de usuario conectado (que puede ser el administrador).</param>
        /// <returns>El paquete para cambiar el nombre completo en formato string.</returns>
        public static string ConstructChangeFullNamePacket(string sessionToken, string usernameToChange, string newFullName, string connectedUsername)
        {
            // Longitudes de los parámetros
            string sessionTokenOffset = sessionToken.Length.ToString("D2");
            string usernameToChangeOffset = usernameToChange.Length.ToString("D2");
            string newFullNameOffset = newFullName.Length.ToString("D2");
            string connectedUsernameOffset = connectedUsername.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo
            return $"2{"02"}{sessionTokenOffset}{sessionToken}{usernameToChangeOffset}{usernameToChange}{newFullNameOffset}{newFullName}{connectedUsernameOffset}{connectedUsername}\n";
        }

        /// <summary>
        /// Genera un paquete para cambiar la fecha de nacimiento de un usuario.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="usernameToChange">Nombre de usuario cuyo fecha de nacimiento se va a cambiar.</param>
        /// <param name="newBirthDate">Nueva fecha de nacimiento.</param>
        /// <param name="connectedUsername">Nombre de usuario conectado.</param>
        /// <returns>El paquete para cambiar la fecha de nacimiento en formato string.</returns>
        public static string ConstructChangeBirthDatePacket(string sessionToken, string usernameToChange, string newBirthDate, string connectedUsername)
        {
            // Longitudes de los parámetros
            string sessionTokenOffset = sessionToken.Length.ToString("D2");
            string usernameToChangeOffset = usernameToChange.Length.ToString("D2");
            string newBirthDateOffset = newBirthDate.Length.ToString("D2");
            string connectedUsernameOffset = connectedUsername.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo
            string response = $"2{"03"}{sessionTokenOffset}{sessionToken}{connectedUsernameOffset}{connectedUsername}{usernameToChangeOffset}{usernameToChange}{newBirthDateOffset}{newBirthDate}\n";

            // Mensaje de verificación temporal
            MessageBox.Show(response);

            return response;
        }

        /// <summary>
        /// Genera un paquete para cambiar la contraseña de un usuario.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="usernameToEdit">Nombre de usuario cuyo contraseña se va a cambiar.</param>
        /// <param name="currentPassword">Contraseña actual.</param>
        /// <param name="newPassword">Nueva contraseña.</param>
        /// <param name="connectedUsername">Nombre de usuario conectado (quien está realizando el cambio).</param>
        /// <returns>El paquete para cambiar la contraseña en formato string.</returns>
        public static string ConstructChangePasswordPacket(string sessionToken, string usernameToEdit, string currentPassword, string newPassword, string connectedUsername)
        {
            // Longitudes de los parámetros
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameToEditLengthStr = usernameToEdit.Length.ToString("D2");
            string currentPasswordLengthStr = currentPassword.Length.ToString("D2");
            string newPasswordLengthStr = newPassword.Length.ToString("D2");
            string connectedUsernameLengthStr = connectedUsername.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo
            string response = $"2{"01"}{tokenLengthStr}{sessionToken}" +
                              $"{usernameToEditLengthStr}{usernameToEdit}" +
                              $"{currentPasswordLengthStr}{currentPassword}" +
                              $"{newPasswordLengthStr}{newPassword}" +
                              $"{connectedUsernameLengthStr}{connectedUsername}\n";

            // Mensaje de verificación temporal
            MessageBox.Show(response);

            return response;
        }

        /// <summary>
        /// Genera un paquete para cambiar otros datos de un usuario (como dirección, teléfono, etc.).
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="usernameToChange">Nombre de usuario cuyo otros datos se van a cambiar.</param>
        /// <param name="connectedUsername">Nombre de usuario conectado.</param>
        /// <param name="newOtherData">Nuevo dato (por ejemplo, teléfono o dirección).</param>
        /// <returns>El paquete para cambiar otros datos en formato string.</returns>
        public static string ConstructChangeOtherDataPacket(string sessionToken, string usernameToChange, string connectedUsername, string newOtherData)
        {
            // Verificación temporal de valores
            Console.WriteLine($"Connected Username: '{connectedUsername}', Length: {connectedUsername.Length}");

            // Longitudes de los parámetros
            string tokenOffset = $"{sessionToken.Length:D2}";
            string usernameOffset = $"{connectedUsername.Length:D2}";
            string targetUsernameOffset = $"{usernameToChange.Length:D2}";
            string otherDataOffset = $"{newOtherData.Length:D4}"; // Zero-fill a 4 caracteres

            // Construcción del paquete siguiendo el protocolo
            string response = $"2{"04"}{tokenOffset}{sessionToken}{targetUsernameOffset}{usernameToChange}{usernameOffset}{connectedUsername}{otherDataOffset}{newOtherData}";

            // Mensaje de verificación temporal
            MessageBox.Show(response);

            return response;
        }

        /// <summary>
        /// Genera un paquete para eliminar un usuario.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="usernameToDelete">Nombre de usuario a eliminar.</param>
        /// <param name="connectedUsername">Nombre de usuario conectado.</param>
        /// <returns>El paquete para eliminar el usuario en formato string.</returns>
        public static string ConstructDeleteUserPacket(string sessionToken, string usernameToDelete, string connectedUsername)
        {
            // Longitudes de los parámetros
            string sessionTokenOffset = sessionToken.Length.ToString("D2");
            string usernameToDeleteOffset = usernameToDelete.Length.ToString("D2");
            string connectedUsernameOffset = connectedUsername.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo
            string packet = $"2{"07"}{sessionTokenOffset}{sessionToken}{connectedUsernameOffset}{connectedUsername}{usernameToDeleteOffset}{usernameToDelete}\n";

            return packet;
        }

        /// <summary>
        /// Construye un paquete para crear un nuevo usuario con los datos proporcionados.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario conectado.</param>
        /// <param name="connectedUsername">El nombre de usuario del usuario conectado.</param>
        /// <param name="newUsername">El nombre de usuario para el nuevo usuario.</param>
        /// <param name="password">La contraseña del nuevo usuario.</param>
        /// <param name="fullName">El nombre completo del nuevo usuario.</param>
        /// <param name="birthDate">La fecha de nacimiento del nuevo usuario.</param>
        /// <param name="otrosDatos">Otros datos adicionales del nuevo usuario.</param>
        /// <param name="rolPermisos">El rol y permisos del nuevo usuario.</param>
        /// <param name="apodo">El apodo del nuevo usuario.</param>
        /// <returns>El paquete en formato string listo para ser enviado.</returns>
        public static string ConstructCreateUserPacket(string sessionToken, string connectedUsername, string newUsername, string password, string fullName, string birthDate, string otrosDatos, string rolPermisos, string apodo)
        {
            // Calcular los offsets para cada campo de acuerdo a su longitud.
            string sessionTokenOffset = sessionToken.Length.ToString("D2");
            string connectedUsernameOffset = connectedUsername.Length.ToString("D2");
            string newUsernameOffset = newUsername.Length.ToString("D2");
            string passwordOffset = password.Length.ToString("D2");
            string fullNameOffset = fullName.Length.ToString("D2");
            string birthDateOffset = birthDate.Length.ToString("D2");
            string otrosDatosOffset = otrosDatos.Length.ToString("D4");
            string rolPermisosOffset = rolPermisos.Length.ToString("D2");
            string apodoOffset = apodo.Length.ToString("D2");

            // Construcción del paquete de datos.
            string packet = $"2{"06"}{sessionTokenOffset}{sessionToken}" +
                            $"{connectedUsernameOffset}{connectedUsername}" +
                            $"{newUsernameOffset}{newUsername}" +
                            $"{passwordOffset}{password}" +
                            $"{fullNameOffset}{fullName}" +
                            $"{birthDateOffset}{birthDate}" +
                            $"{otrosDatosOffset}{otrosDatos}" +
                            $"{rolPermisosOffset}{rolPermisos}" +
                            $"{apodoOffset}{apodo}\n";

            // Construcción del mensaje descompuesto por cada campo.
            string message = $"Protocolo: 2\n" +
                             $"Acción: 06\n" +
                             $"Session Token Offset: {sessionTokenOffset}\n" +
                             $"Session Token: {sessionToken}\n" +
                             $"Connected Username Offset: {connectedUsernameOffset}\n" +
                             $"Connected Username: {connectedUsername}\n" +
                             $"New Username Offset: {newUsernameOffset}\n" +
                             $"New Username: {newUsername}\n" +
                             $"Password Offset: {passwordOffset}\n" +
                             $"Password: {password}\n" +
                             $"Full Name Offset: {fullNameOffset}\n" +
                             $"Full Name: {fullName}\n" +
                             $"Birth Date Offset: {birthDateOffset}\n" +
                             $"Birth Date: {birthDate}\n" +
                             $"Otros Datos Offset: {otrosDatosOffset}\n" +
                             $"Otros Datos: {otrosDatos}\n" +
                             $"Rol Permisos Offset: {rolPermisosOffset}\n" +
                             $"Rol Permisos: {rolPermisos}\n" +
                             $"Apodo Offset: {apodoOffset}\n" +
                             $"Apodo: {apodo}";

            // Mostrar el mensaje en MessageBox para visualización.
            MessageBox.Show(message, "Detalle del Paquete");

            return packet;
        }


        /// <summary>
        /// Construye un paquete para añadir permisos a un rol para un usuario específico.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario conectado.</param>
        /// <param name="connectedUsername">El nombre de usuario del usuario conectado.</param>
        /// <param name="roleName">El nombre del rol al que se añadirán los permisos.</param>
        /// <param name="permissions">Los permisos que se asignarán al rol, en formato de valores separados por comas.</param>
        /// <returns>El paquete en formato string listo para ser enviado.</returns>
        /// <exception cref="ArgumentException">Lanzada si los permisos no son exactamente 7 valores separados por comas.</exception>
        public static string ConstructAddPermissionPacket(string sessionToken, string connectedUsername, string roleName, string permissions)
        {
            // Asegúrate de que los permisos sean exactamente 7 valores separados por comas.
            if (permissions.Split(',').Length != 7)
            {
                throw new ArgumentException("Los permisos deben ser exactamente 7 valores separados por comas.");
            }

            // Calcular los offsets para cada campo.
            string sessionTokenOffset = sessionToken.Length.ToString("D2");
            string connectedUsernameOffset = connectedUsername.Length.ToString("D2");
            string roleNameOffset = roleName.Length.ToString("D2");
            string permissionsOffset = permissions.Length.ToString("D2");

            // Construir el paquete
            return $"2{"09"}{sessionTokenOffset}{sessionToken}{connectedUsernameOffset}{connectedUsername}{roleNameOffset}{roleName}{permissionsOffset}{permissions}\n";
        }

        /// <summary>
        /// Construye un paquete para obtener los permisos de un usuario específico.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario conectado.</param>
        /// <param name="connectedUsername">El nombre de usuario del usuario conectado.</param>
        /// <returns>El paquete en formato string listo para ser enviado.</returns>
        public static string ConstructGetPermissionsPacket(string sessionToken, string connectedUsername)
        {
            // Calcular los offsets para cada campo.
            string sessionTokenOffset = sessionToken.Length.ToString("D2");
            string connectedUsernameOffset = connectedUsername.Length.ToString("D2");

            // Construir el paquete
            return $"2{"12"}{sessionTokenOffset}{sessionToken}{connectedUsernameOffset}{connectedUsername}\n";
        }



        /// <summary>
        /// Construye un paquete para editar los permisos de un usuario.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario al que se le editarán los permisos.</param>
        /// <param name="rol">Rol asignado al usuario.</param>
        /// <param name="permisos">Arreglo de permisos que se asignarán al usuario (7 elementos booleanos).</param>
        /// <returns>Cadena que representa el paquete de datos para editar los permisos de un usuario.</returns>
        /// <exception cref="ArgumentException">Lanzado si los permisos no tienen exactamente 7 valores (0 o 1).</exception>
        public static string ConstructEditPermissionsPacket(string sessionToken, string username, string rol, bool[] permisos)
        {
            // Calcular los offsets para el token, nombre de usuario y rol
            string tokenOffset = sessionToken.Length.ToString("D2"); // Aseguramos que el offset tenga dos dígitos
            string usernameOffset = username.Length.ToString("D2");  // Longitud del nombre de usuario
            string rolOffset = rol.Length.ToString("D2");            // Longitud del rol

            // Convertir los permisos en un string de 7 caracteres, con 1 para los permisos marcados y 0 para los no marcados
            string permisosString = string.Join("", permisos.Select(p => p ? "1" : "0"));

            // Verificar que los permisos tengan exactamente 7 valores
            if (permisosString.Length != 7)
            {
                throw new ArgumentException("Los permisos deben estar bien formateados con 7 valores (0 o 1).");
            }

            // Construir y devolver el paquete final
            return $"2{"10"}{tokenOffset}{sessionToken}{usernameOffset}{username}{rolOffset}{rol}{permisosString}";
        }



        /// <summary>
        /// Construye un paquete para activar o desactivar el Modo Gestión.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario del usuario conectado.</param>
        /// <param name="activarModoGestion">Indica si el Modo Gestión debe ser activado (true) o desactivado (false).</param>
        /// <returns>Cadena que representa el paquete de datos para activar o desactivar el Modo Gestión.</returns>
        public static string ConstructModoGestionPacket(string sessionToken, string username, bool activarModoGestion)
        {
            // Calcular los offsets para el token y el nombre de usuario
            string tokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token (con dos dígitos)
            string usernameOffset = username.Length.ToString("D2");  // Longitud del nombre de usuario (con dos dígitos)

            // Determinar la acción (13 para activar, 14 para desactivar)
            int accion = activarModoGestion ? 13 : 14;

            // Construir y devolver el paquete final para la activación/desactivación del Modo Gestión
            return $"2{accion}{tokenOffset}{sessionToken}{usernameOffset}{username}";
        }



        /// <summary>
        /// Construye un paquete para crear un nuevo evento en el sistema, incluyendo la fecha, contenido, tags y detalles sobre si es grupal.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="fecha">Fecha del evento en formato de cadena (por ejemplo, "2024-11-20").</param>
        /// <param name="contenido">Descripción o contenido del evento.</param>
        /// <param name="tags">Etiquetas asociadas al evento. Puede ser una cadena vacía si no hay etiquetas.</param>
        /// <param name="esGrupal">Indica si el evento es grupal (true) o individual (false).</param>
        /// <param name="nombreGrupo">Nombre del grupo asociado al evento, si es un evento grupal.</param>
        /// <returns>Cadena que representa el paquete de datos para crear un evento.</returns>
        public static string ConstructCreateDayPacket(string sessionToken, string username, string fecha, string contenido, string tags, bool esGrupal, string nombreGrupo)
        {
            // Calcular los offsets para el token de sesión, usuario, fecha, contenido y etiquetas
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del nombre del usuario
            string fechaOffset = fecha.Length.ToString("D2");                // Longitud de la fecha
            string contenidoOffset = contenido.Length.ToString("D4");        // Longitud del contenido (cuatro dígitos)
            string tagsOffset = string.IsNullOrEmpty(tags) ? "0000" : tags.Length.ToString("D4"); // Longitud de los tags (cuatro dígitos)

            // Establecer un flag (1 para grupal, 0 para individual)
            string grupalFlag = esGrupal ? "1" : "0";

            // Si el evento es grupal, calcular el offset y el nombre del grupo
            string groupNameOffset = esGrupal && !string.IsNullOrEmpty(nombreGrupo)
                                     ? nombreGrupo.Length.ToString("D2") // Longitud del nombre del grupo
                                     : "";

            string groupName = esGrupal && !string.IsNullOrEmpty(nombreGrupo)
                               ? nombreGrupo // Nombre del grupo si es un evento grupal
                               : "";

            // Añadir los 2 bytes adicionales requeridos por la especificación
            string unusedBytes = "00"; 

            // Construir y devolver el paquete final para crear el evento
            return $"4{"06"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}{fechaOffset}{fecha}{contenidoOffset}{contenido}{tagsOffset}{tags}{unusedBytes}{grupalFlag}{groupNameOffset}{groupName}\n";
        }




        public static string ConstructCreateGroupPacket(string sessionToken, string username, string nombreGrupo)
        {
            // Calcular los offsets
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del usuario conectado
            string groupNameOffset = nombreGrupo.Length.ToString("D2");      // Longitud del nombre del grupo

            // Construir y devolver el paquete
            return $"2{"15"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}{groupNameOffset}{nombreGrupo}\n";
        }


        public static string ConstructDeleteGroupPacket(string sessionToken, string username, string nombreGrupo)
        {
            // Calcular los offsets
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del usuario conectado
            string groupNameOffset = nombreGrupo.Length.ToString("D2");      // Longitud del nombre del grupo

            // Construir el paquete
            string packet = $"2{"16"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}{groupNameOffset}{nombreGrupo}\n";

            // Mostrar el paquete generado en un MessageBox
            MessageBox.Show($"Paquete generado:\n{packet}", "Delete Group Packet", MessageBoxButton.OK, MessageBoxImage.Information);

            // Retornar el paquete
            return packet;
        }



        // Método para construir el paquete de solicitud de grupos propios
        public static string ConstructViewOwnedGroupsPacket(string sessionToken, string username)
        {
            // Calcular los offsets
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del usuario conectado

            // Construir el paquete
            string packet = $"2{"18"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}\n";

            // Mostrar el paquete generado en un MessageBox
            MessageBox.Show($"Paquete generado:\n{packet}", "View Owned Groups Packet", MessageBoxButton.OK, MessageBoxImage.Information);

            // Retornar el paquete
            return packet;
        }



        // Método para construir el paquete de solicitud de grupos donde el usuario es miembro
        public static string ConstructViewMembershipGroupsPacket(string sessionToken, string username)
        {
            // Calcular los offsets
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del usuario conectado

            // Construir y devolver el paquete
            return $"2{"19"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}\n";
        }



        // Método para construir el paquete de solicitud de ver todos los grupos
        public static string ConstructViewAllGroupsPacket(string sessionToken, string username)
        {
            // Calcular los offsets
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del usuario conectado

            // Construir y devolver el paquete
            return $"2{"20"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}\n";
        }



        public static string ConstructShowMonthPacket(string sessionToken, string username)
        {
            // Calcular los offsets basados en las longitudes de los datos
            string sessionTokenOffset = sessionToken.Length.ToString("D2");
            string usernameOffset = username.Length.ToString("D2");

            // El conjunto de 2 bytes (cualquier valor, en este caso 00) 
            string filler = "00";

            // Crear el paquete con la estructura adecuada
            return $"4{"01"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}{filler}109IOCtest11\n";


        }





        /// <summary>
        /// Extrae la acción y el mensaje de una respuesta de paquete.
        /// </summary>
        /// <param name="response">La respuesta del paquete que contiene la acción y el mensaje.</param>
        /// <returns>Un tuple que contiene un entero (acción) y una cadena (mensaje).</returns>
        /// <exception cref="FormatException">Se lanza si la acción no tiene el formato esperado.</exception>
        public static Tuple<int, string> LecturaPartesPaquete(string response)
        {
            // Extraemos los primeros tres caracteres de la respuesta, que corresponden a la acción
            string actionString = response.Substring(0, 3);

            // Intentamos convertir la cadena de acción en un entero
            if (int.TryParse(actionString, out int action))
            {
                // Si la longitud de la respuesta es mayor a 3, el resto es el mensaje
                string message = response.Length > 3 ? response.Substring(3) : string.Empty;

                // Retornamos un tuple con la acción (entero) y el mensaje (cadena)
                return new Tuple<int, string>(action, message);
            }

            // Si no se pudo convertir la acción en un entero, lanzamos una excepción
            throw new FormatException("Invalid action format in response.");
        }

    }
}
