using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Game.Models;
using Game.ViewModels;

namespace Game.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MyReferences.MainWindowOb = this;
    }

    public void ChangeContent(Control newContent)
    {
        Content = newContent;
    }
    
}