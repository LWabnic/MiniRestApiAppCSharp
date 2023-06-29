using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using MiniRestApp.Models;
using Moq;
using Moq.Protected;

namespace MiniRestAppTests;

public class CsvProcessorTests
{
    private string _filePath;
    private CsvConfiguration _config;

    [SetUp]
    public void Setup()
    {
        // test.csv file in the same directory as the DLLs
        _filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData/test.csv");
        _config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
        };
    }

    [Test]
    public void ShouldReadAllCustomersFromCsvFile()
    {
        using (var reader = new StreamReader(_filePath))
        using (var csv = new CsvReader(reader, _config))
        {
            var records = csv.GetRecords<Customer>();
            Assert.AreEqual(5, records.Count()); // 5 records in the CSV file
        }
    }
    
    [Test]
    public async Task ShouldSendCorrectRequests()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{}", Encoding.UTF8, "application/json"),
        };

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://localhost:5001/"),
        };

        // Act
        using (var reader = new StreamReader(_filePath))
        using (var csv = new CsvReader(reader, _config))
        {
            var records = csv.GetRecords<Customer>();
            foreach (var record in records)
            {
                var jsonString = JsonSerializer.Serialize(record);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var apiResponse = await httpClient.PostAsync("api/customers", content);

                // Assert
                Assert.AreEqual(HttpStatusCode.OK, apiResponse.StatusCode);
            }
        }

        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(5), // 5 records in the CSV file
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post
            ),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}