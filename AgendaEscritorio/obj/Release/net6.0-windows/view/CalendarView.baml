   M S B A M L   `   `   `  ����   AgendaEscritorio(    "AgendaEscritorio.view.CalendarView   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  
Calendario�����	#FF1B1448 � WindowStyle$
 None=� � 
ResizeMode$ CanResizeWithGrip=� � WindowState$ 	Maximized=� � WindowStartupLocation$ CenterScreen=�.���  � ColumnDefinitions �� $	��250 ��� $��* �.+��� ���#2B2B2B$��0��.+��� $��Gestión de Eventos��$��18(�
�������$��10q��� -   $��Crear Evento��$��10q����#007BA7
��������� -   $��
Ver Evento��$��10q����#007BA7
��������� -   $��Modificar Evento��$��10q����#007BA7
��������� -   $��Eliminar Evento��$��10q����#007BA7
�������� $��1��.+���  �� CornerRadius$ 12�����	#FF1B1448$��0q�._��� $��2q� J� Opacity$
 0.95Q�$ 10������  �� 
StartPoint$	 0,0&�	 �� EndPoint$		 1,1&�.��� 
 � Color$
 #007BA7�� � Offset$ 0Q�� $
 #00FFFF��$	 0.5Q�� $
 #005F6B��$ 1Q�����  �� ImageSource$ ../images/descarga.jpg�� j� Stretch$
 Fill=�._�� .+��� ���	#CC00AABB$ 10��$	 0.9Q�._��� .+���  �� Orientation$ 
Horizontal=�$��Right=�$	��Top=�$��0,5,5,0q�.+��� -   $��-��$��30��
������ 
��������� -    ��Name$ MaximizeRestoreButton��$	��☐��$��30��
������ 
��������� -   $��X��$��30��
������ 
��������� -   $��Volver��$��80��$
��Left=�$��10,0,0,0q�$	��Top=����#007BA7
����������#005F6B�� $��Center=�$��Center=�$��0,50,0,0q�.+��� $ 
Horizontal=�$��Center=�$��10q�.+��� -	   $��<��$��50�����#007BA7
����������#005F6B�� -
    ��Name$ MonthYearText��$��24(�
�������$��Center=�$��Center=�$	��5,0q��� -   $��>��$��50�����#007BA7
����������#005F6BA� -    A�Name$ DaysGrid�� A� Rows$ 7�� A� Columns$ 7��$��Center=�$��Center=�$��10q�