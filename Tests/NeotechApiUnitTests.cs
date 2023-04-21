

namespace Tests;

public class NeotechApiUnitTests
{
    private IConfiguration _config;
    public NeotechApiUnitTests(IConfiguration config) => _config = config;

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