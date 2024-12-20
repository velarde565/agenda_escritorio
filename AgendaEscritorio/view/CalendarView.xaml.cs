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
        /// Cambia al mes anterior, actualiza el calendario y solicita al servidor retroceder un mes.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón de mes anterior).</param>
        /// <param name="e">Los argumentos del evento.</param>
        private async void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            // Primero, retrocedemos el mes localmente en la vista del calendario
            currentDate = currentDate.AddMonths(-1); // Retrocede un mes
            PopulateCalendar(); // Actualiza el calendario con el mes anterior

            try
            {

                // Llamar al método para retroceder el mes en el servidor
                await client.RequestGoBackMonthAsync(client.SessionToken, client.Username);

                // Feedback opcional para el usuario
                MessageBox.Show("Solicitud para retroceder un mes enviada correctamente.");
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si algo falla
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// Cambia al siguiente mes, actualiza el calendario y solicita al servidor avanzar un mes.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón de siguiente mes).</param>
        /// <param name="e">Los argumentos del evento.</param>
        private async void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            // Actualizar la vista local del calendario
            currentDate = currentDate.AddMonths(1); // Avanza un mes
            PopulateCalendar(); // Actualiza el calendario con el mes siguiente

            try
            {

                // Llamar al método para enviar la solicitud al servidor
                await client.RequestAdvanceMonthAsync(client.SessionToken, client.Username);

                // Feedback opcional para el usuario
                MessageBox.Show("Solicitud para avanzar un mes enviada correctamente.");
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si algo falla
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// Cambia al año anterior, actualiza el calendario y solicita al servidor retroceder un año.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón de año anterior).</param>
        /// <param name="e">Los argumentos del evento.</param>
        private async void PreviousYear_Click(object sender, RoutedEventArgs e)
        {
            // Retroceder el año localmente
            currentDate = currentDate.AddYears(-1); // Retrocede un año
            PopulateCalendar(); // Actualiza el calendario con el año anterior

            try
            {
                // Llamar al método para retroceder un año en el servidor
                await client.RequestGoBackYearAsync(client.SessionToken, client.Username);

                // Feedback opcional para el usuario
                MessageBox.Show("Solicitud para retroceder un año enviada correctamente.");
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si algo falla
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Cambia al siguiente año, actualiza el calendario y solicita al servidor avanzar un año.
        /// </summary>
        /// <param name="sender">El objeto que disparó el evento (el botón de siguiente año).</param>
        /// <param name="e">Los argumentos del evento.</param>
        private async void NextYear_Click(object sender, RoutedEventArgs e)
        {
            // Avanzar el año localmente
            currentDate = currentDate.AddYears(1); // Avanza un año
            PopulateCalendar(); // Actualiza el calendario con el año siguiente

            try
            {
                // Llamar al método para avanzar el año en el servidor
                await client.RequestAdvanceYearAsync(client.SessionToken, client.Username);

                // Feedback opcional para el usuario
                MessageBox.Show("Solicitud para avanzar un año enviada correctamente.");
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si algo falla
                MessageBox.Show($"Error: {ex.Message}");
            }
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



        /// <summary>
        /// Maneja el evento de clic en el botón para crear un grupo.
        /// Muestra el panel de creación de grupo y configura la visibilidad de los controles necesarios.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón de crear grupo).</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Muestra el panel de creación de grupo (`crearGrupoPanel`).
        /// 2. Configura la visibilidad de los controles relacionados con la creación de grupo, como:
        ///    - La etiqueta del nombre del grupo (`textNombreGrupoLabelCrear`).
        ///    - El campo de texto para ingresar el nombre del grupo (`txtNombreGrupoCrear`).
        ///    - El botón para enviar la solicitud de creación de grupo (`btnEnviarGrupo`).
        /// El propósito es permitir al usuario ingresar los datos necesarios para crear un nuevo grupo.
        /// </remarks>
        private void CrearGrupo_Click(object sender, RoutedEventArgs e)
        {
            // Mostrar el panel de creación de grupo
            MostrarUnicoPanel(crearGrupoPanel);

            // Configurar visibilidad de los controles específicos
            textNombreGrupoLabelCrear.Visibility = Visibility.Visible;
            txtNombreGrupoCrear.Visibility = Visibility.Visible;
            btnEnviarGrupo.Visibility = Visibility.Visible;
        }




        /// <summary>
        /// Maneja el evento de clic en el botón "Enviar Grupo", que envía la solicitud para crear un nuevo grupo.
        /// Recoge los datos de la interfaz de usuario, valida la entrada, y envía la solicitud al servidor.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón de enviar grupo).</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Recoge el nombre del grupo desde el campo de texto `txtNombreGrupoCrear`.
        /// 2. Realiza una validación básica para asegurarse de que el nombre del grupo no esté vacío.
        /// 3. Si la validación falla, muestra un mensaje indicando que se debe ingresar un nombre.
        /// 4. Si la validación es exitosa, se envía la solicitud de creación de grupo al servidor utilizando `RequestCreateGroupAsync`.
        /// 5. Si la solicitud es exitosa, muestra un mensaje de éxito y oculta los controles de creación de grupo.
        /// 6. Si ocurre una excepción durante el proceso, muestra un mensaje de error y también oculta los controles de creación de grupo.
        /// 7. (Opcional) Limpia el campo de texto después de un intento exitoso de creación.
        /// </remarks>
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




        /// <summary>
        /// Maneja el evento de clic en el botón "Eliminar Grupo", mostrando el panel de eliminación de grupo.
        /// Configura la visibilidad de los controles necesarios para permitir al usuario introducir el nombre del grupo que desea eliminar.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón de eliminar grupo).</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Muestra el panel de eliminación de grupo (`eliminarGrupoPanel`), permitiendo al usuario introducir los datos necesarios.
        /// 2. Configura la visibilidad de los controles relacionados con la eliminación del grupo, como la etiqueta de nombre de grupo (`textNombreGrupoLabelEliminar`),
        ///    el campo de texto para el nombre del grupo (`txtNombreGrupoEliminar`), y el botón para enviar la solicitud de eliminación (`btnEnviarEliminarGrupo`).
        /// </remarks>
        private void EliminarGrupo_Click(object sender, RoutedEventArgs e)
        {
            // Muestra el panel para eliminar un grupo
            MostrarUnicoPanel(eliminarGrupoPanel);

            // Configura visibilidad de controles específicos para la eliminación de grupo
            textNombreGrupoLabelEliminar.Visibility = Visibility.Visible;
            txtNombreGrupoEliminar.Visibility = Visibility.Visible;
            btnEnviarEliminarGrupo.Visibility = Visibility.Visible;
        }




        /// <summary>
        /// Maneja el evento de clic en el botón "Enviar Eliminar Grupo", enviando una solicitud para eliminar el grupo especificado.
        /// Valida los datos ingresados y maneja posibles excepciones durante la eliminación.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón de enviar eliminación de grupo).</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Recoge el nombre del grupo a eliminar desde el campo de texto (`txtNombreGrupoEliminar`).
        /// 2. Valida que el campo de texto no esté vacío y muestra un mensaje si es necesario.
        /// 3. Llama al método `RequestDeleteGroupAsync` para enviar la solicitud de eliminación del grupo.
        /// 4. Si la eliminación es exitosa, muestra un mensaje de éxito.
        /// 5. Si ocurre una excepción durante el proceso, muestra un mensaje de error.
        /// 6. Se asegura de que, después de la operación, el estado de la interfaz de usuario se actualice adecuadamente.
        /// </remarks>
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

                // Muestra un mensaje de éxito si la eliminación es exitosa
                MessageBox.Show("Grupo eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre una excepción
                MessageBox.Show($"Error al eliminar el grupo: {ex.Message}");
            }
        }



        /// <summary>
        /// Maneja el evento de clic en el botón "Ver Grupos Propios". Envía una solicitud para obtener los grupos propios del usuario 
        /// y muestra los resultados en la interfaz de usuario. Si no se encuentran grupos o si ocurre un error, muestra un mensaje adecuado.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón "Ver Grupos Propios").</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Llama al método `RequestViewOwnedGroupsAsync` del cliente para obtener los grupos propios del usuario.
        /// 2. Verifica si se encontraron grupos y los muestra en un cuadro de mensaje.
        /// 3. Si no se encuentran grupos, muestra un mensaje indicando que no hay grupos propios.
        /// 4. Si ocurre una excepción (por ejemplo, problemas de red o servidor), muestra un mensaje de error.
        /// </remarks>
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



        /// <summary>
        /// Maneja el evento de clic en el botón "Ver Grupos de Membresía". Envía una solicitud para obtener los grupos donde el usuario es miembro 
        /// y muestra los resultados en la interfaz de usuario. Si no se encuentran grupos o si ocurre un error, muestra un mensaje adecuado.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón "Ver Grupos de Membresía").</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Llama al método `RequestViewMembershipGroupsAsync` del cliente para obtener los grupos donde el usuario es miembro.
        /// 2. Verifica si se encontraron grupos y los muestra en un cuadro de mensaje.
        /// 3. Si no se encuentran grupos, muestra un mensaje indicando que el usuario no es miembro de ningún grupo.
        /// 4. Si ocurre una excepción (por ejemplo, problemas de red o servidor), muestra un mensaje de error.
        /// </remarks>
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





        /// <summary>
        /// Maneja el evento de clic en el botón "Ver Todos los Grupos". Envía una solicitud para obtener todos los grupos disponibles 
        /// y muestra los resultados en la interfaz de usuario. Si no se encuentran grupos o si ocurre un error, muestra un mensaje adecuado.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón "Ver Todos los Grupos").</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Llama al método `RequestViewAllGroupsAsync` del cliente para obtener todos los grupos disponibles en el servidor.
        /// 2. Verifica si se encontraron grupos y los muestra en un cuadro de mensaje.
        /// 3. Si no se encuentran grupos, muestra un mensaje indicando que no hay grupos disponibles en el servidor.
        /// 4. Si ocurre una excepción (por ejemplo, problemas de red o servidor), muestra un mensaje de error.
        /// </remarks>
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
            // Hacer visible el panel para modificar el evento
            modificarEventoPanel.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// Maneja el evento de clic en el botón "Enviar Modificar Evento". Valida los campos de la interfaz de usuario, 
        /// luego llama al método para modificar un evento en el servidor con los datos proporcionados por el usuario.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón "Enviar Modificar Evento").</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Recoge los datos ingresados en la interfaz de usuario: la fecha del evento, el nuevo contenido y si es un evento grupal o no.
        /// 2. Valida si los campos esenciales (fecha y contenido) están completos, mostrando un mensaje de advertencia si alguno está vacío.
        /// 3. Llama al método `ModificarEventoAsync` del cliente para enviar los datos al servidor y modificar el evento.
        /// 4. Si ocurre un error durante la solicitud, muestra un mensaje con la descripción del error.
        /// </remarks>
        private async void EnviarModificarEvento_Click(object sender, RoutedEventArgs e)
        {
            string fecha = txtFechaEvento.Text;            // Obtiene la fecha del evento desde el campo de texto.
            string nuevoContenido = txtNuevoContenido.Text; // Obtiene el nuevo contenido del evento desde el campo de texto.
            bool esGrupal = chkEsGrupalModificar.IsChecked == true; // Verifica si el evento es grupal.

            // Validar los campos necesarios
            if (string.IsNullOrEmpty(fecha) || string.IsNullOrEmpty(nuevoContenido))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return; // Detiene la ejecución si algún campo está vacío.
            }

            try
            {
                // Llamar al método para editar el evento
                await client.ModificarEventoAsync(client.SessionToken, client.Username, fecha, nuevoContenido, esGrupal);
                MessageBox.Show("Evento modificado exitosamente.");
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si ocurre alguna excepción
                MessageBox.Show($"Error: {ex.Message}");
            }
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





        /// <summary>
        /// Maneja el evento de clic en el botón "Eliminar Agenda Grupal". Muestra el panel de eliminación de la agenda grupal 
        /// y configura la visibilidad de los controles necesarios para completar la acción de eliminación.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón "Eliminar Agenda Grupal").</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Llama al método `MostrarUnicoPanel` para mostrar el panel de eliminación de la agenda grupal.
        /// 2. Configura la visibilidad de los controles necesarios para que el usuario ingrese el nombre del grupo a eliminar.
        /// 3. Asegura que los controles, como los campos de texto y botones, sean visibles para el usuario.
        /// </remarks>
        private void EliminarAgendaGrupal_Click(object sender, RoutedEventArgs e)
        {
            // Muestra el panel de eliminación de la agenda grupal.
            MostrarUnicoPanel(eliminarAgendaGrupalPanel);

            // Configurar visibilidad de controles específicos para la eliminación de la agenda.
            textNombreGrupoLabelEliminarAgenda.Visibility = Visibility.Visible;  // Muestra la etiqueta del nombre del grupo.
            txtNombreGrupoEliminarAgenda.Visibility = Visibility.Visible;        // Muestra el campo de texto para ingresar el nombre del grupo.
            btnEnviarEliminarAgendaGrupal.Visibility = Visibility.Visible;       // Muestra el botón para enviar la solicitud de eliminación.
        }





        /// <summary>
        /// Maneja el evento de clic en el botón "Eliminar Agenda Grupal". Recoge el nombre del grupo desde la interfaz de usuario,
        /// valida el campo y luego envía una solicitud al cliente para eliminar la agenda grupal.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón "Enviar Eliminar Agenda Grupal").</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Recoge el nombre del grupo desde el campo de texto `txtNombreGrupoEliminarAgenda`.
        /// 2. Valida si el campo no está vacío. Si está vacío, muestra un mensaje pidiendo al usuario que introduzca un nombre.
        /// 3. Si el campo está correctamente llenado, llama a la función `RequestDeleteGroupAgendaAsync` del cliente para enviar la solicitud de eliminación.
        /// 4. Si la eliminación es exitosa, muestra un mensaje de éxito.
        /// 5. Si ocurre algún error, muestra un mensaje de error detallado.
        /// </remarks>
        private async void EnviarEliminarAgendaGrupal_Click(object sender, RoutedEventArgs e)
        {
            // Recoge el nombre del grupo desde la interfaz de usuario
            string nombreGrupo = txtNombreGrupoEliminarAgenda.Text.Trim();

            // Validación básica del campo
            if (string.IsNullOrEmpty(nombreGrupo))
            {
                // Muestra un mensaje si el campo está vacío
                MessageBox.Show("Por favor, introduce un nombre para el grupo.");
                return;
            }

            try
            {
                // Llama al cliente para enviar la solicitud de eliminación de la agenda grupal
                await client.RequestDeleteGroupAgendaAsync(client.SessionToken, client.Username, nombreGrupo);

                // Muestra un mensaje de éxito si la solicitud es procesada correctamente
                MessageBox.Show("Agenda grupal eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre una excepción
                MessageBox.Show($"Error al eliminar la agenda grupal: {ex.Message}");
            }
        }






        /// <summary>
        /// Maneja el evento de clic en el botón "Invitar Usuario". Muestra el panel de invitación de usuario y configura la visibilidad
        /// de los controles necesarios para la interacción con el usuario.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón "Invitar Usuario").</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Muestra el panel de invitación de usuario (`invitarUsuarioPanel`) usando el método `MostrarUnicoPanel`.
        /// 2. Configura la visibilidad de los controles necesarios para la interacción:
        ///    - Etiquetas y campos para el nombre del grupo (`textNombreGrupoLabelInvitar` y `txtNombreGrupoInvitar`).
        ///    - Etiqueta y campo para el sobrenombre del usuario a invitar (`textSobrenombreLabelInvitar` y `txtSobrenombreInvitar`).
        ///    - El botón de envío de la invitación (`btnEnviarInvitarUsuario`).
        /// </remarks>
        private void InvitarUsuario_Click(object sender, RoutedEventArgs e)
        {
            // Muestra el panel de invitar usuario
            MostrarUnicoPanel(invitarUsuarioPanel);

            // Configura la visibilidad de los controles específicos
            textNombreGrupoLabelInvitar.Visibility = Visibility.Visible;
            txtNombreGrupoInvitar.Visibility = Visibility.Visible;
            textSobrenombreLabelInvitar.Visibility = Visibility.Visible;
            txtSobrenombreInvitar.Visibility = Visibility.Visible;
            btnEnviarInvitarUsuario.Visibility = Visibility.Visible;
        }






        /// <summary>
        /// Maneja el evento de clic en el botón "Enviar Invitación". Recoge los datos de la interfaz de usuario, valida los campos 
        /// y envía una solicitud para invitar a un usuario a un grupo.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón "Enviar Invitación").</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Recoge los datos ingresados por el usuario:
        ///    - El nombre del grupo (`nombreGrupo`) desde el campo de texto `txtNombreGrupoInvitar`.
        ///    - El sobrenombre del usuario a invitar (`sobrenombreUsuario`) desde el campo de texto `txtSobrenombreInvitar`.
        /// 2. Valida que ambos campos no estén vacíos. Si alguno de los campos está vacío, muestra un mensaje solicitando que se 
        ///    rellenen todos los campos y termina la ejecución.
        /// 3. Si los campos son válidos, llama al método `RequestInviteUserToGroupAsync` del cliente para enviar la solicitud de invitación.
        /// 4. Si la solicitud se envía con éxito, muestra un mensaje indicando que el usuario fue invitado exitosamente.
        /// 5. Si ocurre una excepción durante el proceso, muestra un mensaje de error.
        /// </remarks>
        private async void EnviarInvitarUsuario_Click(object sender, RoutedEventArgs e)
        {
            // Recoge los datos desde la interfaz de usuario
            string nombreGrupo = txtNombreGrupoInvitar.Text.Trim();
            string sobrenombreUsuario = txtSobrenombreInvitar.Text.Trim();

            // Validación básica
            if (string.IsNullOrEmpty(nombreGrupo) || string.IsNullOrEmpty(sobrenombreUsuario))
            {
                MessageBox.Show("Por favor, rellena todos los campos.");
                return;
            }

            try
            {
                // Llama al cliente para enviar la solicitud de invitación
                await client.RequestInviteUserToGroupAsync(client.SessionToken, client.Username, nombreGrupo, sobrenombreUsuario);

                // Mostrar un mensaje de éxito
                MessageBox.Show("Usuario invitado exitosamente.");
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si ocurre una excepción
                MessageBox.Show($"Error al invitar al usuario: {ex.Message}");
            }
        }



        /// <summary>
        /// Maneja el evento de clic en el botón "Mostrar Mes". Recoge los datos de la interfaz de usuario, 
        /// determina si la solicitud es para una agenda grupal o no, y luego hace la solicitud correspondiente.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón "Mostrar Mes").</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Obtiene el estado del CheckBox `chkEsGrupal` para determinar si la solicitud es para una agenda grupal o no.
        ///    - Si el CheckBox está marcado, se considera una solicitud grupal.
        /// 2. Si la solicitud es grupal, recoge el nombre del grupo desde el campo de texto `txtNombreGrupoOpcional`.
        ///    - Si no es grupal, el nombre del grupo se deja vacío.
        /// 3. Llama al método `RequestShowAgendaAsync` del cliente, pasando el token de sesión, el nombre de usuario, 
        ///    si la solicitud es grupal y el nombre del grupo (si es necesario).
        /// 4. Si ocurre un error durante el proceso, muestra un mensaje de error con los detalles.
        /// </remarks>
        private async void MostrarMes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtener el estado del CheckBox (si es grupal o no)
                bool isGrupal = chkEsGrupal.IsChecked == true;

                // Obtener el nombre del grupo, solo si es grupal
                string groupName = isGrupal ? txtNombreGrupoOpcional.Text : string.Empty;

                // Llamar al método para hacer la solicitud
                await client.RequestShowAgendaAsync(client.SessionToken, client.Username, isGrupal, groupName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar la agenda: {ex.Message}");
            }
        }






        /// <summary>
        /// Maneja el evento de clic en el CheckBox "Es Grupal". Habilita o deshabilita el campo de texto 
        /// "Nombre del Grupo" en función de si el CheckBox está marcado o no.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el CheckBox "Es Grupal").</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 1. Comprueba si el CheckBox `chkEsGrupal` está marcado o no. 
        ///    - Si el CheckBox está marcado (es decir, la solicitud es grupal), habilita el campo de texto `txtNombreGrupoOpcional` para que el usuario pueda ingresar el nombre del grupo.
        ///    - Si el CheckBox no está marcado (es decir, la solicitud no es grupal), deshabilita el campo de texto `txtNombreGrupoOpcional` para evitar que el usuario ingrese un nombre de grupo.
        /// </remarks>
        private void ChkEsGrupal_Click(object sender, RoutedEventArgs e)
        {
            // Habilitar o deshabilitar el TextBox según el estado del CheckBox
            txtNombreGrupoOpcional.IsEnabled = chkEsGrupal.IsChecked == true;
        }





        private void txtNombreGrupoOpcional_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Aquí puedes agregar cualquier lógica que necesites
        }






        /// <summary>
        /// Maneja el evento de clic en el botón "Insertar Tag". Muestra el panel para permitir al usuario insertar
        /// un tag y asociarlo a una fecha.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón "Insertar Tag").</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método hace visible el panel `insertarTagPanel`, que contiene los controles necesarios para que
        /// el usuario ingrese un tag y lo asocie a una fecha específica. El panel estará visible para el usuario
        /// después de hacer clic en el botón correspondiente.
        /// </remarks>
        private void InsertarTag_Click(object sender, RoutedEventArgs e)
        {
            // Hacer visible el panel para insertar fecha y tag
            insertarTagPanel.Visibility = Visibility.Visible;
        }






        /// <summary>
        /// Maneja el evento de clic en el botón para insertar un tag. Valida los campos proporcionados por el usuario
        /// y, si son válidos, envía los datos al servidor para asociar el tag con una fecha específica.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón de insertar tag).</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método se activa cuando el usuario hace clic en el botón "Enviar" para insertar un nuevo tag. 
        /// Valida que los campos de fecha y el nuevo tag no estén vacíos. Si los datos son válidos, se llama 
        /// al método `InsertarTagAsync` del cliente para asociar el tag con la fecha en el servidor. En caso de 
        /// error, se muestra un mensaje con la descripción del problema.
        /// </remarks>
        private async void EnviarInsertarTag_Click(object sender, RoutedEventArgs e)
        {
            string fecha = txtFechaTag.Text; // Usar el nuevo nombre aquí
            string nuevoTag = txtNuevoTag.Text;

            // Validación y envío de datos
            if (string.IsNullOrEmpty(fecha) || string.IsNullOrEmpty(nuevoTag))
            {
                MessageBox.Show("Por favor, ingrese todos los campos.");
                return;
            }

            try
            {
                // Llamar al método para insertar el tag con los datos proporcionados
                await client.InsertarTagAsync(client.SessionToken, client.Username, fecha, nuevoTag);
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si ocurre una excepción
                MessageBox.Show($"Error: {ex.Message}");
            }
        }



        /// <summary>
        /// Maneja el evento de clic en el botón para mostrar el panel de eliminación de un tag.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón de eliminar tag).</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método se activa cuando el usuario hace clic en el botón "Eliminar Tag". 
        /// Su propósito es hacer visible el panel donde el usuario podrá especificar la fecha 
        /// y el tag a eliminar. De este modo, el usuario podrá interactuar con los controles 
        /// necesarios para realizar la eliminación del tag.
        /// </remarks>
        private void EliminarTag_Click(object sender, RoutedEventArgs e)
        {
            // Hacer visible el panel de eliminación de tag
            eliminarTagPanel.Visibility = Visibility.Visible;
        }




        /// <summary>
        /// Maneja el evento de clic en el botón para eliminar un tag asociado a una fecha específica.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón de eliminar tag).</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método se activa cuando el usuario hace clic en el botón "Enviar" para eliminar un tag.
        /// Recoge los datos de la interfaz de usuario (fecha y tag), valida que ambos campos estén completos,
        /// y luego llama al servicio para eliminar el tag en el servidor. Si ocurre un error, muestra un mensaje
        /// de error en la interfaz de usuario.
        /// </remarks>
        private async void EnviarEliminarTag_Click(object sender, RoutedEventArgs e)
        {
            // Recoge los valores introducidos por el usuario en los campos de fecha y tag
            string fecha = txtFechaEliminar.Text;
            string tagAEliminar = txtTagEliminar.Text;

            // Validación de los campos
            if (string.IsNullOrEmpty(fecha) || string.IsNullOrEmpty(tagAEliminar))
            {
                // Si alguno de los campos está vacío, muestra un mensaje de error
                MessageBox.Show("Por favor, ingrese todos los campos.");
                return;
            }

            try
            {
                // Llama al método del cliente para eliminar el tag
                await client.EliminarTagAsync(client.SessionToken, client.Username, fecha, tagAEliminar);
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre una excepción
                MessageBox.Show($"Error: {ex.Message}");
            }
        }



        /// <summary>
        /// Maneja el evento de clic en el botón para buscar un tag.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón de buscar tag).</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método se activa cuando el usuario hace clic en el botón "Buscar Tag".
        /// Muestra el panel para realizar la búsqueda de un tag. Además, maneja la visibilidad del panel 
        /// relacionado con los grupos, dependiendo de si el checkbox "Es grupal" está marcado o no.
        /// </remarks>
        private void BuscarTag_Click(object sender, RoutedEventArgs e)
        {
            // Muestra el panel de búsqueda de tag
            buscarTagPanel.Visibility = Visibility.Visible;

            // Maneja la visibilidad del panel de grupo basado en el estado del checkbox "Es grupal"
            chkEsGrupalBuscarTag.Checked += (s, args) =>
            {
                // Si el checkbox "Es grupal" está marcado, muestra el panel del grupo
                panelGrupoBuscarTag.Visibility = Visibility.Visible;
            };

            chkEsGrupalBuscarTag.Unchecked += (s, args) =>
            {
                // Si el checkbox "Es grupal" no está marcado, oculta el panel del grupo
                panelGrupoBuscarTag.Visibility = Visibility.Collapsed;
            };
        }



        /// <summary>
        /// Maneja el evento de clic en el botón para realizar la búsqueda de un tag.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (el botón de buscar tag).</param>
        /// <param name="e">Los argumentos del evento.</param>
        /// <remarks>
        /// Este método se activa cuando el usuario hace clic en el botón "Buscar Tag".
        /// Valida que los campos necesarios estén completos y luego realiza una solicitud de búsqueda de un tag.
        /// Si la búsqueda es grupal, también valida que el nombre del grupo esté ingresado.
        /// </remarks>
        private async void EnviarBuscarTag_Click(object sender, RoutedEventArgs e)
        {
            // Obtiene el tag que el usuario quiere buscar
            string tag = txtTagBuscar.Text;

            // Verifica si la búsqueda es grupal a través del checkbox
            bool esGrupal = chkEsGrupalBuscarTag.IsChecked == true;

            // Obtiene el nombre del grupo, si la búsqueda es grupal
            string nombreGrupo = txtNombreGrupoBuscarTag.Text;

            // Validación de los campos: si no se ingresó el tag o, en caso de ser grupal, el nombre del grupo
            if (string.IsNullOrEmpty(tag) || (esGrupal && string.IsNullOrEmpty(nombreGrupo)))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            try
            {
                // Llama al método para realizar la búsqueda del tag
                await client.SearchTagAsync(client.SessionToken, client.Username, esGrupal, tag, nombreGrupo);
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si algo falla al realizar la búsqueda
                MessageBox.Show($"Error: {ex.Message}");
            }
        }





    }
}