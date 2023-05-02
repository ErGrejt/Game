
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;


namespace Game.Models;

public class Brick : Rectangle
{
    public Brick()
    {
        
        
        
        
        // Ustawiamy cegłe
        

        this.Width = 79;
        this.Height = 30;
        
        //Ustawiamy kolor cegiełki
        
        this.Fill = Brushes.Purple;
        //Dodajemy obramowanie cegły
        
        this.StrokeThickness = 2;
        this.Stroke = new SolidColorBrush(Colors.Black);

        
    }

    public void SetPosition(double x, double y)
    {
        Canvas.SetLeft(this, x);
        Canvas.SetTop(this, y);
    }
    
}