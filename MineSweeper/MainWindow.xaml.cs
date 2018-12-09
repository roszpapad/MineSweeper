using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private String Level { get; set; }
        private int Rows { get; set; }
        private int Columns { get; set; }
        private int Mines { get; set; }
        private Button[,] buttons;
        private const int BOX_SIZE = 20;
        private int GameWidth { get; set; }
        private int GameHeight { get; set; }
        private static System.Windows.Forms.Timer timer;
        private static int seconds;
        private bool isFirstClick;

        public MainWindow()
        {
            InitializeComponent();
            initStartingValues();
            initComponents();
            
            initGameField();
                       
        }

        private void initStartingValues()
        {
            Level = "Beginner";
            Rows = 9;
            Columns = 9;
            Mines = 10;
        }

        private void initGameField()
        {
            ButtonGrid.Children.Clear();
            ButtonGrid.RowDefinitions.Clear();
            ButtonGrid.ColumnDefinitions.Clear();
  
            int i, j;
            
            buttons = new Button[table.Rows, table.Columns];
            for (i = 0; i < table.Rows; i++)
            {
                for (j = 0; j < table.Columns; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].Width = BOX_SIZE;
                    buttons[i, j].Height = BOX_SIZE;
                }
            }

            ButtonGrid.Width = GameWidth;
            ButtonGrid.Height = GameHeight;

          
            for (i = 0; i < table.Rows; i++)
            {
                ButtonGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (i = 0; i < table.Columns; i++)
            {
                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (i = 0; i < table.Rows; i++)
            {
                for (j = 0; j < table.Columns; j++)
                {
                    Button button = buttons[i, j];
                    button.Click += hitButton;
                    button.MouseRightButtonUp += rightClickButton;
                    button.Background = Brushes.LightGray;                                      
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    ButtonGrid.Children.Add(button);
                }
            }

            timer.Start();
        }

        private void initComponents()
        {   
            GameWidth = Columns * BOX_SIZE;
            GameHeight = Rows * BOX_SIZE;
            table = new Table(Rows, Columns, Mines);
            seconds = 0;
            remainingMines.Content = table.Mines;
            isFirstClick = true;
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(countSeconds);
            timer.Interval = 1000;
        }

        private void resetField(object sender, RoutedEventArgs e)
        {
            table = new Table(Rows, Columns, Mines);
            for (int i = 0; i < table.Rows; i++)
            {
                for (int j = 0; j < table.Columns; j++)
                {
                    buttons[i, j].IsEnabled = true;
                    buttons[i, j].Content = "";
                    buttons[i, j].Background = Brushes.LightGray;
                }
            }
            timer.Stop();
            seconds = 0;
            timer.Start();
            isFirstClick = true;
        }

        private void changeDifficulty(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            MenuItem difficulty = (MenuItem)sender;
            setDifficulty(difficulty.Header.ToString());
            initComponents();
            initGameField();
        }

        public void setDifficulty(string difficulty)
        {
            StreamReader st = new StreamReader("difficulties.txt");
            string line;
      
            while((line = st.ReadLine()) != null)
            {
                string[] lineParts = line.Split(' ');
                if (lineParts[0].Equals(difficulty))
                {
                    Level = lineParts[0];
                    Rows = Int32.Parse(lineParts[1]);
                    Columns = Int32.Parse(lineParts[2]);
                    Mines = Int32.Parse(lineParts[3]);
                    break;
                }
            }
        }

        public void hitButton(object sender, RoutedEventArgs e)
        {
            Button pressed = (Button)sender;
            int index = ButtonGrid.Children.IndexOf(pressed);
            int pressedRow = index / table.Columns;
            int pressedCol = index % table.Columns;
            buttonClicked(pressedRow, pressedCol);
            if (checkEverythingRevealedWin())
            {
                timer.Stop();
                Score window = new Score(seconds,Level);
                window.Show();
                this.Close();
            }
        }

        private void buttonClicked(int i, int j)
        {

            if (isFirstClick)
            {
                if (table.getFields()[i, j].IsMine)
                {
                    table.changeMine(i, j);
                }
                isFirstClick = false;
            }

            if (!table.getFields()[i, j].IsRevealed && table.getFields()[i, j].IsMine)
            {
                showMines();
                timer.Stop();
                MessageBox.Show("Sajnos ez nem sikerült. Az időd : " + seconds,"Játék vége",MessageBoxButton.OK);
                initComponents();
                initGameField();
                return;
            }

            if (!table.getFields()[i, j].IsRevealed && !table.getFields()[i,j].IsMine)
            {
                table.getFields()[i, j].IsRevealed = true;
                buttons[i, j].IsEnabled = false;
                if (table.getFields()[i, j].Value == 0)
                {
                    buttons[i, j].Content = "";
                }
                else
                {
                    buttons[i, j].Content = table.getFields()[i, j].Value;
                }
                if (table.getFields()[i, j].Value == 0)
                {
                    if (i >= 0 && i < Rows - 1)
                    {
                        buttonClicked(i + 1, j);
                        if (j >= 0 && j < Columns - 1)
                            buttonClicked(i + 1, j + 1);
                        
                        if (j > 0 && j < Columns)
                            buttonClicked(i + 1, j - 1);
                    }

                    if (i > 0 && i < Rows)
                    {
                        buttonClicked(i - 1, j);
                        if (j > 0 && j < Columns)
                            buttonClicked(i - 1, j - 1);

                        if (j >= 0 && j < Columns - 1)
                            buttonClicked(i - 1, j + 1);
                    }

                    if (j >= 0 && j < Columns - 1)
                        buttonClicked(i, j + 1);

                    if (j > 0 && j < Columns)
                        buttonClicked(i, j - 1);
                }
            }
        }

        private void showMines()
        {
            for (int i = 0; i < table.Rows; i++)
            {
                for (int j = 0; j < table.Columns; j++)
                {
                    if (table.getFields()[i, j].IsMine)
                        buttons[i, j].Background = Brushes.Black;
                }
            }
        }

        private void hideMines()
        {
            for (int i = 0; i < table.Rows; i++)
            {
                for (int j = 0; j < table.Columns; j++)
                {
                    if (table.getFields()[i, j].IsMine)
                        buttons[i, j].Background = Brushes.LightGray;
                }
            }
        }

        public void countSeconds(object sender, EventArgs e)
        {
            seconds++;
            scoreLabel.Content = seconds;
        }

        public void rightClickButton(object sender, RoutedEventArgs e)
        {
            Button pressed = (Button)sender;
            int index = ButtonGrid.Children.IndexOf(pressed);
            int pressedRow = index / table.Columns;
            int pressedCol = index % table.Columns;

            if (!table.getFields()[pressedRow, pressedCol].IsFlaged)
            {
                buttons[pressedRow, pressedCol].Background = Brushes.Red;
                remainingMines.Content = --table.Mines;
                table.getFields()[pressedRow, pressedCol].IsFlaged = true;
            }
            else
            {
                buttons[pressedRow, pressedCol].Background = Brushes.LightGray;
                remainingMines.Content = ++table.Mines;
                table.getFields()[pressedRow, pressedCol].IsFlaged = false;
            }

            if (checkFlaggedWin())
            {
                timer.Stop();
                Score window = new Score(seconds, Level);
                window.Show();
                this.Close();
            }

        }

        private bool checkFlaggedWin()
        {

            if (table.Mines == 0)
            {
                for (int i = 0; i < table.Rows; i++)
                {
                    for (int j = 0; j < table.Columns; j++)
                    {
                        if (!table.getFields()[i, j].IsFlaged && table.getFields()[i, j].IsMine)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private bool checkEverythingRevealedWin()
        {
            for (int i = 0; i < table.Rows; i++)
            {
                for (int j = 0; j < table.Columns; j++)
                {
                    if (!table.getFields()[i, j].IsRevealed && !table.getFields()[i, j].IsMine)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void exitGame(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void gameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S)
            {
                showMines();
            }

            if (e.Key == Key.H)
            {
                hideMines();
            }
        }
    }
}
