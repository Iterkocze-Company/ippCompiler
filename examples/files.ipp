def int main();
	CreateFile textFile;
	File file;

	file.Open "textFile.txt";
	file.Write "hello";
	file.Close;
end;