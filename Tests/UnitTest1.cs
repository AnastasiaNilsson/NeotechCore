

namespace Tests;

public class UnitTest1
{
    [Fact]
    public void ShouldReceiveOkResponseFromGoogle()
    {
        // arrange
        var datalists = new DataLists();

        // act
        var responseCode = datalists.Update("Generations").Result.StatusCode;

        // assert
        responseCode.Should().Be(HttpStatusCode.OK);
    }
}