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
        private static bool modoGestionActivo = false;
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
        /// Construye un paquete de desconexión según el protocolo especificado, incluyendo el token de sesión, el nombre de usuario y la contraseña.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario.</param>
        /// <param name="username">El nombre de usuario del cliente.</param>
        /// <param name="password">La contraseña del usuario.</param>
        /// <returns>Una cadena que representa el paquete de desconexión listo para ser enviado al servidor.</returns>
        /// <remarks>
        /// El paquete se construye con una estructura específica, donde cada parámetro tiene una longitud especificada en dos dígitos 
        /// y el paquete sigue un formato predefinido para asegurar la correcta comunicación con el servidor.
        /// </remarks>
        public static string ConstructShutdownPacket(string sessionToken, string username, string password)
        {
            // Longitudes de los parámetros (en formato de dos dígitos)
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameLengthStr = username.Length.ToString("D2");
            string passwordLengthStr = password.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo
            string packet = $"1{"05"}{tokenLengthStr}{sessionToken}{usernameLengthStr}{username}{passwordLengthStr}{password}\n";

            // Mostrar el paquete en un MessageBox
            //MessageBox.Show("Paquete a enviar:\n" + packet);

            return packet;
        }



        /// <summary>
        /// Construye un paquete que contiene información sobre el usuario, siguiendo un formato de protocolo específico.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario.</param>
        /// <param name="username">El nombre de usuario del cliente.</param>
        /// <param name="infoSobre">La información adicional sobre el usuario o el contexto.</param>
        /// <returns>Una cadena que representa el paquete con la información sobre el usuario y su sesión, listo para ser enviado al servidor.</returns>
        /// <remarks>
        /// El paquete se construye concatenando los valores de los parámetros, precedidos por sus longitudes en dos dígitos. 
        /// Este formato es parte de un protocolo predefinido para asegurar que el servidor pueda interpretar correctamente 
        /// los datos recibidos. La longitud de cada parámetro es codificada antes del propio dato.
        /// </remarks>
        public static string ConstructInfoSobrePacket(string sessionToken, string username, string infoSobre)
        {
            // Longitudes de los parámetros (en formato de dos dígitos)
            string tokenLengthStr = sessionToken.Length.ToString("D2"); // Longitud del token (con 2 dígitos)
            string usernameLengthStr = username.Length.ToString("D2"); // Longitud del nombre de usuario (con 2 dígitos)
            string infoSobreLengthStr = infoSobre.Length.ToString("D2"); // Longitud de la info sobre (con 2 dígitos)

            // Construcción del paquete siguiendo el protocolo
            return $"1{"03"}{tokenLengthStr}{sessionToken}{usernameLengthStr}{username}{infoSobreLengthStr}{infoSobre}\n";
        }



        /// <summary>
        /// Construye un paquete para mostrar la lista de usuarios, siguiendo un formato de protocolo específico.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que realiza la solicitud.</param>
        /// <param name="username">El nombre de usuario del cliente que realiza la solicitud.</param>
        /// <returns>Una cadena que representa el paquete con la información necesaria para mostrar los usuarios, listo para ser enviado al servidor.</returns>
        /// <remarks>
        /// El paquete se construye concatenando los valores de los parámetros, precedidos por sus longitudes en dos dígitos.
        /// El paquete incluye un identificador de acción y el token de sesión con el nombre de usuario del cliente.
        /// Este formato asegura que el servidor pueda interpretar correctamente los datos recibidos.
        /// </remarks>
        public static string ConstructShowUsersPacket(string sessionToken, string username)
        {
            // Longitudes de los parámetros (en formato de dos dígitos)
            string tokenLengthStr = sessionToken.Length.ToString("D2"); // Longitud del token (con 2 dígitos)
            string usernameLengthStr = username.Length.ToString("D2"); // Longitud del nombre de usuario (con 2 dígitos)

            // Construcción del paquete siguiendo el protocolo
            return $"2{"08"}{tokenLengthStr}{sessionToken}{usernameLengthStr}{username}\n";
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
            //MessageBox.Show(response);

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
            //MessageBox.Show(response);

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
            //MessageBox.Show(response);

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
        /// Construye un paquete para eliminar un rol de un usuario, siguiendo el formato del protocolo específico.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que está realizando la solicitud.</param>
        /// <param name="connectedUsername">El nombre de usuario del cliente que está conectado y realizando la solicitud.</param>
        /// <param name="roleToDelete">El nombre del rol que se va a eliminar.</param>
        /// <returns>Una cadena que representa el paquete con los parámetros necesarios para la eliminación del rol, listo para ser enviado al servidor.</returns>
        /// <remarks>
        /// El paquete incluye los parámetros: el token de sesión, el nombre de usuario conectado y el rol que se va a eliminar,
        /// con sus respectivas longitudes representadas como cadenas de dos dígitos, seguido de los valores de los parámetros.
        /// Este formato asegura que el servidor pueda interpretar correctamente los datos recibidos.
        /// </remarks>
        public static string ConstructDeleteRolePacket(string sessionToken, string connectedUsername, string roleToDelete)
        {
            // Calcular los offsets para cada campo (longitud de cada parámetro en dos dígitos)
            string sessionTokenOffset = sessionToken.Length.ToString("D2");
            string connectedUsernameOffset = connectedUsername.Length.ToString("D2");
            string roleToDeleteOffset = roleToDelete.Length.ToString("D2");

            // Construir el paquete siguiendo el protocolo
            string packet = $"2{"11"}{sessionTokenOffset}{sessionToken}{connectedUsernameOffset}{connectedUsername}{roleToDeleteOffset}{roleToDelete}\n";
            //MessageBox.Show(packet);

            return packet;
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
            String packet = $"2{"12"}{sessionTokenOffset}{sessionToken}{connectedUsernameOffset}{connectedUsername}\n";
            //MessageBox.Show(packet);


            return packet;
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

            // Construir el paquete final
            string packet = $"2{"10"}{tokenOffset}{sessionToken}{usernameOffset}{username}{rolOffset}{rol}{permisosString}";

            // Mostrar el paquete generado en un MessageBox
            //MessageBox.Show($"Paquete generado:\n{packet}", "Paquete Editar Permisos", MessageBoxButton.OK, MessageBoxImage.Information);

            return packet;
        }




        /// <summary>
        /// Construye un paquete para activar o desactivar el Modo Gestión.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario del usuario conectado.</param>
        /// <param name="activarModoGestion">Indica si el Modo Gestión debe ser activado (true) o desactivado (false).</param>
        /// <returns>Cadena que representa el paquete de datos para activar o desactivar el Modo Gestión.</returns>
        public static string ConstructModoGestionPacket(string sessionToken, string username)
        {
            // Calcular los offsets para el token y el nombre de usuario
            string tokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token (con dos dígitos)
            string usernameOffset = username.Length.ToString("D2");  // Longitud del nombre de usuario (con dos dígitos)

            // Alternar el estado del Modo Gestión
            modoGestionActivo = !modoGestionActivo;

            // Determinar la acción basada en el nuevo estado
            int accion = modoGestionActivo ? 13 : 14;

            // Construir el paquete final para la activación/desactivación del Modo Gestión
            string packet = $"2{accion}{tokenOffset}{sessionToken}{usernameOffset}{username}";

            // Mostrar el paquete generado en un MessageBox
            string estado = modoGestionActivo ? "activado" : "desactivado";
            MessageBox.Show($"Paquete generado:\n{packet}\nEstado del Modo Gestión: {estado}", "Paquete Modo Gestión", MessageBoxButton.OK, MessageBoxImage.Information);

            return packet;
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

            // Construir el paquete final para crear el evento
            string packet = $"4{"06"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}{fechaOffset}{fecha}{contenidoOffset}{contenido}{tagsOffset}{tags}{unusedBytes}{grupalFlag}{groupNameOffset}{groupName}\n";

            // Mostrar el paquete generado en un MessageBox
            MessageBox.Show($"Paquete generado:\n{packet}", "Create Day Packet", MessageBoxButton.OK, MessageBoxImage.Information);

            // Retornar el paquete
            return packet;
        }





        /// <summary>
        /// Construye un paquete para crear un nuevo grupo, siguiendo el formato del protocolo específico.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que está realizando la solicitud.</param>
        /// <param name="username">El nombre de usuario del cliente que está conectado y realizando la solicitud.</param>
        /// <param name="nombreGrupo">El nombre del nuevo grupo que se va a crear.</param>
        /// <returns>Una cadena que representa el paquete con los parámetros necesarios para la creación del grupo, listo para ser enviado al servidor.</returns>
        /// <remarks>
        /// El paquete incluye los parámetros: el token de sesión, el nombre de usuario conectado y el nombre del grupo a crear,
        /// con sus respectivas longitudes representadas como cadenas de dos dígitos, seguido de los valores de los parámetros.
        /// Este formato asegura que el servidor pueda interpretar correctamente los datos recibidos y proceder con la creación del grupo.
        /// </remarks>
        public static string ConstructCreateGroupPacket(string sessionToken, string username, string nombreGrupo)
        {
            // Calcular los offsets para cada parámetro (longitud de cada campo en dos dígitos)
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del nombre de usuario conectado
            string groupNameOffset = nombreGrupo.Length.ToString("D2");      // Longitud del nombre del grupo

            // Construir y devolver el paquete
            String packet = $"2{"15"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}{groupNameOffset}{nombreGrupo}\n";
            //MessageBox.Show(packet);

            return packet;
        }



        /// <summary>
        /// Construye un paquete para eliminar un grupo, siguiendo el formato del protocolo específico.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que está realizando la solicitud.</param>
        /// <param name="username">El nombre de usuario del cliente que está conectado y realizando la solicitud.</param>
        /// <param name="nombreGrupo">El nombre del grupo que se desea eliminar.</param>
        /// <returns>Una cadena que representa el paquete con los parámetros necesarios para la eliminación del grupo, listo para ser enviado al servidor.</returns>
        /// <remarks>
        /// El paquete incluye los parámetros: el token de sesión, el nombre de usuario conectado y el nombre del grupo a eliminar,
        /// con sus respectivas longitudes representadas como cadenas de dos dígitos, seguido de los valores de los parámetros.
        /// Este formato asegura que el servidor pueda interpretar correctamente los datos recibidos y proceder con la eliminación del grupo.
        /// Además, el paquete generado se muestra en un MessageBox para fines de depuración antes de enviarlo al servidor.
        /// </remarks>
        public static string ConstructDeleteGroupPacket(string sessionToken, string username, string nombreGrupo)
        {
            // Calcular los offsets para cada parámetro (longitud de cada campo en dos dígitos)
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del nombre de usuario conectado
            string groupNameOffset = nombreGrupo.Length.ToString("D2");      // Longitud del nombre del grupo

            // Construir el paquete siguiendo el formato del protocolo
            string packet = $"2{"16"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}{groupNameOffset}{nombreGrupo}\n";

            // Mostrar el paquete generado en un MessageBox (para depuración)
            //MessageBox.Show($"Paquete generado:\n{packet}", "Delete Group Packet", MessageBoxButton.OK, MessageBoxImage.Information);

            // Retornar el paquete generado
            return packet;
        }



        /// <summary>
        /// Construye un paquete para eliminar la agenda asociada a un grupo, siguiendo el formato del protocolo específico.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que está realizando la solicitud.</param>
        /// <param name="username">El nombre de usuario del cliente que está conectado y realizando la solicitud.</param>
        /// <param name="nombreGrupo">El nombre del grupo cuya agenda se desea eliminar.</param>
        /// <returns>Una cadena que representa el paquete con los parámetros necesarios para la eliminación de la agenda del grupo, listo para ser enviado al servidor.</returns>
        /// <remarks>
        /// El paquete incluye los parámetros: el token de sesión, el nombre de usuario conectado y el nombre del grupo cuya agenda
        /// se desea eliminar, con sus respectivas longitudes representadas como cadenas de dos dígitos, seguido de los valores de los parámetros.
        /// Este formato asegura que el servidor pueda interpretar correctamente los datos recibidos y proceder con la eliminación de la agenda del grupo.
        /// Además, el paquete generado se muestra en un MessageBox para fines de depuración antes de enviarlo al servidor.
        /// </remarks>
        public static string ConstructDeleteGroupAgendaPacket(string sessionToken, string username, string nombreGrupo)
        {
            // Calcular los offsets para cada parámetro (longitud de cada campo en dos dígitos)
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del nombre de usuario conectado
            string groupNameOffset = nombreGrupo.Length.ToString("D2");      // Longitud del nombre del grupo

            // Construir el paquete con la acción correspondiente para eliminar la agenda grupal (acción 17)
            string packet = $"2{"17"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}{groupNameOffset}{nombreGrupo}\n";

            // Mostrar el paquete generado en un MessageBox (para depuración)
            //MessageBox.Show($"Paquete generado:\n{packet}", "Delete Group Agenda Packet", MessageBoxButton.OK, MessageBoxImage.Information);

            // Retornar el paquete generado
            return packet;
        }



        /// <summary>
        /// Construye un paquete para invitar a un usuario a un grupo específico, siguiendo el formato del protocolo definido.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que está realizando la invitación.</param>
        /// <param name="username">El nombre de usuario del cliente que está enviando la invitación.</param>
        /// <param name="nombreGrupo">El nombre del grupo al cual se desea invitar al usuario.</param>
        /// <param name="sobrenombreUsuario">El sobrenombre del usuario que se va a invitar.</param>
        /// <returns>Una cadena que representa el paquete con los parámetros necesarios para la invitación, listo para ser enviado al servidor.</returns>
        /// <remarks>
        /// El paquete incluye los siguientes parámetros: el token de sesión, el nombre de usuario del que envía la invitación,
        /// el nombre del grupo al que se le invita y el sobrenombre del usuario invitado. Estos parámetros son precedidos por su longitud 
        /// representada en dos dígitos para asegurar que el servidor pueda leer y procesar correctamente los datos.
        /// También, el paquete generado se muestra en un MessageBox para fines de depuración antes de enviarlo al servidor.
        /// </remarks>
        public static string ConstructInviteUserPacket(string sessionToken, string username, string nombreGrupo, string sobrenombreUsuario)
        {
            // Calcular los offsets (longitudes) de cada parámetro
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del nombre de usuario
            string groupNameOffset = nombreGrupo.Length.ToString("D2");      // Longitud del nombre del grupo
            string nicknameOffset = sobrenombreUsuario.Length.ToString("D2"); // Longitud del sobrenombre del usuario

            // Construir el paquete con la acción correspondiente para invitar a un usuario (acción 21)
            string packet = $"2{"21"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}{groupNameOffset}{nombreGrupo}{nicknameOffset}{sobrenombreUsuario}\n";

            // Mostrar el paquete generado para depuración
            //MessageBox.Show($"Paquete generado:\n{packet}", "Vista previa del paquete", MessageBoxButton.OK, MessageBoxImage.Information);

            // Retornar el paquete generado
            return packet;
        }



        /// <summary>
        /// Construye un paquete para crear o mostrar una agenda, incluyendo información sobre si es una agenda grupal o personal.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que solicita la creación o visualización de la agenda.</param>
        /// <param name="username">El nombre de usuario del cliente que está interactuando con la agenda.</param>
        /// <param name="isGrupal">Indica si la agenda es grupal o no. True si es grupal, False si es personal.</param>
        /// <param name="groupName">El nombre del grupo asociado a la agenda, opcional, solo se usa si la agenda es grupal.</param>
        /// <returns>Una cadena que representa el paquete listo para ser enviado al servidor para la creación o consulta de la agenda.</returns>
        /// <remarks>
        /// El paquete incluye los siguientes parámetros:
        /// - Token de sesión, con su longitud antes del valor (en formato de 2 dígitos).
        /// - Nombre de usuario, con su longitud también precedida por 2 dígitos.
        /// - Un conjunto de 2 bytes vacíos, representado por "00".
        /// - Un byte que indica si la agenda es grupal (1) o personal (0).
        /// - Si la agenda es grupal, se incluye el nombre del grupo con su longitud también representada por 2 dígitos.
        /// Este paquete está diseñado para seguir el protocolo de comunicación específico entre el cliente y el servidor.
        /// </remarks>
        public static string ConstructAgendaPacket(string sessionToken, string username, bool isGrupal, string groupName = "")
        {
            // Calcular los offsets (longitudes) de los parámetros
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del nombre de usuario

            // Crear los componentes del paquete básico
            string packet = $"401{sessionTokenOffset}{sessionToken}{usernameOffset}{username}";

            // Añadir conjunto de 2 bytes (en este caso, dos bytes vacíos representados por '00')
            string conjunto2Bytes = "00";  // Representación de los 2 bytes sin usar
            packet += $"{conjunto2Bytes}";

            // Añadir el byte que indica si es grupal o no (1 para grupal, 0 para no grupal)
            string byteGrupal = isGrupal ? "1" : "0";
            packet += $"{byteGrupal}";

            // Si es grupal, añadir el nombre del grupo
            if (isGrupal)
            {
                string groupNameOffset = groupName.Length.ToString("D2");  // Longitud del nombre del grupo
                packet += $"{groupNameOffset}{groupName}";
            }

            // Retornar el paquete generado
            //MessageBox.Show($"Paquete generado:\n{packet}", "Agenda Packet", MessageBoxButton.OK, MessageBoxImage.Information);
            return packet;
        }








        /// <summary>
        /// Construye un paquete para solicitar los grupos que el usuario posee (grupos de los cuales es administrador o creador).
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que realiza la solicitud.</param>
        /// <param name="username">El nombre de usuario que solicita los grupos que posee.</param>
        /// <returns>Una cadena que representa el paquete listo para ser enviado al servidor con la solicitud de grupos propios.</returns>
        /// <remarks>
        /// El paquete incluye los siguientes parámetros:
        /// - Token de sesión, con su longitud antes del valor (en formato de 2 dígitos).
        /// - Nombre de usuario, con su longitud también precedida por 2 dígitos.
        /// La acción para esta solicitud está identificada por el código "18" en el protocolo.
        /// Este paquete sigue el formato específico requerido para solicitar los grupos que un usuario posee.
        /// </remarks>
        public static string ConstructViewOwnedGroupsPacket(string sessionToken, string username)
        {
            // Calcular los offsets (longitudes) de los parámetros
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del nombre de usuario

            // Construir el paquete
            string packet = $"2{"18"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}\n";

            // Mostrar el paquete generado en un MessageBox para su revisión
            //MessageBox.Show($"Paquete generado:\n{packet}", "View Owned Groups Packet", MessageBoxButton.OK, MessageBoxImage.Information);

            // Retornar el paquete generado
            return packet;
        }



        /// <summary>
        /// Construye un paquete para solicitar los grupos en los que el usuario es miembro.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que realiza la solicitud.</param>
        /// <param name="username">El nombre de usuario que solicita los grupos donde es miembro.</param>
        /// <returns>Una cadena que representa el paquete listo para ser enviado al servidor con la solicitud de grupos de membresía.</returns>
        /// <remarks>
        /// El paquete incluye los siguientes parámetros:
        /// - Token de sesión, con su longitud antes del valor (en formato de 2 dígitos).
        /// - Nombre de usuario, con su longitud también precedida por 2 dígitos.
        /// La acción para esta solicitud está identificada por el código "19" en el protocolo.
        /// Este paquete sigue el formato específico requerido para solicitar los grupos donde el usuario es miembro.
        /// </remarks>
        public static string ConstructViewMembershipGroupsPacket(string sessionToken, string username)
        {
            // Calcular los offsets (longitudes) de los parámetros
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del nombre de usuario

            // Construir el paquete y devolverlo
            return $"2{"19"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}\n";
        }



        /// <summary>
        /// Construye un paquete para solicitar ver todos los grupos disponibles.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que realiza la solicitud.</param>
        /// <param name="username">El nombre de usuario que solicita la información sobre todos los grupos.</param>
        /// <returns>Una cadena que representa el paquete listo para ser enviado al servidor con la solicitud de todos los grupos.</returns>
        /// <remarks>
        /// El paquete incluye los siguientes parámetros:
        /// - Token de sesión, con su longitud antes del valor (en formato de 2 dígitos).
        /// - Nombre de usuario, con su longitud también precedida por 2 dígitos.
        /// La acción para esta solicitud está identificada por el código "20" en el protocolo.
        /// Este paquete sigue el formato específico requerido para solicitar la lista completa de grupos.
        /// </remarks>
        public static string ConstructViewAllGroupsPacket(string sessionToken, string username)
        {
            // Calcular los offsets (longitudes) de los parámetros
            string sessionTokenOffset = sessionToken.Length.ToString("D2");  // Longitud del token de sesión
            string usernameOffset = username.Length.ToString("D2");          // Longitud del nombre de usuario

            // Construir el paquete y devolverlo
            return $"2{"20"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}\n";
        }



        /// <summary>
        /// Construye un paquete para solicitar la vista del mes en el sistema.
        /// </summary>
        /// <param name="sessionToken">El token de sesión del usuario que realiza la solicitud.</param>
        /// <param name="username">El nombre de usuario que solicita la vista del mes.</param>
        /// <returns>Una cadena que representa el paquete listo para ser enviado al servidor para solicitar la vista del mes.</returns>
        /// <remarks>
        /// El paquete incluye los siguientes parámetros:
        /// - Token de sesión con su longitud precedida por 2 dígitos.
        /// - Nombre de usuario con su longitud precedida por 2 dígitos.
        /// - Un conjunto de 2 bytes vacíos ("00") para completar el paquete.
        /// - Información adicional estática como "109IOCtest11" al final del paquete.
        /// Este paquete sigue el formato específico requerido para mostrar la vista de un mes.
        /// </remarks>
        public static string ConstructShowMonthPacket(string sessionToken, string username)
        {
            // Calcular los offsets (longitudes) de los parámetros
            string sessionTokenOffset = sessionToken.Length.ToString("D2");
            string usernameOffset = username.Length.ToString("D2");

            // El conjunto de 2 bytes (cualquier valor, en este caso 00) 
            string filler = "00";

            // Crear el paquete con la estructura adecuada
            return $"4{"01"}{sessionTokenOffset}{sessionToken}{usernameOffset}{username}{filler}109IOCtest11\n";
        }



        /// <summary>
        /// Genera un paquete para avanzar un mes en la agenda.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario conectado.</param>
        /// <returns>El paquete para avanzar un mes en formato string.</returns>
        public static string ConstructAdvanceMonthPacket(string sessionToken, string username)
        {
            // Longitudes de los parámetros
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameLengthStr = username.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo
            string response = $"4{"02"}{tokenLengthStr}{sessionToken}" +
                              $"{usernameLengthStr}{username}";

            // Mensaje de verificación temporal (opcional)
            //MessageBox.Show(response);

            return response;
        }


        /// <summary>
        /// Genera un paquete para retroceder un mes en la agenda.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario conectado.</param>
        /// <returns>El paquete para retroceder un mes en formato string.</returns>
        public static string ConstructGoBackMonthPacket(string sessionToken, string username)
        {
            // Longitudes de los parámetros
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameLengthStr = username.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo
            string response = $"4{"03"}{tokenLengthStr}{sessionToken}" +
                              $"{usernameLengthStr}{username}";

            // Mensaje de verificación temporal (opcional)
            //MessageBox.Show(response);

            return response;
        }


        /// <summary>
        /// Genera un paquete para avanzar un año en la agenda.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario conectado.</param>
        /// <returns>El paquete para avanzar un año en formato string.</returns>
        public static string ConstructAdvanceYearPacket(string sessionToken, string username)
        {
            // Longitudes de los parámetros
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameLengthStr = username.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo para avanzar un año
            string response = $"4{"04"}{tokenLengthStr}{sessionToken}" +
                              $"{usernameLengthStr}{username}";

            // Mensaje de verificación temporal (opcional)
            //MessageBox.Show(response);

            return response;
        }

        /// <summary>
        /// Genera un paquete para retroceder un año en la agenda.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario conectado.</param>
        /// <returns>El paquete para retroceder un año en formato string.</returns>
        public static string ConstructGoBackYearPacket(string sessionToken, string username)
        {
            // Longitudes de los parámetros
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameLengthStr = username.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo para retroceder un año
            string response = $"4{"05"}{tokenLengthStr}{sessionToken}" +
                              $"{usernameLengthStr}{username}";

            // Mensaje de verificación temporal (opcional)
            //MessageBox.Show(response);

            return response;
        }


        /// <summary>
        /// Genera un paquete para insertar un tag en la agenda.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario conectado.</param>
        /// <param name="fecha">Fecha en formato DD/MM/YYYY.</param>
        /// <param name="nuevoTag">Nuevo tag a insertar.</param>
        /// <returns>El paquete para insertar el tag en formato string.</returns>
        public static string ConstructInsertTagPacket(string sessionToken, string username, string fecha, string nuevoTag)
        {
            // Longitudes de los parámetros
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameLengthStr = username.Length.ToString("D2");
            string fechaLengthStr = fecha.Length.ToString("D2");
            string nuevoTagLengthStr = nuevoTag.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo para insertar el tag
            string response = $"4{"08"}" + // Acción 08: Insertar tag
                              $"{tokenLengthStr}{sessionToken}" + // Longitud y token de sesión
                              $"{usernameLengthStr}{username}" + // Longitud y nombre de usuario
                              $"{fechaLengthStr}{fecha}" + // Longitud y fecha
                              $"{nuevoTagLengthStr}{nuevoTag}"; // Longitud y nuevo tag

            // Mensaje de verificación temporal (opcional)
            //MessageBox.Show(response);

            return response;
        }



        /// <summary>
        /// Genera un paquete para eliminar un tag en la agenda.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario conectado.</param>
        /// <param name="fecha">Fecha en formato DD/MM/YYYY.</param>
        /// <param name="tagAEliminar">Tag a eliminar.</param>
        /// <returns>El paquete para eliminar el tag en formato string.</returns>
        public static string ConstructEliminarTagPacket(string sessionToken, string username, string fecha, string tagAEliminar)
        {
            // Longitudes de los parámetros
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameLengthStr = username.Length.ToString("D2");
            string fechaLengthStr = fecha.Length.ToString("D2");
            string tagAEliminarLengthStr = tagAEliminar.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo para eliminar el tag
            string response = $"4{"09"}" + // Acción 09: Eliminar tag
                              $"{tokenLengthStr}{sessionToken}" + // Longitud y token de sesión
                              $"{usernameLengthStr}{username}" + // Longitud y nombre de usuario
                              $"{fechaLengthStr}{fecha}" + // Longitud y fecha
                              $"{tagAEliminarLengthStr}{tagAEliminar}"; // Longitud y tag a eliminar

            // Mensaje de verificación temporal (opcional)
            //MessageBox.Show(response);

            return response;
        }


        /// <summary>
        /// Genera un paquete para buscar días por tag.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario.</param>
        /// <param name="username">Nombre de usuario.</param>
        /// <param name="grupal">Indica si la búsqueda es grupal.</param>
        /// <param name="tag">Tag a buscar.</param>
        /// <param name="groupName">Nombre del grupo (si aplica).</param>
        /// <returns>El paquete de búsqueda de tag en formato string.</returns>
        public static string ConstructSearchTagPacket(string sessionToken, string username, bool grupal, string tag, string groupName = null)
        {
            // Longitudes de los parámetros
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameLengthStr = username.Length.ToString("D2");
            string tagLengthStr = tag.Length.ToString("D2");
            string groupNameLengthStr = grupal ? groupName.Length.ToString("D2") : "00";

            // Byte grupal: 0 si no es grupal, 1 si es grupal
            string grupalByte = grupal ? "1" : "0";

            // Construcción del paquete
            string packet = $"4{"10"}" + // Acción 10: Mostrar resultados por búsqueda de tag
                            $"{tokenLengthStr}{sessionToken}" + // Offset y token
                            $"{usernameLengthStr}{username}" + // Offset y usuario
                            "00" + // 2 bytes de relleno
                            $"{grupalByte}" + // Byte grupal
                            $"{tagLengthStr}{tag}"; // Offset y tag

            // Si es grupal, añadir el nombre del grupo
            if (grupal)
            {
                packet += $"{groupNameLengthStr}{groupName}";
            }

            // Mensaje de verificación temporal (opcional)
            //MessageBox.Show(packet);

            return packet;
        }



        /// <summary>
        /// Genera un paquete para editar el contenido de un evento en la agenda.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario conectado.</param>
        /// <param name="fecha">Fecha del evento a editar (formato DD/MM/YYYY).</param>
        /// <param name="nuevoContenido">Nuevo contenido para el evento.</param>
        /// <param name="esGrupal">Indica si es un evento grupal.</param>
        /// <returns>El paquete para editar el evento en formato string.</returns>
        public static string ConstructModificarEventoPacket(string sessionToken, string username, string fecha, string nuevoContenido, bool esGrupal)
        {
            // Longitudes de los parámetros
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameLengthStr = username.Length.ToString("D2");
            string fechaLengthStr = fecha.Length.ToString("D2");
            string nuevoContenidoLengthStr = nuevoContenido.Length.ToString("D4"); // 4 bytes para el nuevo contenido

            // Longitud de los valores de relleno (2 bytes)
            string rellenoLengthStr = "00"; // 2 bytes de relleno (00)

            // Longitud para indicar si es grupal (1 byte)
            byte grupalByte = (byte)(esGrupal ? 1 : 0);

            // Construcción del paquete siguiendo el protocolo para editar el contenido
            string response = $"4{"07"}" + // Acción 07: Editar contenido
                              $"{tokenLengthStr}{sessionToken}" + // Longitud y token de sesión
                              $"{usernameLengthStr}{username}" + // Longitud y nombre de usuario
                              $"{fechaLengthStr}{fecha}" + // Longitud y fecha
                              $"{nuevoContenidoLengthStr}{nuevoContenido}" + // Longitud y nuevo contenido
                              $"{rellenoLengthStr}00" + // Relleno (2 bytes)
                              $"{grupalByte}"; // Indicador de si es grupal (1 byte)

            // Mensaje de verificación temporal (opcional)
            //MessageBox.Show(response);

            return response;
        }



        /// <summary>
        /// Genera un paquete para bloquear un día para edición en una agenda grupal.
        /// </summary>
        /// <param name="sessionToken">Token de sesión del usuario conectado.</param>
        /// <param name="username">Nombre de usuario conectado.</param>
        /// <param name="fecha">Fecha del día a bloquear (formato DD/MM/YYYY).</param>
        /// <param name="nombreGrupo">Nombre del grupo.</param>
        /// <returns>El paquete para bloquear el día en formato string.</returns>
        public static string ConstructBloquearDiaEdicionPacket(string sessionToken, string username, string fecha, string nombreGrupo)
        {
            // Longitudes de los parámetros
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameLengthStr = username.Length.ToString("D2");
            string fechaLengthStr = fecha.Length.ToString("D2");
            string nombreGrupoLengthStr = nombreGrupo.Length.ToString("D2");

            // Construcción del paquete siguiendo el protocolo para bloquear el día para edición
            string response = $"4{"11"}" + // Acción 11: Bloquear edición de día
                              $"{tokenLengthStr}{sessionToken}" + // Longitud y token de sesión
                              $"{usernameLengthStr}{username}" + // Longitud y nombre de usuario
                              $"{fechaLengthStr}{fecha}" + // Longitud y fecha
                              $"{nombreGrupoLengthStr}{nombreGrupo}"; // Longitud y nombre del grupo

            // Mensaje de verificación temporal (opcional)
            //MessageBox.Show(response);

            return response;
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
