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
        private struct Compiler_Flags
        {
        }

        //static string[] Tokens = {"echo"};

        public static string[] ReadFileContents(string pathToFile)
        {
            return File.ReadAllText(pathToFile).Replace("\r\n", " ").Split(";");
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

                        default:
                            Program.Error($"\"{line}\"\n ^ Tutaj błąd");
                            break;
                    }

                }
            }

            File.WriteAllText("genCode.cpp", "");
            File.AppendAllText("genCode.cpp", "#include <iostream>\n\nusing namespace std;\n\nint main() {");

            for (int i = 0; i < GeneratedCode.Length; i++)
                File.AppendAllText("genCode.cpp", "\n" + GeneratedCode[i]);

            File.AppendAllText("genCode.cpp", "\nreturn 0;\n}");

            Console.WriteLine("Kompiluję...");
            Process.Start("g++","genCode.cpp");
        }
    }
}
