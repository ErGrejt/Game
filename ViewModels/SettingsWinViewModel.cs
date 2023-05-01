using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Game.Models;

namespace Game.ViewModels;

public class SettingsWinViewModel
{
    public void ClickMenuWindow()
    {
        MyReferences.MainWindowOb.ChangeContent(MyReferences.MenuWindow);
    }

    

    
}