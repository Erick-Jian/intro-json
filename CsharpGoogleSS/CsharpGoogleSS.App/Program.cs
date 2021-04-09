using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;   
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CsharpGoogleSS.App
{
    #region Google Spreadsheet API:
    ///     Client ID: 147244942006-lidco6hc26nm2pe4af5nb1rn1131u722.apps.googleusercontent.com
    ///     Client Secret: LqYsz0eNNsENarwmZeQrSqmf
    ///     .Net setup: https://developers.google.com/sheets/api/quickstart/dotnet
    ///     Download Clinet Config: data:attachment/json;charset=utf-8,%7B%22installed%22%3A%7B%22client_id%22%3A%22147244942006-lidco6hc26nm2pe4af5nb1rn1131u722.apps.googleusercontent.com%22%2C%22project_id%22%3A%22quickstart-1616964666391%22%2C%22auth_uri%22%3A%22https%3A%2F%2Faccounts.google.com%2Fo%2Foauth2%2Fauth%22%2C%22token_uri%22%3A%22https%3A%2F%2Foauth2.googleapis.com%2Ftoken%22%2C%22auth_provider_x509_cert_url%22%3A%22https%3A%2F%2Fwww.googleapis.com%2Foauth2%2Fv1%2Fcerts%22%2C%22client_secret%22%3A%22LqYsz0eNNsENarwmZeQrSqmf%22%2C%22redirect_uris%22%3A%5B%22urn%3Aietf%3Awg%3Aoauth%3A2.0%3Aoob%22%2C%22http%3A%2F%2Flocalhost%22%5D%7D%7D
    ///     
    ///     Name: Quickstart
    ///     Type: Desktop App

    #endregion

    #region Connecting Spreadsheet with Visual Studio C#   &   fetching data (with credentials.j)
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        static void Main(string[] args)
        {
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

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";  // the series AFTER  spreadsheets/d/
            String range = "Class Data!A2:E";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                Console.WriteLine("Name, Major");
                foreach (var row in values)
                {
                    // Print columns A and E, which correspond to indices 0 and 4.
                    Console.WriteLine("{0}, {1}", row[0], row[4]);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            Console.Read();
        }
    }
    #endregion
}