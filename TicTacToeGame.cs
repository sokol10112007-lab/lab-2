using System;
using System.Collections.Generic;

namespace TicTacToeLab2
{
    public enum Player
    {
        Empty,
        Human,
        Computer
    }

    public enum ComputerLevel
    {
        Random,
        BlockHuman,
        WinBlockAndRandom
    }

    public class TicTacToeGame
    {
        public const int BoardSize = 3;

        public Player[,] Board { get; private set; }

        private readonly Random random = new Random();

        public TicTacToeGame()
        {
            Board = new Player[BoardSize, BoardSize];
            Reset();
        }

        public void Reset()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Board[row, col] = Player.Empty;
                }
            }
        }

        public bool MakeMove(int row, int col, Player player)
        {
            if (row < 0 || row >= BoardSize ||
                col < 0 || col >= BoardSize)
            {
                return false;
            }

            if (player == Player.Empty)
            {
                return false;
            }

            if (Board[row, col] != Player.Empty)
            {
                return false;
            }

            Board[row, col] = player;
            return true;
        }

        public List<(int row, int col)> GetAvailableMoves()
        {
            List<(int row, int col)> moves =
                new List<(int row, int col)>();

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (Board[row, col] == Player.Empty)
                    {
                        moves.Add((row, col));
                    }
                }
            }

            return moves;
        }

        public Player CheckWinner()
        {
            
            for (int row = 0; row < BoardSize; row++)
            {
                Player first = Board[row, 0];

                if (first != Player.Empty)
                {
                    bool win = true;

                    for (int col = 1; col < BoardSize; col++)
                    {
                        if (Board[row, col] != first)
                        {
                            win = false;
                            break;
                        }
                    }

                    if (win)
                    {
                        return first;
                    }
                }
            }

           
            for (int col = 0; col < BoardSize; col++)
            {
                Player first = Board[0, col];

                if (first != Player.Empty)
                {
                    bool win = true;

                    for (int row = 1; row < BoardSize; row++)
                    {
                        if (Board[row, col] != first)
                        {
                            win = false;
                            break;
                        }
                    }

                    if (win)
                    {
                        return first;
                    }
                }
            }

            Player diagonal = Board[0, 0];

            if (diagonal != Player.Empty)
            {
                bool win = true;

                for (int i = 1; i < BoardSize; i++)
                {
                    if (Board[i, i] != diagonal)
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                {
                    return diagonal;
                }
            }

            diagonal = Board[0, BoardSize - 1];

            if (diagonal != Player.Empty)
            {
                bool win = true;

                for (int i = 1; i < BoardSize; i++)
                {
                    if (Board[i, BoardSize - 1 - i] != diagonal)
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                {
                    return diagonal;
                }
            }

            return Player.Empty;
        }

        public bool IsDraw()
        {
            return CheckWinner() == Player.Empty &&
                   GetAvailableMoves().Count == 0;
        }

        public (int row, int col)? ChooseComputerMove(
            ComputerLevel level)
        {
            List<(int row, int col)> moves =
                GetAvailableMoves();

            if (moves.Count == 0)
            {
                return null;
            }

            if (level == ComputerLevel.WinBlockAndRandom)
            {
                foreach (var move in moves)
                {
                    Board[move.row, move.col] = Player.Computer;

                    if (CheckWinner() == Player.Computer)
                    {
                        Board[move.row, move.col] = Player.Empty;
                        return move;
                    }

                    Board[move.row, move.col] = Player.Empty;
                }
            }

           
            if (level == ComputerLevel.BlockHuman ||
                level == ComputerLevel.WinBlockAndRandom)
            {
                foreach (var move in moves)
                {
                    Board[move.row, move.col] = Player.Human;

                    if (CheckWinner() == Player.Human)
                    {
                        Board[move.row, move.col] = Player.Empty;
                        return move;
                    }

                    Board[move.row, move.col] = Player.Empty;
                }
            }

           
            return moves[random.Next(moves.Count)];
        }
    }
}