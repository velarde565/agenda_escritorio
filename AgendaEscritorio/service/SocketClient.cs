using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; 

namespace AgendaEscritorio.service
{
    public class SocketClient
    {
        private string serverIp = "127.0.0.1"; // IP del servidor (en este caso local)
        private int port = 8080; // Puerto donde escuchará el servidor simulado

        public async Task<string> SendMessageAsync(object message)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync(serverIp, port);
                    NetworkStream stream = client.GetStream();

                    // Convertir el mensaje a JSON
                    string jsonMessage = JsonConvert.SerializeObject(message);
                    byte[] data = Encoding.UTF8.GetBytes(jsonMessage);

                    // Enviar el mensaje al servidor
                    await stream.WriteAsync(data, 0, data.Length);

                    // Recibir la respuesta del servidor
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Imprimir la respuesta recibida para depuración
                    Console.WriteLine("Respuesta recibida del servidor: " + response);


                    return response;
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
