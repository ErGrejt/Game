using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using Game.Views;

namespace Game.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private string _greeting = "Siema";

    public MainWindowViewModel()
    {
        MyReferences.MWVMOb = this;
        MyReferences.MenuWindow = new MenuWin();
        MyReferences.GameInfo = new GameInfoWin();
        MyReferences.GameBreakout = new GameBreakout();
        MyReferences.SettingsWin = new SettingsWin();
        MyReferences.MainWindowOb.ChangeContent(MyReferences.MenuWindow);
    }
}