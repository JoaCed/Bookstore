   M S B A M L   `   `   `  ����Q  MBookstoreClient, Version=1.0.5926.22609, Culture=neutral, PublicKeyToken=null1clr-namespace:BookstoreClientBookstoreClient  5   	   !    BookstoreClient.WindowOrder   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:BookstoreClient     5   	   -   5          �Name$  window��5   	    � Title$ 
Your order��5   	   $	��521��6   $	��696��6)    � WindowStartupLocation$ CenterOwner=�6L    � 	Resources 5	       �� 
TargetType �� Property �� Value��%>  '��       '�� �     '�� �     �� 5
   
   " ��6   .�5      ��  � ��$ 25��6'   �� 5       � ��$ 10q�6'   �� 5       � ��$ 50��6)   5      �� 5   
   " ��6   .�5      ��  � ��$	 100��6)   5      �� 5   
   " ��6   .�5      ��  � ��$ 3q�6'   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $	��100 �6   �� 5      $��60 �6   .+�5   
   � .+�5      ��  �� SelectionMode$ Single=�6   ��6.   �� *SelectedBooks+	 �� ElementName$	 window��
 �� View
 5      �� 5      .P�5       �� $��Title��6)    �� DisplayMemberBinding 68   �� *Title+�� 5!      $��Author��6)    69   �� *Author+�� 5"      $��Price��6)    68   �� *Price+ �� StringFormat$ {0:#.00}���� 5#      $��Quantity��6)    6;   �� *
Quantity+�� 5$      $��Missing��6)    6:   �� *	Missing+5%      5'      5(      �� 5)   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�5*      �� -   5        $��LabelTotalPrice��5*      $��1��6+   $��Total price 0.00��6;   $��Center=�6V   $��16(�6x   5,      �� 5-   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   .+�5.      �� -   5        $��ButtonOK��5.      $��OK��6%   62   �� 5/      -   5        $��ButtonCancel��5/      $��Cancel��6)   6:   50      51      52      