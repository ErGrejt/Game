using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Game.ViewModels;

namespace Game.Views;

public partial class GameBreakout : UserControl
{
    public GameBreakout()
    {
        InitializeComponent();
        DataContext = new GameBreakoutViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}