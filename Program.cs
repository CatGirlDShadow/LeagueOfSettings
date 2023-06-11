using System.Diagnostics;
using static System.Environment;

namespace LeagueOfSettings
{
    public class Program
    {
        public static readonly Dictionary<string, string> NumToLang = new Dictionary<string, string>
        {
            {"1", "English" },
            {"2", "English(GB)"},
            {"3", "Arabic"},
            {"4", "Bosnian"},
            {"5", "Bulgarian" },
            {"6", "Catalan" },
            {"7", "Simplified Chinese" },
            {"8", "Traditional Chinese" },
            {"9", "Croatian" },
            {"10", "Czech" },
            {"11", "Danish" },
            {"12", "German" },
            {"13", "Dutch" },
            {"14", "Finnish" },
            {"15", "French" },
            {"16", "Georgian" },
            {"17", "Greek" },
            {"18", "Hebrew" },
            {"19", "Hungarian" },
            {"20", "Indonesian" },
            {"21", "Italian" },
            {"22", "Japanese" },
            {"23", "Korean" },
            {"24", "Norwegian" },
            {"25", "Polish" },
            {"26", "Portuguese(Brasil)" },
            {"27", "Portuguese" },
            {"28", "Romanian" },
            {"29", "Russian" },
            {"30", "Serbian" },
            {"31", "Slovak" },
            {"32", "Spanish" },
            {"33", "Swedish" },
            {"34", "Thai" },
            {"35", "Turkish" },
            {"36", "Ukrainian" },
            {"37", "Vietnamese" }
        };
        public static readonly Dictionary<string, string> LangugeToLocale = new Dictionary<string, string>
        {
            {"English", "en_US" },
            {"English(GB)", "en_GB" },
            {"Arabic", "ar_SA" },
            {"Bosnian", "bs_BA" },
            {"Bulgarian", "bg_BG" },
            {"Catalan", "ca_ES" },
            {"Simplified Chinese", "zh_CN" },
            {"Traditional Chinese", "zh_TW" },
            {"Croatian", "hr_HR" },
            {"Czech", "cs_CZ" },
            {"Danish", "da_DK" },
            {"German", "de_DE" },
            {"Dutch", "nl_NL" },
            {"Finnish", "fi_FI" },
            {"French", "fr_FR" },
            {"Georgian", "ka_GE" },
            {"Greek", "el_GR" },
            {"Hebrew", "he_IL" },
            {"Hungarian", "hu_HU" },
            {"Indonesian", "id_ID" },
            {"Italian", "it_IT" },
            {"Japanese", "ja_JP" },
            {"Korean", "ko_KR" },
            {"Norwegian", "nb_NO" },
            {"Polish", "pl_PL" },
            {"Portuguese(Brasil)", "pt_BR" },
            {"Portuguese", "pt_PT" },
            {"Romanian", "ro_RO" },
            {"Russian", "ru_RU" },
            {"Serbian", "sr_CS" },
            {"Slovak", "sk_SK" },
            {"Spanish", "es_ES" },
            {"Swedish", "sv_SE" },
            {"Thai", "th_TH" },
            {"Turkish", "tr_TR" },
            {"Ukrainian", "uk_UA" },
            {"Vietnamese", "vi_VN" },
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
            Console.WriteLine("Maximize this console window to see the list properly");
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