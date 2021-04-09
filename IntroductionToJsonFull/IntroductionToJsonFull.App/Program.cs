using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;   // WebClient
using System.Text;
using System.IO;
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


            while (Console.ReadKey().Key != ConsoleKey.Escape)
                Console.WriteLine("Press [esc] to exit");
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
            int _time;              // in Unix Format: https://en.wikipedia.org/wiki/Unix_time
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


namespace Week_5_File_PREP.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Q1();
            Q2();
            Q3();

            Console.ReadKey();
        }

        static void Q1()
        {
            // Input String
            string content = "Hello File";          // Could be replaced by Console.WriteLine & ReadLine

            // create new file
            string filename = "sentences_original.txt";
            File.WriteAllText(filename, content);   // Create an "intermediate" file and write the contents of "content" to it

            // grab the info: both the content and the path
            StreamReader filereader = File.OpenText(filename);          // stores the content in the file in "filereader"
            string docpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);  // desired output location

            // combine
            string docinfo = Path.Combine(docpath, "sentences.txt");     /* combine path and O/P file name
                                                                                         to form the location */
            bool exists = File.Exists(filename);    // presence check

            // add text after the text that already exists
            if (exists)
            {
                Console.WriteLine("Already Exists");
                Console.WriteLine("Do you want to write sth else ?");

                // using provides a localized "function" - wrapping for output file so don'e need to close it
                using (StreamWriter filewriter = File.AppendText(docinfo))     // alternative: new StreamWriter(docinfo, true)
                {
                    string line;
                    while (true)
                    {
                        line = filereader.ReadLine();       // reads the text in the file stored in "filereader" line by line
                        if (line == null)
                        { break; }                          // stops when all lines has been read and written
                        filewriter.WriteLine(line);
                    }
                    // close the Streamreader
                    filewriter.Close();
                }
                // close the Streamwriter
                filereader.Close();
            }
            // creating new file / overwriting nothing
            else
            {
                using (StreamWriter filewriter = File.CreateText(docinfo))      // alternative: new StreamWriter(docinfo)
                {
                    // doing the same thing
                    string line = string.Empty;
                    while (line != null)
                    {
                        line = filereader.ReadLine();
                        if (line != null)
                        {
                            filewriter.WriteLine(line);
                        }
                    }
                    // close both
                    filewriter.Close();
                }
                filereader.Close();
            }
        }

        static void Q2()
        {
            int counts = 4;
            int max = 10;

            // Random Number Generator setup
            Random rand = new Random();         // variable type: random number

            // Create the location of the output file
            string doc_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string doc_loc = Path.Combine(doc_path, "Q2_Main.txt");

            if (!File.Exists(doc_loc))
            {
                using (StreamWriter SW = File.CreateText(doc_loc))
                {
                    for (int i = 0; i < counts; i++)
                    {
                        SW.WriteLine(rand.Next(1, max + 1));
                    }
                    SW.Close();
                }
            }

            // 2nd file - sorting   
            string doc_path2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string doc_loc2 = Path.Combine(doc_path2, "Q2_Sorted.txt");     // generate full path

            // placing the values in list

            List<string> ListofStrings = File.ReadAllLines(doc_loc).ToList();

            foreach (string n in ListofStrings)
            {
                Console.WriteLine(n);
            }
            List<int> ListofIntegers = ListofStrings.Select(x => int.Parse(x)).ToList();  // Stack Overflow

            int SumOfList = 0;
            foreach (int INTEGERS in ListofIntegers)
            {
                SumOfList += INTEGERS;      // compare using system method & manual method
            }
            Debug.Assert(SumOfList == ListofIntegers.Sum());    // type & value check

            //List<int> ListofIntegers = ListofStrings.Select(x => new int() { SomeValue = x.SomeValue }).ToList();

            /* Sorting strategy: bubble sort, the smaller value goes into another list, where this process is repeated until 
              foreach int in list Debug.Assert(PREVIOUS_VALUE < NEXT_VALUE) */
        }

        static void Q3()
        {
            // get the path of the file
            string filename = "stations.txt";
            string PATH = Path.GetFullPath(filename);
            List<string> Alphabet = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            if (!String.IsNullOrEmpty(PATH))    // Stack Overflow - LinQ Query method
            {
                var query = File.ReadLines(PATH).Where(line => !String.IsNullOrEmpty(line)).GroupBy(line => line.First());
                /*IDs = query.Select(a => a.ID).ToList();
                List<string> IDs = (from c in doc.Root.Elements("a").Elements("b") select c.Element("val").Value).ToList()*/
                // this structure will be used to get values from linQ query to a string List

                foreach (string l in Alphabet)
                {
                    if (!IDs.Contains(l))
                    {
                        Console.WriteLine(l);
                    }
                }
            }

        }
    }
}
