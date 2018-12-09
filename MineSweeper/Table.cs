using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    class Table
    {

        private Field[,] fields;
        public int Mines { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public Table(int rows, int columns, int mines)
        {
            this.Mines = mines;
            this.Rows = rows;
            this.Columns = columns;
            this.fields = new Field[rows, columns];
            initFields();
            initMines();
            initValues();
        }

        private void initFields()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    fields[i, j] = new Field();
                }
            }
        }

        private void initMines()
        {
            Random random = new Random();
            int minesRemaining = Mines;
            while (minesRemaining > 0)
            {
                
                int randomRow = random.Next(Rows);
                int randomCol = random.Next(Columns);

                if (!fields[randomRow, randomCol].IsMine)
                {
                    fields[randomRow, randomCol].IsMine = true;
                    minesRemaining--;
                }
            }
        }

        private void initValues()
        {
            int i, j;

            for (i = 0; i < Rows; i++)
            {
                for (j = 0; j < Columns; j++)
                {
                    if (fields[i, j].IsMine)
                    {
                        List<Field> fieldsToInc = getSurroundingFields(i, j);
                        fieldsToInc.ForEach(field => field.Value++);
                    }
                }
            }

        }


        private List<Field> getSurroundingFields(int i, int j)
        {
            List<Field> fieldsToReturn = new List<Field>();

            if (i != 0 && j != 0 && i != Rows - 1 && j != Columns - 1)
            {
                fieldsToReturn.Add(fields[i - 1, j + 1]);
                fieldsToReturn.Add(fields[i, j + 1]);
                fieldsToReturn.Add(fields[i + 1, j + 1]);
                fieldsToReturn.Add(fields[i + 1, j]);
                fieldsToReturn.Add(fields[i + 1, j - 1]);
                fieldsToReturn.Add(fields[i, j - 1]);
                fieldsToReturn.Add(fields[i - 1, j - 1]);
                fieldsToReturn.Add(fields[i - 1, j]);
                return fieldsToReturn;
            }

            if (i == 0)
            {
                if (j == 0)
                {
                    fieldsToReturn.Add(fields[i, j + 1]);
                    fieldsToReturn.Add(fields[i + 1, j]);
                    fieldsToReturn.Add(fields[i + 1, j + 1]);
                    return fieldsToReturn;
                }

                if (j == Columns - 1)
                {
                    fieldsToReturn.Add(fields[i, j - 1]);
                    fieldsToReturn.Add(fields[i + 1, j - 1]);
                    fieldsToReturn.Add(fields[i + 1, j]);
                    return fieldsToReturn;
                }

                fieldsToReturn.Add(fields[i, j - 1]);
                fieldsToReturn.Add(fields[i, j + 1]);
                fieldsToReturn.Add(fields[i + 1, j]);
                fieldsToReturn.Add(fields[i + 1, j - 1]);
                fieldsToReturn.Add(fields[i + 1, j + 1]);
                return fieldsToReturn;
            }

            if (i == Rows - 1)
            {
                if (j == 0)
                {
                    fieldsToReturn.Add(fields[i - 1, j]);
                    fieldsToReturn.Add(fields[i - 1, j + 1]);
                    fieldsToReturn.Add(fields[i, j + 1]);
                    return fieldsToReturn;
                }

                if (j == Columns - 1)
                {
                    fieldsToReturn.Add(fields[i - 1, j]);
                    fieldsToReturn.Add(fields[i - 1, j - 1]);
                    fieldsToReturn.Add(fields[i, j - 1]);
                    return fieldsToReturn;
                }

                fieldsToReturn.Add(fields[i, j - 1]);
                fieldsToReturn.Add(fields[i, j + 1]);
                fieldsToReturn.Add(fields[i - 1, j]);
                fieldsToReturn.Add(fields[i - 1, j - 1]);
                fieldsToReturn.Add(fields[i - 1, j + 1]);
                return fieldsToReturn;
            }

            if (j == 0)
            {
                fieldsToReturn.Add(fields[i - 1, j]);
                fieldsToReturn.Add(fields[i + 1, j]);
                fieldsToReturn.Add(fields[i, j + 1]);
                fieldsToReturn.Add(fields[i + 1, j + 1]);
                fieldsToReturn.Add(fields[i - 1, j + 1]);
                return fieldsToReturn;
            }

            if (j == Columns - 1)
            {
                fieldsToReturn.Add(fields[i - 1, j]);
                fieldsToReturn.Add(fields[i + 1, j]);
                fieldsToReturn.Add(fields[i, j - 1]);
                fieldsToReturn.Add(fields[i + 1, j - 1]);
                fieldsToReturn.Add(fields[i - 1, j - 1]);
                return fieldsToReturn;
            }

            return fieldsToReturn;
        }

        public Field[,] getFields()
        {
            return fields;
        }

        public void changeMine(int i, int j)
        {
            List<Field> surroundingFields = getSurroundingFields(i, j);
            fields[i, j].IsMine = false;
            surroundingFields.ForEach(field => field.Value--);
            for (int k = 0; k < Rows; k++)
            {
                for (int l = 0; l < Columns; l++)
                {
                    if (!fields[k, l].IsMine && (i != k || j != l))
                    {
                        fields[k, l].IsMine = true;
                        surroundingFields = getSurroundingFields(k, l);
                        surroundingFields.ForEach(field => field.Value++);
                        return;
                    }
                }
            }
        }
    }

}
