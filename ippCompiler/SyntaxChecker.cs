/*  This file is part of Iterkocze ippCompiler and it's under BSD-3-Clause License.
    Copyright (c) 2021, Iterkocze-Company
    All rights reserved.
    https://github.com/Iterkocze-Company/ippCompiler
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ippCompiler
{
    static class SyntaxChecker
    {
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
        }
    }
}
