   M S B A M L   `   `   `  ���� F  BFilesCloner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:FilesCloner.ViewsFilesCloner.Views  ^ ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21fP=clr-namespace:Caliburn.Micro;assembly=Caliburn.Micro.PlatformCaliburn.Micro !    FilesCloner.Views.ShellView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:FilesCloner.Views     Mcal=clr-namespace:Caliburn.Micro;assembly=Caliburn.Micro.Platform     D @ModernWpf, Version=0.9.4.0, Culture=neutral, PublicKeyToken=nullM IModernWpf.Controls, Version=0.9.4.0, Culture=neutral, PublicKeyToken=null,ui!http://schemas.modernwpf.com/2019   -   0  *ModernWpf.Controls.Primitives.WindowHelper    UseModernWindowStyle  . $��0q� &  "SystemControlPageTextBaseHighBrush#���    � Title$ FilesCloner�� � SizeToContent$ WidthAndHeight=� � WindowStartupLocation$ CenterScreen=�.���� $��Hidden=�$��Hidden=�.���  � ColumnDefinitions �� $��20 ��� $
��auto ��� $
��auto ��� $
��auto ��� $��* ��� $
��auto ��� $
��auto ��� $
��auto ��� $��20 � � RowDefinitions �� $��20 ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��* ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��20 �.+��� $��1��$��1��$��10 10q�	Source:�� $��2��$��1��$��10 10q�$	��400������  �� Path$ 	SourceDis� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� -   	 ��Name$	 SourceBrowse��$��3��$��1��$��10 10q�$��50��.��Browse� $��5��$��1��$��7��$
��Left=�$	��Top=� �� $
��auto ��� $
��auto ��� $
��auto � �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �.+��� -   
 ��Name$
 CloneAllFilesBool��$��0��$��0��$��0 5 0 5q�.��Clone All Files�� -   $
 DelAllFilesBool��$��0��$��1��$��0 5 0 10q�.��Delete All Files�� -    ��Name$ 	ListOfExt��$��0��$��2��$��1q�$	��150���� -   $	 EXTMNG��$��0��$��3��$��0 5 0 0q�$��5q�$	��100��.��Extension Manager�� $��1��$��2��$��10 10q�	Target:�� $��2��$��2��$��10 10q�$	��400������ $ 	TargetDis�$ TwoWay=�$ PropertyChanged=��� -   $	 TargetBrowse��$��3��$��2��$��10 10q�$��50��.��Browse�� $��1��$��5��$��3��$��10 10 10 0 q�List of subfolders:� $��1��$��4��$��4��$��10 10 10 0 q� �� $��* ��� $
��auto �.+��� $��0��$��1������ *	SearchBar+$ TwoWay=�$ PropertyChanged=��� -   $	 Refresh��$��1��$��10 0 0 0q�.��	Refresh�� -	   $ 
Subfolders��$��1��$��4��$��5��$��1q�$��10 10q�$	��250�� �� SelectedItem �� *SelectedSubFolder+� $��1��$��4��$��6�� �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��* ��� $
��auto � �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��* ��� $
��auto �.+��� -
   $
 OverWriteBool��$��1��$��1��$��10 10 10 0 q�.��	Overwrite�� -   $
 MainBool��$��2��$��1��$��10 10 10 0 q�.�� Clone the main directory files�� -   $
 CopySubfoldersBool��$��1��$��2��$��10 10 10 0 q�.��Clone subfolders�� -   $
 DeleteSubfoldersBool��$��2��$��2��$��10 10 10 0 q�.��Delete subfolders�� -   $
 DelAllTargetFilesBefore��$��1��$��3��$��10 10 10 0 q�.��(&Delete all target files before cloning�� -   $
 DelAllSourceFilesAfter��$��2��$��3��$��10 10 10 0 q�.��'%Delete all source files after cloning� -    �Name$ 
ProgBarVal�� � Minimum$ 0Q� � Maximum$	 100Q�$��1��$��5��$��8��$��10q�$��30�����#ccc� $��1��$��5��$��9��$
��10 0q� �� $
��auto ��� $��* �.+��� $��0��Info:�� $��1��$
��10 0q����� $ LogInfo�$ TwoWay=�$ PropertyChanged=�Connecting...�� -   $	 SettingsWindow��$��1��$��10��$	��100��$��Center=�$��10 30q�.��
Settings�� -   $	 
StartClone��$��5��$��10��$	��100��$��Center=�$��10 30q�.��Clone