using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Game.Models;

public class Paddle : Rectangle
{
    public Paddle()
    {
        // Ustawiamy paletke
        this.Width = 100;
        this.Height = 20;
        this.Fill = Brushes.White;

        // Ustawiamy pozycje startową paletki
        Canvas.SetLeft(this, 350);
        Canvas.SetTop(this, 560);
    }

    public void Move(double dx)
    {
        // Pobieramy pozycje paletki
        double x = Canvas.GetLeft(this);

        // Aktualizujemy pozycje paletki
        x += dx;

        // Upewniamy sie ze paletka nie wyjdzie poza ekran
        if (x < 0)
        {
            x = 0;
        }
        if (x + 100 > 800)
        {
            x = 800;
        }

        // Ustawiamy nowa pozycje paletki
        Canvas.SetLeft(this, x);
    }
}