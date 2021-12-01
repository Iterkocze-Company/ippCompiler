# ippCompiler
ipp Language Compiler. ipp is a cross-platform language that is compiled to c++.

#### 
      "i made it, and it work" 
                        ~Michał 2021

##### To use the compiler, you need to have g++ in your PATH. <br>
##### Do not use blank spaces in code, use tabs instead. <br>

### Instructions in i++
`Echo "hello"` Lub `EchoLine "hello"` Displays string in console window. You can use `~` operator to concatenate two or more strings. <br> <br>

`ReadKey` Halt program and wait for user input. Returns ASCII value of inputed char. <br> <br>

`ReadString` Halt program and wait for user input. Returns inputed string. Since i++ is compiled to C++, this code is not legal in i++: `string s = ReadString;`. <br>

`Wait 'int millisecond'` ex. `Wait 2000` Halts program for 2 seconds. <br>

`return` Returns value from function. ex. `return 4;`. <br>

`if` and `else` work in the same way as in C++. ex. `if x == 1;`. To end `if` block, Use `end`. <br>
```
if x == 1;
  EchoLine x; 
end; 
else; 
  EchoLine "else"; 
end;
``` 

`for` ex. `for int i = 1; i < 10; i++;`. <br>
`while` ex. `while 1==1;`. <br>
`DoWhile` ex.
```
DoWhile 1==1;
 EchoLine "Helo";
end;
```
<br>

#### File handling in i++
`File` To create file object. ex. `File myFile;`. <br>
`File.Open` To open a file. ex. `myFile.Open "file.txt";`. <br>
`File.Write` Write to file. ex. `myFile.Write "text";`. <br>
`File.Close` Close file. ex. `myFile.Close;`. <br>
`CreateFile 'FileName'`. ex. `CreateFile file;` Will create file.txt <br>
`File.ReadByLine 'string output'` or `File.ReadByLineEcho` To read file content. ex. `myFile.ReadByLine str;`. `File.ReadByLine` and `File.ReadBylineEcho` are loops, so they need to be ended with `end;`. <br>

### Stuff
Read specific character from string:
```
string s = "text";
EchoLine s[0];
```
### Operators in i++
`~` Concatenate operator. You can use it in `Echo` or `EchoLine`, to merge strings. <br> <br>
`=` Use it to assign value to a variable. ex. `int hello = 123;`. After using `=` add one (` `)/blank space after variable name. <br> <br>

#### Arithmetic operators in i++
##### Use in `Echo` or `EchoLine`. ex. `EchoLine x*2;`
`+` Addition <br>
`-` subtraction <br>
`~` Concatenate. ex. `EchoLine "Hello" ~ " World";` <br>
`*` Multiplication <br>
`/` Division <br>
`%` Modulo <br>
##### Outside of `Echo` or `EchoLine` ex. `x++;`
`++` Increment <br>
`*` Multiplication <br> 
`/` Division <br> <br>

### Types in i++
`int` <br>
`string` ex. `string s;` or `string s = "hello";`. <br> 
`char` <br>
`float` <br>
`double` <br>
`void` In function definitions. ex. `def void test();`. <br> <br>

### Functions in i++
Use `def`, in order to define a function. After `def` keyword provide return type. ex. `def int test();`. End function definition with `end` keyword. ex.
```
def void test();
  EchoLine "Function!";
end;
```

### Adding other .ipp
In order to add other, existing .ipp file Use `use` keyword. ex.
```
use "code.ipp";

def int main();
  FromDiffrentFile();
end;
```

Functions can take arguments. ex. `def int test(int x, int y);`.

### Compiler Flags
`name=example` Change compiled .exe filename to 'example'. <br> <br>
`run` Run program after compilation. <br> <br>
`linux` Use this flag, if you are using ippCompiler in Linux. <br> <br>
`force` Use this flag to force compile program regardless of errors detected by ippCompiler. <br> <br>
`macros` Downloads additional files needed by ippCompiler. <br> <br>
ex: `name=hello, run, force` <br>

You can use ippCompiler from command line too. ex. `ippCompiler.exe code.ipp run` or `ippCompiler.exe code.ipp -run`

### Macros
`bool MacroContains(string s1, string s2)` Returns true, is s1 contains s2, otherwise false.

### Hello World in i++
Every i++ program needs to have main function defined. Use `def int main();`. <br>
```
def int main();
  EchoLine "Hello, World!";
end;
```

Function calling example: <br>
```
def void test(); 
  EchoLine "Function"; 
end; 

def int main(); 
  test(); 
end;
```
