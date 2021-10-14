/*  This file is part of Iterkocze ippCompiler and it's under BSD-3-Clause License.
    Copyright (c) 2021, Iterkocze-Company
    All rights reserved.
    https://github.com/Iterkocze-Company/ippCompiler
*/
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace ippCompiler
{
    public static class Compiler
    {
        public static List<string> VARS = new List<string>(); //Zawiera nazwy wszystkich zadeklarowancyh zmiennych i funkcji.

        static string lastVar = "";

        static bool IS_DOWHILE = false;

        static string DoWhileCondition = "";

        static int errors = 0;

        public static string[] lines = ReadFileContents(Program.CODE_FILE_PATH);

        public static string[] GeneratedCode = new string[Program.FILE_LEN + 4];

        public static string[] ReadFileContents(string pathToFile)
        {
            return File.ReadAllText(pathToFile).Replace("\r\n", "").Split(";");
        }

        public static void Compile()
        {
            
            int index = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Replace("\n", "").Replace("\t", "");
                
                if (line != lines[lines.Length - 1])
                {
                    string[] parts = line.Split(" ");
                    int a = 0;

                    while (parts[a] == "") a = a + 1;

                    string afterFirst = line;
                    string toFirst = "";
                    for (int j = 0; j <= a; j = j + 1)
                    {
                        if (parts[j] == "") toFirst = toFirst + " ";
                        else toFirst = toFirst + parts[j];
                    }
                    afterFirst = afterFirst.Replace(toFirst, "");

                    switch (parts[a].Replace("\t", ""))
                    {
                        case "EchoLine":
                            GeneratedCode[index] = "cout << ";
                            
                            foreach (char ch in afterFirst)
                            {
                                if (ch == '+')
                                    GeneratedCode[index] = GeneratedCode[index] + " << ";
                                else
                                {
                                    if (ch == ' ')
                                    {
                                        GeneratedCode[index] = GeneratedCode[index] + " ";
                                    }
                                    else
                                    {
                                        GeneratedCode[index] = GeneratedCode[index] + ch;
                                    }
                                }
                            }
                            GeneratedCode[index] += " << endl;";
                            index++;
                            break;

                        case "Echo":
                            GeneratedCode[index] = "cout << ";
                            
                            foreach (char ch in afterFirst) 
                            {
                                if (ch == '+')
                                    GeneratedCode[index] = GeneratedCode[index] + " << ";
                                else
                                {
                                    if (ch == ' ')
                                    {
                                        GeneratedCode[index] = GeneratedCode[index] + " ";
                                    }
                                    else
                                    {
                                        GeneratedCode[index] = GeneratedCode[index] + ch;
                                    }
                                }
                            }
                            GeneratedCode[index] += " ;";
                            index++;
                            break;

                        case "int":
                        case "float":
                        case "double":
                        case "char":
                        case "string":
                            VariableGenerator.Analyse(line, index);
                            index++;
                            break;

                        case "end":
                            GeneratedCode[index] = "}";
                            if (IS_DOWHILE)
                            {
                                GeneratedCode[index] += "while(" + DoWhileCondition + ");";
                                IS_DOWHILE = false;
                            }
                            index++;
                            break;

                        case "ReadKey":
                            GeneratedCode[index] = "getch();";
                            index++;
                            break;

                        case "if":
                            GeneratedCode[index] = lines[index].Replace("if", "if (").Replace(":--", "==").Replace("!:--", "!=").Replace("<:-", "<=").Replace(">:-", ">=");
                            GeneratedCode[index] += ")";
                            GeneratedCode[index] += "{";
                            index++;
                            break;

                        case "while":
                            GeneratedCode[index] = lines[index].Replace("while", "while (").Replace(":--", "==").Replace("!:--", "!=").Replace("<:-", "<=").Replace(">:-", ">=");
                            GeneratedCode[index] += ")";
                            GeneratedCode[index] += "{";
                            index++;
                            break;

                        case "DoWhile":
                            DoWhileCondition = line.Replace("DoWhile", "").Trim();
                            GeneratedCode[index] = line.Replace("DoWhile", "do{").Replace(DoWhileCondition, "");
                            //GeneratedCode[index] += "while(" + DoWhileStr + ")";
                            IS_DOWHILE = true;
                            index++;
                            break;

                        case "else":
                            GeneratedCode[index] += "else{";
                            index++;
                            break;

                        case "for":
                            lines[index] = lines[index].Replace("for", "for (");
                            GeneratedCode[index] += lines[index] + ";";
                            GeneratedCode[index] += lines[index + 1] + ";";
                            GeneratedCode[index] += lines[index + 2] + "){";
                            index += 3;
                            break;

                        case "return":
                            GeneratedCode[index] = line + ";";
                            index++;
                            break;

                        case "File":
                            string fileName = line.Substring(line.IndexOf("File") + 5).Replace(" ", "").Replace("\t", "");
                            VARS.Add(fileName);
                            GeneratedCode[index] = "fstream " + fileName + ";";
                            index++;
                            break;

                        case "MacroTest()":
                            Console.WriteLine("macro!");
                            GeneratedCode[index] = "MacroTest();";
                            index++;
                            break;

                        default:
                            bool quit = false;
                            if (line.Contains("def")) break;
                            if (line.Contains("Macro")) break;
                            foreach (string var in VARS)
                            {
                                if (line.Contains(var) && var != "")
                                    quit = true;
                            }
                            if (quit) break;

                            Program.Error("Błąd składni:");
                            Console.WriteLine(" " + line.Trim());
                            errors++;
                            break;
                    }
                }

                foreach (string var in VARS)
                {
                    string numericArgs = "";
                    
                    if (var != "")
                    if (line.Contains(var) && line.Replace(" ", "").Replace("\t", "").StartsWith(var[0]))
                    {
                        try
                        {
                            numericArgs = line.Remove(0, line.IndexOf("(")).Replace("(", "").Replace(")", "");
                        }
                        catch
                        {
                                
                        }
                        
                        if (line.EndsWith("ReadKey"))
                        {
                            GeneratedCode[index] = var + " = getch();";
                            index++;
                            break;
                        }

                        if (line.EndsWith("ReadString"))
                        {
                            GeneratedCode[index] = "cin >> " + var + ";";
                            index++;
                            break;
                        }

                        if (line.EndsWith("++"))
                        {
                            GeneratedCode[index] = var + "++;";
                            index++;
                            break;
                        }

                        if (line.Contains('*'))
                        {
                            string valMul = line.Substring(line.IndexOf('*')+1);
                            GeneratedCode[index] = var + "=" + var + "*" + valMul + ";";
                            index++;
                            break;
                        }

                        if (line.Contains('/'))
                        {
                            string valMul = line.Substring(line.IndexOf('/') + 1);
                            GeneratedCode[index] = var + "=" + var + "/" + valMul + ";";
                            index++;
                            break;
                        }

                        if (line.EndsWith("()")) //Jeśli wykryjemy, że to nazwa funkcji.
                        {
                            GeneratedCode[index] = var + "();";
                            index++;
                            break;
                        }

                        if (line.EndsWith(")") && var != lastVar)
                        {
                            GeneratedCode[index] = var + "(" + numericArgs + ");";
                            index++;
                            lastVar = var;
                            break;
                        }

                        if (line.Contains(".Open"))
                        {
                            string fileName = line.Substring(line.IndexOf(".Open")+5).Replace(" ", "").Replace("\t", "");
                            string fileObjName = line.Substring(0, line.IndexOf( ".Open")).Replace(" ", "").Replace("\t", "");
                            GeneratedCode[index] += fileObjName + ".open(" + fileName + ");";
                            index++;
                            break;
                        }

                        if (line.Contains(".Write"))
                        {
                            string textToWrite = line.Substring(line.IndexOf(".Write") + 6).Replace(" ", "").Replace("\t", "");
                            string fileObjName = line.Substring(0, line.IndexOf(".Write")).Replace(" ", "").Replace("\t", "");
                            GeneratedCode[index] += fileObjName + " << " + textToWrite + ";";
                            
                            index++;
                            break;
                        }

                        if (line.Contains(".Close"))
                        {
                            string fileObjName = line.Substring(0, line.IndexOf(".Close")).Replace(" ", "").Replace("\t", "");
                            GeneratedCode[index] += fileObjName + ".close();";
                            index++;
                            break;
                        }

                        if (line.Contains(".ReadByLine"))
                        {
                            bool isEcho = false;
                            string stringName = "";
                            string fileObjName = "";
                            if (line.Contains(".ReadByLineEcho")) isEcho = true;
                            if (isEcho)
                            {
                                stringName = line.Substring(line.IndexOf(".ReadByLine") + 15).Replace(" ", "").Replace("\t", "");
                                fileObjName = line.Substring(0, line.IndexOf(".ReadByLine")).Replace(" ", "").Replace("\t", "");
                                GeneratedCode[index] += "while (getline(" + fileObjName + "," + stringName + ")){\n" + "cout << " + stringName + ";\n}";
                            }
                                    
                            else
                            {
                                stringName = line.Substring(line.IndexOf(".ReadByLine") + 11).Replace(" ", "").Replace("\t", "");
                                fileObjName = line.Substring(0, line.IndexOf(".ReadByLine")).Replace(" ", "").Replace("\t", "");
                                GeneratedCode[index] += "while (getline(" + fileObjName + "," + stringName + ")){\n}";
                            }
                            index++;
                            break;
                        }

                        if (!line.Contains("int") && !line.Contains("string") && !line.Contains("char") && !line.Contains("float") && !line.Contains("double") && var != lastVar)
                        {
                            string val = line.Substring(line.IndexOf('=') + 1);
                            val = val.Replace(" ", "");
                            GeneratedCode[index] = var + "=" + (string)val + ";";
                            index++;
                        }
                    }
                }

                if (line.ToLower().Contains("def")) //Obsługa funkcji.
                {
                    string lastFuncName = "";
                    string[] splitted = line.Split();
                    string funcName = "";
                    if (!splitted[2].Contains("main"))
                        funcName = splitted[2].Substring(0, splitted[2].IndexOf("("));
                    if (splitted[2].Contains("main"))
                    {
                        GeneratedCode[index] += "int main(){";
                        index++;
                    }
                    VARS.Add(funcName);

                    foreach (string lineDef in splitted)
                    {
                        if ((lineDef.Contains("int") || lineDef.Contains("float")) && funcName != lastFuncName)
                        {
                            lastFuncName = funcName;

                            string listOfArguments = line.Substring(line.IndexOf('(')).Replace("(", "").Replace(")", "");
                            string[] listOfArgumentsFinal = new string[8];
                            int argumentsIntIndex = 0;
                            if (line.Remove(line.IndexOf("(")).Contains("int"))
                                GeneratedCode[index] += "int ";
                            if (line.Remove(line.IndexOf("(")).Contains("float"))
                                GeneratedCode[index] += "float ";
                            if (line.Remove(line.IndexOf("(")).Contains("double"))
                                GeneratedCode[index] += "double ";
                            if (line.Remove(line.IndexOf("(")).Contains("char"))
                                GeneratedCode[index] += "char ";
                            if (line.Remove(line.IndexOf("(")).Contains("string"))
                                GeneratedCode[index] += "string ";
                            if (line.Remove(line.IndexOf("(")).Contains("bool"))
                                GeneratedCode[index] += "bool ";
                            GeneratedCode[index] += funcName + "(";


                            string[] argNames = listOfArguments.Replace("float", "").Split(',');
                            GeneratedCode[index] += listOfArguments;
                            if (argumentsIntIndex > 1)
                                GeneratedCode[index] += ", ";
                            foreach (string var in argNames)
                            {
                                VARS.Add(var.Trim());
                            }

                            if (argumentsIntIndex > 1)
                                GeneratedCode[index] = GeneratedCode[index].Remove(GeneratedCode[index].Length - 2);

                            GeneratedCode[index] += ")";
                            GeneratedCode[index] += "{";
                            index++;
                            break;
                        }

                        if (lineDef.Contains("void"))
                        {
                            GeneratedCode[index] += "void ";
                            GeneratedCode[index] += funcName + "()";
                            GeneratedCode[index] += "{";
                            index++;
                        }
                    }
                }
            }

            File.WriteAllText("genCode.cpp", "");
            File.AppendAllText("genCode.cpp", "#include <iostream>\n#include <conio.h>\n#include <fstream>\n#include \"Macros.cpp\"\n\nusing namespace std;\n");

            for (int i = 0; i < GeneratedCode.Length; i++)
                File.AppendAllText("genCode.cpp", "\n" + GeneratedCode[i]);

            if (errors != 0 && Program.FLAG_FORCE_COMPILE != true)
            {
                Program.Debug("Program nie zostanie wykompilowany, ponieważ wykryo " + errors.ToString() + " błedów składni.");
                Console.ReadLine();
                Environment.Exit(1);
            }
            if (Program.FLAG_FORCE_COMPILE)
            {
                Program.Debug("Wykryto " + errors.ToString() + " błędów składni. Kompilacja wymuszona przez flagę.");
            }
            Console.WriteLine("Kompiluję...");
            string args = "";
            
            args = args + $"{"-o " + Program.FLAG_NAME}";
            Process.Start("g++", $"{args} -O2 -s genCode.cpp");
            var startInfo = new ProcessStartInfo();
            if (Program.FLAG_IS_LINUX)
            {
                startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                startInfo.FileName = Program.FLAG_NAME + ".out";
            }
            else
            {
                startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                startInfo.FileName = Program.FLAG_NAME + ".exe";
            }
            
            Process.Start("g++", "-O2 -s genCode.cpp");
            
            Console.WriteLine("Gotowe");
            if (Program.FLAG_RUN)
            {
                Thread.Sleep(2000);
                try
                {
                    Process.Start(startInfo);
                }
                catch
                {
                    Program.Error("Wystąpił błąd podczas uruchamiania pliku! Czy jesteś pewien, że wybrałeś odpowiednią platformę?");
                    Console.ReadLine();
                    Environment.Exit(1);
                }
            }
            else
                Console.ReadKey();
        }
    }
}
