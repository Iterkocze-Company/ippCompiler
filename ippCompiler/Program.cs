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
        public static bool FLAG_INTERPRET;
        public static bool FLAG_NO_GENCODE;
        public static bool FLAG_NO_OUTPUT;

        private static bool SetFilePath(string path)
        {
            if (!File.Exists(path))
            {
                Log.Error("Error while reading code file! Is the path correct?");
                Console.ReadLine();
                return false;
            }
            else
            {
                CODE_FILE_PATH = Path.GetFullPath(path);
                FILE_LEN = File.ReadLines(CODE_FILE_PATH).Count();
                return true;
            }
        }

        private static void CheckMacrosDownloaded()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Macros.cpp"))
            {
                Log.Debug("It seems that you don't have additional files downloaded. Download them now? [Y/N]");
                if (Console.ReadLine().Trim().ToLower() == "y")
                {
                    PackageManager.DownloadMacros();
                }
            }
        }

        private static void HandleCompilerFlags(bool isCommandLine, string[] flags)
        {
            if (isCommandLine == false)
            {
                Console.Write("Enter compiler flags: ");
                string flagsStringBig = Console.ReadLine();
                flags = flagsStringBig.Split(",");
            }
            
            foreach (string flag in flags)
            {
                if (flag.Trim().Contains(".ipp")) CODE_FILE_PATH = Path.GetFullPath(flag);
                if (flag.Replace("-", "").Trim().ToLower() == "run") FLAG_RUN = true;
                if (flag.Replace("-", "").Trim().ToLower() == "sim") FLAG_INTERPRET = true;
                if (flag.Replace("-", "").Trim() == "SelfInvoke") FLAG_SELF_INVOKE = true;
                if (flag.Replace("-", "").Trim().ToLower() == "linux") FLAG_IS_LINUX = true;
                if (flag.Replace("-", "").Trim().ToLower() == "force") FLAG_FORCE_COMPILE = true;
                if (flag.Replace("-", "").Trim().ToLower() == "nogencode") FLAG_NO_GENCODE = true;
                if (flag.Replace("-", "").Trim().ToLower() == "noout") FLAG_NO_OUTPUT = true;
                if (flag.Replace("-", "").Trim().ToLower() == "macros") PackageManager.DownloadMacros();
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
                if (flag.Replace("-", "").Trim().ToLower() == "help")
                {
                    Console.WriteLine("'name=example' Change compiled .exe filename to 'example'.\n" +
                    "'run' Run program after compilation.\n" +
                    "'linux' Use this flag, if you are using ippCompiler in Linux.\n" +
                    "'force' Use this flag to force compile program regardless of errors detected by ippCompiler.\n" +
                    "'macros' Downloads additional files needed by ippCompiler.\n" +
                    "'nogencode' Compiler outputs only runable file whitout generated .cpp file.\n" +
                    "'noout' Produces no output. Used in compiling examples.\n");
                    Environment.Exit(0);
                }
            }
        }

        private static void InterpretOrCompile()
        {
            if (FLAG_INTERPRET)
                Interpreter.Interpret();
            else
                Compiler.Compile();
        }

        public static void Main(string[] args)
        {
            CheckMacrosDownloaded();
            if (args.Length == 0)
            {
                Console.WriteLine("Welcome to i++ language compiler.");
                Console.Write("Enter the path of .ipp file: ");
                bool opt = SetFilePath(Console.ReadLine());
                if (opt) HandleCompilerFlags(false, args);
                if (opt) InterpretOrCompile();
            }
            else
            {
                HandleCompilerFlags(true, args);
                InterpretOrCompile();
            }
        }
    }
}
