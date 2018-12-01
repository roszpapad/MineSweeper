using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    class Field
    {

        public int Value { get; set; }
        public bool IsMine { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlaged { get; set; }

        public Field()
        {
            Value = 0;
            this.IsMine = false;
            this.IsRevealed = false;
            this.IsFlaged = false;
        }
    }
}
