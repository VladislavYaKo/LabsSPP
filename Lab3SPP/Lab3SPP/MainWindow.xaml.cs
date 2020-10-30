using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab3SPP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}

/*
<HierarchicalDataTemplate DataType="{x:Type model:Class}" ItemsSource="{Binding Path=Collections}">
                    <TextBlock Text="{Binding Name}"/>
                </HierarchicalDataTemplate>
                <DataTemplate DataType="{x:Type model:Method}">
                    <TextBlock Text="{Binding Signature}"/>
                </DataTemplate>
                <DataTemplate DataType="{x:Type model:Field}">
                    <TextBlock Text="{Binding Signature}"/>
                </DataTemplate>
                <DataTemplate DataType="{x:Type model:Property}">
                    <TextBlock Text="{Binding Signature}"/>
                </DataTemplate>
                <DataTemplate DataType="{x:Type model:Constructor}">
                    <TextBlock Text="{Binding Signature}"/>
                </DataTemplate>
*/
