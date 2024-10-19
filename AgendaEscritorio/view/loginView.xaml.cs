using System.Windows;
using System.Windows.Input;
using AgendaEscritorio.service;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using AgendaEscritorio.view;


namespace AgendaEscritorio.view
{
    public partial class loginView : Window
    {
        private SocketClient socketClient;

        public loginView()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            socketClient = new SocketClient(); // Cliente para el servidor simulado
        }

        // Método para mover la ventana arrastrándola desde cualquier parte
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove(); // Permite mover la ventana al arrastrar
            }
        }

        // Minimizar la ventana
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Cerrar la ventana
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Métodos para simular el PlaceholderText
        private void RemoveText(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == "Usuario")
            {
                txtUsername.Text = "";
            }
        }

        private void AddText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Usuario";
            }
        }

        // Evento de login
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Limpiar mensajes de error previos
            txtUsernameError.Visibility = Visibility.Collapsed;
            txtPasswordError.Visibility = Visibility.Collapsed;

            // Validar campos vacíos
            if (string.IsNullOrWhiteSpace(username) || username == "Usuario")
            {
                txtUsernameError.Text = "Por favor, ingrese un nombre de usuario.";
                txtUsernameError.Visibility = Visibility.Visible;
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                txtPasswordError.Text = "Por favor, ingrese una contraseña.";
                txtPasswordError.Visibility = Visibility.Visible;
                return;
            }

            // Enviar datos al servidor simulado
            var loginRequest = new { Username = username, Password = password };
            string jsonResponse = await socketClient.SendMessageAsync(loginRequest);

            // Verificar si la respuesta es null
            if (string.IsNullOrEmpty(jsonResponse))
            {
                MessageBox.Show("No se recibió respuesta del servidor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Procesar respuesta
                var response = JsonConvert.DeserializeObject<LoginResponse>(jsonResponse);

                // Verificar si la deserialización fue exitosa
                if (response == null)
                {
                    MessageBox.Show("Respuesta no válida del servidor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (response.Success)
                {
                    MessageBox.Show($"Login exitoso. Rol: {response.Role}", "Información", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Cerrar ventana de login y abrir el Menú Principal con el rol del usuario
                    this.Hide(); // Ocultar ventana actual de login
                    MainMenuView mainMenu = new MainMenuView(response.Role); // Pasar el rol a la ventana del menú principal
                    mainMenu.Show();

                    // Cierra completamente la ventana de login (opcional)
                    this.Close();
                }
                else
                {
                    MessageBox.Show(response.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (JsonReaderException ex)
            {
                MessageBox.Show($"Error al procesar la respuesta del servidor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

    public class LoginResponse
    {
        public bool Success { get; set; } // Cambiar a set para permitir la deserialización
        public string Role { get; set; } = string.Empty; // Inicializa como cadena vacía
        public string ErrorMessage { get; set; } = string.Empty; // Inicializa como cadena vacía

        // Constructor
        public LoginResponse() { } // Constructor predeterminado para la deserialización

        public LoginResponse(bool success, string role = "", string errorMessage = "")
        {
            Success = success;
            Role = role;
            ErrorMessage = errorMessage;
        }
    }
}
