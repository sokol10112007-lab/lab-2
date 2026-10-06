using System.Windows;

namespace TicTacToeLab2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}