/*  This file is part of Iterkocze ippCompiler and it's under BSD-3-Clause License.
    Copyright (c) 2021, Iterkocze-Company
    All rights reserved.
    https://github.com/Iterkocze-Company/ippCompiler
*/
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace ippCompiler
{
    /* Kody zakończenia programu
          1 - Wykryto błędy w kodzie .ipp, a flaga nie wymusiła kompilacji.
          2 - Nie udało się uruchomić G++.
          3 - Nie udało się uruchomić wykompilowanego pliku.
    */
    public static class Compiler
    {
        public static List<string> VARS = new(); //Zawiera nazwy wszystkich zadeklarowancyh zmiennych i funkcji.

        static string lastVar = string.Empty;

        public static string CODE_FILE_GEN_PATH = string.Empty;

        static bool IS_DOWHILE = false;

        static string DoWhileCondition = string.Empty;

        public static int errors = 0;

        public static string[] lines = ReadFileContents(Program.CODE_FILE_PATH);

        public static string[] GeneratedCode = new string[Program.FILE_LEN + 12]; //Change this to List.

        public static List<string> includes = new();

        public static List<string> GENERATED_FILES = new();

        public static string GenerateRandomAlphanumericString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }

        public static string[] ReadFileContents(string pathToFile)
        {
            //return Regex.Split(File.ReadAllText(pathToFile), @"(?<=[;])");
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

                    string afterFirst = line;
                    string toFirst = string.Empty;
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
                            GeneratedCode[index] += " ;";
                            index++;
                            skip = true;
                            break;

                        case "int":
                        case "float":
                        case "double":
                        case "char":
                        case "string":
                        case "int*":
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

                        case "CreateFile":
                            string fileName2 = line.Substring(line.IndexOf("File") + 5).Replace(" ", "").Replace("\t", "");
                            string fileName2Formatted = fileName2.Replace("\"", "");
                            string id = GenerateRandomAlphanumericString();
                            GeneratedCode[index] = "ofstream " + id + ";";
                            index++;
                            GeneratedCode[index] = $"{id}.open(\"{fileName2Formatted + ".txt"}\");";
                            index++;
                            GeneratedCode[index] = $"{id}.close();";
                            index++;
                            GeneratedCode[index] = "Sleep(100);";
                            index++;
                            break;

                        case "Wait":
                            string howLong = line.Replace("Wait", "").Trim();
                            GeneratedCode[index] = $"Sleep({howLong});";
                            index++;
                            break;

                        case "const":
                            GeneratedCode[index] += line + ";";
                            index++;
                            break;

                        case "static":
                            GeneratedCode[index] += line + ";";
                            index++;
                            break;

                        default:
                            if (line.EndsWith(")") && !line.Contains("def"))
                            {
                                GeneratedCode[index] = line + ";";
                                skip = true;
                                index++;
                            }
                            break;
                    }
                    SyntaxChecker.Analyse(line);
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
                            GeneratedCode[index] += fileObjName + ".open(" + fileName + ", std::fstream::in | std::fstream::out);";
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
                                GeneratedCode[index] += "while (getline(" + fileObjName + "," + stringName + ")){\n" + "cout << " + stringName + ";\n";
                            }
                                    
                            else
                            {
                                stringName = line.Substring(line.IndexOf(".ReadByLine") + 11).Replace(" ", "").Replace("\t", "");
                                fileObjName = line.Substring(0, line.IndexOf(".ReadByLine")).Replace(" ", "").Replace("\t", "");
                                GeneratedCode[index] += "while (getline(" + fileObjName + "," + stringName + ")){\n";
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

            int index2 = 0;
            foreach (string line in GeneratedCode) //I just don't know what I'm doing with this anymore. I know, I just gave up on this compiler at this point.
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
                    GeneratedCode[index2] = "";
                    foreach (string line2 in lineSplit)
                    {
                        GeneratedCode[index2] += line2;
                    }
                }
                index2++;
            }

            CODE_FILE_GEN_PATH = "gen" + Program.CODE_FILE_PATH.Remove(0, Program.CODE_FILE_PATH.Replace("/", "\\").LastIndexOf('\\')+1).Replace(".ipp", ".cpp");

            if (Program.FLAG_SELF_INVOKE != true)
                File.WriteAllText(CODE_FILE_GEN_PATH, "");

            if (Program.FLAG_SELF_INVOKE)
            {
                File.AppendAllText(CODE_FILE_GEN_PATH, "\n" + "#include <iostream>\n\nusing namespace std;\n\n");

                for (int i = 0; i < GeneratedCode.Length; i++)
                    File.AppendAllText(CODE_FILE_GEN_PATH, "\n" + GeneratedCode[i]);
                Environment.Exit(0);
            }

            foreach (string includeName in includes)
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "ippCompiler.exe";
                info.Arguments = $"{includeName} -SelfInvoke";
                info.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;

                Process p = Process.Start(info);
                p.WaitForExit();
            }

            if (Program.FLAG_IS_LINUX)
            {
                File.AppendAllText(CODE_FILE_GEN_PATH, "#include <curses.h>\n");
                File.AppendAllText(CODE_FILE_GEN_PATH, "#include <unistd.h>\n");
                GENERATED_FILES.Add(CODE_FILE_GEN_PATH);
            }
            else
            {
                File.AppendAllText(CODE_FILE_GEN_PATH, "#include <conio.h>\n");
                File.AppendAllText(CODE_FILE_GEN_PATH, "#include <Windows.h>\n");
                GENERATED_FILES.Add(CODE_FILE_GEN_PATH);
            }
            string MacrosPath = AppDomain.CurrentDomain.BaseDirectory + "Macros.cpp";

            File.AppendAllText(CODE_FILE_GEN_PATH, $"#include <iostream>\n#include <fstream>\n#include \"{MacrosPath}\"\n\nusing namespace std;\n");

            for (int i = 0; i < GeneratedCode.Length; i++)
                File.AppendAllText(CODE_FILE_GEN_PATH, "\n" + GeneratedCode[i]);

            if (errors != 0 && Program.FLAG_FORCE_COMPILE != true)
            {
                Log.Debug("Program is not going to be compiled > Detected " + errors.ToString() + " syntax errors.");
                Console.ReadLine();
                Environment.Exit(1);
            }
            if (Program.FLAG_FORCE_COMPILE)
            {
                Log.Debug("Detected " + errors.ToString() + " syntax errors. But compilation is forced by flag.");
            }
            Console.WriteLine("Compilation...");
            string args = string.Empty;
            
            args += $"{"-o " + Program.FLAG_NAME}";
            try
            {
                Process gpp = Process.Start("g++", $"{args} -O2 -s {CODE_FILE_GEN_PATH}");
                gpp.WaitForExit();
            }
            catch
            {
                Log.Error("Can't run G++. Are you sure, that it is located in your PATH?");
                Environment.Exit(2);
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
            
            Console.WriteLine("Done");
            if (Program.FLAG_RUN)
            {
                try
                {
                    Console.WriteLine("Press any key to run your program.");
                    Console.ReadKey();
                    Console.Clear();
                    Process.Start(startInfo);
                }
                catch
                {
                    Log.Error("Error while opening your program! Are you compiling for the right platform?");
                    Console.ReadKey();
                    Environment.Exit(3);
                }
            }
                
            if (Program.FLAG_NO_GENCODE)
            {
                foreach (string name in GENERATED_FILES)
                {
                    File.Delete(name);
                }
            }
            if (Program.FLAG_NO_OUTPUT)
            {
                if (Program.FLAG_IS_LINUX)
                    File.Delete(Program.FLAG_NAME + ".out");
                else
                    File.Delete(Program.FLAG_NAME + ".exe");
            }
        }
    }
}
