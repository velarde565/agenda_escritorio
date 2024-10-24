using System;

namespace AgendaEscritorio.service
{
    public static class DataExtractor
    {
        public static string ExtractData(string response, ref int index)
        {
            int offset = int.Parse(response.Substring(index, 2));
            index += 2;
            string data = response.Substring(index, offset);
            index += offset;
            return data;
        }

        public static string ExtractOtherData(string response, ref int index)
        {
            int offset = int.Parse(response.Substring(index, 4));
            index += 4;
            string data = response.Substring(index, offset);
            index += offset;
            return data;
        }

        public static string ExtractToken(string message)
        {
            int tokenOffset = int.Parse(message.Substring(0, 2));
            return message.Substring(2, tokenOffset);
        }
    }
}
