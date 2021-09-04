using System;
using System.IO;
using System.Linq;

namespace ippCompiler
{
    public static class Program
    {
        public static string CODE_FILE_PATH = "";
        public static int FILE_LEN = 0;
        private static void SetFilePath(string path)
        {
            if (!File.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wystąpił błąd podczas wczytywania pliku kodu! Upewnij się, że podełeś jego prawidłową lokalizację.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                CODE_FILE_PATH = path;
                FILE_LEN = File.ReadLines(path).Count();
            }
                
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Witaj w kompilatorze dla języka i++");
                Console.Write("Wprowadź ścieżkę do pliku języka i++ (relatywną): ");
                SetFilePath(Console.ReadLine());
                Compiler.Compile();
            }
        }
    }
}
