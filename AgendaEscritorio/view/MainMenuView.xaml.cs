using AgendaEscritorio.service;
using System.Windows;
using System.Windows.Controls;

namespace AgendaEscritorio.view
{
    public partial class MainMenuView : Window
    {
        private Client client;
        public MainMenuView(Client client)
        {
            InitializeComponent();
            this.client = client;
            ConfigureMenuBasedOnRole();
        }

        // Configurar el menú en función del rol del usuario
        private void ConfigureMenuBasedOnRole()
        {
            Dispatcher.Invoke(() =>
            {
                if (client.IsAdmin == '1') // Compara si IsAdmin es '1' para tratarlo como un administrador
                {
                    btnAjustes.Visibility = Visibility.Visible;
                }
                else // Si IsAdmin es false (0), ocultamos el botón
                {
                    btnAjustes.Visibility = Visibility.Collapsed;
                }
            });
        }

        // Evento para abrir la ventana del calendario
        private void BtnAgenda_Click(object sender, RoutedEventArgs e)
        {
            CalendarView calendarView = new CalendarView(client); // Pasamos la instancia de Client
            calendarView.Show();
            this.Close();
        }


        // Evento para cerrar sesión y volver a la ventana de login
        private async void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            await client.SendLogoutAsync(); // Llamar al método a través de la instancia de Client

            loginView loginView = new loginView();
            loginView.Show(); // Abre la ventana de login
            this.Close(); // Luego cierra la ventana actual
        }



        // Evento para abrir la vista de ajustes
        private void BtnAjustes_Click(object sender, RoutedEventArgs e)
        {
            AjustesView ajustesView = new AjustesView();
            ajustesView.Show();
            this.Close();
        }

        // Evento para minimizar la ventana
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Minimiza la ventana
        }

        // Evento para maximizar/restaurar la ventana
        private void MaximizeRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized; // Maximiza la ventana
                MaximizeRestoreButton.Content = ""; // Cambia el contenido del botón
            }
            else
            {
                this.WindowState = WindowState.Normal; // Restaura la ventana a su tamaño original
                MaximizeRestoreButton.Content = "☐"; // Cambia el contenido del botón
            }
        }

        // Evento para cerrar la ventana
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Cierra la ventana
        }

        // Evento que se llama al cargar la ventana
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Aquí puedes añadir lógica que necesites al cargar la ventana
        }
    }
}
