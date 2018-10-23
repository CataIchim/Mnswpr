using System;
using System.IO;
using System.Text;

namespace Minesweeper
{
    class Program
    {
        static byte?[,] mat = new byte?[100, 100];
        static int lineCount = 1, colCount = 1;
        const string inputFileName = "minefields/minefield3.txt";
        const string outputFileName = "cheatsheets/minefield3_cheatsheet.txt";

        static void Main(string[] args)
        {
            InitMatrix(mat);

            ProcessFile();

            WriteCheatsheet();

            Console.WriteLine($"Cheatsheet generated at {outputFileName}. Enjoy!");
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }

        private static void ProcessFile()
        {
            var inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(inputFileStream, Encoding.UTF8))
            {

                int next = 0;

                while ((next = streamReader.Read()) != -1)
                {
                    ProcessCharacter(next);
                }

            }
        }

        private static void ProcessCharacter(int next)
        {
            switch (next)
            {
                case 13: //line feed
                    colCount = 1;
                    lineCount++;
                    break;
                case 10: //carriage return
                    break;
                case 42: //* symbol
                    mat[lineCount, colCount] = null;
                    mat[lineCount - 1, colCount - 1] |= 1 << 1;
                    mat[lineCount - 1, colCount] |= 1 << 2;
                    mat[lineCount - 1, colCount + 1] |= 1 << 3;
                    mat[lineCount, colCount + 1] |= 1 << 4;
                    mat[lineCount + 1, colCount + 1] |= 1 << 5;
                    mat[lineCount + 1, colCount] |= 1 << 6;
                    mat[lineCount + 1, colCount - 1] |= 1 << 7;
                    mat[lineCount, colCount - 1] |= 1 << 0;
                    colCount++;
                    break;
                default:
                    colCount++;
                    break;
            }
        }

        private static void WriteCheatsheet()
        {
            var outputFileStream = new FileStream($"{outputFileName}", FileMode.OpenOrCreate, FileAccess.Write);
            using (var streamWriter = new StreamWriter(outputFileStream, Encoding.UTF8))
            {
                for (int i = 1; i < lineCount + 1; i++)
                {
                    for (int j = 1; j < colCount; j++)
                    {
                        streamWriter.Write(CountBits(mat[i, j]));
                        streamWriter.Write(" ");
                    }
                    streamWriter.WriteLine();
                }
            }
        }

        private static void InitMatrix(byte?[,] mat)
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    mat[i, j] = 0;
                }
            }
        }

        private static string CountBits(byte? n)
        {
            if (n == null)
                return "*";
            else
            {
                int value = n.Value;
                int count = 0;
                while (value != 0)
                {
                    count++;
                    value &= (value - 1);
                }
                return count.ToString();
            }
        }
    }
}