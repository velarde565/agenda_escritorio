   M S B A M L   `   `   `  ����   AgendaEscritorio(    "AgendaEscritorio.view.CalendarView   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  
Calendario�����	#FF1B1448 � WindowStyle$
 None=� � 
ResizeMode$ CanResizeWithGrip=� � WindowState$ 	Maximized=� � WindowStartupLocation$ CenterScreen=�.���  � ColumnDefinitions �� $	��250 ��� $��* �.+�� ���#2B2B2B$��0�� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� ���#2B2B2B$��0��$��5q�$	��Top=�.@��� $��Eventos��.����  �� Orientation$ Vertical=�$	��Top=�.+��� $��Gestión de Eventos��$��18(�
�������$��10q��� -   $��Crear Evento��$��10q����#007BA7
��������� -   $��
Ver Evento��$��10q����#007BA7
��������� -   $��Modificar Evento��$��10q����#007BA7
��������� -   $��Eliminar Evento��$��10q����#007BA7
��������� -   $��Eliminar Agenda Grupal��$��10q����#007BA7
��������� -   $��Insertar Tag��$��10q����#007BA7
��������� -   $��Eliminar Tag��$��10q����#007BA7
��������� -   $��Buscar por Tag��$��10q����#007BA7
��������� $��Grupos��.���� $ Vertical=�$	��Top=�.+��� $��Gestión de Grupos��$��18(�
�������$��10q��� -	   $��Crear Grupo��$��10q����#007BA7
��������� -
   $��Eliminar Grupo��$��10q����#007BA7
��������� -   $��Ver Grupos (propietario)��$��10q����#007BA7
��������� -   $��Ver Grupos(miembro)��$��10q����#007BA7
��������� -   $��Ver Todos los Grupos��$��10q����#007BA7
��������� -   $��Invitar a Usuario��$��10q����#007BA7
��������� -    ��Name$ crearEventoPanel��$��2��$y�	Collapsed=�$��20q�.+��� -   	 ��Name$	 textFechaLabel��$��Fecha��
�������$��10q�$��Center=��� -   
 ��Name$
 txtFecha��$��10q�$	��200��$��30���� -   $	 textContenidoLabel��$��	Contenido��
�������$��10q�$��Center=��� -   $
 txtContenido��$��10q�$	��200��$��30���� -   $	 textTagsLabel��$
��Tags��
�������$��10q�$��Center=��� -   $
 txtTags��$��10q�$	��200��$��30���� -    ��Name$ 	chkGrupal��$��Evento Grupal��$��10q�
��������� -   $	 textNombreGrupoLabel��$!��Nombre del Grupo (opcional)��
�������$��10q�$��Center=��� -   $
 txtNombreGrupo��$��10q�$	��200��$��30���� -    ��Name$ 	btnEnviar��$��Enviar Evento��$	��200��$��30��$��10q��� -   $ modificarEventoPanel��$��2��$y�	Collapsed=�$��20q�.+��� $��Fecha del evento:��
�������$��10q�$��Center=��� -   $
 txtFechaEvento��$��10q�$	��200��$��30���� $!��Nuevo contenido del evento:��
�������$��10q�$��Center=��� -   $
 txtNuevoContenido��$��10q�$	��200��$��30���� -   $ chkEsGrupalModificar��$��¿Es grupal?��
�������$��10q�$��Center=��� -   $��Modificar Evento��$	��200��$��30��$��10q��� -   $ crearGrupoPanel��$��2��$y�	Collapsed=�$��20q�.+��� -    $	 textNombreGrupoLabelCrear��$��Nombre del Grupo��
�������$��10q�$��Center=��� -!   $
 txtNombreGrupoCrear��$��10q�$	��200��$��30���� -"   $ btnEnviarGrupo��$��Crear Grupo��$	��200��$��30��$��10q��� -#   $ eliminarGrupoPanel��$��2��$y�	Collapsed=�$��20q�.+��� -$   $"	 textNombreGrupoLabelEliminar��$��Nombre del Grupo��
�������$��10q�$��Center=��� -%   $
 txtNombreGrupoEliminar��$��10q�$	��200��$��30���� -&   $ btnEnviarEliminarGrupo��$��Eliminar Grupo��$	��200��$��30��$��10q��� -'   $ eliminarAgendaGrupalPanel��$��2��$y�	Collapsed=�$��20q�.+��� -(   $(	 "textNombreGrupoLabelEliminarAgenda��$��Nombre del Grupo��
�������$��10q�$��Center=��� -)   $"
 txtNombreGrupoEliminarAgenda��$��10q�$	��200��$��30���� -*   $# btnEnviarEliminarAgendaGrupal��$��Eliminar Agenda Grupal��$	��200��$��30��$��10q��� -+   $ insertarTagPanel��$��2��$y�	Collapsed=�$��20q�.+��� $��Fecha (DD/MM/YYYY):��
�������$��10q�$��Center=��� -,   $
 txtFechaTag��$��10q�$	��200��$��30���� $��
Nuevo Tag:��
�������$��10q�$��Center=��� --   $
 txtNuevoTag��$��10q�$	��200��$��30���� -.   $��Insertar Tag��$	��200��$��30��$��10q��� -/   $ eliminarTagPanel��$��2��$y�	Collapsed=�$��20q�.+��� $��Fecha (DD/MM/YYYY):��
�������$��10q�$��Center=��� -0   $
 txtFechaEliminar��$��10q�$	��200��$��30���� $��Tag a eliminar:��
�������$��10q�$��Center=��� -1   $
 txtTagEliminar��$��10q�$	��200��$��30���� -2   $��Eliminar Tag��$	��200��$��30��$��10q��� -3   $ buscarTagPanel��$��2��$y�	Collapsed=�$��20q�.+��� $��Tag a buscar:��
�������$��10q�$��Center=��� -4   $
 txtTagBuscar��$��10q�$	��200��$��30���� -5   $ chkEsGrupalBuscarTag��$��¿Es grupal?��
�������$��10q�$��Center=��� -6   $ panelGrupoBuscarTag��$y�	Collapsed=�$��10q�.+��� $��Nombre del grupo:��
�������$��10q�$��Center=��� -7   $
 txtNombreGrupoBuscarTag��$��10q�$	��200��$��30���� -8   $��
Buscar Tag��$	��200��$��30��$��10q��� -9   $ invitarUsuarioPanel��$��2��$y�	Collapsed=�$��20q�.+��� -:   $!	 textNombreGrupoLabelInvitar��$��Nombre del Grupo��
�������$��10q�$��Center=��� -;   $
 txtNombreGrupoInvitar��$��10q�$	��200��$��30���� -<   $!	 textSobrenombreLabelInvitar��$��Sobrenombre del Usuario��
�������$��10q�$��Center=��� -=   $
 txtSobrenombreInvitar��$��10q�$	��200��$��30���� ->   $ btnEnviarInvitarUsuario��$��Enviar Invitación��$	��200��$��30��$��10q�� $��1��.+���  �� CornerRadius$ 12�����	#FF1B1448$��0q�._��� $��2q� J� Opacity$
 0.95Q�$ 10������  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,1&�.���  � Color$ #007BA7�� � Offset$ 0Q�� $ #00FFFF��$	 0.5Q�� $ #005F6B��$ 1Q�����  �� ImageSource$ ../images/descarga.jpg�� j� Stretch$
 Fill=�._�� .+��� ���	#CC00AABB$ 10��$	 0.9Q�._��� .+��� $ 
Horizontal=�$��Right=�$	��Top=�$��0,5,5,0q�.+��� -?   $��-��$��30��
������ 
��������� -@   $ MaximizeRestoreButton��$	��☐��$��30��
������ 
��������� -A   $��X��$��30��
������ 
��������� -B   $��Mostrar Agenda��$	��100��$��Center=�$	��Top=�$��10q����#007BA7
����������#005F6B�� $ 
Horizontal=�$��Center=�$	��Top=�$��10q�.+��� -C   $ chkEsGrupal��$��¿Es grupal?��
�������$��Center=��� -D   $
 txtNombreGrupoOpcional��$	��200��$��30��$��10,0,0,0q�${�False���� -E   $��Volver��$��80��$
��Left=�$��10,0,0,0q�$	��Top=����#007BA7
����������#005F6B�� $��Center=�$��Center=�$��0,50,0,0q�.+��� $ 
Horizontal=�$��Center=�$��10q�.+��� -F   $	��←��$��50�����#007BA7
����������#005F6B�� -G   $��<��$��50�����#007BA7
����������#005F6B�� -H   $	 MonthYearText��$��24(�
�������$��Center=�$��Center=�$	��5,0q��� -I   $��>��$��50�����#007BA7
����������#005F6B�� -J   $	��→��$��50�����#007BA7
����������#005F6BA� -K    A�Name$ DaysGrid�� A� Rows$ 7�� A� Columns$ 7��$��Center=�$��Center=�$��10q��� -L   $ DetalleEventoPanel��$��1��$��10,10,10,200q�$y�	Collapsed=�$��Center=�$��Bottom=�.+��� -M   $	 txtDetalles��$��Detalles del evento��
�������$��10q�$��16(� �� TextAlignment$ Center=��� -N   $	 txtFechaDetalle��
�������$��10q�$��14(�$ Center=��� -O   $	 txtContenidoDetalle��
�������$��10q�$��14(�$ Center=�