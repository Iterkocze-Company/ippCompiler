def int main();
	CreateFile plik;
	File plik;

	plik.Open "plik.txt";
	plik.Write "hello";
	plik.Close;
end;