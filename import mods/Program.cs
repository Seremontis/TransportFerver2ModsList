
using ImporterMods.Logic;


FileOperation FileOperation = new FileOperation();
FileData FileData = new FileData();


Console.WriteLine("Sprawdzić poprawność przed importem w SteamCmd- 1 \nSegracja do folderów - 2\n Napisz skrpyt SteamCmd - 3\n Rename mods-4\n Correct mods-5");
string input = Console.ReadLine();

if (input == "1")
{
    FileOperation.CheckCorretionFile(FileOperation, FileData);
    FileOperation.WriteScript(FileData);
}
else if (input == "2")
{
    Console.WriteLine("Opcja wyłączona");
    /*if (!Directory.Exists("./pliki"))
        Directory.CreateDirectory("./pliki");

    if (!Directory.Exists("./pliki/Mods"))
        Directory.CreateDirectory("./pliki/Mods");

    if (!Directory.Exists("./pliki/Assets"))
        Directory.CreateDirectory("./pliki/Assets");

    if (!Directory.Exists("./pliki/ColorCorrections"))
        Directory.CreateDirectory("./pliki/ColorCorrections");

    if (!Directory.Exists("./pliki/Maps"))
        Directory.CreateDirectory("./pliki/Maps");

    if (!Directory.Exists("./pliki/Raw"))
        Directory.CreateDirectory("./pliki/Raw");

    string[] listfiles = Directory.GetFiles("./pliki/Raw");

    foreach (var item in setup)
    {
        Extract(item, listfiles, ref FileData.current);
    }*/
}
else if (input == "3")
    FileOperation.WriteScript(FileData);
else if (input == "4")
    FileOperation.RenameSteammods();
else if (input == "5")
    FileOperation.CorrectNameMod();

Console.WriteLine("=====Koniec====");
Console.ReadKey();


