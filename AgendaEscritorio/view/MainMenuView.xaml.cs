using System.Windows;
using System.Windows.Controls;

namespace AgendaEscritorio.view
{
    public partial class MainMenuView : Window
    {
        private string userRole; // Variable para almacenar el rol del usuario

        public MainMenuView(string role)
        {
            InitializeComponent();
            userRole = role;
            ConfigureMenuBasedOnRole();
        }

        // Configurar el menú en función del rol del usuario
        private void ConfigureMenuBasedOnRole()
        {
            Dispatcher.Invoke(() =>
            {
                if (userRole == "admin")
                {
                    btnAjustes.Visibility = Visibility.Visible; // Muestra el botón si el rol es Admin
                }
                else
                {
                    btnAjustes.Visibility = Visibility.Collapsed; // Oculta el botón si no es Admin
                }
            });
        }

        // Evento para abrir la ventana del calendario
        // Evento para abrir la ventana del calendario
        private void BtnAgenda_Click(object sender, RoutedEventArgs e)
        {
            CalendarView calendarView = new CalendarView(userRole); // Pasa el rol de usuario
            calendarView.Show(); // Abrir CalendarView como nueva ventana
            this.Close();  // Cerrar el menú principal
        }


        // Evento para cerrar sesión y volver a la ventana de login
        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            loginView loginView = new loginView();
            loginView.Show();
            this.Close();  // Cerramos el menú principal y abrimos la ventana de login
        }

        // Evento para abrir la vista de ajustes
        private void BtnAjustes_Click(object sender, RoutedEventArgs e)
        {
            AjustesView ajustesView = new AjustesView();
            ajustesView.Show(); // Abrir AjustesView como nueva ventana
            this.Close();  // Cerrar el menú principal
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
