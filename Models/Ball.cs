using System;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;


namespace Game.Models;

public class Ball : Ellipse
{
    private static Random random = new Random();
    
    public double VelocityX { get; set; }
    
    public double VelocityY { get; set; }

    public Ball()
    {
        this.Width = 20;
        this.Height = 20;
        this.Fill = Brushes.White;
    }

    public void Start()
    {
        //Ustawiamy pozycje startową kuli
        Canvas.SetLeft(this, 400 - Width/2);
        Canvas.SetTop(this, 300 - Height/2);
        
        //Ustawiamy prędkośc kuli
        VelocityX = 200;
        VelocityY = -200;
        
        //Wybieramy losowy kierunek
        if (random.NextDouble() < 0.5)
        {
            VelocityX = -VelocityY;
        }
    }

    public void Update()
    {
        // Pobieramy aktualną pozycje kili
        double x = Canvas.GetLeft(this);
        double y = Canvas.GetTop(this);

        // Aktualizujemy pozyje kuli względem jej predkości
        x += VelocityX * 16 / 1000;
        y += VelocityY * 16 / 1000;

        // Sprawdzamy czy kula dotknęła ściany
        
        //---------------------------------------------------------------------------
        if (x < 0 || x + 20 > 1423)
        {
            VelocityX = -VelocityX;
        }
        if (y < 0)
        {
            VelocityY = -VelocityY;
        }

        // Ustawiamy nową pozycje kuli
        Canvas.SetLeft(this, x);
        Canvas.SetTop(this, y);
    }
}