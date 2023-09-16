// See https://aka.ms/new-console-template for more information
using dji_marks_sync;
using System.Text.RegularExpressions;

Console.WriteLine("DJI-MARKS-SYNC");

string inputPattern = @"(?<in>\-[iI]\s+(?<inPath>.+))?";

while (true) {
    var cmd = Console.ReadLine();
    if (cmd == null || cmd == "exit") {
        return;
    }
    Regex reg = new(@"(?<out>\-[oO]\s+(?<outPath>.+))?" + inputPattern, RegexOptions.CultureInvariant);
    var match = reg.Match(cmd);
    if (reg.Match(cmd).Success) {
        Console.WriteLine($"Match: {match.Groups["inPath"].Value}");
        Parser parser = new();
        var marks = parser.ParseMrkFile("./data/DJI_202309121711_009_Элитный-ОП50_Timestamp.MRK");
        Console.WriteLine(marks);
    }
}