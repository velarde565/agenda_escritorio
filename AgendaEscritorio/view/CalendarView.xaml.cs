using AgendaEscritorio.service;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace AgendaEscritorio.view
{
    public partial class CalendarView : Window
    {
        private DateTime currentDate;
        private string userRole;
        private Client client;

        public CalendarView(Client client)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized; // Abrir la ventana a pantalla completa
            this.client = client;
            currentDate = DateTime.Now; // Inicializa con la fecha actual
            PopulateCalendar();
        }

        private void PopulateCalendar()
        {
            // Limpiar días anteriores
            DaysGrid.Children.Clear();

            // Agregar las iniciales de los días de la semana
            string[] weekDays = { "L", "M", "X", "J", "V", "S", "D" };
            foreach (var day in weekDays)
            {
                TextBlock dayText = new TextBlock
                {
                    Text = day,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold
                };
                DaysGrid.Children.Add(dayText);
            }

            // Muestra el mes y el año actual
            MonthYearText.Text = currentDate.ToString("MMMM yyyy");
            // Obtener el primer día del mes y el número de días en el mes
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            // Obtener el día de la semana del primer día del mes
            // Ajustar para que la semana comience en lunes
            int startDay = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            // Agregar días vacíos antes del primer día del mes
            for (int i = 0; i < startDay; i++)
            {
                DaysGrid.Children.Add(new Button { Content = "", IsEnabled = false, Background = Brushes.Transparent }); // Botones vacíos
            }
            // Agregar los días del mes
            for (int day = 1; day <= daysInMonth; day++)
            {
                var dayButton = new Button
                {
                    Content = day.ToString(),
                    Background = new SolidColorBrush(Color.FromRgb(0x00, 0x7B, 0xA7)), // Color de fondo
                    Foreground = Brushes.White, // Texto en blanco
                    BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0x5F, 0x6B)), // Borde igual que el de las flechas
                    Margin = new Thickness(5), // Margen de 5
                    Padding = new Thickness(10, 5, 10, 5), // Ajusta el padding
                    BorderThickness = new Thickness(1) // Ancho del borde
                };
                DaysGrid.Children.Add(dayButton);
            }
        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            currentDate = currentDate.AddMonths(-1);
            PopulateCalendar();
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            currentDate = currentDate.AddMonths(1);
            PopulateCalendar();
        }

        // Método para cerrar la ventana
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Cierra la ventana
        }

        // Método para minimizar la ventana
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Minimiza la ventana
        }

        // Método para maximizar o restaurar la ventana
        private void MaximizeRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                MaximizeRestoreButton.Content = "☐";  // Cambia al icono de maximizar
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                MaximizeRestoreButton.Content = "";  // Cambia al icono de restaurar
            }
        }

        // Método para volver al menú principal
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuView mainMenuView = new MainMenuView(client); // Pasar el rol de usuario
            mainMenuView.Show();
            this.Close();
        }

        // Métodos de eventos para la gestión de eventos
        private void CrearEvento_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Crear nuevo evento - Implementar lógica aquí");
        }

        private void VerEvento_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ver detalles del evento - Implementar lógica aquí");
        }

        private void ModificarEvento_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Modificar evento - Implementar lógica aquí");
        }

        private void EliminarEvento_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Eliminar evento - Implementar lógica aquí");

        }


    }
}