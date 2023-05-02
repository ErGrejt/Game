using System.Reactive;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Media;
using Avalonia.Controls;
using Game.Models;

namespace Game.ViewModels;

public partial class MenuWindowViewModel : ObservableObject
{
    
    
    [RelayCommand]
    public void ClickGameInfoWindow()
    {
        MyReferences.MainWindowOb.ChangeContent(MyReferences.GameInfo);
        
    }
    
    [RelayCommand]
    public void ClickGameBreakout()
    {

        var gameBoard = new GameBoard();
        var control = new ContentControl();
        control.Content = gameBoard;

        MyReferences.MainWindowOb.Content = control;
    }


}