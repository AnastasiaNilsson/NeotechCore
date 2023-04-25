
using Microsoft.Extensions.Configuration;

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
        var responseCode = datalists.Update("Generations").Result.StatusCode;

        // assert
        responseCode.Should().Be(HttpStatusCode.OK);
    }
}