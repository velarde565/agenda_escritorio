   M S B A M L   `   `   `  ����   AgendaEscritorio(    "AgendaEscritorio.view.MainMenuView   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Menú Principal��$	��450��$	��600�����	#FF1B1448 � WindowStyle$
 None=� � 
ResizeMode$ CanResizeWithGrip=� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=�.��� .+���  �� CornerRadius$ 12�����	#FF1B1448$��0q�._��� $��2q� J� Opacity$
 0.95Q�$ 10������  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,1&�.��� 	 � Color$	 #007BA7��
 � Offset$
 0Q�� $	 #00FFFF��$	
 0.5Q�� $	 #005F6B��$
 1Q�����  �� ImageSource$ ../images/descarga.jpg�� j� Stretch$
 Fill=�._�� .+��� ���	#CC00AABB$ 10��$	 0.9Q�._�� .+���  �� Orientation$ 
Horizontal=�$��Right=�$	��Top=�$��0,5,5,0q�.+��� -   $��-��$��30��
������ 
��������� -    ��Name$ MaximizeRestoreButton��$	��☐��$��30��
������ 
��������� -   $��X��$��30��
������ 
��������� $��Center=�$��Center=�$��0,20,0,0q�.+��� $��Menú Principal��$��24(�
�������$��Center=�$	��Top=�$��0,20,0,0q��� $��Center=�$��Center=�$	��200��.+��� -   $ 	btnAgenda��$��Agenda��$��50�����#007BA7
�������$��0,10,0,0q��� -   $ btnGestionPerfiles��$��Gestión Perfiles��$��50�����#007BA7
�������$��0,10,0,0q��� -   $ 
btnAjustes��$��Ajustes��$��50�����#007BA7
�������$y�	Collapsed=�$��0,10,0,0q��� -   $ btnPeticionManual��$��Petición Manual��$��50�����#007BA7
�������$��0,10,0,0q��� -	   $ btnIntroducirInfoSobre��$&�� Introducir Info 'ABOUT' Servidor��$��50�����#007BA7
�������$��0,10,0,0q��� -
    ��Name$ panelPeticionManual��$ Vertical=�$	��300��$y�	Collapsed=����	#FF242424$��10q�.+��� $��Introduce tu petición:��
�������$��16(�$��0,0,0,10q��� -    ��Name$ txtPeticionManual��$��30�����#FFF
���   �$��0,0,0,10q��� -   $ btnEnviarPeticion��$��Enviar Petición��$��40�����#007BA7
��������� -   $ panelInfoSobre��$ Vertical=�$	��300��$y�	Collapsed=����	#FF242424$��10q�.+��� $8��2Introduce la nueva información sobre el servidor:��
�������$��16(�$��0,0,0,10q��� -   $ txtInfoSobre��$��30�����#FFF
���   �$��0,0,0,10q��� -   $ btnEnviarInfoSobre��$��Enviar Información��$��40�����#007BA7
��������� -   $��Cerrar Sesión��$��Right=�$��Bottom=�$��10q�$	��100���� -   $ PowerButton��$��60��$��60��$��10q�$��Bottom=�$��Right=� � ToolTip$ Apagar Servidor�����	#FF007BA7
�������$��2q� � Cursor$
 Hand������  �� 
TargetType" ��.R�� .+�A� #��z��#��z��#��z���� $	��⏻��$��28(�
�������$��Center=�$��Center=�