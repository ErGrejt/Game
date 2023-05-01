using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Game.ViewModels;

namespace Game.Views;

public partial class MenuWin : UserControl
{
    public MenuWin()
    {
        InitializeComponent();
        DataContext = new MenuWindowViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void CloseMenuClick(object sender, RoutedEventArgs e)
    {
        Window window = (Window)this.VisualRoot;
        window.Close();
    }
}