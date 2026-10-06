using System.ComponentModel;

namespace TicTacToeLab2
{
    public class CellViewModel : INotifyPropertyChanged
    {
        private string value;

        public int Row { get; }
        public int Col { get; }

        public string Value
        {
            get
            {
                return value;
            }

            set
            {
                if (this.value == value)
                {
                    return;
                }

                this.value = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        public CellViewModel(int row, int col)
        {
            Row = row;
            Col = col;
            Value = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}