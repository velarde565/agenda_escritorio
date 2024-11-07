using System;
using System.Windows;

namespace AgendaEscritorio.service
{
    public static class ProtocolHelper
    {
        public static string ConstructLoginPacket(string username, string password)
        {
            string usernameLengthStr = username.Length.ToString("D2");
            string passwordLengthStr = password.Length.ToString("D2");
            return $"1{"01"}{usernameLengthStr}{username}{passwordLengthStr}{password}\n";
        }

        public static string ConstructUserDataRequestPacket(string sessionToken, string connectedUsername, string usernameToChange)
        {
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string connectedUsernameLengthStr = connectedUsername.Length.ToString("D2");
            string usernameToChangeLengthStr = usernameToChange.Length.ToString("D2");

            return $"2{"05"}{tokenLengthStr}{sessionToken}{usernameToChangeLengthStr}{usernameToChange}{connectedUsernameLengthStr}{connectedUsername}\n";
        }


        public static string ConstructLogoutPacket(string sessionToken, string username)
        {
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameLengthStr = username.Length.ToString("D2");
            return $"1{"02"}{tokenLengthStr}{sessionToken}{usernameLengthStr}{username}\n";
        }

        public static string ConstructChangeFullNamePacket(string sessionToken, string usernameToChange, string newFullName, string connectedUsername)
        {
            string sessionTokenOffset = sessionToken.Length.ToString("D2");
            string usernameToChangeOffset = usernameToChange.Length.ToString("D2");
            string newFullNameOffset = newFullName.Length.ToString("D2");
            string connectedUsernameOffset = connectedUsername.Length.ToString("D2");

            return $"2{"02"}{sessionTokenOffset}{sessionToken}{usernameToChangeOffset}{usernameToChange}{newFullNameOffset}{newFullName}{connectedUsernameOffset}{connectedUsername}\n";
        }

        public static string ConstructChangeBirthDatePacket(string sessionToken, string usernameToChange, string newBirthDate, string connectedUsername)
        {
            string sessionTokenOffset = sessionToken.Length.ToString("D2");
            string usernameToChangeOffset = usernameToChange.Length.ToString("D2");
            string newBirthDateOffset = newBirthDate.Length.ToString("D2");
            string connectedUsernameOffset = connectedUsername.Length.ToString("D2");

            string response;
            response = $"2{"03"}{sessionTokenOffset}{sessionToken}{connectedUsernameOffset}{connectedUsername}{usernameToChangeOffset}{usernameToChange}{newBirthDateOffset}{newBirthDate}\n";
            MessageBox.Show(response);
            return $"2{"03"}{sessionTokenOffset}{sessionToken}{connectedUsernameOffset}{connectedUsername}{usernameToChangeOffset}{usernameToChange}{newBirthDateOffset}{newBirthDate}\n";
        }


        public static string ConstructChangePasswordPacket(string sessionToken, string usernameToEdit, string currentPassword, string newPassword, string connectedUsername)
        {
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameToEditLengthStr = usernameToEdit.Length.ToString("D2");
            string currentPasswordLengthStr = currentPassword.Length.ToString("D2");
            string newPasswordLengthStr = newPassword.Length.ToString("D2");
            string connectedUsernameLengthStr = connectedUsername.Length.ToString("D2");

            string response= $"2{"01"}{tokenLengthStr}{sessionToken}" +
                   $"{usernameToEditLengthStr}{usernameToEdit}" +
                   $"{currentPasswordLengthStr}{currentPassword}" +
                   $"{newPasswordLengthStr}{newPassword}" +
                   $"{connectedUsernameLengthStr}{connectedUsername}\n";
            MessageBox.Show(response);

            return $"2{"01"}{tokenLengthStr}{sessionToken}" +
                   $"{usernameToEditLengthStr}{usernameToEdit}" +
                   $"{currentPasswordLengthStr}{currentPassword}" +
                   $"{newPasswordLengthStr}{newPassword}" +
                   $"{connectedUsernameLengthStr}{connectedUsername}\n";
        }


        public static string ConstructChangeOtherDataPacket(string sessionToken, string usernameToChange, string connectedUsername, string newOtherData)
        {
            // Verificación temporal de valores
            Console.WriteLine($"Connected Username: '{connectedUsername}', Length: {connectedUsername.Length}");

            string tokenOffset = $"{sessionToken.Length:D2}";
            string usernameOffset = $"{connectedUsername.Length:D2}";
            string targetUsernameOffset = $"{usernameToChange.Length:D2}";
            string otherDataOffset = $"{newOtherData.Length:D4}"; // Zero-fill to 4 characters

            string response = $"2{"04"}{tokenOffset}{sessionToken}{targetUsernameOffset}{usernameToChange}{usernameOffset}{connectedUsername}{otherDataOffset}{newOtherData}";

            MessageBox.Show(response);

            return response;
        }

        public static string ConstructDeleteUserPacket(string sessionToken, string usernameToDelete, string connectedUsername)
        {
        string sessionTokenOffset = sessionToken.Length.ToString("D2");
        string usernameToDeleteOffset = usernameToDelete.Length.ToString("D2");
        string connectedUsernameOffset = connectedUsername.Length.ToString("D2");

        // Paquete: Acción 07 para eliminar usuario
        string packet = $"2{"07"}{sessionTokenOffset}{sessionToken}{connectedUsernameOffset}{connectedUsername}{usernameToDeleteOffset}{usernameToDelete}\n";
    
        return packet;
        }

        public static string ConstructCreateUserPacket(string sessionToken, string connectedUsername, string newUsername, string password, string fullName, string birthDate, string otrosDatos, string rolPermisos)
        {
            string sessionTokenOffset = sessionToken.Length.ToString("D2");
            string newUsernameOffset = newUsername.Length.ToString("D2");
            string passwordOffset = password.Length.ToString("D2");
            string fullNameOffset = fullName.Length.ToString("D2");
            string birthDateOffset = birthDate.Length.ToString("D2");
            string otrosDatosOffset = otrosDatos.Length.ToString("D4");
            string rolPermisosOffset = rolPermisos.Length.ToString("D2");
            string connectedUsernameOffset = connectedUsername.Length.ToString("D2");

            // Construcción del paquete de datos.
            string packet = $"2{"06"}{sessionTokenOffset}{sessionToken}{connectedUsernameOffset}{connectedUsername}{newUsernameOffset}{newUsername}{passwordOffset}{password}{fullNameOffset}{fullName}{birthDateOffset}{birthDate}{otrosDatosOffset}{otrosDatos}{rolPermisosOffset}{rolPermisos}\n";

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
                             $"Rol Permisos: {rolPermisos}";

            // Mostrar el mensaje en MessageBox.
            MessageBox.Show(message, "Detalle del Paquete");

            return packet;
        }





        public static Tuple<int, string> LecturaPartesPaquete(string response)
        {
            string actionString = response.Substring(0, 3);
            if (int.TryParse(actionString, out int action))
            {
                string message = response.Length > 3 ? response.Substring(3) : string.Empty;
                return new Tuple<int, string>(action, message);
            }
            throw new FormatException("Invalid action format in response.");
        }
    }
}
