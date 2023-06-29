using System.Globalization;
using System.Text;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using MiniRestApp;
using MiniRestApp.Models;

string filePath = args[0];
var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    PrepareHeaderForMatch = args => args.Header.ToLower(),
};

using var httpClient = new HttpClient();

using (var reader = new StreamReader(filePath))
using (var csv = new CsvReader(reader, config))
{
    var records = csv.GetRecords<Customer>();
    foreach (var record in records)
    {
        Console.WriteLine(record.CustomerName);
        var jsonString = JsonSerializer.Serialize(record);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("[YourApiBaseUrl]/api/customers", content);

        // TODO: Handle response here. E.g. check if request was successful.
    }
}