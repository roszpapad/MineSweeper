using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    class PlayerScore
    {

        public int Score { get; set; }
        public string Name { get; set; }

        public PlayerScore(int score)
        {
            Score = score;
        }

        public override string ToString()
        {
            int length = 40 - Name.Length;
            //return string.Format("{0, -" + length + "}{1}", Name, Score);
            return Name.PadRight(length) + Score;
        }
    }
}
