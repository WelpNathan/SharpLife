using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace GameOfLife.Library
{
    public class LifeGame
    {
        /// <summary>
        /// Initialise a new board.
        /// Value:
        ///     true: point is 'alive'
        ///     false: point is 'dead'
        /// </summary>
        //private List<LifePoint> _gameBoard = new List<LifePoint>();

        private LifePoint[,] _gameBoard;

        /// <summary>
        /// Reference's the y axis.
        /// </summary>
        private readonly int _xCount;

        /// <summary>
        /// Reference's the y axis.
        /// </summary>
        private readonly int _yCount;

        /// <summary>
        /// Creates a new instance of LifeGame.
        /// </summary>
        /// <param name="xCount">How many x axis</param>
        /// <param name="yCount">How many y axis</param>
        public LifeGame(int xCount, int yCount)
        {
            _xCount = xCount;
            _yCount = yCount;

            InitialiseGameBoard();
        }

        /// <summary>
        /// Initialise the GameBoard.
        /// </summary>
        private void InitialiseGameBoard()
        {
            _gameBoard = new LifePoint[_xCount, _yCount];

            for (var x = 0; x < _xCount; x++)
            {
                for (var y = 0; y < _yCount; y++)
                {
                    var point = new LifePoint(this, new Point(x, y));
                    _gameBoard[x, y] = point;
                }
            }
        }

        /// <summary>
        /// Searches the game board for the requested point.
        /// </summary>
        /// <param name="point">Point object</param>
        public LifePoint GetPointFromBoard(Point point)
        {
            var x = point.X;
            var y = point.Y;

            if ((x >= 0 && y >= 0) && (point.X < _xCount && point.Y < _yCount))
            {
                return _gameBoard[point.X, point.Y];
            }

            return null;
        }

        /// <summary>
        /// Processes the board through a new generation.
        /// </summary>
        public LifePoint[,] GetAndUpdateNewGeneration()
        {
            // create a new board
            var newBoard = new LifePoint[_xCount, _yCount];

            // loop through the old list
            for (var x = 0; x < _gameBoard.GetLength(0); x++)
            {
                for (var y = 0; y < _gameBoard.GetLength(1); y++)
                {
                    var lifePoint = _gameBoard[x, y];
                    var isAlive = lifePoint.GetNextGenerationResult();

                    var point = lifePoint.Point;
                    var boardPoint = new LifePoint(this, new Point(x, y), isAlive);
                    newBoard[x, y] = boardPoint;
                }
            }

            // update the board
            _gameBoard = newBoard;

            // return the new board!
            return newBoard;
        }
    }
}
