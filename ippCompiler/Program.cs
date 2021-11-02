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
        public static string INCLUDES_PATH = "";
        public static int FILE_LEN = 0;
        public static bool FLAG_RUN;
        public static string FLAG_NAME = "";
        public static bool FLAG_IS_LINUX;
        public static bool FLAG_FORCE_COMPILE;
        public static bool FLAG_SELF_INVOKE;

        private static bool SetFilePath(string path)
        {
            if (!File.Exists(path))
            {
                Log.Error("Wystąpił błąd podczas wczytywania pliku kodu! Upewnij się, że podełeś jego prawidłową lokalizację.");
                Console.ReadLine();
                return false;
            }
            else
            {
                CODE_FILE_PATH = path;
                FILE_LEN = File.ReadLines(path).Count();
                return true;
            }
                
        }

        private static void CheckMacrosDownloaded()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Macros.cpp"))
            {
                Log.Debug("Wygląda na to, że nie masz pobranych dodatkowych plików wymaganych przez ippCompiler. Czy chcesz je pobrać teraz? [T/N]");
                if (Console.ReadLine().Trim().ToLower() == "t")
                {
                    PackageManager.DownloadMacros();
                }
            }
        }

        private static void HandleCompilerFlags(bool isCommandLine, string[] flags)
        {
            if (isCommandLine == false)
            {
                Console.Write("Podaj flagi kompilatora: ");
                string flagsStringBig = Console.ReadLine();
                flags = flagsStringBig.Split(",");
            }
            
            foreach (string flag in flags)
            {
                if (flag.Trim().Contains(".ipp")) CODE_FILE_PATH = flag;
                if (flag.Replace("-", "").Trim() == "run") FLAG_RUN = true;
                if (flag.Replace("-", "").Trim() == "SelfInvoke") FLAG_SELF_INVOKE = true;
                if (flag.Replace("-", "").Trim() == "linux") FLAG_IS_LINUX = true;
                if (flag.Replace("-", "").Trim() == "force") FLAG_FORCE_COMPILE = true;
                if (flag.Replace("-", "").Trim() == "macros") PackageManager.DownloadMacros();
                if (flag.Contains("name"))
                {
                    FLAG_NAME = "";
                    string GoodFlag = flag.Trim();
                    int i = 0;
                    foreach (char c in GoodFlag)
                    {
                        i++;
                        if (c == ' ')
                            break;
                        else if (i >= 6)
                            FLAG_NAME += c;
                    }
                }
                if (!flag.Contains("name") && FLAG_NAME == "")
                    FLAG_NAME = "a";
            }
        }

        public static void Main(string[] args)
        {
            CheckMacrosDownloaded();
            if (args.Length == 0)
            {
                Console.WriteLine("Witaj w kompilatorze języka i++");
                Console.Write("Wprowadź ścieżkę do pliku języka i++ (relatywną): ");
                bool opt = SetFilePath(Console.ReadLine());
                if (opt) HandleCompilerFlags(false, args);
                if (opt) Compiler.Compile();
            }
            else
            {
                HandleCompilerFlags(true, args);
                Compiler.Compile();
            }
        }
    }
}
