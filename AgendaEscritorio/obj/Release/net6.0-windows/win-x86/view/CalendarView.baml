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
��������� $��Grupos��.���� $ Vertical=�$	��Top=�.+��� $��Gestión de Grupos��$��18(�
�������$��10q��� -   $��Crear Grupo��$��10q����#007BA7
��������� -   $��Eliminar Grupo��$��10q����#007BA7
��������� -   $��Ver Grupos (propietario)��$��10q����#007BA7
��������� -   $��Ver Grupos(miembro)��$��10q����#007BA7
��������� -	   $��Ver Todos los Grupos��$��10q����#007BA7
��������� -
    ��Name$ crearEventoPanel��$��2��$y�	Collapsed=�$��20q�.+��� -   	 ��Name$	 textFechaLabel��$��Fecha��
�������$��10q�$��Center=��� -   
 ��Name$
 txtFecha��$��10q�$	��200��$��30���� -   $	 textContenidoLabel��$��	Contenido��
�������$��10q�$��Center=��� -   $
 txtContenido��$��10q�$	��200��$��30���� -   $	 textTagsLabel��$
��Tags��
�������$��10q�$��Center=��� -   $
 txtTags��$��10q�$	��200��$��30���� -    ��Name$ 	chkGrupal��$��Evento Grupal��$��10q�
��������� -   $	 textNombreGrupoLabel��$!��Nombre del Grupo (opcional)��
�������$��10q�$��Center=��� -   $
 txtNombreGrupo��$��10q�$	��200��$��30���� -    ��Name$ 	btnEnviar��$��Enviar Evento��$	��200��$��30��$��10q��� -   $ crearGrupoPanel��$��2��$y�	Collapsed=�$��20q�.+��� -   $	 textNombreGrupoLabelCrear��$��Nombre del Grupo��
�������$��10q�$��Center=��� -   $
 txtNombreGrupoCrear��$��10q�$	��200��$��30���� -   $ btnEnviarGrupo��$��Crear Grupo��$	��200��$��30��$��10q��� -   $ eliminarGrupoPanel��$��2��$y�	Collapsed=�$��20q�.+��� -   $"	 textNombreGrupoLabelEliminar��$��Nombre del Grupo��
�������$��10q�$��Center=��� -   $
 txtNombreGrupoEliminar��$��10q�$	��200��$��30���� -   $ btnEnviarEliminarGrupo��$��Eliminar Grupo��$	��200��$��30��$��10q�� $��1��.+���  �� CornerRadius$ 12�����	#FF1B1448$��0q�._��� $��2q� J� Opacity$
 0.95Q�$ 10������  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,1&�.���  � Color$ #007BA7�� � Offset$ 0Q�� $ #00FFFF��$	 0.5Q�� $ #005F6B��$ 1Q�����  �� ImageSource$ ../images/descarga.jpg�� j� Stretch$
 Fill=�._�� .+��� ���	#CC00AABB$ 10��$	 0.9Q�._��� .+��� $ 
Horizontal=�$��Right=�$	��Top=�$��0,5,5,0q�.+��� -   $��-��$��30��
������ 
��������� -   $ MaximizeRestoreButton��$	��☐��$��30��
������ 
��������� -   $��X��$��30��
������ 
��������� -    $��Volver��$��80��$
��Left=�$��10,0,0,0q�$	��Top=����#007BA7
����������#005F6B�� $��Center=�$��Center=�$��0,50,0,0q�.+��� $ 
Horizontal=�$��Center=�$��10q�.+��� -!   $��<��$��50�����#007BA7
����������#005F6B�� -"   $	 MonthYearText��$��24(�
�������$��Center=�$��Center=�$	��5,0q��� -#   $��>��$��50�����#007BA7
����������#005F6BA� -$    A�Name$ DaysGrid�� A� Rows$ 7�� A� Columns$ 7��$��Center=�$��Center=�$��10q�