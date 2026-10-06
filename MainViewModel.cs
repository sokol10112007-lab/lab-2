using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TicTacToeLab2
{
    public class DifficultyOption
    {
        public ComputerLevel Level { get; }

        public string Name { get; }

        public DifficultyOption(
            ComputerLevel level,
            string name)
        {
            Level = level;
            Name = name;
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly TicTacToeGame game;

        private bool gameOver;

        private DifficultyOption selectedLevel;

        private string status;

        public ObservableCollection<CellViewModel> Cells
        {
            get;
        }

        public ObservableCollection<DifficultyOption> Levels
        {
            get;
        }

        public RelayCommand MakeMoveCommand
        {
            get;
        }

        public RelayCommand NewGameCommand
        {
            get;
        }

        public DifficultyOption SelectedLevel
        {
            get
            {
                return selectedLevel;
            }

            set
            {
                selectedLevel = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(
                        nameof(SelectedLevel)));
            }
        }

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(
                        nameof(Status)));
            }
        }

        public MainViewModel()
        {
            game = new TicTacToeGame();

            Cells = new ObservableCollection<CellViewModel>();

            for (int row = 0;
                 row < TicTacToeGame.BoardSize;
                 row++)
            {
                for (int col = 0;
                     col < TicTacToeGame.BoardSize;
                     col++)
                {
                    Cells.Add(
                        new CellViewModel(row, col));
                }
            }

            Levels = new ObservableCollection<DifficultyOption>
            {
                new DifficultyOption(
                    ComputerLevel.Random,
                    "1 - випадковий хід"),

                new DifficultyOption(
                    ComputerLevel.BlockHuman,
                    "2 - блокує суперника"),

                new DifficultyOption(
                    ComputerLevel.WinBlockAndRandom,
                    "3 - перемагає та блокує")
            };

            SelectedLevel = Levels[0];

            MakeMoveCommand = new RelayCommand(
                parameter => MakeMove(parameter));

            NewGameCommand = new RelayCommand(
                parameter => NewGame());

            Status = "Ваш хід (X)";

            UpdateBoard();
        }

        private void MakeMove(object parameter)
        {
            if (gameOver)
            {
                return;
            }

            CellViewModel cell =
                parameter as CellViewModel;

            if (cell == null)
            {
                return;
            }

            if (!game.MakeMove(
                cell.Row,
                cell.Col,
                Player.Human))
            {
                Status = "Ця клітинка вже зайнята.";
                return;
            }

            UpdateBoard();

            if (FinishGame())
            {
                return;
            }

            var computerMove =
                game.ChooseComputerMove(
                    SelectedLevel.Level);

            if (computerMove.HasValue)
            {
                game.MakeMove(
                    computerMove.Value.row,
                    computerMove.Value.col,
                    Player.Computer);
            }

            UpdateBoard();

            if (FinishGame())
            {
                return;
            }

            Status = "Ваш хід (X)";
        }

        private bool FinishGame()
        {
            Player winner =
                game.CheckWinner();

            if (winner == Player.Human)
            {
                Status = "Ви перемогли!";
                gameOver = true;
                return true;
            }

            if (winner == Player.Computer)
            {
                Status = "Переміг комп'ютер!";
                gameOver = true;
                return true;
            }

            if (game.IsDraw())
            {
                Status = "Нічия!";
                gameOver = true;
                return true;
            }

            return false;
        }

        private void UpdateBoard()
        {
            foreach (CellViewModel cell in Cells)
            {
                Player player =
                    game.Board[cell.Row, cell.Col];

                if (player == Player.Human)
                {
                    cell.Value = "X";
                }
                else if (player == Player.Computer)
                {
                    cell.Value = "O";
                }
                else
                {
                    cell.Value = "";
                }
            }
        }

        private void NewGame()
        {
            game.Reset();

            gameOver = false;

            UpdateBoard();

            Status = "Ваш хід (X)";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}