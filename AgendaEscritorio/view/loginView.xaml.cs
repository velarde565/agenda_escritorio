using System.Windows;
using System.Windows.Input;
using AgendaEscritorio.service;
using System.Threading.Tasks;

namespace AgendaEscritorio.view
{
    public partial class loginView : Window
    {
        private Client client;

        public loginView()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            client = new Client(); // Instanciar Client
            ConnectToServerAsync(); // Llamar a la conexión de manera asincrónica
        }

        // Método asincrónico para conectarse al servidor
        private async void ConnectToServerAsync()
        {
            await client.ConnectAsync(); // Llamada asincrónica para conectar al servidor
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

            // Enviar solicitud de login al servidor de manera asincrónica
            bool loginSuccess = await client.SendLoginAsync(username, password);

            if (loginSuccess)
            {
                // Si el login es exitoso, abrir la ventana del menú principal
                MainMenuView mainMenuView = new MainMenuView(client); // Cambia "userRole" por el rol real obtenido del servidor
                mainMenuView.Show();
                this.Close(); // Cerrar la ventana de login
            }
            else
            {
                // Manejar el error de login
                txtUsernameError.Text = "Usuario o contraseña incorrectos.";
                txtUsernameError.Visibility = Visibility.Visible;
            }
        }

    }
}
