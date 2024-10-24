using System;

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

        public static string ConstructUserDataRequestPacket(string sessionToken, string username)
        {
            string usernameLengthStr = username.Length.ToString("D2");
            return $"2{"05"}{sessionToken.Length:D2}{sessionToken}{usernameLengthStr}{username}{usernameLengthStr}{username}\n";
        }


        public static string ConstructLogoutPacket(string sessionToken, string username)
        {
            string tokenLengthStr = sessionToken.Length.ToString("D2");
            string usernameLengthStr = username.Length.ToString("D2");
            return $"1{"02"}{tokenLengthStr}{sessionToken}{usernameLengthStr}{username}\n";
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
