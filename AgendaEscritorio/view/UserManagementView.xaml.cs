using AgendaEscritorio.service;
using System;
<<<<<<< HEAD
using System.Collections.Generic;
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
using System.Windows;
using System.Windows.Controls;

namespace AgendaEscritorio.view
{
<<<<<<< HEAD
    /// <summary>
    /// Ventana de gestión de usuarios. Permite realizar distintas acciones sobre los usuarios,
    /// como editar su nombre completo, fecha de nacimiento, otros datos, contraseña, y rol.
    /// </summary>
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
    public partial class UserManagementView : Window
    {
        private Client client;

<<<<<<< HEAD
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UserManagementView"/>.
        /// </summary>
        /// <param name="client">Instancia del cliente que se usará para las comunicaciones con el servidor.</param>
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
        public UserManagementView(Client client)
        {
            InitializeComponent();
            this.client = client;
<<<<<<< HEAD
            ConfigureEditMenu(); // Configura el menú de edición según el rol del usuario
        }

        /// <summary>
        /// Configura el menú de edición basado en los privilegios del usuario.
        /// Si el usuario es administrador, habilita la opción de cambiar la fecha de nacimiento.
        /// </summary>
=======
            ConfigureEditMenu();
        }

>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
        private void ConfigureEditMenu()
        {
            Dispatcher.Invoke(() =>
            {
<<<<<<< HEAD
                // Si el cliente es administrador, se muestra el botón de cambiar fecha de nacimiento
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
                btnCambiarFechaNacimiento.Visibility = client.IsAdmin == '1' ? Visibility.Visible : Visibility.Collapsed;
            });
        }

<<<<<<< HEAD
        /// <summary>
        /// Evento que se llama al hacer clic en el botón de editar usuario.
        /// Oculta el menú principal y muestra el menú de edición de usuario.
        /// </summary>
        private void BtnEditarUsuario_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Visibility = Visibility.Collapsed; // Oculta el menú principal
            editMenu.Visibility = Visibility.Visible; // Muestra el menú de edición
            HideAndClearInputFields(); // Limpia y oculta los campos de entrada
        }

        /// <summary>
        /// Evento que se llama al hacer clic en la opción de cambiar el nombre completo del usuario.
        /// Muestra los campos de entrada para modificar el nombre completo.
        /// </summary>
        private void CambiarNombreCompleto(object sender, RoutedEventArgs e)
        {
            HideAndClearInputFields(); // Ocultar y limpiar otros campos de entrada
            inputFields.Visibility = Visibility.Visible; // Mostrar los campos para el cambio de nombre
        }

        /// <summary>
        /// Evento que se llama al hacer clic en la opción de cambiar la fecha de nacimiento del usuario.
        /// Muestra los campos de entrada para modificar la fecha de nacimiento.
        /// </summary>
        private void CambiarFechaNacimiento(object sender, RoutedEventArgs e)
        {
            HideAndClearInputFields(); // Ocultar y limpiar otros campos de entrada
            inputFieldsFechaNacimiento.Visibility = Visibility.Visible; // Mostrar los campos para cambiar la fecha de nacimiento
        }

        /// <summary>
        /// Evento que se llama al hacer clic en la opción de cambiar otros datos del usuario.
        /// Muestra los campos de entrada para modificar otros datos.
        /// </summary>
        private void CambiarOtrosDatos(object sender, RoutedEventArgs e)
        {
            HideAndClearInputFields(); // Ocultar y limpiar otros campos de entrada
            inputFieldsOtrosDatos.Visibility = Visibility.Visible; // Mostrar los campos para cambiar otros datos
        }

        /// <summary>
        /// Evento que se llama al hacer clic en la opción de cambiar la contraseña del usuario.
        /// Muestra los campos de entrada para modificar la contraseña.
        /// </summary>
        private void CambiarContrasena(object sender, RoutedEventArgs e)
        {
            HideAndClearInputFields(); // Ocultar y limpiar otros campos de entrada
            inputFieldsCambioContrasena.Visibility = Visibility.Visible; // Mostrar los campos para cambiar la contraseña
        }

        /// <summary>
        /// Evento que se llama al hacer clic en la opción de mostrar los datos del usuario.
        /// Muestra los campos de entrada para visualizar los datos del usuario.
        /// </summary>
        private void MostrarDatosUsuario(object sender, RoutedEventArgs e)
        {
            HideAndClearInputFields(); // Ocultar y limpiar otros campos de entrada
            inputFieldsMostrarDatos.Visibility = Visibility.Visible; // Mostrar los campos para mostrar los datos
        }

        /// <summary>
        /// Evento que se llama al hacer clic en la opción de cambiar el rol del usuario.
        /// Muestra el panel de creación de rol.
        /// </summary>
        private void CambiarRol(object sender, RoutedEventArgs e)
        {
            mainMenu.Visibility = Visibility.Collapsed; // Ocultar el menú principal
            inputFieldsCrearRol.Visibility = Visibility.Visible; // Mostrar el panel de creación de rol
        }




        /// <summary>
        /// Oculta y limpia todos los campos de entrada en el formulario de gestión de usuario.
        /// </summary>
=======
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


>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
        private void HideAndClearInputFields()
        {
            // Ocultar todos los campos de entrada
            inputFields.Visibility = Visibility.Collapsed;
            inputFieldsFechaNacimiento.Visibility = Visibility.Collapsed;
            inputFieldsOtrosDatos.Visibility = Visibility.Collapsed;
            inputFieldsCambioContrasena.Visibility = Visibility.Collapsed;
            inputFieldsMostrarDatos.Visibility = Visibility.Collapsed;
<<<<<<< HEAD
            inputFieldsCrearRol.Visibility = Visibility.Collapsed;
            inputFieldsMostrarPermisos.Visibility = Visibility.Collapsed; // Ocultar el panel de mostrar permisos
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7

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
<<<<<<< HEAD
            txtNuevoRol.Clear();
            chkAdmin.IsChecked = false; // Limpiar los checkboxes
            chkCrearAgenda.IsChecked = false;
            chkCrearGrupos.IsChecked = false;
            chkCrearAgendaGrupal.IsChecked = false;
            chkEliminarAgendaPropia.IsChecked = false;
            chkEliminarGrupos.IsChecked = false;
            chkEliminarAgendaGrupal.IsChecked = false;
        }

        /// <summary>
        /// Evento que se llama al hacer clic en el botón de confirmar cambios de nombre de usuario.
        /// Verifica que los campos estén completos y luego muestra un mensaje con la confirmación.
        /// </summary>
=======
        }


>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
        private void ConfirmarCambios_Click(object sender, RoutedEventArgs e)
        {
            string usuarioCambio = txtUsuarioCambio.Text.Trim();
            string nuevoNombre = txtNuevoNombre.Text.Trim();

<<<<<<< HEAD
            // Validar que los campos no estén vacíos
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
            if (string.IsNullOrEmpty(usuarioCambio) || string.IsNullOrEmpty(nuevoNombre))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

<<<<<<< HEAD
            // Mostrar el mensaje de confirmación
=======
            // Aquí simplemente recoges los datos
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
            MessageBox.Show($"Nombre de usuario '{usuarioCambio}' cambiado a '{nuevoNombre}'.");

            // Limpiar los campos después de la acción
            HideAndClearInputFields(); // Ocultar y limpiar todos los campos de entrada nuevamente
        }

<<<<<<< HEAD
        /// <summary>
        /// Evento que se llama al hacer clic en el botón de confirmar el cambio de nombre completo.
        /// Valida que los campos estén completos y luego llama al cliente para realizar la solicitud de cambio de nombre.
        /// </summary>
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
        private async void ConfirmarCambioNombreCompleto_Click(object sender, RoutedEventArgs e)
        {
            string usuarioACambiar = txtUsuarioCambio.Text.Trim();
            string nuevoNombreCompleto = txtNuevoNombre.Text.Trim();

<<<<<<< HEAD
            // Validar que los campos no estén vacíos
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
            if (string.IsNullOrEmpty(usuarioACambiar) || string.IsNullOrEmpty(nuevoNombreCompleto))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

<<<<<<< HEAD
            // Llamar al cliente para realizar la solicitud de cambio de nombre completo
            await client.RequestChangeFullNameAsync(client.SessionToken, usuarioACambiar, nuevoNombreCompleto);

            // Limpiar los campos después de la acción
            HideAndClearInputFields();
        }

        /// <summary>
        /// Evento que se llama al hacer clic en el botón de confirmar el cambio de fecha de nacimiento.
        /// Valida el formato y la validez de la fecha antes de realizar la solicitud de cambio.
        /// </summary>
=======
            await client.RequestChangeFullNameAsync(client.SessionToken, usuarioACambiar, nuevoNombreCompleto);

            // Limpiar los campos de entrada después de la acción
            HideAndClearInputFields();
        }


>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
        private async void ConfirmarCambioFechaNacimiento_Click(object sender, RoutedEventArgs e)
        {
            string usuarioCambio = txtUsuarioCambioFecha.Text.Trim();
            string nuevaFechaNacimiento = txtNuevaFechaNacimiento.Text.Trim();

<<<<<<< HEAD
            // Validar que los campos no estén vacíos
=======
            // Validar el formato de la fecha
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
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

<<<<<<< HEAD
            // Llamar al cliente para solicitar el cambio de fecha de nacimiento
=======
            // Llamar al cliente para solicitar el cambio
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
            await client.RequestChangeBirthDateAsync(client.SessionToken, usuarioCambio, nuevaFechaNacimiento, client.Username);

            // Limpiar los campos después de la acción
            HideAndClearInputFields(); // Ocultar y limpiar todos los campos de entrada nuevamente
        }


<<<<<<< HEAD
        /// <summary>
        /// Evento que se llama al hacer clic en el botón para confirmar el cambio de otros datos de un usuario.
        /// Valida que los campos estén completos, realiza la solicitud de cambio de datos al servidor y muestra una confirmación.
        /// </summary>
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
        private async void ConfirmarCambioOtrosDatos_Click(object sender, RoutedEventArgs e)
        {
            string usuarioCambio = txtUsuarioCambioOtros.Text.Trim();
            string otrosDatos = txtOtrosDatos.Text.Trim();

<<<<<<< HEAD
            // Validar que los campos no estén vacíos
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
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
<<<<<<< HEAD
                // Mostrar mensaje de error si ocurre algún problema
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
                MessageBox.Show($"Error al cambiar los otros datos: {ex.Message}");
            }
        }

<<<<<<< HEAD
        /// <summary>
        /// Evento que se llama al hacer clic en el botón para mostrar los datos de un usuario.
        /// Verifica que el campo de nombre de usuario no esté vacío y luego solicita los datos del usuario al servidor.
        /// </summary>
=======

>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
        private async void MostrarDatosUsuario_Click(object sender, RoutedEventArgs e)
        {
            string usuarioAMostrar = txtUsuarioAMostrar.Text.Trim(); // Obtener el nombre de usuario del TextBox

<<<<<<< HEAD
            // Validar que el campo no esté vacío
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
            if (string.IsNullOrEmpty(usuarioAMostrar))
            {
                MessageBox.Show("Por favor, introduce un nombre de usuario.");
                return;
            }

            // Establecer username2 como el nombre de usuario que se quiere mostrar
            string usernameToChange = usuarioAMostrar;

            // Llama al método RequestUserDataAsync con el token de sesión y el nombre de usuario a mostrar
            await client.RequestUserDataAsync(client.SessionToken, client.Username, usernameToChange); // Usa el token, el usuario a mostrar y el usuario conectado

<<<<<<< HEAD
            // Limpiar los campos de entrada después de la acción
            HideAndClearInputFields();

            // Limpiar el campo de entrada opcionalmente
            txtUsuarioAMostrar.Clear();
        }

        /// <summary>
        /// Evento que se llama al hacer clic en el botón para confirmar el cambio de contraseña.
        /// Valida que los campos estén completos y realiza la solicitud de cambio de contraseña al servidor.
        /// </summary>
=======
            HideAndClearInputFields();

            // Limpiar el campo de entrada después de la acción (opcional)
            txtUsuarioAMostrar.Clear();
        }



>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
        private async void ConfirmarCambioContrasena_Click(object sender, RoutedEventArgs e)
        {
            string usuarioCambio = txtUsuarioCambioContrasena.Text.Trim();
            string contrasenaActual = txtContrasenaActual.Password.Trim(); // Usar .Password para obtener el texto
            string nuevaContrasena = txtNuevaContrasena.Password.Trim(); // Usar .Password para obtener el texto

<<<<<<< HEAD
            // Validar que los campos no estén vacíos
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
            if (string.IsNullOrEmpty(usuarioCambio) || string.IsNullOrEmpty(contrasenaActual) || string.IsNullOrEmpty(nuevaContrasena))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

<<<<<<< HEAD
            // Llamada asíncrona para solicitar el cambio de contraseña al servidor
            await client.RequestChangePasswordAsync(client.SessionToken, usuarioCambio, contrasenaActual, nuevaContrasena, client.Username);

            // Limpiar los campos de entrada después de la acción
=======
            await client.RequestChangePasswordAsync(client.SessionToken, usuarioCambio, contrasenaActual, nuevaContrasena, client.Username);
            // Aquí simplemente recoges los datos


            // Limpiar los campos después de la acción
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
            HideAndClearInputFields(); // Ocultar y limpiar todos los campos de entrada nuevamente
        }







<<<<<<< HEAD
        /// <summary>
        /// Evento que se llama al hacer clic en el botón para crear un nuevo usuario.
        /// Este evento oculta los paneles no relacionados y muestra el panel de creación de usuario.
        /// </summary>
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
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

<<<<<<< HEAD
        /// <summary>
        /// Evento que se llama al hacer clic en el botón para confirmar la creación de un nuevo usuario.
        /// Valida los campos de entrada, solicita la creación del usuario al servidor, y limpia los campos después.
        /// </summary>
        private async void ConfirmarCreacionUsuario_Click(object sender, RoutedEventArgs e)
        {
            // Obtener los datos del nuevo usuario desde los controles del formulario
=======
        private async void ConfirmarCreacionUsuario_Click(object sender, RoutedEventArgs e)
        {
            // Actualiza los nombres de las variables para que coincidan con los controles en el XAML
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
            string nuevoNombreUsuario = txtCrearUsuarioNombreUsuario.Text.Trim();
            string nuevaContrasena = txtCrearUsuarioContrasena.Password.Trim();
            string nuevoNombreCompleto = txtCrearUsuarioNombreCompleto.Text.Trim();
            string nuevaFechaNacimiento = txtCrearUsuarioFechaNacimiento.Text.Trim();
            string otrosDatos = txtCrearUsuarioOtrosDatos.Text.Trim();
            string rolPermisos = txtCrearUsuarioRolPermisos.Text.Trim();

<<<<<<< HEAD
            // Validación de campos obligatorios
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
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

<<<<<<< HEAD
        /// <summary>
        /// Evento que se llama al hacer clic en el botón para eliminar un usuario.
        /// Este evento oculta los paneles no relacionados y muestra el panel de eliminación de usuario.
        /// </summary>
=======


>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
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

<<<<<<< HEAD
        /// <summary>
        /// Evento que se llama al hacer clic en el botón para confirmar la eliminación de un usuario.
        /// Valida el nombre de usuario a eliminar, solicita la eliminación al servidor, y limpia los campos después.
        /// </summary>
        private async void ConfirmarEliminacionUsuario_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el nombre del usuario a eliminar
            string usuarioAEliminar = txtUsuarioAEliminar.Text.Trim();

            // Validación de campo obligatorio
=======
        // Confirmación de eliminación
        private async void ConfirmarEliminacionUsuario_Click(object sender, RoutedEventArgs e)
        {
            string usuarioAEliminar = txtUsuarioAEliminar.Text.Trim();

>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
            if (string.IsNullOrEmpty(usuarioAEliminar))
            {
                MessageBox.Show("Por favor, ingrese el nombre de usuario a eliminar.");
                return;
            }

<<<<<<< HEAD
            // Llamar al cliente para solicitar la eliminación del usuario
=======
            // Llamar al cliente para solicitar la eliminación
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
            await client.RequestDeleteUserAsync(client.SessionToken, usuarioAEliminar, client.Username);

            // Limpiar y ocultar el campo de entrada después de la eliminación
            txtUsuarioAEliminar.Clear();
            inputFieldsEliminarUsuario.Visibility = Visibility.Collapsed;
            mainMenu.Visibility = Visibility.Visible;
        }


<<<<<<< HEAD
        /// <summary>
        /// Maneja el clic en el botón "Crear Rol". 
        /// Oculta el menú principal y muestra el panel de creación de rol.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (en este caso, el botón).</param>
        /// <param name="e">Argumentos del evento.</param>
        private void BtnCrearRol_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Visibility = Visibility.Collapsed; // Ocultar el menú principal
            inputFieldsCrearRol.Visibility = Visibility.Visible; // Mostrar el panel de creación de rol
        }

        /// <summary>
        /// Maneja el clic en el botón de confirmación para crear un nuevo rol.
        /// Valida los campos y llama al cliente para solicitar la creación del rol con los permisos seleccionados.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (en este caso, el botón).</param>
        /// <param name="e">Argumentos del evento.</param>
        private async void ConfirmarCreacionRol_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el nombre del rol
            string nuevoRol = txtNuevoRol.Text.Trim();

            // Validar si el nombre del rol está vacío
            if (string.IsNullOrEmpty(nuevoRol))
            {
                MessageBox.Show("Por favor, ingrese el nombre del rol.");
                return;
            }

            // Generar la cadena de permisos basada en las casillas de verificación
            string permisos = $"{(chkAdmin.IsChecked == true ? "1" : "0")}," +
                              $"{(chkCrearAgenda.IsChecked == true ? "1" : "0")}," +
                              $"{(chkCrearGrupos.IsChecked == true ? "1" : "0")}," +
                              $"{(chkCrearAgendaGrupal.IsChecked == true ? "1" : "0")}," +
                              $"{(chkEliminarAgendaPropia.IsChecked == true ? "1" : "0")}," +
                              $"{(chkEliminarGrupos.IsChecked == true ? "1" : "0")}," +
                              $"{(chkEliminarAgendaGrupal.IsChecked == true ? "1" : "0")}";

            try
            {
                // Llamar al cliente para solicitar la creación del rol
                await client.RequestAddPermissionAsync(client.SessionToken, client.Username, nuevoRol, permisos);

                // Mensaje de confirmación
                MessageBox.Show("Rol creado exitosamente.");

                // Limpiar los campos después de la creación
                txtNuevoRol.Clear();
                chkAdmin.IsChecked = false;
                chkCrearAgenda.IsChecked = false;
                chkCrearGrupos.IsChecked = false;
                chkCrearAgendaGrupal.IsChecked = false;
                chkEliminarAgendaPropia.IsChecked = false;
                chkEliminarGrupos.IsChecked = false;
                chkEliminarAgendaGrupal.IsChecked = false;

                // Ocultar los campos y mostrar el menú principal
                HideAndClearInputFields();
            }
            catch (Exception ex)
            {
                // Manejo de excepciones al crear el rol
                MessageBox.Show($"Error al crear el rol: {ex.Message}");
            }
        }

        /// <summary>
        /// Diccionario que almacena los roles y sus permisos asociados.
        /// </summary>
        private Dictionary<string, string> rolesAndPermissions = new Dictionary<string, string>();

        /// <summary>
        /// Maneja el clic en el botón "Mostrar Permisos". 
        /// Oculta el menú principal y muestra el panel de permisos.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (en este caso, el botón).</param>
        /// <param name="e">Argumentos del evento.</param>
        private void BtnMostrarPermisos_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Visibility = Visibility.Collapsed; // Ocultar el menú principal
            inputFieldsMostrarPermisos.Visibility = Visibility.Visible; // Mostrar el panel para mostrar permisos
        }

        /// <summary>
        /// Maneja el clic en el botón "Consultar Permisos de Rol". 
        /// Solicita los permisos del rol seleccionado y los muestra en un ComboBox.
        /// </summary>
        /// <param name="sender">El objeto que genera el evento (en este caso, el botón).</param>
        /// <param name="e">Argumentos del evento.</param>
        /// <exception cref="Exception">Se lanza si hay un error al obtener los permisos del rol.</exception>
        private async void ConsultarPermisosRol_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Llamar al cliente para solicitar los permisos del rol
                rolesAndPermissions = await client.RequestGetPermissionsAsync(client.SessionToken, client.Username);

                // Cargar los roles en el ComboBox
                cmbRolesMostrarPermisos.Items.Clear(); // Limpiar los elementos actuales del ComboBox
                foreach (var role in rolesAndPermissions.Keys)
                {
                    cmbRolesMostrarPermisos.Items.Add(role); // Agregar los roles al ComboBox
                }

                // Mensaje de confirmación de roles cargados
                MessageBox.Show("Roles cargados correctamente. Selecciona un rol para ver sus permisos.");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones al obtener los permisos
                MessageBox.Show($"Error al obtener permisos: {ex.Message}");
            }
        }



        /// <summary>
        /// Maneja el cambio de selección en el ComboBox de roles para mostrar los permisos asociados.
        /// </summary>
        /// <param name="sender">El objeto que desencadena el evento, en este caso el ComboBox.</param>
        /// <param name="e">Los argumentos del evento, que contienen información sobre la selección.</param>
        private void cmbRolesMostrarPermisos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verificar si hay una selección válida
            if (cmbRolesMostrarPermisos.SelectedItem == null)
                return;

            // Obtener el rol seleccionado
            string rolSeleccionado = cmbRolesMostrarPermisos.SelectedItem.ToString();

            // Verificar si el rol está en el diccionario de roles y permisos
            if (rolesAndPermissions.ContainsKey(rolSeleccionado))
            {
                // Obtener los permisos para el rol seleccionado
                string permisos = rolesAndPermissions[rolSeleccionado];

                // Actualizar los checkboxes según los permisos recibidos
                chkAdminMostrar.IsChecked = permisos[0] == '1';
                chkCrearAgendaMostrar.IsChecked = permisos[1] == '1';
                chkCrearGruposMostrar.IsChecked = permisos[2] == '1';
                chkCrearAgendaGrupalMostrar.IsChecked = permisos[3] == '1';
                chkEliminarAgendaPropiaMostrar.IsChecked = permisos[4] == '1';
                chkEliminarGruposMostrar.IsChecked = permisos[5] == '1';
                chkEliminarAgendaGrupalMostrar.IsChecked = permisos[6] == '1';
            }
            else
            {
                MessageBox.Show("El rol seleccionado no tiene permisos asociados.");
            }
        }

        /// <summary>
        /// Confirma la edición de los permisos para el rol seleccionado y actualiza el servidor.
        /// </summary>
        /// <param name="sender">El objeto que desencadena el evento, en este caso el botón de confirmación.</param>
        /// <param name="e">Los argumentos del evento.</param>
        private async void ConfirmarEdicion_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el nombre de usuario, rol y permisos
            string username = client.Username;  // Nombre de usuario conectado
            string sessionToken = client.SessionToken;  // Token de sesión
            string rol = cmbRolesMostrarPermisos.SelectedItem.ToString();  // Nombre del rol seleccionado

            // Obtener los permisos de los CheckBoxes
            bool[] permisos = new bool[]
            {
        chkAdmin.IsChecked ?? false,
        chkCrearAgendaMostrar.IsChecked ?? false,
        chkCrearGruposMostrar.IsChecked ?? false,
        chkCrearAgendaGrupalMostrar.IsChecked ?? false,
        chkEliminarAgendaPropiaMostrar.IsChecked ?? false,
        chkEliminarGruposMostrar.IsChecked ?? false,
        chkEliminarAgendaGrupalMostrar.IsChecked ?? false
            };

            // Llamar al método de envío de permisos
            int respuesta = await client.SendEditPermissionsAsync(sessionToken, username, rol, permisos);

            // Comprobar la respuesta del servidor
            if (respuesta == 213)
            {
                MessageBox.Show("Permisos editados con éxito.");
            }
            else if (respuesta == 112)
            {
                MessageBox.Show("Error: No tiene permisos suficientes.");
            }
            else if (respuesta == 1301)
            {
                MessageBox.Show("Error: Permiso no válido.");
            }
            else if (respuesta == 1106)
            {
                MessageBox.Show("Error: Formato de permisos incorrecto.");
            }
            else if (respuesta == 18)
            {
                MessageBox.Show("Error: El modo gestión no está activo.");
            }
            else
            {
                MessageBox.Show($"Error desconocido: {respuesta}");
            }
        }

        /// <summary>
        /// Activa el modo gestión para el usuario actual.
        /// </summary>
        /// <param name="sender">El objeto que desencadena el evento, en este caso el botón para activar el modo gestión.</param>
        /// <param name="e">Los argumentos del evento.</param>
        private async void BtnModoGestion_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el nombre de usuario y el token de sesión
            string sessionToken = client.SessionToken;  // Token de sesión
            string username = client.Username;  // Nombre de usuario conectado

            // Llamar al método para activar el modo gestión
            int respuesta = await client.SendModoGestionAsync(sessionToken, username, true);  // True para activar el modo gestión

            // Comprobar la respuesta del servidor
            if (respuesta == 213)
            {
                MessageBox.Show("Modo gestión activado.");
            }
            else if (respuesta == 112)
            {
                MessageBox.Show("Error: No tiene permisos suficientes.");
            }
            else
            {
                MessageBox.Show($"Error desconocido: {respuesta}");
            }
        }




        /// <summary>
        /// Maneja la acción del botón "Volver", cerrando la ventana actual o regresando a la vista anterior.
        /// </summary>
        /// <param name="sender">El objeto que desencadena el evento, en este caso el botón "Volver".</param>
        /// <param name="e">Los argumentos del evento.</param>
=======

>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Cerrar la ventana o volver a la vista anterior
        }

<<<<<<< HEAD
        /// <summary>
        /// Maneja la acción del botón para minimizar la ventana actual.
        /// </summary>
        /// <param name="sender">El objeto que desencadena el evento, en este caso el botón de minimizar.</param>
        /// <param name="e">Los argumentos del evento.</param>
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

<<<<<<< HEAD
        /// <summary>
        /// Alterna entre maximizar y restaurar la ventana actual según su estado.
        /// </summary>
        /// <param name="sender">El objeto que desencadena el evento, en este caso el botón de maximizar/restaurar.</param>
        /// <param name="e">Los argumentos del evento.</param>
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
        private void MaximizeRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

<<<<<<< HEAD
        /// <summary>
        /// Maneja la acción del botón para cerrar la ventana actual.
        /// </summary>
        /// <param name="sender">El objeto que desencadena el evento, en este caso el botón de cerrar.</param>
        /// <param name="e">Los argumentos del evento.</param>
=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
<<<<<<< HEAD

=======
>>>>>>> 89df6b4b1f1043e90658fa9d098020598874adf7
    }
}
