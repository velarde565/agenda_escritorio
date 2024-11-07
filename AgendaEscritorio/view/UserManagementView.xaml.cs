using AgendaEscritorio.service;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AgendaEscritorio.view
{
    public partial class UserManagementView : Window
    {
        private Client client;

        public UserManagementView(Client client)
        {
            InitializeComponent();
            this.client = client;
            ConfigureEditMenu();
        }

        private void ConfigureEditMenu()
        {
            Dispatcher.Invoke(() =>
            {
                btnCambiarFechaNacimiento.Visibility = client.IsAdmin == '1' ? Visibility.Visible : Visibility.Collapsed;
            });
        }

        private void BtnEditarUsuario_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Visibility = Visibility.Collapsed; // Ocultar el menú principal
            editMenu.Visibility = Visibility.Visible; // Mostrar el menú de edición
            HideAndClearInputFields(); // Ocultar y limpiar todos los campos de entrada
        }

        private void CambiarNombreCompleto(object sender, RoutedEventArgs e)
        {
            HideAndClearInputFields(); // Ocultar y limpiar otros campos de entrada
            inputFields.Visibility = Visibility.Visible; // Mostrar los campos de entrada para el cambio de nombre
        }

        private void CambiarFechaNacimiento(object sender, RoutedEventArgs e)
        {
            HideAndClearInputFields(); // Ocultar y limpiar otros campos de entrada
            inputFieldsFechaNacimiento.Visibility = Visibility.Visible; // Mostrar los campos de entrada para la fecha de nacimiento
        }

        private void CambiarOtrosDatos(object sender, RoutedEventArgs e)
        {
            HideAndClearInputFields(); // Ocultar y limpiar otros campos de entrada
            inputFieldsOtrosDatos.Visibility = Visibility.Visible; // Mostrar los campos de entrada para otros datos
        }

        private void CambiarContrasena(object sender, RoutedEventArgs e)
        {
            HideAndClearInputFields(); // Ocultar y limpiar otros campos de entrada
            inputFieldsCambioContrasena.Visibility = Visibility.Visible; // Mostrar los campos de entrada para cambiar contraseña
        }

        private void MostrarDatosUsuario(object sender, RoutedEventArgs e)
        {
            HideAndClearInputFields();
            inputFieldsMostrarDatos.Visibility = Visibility.Visible;
        }


        private void HideAndClearInputFields()
        {
            // Ocultar todos los campos de entrada
            inputFields.Visibility = Visibility.Collapsed;
            inputFieldsFechaNacimiento.Visibility = Visibility.Collapsed;
            inputFieldsOtrosDatos.Visibility = Visibility.Collapsed;
            inputFieldsCambioContrasena.Visibility = Visibility.Collapsed;
            inputFieldsMostrarDatos.Visibility = Visibility.Collapsed;

            // Limpiar los campos de entrada
            txtUsuarioCambio.Clear();
            txtNuevoNombre.Clear();
            txtUsuarioCambioFecha.Clear();
            txtNuevaFechaNacimiento.Clear();
            txtUsuarioCambioOtros.Clear();
            txtOtrosDatos.Clear();
            txtUsuarioCambioContrasena.Clear();
            txtContrasenaActual.Clear();
            txtNuevaContrasena.Clear();
            txtUsuarioAMostrar.Clear();
        }


        private void ConfirmarCambios_Click(object sender, RoutedEventArgs e)
        {
            string usuarioCambio = txtUsuarioCambio.Text.Trim();
            string nuevoNombre = txtNuevoNombre.Text.Trim();

            if (string.IsNullOrEmpty(usuarioCambio) || string.IsNullOrEmpty(nuevoNombre))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            // Aquí simplemente recoges los datos
            MessageBox.Show($"Nombre de usuario '{usuarioCambio}' cambiado a '{nuevoNombre}'.");

            // Limpiar los campos después de la acción
            HideAndClearInputFields(); // Ocultar y limpiar todos los campos de entrada nuevamente
        }

        private async void ConfirmarCambioNombreCompleto_Click(object sender, RoutedEventArgs e)
        {
            string usuarioACambiar = txtUsuarioCambio.Text.Trim();
            string nuevoNombreCompleto = txtNuevoNombre.Text.Trim();

            if (string.IsNullOrEmpty(usuarioACambiar) || string.IsNullOrEmpty(nuevoNombreCompleto))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            await client.RequestChangeFullNameAsync(client.SessionToken, usuarioACambiar, nuevoNombreCompleto);

            // Limpiar los campos de entrada después de la acción
            HideAndClearInputFields();
        }


        private async void ConfirmarCambioFechaNacimiento_Click(object sender, RoutedEventArgs e)
        {
            string usuarioCambio = txtUsuarioCambioFecha.Text.Trim();
            string nuevaFechaNacimiento = txtNuevaFechaNacimiento.Text.Trim();

            // Validar el formato de la fecha
            if (string.IsNullOrEmpty(usuarioCambio) || string.IsNullOrEmpty(nuevaFechaNacimiento))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            // Verificar si la fecha es válida y no está en el futuro
            if (!DateTime.TryParseExact(nuevaFechaNacimiento, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fechaNacimiento) || fechaNacimiento > DateTime.Now)
            {
                MessageBox.Show("La fecha de nacimiento debe ser válida y no puede ser futura.");
                return;
            }

            // Llamar al cliente para solicitar el cambio
            await client.RequestChangeBirthDateAsync(client.SessionToken, usuarioCambio, nuevaFechaNacimiento, client.Username);

            // Limpiar los campos después de la acción
            HideAndClearInputFields(); // Ocultar y limpiar todos los campos de entrada nuevamente
        }


        private async void ConfirmarCambioOtrosDatos_Click(object sender, RoutedEventArgs e)
        {
            string usuarioCambio = txtUsuarioCambioOtros.Text.Trim();
            string otrosDatos = txtOtrosDatos.Text.Trim();

            if (string.IsNullOrEmpty(usuarioCambio) || string.IsNullOrEmpty(otrosDatos))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            try
            {
                // Llamada asíncrona al método para cambiar otros datos en el servidor
                await client.RequestChangeOtherDataAsync(client.SessionToken, usuarioCambio, otrosDatos);

                MessageBox.Show($"Otros datos del usuario '{usuarioCambio}' cambiados correctamente.");

                // Limpiar los campos de entrada después de la acción
                HideAndClearInputFields(); // Ocultar y limpiar todos los campos de entrada nuevamente
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar los otros datos: {ex.Message}");
            }
        }


        private async void MostrarDatosUsuario_Click(object sender, RoutedEventArgs e)
        {
            string usuarioAMostrar = txtUsuarioAMostrar.Text.Trim(); // Obtener el nombre de usuario del TextBox

            if (string.IsNullOrEmpty(usuarioAMostrar))
            {
                MessageBox.Show("Por favor, introduce un nombre de usuario.");
                return;
            }

            // Establecer username2 como el nombre de usuario que se quiere mostrar
            string usernameToChange = usuarioAMostrar;

            // Llama al método RequestUserDataAsync con el token de sesión y el nombre de usuario a mostrar
            await client.RequestUserDataAsync(client.SessionToken, client.Username, usernameToChange); // Usa el token, el usuario a mostrar y el usuario conectado

            HideAndClearInputFields();

            // Limpiar el campo de entrada después de la acción (opcional)
            txtUsuarioAMostrar.Clear();
        }



        private async void ConfirmarCambioContrasena_Click(object sender, RoutedEventArgs e)
        {
            string usuarioCambio = txtUsuarioCambioContrasena.Text.Trim();
            string contrasenaActual = txtContrasenaActual.Password.Trim(); // Usar .Password para obtener el texto
            string nuevaContrasena = txtNuevaContrasena.Password.Trim(); // Usar .Password para obtener el texto

            if (string.IsNullOrEmpty(usuarioCambio) || string.IsNullOrEmpty(contrasenaActual) || string.IsNullOrEmpty(nuevaContrasena))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            await client.RequestChangePasswordAsync(client.SessionToken, usuarioCambio, contrasenaActual, nuevaContrasena, client.Username);
            // Aquí simplemente recoges los datos


            // Limpiar los campos después de la acción
            HideAndClearInputFields(); // Ocultar y limpiar todos los campos de entrada nuevamente
        }







        private void BtnCrearUsuario_Click(object sender, RoutedEventArgs e)
        {
            // Ocultar otros paneles
            mainMenu.Visibility = Visibility.Collapsed;
            editMenu.Visibility = Visibility.Collapsed;
            inputFields.Visibility = Visibility.Collapsed;
            inputFieldsFechaNacimiento.Visibility = Visibility.Collapsed;
            inputFieldsOtrosDatos.Visibility = Visibility.Collapsed;
            inputFieldsCambioContrasena.Visibility = Visibility.Collapsed;
            inputFieldsMostrarDatos.Visibility = Visibility.Collapsed;
            inputFieldsEliminarUsuario.Visibility = Visibility.Collapsed;

            // Mostrar el panel de creación de usuario
            inputFieldsCrearUsuario.Visibility = Visibility.Visible;
        }

        private async void ConfirmarCreacionUsuario_Click(object sender, RoutedEventArgs e)
        {
            // Actualiza los nombres de las variables para que coincidan con los controles en el XAML
            string nuevoNombreUsuario = txtCrearUsuarioNombreUsuario.Text.Trim();
            string nuevaContrasena = txtCrearUsuarioContrasena.Password.Trim();
            string nuevoNombreCompleto = txtCrearUsuarioNombreCompleto.Text.Trim();
            string nuevaFechaNacimiento = txtCrearUsuarioFechaNacimiento.Text.Trim();
            string otrosDatos = txtCrearUsuarioOtrosDatos.Text.Trim();
            string rolPermisos = txtCrearUsuarioRolPermisos.Text.Trim();

            if (string.IsNullOrEmpty(nuevoNombreUsuario) || string.IsNullOrEmpty(nuevaContrasena) ||
                string.IsNullOrEmpty(nuevoNombreCompleto) || string.IsNullOrEmpty(nuevaFechaNacimiento) ||
                string.IsNullOrEmpty(rolPermisos))
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.");
                return;
            }

            // Llamar al cliente para solicitar la creación del nuevo usuario
            await client.RequestCreateUserAsync(client.SessionToken, client.Username, nuevoNombreUsuario, nuevaContrasena, nuevoNombreCompleto, nuevaFechaNacimiento, otrosDatos, rolPermisos);

            // Limpiar y ocultar el campo de entrada después de la creación
            txtCrearUsuarioNombreUsuario.Clear();
            txtCrearUsuarioContrasena.Clear();
            txtCrearUsuarioNombreCompleto.Clear();
            txtCrearUsuarioFechaNacimiento.Clear();
            txtCrearUsuarioOtrosDatos.Clear();
            txtCrearUsuarioRolPermisos.Clear();
            inputFieldsCrearUsuario.Visibility = Visibility.Collapsed;
            mainMenu.Visibility = Visibility.Visible;
        }



        private void BtnEliminarUsuario_Click(object sender, RoutedEventArgs e)
        {
            // Ocultar otros paneles
            mainMenu.Visibility = Visibility.Collapsed;
            editMenu.Visibility = Visibility.Collapsed;
            inputFields.Visibility = Visibility.Collapsed;
            inputFieldsFechaNacimiento.Visibility = Visibility.Collapsed;
            inputFieldsOtrosDatos.Visibility = Visibility.Collapsed;
            inputFieldsCambioContrasena.Visibility = Visibility.Collapsed;
            inputFieldsMostrarDatos.Visibility = Visibility.Collapsed;

            // Mostrar el panel de eliminación de usuario
            inputFieldsEliminarUsuario.Visibility = Visibility.Visible;
        }

        // Confirmación de eliminación
        private async void ConfirmarEliminacionUsuario_Click(object sender, RoutedEventArgs e)
        {
            string usuarioAEliminar = txtUsuarioAEliminar.Text.Trim();

            if (string.IsNullOrEmpty(usuarioAEliminar))
            {
                MessageBox.Show("Por favor, ingrese el nombre de usuario a eliminar.");
                return;
            }

            // Llamar al cliente para solicitar la eliminación
            await client.RequestDeleteUserAsync(client.SessionToken, usuarioAEliminar, client.Username);

            // Limpiar y ocultar el campo de entrada después de la eliminación
            txtUsuarioAEliminar.Clear();
            inputFieldsEliminarUsuario.Visibility = Visibility.Collapsed;
            mainMenu.Visibility = Visibility.Visible;
        }



        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Cerrar la ventana o volver a la vista anterior
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
