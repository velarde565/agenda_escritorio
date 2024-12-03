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

        /// <summary>
        /// Inicializa la ventana del calendario.
        /// </summary>
        /// <param name="client">Instancia del cliente que se usará para interactuar con el servidor.</param>
        public CalendarView(Client client)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized; // Abrir la ventana a pantalla completa
            this.client = client;
            currentDate = DateTime.Now; // Inicializa con la fecha actual
            PopulateCalendar(); // Llama al método para poblar el calendario con el mes actual
        }

        /// <summary>
        /// Rellena la vista del calendario con los días del mes actual.
        /// </summary>
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
                DaysGrid.Children.Add(dayText); // Añade el día de la semana a la cuadrícula
            }

            // Muestra el mes y el año actual en el TextBlock correspondiente
            MonthYearText.Text = currentDate.ToString("MMMM yyyy");

            // Obtener el primer día del mes y el número de días en el mes
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

            // Obtener el día de la semana del primer día del mes (ajustado para que la semana comience en lunes)
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
                    Content = day.ToString(), // Número del día
                    Background = new SolidColorBrush(Color.FromRgb(0x00, 0x7B, 0xA7)), // Color de fondo
                    Foreground = Brushes.White, // Texto en blanco
                    BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0x5F, 0x6B)), // Borde igual que el de las flechas
                    Margin = new Thickness(5), // Margen de 5
                    Padding = new Thickness(10, 5, 10, 5), // Ajusta el padding
                    BorderThickness = new Thickness(1) // Ancho del borde
                };
                DaysGrid.Children.Add(dayButton); // Añade el botón para el día al calendario
            }
        }

        /// <summary>
        /// Cambia al mes anterior y actualiza el calendario.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón de mes anterior).</param>
        /// <param name="e">Los argumentos del evento.</param>
        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            currentDate = currentDate.AddMonths(-1); // Retrocede un mes
            PopulateCalendar(); // Actualiza el calendario con el mes anterior
        }

        /// <summary>
        /// Cambia al siguiente mes y actualiza el calendario.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón de siguiente mes).</param>
        /// <param name="e">Los argumentos del evento.</param>
        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            currentDate = currentDate.AddMonths(1); // Avanza un mes
            PopulateCalendar(); // Actualiza el calendario con el mes siguiente
        }

        /// <summary>
        /// Cierra la ventana de la aplicación.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón de cerrar).</param>
        /// <param name="e">Los argumentos del evento.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Cierra la ventana
        }

        /// <summary>
        /// Minimiza la ventana de la aplicación.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón de minimizar).</param>
        /// <param name="e">Los argumentos del evento.</param>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Minimiza la ventana
        }

        /// <summary>
        /// Maximiza o restaura la ventana dependiendo del estado actual.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón de maximizar/restaurar).</param>
        /// <param name="e">Los argumentos del evento.</param>
        private void MaximizeRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal; // Restaura la ventana a su tamaño normal
                MaximizeRestoreButton.Content = "☐";  // Cambia al icono de maximizar
            }
            else
            {
                this.WindowState = WindowState.Maximized; // Maximiza la ventana
                MaximizeRestoreButton.Content = "";  // Cambia al icono de restaurar
            }
        }


        /// <summary>
        /// Vuelve al menú principal al hacer clic en el botón "Atrás".
        /// Cierra la ventana actual y abre la vista del menú principal.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón de "Atrás").</param>
        /// <param name="e">Los argumentos del evento.</param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuView mainMenuView = new MainMenuView(client); // Crea una nueva instancia de la vista de menú principal, pasando el cliente actual.
            mainMenuView.Show(); // Muestra la vista del menú principal.
            this.Close(); // Cierra la ventana actual.
        }



        /// <summary>
        /// Gestiona la visibilidad de los paneles en función de la acción seleccionada.
        /// </summary>
        /// <param name="mostrarPanel">El panel que debe mostrarse. Los demás se ocultarán.</param>
        private void MostrarUnicoPanel(UIElement mostrarPanel)
        {
            // Oculta todos los paneles
            crearEventoPanel.Visibility = Visibility.Collapsed;
            crearGrupoPanel.Visibility = Visibility.Collapsed;
            eliminarGrupoPanel.Visibility = Visibility.Collapsed;

            // Muestra el panel solicitado, si no es nulo
            if (mostrarPanel != null)
            {
                mostrarPanel.Visibility = Visibility.Visible;
            }
        }




        /// <summary>
        /// Muestra los campos necesarios para crear un evento al hacer clic en el botón "Crear Evento".
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón "Crear Evento").</param>
        /// <param name="e">Los argumentos del evento.</param>
        private void CrearEvento_Click(object sender, RoutedEventArgs e)
        {
            MostrarUnicoPanel(crearEventoPanel);

            // Configurar visibilidad de controles específicos
            textFechaLabel.Visibility = Visibility.Visible;
            txtFecha.Visibility = Visibility.Visible;
            textContenidoLabel.Visibility = Visibility.Visible;
            txtContenido.Visibility = Visibility.Visible;
            textTagsLabel.Visibility = Visibility.Visible;
            txtTags.Visibility = Visibility.Visible;
            chkGrupal.Visibility = Visibility.Visible;
            textNombreGrupoLabel.Visibility = Visibility.Visible;
            txtNombreGrupo.Visibility = Visibility.Visible;
            btnEnviar.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// Envía los datos del evento cuando se hace clic en el botón "Enviar Evento".
        /// Recoge los datos de los campos de la interfaz, valida la información, y hace la solicitud al servidor.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón "Enviar Evento").</param>
        /// <param name="e">Los argumentos del evento.</param>
        private async void EnviarEvento_Click(object sender, RoutedEventArgs e)
        {
            // Recoge los datos de la interfaz de usuario
            string fecha = txtFecha.Text.Trim();
            string contenido = txtContenido.Text.Trim();
            string tags = txtTags.Text.Trim();
            bool esGrupal = chkGrupal.IsChecked == true;
            string nombreGrupo = txtNombreGrupo?.Text.Trim();

            // Validación básica de los campos
            if (string.IsNullOrEmpty(fecha) || string.IsNullOrEmpty(contenido))
            {
                MessageBox.Show("Por favor, completa los campos obligatorios (fecha y contenido).");
                return;
            }

            if (esGrupal && string.IsNullOrEmpty(nombreGrupo))
            {
                MessageBox.Show("El nombre del grupo es obligatorio para un evento grupal.");
                return;
            }

            try
            {
                // Llama al método del cliente para enviar la solicitud de creación de evento
                await client.RequestCreateDayAsync(client.SessionToken, client.Username, fecha, contenido, tags, esGrupal, nombreGrupo);

                // Muestra un mensaje de éxito
                MessageBox.Show("Evento creado exitosamente.");
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre una excepción al crear el evento
                MessageBox.Show($"Error al crear el evento: {ex.Message}");
            }
        }



        private void CrearGrupo_Click(object sender, RoutedEventArgs e)
        {
            MostrarUnicoPanel(crearGrupoPanel);

            // Configurar visibilidad de controles específicos
            textNombreGrupoLabelCrear.Visibility = Visibility.Visible;
            txtNombreGrupoCrear.Visibility = Visibility.Visible;
            btnEnviarGrupo.Visibility = Visibility.Visible;
        }




        // Este es el método para manejar el clic del botón "Enviar Grupo"
        private async void EnviarGrupo_Click(object sender, RoutedEventArgs e)
        {
            // Recoge los datos de la interfaz de usuario
            string nombreGrupo = txtNombreGrupoCrear.Text.Trim();

            // Validación básica del campo
            if (string.IsNullOrEmpty(nombreGrupo))
            {
                MessageBox.Show("Por favor, introduce un nombre para el grupo.");
                return;
            }

            try
            {
                // Llama al método del cliente para enviar la solicitud de creación de grupo
                await client.RequestCreateGroupAsync(client.SessionToken, client.Username, nombreGrupo);

                // Muestra un mensaje de éxito
                MessageBox.Show("Grupo creado exitosamente.");

                // Oculta los controles después de crear el grupo exitosamente
                textNombreGrupoLabelCrear.Visibility = Visibility.Collapsed;
                txtNombreGrupoCrear.Visibility = Visibility.Collapsed;
                btnEnviarGrupo.Visibility = Visibility.Collapsed;

                // (Opcional) Limpia el campo de texto
                txtNombreGrupoCrear.Text = string.Empty;
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre una excepción
                MessageBox.Show($"Error al crear el grupo: {ex.Message}");

                // Opcional: También puedes ocultar los controles aquí si lo deseas
                textNombreGrupoLabelCrear.Visibility = Visibility.Collapsed;
                txtNombreGrupoCrear.Visibility = Visibility.Collapsed;
                btnEnviarGrupo.Visibility = Visibility.Collapsed;
            }
        }



        private void EliminarGrupo_Click(object sender, RoutedEventArgs e)
        {
            MostrarUnicoPanel(eliminarGrupoPanel);

            // Configurar visibilidad de controles específicos
            textNombreGrupoLabelEliminar.Visibility = Visibility.Visible;
            txtNombreGrupoEliminar.Visibility = Visibility.Visible;
            btnEnviarEliminarGrupo.Visibility = Visibility.Visible;
        }



        private async void EnviarEliminarGrupo_Click(object sender, RoutedEventArgs e)
        {
            // Recoge los datos de la interfaz de usuario
            string nombreGrupo = txtNombreGrupoEliminar.Text.Trim();

            // Validación básica del campo
            if (string.IsNullOrEmpty(nombreGrupo))
            {
                MessageBox.Show("Por favor, introduce un nombre para el grupo.");
                return;
            }

            try
            {
                // Llama al método del cliente para enviar la solicitud de eliminación de grupo
                await client.RequestDeleteGroupAsync(client.SessionToken, client.Username, nombreGrupo);

                // Muestra un mensaje de éxito
                MessageBox.Show("Grupo eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre una excepción
                MessageBox.Show($"Error al eliminar el grupo: {ex.Message}");
            }
        }


        private async void VerGruposPropietario_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Llama al método del cliente para enviar la solicitud de ver grupos propios
                var grupos = await client.RequestViewOwnedGroupsAsync(client.SessionToken, client.Username);

                // Verifica si se encontraron grupos
                if (grupos.Count > 0)
                {
                    // Muestra los grupos en la interfaz (puedes adaptarlo a cómo deseas mostrar la información)
                    string gruposList = string.Join("\n", grupos);
                    MessageBox.Show($"Grupos propios:\n{gruposList}");

                }
                else
                {
                    MessageBox.Show("No se encontraron grupos propios.");
                }
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre una excepción
                MessageBox.Show($"Error al obtener los grupos propios: {ex.Message}");
            }
        }



        private async void VerGruposMembresia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Llama al método del cliente para enviar la solicitud de ver grupos donde es miembro
                var grupos = await client.RequestViewMembershipGroupsAsync(client.SessionToken, client.Username);

                // Verifica si se encontraron grupos
                if (grupos.Count > 0)
                {
                    // Muestra los grupos en la interfaz
                    string gruposList = string.Join("\n", grupos);
                    MessageBox.Show($"Grupos donde eres miembro:\n{gruposList}");
                }
                else
                {
                    MessageBox.Show("No se encontraron grupos donde seas miembro.");
                }
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre una excepción
                MessageBox.Show($"Error al obtener los grupos de membresía: {ex.Message}");
            }
        }




        private async void VerTodosGrupos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Llama al método del cliente para enviar la solicitud de ver todos los grupos
                var grupos = await client.RequestViewAllGroupsAsync(client.SessionToken, client.Username);

                // Verifica si se encontraron grupos
                if (grupos.Count > 0)
                {
                    // Muestra los grupos en la interfaz
                    string gruposList = string.Join("\n", grupos);
                    MessageBox.Show($"Todos los grupos:\n{gruposList}");
                }
                else
                {
                    MessageBox.Show("No se encontraron grupos en el servidor.");
                }
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre una excepción
                MessageBox.Show($"Error al obtener los grupos: {ex.Message}");
            }
        }







        /// <summary>
        /// Muestra los detalles del evento cuando se hace clic en el botón "Ver Evento".
        /// Este método debe implementar la lógica para mostrar los detalles del evento.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón "Ver Evento").</param>
        /// <param name="e">Los argumentos del evento.</param>
        private void VerEvento_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ver detalles del evento - Implementar lógica aquí");
        }

        /// <summary>
        /// Permite modificar un evento cuando se hace clic en el botón "Modificar Evento".
        /// Este método debe implementar la lógica para editar un evento existente.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón "Modificar Evento").</param>
        /// <param name="e">Los argumentos del evento.</param>
        private void ModificarEvento_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Modificar evento - Implementar lógica aquí");
        }

        /// <summary>
        /// Elimina un evento cuando se hace clic en el botón "Eliminar Evento".
        /// Este método debe implementar la lógica para eliminar un evento existente.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón "Eliminar Evento").</param>
        /// <param name="e">Los argumentos del evento.</param>
        private void EliminarEvento_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Eliminar evento - Implementar lógica aquí");
        }



    }
}