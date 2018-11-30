using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Table table;
        private int Rows { get; set; }
        private int Columns { get; set; }
        private int Mines { get; set; }
        private Button[,] buttons;

        public MainWindow()
        {
            InitializeComponent();
            //table = new Table(Rows, Columns, Mines);
            initGameField();
        }

        private void initGameField()
        {
            int i, j;
            Rows = 9;
            Columns = 9;
            buttons = new Button[Rows, Columns];
            for (i = 0; i < Rows; i++)
            {
                for (j = 0; j < Columns; j++)
                {
                    buttons[i, j] = new Button();
                }
            }

           

            for (i = 0; i < Rows; i++)
            {
                ButtonGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (i = 0; i < Columns; i++)
            {
                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (i = 0; i < Rows; i++)
            {
                for (j = 0; j < Columns; j++)
                {
                    Button button = buttons[i, j];
                    button.Click += hitButton;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    ButtonGrid.Children.Add(button);
                }
            }
        }

        public void hitButton(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
