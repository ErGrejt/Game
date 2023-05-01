using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Game.ViewModels;

namespace Game.Views;

public partial class GameInfoWin : UserControl
{
    public GameInfoWin()
    {
        InitializeComponent();
        DataContext = new GameInfoViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}