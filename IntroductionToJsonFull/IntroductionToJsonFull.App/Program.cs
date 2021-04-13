using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;   // WebClient
using System.Text;
using System.IO;
using System.Timers;
using System.Threading.Tasks;

namespace IntroductionToJsonFull.App
    /// serialize - storing stuff as text (Lists, Objects) -- language independent
    /// build allows the program to search for new packages, and "inclnude" them in program
{
    class Program
    {
        // step one - using the nuget package manager
        // https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio 
        static void Main(string[] args)
        {
            OpenFile();

            while (Console.ReadKey().Key != ConsoleKey.Escape)
                Console.WriteLine("press [Esc] to exit");
        }

        static void JsonString()
        {
            Console.WriteLine("Hello World!");

            string json = @"{""key1"":""value1"",""key2"":""value2""}";

            IDictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            // An Interface

            foreach (string k in values.Keys)
                Console.WriteLine($"{k}: {values[k]}");


            // e.g. https://www.newtonsoft.com/json/help/html/DeserializeObject.htm 
            // json string
            string json2 = @"{
            'Email': 'james@example.com',
            'Active': true,
            'CreatedDate': '2021-01-20T00:00:00Z',
            'Roles': [
                'User',
                'Admin'     ]   }";

            Account account = JsonConvert.DeserializeObject<Account>(json2);    // takes the text match the field names

            Console.WriteLine(account.Email);

            Account a2 = new Account("a@b.com", false, DateTime.UtcNow, new List<string> { "user" });
            string j3 = JsonConvert.SerializeObject(a2);
            Console.WriteLine(j3);


            string jsonDownload;

            // IDisposible: create a Dispose "method"
            // WebClient: browser, fetch data.
            using (var wc = new WebClient())    // creates the connetion & close it
            {
                jsonDownload = wc.DownloadString("https://blockchain.info/latestblock");
                // CONNECT, DOWNLOAD the text in the url
            }
            // DISCONNECT the linkage to the URL

            dynamic j = JArray.Parse(jsonDownload);

            dynamic l = j[1];


            Console.WriteLine(l.Count);
            foreach (dynamic row in l)
            {
                Console.WriteLine($"{row.date}: {row.countryiso3code}: ${row.value / 1000000000:F3}bn");
            }
        }

        // private static Timer Timer;     // create a Timer

        static void OpenFile()
        {
            string filepath = @"C:\Users\stbay\Desktop\Computer Science\C#\Github\intro-json-Erick-Jian\IntroductionToJsonFull\IntroductionToJsonFull.App\Bitcoin_Blocks.txt";
            string content;


            using (var BuiltInBrowser = new WebClient())    // creates the connetion & close it
            {
                content = BuiltInBrowser.DownloadString("https://blockchain.info/latestblock");
                // CONNECT, DOWNLOAD the text in the url
            }

            bool Existence = File.Exists(filepath); 
            if (Existence == true)      // txt exists
            {
                using (StreamWriter FileWriter = File.AppendText(filepath))
                {    
                    while(true)
                    {
                        FileWriter.WriteLine(content + "\n");
                        System.Threading.Thread.Sleep(18000);
                        continue;
                    }
                }
            }
        }

        public class Account
        {
            string _email;
            bool _active;
            DateTime _created;
            IList<string> _roles;

            public string Email { get => _email; set => _email = value; }
            public bool Active { get => _active; set => _active = value; }
            public DateTime CreatedDate { get => _created; set => _created = value; }
            public IList<string> Roles { get => _roles; set => _roles = value; }

            public Account(string e, bool a, DateTime c, IList<string> r)
            {
                _email = e;
                _active = a;
                _created = c;
                _roles = r;
            }
        }

        public class BlocksBTC
        {
            string _SHA256hash;
            int _time;              // in Unix Timestamp Format: https://en.wikipedia.org/wiki/Unix_time
            int _BlockIndex;
            int _height;
            Array[] _txIndexes;

            public BlocksBTC(string HASH, int Unix_TIME, int INDEX, int HEIGHT, Array[] TXIND)
            {
                _SHA256hash = HASH;
                _time = Unix_TIME;      // Unix epoch is 00:00:00 UTC on 1 January 1970; number of SECONDs
                _BlockIndex = INDEX;
                _height = HEIGHT;
                _txIndexes = TXIND;
            }

            public string SHA256Hash { get => _SHA256hash; set => _SHA256hash = value; }
            public int Time { get => _time; set => _time = value; }
            public int BlockIndex { get => _BlockIndex; set => _BlockIndex = value; }
            public int Height { get => _height; set => _height = value; }
            public Array[] TxIndexes { get => _txIndexes; set => _txIndexes = value; }

        }
    }
}
