using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Game.ViewModels;

public partial class GameInfoViewModel : ObservableObject
{
    [RelayCommand]

    public void ClickMenuWindow()
    {
        MyReferences.MainWindowOb.ChangeContent(MyReferences.MenuWindow);
    }
}