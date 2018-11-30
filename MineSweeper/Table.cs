using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    class Table
    {

        private Field[,] fields;
        int Mines { get; set; }
        int Rows { get; set; }
        int Columns { get; set; }

        public Table(int rows, int columns, int mines)
        {
            this.Mines = mines;
            this.Rows = rows;
            this.Columns = columns;
            this.fields = new Field[rows, columns];
            initMines();
            initValues();
        }

        private void initMines()
        {
            Random random = new Random();

            while (Mines > 0)
            {
                int randomRow = random.Next(Rows);
                int randomCol = random.Next(Columns);

                if (!fields[randomRow, randomCol].IsMine)
                {
                    fields[randomRow, randomCol].IsMine = true;
                    Mines--;
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
                    fieldsToReturn.Add(fields[i - 1, j + 1]);
                    fieldsToReturn.Add(fields[i, j + 1]);
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
    }
}
