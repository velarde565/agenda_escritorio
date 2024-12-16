using System.Windows;
using System.Windows.Input;
using AgendaEscritorio.service;
using System.Threading.Tasks;

namespace AgendaEscritorio.view
{
    /// <summary>
    /// Vista de la ventana de login donde los usuarios ingresan sus credenciales para acceder al sistema.
    /// Incluye validación de entrada, conexión con el servidor y manejo de la interfaz.
    /// </summary>
    public partial class loginView : Window
    {
        private Client client;

        /// <summary>
        /// Constructor de la clase <see cref="loginView"/>. Inicializa los componentes de la interfaz y
        /// establece la conexión con el servidor de manera asincrónica.
        /// </summary>
        public loginView()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            client = new Client(); // Instancia un objeto Client para conectarse al servidor
            ConnectToServerAsync(); // Llama al método asincrónico para conectar al servidor

        }

        /// <summary>
        /// Método asincrónico que establece la conexión con el servidor.
        /// </summary>
        private async void ConnectToServerAsync()
        {
            await client.ConnectAsync(); // Conexión asincrónica al servidor utilizando el objeto client

            await client.SendClientPublicKeyAsync();
        }

        /// <summary>
        /// Permite mover la ventana al arrastrarla desde cualquier parte. Este evento se dispara cuando
        /// el usuario hace clic y mantiene presionado el botón izquierdo del ratón.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (ventana de la aplicación).</param>
        /// <param name="e">Argumentos del evento que contienen información del ratón.</param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove(); // Permite mover la ventana al arrastrar con el ratón
            }
        }

        /// <summary>
        /// Minimiza la ventana cuando se hace clic en el botón correspondiente.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (botón de minimizar).</param>
        /// <param name="e">Argumentos del evento.</param>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Cambia el estado de la ventana a minimizada
        }

        /// <summary>
        /// Cierra la ventana de login cuando el usuario hace clic en el botón de cerrar.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (botón de cerrar).</param>
        /// <param name="e">Argumentos del evento.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Cierra la ventana de login
        }

        /// <summary>
        /// Elimina el texto por defecto del cuadro de texto de nombre de usuario cuando el usuario hace clic
        /// en el campo de texto para empezar a escribir.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (campo de texto para nombre de usuario).</param>
        /// <param name="e">Argumentos del evento.</param>
        private void RemoveText(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == "Usuario")
            {
                txtUsername.Text = ""; // Elimina el texto predeterminado "Usuario"
            }
        }

        /// <summary>
        /// Restaura el texto por defecto en el cuadro de texto de nombre de usuario si el campo está vacío
        /// y el usuario no ha ingresado ningún valor.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (campo de texto para nombre de usuario).</param>
        /// <param name="e">Argumentos del evento.</param>
        private void AddText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Usuario"; // Restaura el texto predeterminado "Usuario"
            }
        }

        /// <summary>
        /// Maneja el evento de login cuando el usuario hace clic en el botón "Iniciar sesión".
        /// Valida las credenciales ingresadas y envía la solicitud al servidor para autenticar al usuario.
        /// Si el login es exitoso, abre la ventana principal; de lo contrario, muestra un mensaje de error.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (botón de login).</param>
        /// <param name="e">Argumentos del evento.</param>
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Limpiar mensajes de error previos
            txtUsernameError.Visibility = Visibility.Collapsed;
            txtPasswordError.Visibility = Visibility.Collapsed;

            // Validar que los campos no estén vacíos
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

            // Llamada asincrónica para enviar las credenciales al servidor y recibir una respuesta
            bool loginSuccess = await client.SendLoginAsync(username, password);

            if (loginSuccess)
            {
                // Si el login es exitoso, abre la ventana principal del menú
                MainMenuView mainMenuView = new MainMenuView(client); // Cambia "userRole" por el rol obtenido del servidor
                mainMenuView.Show();
                this.Close(); // Cierra la ventana de login
            }
            else
            {
                // Si el login falla, muestra un mensaje de error
                txtUsernameError.Text = "Usuario o contraseña incorrectos.";
                txtUsernameError.Visibility = Visibility.Visible;
            }
        }
    }
}
