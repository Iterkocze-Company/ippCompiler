/*  This file is part of Iterkocze ippCompiler and it's under BSD-3-Clause License.
    Copyright (c) 2021, Iterkocze-Company
    All rights reserved.
    https://github.com/Iterkocze-Company/ippCompiler
*/
namespace ippCompiler
{
    static class SyntaxChecker
    {
        private static readonly string[] Keywords = new string[] { "Echo", "EchoLine", "def", "int", "float", "double", "char", "string", "end", "ReadKey", "if", "while", "DoWhile", "else", "for", "use", "return", "File", "Write", "CreateFile", "Wait" };

        public static byte StringsChars = 0;
        public static void Analyse(string line)
        {
            foreach (char c in line)
            {
                if (c == '\"')
                {
                    StringsChars++;
                }
            }

            if (line.StartsWith(" "))
            {
                Log.Error("Detected blank space in code. Use only tabs!\n");
                Compiler.errors++;
            }

            if (StringsChars % 2 != 0)
            {
                Log.Error($"Detected string with no end: {line}\n");
                Compiler.errors++;
                StringsChars = 0;
            }

            
            // An attempt to make syntax checker.

            /*foreach (string codeKeyword in line.Split(' '))
            {
                if (!Keywords.Any(codeKeyword.Contains))
                {
                    Log.Error($"Unknown keyword: {codeKeyword}\n");
                    Compiler.errors++;
                }
            }
            foreach (string codeKeyword in line.Split('.'))
            {
                if (!Keywords.Any(line.Contains))
                {
                    Log.Error($"Unknown keyword: {codeKeyword}\n");
                    Compiler.errors++;
                }
            }*/
        }
    }
}
