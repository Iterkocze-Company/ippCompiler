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
            return File.ReadAllText(pathToFile).Replace("\r\n", " ").Split(" ");
        }

        public static void Compile()
        {
            string[] contents = ReadFileContents(Program.CODE_FILE_PATH);
            string[] GeneratedCode = new string[Program.FILE_LEN+10];
            int index = 0;
            foreach (string line in contents)
            {
                if (line == "echo")
                {
                    Console.WriteLine("echo wykryte!");
                    GeneratedCode[index] = $"printf({contents[index + 1]} ";
                    if (contents[index+2].EndsWith("\""))
                        GeneratedCode[index] += $"{contents[index + 2]}";
                    GeneratedCode[index] += ");";
                        
                    index++;
                }
            }

            File.WriteAllText("genCode.c", "");
            File.AppendAllText("genCode.c", "#include <stdio.h>\nint main() {");

            for (int i = 0; i < GeneratedCode.Length; i++)
                File.AppendAllText("genCode.c", "\n" + GeneratedCode[i]);

            File.AppendAllText("genCode.c", "\nreturn 0;\n}");

            Console.WriteLine("Kompiluję...");
            Process.Start("gcc","genCode.c");
        }
    }
}
