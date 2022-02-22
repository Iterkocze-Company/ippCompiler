/*  This file is part of Iterkocze ippCompiler and it's under BSD-3-Clause License.
    Copyright (c) 2021, Iterkocze-Company
    All rights reserved.
    https://github.com/Iterkocze-Company/ippCompiler
*/
using System.Linq;

namespace ippCompiler
{
    static class SyntaxChecker
    {
        private static int forIndex = 0;
        private static readonly string[] Keywords = new string[] { "Echo", "EchoLine", "def", "int", "float", "double", "char", "string", "end", "ReadKey", "if", "while", "DoWhile", "else", "for", "use", "return", "File", "Write", "CreateFile", "Wait", "main()" };

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

            if (line.StartsWith("for"))
            {
                forIndex = 2;
            }

            if (line.StartsWith(" "))
            {
                if (forIndex > 0)
                {
                    forIndex--; 
                    return;
                }
                Log.Error("Detected blank space in code. Use only tabs!\n");
                Compiler.errors++;
            }

            if (StringsChars % 2 != 0)
            {
                Log.Error($"Detected string with no end: {line}\n");
                Compiler.errors++;
                StringsChars = 0;
            }
            
        }
    }
}
