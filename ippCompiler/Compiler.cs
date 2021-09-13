using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ippCompiler
{
    public static class Compiler
    {
        private static int FUNCTION_FLAG = 0; //0= int, 1 = string
        public static List<string> VARS = new List<string>(); //Zawiera nazwy wszystkich zadeklarowancyh zmiennych i funkcji.

        public static string[] ReadFileContents(string pathToFile)
        {
            return File.ReadAllText(pathToFile).Replace("\r\n", "").Split(";");
        }

        public static string deleteFirstArg(string str)
        {
            string opt = "";
            bool firstSpace = false;

            foreach(char ch in str)
            {
                if (firstSpace) opt = opt + ch;
                if (ch == ' ') firstSpace = true;
            }

            return opt;
        }

        public static void Compile()
        {
            string[] lines = ReadFileContents(Program.CODE_FILE_PATH);
            string[] GeneratedCode = new string[Program.FILE_LEN+4];
            int index = 0;
            
            foreach (string line in lines)
            {
                if (line != lines[lines.Length - 1])
                {
                    string parts = line.Split(" ");
                    int a = 0;

                    while (parts[a] == "") a = a + 1;

                    string afterFirst = line;
                    string toFirst = "";
                    for (int i = 0; i <= a; i = i + 1)
                    {
                        if (parts[i] == "") toFirst = toFirst + " ";
                        else toFirst = toFirst + parts[i];
                    }
                    afterFirst = afterFirst.Replace(toFirst, "");

                    //switch (line.Split(" ")[0])
                    switch (parts[a].Replace("\t", ""))
                    {
                        case "echoLine":
                            GeneratedCode[index] = "cout << ";
                            
                            foreach (char ch in afterFirst)
                            {
                                if (ch == '+')
                                    GeneratedCode[index] = GeneratedCode[index] + " << ";
                                else
                                {
                                    if (ch == ' ')
                                    {
                                        GeneratedCode[index] = GeneratedCode[index] + " ";
                                    }
                                    else
                                    {
                                        GeneratedCode[index] = GeneratedCode[index] + ch;
                                    }
                                }
                            }
                            GeneratedCode[index] += " << endl;";
                            index++;
                            break;

                        case "echo":
                            GeneratedCode[index] = "cout << ";
                            
                            foreach (char ch in afterFirst) 
                            {
                                if (ch == '+')
                                    GeneratedCode[index] = GeneratedCode[index] + " << ";
                                else
                                {
                                    if (ch == ' ')
                                    {
                                        GeneratedCode[index] = GeneratedCode[index] + " ";
                                    }
                                    else
                                    {
                                        GeneratedCode[index] = GeneratedCode[index] + ch;
                                    }
                                }
                            }
                            GeneratedCode[index] += " ;";
                            index++;
                            break;

                        case "int":
                            GeneratedCode[index] = "int ";
                            if (line.EndsWith("readKey"))
                            {
                                string nameRead = lines[index].Substring(lines[index].IndexOf(' ')).Replace(" ", "");
                                nameRead = nameRead.Substring(0, nameRead.IndexOf('='));
                                GeneratedCode[index] = $"int {nameRead} =  getch();";
                                index++;
                                break;
                            }
                            string name = lines[index].Substring(lines[index].IndexOf(' ')).Replace(" ", "");

                            GeneratedCode[index] = GeneratedCode[index] + name + ";";

                            if (name.Contains('='))
                                name = name.Substring(0, name.IndexOf('='));

                            VARS.Add(name);

                            index++;
                            break;

                        case "string":
                            GeneratedCode[index] = "string ";

                            string nameStr = lines[index].Substring(lines[index].IndexOf(' ')).Replace(" ", "");
                            GeneratedCode[index] = GeneratedCode[index] + nameStr + ";";
                            if (nameStr.Contains('='))
                                nameStr = nameStr.Substring(0, nameStr.IndexOf('='));

                            VARS.Add(nameStr);

                            index++;
                            break;

                        case "end":

                            if (FUNCTION_FLAG == 0)
                                GeneratedCode[index] = "return 0;";
                            if (FUNCTION_FLAG == 1)
                                GeneratedCode[index] = "return \"0\";";

                            index++;
                            GeneratedCode[index] = "}";

                            index++;
                            break;

                        case "readKey":
                            GeneratedCode[index] = "getch();";
                            index++;
                            break;

                        default:
                            break;
                    }
                }

                foreach (string var in VARS)
                {
                    if (line.Contains(var) && line.Replace(" ", "").Replace("\t", "").StartsWith(var[0]))
                    {
                        if (line.EndsWith("readKey"))
                        {
                            GeneratedCode[index] = var + " = getch();";
                            index++;
                            break;
                        }

                        if (line.EndsWith("readString"))
                        {
                            GeneratedCode[index] = "cin >> " + var + ";";
                            index++;
                            break;
                        }

                        if (line.EndsWith("++"))
                        {
                            GeneratedCode[index] = var + "++;";
                            index++;
                            break;
                        }

                        if (line.Contains('*'))
                        {
                            string valMul = line.Substring(line.IndexOf('*')+1);
                            GeneratedCode[index] = var + "=" + var + "*" + valMul + ";";
                            index++;
                            break;
                        }

                        if (line.Contains('/'))
                        {
                            string valMul = line.Substring(line.IndexOf('/') + 1);
                            GeneratedCode[index] = var + "=" + var + "/" + valMul + ";";
                            index++;
                            break;
                        }

                        if (line.EndsWith("()"))
                        {
                            GeneratedCode[index] = var + "();";
                            index++;
                            break;
                        }

                        string val = line.Substring(line.IndexOf('=')+1);
                        val = val.Replace(" ", "");
                        GeneratedCode[index] = var + "=" + (string)val + ";";
                        index++;
                    }
                }

                if (line.ToLower().Contains("def")) //Obsługa funkcji. Póki co nie wiem jak to zrobić.
                {
                    string[] splitted = line.Split();
                    string funcName = splitted[2];
                    VARS.Add(funcName);

                    foreach (string lineDef in splitted)
                    {
                        if (lineDef.Contains("int"))
                        {
                            FUNCTION_FLAG = 0;
                            GeneratedCode[index] += "int ";
                            GeneratedCode[index] += funcName + "()";
                            index++;
                            GeneratedCode[index] = "{";
                            index++;
                        }

                        if (lineDef.Contains("string"))
                        {
                            FUNCTION_FLAG = 1;
                            GeneratedCode[index] += "string ";
                            GeneratedCode[index] += funcName + "()";
                            index++;
                            GeneratedCode[index] = "{";
                            index++;
                        }
                    }
                }
            }

            File.WriteAllText("genCode.cpp", "");
            //File.AppendAllText("genCode.cpp", "#include <iostream>\n#include <conio.h>\n\nusing namespace std;\n\nint main() {");
            File.AppendAllText("genCode.cpp", "#include <iostream>\n#include <conio.h>\n\nusing namespace std;\n");

            for (int i = 0; i < GeneratedCode.Length; i++)
                File.AppendAllText("genCode.cpp", "\n" + GeneratedCode[i]);

            //File.AppendAllText("genCode.cpp", "\nreturn 0;\n}");

            Console.WriteLine("Kompiluję...");
            string args = "";
            
            
            args = args + $"{"-o " + Program.FLAG_NAME}";
            Process.Start("g++", $"{args} -O2 -s genCode.cpp");
            var startInfo = new ProcessStartInfo();

            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.FileName = Program.FLAG_NAME + ".exe";
            
            Process.Start("g++", "-O2 -s genCode.cpp");
            
            Console.WriteLine("Gotowe");
            if (Program.FLAG_RUN)
            {
                Thread.Sleep(2000);
                Process.Start(startInfo);
            }
            else
                Console.ReadKey();
        }
    }
}
