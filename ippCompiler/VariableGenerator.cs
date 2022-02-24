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
    public static class VariableGenerator
    {
        public static void Analyse(string line, int index)
        {
            //string afterFirst = line;
            if (line.Contains("int"))
            {
                bool hasEnd = false;
                Compiler.GeneratedCode[index] += "int ";
                if (line.EndsWith("ReadKey"))
                {
                    string nameRead = Compiler.lines[index].Substring(Compiler.lines[index].IndexOf(' ')).Replace(" ", "");
                    nameRead = nameRead.Substring(0, nameRead.IndexOf('='));
                    Compiler.GeneratedCode[index] = $"int {nameRead} =  getch();";
                    index++;
                    hasEnd = true;
                }
                string name = line.Substring(line.IndexOf(' ')).Replace(" ", "");
                if (hasEnd == false)
                    Compiler.GeneratedCode[index] = Compiler.GeneratedCode[index] + name + ";";

                if (name.Contains('='))
                    name = name.Substring(0, name.IndexOf('='));

                Compiler.VARS.Add(name);
            }
            if (line.Contains("string"))
            {
                Compiler.GeneratedCode[index] += "string ";

                string nameStr = line.Substring(line.IndexOf(' ')).Replace(" ", "");
                Compiler.GeneratedCode[index] = Compiler.GeneratedCode[index] + nameStr + ";";
                if (nameStr.Contains('='))
                    nameStr = nameStr.Substring(0, nameStr.IndexOf('='));

                Compiler.VARS.Add(nameStr);
            }
            if (line.Contains("char"))
            {
                Compiler.GeneratedCode[index] += "char ";

                string nameStr = line.Substring(line.IndexOf(' ')).Replace(" ", "");
                Compiler.GeneratedCode[index] = Compiler.GeneratedCode[index] + nameStr + ";";
                if (nameStr.Contains('='))
                    nameStr = nameStr.Substring(0, nameStr.IndexOf('='));

                Compiler.VARS.Add(nameStr);
            }
            if (line.Contains("float"))
            {
                Compiler.GeneratedCode[index] += "float ";

                string nameStr = line.Substring(line.IndexOf(' ')).Replace(" ", "");
                Compiler.GeneratedCode[index] = Compiler.GeneratedCode[index] + nameStr + ";";
                if (nameStr.Contains('='))
                    nameStr = nameStr.Substring(0, nameStr.IndexOf('='));

                Compiler.VARS.Add(nameStr);
            }
            if (line.Contains("double"))
            {
                Compiler.GeneratedCode[index] += "double ";

                string nameStr = line.Substring(line.IndexOf(' ')).Replace(" ", "");
                Compiler.GeneratedCode[index] = Compiler.GeneratedCode[index] + nameStr + ";";
                if (nameStr.Contains('='))
                    nameStr = nameStr.Substring(0, nameStr.IndexOf('='));

                Compiler.VARS.Add(nameStr);
            }
            if (line.Contains("bool"))
            {
                Compiler.GeneratedCode[index] += "bool ";

                string nameStr = line.Substring(line.IndexOf(' ')).Replace(" ", "");
                Compiler.GeneratedCode[index] = Compiler.GeneratedCode[index] + nameStr + ";";
                if (nameStr.Contains('='))
                    nameStr = nameStr.Substring(0, nameStr.IndexOf('='));

                Compiler.VARS.Add(nameStr);
            }


            if (line.Contains("int*"))
            {
                bool hasEnd = false;
                string pointerName = null;
                Compiler.GeneratedCode[index] = "int* ";
                string name = line.Substring(line.IndexOf(' ')).Replace(" ", "");
                if (hasEnd == false)
                {
                    if (line != null && line.Contains("Address"))
                    {
                        int index22 = 0;
                        string[] lineSplit = line.Split(" ");
                        foreach (string line2 in lineSplit)
                        {
                            if (line2.Contains("Address"))
                            {
                                string beforeEqual = null;
                                string line22 = line2.Replace(".Address", "");
                                try
                                {
                                    //beforeEqual = line22.Remove(line22.IndexOf("="));
                                    line22 = line22.Remove(0, line2.IndexOf("=") + 1);
                                }
                                catch { }
                                string line222 = " " + beforeEqual;
                                line222 += " &" + line22;
                                lineSplit[index22] = line222;
                            }
                            index22++;
                        }
                        foreach (string line2 in lineSplit)
                        {
                            if (line2.Contains('&'))
                                pointerName = line2;
                        }
                    }
                }
                if (name.Contains('='))
                    name = name.Substring(0, name.IndexOf('='));

                Compiler.GeneratedCode[index] = Compiler.GeneratedCode[index] + name + " = " + pointerName + ";";

                Compiler.VARS.Add(name);
            }
        }
    }
}
