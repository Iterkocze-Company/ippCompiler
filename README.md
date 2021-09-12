# ippCompiler
Kompilator języka i++ :3 <br> <br>

### Instrukcje Języka i++
`echo "helo"` Lub 'echoLine "helo"' Powoduje wypisanie ciągu znaków w konsoli. Można użyć z operatorem '+'. <br> <br>
`readKey` Powoduje postój w oczekiwaniu na input ze strony użytkownika programu. W przypadku użycia ze zadeklarowaną zmienną, zwróci kod ASCII wpisanej liery. <br&gt; <br&gt;

### Operatory w Języku i++
`+` Operator Konkatenacji. Można użyć go razem z instrukcją `echo` Lub `echoLine`, aby połączyć dwa (lub więcej) napisów w jeden. <br> <br>
`=` Operator Przypisania. Używa się go, aby przypisać wartość do zadeklarowanej zmiennej. Na przykład `int helo = 123;`. Podczas używania operatora przypisania, należy dodać jeden odstęp (` `) po nazwie zmiennej. <br> <br>

### Typy w Języku i++
`int` Deklaruje zmienną typu Int32 lub Int64, zależnie od platformy. Można tej zmiennej przypisać wartość w tej samej linijce, zaraz po deklaracji, lub później za pomocą operatora przypisania `=`. <br> <br>

### Flagi Kompilatora
`name=example` Dodanie tej flagi spowoduje zmianę nazwy wykompilowanego pliku .exe na 'example'. <br> <br>
`run` Uruchamia program po jego kompilacji. Flaga `run` powinna być zawsze pierwsza (jeśli chcesz jej użyć). <br> <br>
