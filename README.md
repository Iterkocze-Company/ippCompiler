# ippCompiler
Kompilator języka i++
##### Aby używać kompilatora, musisz mieć działający kompilator GNU G++ w zmiennej PATH. <br>

### Instrukcje Języka i++
`Echo "helo"` Lub 'echoLine "helo"' Powoduje wypisanie ciągu znaków w konsoli. Można użyć z operatorem '+'. <br> <br>

`ReadKey` Powoduje postój w oczekiwaniu na input ze strony użytkownika programu. W przypadku użycia ze zadeklarowaną zmienną, zwróci kod ASCII wpisanej liery. <br> <br>

`ReadString` Powoduje postój w oczekiwaniu na input ze strony użytkownika programu. Musi być użyty, aby odczytać wartość wpisaną przez użytkownika w konsoli. Instrukcję `ReadString` można też użyć ze zmienną typu int. Input zostanie automatycznie przekonwertowany na typ int. Na przykład po deklaracji zmiennej `s = readString;`. Należy pamiętać, że język C++, do którego konwertowany jest język i++ nie wspiera odczytywania wartości operatorem 'cin', w tej samej linijce, po deklaracji zmiennej. Ten kod jest niepoprawny `string s = readString;`. <br>

`return` Zwraca wartość po wykonaniu funkcji. Na przykład `return 4;`. <br>

`if` oraz `else` Służy do podejmowania decyzji na podstawie konkretnego warunku/warunków. Na przykład `if (x == 1);`. Aby zakończyć instrukcję `if`, użyj słowa kluczowego `end`. Możesz też użyć `else` razem, z instrukcją `if` w następujący sposób: <br>
```
if x == 1;
  echoLine x; 
end; 
else; 
  EchoLine "else"; 
end;
``` 

`for` Na przykład: `for int i = 1; i < 10; i++;`. <br>
`while` Na przykład: `while 1==1;`. <br>
`DoWhile` Na przykład:
`DoWhile 1==1;`
`EchoLine "Helo";`
`end;` <br>

Wszystkie spójniki logiczne są identyczne do tych, spotykanych w językach C/C++. <br> <br>

#### Obsługa Plików w Języku i++
`File` Aby stworzyć obiekt pliku. Na przykład `File mojPlik;`. <br>
`File.Open` Aby otworzyć plik. Na przykład `mojPlik.Open "plik.txt";`. <br>
`File.Write` Aby zapisać dane w pliku. Na przykład `mojPlik.Write "tekst";`. <br>
`File.Close` Aby zamknąć plik. Na przykład `mojPlik.Close;`. <br>
`File.ReadByLine 'string output'` lub `File.ReadByLineEcho` Aby wczytać zawartość pliku. Na przykład `mojPlik.ReadByLine str;`. <br>

### Możliwości
Można wybrać pojedynczy znak ze stringa:
```
string s = "text";
EchoLine s[0];
```
### Operatory w Języku i++
`+` Operator Konkatenacji. Można użyć go razem z instrukcją `echo` Lub `echoLine`, aby połączyć dwa (lub więcej) napisów w jeden. <br> <br>
`=` Operator Przypisania. Używa się go, aby przypisać wartość do zadeklarowanej zmiennej. Na przykład `int helo = 123;`. Podczas używania operatora przypisania, należy dodać jeden odstęp (` `) po nazwie zmiennej. <br> <br>

#### Operatory Arytmetyczne w Języku i++
##### Należy używać ich w instrukcji `echo` lub `echoLine`. Na przykład `echoLine x*2;`
`+` Dodawanie <br>
`*` Mnożenie <br>
`/` Dzielenie <br>
`%` Dzielenie Modulo <br>
##### Należy ich używać samodzielnie, w środku kodu. Na przykład `x++;`
`++` Inkrementacja <br>
`*` Mnożenie <br> 
`/` Dzielenie <br> <br>

### Typy w Języku i++
`int` Deklaruje zmienną typu Int32 lub Int64, zależnie od platformy. Można tej zmiennej przypisać wartość w tej samej linijce, zaraz po deklaracji, lub później za pomocą operatora przypisania `=`. <br> <br>
`string` Deklaruje zmienną typu String. Na przykład `string s;` lub `string s = "helo";`. <br> 
`char` <br>
`float` <br>
`double` <br>
`void` Dozwolony przy deklarowaniu funkcji. Na przykład `def void test();`. Oznacza brak zwracanego typu. <br> <br>

### Deklarowanie Funkcji w Języku i++
Użyj słowa kluczowego `def`, aby zadeklarować funkcję. Po słowie kluczowyn `def` wpisz typ zwracany przez funkcję. Na przykład `def int test();`. Po zakończeniu pracy z funkcją, wpisz słowo kluczowe `end`. Przykładowa deklaracja funkcji:
```
def int test();
  echoLine "Helo z funkcji!";
end;
```

### Używanie innych plików .ipp
Aby dołączyć plik .ipp do innego pliku kodu .ipp należy użyć `use`. Na przykład:
```
use "kod.ipp";

def int main();
  EchoLine "Hello";
end;
```

Funkcje mogą przyjmować też argumenty. Na przykład `def int test(int x, int y);`.

### Flagi Kompilatora
`name=example` Dodanie tej flagi spowoduje zmianę nazwy wykompilowanego pliku .exe na 'example'. <br> <br>
`run` Uruchamia program po jego kompilacji. <br> <br>
`linux` Dodaj tę flagę, jeśli używasz ippCompiler na Linuxie. <br> <br>
`force` Dodaj tę flagę, aby wykompilować program pomomo błędów wykrytych przez ippCompiler. <br> <br>
`macros` Pobiera pliki wymagane przez ippCompiler. <br> <br>
Przykładowy sposób użycia flag kompilatora: `name=hello, run, force` <br>

Mozesz też wprowadzić flagi z poziomu konsoli systemowej. np. `ippCompiler.exe kod.ipp run` lub `ippCompiler.exe kod.ipp -run`

### Dostępne Makra
`bool MacroContains(string s1, string s2)` Zwraca true, jeśli s1 zawiera s2, jeśli nie, false.

### Twój Pierwszy Program w Języku i++
Aby pomyślnie wykompilować program, musisz zdefiniować funkcję main za pomocą `def int main;`. Oto program Hello World w i++: <br>
```
def int main();
  EchoLine "Hello, World!";
end;
```

Aby wywołać funkcję, zwyczajnie wpisz jej nazwę zakończoną klamrami. Oto przykładowy program demonstrujący wywoływanie zadeklarowanej funkcji: <br>
```
def int test(); 
  EchoLine "Funkcja"; 
end; 
def int main; 
  test() 
end;
```
