   M S B A M L   `   `   `  ����   AgendaEscritorio.    (AgendaEscritorio.view.UserManagementView   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Gestión de Usuarios��$	��450��$	��800�����	#FF1B1448 � WindowStyle$
 None=� � 
ResizeMode$ CanResizeWithGrip=� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=�.��� .+���  �� CornerRadius$ 12�����	#FF1B1448$��0q�._��� $��2q� J� Opacity$
 0.95Q�$ 10������  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,1&�.��� 	 � Color$	 #007BA7��
 � Offset$
 0Q�� $	 #00FFFF��$	
 0.5Q�� $	 #005F6B��$
 1Q�����  �� ImageSource$ ../images/descarga.jpg�� j� Stretch$
 Fill=�._�� .+��� ���	#CC00AABB$ 10��$	 0.9Q�._�� .+���  �� Orientation$ 
Horizontal=�$��Right=�$	��Top=�$��0,5,5,0q�.+��� -   $��-��$��30��
������ 
��������� -    ��Name$ MaximizeRestoreButton��$	��☐��$��30��
������ 
��������� -   $��X��$��30��
������ 
��������� $��Center=�$��Center=�$��0,20,0,0q�.+��� $��Gestión de Usuarios��$��24(�
�������$��Center=�$	��Top=�$��0,20,0,0q��� -    ��Name$ mainMenu��$��Center=�$��Center=�$	��300��.+��� -   $ btnMostrarUsuarios��$��Mostrar Usuarios��$��50�����#007BA7
�������$��0,10,0,0q��� -   $ btnCrearUsuario��$��Crear Usuario��$��50�����#007BA7
�������$��0,10,0,0q��� -   $ btnEditarUsuario��$��Editar Usuario��$��50�����#007BA7
�������$��0,10,0,0q��� -   $ btnEliminarUsuario��$��Eliminar Usuario��$��50�����#007BA7
�������$��0,10,0,0q��� -	   $ btnCrearRol��$��	Crear Rol��$��50�����#007BA7
�������$��0,10,0,0q��� -
   $ btnEliminarRol��$��Elminar Rol��$��50�����#007BA7
�������$��0,10,0,0q��� -   $ btnMostrarPermisos��$��Mostrar Permisos��$��50�����#007BA7
�������$��0,10,0,0q��� -   $ editMenu��$y�	Collapsed=�$��Center=�$��Center=�$	��300��.+��� -   $��Cambiar Contraseña��$��50�����#007BA7
�������$	��0,5q��� -   $��Cambiar Nombre Completo��$��50�����#007BA7
�������$	��0,5q��� -   $ btnCambiarFechaNacimiento��$!��Cambiar Fecha de Nacimiento��$��50�����#007BA7
�������$	��0,5q��� -   $��Cambiar Otros Datos��$��50�����#007BA7
�������$	��0,5q��� -   $��Mostrar Datos de Usuario��$��50�����#007BA7
�������$	��0,5q��� -   $��Volver��$��Center=�$��Bottom=�$��	0,20,0,10q�$	��100���� -   $ inputFields��$y�	Collapsed=�$
��Left=�$	��Top=�$��20q�.+��� $��Usuario a cambiar:��
�������$��0,5,0,0q��� -    ��Name$ txtUsuarioCambio��$	��200��$��0,5,0,10q��� $��Nuevo nombre completo:��
�������$��0,5,0,0q��� -   $ txtNuevoNombre��$	��200��$��0,5,0,10q��� -   $��Confirmar Cambios��$	��200��$��0,10,0,0q��� -   $  inputFieldsFechaNacimiento��$y�	Collapsed=�$
��Left=�$	��Top=�$��20q�.+��� $��Usuario a cambiar:��
�������$��0,5,0,0q��� -   $ txtUsuarioCambioFecha��$	��200��$��0,5,0,10q��� $-��'Nueva fecha de nacimiento (dd/mm/yyyy):��
�������$��0,5,0,0q��� -   $ txtNuevaFechaNacimiento��$	��200��$��0,5,0,10q��� -   $��Confirmar Cambios��$	��200��$��0,10,0,0q��� -   $ inputFieldsOtrosDatos��$y�	Collapsed=�$
��Left=�$	��Top=�$��20q�.+��� $��Usuario a cambiar:��
�������$��0,5,0,0q��� -   $ txtUsuarioCambioOtros��$	��200��$��0,5,0,10q��� $��Otros datos:��
�������$��0,5,0,0q��� -   $ txtOtrosDatos��$	��200��$��0,5,0,10q��� -   $��Confirmar Cambios��$	��200��$��0,10,0,0q��� -   $! inputFieldsCambioContrasena��$y�	Collapsed=�$
��Left=�$	��Top=�$��20q�.+��� $��Usuario a editar:��
�������$��0,5,0,0q��� -    $  txtUsuarioCambioContrasena��$	��200��$��0,5,0,10q��� $��Contraseña actual:��
�������$��0,5,0,0q�G� -!    G�Name$ txtContrasenaActual��$	��200��$��0,5,0,10q��� $��Nueva contraseña:��
�������$��0,5,0,0q�G� -"   $ txtNuevaContrasena��$	��200��$��0,5,0,10q��� -#   $��Confirmar Cambios��$	��200��$��0,10,0,0q��� -$   $ inputFieldsMostrarDatos��$y�	Collapsed=�$
��Left=�$	��Top=�$��20q�.+��� $��Ver datos del usuario:��
�������$��0,5,0,0q��� -%   $ txtUsuarioAMostrar��$	��200��$��0,5,0,10q��� -&   $��Mostrar Datos��$	��200��$��0,10,0,0q��� -'   $  inputFieldsEliminarUsuario��$y�	Collapsed=�$
��Left=�$	��Top=�$��20q�.+��� $��Usuario a eliminar:��
�������$��0,5,0,0q��� -(   $ txtUsuarioAEliminar��$	��200��$��0,5,0,10q��� -)   $��Confirmar Eliminación��$	��200��$��0,10,0,0q��� -*   $ inputFieldsCrearUsuario��$y�	Collapsed=�$
��Left=�$	��Top=�$��20q�.+��� $��Nuevo nombre de usuario:��
�������$��0,5,0,0q��� -+   $" txtCrearUsuarioNombreUsuario��$	��200��$��0,5,0,10q��� $��Contraseña:��
�������$��0,5,0,0q�G� -,   $ txtCrearUsuarioContrasena��$	��200��$��0,5,0,10q��� $��Nombre completo:��
�������$��0,5,0,0q��� --   $# txtCrearUsuarioNombreCompleto��$	��200��$��0,5,0,10q��� $'��!Fecha de nacimiento (dd/mm/yyyy):��
�������$��0,5,0,0q��� -.   $$ txtCrearUsuarioFechaNacimiento��$	��200��$��0,5,0,10q��� $��Otros datos:��
�������$��0,5,0,0q��� -/   $ txtCrearUsuarioOtrosDatos��$	��200��$��0,5,0,10q��� $��Rol/Permisos:��
�������$��0,5,0,0q��� -0   $  txtCrearUsuarioRolPermisos��$	��200��$��0,5,0,10q��� $��Apodo:��
�������$��0,5,0,0q��� -1   $ txtCrearUsuarioApodo��$	��200��$��0,5,0,10q��� -2   $��Confirmar Creación��$	��200��$��0,10,0,0q��� -3   $ inputFieldsCrearRol��$y�	Collapsed=�$
��Left=�$	��Top=�$��20q�.+��� $��
Nuevo rol:��
�������$��0,5,0,0q��� -4   $ txtNuevoRol��$	��200��$��0,5,0,10q��� $��	Permisos:��
�������$��0,5,0,10q��� -5    ��Name$ chkAdmin��$��Admin��
�������$	��0,5q��� -6   $ chkCrearAgenda��$��Crear agenda��
�������$	��0,5q��� -7   $ chkCrearGrupos��$��Crear grupos��
�������$	��0,5q��� -8   $ chkCrearAgendaGrupal��$��Crear agenda grupal��
�������$	��0,5q��� -9   $ chkEliminarAgendaPropia��$��Eliminar agenda propia��
�������$	��0,5q��� -:   $ chkEliminarGrupos��$��Eliminar grupos��
�������$	��0,5q��� -;   $ chkEliminarAgendaGrupal��$��Eliminar agenda grupal��
�������$	��0,5q��� -<   $ ��Confirmar Creación de Rol��$	��200��$��0,10,0,0q��� -=   $ inputFieldsEliminarRol��$y�	Collapsed=�$
��Left=�$	��Top=�$��20q�.+��� $��Rol a eliminar:��
�������$��0,5,0,0q��� ->   $ txtEliminaRol��$	��200��$��0,5,0,10q��� -?   $��Confirmar Eliminar de Rol��$	��200��$��0,10,0,0q��� -@   $  inputFieldsMostrarPermisos��$y�	Collapsed=�$
��Left=�$	��Top=�$��20q�.+��� $
��Rol:��
�������$��0,5,0,0q��� -A    ��Name$ cmbRolesMostrarPermisos��$	��200��$��0,5,0,10q�$
{�True���� $��	Permisos:��
�������$��0,5,0,10q��� -B   $ chkAdminMostrar��$��Admin��
�������$	��0,5q�$
{�True���� -C   $ chkCrearAgendaMostrar��$��Crear agenda��
�������$	��0,5q�$
{�True���� -D   $ chkCrearGruposMostrar��$��Crear grupos��
�������$	��0,5q�$
{�True���� -E   $! chkCrearAgendaGrupalMostrar��$��Crear agenda grupal��
�������$	��0,5q�$
{�True���� -F   $$ chkEliminarAgendaPropiaMostrar��$��Eliminar agenda propia��
�������$	��0,5q�$
{�True���� -G   $ chkEliminarGruposMostrar��$��Eliminar grupos��
�������$	��0,5q�$
{�True���� -H   $$ chkEliminarAgendaGrupalMostrar��$��Eliminar agenda grupal��
�������$	��0,5q�$
{�True���� -I   $��Consultar Permisos��$	��200��$��0,10,0,0q��� -J   $��Confirmar Edición��$	��200��$��0,10,0,0q��� -K   $&�� Activar/Desactivar Modo Gestión��