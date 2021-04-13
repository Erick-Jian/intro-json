using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CsharpGoogleSS.App
{
    # region Google Spreadsheet API Intro Info:
    ///     Client ID: 147244942006-lidco6hc26nm2pe4af5nb1rn1131u722.apps.googleusercontent.com
    ///     Client Secret: LqYsz0eNNsENarwmZeQrSqmf
    ///     .Net setup: https://developers.google.com/sheets/api/quickstart/dotnet
    ///     Download Clinet Config: data:attachment/json;charset=utf-8,%7B%22installed%22%3A%7B%22client_id%22%3A%22147244942006-lidco6hc26nm2pe4af5nb1rn1131u722.apps.googleusercontent.com%22%2C%22project_id%22%3A%22quickstart-1616964666391%22%2C%22auth_uri%22%3A%22https%3A%2F%2Faccounts.google.com%2Fo%2Foauth2%2Fauth%22%2C%22token_uri%22%3A%22https%3A%2F%2Foauth2.googleapis.com%2Ftoken%22%2C%22auth_provider_x509_cert_url%22%3A%22https%3A%2F%2Fwww.googleapis.com%2Foauth2%2Fv1%2Fcerts%22%2C%22client_secret%22%3A%22LqYsz0eNNsENarwmZeQrSqmf%22%2C%22redirect_uris%22%3A%5B%22urn%3Aietf%3Awg%3Aoauth%3A2.0%3Aoob%22%2C%22http%3A%2F%2Flocalhost%22%5D%7D%7D
    ///     
    ///     Name: Quickstart
    ///     Type: Desktop App

    # endregion

    # region Connecting Spreadsheet with Visual Studio C#   &   fetching data (with credentials.j)
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        static void Main(string[] args)
        {
            #region User Credential
            UserCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync( GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            #endregion

            # region Create Google Sheets API service  -- a pathway
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            # endregion

            SampleSS(service);
            //TargetSS(service);
        }

        # region Sample Spreadsheet
        static void SampleSS(SheetsService service)
        {
            // Define request parameters.
            String spreadsheetId = "1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";  // the series AFTER  spreadsheets/d/
            String range = "Class Data!A2:E";       // Data range: row A, E start from A2
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);          // "webclient" grab those data from SS

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                Console.WriteLine("Name, Major");   // Column Identifier: A1, E1
                foreach (var row in values)
                {
                    // Print columns A and E, which correspond to indices 0 and 4.
                    Console.WriteLine("{0}, {1}", row[0 ], row[4]);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            Console.Read();
        }
        #endregion

        # region Target Spreadsheet
        static void TargetSS(SheetsService service)
        {
            // Define request parameters.
            String sheetID = "1SqYrK3ekXRpirdJZxr52Sbk7ENXGcieJHmRndN4lq1o";       // the series AFTER  spreadsheets/d/
            String sheetRANGE = "Class Data!A2:E";
            SpreadsheetsResource.ValuesResource.GetRequest dataRequest =
                    service.Spreadsheets.Values.Get(sheetID, sheetRANGE);          // "webclient" grab those data from SS

            // Prints the target data:
            ValueRange sheetResponse = dataRequest.Execute();
            IList<IList<Object>> sheetValues = sheetResponse.Values;
            if (sheetValues != null && sheetValues.Count > 0)
            {
                Console.WriteLine("Serial Order, TimeStamp");   // Column Identifier: A1, E1
                foreach (var row in sheetValues)    
                {
                    // Print columns A and E, which correspond to indices 0 and 4.
                    Console.WriteLine("{0}, {1}", row[0], row[4]);
                }
            }
            else
                Console.WriteLine("No data found.");

            Console.Read();
        }
        # endregion
    }
    #endregion

    public class UnixtoDateTime
    {
        public static void Main(string[] args)
        {
            double UnixTimeStamp = 1618221585;
            DateTime dt = DateTime.Now;
            Debug.Assert(UnixTimeStampToDateTime(UnixTimeStamp) == dt);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();    // seconds add to 1970.1.1
            return dtDateTime;
        }
    }
}