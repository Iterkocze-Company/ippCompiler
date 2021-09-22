using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ippCompiler
{
    public static class Compiler
    {
        public static List<string> VARS = new List<string>(); //Zawiera nazwy wszystkich zadeklarowancyh zmiennych i funkcji.

        public static string lastVar = "";
        public static string[] ReadFileContents(string pathToFile)
        {
            return File.ReadAllText(pathToFile).Replace("\r\n", "").Split(";");
        }

        public static string deleteFirstArg(string str)
        {
            string opt = "";
            bool firstSpace = false;

            foreach(char ch in str)
            {
                if (firstSpace) opt = opt + ch;
                if (ch == ' ') firstSpace = true;
            }

            return opt;
        }

        public static void Compile()
        {
            string[] lines = ReadFileContents(Program.CODE_FILE_PATH);
            string[] GeneratedCode = new string[Program.FILE_LEN+4];
            int index = 0;

            for (int i = 0; i < lines.Length; i = i + 1)
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

                    //switch (line.Split(" ")[0])
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
                            GeneratedCode[index] = "int ";
                            if (line.EndsWith("readKey"))
                            {
                                string nameRead = lines[index].Substring(lines[index].IndexOf(' ')).Replace(" ", "");
                                nameRead = nameRead.Substring(0, nameRead.IndexOf('='));
                                GeneratedCode[index] = $"int {nameRead} =  getch();";
                                index++;
                                break;
                            }
                            string name = afterFirst.Substring(afterFirst.IndexOf(' ')).Replace(" ", "");

                            GeneratedCode[index] = GeneratedCode[index] + name + ";";

                            if (name.Contains('='))
                                name = name.Substring(0, name.IndexOf('='));

                            VARS.Add(name);

                            index++;
                            break;

                        case "string":
                            GeneratedCode[index] = "string ";

                            string nameStr = line.Substring(line.IndexOf(' ')).Replace(" ", "");
                            GeneratedCode[index] = GeneratedCode[index] + nameStr + ";";
                            if (nameStr.Contains('='))
                                nameStr = nameStr.Substring(0, nameStr.IndexOf('='));

                            VARS.Add(nameStr);

                            index++;
                            break;

                        case "end":
                            GeneratedCode[index] = "}";
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

                        case "FileR":
                        case "FileW":
                            bool isReadFile = false;
                            if (line.Contains("FileR")) isReadFile = true; 
                            string fileName = line.Substring(line.IndexOf("File")+5).Replace(" ", "").Replace("\t", "");
                            if (isReadFile)
                            {
                                GeneratedCode[index] = "ofstream " + fileName + ";";
                            }
                            else
                            {
                                GeneratedCode[index] = "ifstream " + fileName + ";";
                            }
                            VARS.Add(fileName);
                            index++;
                            break;

                        default:
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
                            GeneratedCode[index] += var + ".open(" + fileName + ");";
                            index++;
                            break;
                        }

                        if (line.Contains(".Write"))
                        {
                            string textToWrite = line.Substring(line.IndexOf(".Write") + 6).Replace(" ", "").Replace("\t", "");
                            GeneratedCode[index] += var + " << " + textToWrite + ";";
                            index++;
                            break;
                        }

                        if (line.Contains(".Close"))
                        {
                            string fileName = line.Substring(line.IndexOf(".Write") + 5).Replace(" ", "").Replace("\t", "");
                            GeneratedCode[index] += var + ".close();";
                            index++;
                            break;
                        }

                        if (line.Contains(".ReadByLine"))
                        {
                            string stringName = line.Substring(line.IndexOf(".ReadByLine")+11).Replace(" ", "").Replace("\t", "");
                            GeneratedCode[index] += "while (getline(" + var + "," + stringName + ")){\n" + "cout << " + stringName + ";\n}";
                            index++;
                            break;
                        }

                        if (!line.Contains("int") && !line.Contains("string") && var != lastVar)
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
                        if (lineDef.Contains("int") && funcName != lastFuncName)
                        {
                            lastFuncName = funcName;

                            string listOfArgumentsInt = lines[index].Substring(lines[index].IndexOf('(')).Replace("(", "").Replace(")", "");
                            listOfArgumentsInt = listOfArgumentsInt.Replace("string", "").Replace("int", "").Replace(",", "");
                            string[] listOfArgumentsFinal = new string[8];
                            int argumentsIntIndex = 0;
                            foreach (char c in listOfArgumentsInt)
                            {
                                if (c != ' ')
                                {
                                    listOfArgumentsFinal[argumentsIntIndex] += c;
                                    if (c == ',')
                                        argumentsIntIndex++;
                                }
                            }
                            GeneratedCode[index] += "int ";
                            GeneratedCode[index] += funcName + "(";
                            foreach (string arg in listOfArgumentsFinal)
                            {
                                if (arg != null)
                                {
                                    GeneratedCode[index] += "int " + arg;
                                    if (argumentsIntIndex > 1)
                                        GeneratedCode[index] += ", ";
                                    VARS.Add(arg);
                                }
                            }

                            if (argumentsIntIndex > 1)
                                GeneratedCode[index] = GeneratedCode[index].Remove(GeneratedCode[index].Length - 2);

                            GeneratedCode[index] += ")";
                            GeneratedCode[index] += "{";
                            index++;
                        }

                        if (lineDef.Contains("string"))
                        {
                            lastFuncName = funcName;

                            string listOfArgumentsInt = lines[index].Substring(lines[index].IndexOf('(')).Replace("(", "").Replace(")", "");
                            listOfArgumentsInt = listOfArgumentsInt.Replace("int", "").Replace("string", "").Replace(",", "");
                            string[] listOfArgumentsFinal = new string[8];
                            int argumentsIntIndex = 0;
                            foreach (char c in listOfArgumentsInt)
                            {
                                if (c != ' ')
                                {
                                    listOfArgumentsFinal[argumentsIntIndex] += c;
                                    if (c == ',')
                                        argumentsIntIndex++;

                                }
                            }
                            GeneratedCode[index] += "string ";
                            GeneratedCode[index] += funcName + "(";
                            foreach (string arg in listOfArgumentsFinal)
                            {
                                if (arg != null)
                                {
                                    GeneratedCode[index] += "string " + arg;
                                    if (argumentsIntIndex > 1)
                                        GeneratedCode[index] += ", ";
                                    VARS.Add(arg);
                                }
                            }

                            if (argumentsIntIndex > 1)
                                GeneratedCode[index] = GeneratedCode[index].Remove(GeneratedCode[index].Length - 2);

                            GeneratedCode[index] += ")";
                            GeneratedCode[index] += "{";
                            index++;
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
            File.AppendAllText("genCode.cpp", "#include <iostream>\n#include <conio.h>\n#include <fstream>\n\nusing namespace std;\n");

            for (int i = 0; i < GeneratedCode.Length; i++)
                File.AppendAllText("genCode.cpp", "\n" + GeneratedCode[i]);

            Console.WriteLine("Kompiluję...");
            string args = "";
            
            args = args + $"{"-o " + Program.FLAG_NAME}";
            Process.Start("g++", $"{args} -O2 -s genCode.cpp");
            var startInfo = new ProcessStartInfo
            {
                WorkingDirectory = Directory.GetCurrentDirectory(),
                FileName = Program.FLAG_NAME + ".exe"
            };

            Process.Start("g++", "-O2 -s genCode.cpp");
            
            Console.WriteLine("Gotowe");
            if (Program.FLAG_RUN)
            {
                Thread.Sleep(2000);
                Process.Start(startInfo);
            }
            else
                Console.ReadKey();
        }
    }
}
