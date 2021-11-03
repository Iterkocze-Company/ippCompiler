using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ippCompiler
{
    public static class Interpreter
    {
        private static List<string> VAR_NAMES_INT = new();
        private static List<int> VAR_VALS_INT = new();

        private static List<string> VAR_NAMES_FLOAT = new();
        private static List<float> VAR_VALS_FLOAT = new();

        private static List<string> VAR_NAMES_DOUBLE = new();
        private static List<double> VAR_VALS_DOUBLE = new();

        private static List<string> VAR_NAMES_CHAR = new();
        private static List<char> VAR_VALS_CHAR = new();

        private static List<string> VAR_NAMES_STRING = new();
        private static List<string> VAR_VALS_STRING = new();

        private static string[] ReadFileContents(string pathToFile)
        {
            return File.ReadAllText(pathToFile).Replace("\r\n", "").Split(";");
        }

        private static string[] lines = ReadFileContents(Program.CODE_FILE_PATH);

        public static void Interpret()
        {
            Console.Clear();
            Log.Debug("Interpreter Języka ipp");
            
            foreach (string line in lines)
            {
                if (line != lines[lines.Length - 1])
                {
                    string[] parts = line.Split(" ");
                    int a = 0;

                    while (parts[a] == "") a++;

                    string afterFirst = line;
                    string toFirst = "";
                    for (int j = 0; j <= a; j++)
                    {
                        if (parts[j] == "") toFirst += " ";
                        else toFirst += parts[j];
                    }
                    afterFirst = afterFirst.Replace(toFirst, "");
                    SyntaxChecker.Analyse(line);

                    switch (parts[a].Replace("\t", ""))
                    {
                        case "EchoLine":
                            string textToEchoLine = line.Replace("\"", "").Replace("EchoLine", "").Replace("\t", "");
                            string textToEchoLine2 = "";
                            foreach (char c in textToEchoLine)
                            {
                                if (c != ' ') textToEchoLine2 += c;
                                if (c == '~') textToEchoLine2 += " ";
                                foreach (string name in VAR_NAMES_INT)
                                {
                                    if (name == c.ToString())
                                    {
                                        textToEchoLine2 += VAR_VALS_INT[VAR_NAMES_INT.IndexOf(c.ToString())];
                                        textToEchoLine2 = textToEchoLine2.Replace(c.ToString(), "");
                                    }
                                }
                                foreach (string name in VAR_NAMES_FLOAT)
                                {
                                    if (name == c.ToString())
                                    {
                                        textToEchoLine2 += VAR_VALS_FLOAT[VAR_NAMES_FLOAT.IndexOf(c.ToString())];
                                        textToEchoLine2 = textToEchoLine2.Replace(c.ToString(), "");
                                    }
                                }
                            }
                            Console.WriteLine(textToEchoLine2.Replace("~", ""));
                            break;

                        case "Echo":
                            string textToEcho = line.Replace("\"", "").Replace("Echo", "").Replace("\t", "");
                            string textToEcho2 = "";
                            foreach (char c in textToEcho)
                            {
                                if (c != ' ') textToEcho2 += c;
                                if (c == '~') textToEcho2 += " ";
                                foreach (string name in VAR_NAMES_INT)
                                {
                                    if (name == c.ToString())
                                    {
                                        textToEcho2 += VAR_VALS_INT[VAR_NAMES_INT.IndexOf(c.ToString())];
                                        textToEcho2 = textToEcho2.Replace(c.ToString(), "");
                                    }
                                }
                                foreach (string name in VAR_NAMES_FLOAT)
                                {
                                    if (name == c.ToString())
                                    {
                                        textToEcho2 += VAR_VALS_FLOAT[VAR_NAMES_FLOAT.IndexOf(c.ToString())];
                                        textToEcho2 = textToEcho2.Replace(c.ToString(), "");
                                    }
                                }
                            }
                            Console.Write(textToEcho2.Replace("~", ""));
                            break;

                        //Zmienne
                        case "int":
                            if (line.Contains("="))
                            {
                                string var_name = line.Replace("int", "").Replace("=", "").Trim();
                                string var_val = "";
                                foreach (char c in var_name)
                                {
                                    if (Char.IsDigit(c)) var_val += c;
                                }
                                var_name = var_name.Remove(var_name.IndexOf(" "));
                                VAR_NAMES_INT.Add(var_name);
                                VAR_VALS_INT.Add(int.Parse(var_val));
                            }
                            else
                            {
                                string var_name = line.Replace("int", "").Trim();
                                VAR_NAMES_INT.Add(var_name);
                                VAR_VALS_INT.Add(0);
                            }
                            break;
                        case "float":
                            if (line.Contains("="))
                            {
                                string var_name = line.Replace("float", "").Replace("=", "").Trim();
                                string var_val = "";
                                foreach (char c in var_name)
                                {
                                    if (Char.IsDigit(c) || c == '.') var_val += c;
                                }
                                var_name = var_name.Remove(var_name.IndexOf(" "));
                                VAR_NAMES_FLOAT.Add(var_name);
                                VAR_VALS_FLOAT.Add(float.Parse(var_val, CultureInfo.InvariantCulture.NumberFormat));
                            }
                            else
                            {
                                string var_name = line.Replace("float", "").Trim();
                                VAR_NAMES_FLOAT.Add(var_name);
                                VAR_VALS_FLOAT.Add(0);
                            }
                            break;

                    }
                }
            }
            Log.Debug("\nZakończono interpretowanie programu.\n");
            Console.ReadKey();
        }
    }
}
