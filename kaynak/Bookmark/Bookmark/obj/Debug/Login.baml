   M S B A M L   `   `   `  ����C  ?Bookmark, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:BookmarkBookmark  5   	       Bookmark.Login   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Bookmark     5   	   -   5        $	��454��5      $	��438��6'     � Icon$  x.ico��63    � WindowStartupLocation$ CenterScreen=�6@    � ShowInTaskbar .   � ShowActivated .  &   System.Windows.Media.TextOptions   TextFormattingMode$ Display=�6�    � 
ResizeMode$ NoResize=�6�   .��5      �  � RowDefinitions 5	   
   �� 5
       � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $	��10* �6   �� 5      $��2* �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ girisYapLabeli��5      $��
Giriş Yap��6(   $��Center=�6<   $��
10,10,10,0q�6Y   $	��410��6m   $��
Montserrat)�6y   $��55(�6�   $��Center=�6�   $��Center=�6�   $��Light"�6�   $	��104��6�   $	��Top=�6  $��3��6  �� 5   
   -   5        	 ��Name$	 girisKullaniciAdiBox��5   &   

 � Tag$
 Kullanıcı Adı��6   $
��Left=�6D    �� TextWrapping$
 Wrap=�6_   $	��Top=�6s   $	��242��6�   $��1��6�   $��
33,150,0,0q�6�   $��0q�6�    K� ZIndex$ 3��6�   $��4��6�   G� 5   
   -   5         G�Name$ girisSifreBox��5   >   $
��Left=�6   $
 Şifre��62   $	��Top=�6U   $	��242��6m   $��1��6y   $��
33,200,0,0q�6�    J� IsManipulationEnabled . ���	#FF333333$ 2��6�   $��9��6�   �� 5   
   -   5        $ geriLabeli2��5      $
��Geri��6%   $��1��64   $
��Left=�6D   $��103,266,0,0q�6_   $	��Top=�6t   $	��101��6�   $��Center=�6�   $��Center=�6�   $��33��6�   $��22(�6�   $	��5,0q�6�   $��Light"�6   � Cursor$
 Hand��6  6'  �� 5   
   -   5         ��Name$ girisYapButonu��5   8      TabButtonPink#��[  6   $��
Giriş Yap��6P   $��13,323,13,0q�6d   $��22(�6�   $��Center=�6�   $��1q�6�   $
 Hand��6�   $��1��6�   $��
Montserrat)�6�   $��10��6�   $��50��6  $	��Top=�6  �� 5   
   -   5         ��Name$ 	textBlock��5      $
��Left=�6'   $��WrapWithOverflow=�6B   $	��Top=�6b   $��
33,235,0,0q�6z   $��1��6�   $	��242��6�   $��26��6�   $��14(�6�   ���	#FFF86E8F6�   5      5      