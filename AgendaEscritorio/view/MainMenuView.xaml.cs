﻿using AgendaEscritorio.service;
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

        // Evento para mostrar/ocultar el panel de Petición Manual
        private void BtnPeticionManual_Click(object sender, RoutedEventArgs e)
        {
            if (panelPeticionManual.Visibility == Visibility.Visible)
            {
                panelPeticionManual.Visibility = Visibility.Collapsed;
            }
            else
            {
                panelPeticionManual.Visibility = Visibility.Visible;
            }
        }

        // Evento para enviar la petición manual
        private async void BtnEnviarPeticion_Click(object sender, RoutedEventArgs e)
        {
            string peticion = txtPeticionManual.Text;

            if (string.IsNullOrWhiteSpace(peticion))
            {
                MessageBox.Show("Por favor, introduce una petición válida.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Llamar al método SendCustomPacketAsync en tu clase Client
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


        private async void PowerButton_Click(object sender, RoutedEventArgs e)
        {
            // Mostrar un cuadro de texto para la contraseña
            string password = Microsoft.VisualBasic.Interaction.InputBox("Introduce la contraseña para apagar el servidor", "Confirmar Apagado", "");

            // Verificar si el usuario ingresó una contraseña
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Debes ingresar una contraseña.");
                return;
            }

            // Llamar al método para apagar el servidor
            await client.SendShutdownRequestAsync(client.Username, password, client.SessionToken);
        }

        private async void BtnIntroducirInfoSobre_Click(object sender, RoutedEventArgs e)
        {
            // Mostrar un cuadro de texto para que el usuario ingrese la nueva información sobre el servidor
            string newInfo = Microsoft.VisualBasic.Interaction.InputBox("Introduce la nueva información sobre el servidor", "Nueva Información sobre el Servidor", "");

            // Verificar si el usuario ingresó la nueva información
            if (string.IsNullOrEmpty(newInfo))
            {
                MessageBox.Show("Debes ingresar la nueva información.");
                return;
            }

            // Llamar al método para enviar la nueva información al servidor
            await client.SendServerInfoUpdateRequestAsync(client.Username, newInfo, client.SessionToken);
        }


    }
}
