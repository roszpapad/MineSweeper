using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for Score.xaml
    /// </summary>
    public partial class Score : Window
    {
        private PlayerScore playerScore;
        private string difficulty;

        public Score(int score,string difficulty)
        {
            InitializeComponent();
            playerScore = new PlayerScore(score);
            this.difficulty = difficulty;
            topLabel.Content = topLabel.Content + difficulty;
        }

        private void saveScore(object sender, RoutedEventArgs e)
        {
            Button saveButton = (Button) sender;
            saveButton.IsEnabled = false;
            string name = nameTextBox.Text;
            playerScore.Name = name;
            StreamReader st = new StreamReader(difficulty + ".txt");
            string results = st.ReadToEnd();
            st.Close();
            List<PlayerScore> scores = JsonConvert.DeserializeObject<List<PlayerScore>>(results);
            if (scores == null)
            {
                scores = new List<PlayerScore>();
            }
            scores.Add(playerScore);
            showAndSaveScore(scores);
        }

        private void showAndSaveScore(List<PlayerScore> scores)
        {
            scores.Sort((x, y) => x.Score.CompareTo(y.Score));
            List<PlayerScore> scoresToDisplay = new List<PlayerScore>();
            for (int i = 0; i < 10; i++)
            {
                if (i < scores.Count)
                    scoresToDisplay.Add(scores[i]);
            }

            scoreList.ItemsSource = scoresToDisplay;
            
            StreamWriter sw = new StreamWriter(difficulty + ".txt",false);
            sw.Write(JsonConvert.SerializeObject(scores));
            sw.Close();
        }

        private void goBack(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
