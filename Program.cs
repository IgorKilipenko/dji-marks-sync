// See https://aka.ms/new-console-template for more information
using dji_marks_sync;
using System.Text.RegularExpressions;

Console.WriteLine("DJI-MARKS-SYNC");

string inputPattern = @"(?<inMrk>\-[iI]\s+(?<inMrkPath>.+))?";

while (true) {
    var cmd = Console.ReadLine();
    if (cmd == null || cmd == "exit") {
        return;
    }
    Regex reg = new(@"(?<imgsDir>\-[dD]\s+(?<imgsDirPath>.+))?" + inputPattern, RegexOptions.CultureInvariant);
    var match = reg.Match(cmd);
    if (true /*match.Success*/) {
        Editor editor = new();
        //editor.EditTeoboxMrkFile("./data/01_DJI_202309121711_009_-50_PPKOBS.obs.txt", "./data");
        //editor.EditTeoboxMrkFile(match.Groups["inMrkPath"].Value, match.Groups["inMrkPath"].Value);
        editor.EditTeoboxMrkFile("F:/YandexDisk/Компьютер DESKTOP-H5HF4NE/ИГОРЬ_ДАР/_ОФП/29.09.2023_АФС_ПЛИТЫ_ЗОРГЕ/_teobox_data/01_DJI_202309291722_014_-50_PPKOBS.obs_GNS_GLONAS_.txt", "F:/YandexDisk/Компьютер DESKTOP-H5HF4NE/ИГОРЬ_ДАР/_ОФП/29.09.2023_АФС_ПЛИТЫ_ЗОРГЕ/_src/2023-09-29_Elit_Doroga viezd (50) a/Фото");
    } else {
        System.Diagnostics.Debug.Print("Invalid args");
    }
}