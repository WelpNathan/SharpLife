using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GameOfLife.Library;

namespace GameOfLife.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Reference's a new instance of the LifeGame
        /// </summary>
        private LifeGame _game;

        /// <summary>
        /// Reference's a nice black color to use. :)
        /// </summary>
        private readonly SolidColorBrush _blackColor = (SolidColorBrush) new BrushConverter().ConvertFrom("#111111");
        
        /// <summary>
        /// Reference's the current generation of the game.
        /// </summary>
        private int _currentGeneration;

        /// <summary>
        /// Reference's if the game is running or not.
        /// </summary>
        private bool _isGameRunning;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialise the grid by building the
        /// required buttons and definitions.
        /// </summary>
        private void InitialiseGrid(int rowCount, int columnCount)
        {
            // reference master game grid
            var gameGrid = this.GameGrid;

            // set default definition width and height
            var columnDefinitionWidth = new GridLength(1, GridUnitType.Auto);
            var rowDefinitionHeight = new GridLength(1, GridUnitType.Auto);

            // generate the required buttons and definitions
            for (var x = 0; x < columnCount; x++)
            {
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = columnDefinitionWidth });
                for (var y = 0; y < rowCount; y++)
                {
                    gameGrid.RowDefinitions.Add(new RowDefinition {Height = rowDefinitionHeight});

                    var btn = new Button { Background = _blackColor, Height = 20, Width = 20 };
                    btn.Click += GridBtn_OnClick;
                    btn.SetValue(Grid.ColumnProperty, x);
                    btn.SetValue(Grid.RowProperty, y);
                    btn.Tag = new Point(x, y);
                    gameGrid.Children.Add(btn);
                }
            }
        }

        /// <summary>
        /// Searches through the children and returns the
        /// button object from the point.
        /// </summary>
        private Button GetButtonFromPoint(Point point)
        {
            return this.GameGrid.Children.Cast<Button>().FirstOrDefault(btn => btn.Tag.Equals(point));
        }

        /// <summary>
        /// Resets the grid.
        /// </summary>
        private void ResetGame()
        {
            // stop the game from performing any more calculations
            _isGameRunning = false;

            // reference's the master game grid
            var gameGrid = this.GameGrid;

            // reset all contents of the grid
            gameGrid.Children.Clear();

            // get the row and column count for the grid
            var rowCount = (int)gameGrid.ActualHeight / 20;
            var columnCount = (int) gameGrid.ActualWidth / 20;

            // initialise a new game instance
            _game = new LifeGame(columnCount, rowCount);

            // reset current generation
            _currentGeneration = 0;

            // initialise grid
            InitialiseGrid(rowCount, columnCount);

            // reset generation tag
            this.GenerationTag.Text = "Generation 0";

            // enable start button again
            // disable restart button
            this.StartBtn.IsEnabled = true;
            this.ResetBtn.IsEnabled = false;
        }

        /// <summary>
        /// Runs when the main window has loaded.
        /// </summary>
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) => ResetGame();

        /// <summary>
        /// Runs when the start button is pressed.
        /// </summary>
        private async void StartBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var generationLabel = this.GenerationTag;

            // invert the current running state
            _isGameRunning = true;

            // if the game is now running, disable the start button
            // then enable the reset button
            this.StartBtn.IsEnabled = false;
            this.ResetBtn.IsEnabled = true;

            // keep updating the game state
            while (_isGameRunning)
            {
                // create new stopwatch instance
                var watch = Stopwatch.StartNew();
                watch.Start();

                var results = _game.GetAndUpdateNewGeneration();
                for (var x = 0; x < results.GetLength(0); x++)
                {
                    for (var y = 0; y < results.GetLength(1); y++)
                    {
                        var lifePoint = results[x, y];
                        var btn = GetButtonFromPoint(new Point(x, y));
                        btn.Background = lifePoint.IsAlive ? new SolidColorBrush(Colors.Green) : _blackColor;
                    }
                }

                watch.Stop();

                _currentGeneration++;
                generationLabel.Text = $"Generation {_currentGeneration} ({watch.ElapsedMilliseconds}ms)";
                await Task.Delay(50);
            }
        }

        /// <summary>
        /// When a grid button is clicked, set their IsAlive status
        /// to true!
        /// </summary>
        private void GridBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = (Button) sender;
            var point = (Point) btn.Tag;

            // gets the point from the button tag
            var gamePoint = _game.GetPointFromBoard(new System.Drawing.Point((int) point.X, (int) point.Y));

            // alternate between values
            gamePoint.IsAlive = !gamePoint.IsAlive;

            // set the background color depending on if it's alive
            btn.Background = gamePoint.IsAlive ? new SolidColorBrush(Colors.Green) : _blackColor;
        }

        private void ResetBtn_OnClick(object sender, RoutedEventArgs e) => ResetGame();
    }
}
