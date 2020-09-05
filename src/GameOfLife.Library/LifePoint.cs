using System;
using System.Diagnostics;
using System.Drawing;

namespace GameOfLife.Library
{
    public class LifePoint
    {
        /// <summary>
        /// Reference's a C# definition of a 2D point.
        /// </summary>
        public Point Point { get; set; }

        /// <summary>
        /// Reference's if the point is alive or not.
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        /// Reference's the game the point is associated to.
        /// </summary>
        private readonly LifeGame _pointGame;

        /// <summary>
        /// Creates a new instance of a LifePoint.
        /// </summary>
        /// <param name="game">References the game the point is associated to</param>
        /// <param name="point">C# definition of 2D point</param>
        /// <param name="isAlive">Reference's if the point is alive</param>
        public LifePoint(LifeGame game, Point point, bool isAlive = false)
        {
            _pointGame = game;
            Point = point;
            IsAlive = isAlive;
        }

        /// <summary>
        /// Gets the neighbors of the point.
        /// </summary>
        /// <returns></returns>
        public int GetAliveNeighbors()
        {
            var count = 0;
            for (var x = -1; x < 2; x++)
            {
                for (var y = -1; y < 2; y++)
                {
                    var point = new Point(Point.X + x, Point.Y + y);
                    var gameBoardPoint = _pointGame.GetPointFromBoard(point);
                    if (!ReferenceEquals(null, gameBoardPoint) && gameBoardPoint.IsAlive)
                    {
                        count++;
                    }
                }
            }

            // the loop will count the centre one (this point)
            // just take 1 away to solve :)
            if (IsAlive)
            {
                count--;
            }

            return count;
        }

        /// <summary>
        /// Reference's what should happen to the point
        /// at the next generation.
        ///
        /// 1/ Any live cell with fewer than two live neighbors dies, as if by underpopulation.
        /// 2/ Any live cell with two or three live neighbors lives on to the next generation.
        /// 3/ Any live cell with more than three live neighbors dies, as if by overpopulation.
        /// 4/ Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
        /// </summary>
        public bool GetNextGenerationResult()
        {
            var neighbors = GetAliveNeighbors();

            if (neighbors < 2 && IsAlive)
            {
                // 1/ Any live cell with fewer than two live neighbors dies, as if by underpopulation.
                return false;
            }

            if ((neighbors == 2 || neighbors == 3) && IsAlive)
            {
                // 2/ Any live cell with two or three live neighbors lives on to the next generation.
                return true;
            }

            if (neighbors > 3 && IsAlive)
            {
                // 3/ Any live cell with more than three live neighbors dies, as if by overpopulation.
                return false;
            }

            // 4/ Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
            return neighbors == 3 && !IsAlive;
        }
    }
}
