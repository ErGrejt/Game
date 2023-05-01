using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Game.ViewModels;

namespace Game.Views;

public partial class SettingsWin : UserControl
{
    public SettingsWin()
    {
        InitializeComponent();
        DataContext = new SettingsWinViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
   
    
}