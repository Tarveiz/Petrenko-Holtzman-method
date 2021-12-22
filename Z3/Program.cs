using System;
using System.IO;
using System.Collections.Generic;

namespace Z3
{
    class Program
    {
        static string Last;
        static List<double> ruList = new List<double>();
        static List<double> enList = new List<double>();
        static double enIndexComment = 0;

        static void Main()
        {
            bool isNormal = SRFile();
            if (isNormal)
            {
                Z3();
            }
        }
        static bool SRFile()
        {
            string ruFilename = "FileRu.txt";
            string engfilename = "FileEng.txt";
            StreamReader ru = new StreamReader(ruFilename);
            while (!ru.EndOfStream)
            {
                string ruStr = ru.ReadLine();
                if (ruStr != "")
                {
                    Last = ruStr;
                    ruList.Add(Index(ruStr));
                }
            }
            ru.Close();
            double[] ind = lastIndex();
            if (ind.Length != 0)
            {
                ruList.RemoveAt(ruList.Count - 1);
                ruList.Add(ind[0]);
            }
            else
            {
                Console.WriteLine("Ошибка 1. В комментарии русских строк неправильное количество слов.");
                return false;
            }
            StreamReader eng = new StreamReader(engfilename);
            while (!eng.EndOfStream)
            {
                string enStr = eng.ReadLine();

                if (enStr != "")
                {
                    Last = enStr;
                    enList.Add(Index(enStr));
                }

            }
            eng.Close();
            ind = lastIndex();
            if (ind.Length != 0)
            {
                enList.RemoveAt(enList.Count - 1);
                enList.Add(ind[0]);
                enIndexComment = ind[1]; // индекс английского комментария
            }
            else
            {
                Console.WriteLine("Ошибка 2. В комментарии английских строк неправильное количество слов.");
                return false;
            }
            return true;
        }

        static double Index(string str)
        {
            int i = 0;
            double ix = -0.5;
            foreach (char ch in str)
            {
                if (char.IsLetter(ch) || char.IsDigit(ch))
                {
                    ix += 1;
                }
                i++;
            }
            return ix * i;
        }
        static double[] lastIndex()
        {
            int wordCounter = 0;
            int stringLength = 0;
            int commentLength = 0;
            double ix = -0.5;
            double iy = -0.5;
            bool slashChecked = false;
            List<char> wordList = new List<char>();
            foreach (char ch in Last)
            {
                if (Char.ToString(ch) == "|")
                {
                    slashChecked = true;

                }
                if (slashChecked == false)
                {
                    stringLength += 1;
                    if (char.IsLetter(ch) || char.IsDigit(ch))
                    {
                        ix += 1;
                    }
                }
                if (slashChecked == true)
                {
                    iy += 1;
                    commentLength += 1;
                    if (Char.IsWhiteSpace(ch) && wordList.Count >= 1)
                    {
                        wordCounter += 1;
                        wordList.Clear();
                    }
                    if (char.IsLetter(ch) || char.IsDigit(ch))
                    {
                        wordList.Add(ch);
                    }
                }
            }
            if ((wordCounter >= 1 && wordCounter <= 5) || (wordList.Count >=1 && wordCounter <= 5) )
            {

                double[] arr = new double[] { ix * stringLength, iy * commentLength };
                return arr;
            }
            else { double[] error = new double[] { }; return error; }
        }
        static void Z3()
        {
            int i = 0;
            int ruCounter = 0;
            int enCounter = 0;
            foreach (double ruIndex in ruList)
            {
                foreach (double enIndex in enList)
                {
                    if (ruIndex == enIndex + enIndexComment)
                    {
                        Console.WriteLine("Русская строка с индексом:" + ruIndex + "под номером:" + ruCounter + "равняется сумме английской строки с индексом:" + enIndex + "под номером:" + enCounter + "и комментария с индексом:" + enIndexComment);
                        i++;
                    }
                    enCounter++;
                }
                ruCounter++;
            }
            if (i == 0)
            {
                Console.WriteLine("Ни для одной из строк не найдена нужная сумма");
                return;
            }
        }
    }
}
