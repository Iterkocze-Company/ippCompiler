# ippCompiler
Kompilator języka i++
##### Aby używać kompilatora, musisz mieć działający kompilator GNU G++ w zmiennej PATH. <br> <br>

### Instrukcje Języka i++
`echo "helo"` Lub 'echoLine "helo"' Powoduje wypisanie ciągu znaków w konsoli. Można użyć z operatorem '+'. <br> <br>
`readKey` Powoduje postój w oczekiwaniu na input ze strony użytkownika programu. W przypadku użycia ze zadeklarowaną zmienną, zwróci kod ASCII wpisanej liery. <br> <br>
`readString` Powoduje postój w oczekiwaniu na input ze strony użytkownika programu. Musi być użyty, aby odczytać wartość wpisaną przez użytkownika w konsoli. Instrukcję `readString` można też użyć ze zmienną typu int. Input zostanie automatycznie przekonwertowany na typ int. Na przykład po deklaracji zmiennej `s = readString;`. Należy pamiętać, że język C++, do którego konwertowany jest język i++ nie wspiera odczytywania wartości operatorem 'cin', w tej samej linijce, po deklaracji zmiennej. Ten kod jest niepoprawny `string s = readString;`. <br> <br>

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
`string` Deklaruje zmienną typu String. Na przykład `string s;` lub `string s = "helo";`. <br> <br>

### Deklarowanie Funkcji w Języku i++
użyj słowa kluczowego `def`, aby zadeklarować funkcję. Po słowie kluczowyn `def` wpisz typ zwracany przez funkcję. Na przykład `def int test;`. Po zakończeniu pracy z funkcją, wpisz słowo kluczowe `end`. Przykładowa deklaracja funkcji: <br> `def int test;` <br> `echoLine "Helo z funkcji!";` <br> `end;` <br> <br> Niestety, wszystko, co jest w funkcji powinno nie zawierać odstępów ani tabów. Niestety, związane jest to z rdzenną mechaniką kompilatora. Z góry przepraszam za Code Gore. <br> <br>

### Flagi Kompilatora
`name=example` Dodanie tej flagi spowoduje zmianę nazwy wykompilowanego pliku .exe na 'example'. <br> <br>
`run` Uruchamia program po jego kompilacji. Flaga `run` powinna być zawsze pierwsza (jeśli chcesz jej użyć). <br> <br>

### Twój Pierwszy Program w Języku i++
Aby pomyślnie wykompilować protgram, musisz zdefiniować funkcję main za pomocą `def int main;`. Oto program Hello World w i++: <br>
`def int main;` <br>
`echoLine "Hello, World!";` <br>
`end;` <br> <br>