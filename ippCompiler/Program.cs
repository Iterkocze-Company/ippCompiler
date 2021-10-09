/*  This file is part of Iterkocze ippCompiler and it's under BSD-3-Clause License.
    Copyright (c) 2021, Iterkocze-Company
    All rights reserved.
    https://github.com/Iterkocze-Company/ippCompiler
*/
using System;
using System.IO;
using System.Linq;

namespace ippCompiler
{
    public static class Program
    {
        public static string CODE_FILE_PATH = "";
        public static int FILE_LEN = 0;
        public static bool FLAG_RUN;
        public static string FLAG_NAME = "";
        public static bool FLAG_IS_LINUX;

        public static void Error(string str)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(str);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Debug(string str)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(str);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static bool SetFilePath(string path)
        {
            if (!File.Exists(path))
            {
                Error("Wystąpił błąd podczas wczytywania pliku kodu! Upewnij się, że podełeś jego prawidłową lokalizację.");
                return false;
            }
            else
            {
                CODE_FILE_PATH = path;
                FILE_LEN = File.ReadLines(path).Count();
                return true;
            }
                
        }
        private static void HandleCompilerFlags()
        {
            Console.Write("Podaj flagi kompilatora: ");
            string flags = Console.ReadLine();
            if (flags.Contains("run")) FLAG_RUN = true;
            if (flags.Contains("name"))
            {
                int i = 0;
                foreach (char c in flags)
                {
                    i++;
                    if (c == ' ')
                        break;
                    else if (i >= 6)
                        FLAG_NAME += c;
                }
            }
            if (!flags.Contains("name"))
                FLAG_NAME = "a";
        }

        private static bool IsLinux()
        {
            Console.WriteLine("Wykompilować plik wykonywalny na platformę Linux? Jeśli tak, wpisz cokolwiek ");
            string ans = Console.ReadLine();
            if (ans.Length != 0)
                return true;
            else
                return false;
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Witaj w kompilatorze dla języka i++");
                Console.Write("Wprowadź ścieżkę do pliku języka i++ (relatywną): ");
                bool opt = SetFilePath(Console.ReadLine());
                if (opt) HandleCompilerFlags();
                if (opt) FLAG_IS_LINUX = IsLinux();
                if (opt) Compiler.Compile();
            }
        }
    }
}
