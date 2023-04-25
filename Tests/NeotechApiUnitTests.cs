
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using static NeotechAPI.Services.DataLists;

namespace Tests;

public class NeotechApiUnitTests
{
    private readonly IConfiguration _config;
    public NeotechApiUnitTests() 
    {
        _config = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();
    }

    [Fact]
    public void ShouldReceiveOkResponseFromGoogle()
    {
        // arrange
        var datalists = new DataLists(_config);

        // act
        var responseCode = datalists.Download("Generations").Result;

        // assert
        responseCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async void ShouldGetDataFromGoogle()
    {
        // arrange
        var datalists = new DataLists(_config);

        // act
        var responseCode = await datalists.Download("Generations");

        // assert
        responseCode.Should().Be(HttpStatusCode.OK);
        datalists.Generations.Should().NotBeEmpty();

        // print
        datalists.Generations.ForEach(value => Console.WriteLine(value.ToString()));
    }
}