using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace calculatePageRank
{
    class Program
    {
        static void Main(string[] args)
        {
            
            loadMatrix();
        }

        private static void loadMatrix()
        {

            using (StreamReader reader = new StreamReader(@"c:\matrix.txt"))
            {

                int width=94;
                int height = 94;
                //int.TryParse(reader.ReadLine(), out width);
                //int.TryParse(reader.ReadLine(), out height);
                int[,] readValues = new int[width, height];
                for (int i = 0; i < readValues.GetLength(0); i++)
                {
                    for (int j = 0; j < readValues.GetLength(1); j++)
                    {
                        int.TryParse(reader.ReadLine(), out readValues[i, j]);
                    }
                }
                


            }

        }


    }
}
