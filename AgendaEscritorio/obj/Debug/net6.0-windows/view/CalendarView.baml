   M S B A M L   `   `   `  ����  AgendaEscritorio(    "AgendaEscritorio.view.CalendarView   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
Calendario��5   	   ���	#FF1B1448 � WindowStyle$
 None=�5   	    � 
ResizeMode$ CanResizeWithGrip=�5   	    � WindowState$ 	Maximized=�5   	    � WindowStartupLocation$ CenterScreen=�5	   	   .��5
      �  � ColumnDefinitions 5   
   �� 5      $	��250 �6   �� 5      $��* �6   .+�5   
   � ���#2B2B2B$��0��6$    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    �� 5      $
��Auto �6    .+�5      �� ���#2B2B2B$��0��6.   $��5q�6;   $	��Top=�6F   
����������#007BA7 � 	Resources 5       �� 
TargetType	 �� Property
 �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property P� Value �� 
TargetName��%�  '��       �� 5      " ��6   .�5      �� 	 � ��

 �����67   �� 5      	 � ��
 5       �� 5!   "   " ��62   .R�5"   &   � .+�5$   *   �� $��	TabBorder��5%   !   ���#2B2B2B���#007BA7$��1q�5(   !   $ 3��5)   !   $��2q�5*   !   ._�5+   .   �� $��Center=�5,   %   $��Center=�5-   %   $��Header��5.   %   $
��10,2q�5/   %   50   +   51   '    53   &   P� 55   *    �@��
IsSelected$
 True��6H   . �56   .   �� $ 	TabBorder��65   	 � ��
 �#007BA76b   �� 57   .   $ 	TabBorder��65   	 � ��
 �#005F6B6c   �� 58   .   	 � ��

 �����6K   59   +   P� 5;   *    �@J�IsMouseOver$
 True��6I   . �5<   .   �� $ 	TabBorder��65   	 � ��
 �#005F6B6b   5=   +   P� 5?   *    � {�$ False��6G   . �5@   .   �� $ 	TabBorder��65   	 � ��
 �#5555556b   �� 5A   .   	 � ��
 �#AAAAAA6K   5B   +   5D   #   5F      5G      5H      .@�5K      �� $��Eventos��6   .��5L      ��  �� Orientation$ Vertical=�6!   $	��Top=�68   .+�5M      �� $��Gestión de Eventos��6$   $��18(�6>   
�������$��10q�6_   �� 5N      -   5        $��Crear Evento��5N   !   $��10q�68   ���#007BA7
�������6s   �� 5O      -   5        $��Modificar Evento��5O   !   $��10q�6<   ���#007BA7
�������6{   �� 5P      -   5        $��Eliminar Agenda Grupal��5P   !   $��10q�6B   ���#007BA7
�������6�   �� 5Q      -   5        $��Insertar Tag��5Q   !   $��10q�68   ���#007BA7
�������6s   �� 5R      -   5        $��Eliminar Tag��5R   !   $��10q�68   ���#007BA7
�������6s   �� 5S      -   5        $��Buscar por Tag��5S   !   $��10q�6:   ���#007BA7
�������6s   5T      5U      �� 5X      $��Grupos��6   .��5Y      �� $ Vertical=�6!   $	��Top=�68   .+�5Z      �� $��Gestión de Grupos��6$   $��18(�6=   
�������$��10q�6^   �� 5[      -   5        $��Crear Grupo��5[   !   $��10q�67   ���#007BA7
�������6q   �� 5\      -   5        $��Eliminar Grupo��5\   !   $��10q�6:   ���#007BA7
�������6w   �� 5]      -	   5        $��Ver Grupos (propietario)��5]   !   $��10q�6D   ���#007BA7
�������6�   �� 5^      -
   5        $��Ver Grupos (miembro)��5^   !   $��10q�6@   ���#007BA7
�������6�   �� 5_      -   5        $��Ver Todos los Grupos��5_   !   $��10q�6@   ���#007BA7
�������6~   �� 5`      -   5        $��Invitar a Usuario��5`   !   $��10q�6=   ���#007BA7
�������6{   5a      5b      5c      �� 5h      -   5         ��Name$ crearEventoPanel��5h      $��2��63   $y�	Collapsed=�6@   $��20q�6W   .+�5j      �� -   5         ��Name$ textFechaLabel��5j      $��Fecha��64   
�������$��10q�6T   $��Center=�6`   �� 5k      -   5         ��Name$ txtFecha��5k      $��10q�6,   $	��200��68   $��30��6D   �� 5l      -   5        $ textContenidoLabel��5l      $��	Contenido��68   
�������$��10q�6\   $��Center=�6h   �� 5m      -   5        $ txtContenido��5m      $��10q�60   $	��200��6<   $��30��6H   �� 5n      -   5        $ textTagsLabel��5n      $
��Tags��63   
�������$��10q�6R   $��Center=�6^   �� 5o      -   5        $ txtTags��5o      $��10q�6+   $	��200��67   $��30��6C   �� 5p      -   5         ��Name$ 	chkGrupal��5p      $��Evento Grupal��6.   $��10q�6F   
�������6R   �� 5q      -   5        $ textNombreGrupoLabel��5q      $!��Nombre del Grupo (opcional)��6:   
�������$��10q�6p   $��Center=�6|   �� 5r      -   5        $ txtNombreGrupo��5r      $��10q�62   $	��200��6>   $��30��6J   �� 5s      -   5         ��Name$ 	btnEnviar��5s      $��Enviar Evento��6,   $	��200��6D   $��30��6P   $��10q�6\   6h   5t      �� 5x      -   5        $ modificarEventoPanel��5x      $��2��67   $y�	Collapsed=�6D   $��20q�6[   .+�5y      �� $��Fecha del evento:��6   
�������$��10q�6H   $��Center=�6T   �� 5z      -   5        $ txtFechaEvento��5z      $��10q�62   $	��200��6>   $��30��6J   �� 5|      $!��Nuevo contenido del evento:��6   
�������$��10q�6R   $��Center=�6^   �� 5}      -   5        $ txtNuevoContenido��5}      $��10q�65   $	��200��6A   $��30��6M   �� 5      -   5        $ chkEsGrupalModificar��5      $��¿Es grupal?��69   
�������$��10q�6b   $��Center=�6n   �� 5�      -   5        $��Modificar Evento��5�      $	��200��64   $��30��6@   $��10q�6L   6X   5�      �� 5�      -   5        $ crearGrupoPanel��5�      $��2��62   $y�	Collapsed=�6?   $��20q�6V   .+�5�      �� -   5        $ textNombreGrupoLabelCrear��5�      $��Nombre del Grupo��6?   
�������$��10q�6j   $��Center=�6v   �� 5�      -   5        $ txtNombreGrupoCrear��5�      $��10q�67   $	��200��6C   $��30��6O   �� 5�      -    5        $ btnEnviarGrupo��5�      $��Crear Grupo��61   $	��200��6G   $��30��6S   $��10q�6_   6k   5�      �� 5�      -!   5        $ eliminarGrupoPanel��5�      $��2��65   $y�	Collapsed=�6B   $��20q�6Y   .+�5�      �� -"   5        $" textNombreGrupoLabelEliminar��5�      $��Nombre del Grupo��6B   
�������$��10q�6m   $��Center=�6y   �� 5�      -#   5        $ txtNombreGrupoEliminar��5�      $��10q�6:   $	��200��6F   $��30��6R   �� 5�      -$   5        $ btnEnviarEliminarGrupo��5�      $��Eliminar Grupo��69   $	��200��6R   $��30��6^   $��10q�6j   6v   5�      �� 5�      -%   5        $ eliminarAgendaGrupalPanel��5�      $��2��6<   $y�	Collapsed=�6I   $��20q�6`   .+�5�      �� -&   5        $( "textNombreGrupoLabelEliminarAgenda��5�      $��Nombre del Grupo��6H   
�������$��10q�6s   $��Center=�6   �� 5�      -'   5        $" txtNombreGrupoEliminarAgenda��5�      $��10q�6@   $	��200��6L   $��30��6X   �� 5�      -(   5        $# btnEnviarEliminarAgendaGrupal��5�      $��Eliminar Agenda Grupal��6@   $	��200��6a   $��30��6m   $��10q�6y   6�   5�      �� 5�      -)   5        $ insertarTagPanel��5�      $��2��63   $y�	Collapsed=�6@   $��20q�6W   .+�5�      �� $��Fecha (DD/MM/YYYY):��6   
�������$��10q�6J   $��Center=�6V   �� 5�      -*   5        $ txtFechaTag��5�      $��10q�6/   $	��200��6;   $��30��6G   �� 5�      $��
Nuevo Tag:��6   
�������$��10q�6A   $��Center=�6M   �� 5�      -+   5        $ txtNuevoTag��5�      $��10q�6/   $	��200��6;   $��30��6G   �� 5�      -,   5        $��Insertar Tag��5�      $	��200��60   $��30��6<   $��10q�6H   6T   5�      �� 5�      --   5        $ eliminarTagPanel��5�      $��2��63   $y�	Collapsed=�6@   $��20q�6W   .+�5�      �� $��Fecha (DD/MM/YYYY):��6   
�������$��10q�6J   $��Center=�6V   �� 5�      -.   5        $ txtFechaEliminar��5�      $��10q�64   $	��200��6@   $��30��6L   �� 5�      $��Tag a eliminar:��6   
�������$��10q�6F   $��Center=�6R   �� 5�      -/   5        $ txtTagEliminar��5�      $��10q�62   $	��200��6>   $��30��6J   �� 5�      -0   5        $��Eliminar Tag��5�      $	��200��60   $��30��6<   $��10q�6H   6T   5�      �� 5�      -1   5        $ buscarTagPanel��5�      $��2��61   $y�	Collapsed=�6>   $��20q�6U   .+�5�      �� $��Tag a buscar:��6   
�������$��10q�6D   $��Center=�6P   �� 5�      -2   5        $ txtTagBuscar��5�      $��10q�60   $	��200��6<   $��30��6H   �� 5�      -3   5        $ chkEsGrupalBuscarTag��5�      $��¿Es grupal?��69   
�������$��10q�6b   $��Center=�6n   �� 5�      -4   5        $ panelGrupoBuscarTag��5�      $y�	Collapsed=�6:   $��10q�6Q   .+�5�      �� $��Nombre del grupo:��6    
�������$��10q�6L   $��Center=�6X   �� 5�      -5   5        $ txtNombreGrupoBuscarTag��5�      $��10q�6?   $	��200��6K   $��30��6W   5�      �� 5�      -6   5        $��
Buscar Tag��5�      $	��200��6.   $��30��6:   $��10q�6F   6R   5�      �� 5�      -7   5        $ invitarUsuarioPanel��5�      $��2��66   $y�	Collapsed=�6C   $��20q�6Z   .+�5�      �� -8   5        $! textNombreGrupoLabelInvitar��5�      $��Nombre del Grupo��6A   
�������$��10q�6l   $��Center=�6x   �� 5�      -9   5        $ txtNombreGrupoInvitar��5�      $��10q�69   $	��200��6E   $��30��6Q   �� 5�      -:   5        $! textSobrenombreLabelInvitar��5�      $��Sobrenombre del Usuario��6A   
�������$��10q�6s   $��Center=�6   �� 5�      -;   5        $ txtSobrenombreInvitar��5�      $��10q�69   $	��200��6E   $��30��6Q   �� 5�      -<   5        $ btnEnviarInvitarUsuario��5�      $��Enviar Invitación��6:   $	��200��6V   $��30��6b   $��10q�6n   6z   5�      5�      � 5�   
   $��1��6   .+�5�      �� $ 12��6   ���	#FF1B1448$��0q�6>   ._�5�      �� $��2q�6    J� Opacity$
 0.95Q�6-   $ 10��6<   ��5�      �� 5�       �� 
StartPoint$	 0,0&�6.    �� EndPoint$	 1,1&�6?   .��5�      �  � Color$ #007BA7��6+    � Offset$ 0Q�6;   � 5�      $ #00FFFF��6+   $	 0.5Q�6;   � 5�      $ #005F6B��6+   $ 1Q�6;   5�      ��5�      �� 5�       �� ImageSource$ ../images/descarga.jpg��6%    j� Stretch$
 Fill=�6J   ._�5�      � .+�5�      �� ���	#CC00AABB$ 10��68   $	 0.9Q�6J   ._�5�      �� .+�5�   "   �� $ 
Horizontal=�6-   $��Right=�6F   $	��Top=�6b   $��0,5,5,0q�6z   .+�5�   &   �� -=   5        $��-��5�   -   $��30��69   
������ 
�������6z   �� 5�   &   ->   5        $ MaximizeRestoreButton��5�   -   $	��☐��6L   $��30��6X   
������ 
�������6�   �� 5�   &   -?   5        $��X��5�   -   $��30��69   
������ 
�������6w   5�   #   �� 5�   "   -@   5        $��Mostrar Agenda��5�   )   $	��100��6B   $��Center=�6N   $	��Top=�6k   $��10q�6�   ���#007BA7
����������#005F6B6�   �� 5�   "   $ 
Horizontal=�6-   $��Center=�6F   $	��Top=�6c   $��10q�6{   .+�5�   &   �� -A   5        $ chkEsGrupal��5�   /   $��¿Es grupal?��6D   
�������$��Center=�6m   6�   �� 5�   &   -B   5        $ txtNombreGrupoOpcional��5�   .   $	��200��6N   $��30��6Z   $��10,0,0,0q�6f   ${�False��6x   6�   5�   #   �� 5�   "   -C   5        $	��←��5�   )   $��80��65   $
��Left=�6@   $��10,0,0,0q�6[   $	��Top=�6m   ���#007BA7
����������#005F6B$��20(�6�   �� 5�   "   $��Center=�6-   $��Center=�6J   $��0,50,0,0q�6e   .+�5�   &   �� $ 
Horizontal=�61   $��Center=�6J   $��10q�6g   .+�5�   *   �� -D   5        $	��←��5�   1   $��50��6^   ���#007BA7
����������#005F6B6�   �� 5�   *   -E   5        $��<��5�   1   $��50��6\   ���#007BA7
����������#005F6B6�   �� 5�   *   -F   5        $ MonthYearText��5�   4   $��24(�6K   
�������$��Center=�6l   $��Center=�6�   $	��5,0q�6�   �� 5�   *   -G   5        $��>��5�   1   $��50��6X   ���#007BA7
����������#005F6B6�   �� 5�   *   -H   5        $	��→��5�   1   $��50��6Z   ���#007BA7
����������#005F6B6�   5�   '   A� 5�   &   -I   5         A�Name$ DaysGrid��5�   2    A� Rows$ 7��6D     A� Columns$  7��6M   $��Center=�6Y   $��Center=�6v   $��10q�6�   5�   #   5�      5�      5�      5      5     5     �� 5  
   -J   5        $ DetalleEventoPanel��5     $��1��61   $��10,10,10,200q�5     $y�	Collapsed=�6   $��Center=�5     $��Bottom=�6"   .+�5
     �� 
���   �$ 10��6(   ! �� Padding$! 15q�6:   $��0q�6G   ._�5     �� .+�5     �� -K   5        $ txtDetalles��5      $��Detalles del evento��65   
�������$��10q�5  #   $��16(�6/   " �� TextAlignment$" Center=�5     �� 5     -L   5        $ txtFechaDetalle��5      
�������$��10q�5     $��14(�6   $" Center=�6*   �� 5     -M   5        $ txtContenidoDetalle��5      
�������$��10q�5     $��14(�6   $" Center=�6*   5     5     5     5     5     