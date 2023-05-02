using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;


namespace Game.Models;

public class GameBoard : Canvas
{
    // Piłeczka i paletka
    private Ball ball;
    private Paddle paddle;
        
    // Tablica przechowująca cegiełki
    private Brick[,] bricks;

    
    private DispatcherTimer gameTimer;

    // Popupy
    private Popup popup;
    private Popup popupWin;
        
    // Predkosc paletki (piksele na sekunde)
    private const double PaddleSpeed = 300;
        
    private DispatcherTimer keyboardTimer;
    private bool isLeftKeyDown;
    private bool isRightKeyDown;
        
    public int NumRows { get; set; }
    public int NumColumns { get; set; }
    public int GapHeight { get; set; }
    
    private int bricksDestroyed;

    public static readonly StyledProperty<int> ScoreProperty =
        AvaloniaProperty.Register<GameBoard, int>(nameof(Score));

    public int Score
    {
        get => GetValue(ScoreProperty);
        set => SetValue(ScoreProperty, value);
    }
    
    public GameBoard()
    {
        
        this.Background = Brushes.Black; 
        NumRows = 5;
        NumColumns = 18;
        GapHeight = 2 * 30;
                
        // Inicjazja piłeczki i paletki
        ball = new Ball();
        paddle = new Paddle();

        // Dodajemy piłeczke i paletke na ekran
        this.Children.Add(ball);
        ball.Start();

        this.Children.Add(paddle);

        // Inicjalizacja cegiełek
        bricks = new Brick[NumRows, NumColumns];
            
        AddBricks();

        // Dodajemy timer i ustawiamy go
        gameTimer = new DispatcherTimer();
        gameTimer.Interval = TimeSpan.FromMilliseconds(16);
        gameTimer.Tick += OnGameTimerTick;
        gameTimer.Start();
  
        // Tworzymy DispatcherTime zeby kontrolowac połozenie paletki
        keyboardTimer = new DispatcherTimer();
        keyboardTimer.Interval = TimeSpan.FromMilliseconds(16);
        keyboardTimer.Tick += OnKeyboardTimerTick;
            
        // Metody przycisków
        this.KeyDown += OnKeyDown;
        this.KeyUp += OnKeyUp;
            
        // Definizja popupów
            // Popup przegrana
        popup = new Popup
        {
            PlacementMode = PlacementMode.AnchorAndGravity,
            PlacementGravity = PopupGravity.None,
            PlacementTarget = this,
            IsLightDismissEnabled = true,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            IsOpen = false,
            Child = new TextBlock
            {
                Text = $"Przegrałeś! \n Nie udało Ci się zbić wszystkich cegiełek. \n Wciśnij LPM aby zagrać ponownie.",
                TextAlignment = TextAlignment.Center,
                FontSize = 20,
                Foreground = Brushes.White,
                
            }
        };
        this.Children.Add(popup);
        //Popup wygrana
        popupWin = new Popup
        {
            PlacementMode = PlacementMode.AnchorAndGravity,
            PlacementGravity = PopupGravity.None,
            PlacementTarget = this,
            IsLightDismissEnabled = true,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            IsOpen = false,
            Child = new TextBlock
            {
                Text = $"Wygrałeś! \n Zbiłeś wszystkie cegiełki! \n Wciśnij LPM aby zagrać ponownie.",
                TextAlignment = TextAlignment.Center,
                FontSize = 20,
                Foreground = Brushes.White,
                
            }
        };
        
        this.Children.Add(popupWin); 
        
        
        popup.Closed += (sender, args) =>
        {
            // Reset gry
            this.Reset();
        }; 
        
        popupWin.Closed += (sender, args) =>
        {
            // Reset gry
            this.Reset();
        };
        
        TextBlock scoreTextBlock = new TextBlock
        {
            Text = "Score: 0",
            FontSize = 20,
            Foreground = Brushes.White
        };
        
        scoreTextBlock.Bind(TextBlock.TextProperty, new Binding
        {
            Path = "Score",
            Mode = BindingMode.OneWay,
            Source = this
        });
        
        TextBlock scoreNearbyScore = new TextBlock()
        {
            Text = "Wynik: ",
            FontSize = 20,
            Foreground = Brushes.White,
            
            
        };
        // Ustawiamy pozycje scorenearbyscore na ekranie i dodajemy do go ekranu
        Canvas.SetLeft(scoreNearbyScore,10);
        Canvas.SetTop(scoreNearbyScore,10 );
        this.Children.Add(scoreNearbyScore);
        //Ustawiamy pozycje scoretextblock na ekranie
        Canvas.SetLeft(scoreTextBlock, 95);
        Canvas.SetTop(scoreTextBlock, 10);

        // Dodajemy go do ekranu
        this.Children.Add(scoreTextBlock);
    }

    private void Reset()
    {
        // Rest piłeczki
        ball.Start();

        // Usywamy wszystkie cegiełki 
        foreach (var brick in bricks)
        {
            this.Children.Remove(brick);
        }

        // Dodajemy cegiełki na ekran
        AddBricks();

        // Reset score
        Score = 0;
        //Resetujemy ilosc zbitych cegiełek
        bricksDestroyed = 0;
        
        // Restart gametimer
        gameTimer.Start();
    }

    private void AddBricks()
    {
        for (int i = 0; i < NumRows; i++)
        {
            for (int j = 0; j < NumColumns; j++)
            {
                // Tworzymy nowa cegiełke
                var brick = new Brick();

                // Ustawiamy jej połozenie
                brick.SetPosition(j * brick.Width, i * brick.Height + GapHeight);

                // Dodajemy ją do ekranu
                this.Children.Add(brick);

                // Referencja cegiełki w tableci
                bricks[i, j] = brick;
            }
        }
    }

    private void OnKeyboardTimerTick(object sender, EventArgs e)
    {
        // Aktualna pozycja paletki
        double x = Canvas.GetLeft(paddle);

        // Aktualizacja polozenia paletki w zaleznosci od przycisnietego przycisku
        if (isLeftKeyDown)
        {
            x -= PaddleSpeed * 16 / 1000;
        }
        if (isRightKeyDown)
        {
            x += PaddleSpeed * 16 / 1000;
        }

        // Ustawiamy maksymalne pole ruchu paletki
        x = Math.Max(0, x);
        x = Math.Min(1423 - paddle.Width, x);

        //Ustawiamy nowa pozycja paletki
        Canvas.SetLeft(paddle, x);

        
        if (!isLeftKeyDown && !isRightKeyDown)
        {
            keyboardTimer.Stop();
        }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            // Zamykamy popup
            popup.IsOpen = false;
            popupWin.IsOpen = false;

            // Reset gry
            Reset();
        }

        
        if (e.Key == Key.Left)
        {
            isLeftKeyDown = true;
        }
        else if (e.Key == Key.Right)
        {
            isRightKeyDown = true;
        }

        
        if (!keyboardTimer.IsEnabled)
        {
            keyboardTimer.Start();
        }
    }
        
    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        
        if (e.Key == Key.Left)
        {
            isLeftKeyDown = false;
        }
        else if (e.Key == Key.Right)
        {
            isRightKeyDown = false;
        }

        
        if (!isLeftKeyDown && !isRightKeyDown)
        {
            keyboardTimer.Stop();
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        // Robimy aby ekran gry był aktywny
        this.Focusable = true;
        // Robimy aby klawiatura miała dostęp do ekranu
        this.Focus();
    }

    
    private void OnGameTimerTick(object sender, EventArgs e)
    {
        // Aktualizuemy połozenie piłeczki
        ball.Update();

        // Sprawdzamy kolizjce paletki i cegiełek
        CheckPaddleCollision();
        CheckBrickCollision();
        

        // Sprwadzamy czy gra się skonczyła
        CheckGameOver();
        CheckGameWin();
    }

    
    private void CheckPaddleCollision()
    {
        
        var ballRect = new Rect(Canvas.GetLeft(ball), Canvas.GetTop(ball), ball.Width, ball.Height);
        var paddleRect = new Rect(Canvas.GetLeft(paddle), Canvas.GetTop(paddle), paddle.Width, paddle.Height);

        
        if (ballRect.Intersects(paddleRect))
        {
            
            double ballCenter = Canvas.GetLeft(ball) + ball.Width / 2;
            double paddleCenter = Canvas.GetLeft(paddle) + paddle.Width / 2;
            double dx = ballCenter - paddleCenter;
            ball.VelocityX = dx / (paddle.Width / 2) * 200;
            ball.VelocityY = -ball.VelocityY;
        }
    }
        
   
    private void CheckBrickCollision()
    {
        
        
        
        
        var ballRect = new Rect(Canvas.GetLeft(ball), Canvas.GetTop(ball), ball.Width, ball.Height);

        
        for (int i = 0; i < NumRows; i++)
        {
            for (int j = 0; j < NumColumns; j++)
            {
                
                if (bricks[i, j] == null)
                {
                    continue;
                }

                
                var brickRect = new Rect(Canvas.GetLeft(bricks[i, j]), Canvas.GetTop(bricks[i, j]), bricks[i, j].Width, bricks[i, j].Height);

                
                if (brickRect.Intersects(ballRect))
                {
                    
                    this.Children.Remove(bricks[i, j]);
                    bricks[i, j] = null;

                    
                    ball.VelocityY = -ball.VelocityY;

                    
                    Score++;
                    
                    
                    bricksDestroyed++;
                    
                    


                    return;
                }
            }
        }
    }

   
    private void CheckGameOver()
    {
        
        if (Canvas.GetTop(ball) > 600)
        {
            
            gameTimer.Stop();

            
            popup.IsOpen = true;
        }
    }

    private void CheckGameWin()
    {
        if (bricksDestroyed == 50)
        {
            gameTimer.Stop();

            popupWin.IsOpen = true;
        }
    }
        
    
    private void OnPointerMoved(object sender, PointerEventArgs e)
    {
        
        var point = e.GetPosition(this);

        
        paddle.Move(point.X - 50);
    }

    
    
}