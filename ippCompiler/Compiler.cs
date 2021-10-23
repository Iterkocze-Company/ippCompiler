/*  This file is part of Iterkocze ippCompiler and it's under BSD-3-Clause License.
    Copyright (c) 2021, Iterkocze-Company
    All rights reserved.
    https://github.com/Iterkocze-Company/ippCompiler
*/
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace ippCompiler
{
    public static class Compiler
    {
        public static List<string> VARS = new(); //Zawiera nazwy wszystkich zadeklarowancyh zmiennych i funkcji.

        static string lastVar = "";

        static bool IS_DOWHILE = false;

        static string DoWhileCondition = "";

        private static int errors = 0;

        public static string[] lines = ReadFileContents(Program.CODE_FILE_PATH);

        public static string[] GeneratedCode = new string[Program.FILE_LEN + 8];

        public static List<string> includes = new();

        public static string[] ReadFileContents(string pathToFile)
        {
            return File.ReadAllText(pathToFile).Replace("\r\n", "").Split(";");
        }

        public static void Compile()
        {
            int index = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                bool skip = false;
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
                                if (ch == '~')
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
                            skip = true;
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
                            skip = true;
                            break;

                        case "int":
                        case "float":
                        case "double":
                        case "char":
                        case "string":
                            VariableGenerator.Analyse(line, index);
                            index++;
                            skip = true;
                            break;

                        case "end":
                            GeneratedCode[index] = "}";
                            if (IS_DOWHILE)
                            {
                                GeneratedCode[index] += "while(" + DoWhileCondition + ");";
                                IS_DOWHILE = false;
                            }
                            index++;
                            skip = true;
                            break;

                        case "ReadKey":
                            GeneratedCode[index] = "getch();";
                            index++;
                            skip = true;
                            break;

                        case "if":
                            GeneratedCode[index] = lines[index].Replace("if", "if (").Replace(":--", "==").Replace("!:--", "!=").Replace("<:-", "<=").Replace(">:-", ">=");
                            GeneratedCode[index] += ")";
                            GeneratedCode[index] += "{";
                            index++;
                            skip = true;
                            break;

                        case "while":
                            GeneratedCode[index] = lines[index].Replace("while", "while (").Replace(":--", "==").Replace("!:--", "!=").Replace("<:-", "<=").Replace(">:-", ">=");
                            GeneratedCode[index] += ")";
                            GeneratedCode[index] += "{";
                            index++;
                            skip = true;
                            break;

                        case "DoWhile":
                            DoWhileCondition = line.Replace("DoWhile", "").Trim();
                            GeneratedCode[index] = line.Replace("DoWhile", "do{").Replace(DoWhileCondition, "");
                            IS_DOWHILE = true;
                            index++;
                            skip = true;
                            break;

                        case "else":
                            GeneratedCode[index] += "else{";
                            index++;
                            skip = true;
                            break;

                        case "for":
                            lines[index] = lines[index].Replace("for", "for (");
                            GeneratedCode[index] += lines[index] + ";";
                            GeneratedCode[index] += lines[index + 1] + ";";
                            GeneratedCode[index] += lines[index + 2] + "){";
                            index += 3;
                            skip = true;
                            break;

                        case "use":
                            string includeName = line.Replace("use", "").Replace("\"", "").Trim();
                            includes.Add(includeName);
                            index++;
                            skip = true;
                            break;

                        case "return":
                            GeneratedCode[index] = line + ";";
                            index++;
                            skip = true;
                            break;

                        case "File":
                            string fileName = line.Substring(line.IndexOf("File") + 5).Replace(" ", "").Replace("\t", "");
                            VARS.Add(fileName);
                            GeneratedCode[index] = "fstream " + fileName + ";";
                            index++;
                            skip = true;
                            break;

                        default: //Wykrywanie błędów było błędem. I tak pokazywało tylko proste literówki, a powodowało więcej problemów, niż to warte.
                            /*bool quit = false;
                            if (line.Contains("def")) break;
                            if (line.Contains("Macro")) break;
                            
                            foreach (string var in VARS)
                            {
                                if (line.Trim() == var && var != "")
                                    quit = true;

                                if (line.EndsWith(")") && line.Contains(var)) //Do rozpoznawania funkcji z innego pliku
                                {
                                    GeneratedCode[index] = line + ";";
                                    index++;
                                    break;
                                }
                            }
                            if (quit) break;

                            Log.Error("Błąd składni:");
                            Console.WriteLine(" " + line.Trim());
                            errors++;*/
                            break;
                    }
                }

                if (skip != true)
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
                    FunctionsGenerator.Analyse(line, index);
                    index++;
                }
            }

            if (Program.FLAG_SELF_INVOKE != true)
                File.WriteAllText("genCode.cpp", "");

            if (Program.FLAG_SELF_INVOKE)
            {
                File.AppendAllText("gen" + Program.CODE_FILE_PATH.Replace(".ipp", ".cpp"), "\n" + "#include <iostream>\n\nusing namespace std;\n\n");

                for (int i = 0; i < GeneratedCode.Length; i++)
                    File.AppendAllText("gen" + Program.CODE_FILE_PATH.Replace(".ipp", ".cpp"), "\n" + GeneratedCode[i]);
                Environment.Exit(0);
            }

            foreach (string includeName in includes)
            {
                string codeFilename = "gen" + includeName.Replace(".ipp", "") + ".cpp";
                File.WriteAllText(codeFilename, "");
                File.AppendAllText("genCode.cpp", "#include \"" + codeFilename + "\"\n");
                Process p = Process.Start("ippCompiler.exe", $"{includeName} -SelfInvoke");
                p.WaitForExit();
            }

            if (Program.FLAG_IS_LINUX)
            {
                File.AppendAllText("genCode.cpp", "#include <curses.h>\n");
            }
            else
            {
                File.AppendAllText("genCode.cpp", "#include <conio.h>\n");
            }
            File.AppendAllText("genCode.cpp", "#include <iostream>\n#include <fstream>\n#include \"Macros.cpp\"\n\nusing namespace std;\n");

            for (int i = 0; i < GeneratedCode.Length; i++)
                File.AppendAllText("genCode.cpp", "\n" + GeneratedCode[i]);

            if (errors != 0 && Program.FLAG_FORCE_COMPILE != true)
            {
                Log.Debug("Program nie zostanie wykompilowany, ponieważ wykryo " + errors.ToString() + " błedów składni.");
                Console.ReadLine();
                Environment.Exit(1);
            }
            if (Program.FLAG_FORCE_COMPILE)
            {
                Log.Debug("Wykryto " + errors.ToString() + " błędów składni. Kompilacja wymuszona przez flagę.");
            }
            Console.WriteLine("Kompiluję...");
            string args = "";
            
            args += $"{"-o " + Program.FLAG_NAME}";
            try
            {
                Process gpp = Process.Start("g++", $"{args} -O2 -s genCode.cpp");
                gpp.WaitForExit();
            }
            catch
            {
                Log.Error("Nie udało się uruchomić G++. Czy jesteś pewien, że masz go w zmiennej PATH?");
            }
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
            
            Console.WriteLine("Gotowe");
            if (Program.FLAG_RUN)
            {
                try
                {
                    Console.WriteLine("Aby uruchomić program, naciśnij dowolny przycisk.");
                    Console.ReadLine();
                    Process.Start(startInfo);
                }
                catch
                {
                    Log.Error("Wystąpił błąd podczas uruchamiania pliku! Czy jesteś pewien, że wybrałeś odpowiednią platformę?");
                    Console.ReadLine();
                    Environment.Exit(1);
                }
            }
            else
                Console.ReadKey();
        }
    }
}
