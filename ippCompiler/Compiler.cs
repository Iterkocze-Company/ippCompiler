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
        //public static string[] VARS;
        public static List<string> VARS = new List<string>();

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
            string[] GeneratedCode = new string[Program.FILE_LEN+1];
            int index = 0;
            
            foreach (string line in lines)
            {
                string afterEcho = deleteFirstArg(line);

                if (line != lines[lines.Length - 1])
                {
                    switch (line.Split(" ")[0])
                    {
                        case "echoLine":
                            GeneratedCode[index] = "cout << ";
                            
                            foreach (char ch in afterEcho)
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

                        case "echo": // echo "test" + " test2"
                            GeneratedCode[index] = "cout << ";
                            
                            foreach (char ch in afterEcho) 
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

                        case "readKey":
                            GeneratedCode[index] = "getch();";
                            index++;
                            break;

                        default:
                            //Program.Error($"\"{line}\"\n ^ Tutaj błąd");
                            break;
                    }
                }

                foreach (string var in VARS)
                {
                    if (line.Contains(var) && line.StartsWith(var[0]))
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

                        string val = line.Substring(line.IndexOf('=')+1);
                        val = val.Replace(" ", "");
                        GeneratedCode[index] = var + "=" + (string)val + ";";
                        index++;
                    }
                }
            }

            File.WriteAllText("genCode.cpp", "");
            File.AppendAllText("genCode.cpp", "#include <iostream>\n#include <conio.h>\n\nusing namespace std;\n\nint main() {");

            for (int i = 0; i < GeneratedCode.Length; i++)
                File.AppendAllText("genCode.cpp", "\n" + GeneratedCode[i]);

            File.AppendAllText("genCode.cpp", "\nreturn 0;\n}");

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
