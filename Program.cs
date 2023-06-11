using System.Diagnostics;
using static System.Environment;

namespace LeagueOfSettings
{
    public class Program
    {
        public static readonly Dictionary<string, string> NumToLang = new Dictionary<string, string>
        {
            {"1", "English" },
            {"2", "English (GB)"},
            {"3", "English (AU)"},
            {"4", "Simplified Chinese" },
            {"5", "Traditional Chinese" },
            {"6", "German" },
            {"7", "French" },
            {"8", "Greek" },
            {"9", "Italian" },
            {"10", "Japanese" },
            {"11", "Korean" },
            {"12", "Polish" },
            {"13", "Portuguese" },
            {"14", "Romanian" },
            {"15", "Russian" },
            {"16", "Spanish" },
            {"17", "Turkish" },
        };
        public static readonly Dictionary<string, string> LangugeToLocale = new Dictionary<string, string>
        {
            {"English", "en_US" },
            {"English (GB)", "en_GB" },
            {"English (AU)", "en_AU" },
            {"Simplified Chinese", "zh_CN" },
            {"Traditional Chinese", "zh_TW" },
            {"German", "de_DE" },
            {"French", "fr_FR" },
            {"Greek", "el_GR" },
            {"Italian", "it_IT" },
            {"Japanese", "ja_JP" },
            {"Korean", "ko_KR" },
            {"Polish", "pl_PL" },
            {"Portuguese", "pt_PT" },
            {"Romanian", "ro_RO" },
            {"Russian", "ru_RU" },
            {"Spanish", "es_ES" },
            {"Turkish", "tr_TR" },
        };
        public static readonly string LoRClientName = "LoR";
        public static readonly string RiotClientName = "RiotClientServices";
        public static readonly string LeagueClientName = "LeagueClient";
        public static readonly string ValorantClientName = "VALORANT";
        public static readonly string LeagueOfLegendsSettingsFile = "league_of_legends.live.product_settings.yaml";
        public static readonly string RiotPath = Path.Combine(GetFolderPath(SpecialFolder.CommonApplicationData), "Riot Games");
        public static readonly string LeagueOfLegendsPath = Path.Combine(RiotPath, "Metadata", "league_of_legends.live");
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to LeagueOfSettings!");
            Console.WriteLine("You can select game language here (Note that LoL, LoR, Valorant and RiotClient will be closed)!");
            Console.WriteLine("It is recommended to close every Riot application before continuing!");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            if (!FolderExists(RiotPath)){
                Console.WriteLine("You do not have Riot Client installed.");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            else if (!FolderExists(LeagueOfLegendsPath))
            {
                Console.WriteLine("You do not have LeagueOfLegends installed.");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            else if (!FileExists(Path.Combine(LeagueOfLegendsPath, LeagueOfLegendsSettingsFile)))
            {
                Console.WriteLine("LoL settings file is missing. Try checking game's integrity.");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Select the language:");
                Console.WriteLine();
                Console.WriteLine();
                foreach (string lang in NumToLang.Keys)
                {
                    Console.WriteLine($"{lang}) {NumToLang[lang]}");
                }
                string input = Console.ReadLine() ?? "0";
                while (!CheckInput(input))
                {
                    Console.WriteLine("Input selected language code");
                    input = Console.ReadLine() ?? "0";
                }
                Console.WriteLine();
                Console.WriteLine("Killing LoR");
                KillProcess(LoRClientName);
                Console.WriteLine("Killing LoL");
                KillProcess(LeagueClientName);
                Console.WriteLine("Killing Valorant");
                KillProcess(ValorantClientName);
                Console.WriteLine("Killing RiotClient");
                KillProcess(RiotClientName);
                string LangName = NumToLang[input];
                string Locale = LangugeToLocale[LangName];
                Console.WriteLine();
                Console.WriteLine($"Setting locale {Locale} (Lang: {LangName})");
                try
                {
                    SetLocale(Locale);
                    Console.WriteLine($"Set locale {Locale} (Lang: {LangName})");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occured.\n{ex.Message}\n{ex.StackTrace}");
                }
            }
        }
        public static void SetLocale(string localeCode)
        {
            string filePath = Path.Combine(LeagueOfLegendsPath, LeagueOfLegendsSettingsFile);
            string[] arrLine = File.ReadAllLines(filePath);
            int localeLine = arrLine.ToList().FindIndex(0, arrLine.Length, (string line)=>line.Trim().StartsWith("locale: \""));
            string originalLine = arrLine[localeLine];
            string newLine = $"{originalLine.Split("\"")[0]}\"{localeCode}\"";
            arrLine[localeLine] = newLine;
            File.WriteAllLines(filePath, arrLine);
        }
        public static void KillProcess(string name)
        {
            foreach (var process in Process.GetProcessesByName(name))
            {
                process.Kill();
            }
        }
        public static bool CheckInput(string input)
        {
            return NumToLang.ContainsKey(input);
        }
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }
        public static bool FolderExists(string path)
        {
            return Directory.Exists(path);
        }
    }
}