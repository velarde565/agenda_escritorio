using AgendaEscritorio.service;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AgendaEscritorio.view
{
    /// <summary>
    /// Ventana principal que muestra el menú según el rol del usuario.
    /// Permite acceder a distintas secciones como la gestión de perfiles, agenda, ajustes, y cerrar sesión.
    /// </summary>
    public partial class MainMenuView : Window
    {
        private Client client;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MainMenuView"/>.
        /// </summary>
        /// <param name="client">Instancia del cliente que se usará para las comunicaciones con el servidor.</param>
        public MainMenuView(Client client)
        {
            InitializeComponent();
            this.client = client;
            ConfigureMenuBasedOnRole(); // Configura el menú en función del rol del usuario
        }

        /// <summary>
        /// Configura el menú en función del rol del usuario.
        /// Si el usuario es administrador, se habilita la opción de ajustes.
        /// </summary>
        private void ConfigureMenuBasedOnRole()
        {
            Dispatcher.Invoke(() =>
            {
                if (client.IsAdmin == '1') // Compara si IsAdmin es '1' para tratarlo como un administrador
                {
                    btnAjustes.Visibility = Visibility.Visible; // Muestra el botón de ajustes si es administrador
                }
                else // Si no es administrador
                {
                    btnAjustes.Visibility = Visibility.Collapsed; // Oculta el botón de ajustes
                }
            });
        }

        /// <summary>
        /// Evento que se llama al hacer clic en el botón de gestión de perfiles.
        /// Abre la ventana de gestión de usuarios.
        /// </summary>
        private void BtnGestionPerfiles_Click(object sender, RoutedEventArgs e)
        {
            UserManagementView userManagementView = new UserManagementView(client);
            userManagementView.Show(); // Muestra la ventana de gestión de perfiles
        }

        /// <summary>
        /// Evento que se llama al hacer clic en el botón de agenda.
        /// Abre la ventana del calendario.
        /// </summary>
        private async void BtnAgenda_Click(object sender, RoutedEventArgs e)
        {

            // Abrir la ventana del calendario después de la llamada asíncrona
            CalendarView calendarView = new CalendarView(client); // Pasa la instancia de Client
            calendarView.Show(); // Muestra la ventana del calendario

        }

        /// <summary>
        /// Maneja el evento de clic en el botón para mostrar u ocultar el panel de petición manual.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón de petición manual).</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método cambia la visibilidad del panel de petición manual. Si el panel está visible, se ocultará; si está oculto, se mostrará.
        /// </remarks>
        private void BtnPeticionManual_Click(object sender, RoutedEventArgs e)
        {
            // Verifica la visibilidad actual del panel
            if (panelPeticionManual.Visibility == Visibility.Visible)
            {
                // Si el panel está visible, se oculta
                panelPeticionManual.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Si el panel está oculto, se muestra
                panelPeticionManual.Visibility = Visibility.Visible;
            }
        }




        /// <summary>
        /// Maneja el evento de clic en el botón para enviar una petición manual.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón de enviar petición).</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método recoge el texto de la petición manual desde el campo de texto, valida si es válido y, si lo es,
        /// envía la petición utilizando el método `SendCustomPacketAsync` de la clase `Client`.
        /// </remarks>
        private async void BtnEnviarPeticion_Click(object sender, RoutedEventArgs e)
        {
            // Obtiene el texto de la petición manual ingresado por el usuario
            string peticion = txtPeticionManual.Text;

            // Valida que el campo no esté vacío o solo contenga espacios
            if (string.IsNullOrWhiteSpace(peticion))
            {
                // Si la petición está vacía o contiene solo espacios, muestra un mensaje de error
                MessageBox.Show("Por favor, introduce una petición válida.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Si la petición es válida, se llama al método de la clase Client para enviar la solicitud
            await client.SendCustomPacketAsync(peticion);
        }




        /// <summary>
        /// Evento que se llama al hacer clic en el botón de cerrar sesión.
        /// Envia la solicitud de logout y abre la ventana de login.
        /// </summary>
        /// <param name="sender">El objeto que invoca el evento.</param>
        /// <param name="e">Los argumentos del evento.</param>
        private async void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            await client.SendLogoutAsync(); // Llama al método para cerrar sesión a través del cliente

            loginView loginView = new loginView();
            loginView.Show(); // Abre la ventana de login
            this.Close(); // Cierra la ventana actual
        }

        /// <summary>
        /// Evento que se llama al hacer clic en el botón de ajustes.
        /// Abre la ventana de ajustes.
        /// </summary>
        private void BtnAjustes_Click(object sender, RoutedEventArgs e)
        {
            AjustesView ajustesView = new AjustesView();
            ajustesView.Show(); // Muestra la ventana de ajustes
            this.Close(); // Cierra la ventana actual
        }

        /// <summary>
        /// Evento que se llama al hacer clic en el botón de minimizar.
        /// Minimiza la ventana.
        /// </summary>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Minimiza la ventana
        }

        /// <summary>
        /// Evento que se llama al hacer clic en el botón de maximizar/restaurar.
        /// Alterna el estado de la ventana entre maximizado y restaurado.
        /// </summary>
        private void MaximizeRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized; // Maximiza la ventana
                MaximizeRestoreButton.Content = ""; // Cambia el contenido del botón para indicar que puede restaurarse
            }
            else
            {
                this.WindowState = WindowState.Normal; // Restaura la ventana a su tamaño original
                MaximizeRestoreButton.Content = "☐"; // Cambia el contenido del botón a un cuadro vacío (símbolo de restaurar)
            }
        }

        /// <summary>
        /// Evento que se llama al hacer clic en el botón de cerrar.
        /// Cierra la ventana actual.
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Cierra la ventana
        }

        /// <summary>
        /// Evento que se llama cuando la ventana se ha cargado.
        /// Este método se puede utilizar para agregar lógica adicional al cargar la ventana.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Lógica adicional para cargar la ventana, si es necesario
        }


        /// <summary>
        /// Maneja el evento de clic en el botón de apagado, solicitando la contraseña y enviando una solicitud de apagado al servidor.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón de apagado).</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método muestra un cuadro de texto para que el usuario ingrese la contraseña de apagado. Si la contraseña
        /// es válida (no vacía), se envía una solicitud al servidor para apagarlo. Si no se ingresa la contraseña, muestra un error.
        /// </remarks>
        private async void PowerButton_Click(object sender, RoutedEventArgs e)
        {
            // Muestra un cuadro de entrada para que el usuario ingrese la contraseña
            string password = Microsoft.VisualBasic.Interaction.InputBox("Introduce la contraseña para apagar el servidor", "Confirmar Apagado", "");

            // Verifica si el usuario no ha ingresado ninguna contraseña
            if (string.IsNullOrEmpty(password))
            {
                // Si no se ingresó la contraseña, muestra un mensaje de error
                MessageBox.Show("Debes ingresar una contraseña.");
                return;
            }

            // Si la contraseña es válida, llama al método para enviar la solicitud de apagado al servidor
            await client.SendShutdownRequestAsync(client.Username, password, client.SessionToken);
        }





        /// <summary>
        /// Maneja el evento de clic en el botón para introducir nueva información sobre el servidor.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón).</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método solicita al usuario que ingrese nueva información sobre el servidor a través de un cuadro de texto.
        /// Si el usuario no ingresa nada, muestra un mensaje de error. Si la información es válida, se envía al servidor.
        /// </remarks>
        private async void BtnIntroducirInfoSobre_Click(object sender, RoutedEventArgs e)
        {
            // Muestra un cuadro de entrada para que el usuario ingrese la nueva información sobre el servidor
            string newInfo = Microsoft.VisualBasic.Interaction.InputBox("Introduce la nueva información sobre el servidor", "Nueva Información sobre el Servidor", "");

            // Verifica si el usuario no ingresó ninguna información
            if (string.IsNullOrEmpty(newInfo))
            {
                // Si no se ingresó la información, muestra un mensaje de error
                MessageBox.Show("Debes ingresar la nueva información.");
                return;
            }

            // Si la nueva información es válida, llama al método para enviar la actualización al servidor
            await client.SendServerInfoUpdateRequestAsync(client.Username, newInfo, client.SessionToken);
        }


    }
}
