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
    public static class FunctionsGenerator
    {
        public static void Analyse(string line, int index)
        {
            string lastFuncName = "";
            string[] splitted = line.Split();
            string funcName = "";
            if (!splitted[2].Contains("main"))  
                funcName = splitted[2].Substring(0, splitted[2].IndexOf("("));
            if (splitted[2].Contains("main"))
            {
                Compiler.GeneratedCode[index] += "int main(){";
                index++;
            }
            if (funcName != string.Empty) Compiler.VARS.Add(funcName);

            foreach (string lineDef in splitted)
            {
                if (funcName != lastFuncName)
                {
                    lastFuncName = funcName;

                    string listOfArguments = line.Substring(line.IndexOf('(')).Replace("(", "").Replace(")", "");
                    string[] listOfArgumentsFinal = new string[8];
                    int argumentsIntIndex = 0;
                    if (line.Remove(line.IndexOf("(")).Contains("int"))
                        Compiler.GeneratedCode[index] += "int ";
                    if (line.Remove(line.IndexOf("(")).Contains("float"))
                        Compiler.GeneratedCode[index] += "float ";
                    if (line.Remove(line.IndexOf("(")).Contains("double"))
                        Compiler.GeneratedCode[index] += "double ";
                    if (line.Remove(line.IndexOf("(")).Contains("char"))
                        Compiler.GeneratedCode[index] += "char ";
                    if (line.Remove(line.IndexOf("(")).Contains("string"))
                        Compiler.GeneratedCode[index] += "string ";
                    if (line.Remove(line.IndexOf("(")).Contains("bool"))
                        Compiler.GeneratedCode[index] += "bool ";
                    if (line.Remove(line.IndexOf("(")).Contains("void"))
                        Compiler.GeneratedCode[index] += "void ";
                    Compiler.GeneratedCode[index] += funcName + "(";


                    string[] argNames = listOfArguments.Replace("float", "").Split(',');
                    Compiler.GeneratedCode[index] += listOfArguments;
                    if (argumentsIntIndex > 1)
                        Compiler.GeneratedCode[index] += ", ";
                    foreach (string var in argNames)
                    {
                        Compiler.VARS.Add(var.Replace("string", "").Replace("int", "").Replace("float", "").Replace("char", "").Replace("double", "").Trim());
                    }

                    if (argumentsIntIndex > 1)
                        Compiler.GeneratedCode[index] = Compiler.GeneratedCode[index].Remove(Compiler.GeneratedCode[index].Length - 2);

                    Compiler.GeneratedCode[index] += ")";
                    Compiler.GeneratedCode[index] += "{";
                    index++;
                    break;
                }
            }
        }
    }
}
