using Xunit;
using Aloe.Utils.ArgsHelper;

namespace Aloe.Utils.ArgsHelper.Tests;

public class ArgsHelperTests
{
    [Fact]
    public void PreprocessArgs_WithFlagArgs_ShouldAddTrueValue()
    {
        // Arrange
        string[] args = ["--IsFlag"];
        string[] flagArgs = ["--IsFlag"];
        string[] shortArgs = [];

        // Act
        var result = ArgsHelper.PreprocessArgs(args, flagArgs, shortArgs);

        // Assert
        Assert.Equal(["--IsFlag", "true"], result);
    }

    [Fact]
    public void PreprocessArgs_WithShortArgs_ShouldSplitCombinedValue()
    {
        // Arrange
        string[] args = ["-uadmin", "-ppassword"];
        string[] flagArgs = [];
        string[] shortArgs = ["-u", "-p"];

        // Act
        var result = ArgsHelper.PreprocessArgs(args, flagArgs, shortArgs);

        // Assert
        Assert.Equal(["-u", "admin", "-p", "password"], result);
    }

    [Fact]
    public void PreprocessArgs_WithMixedArgs_ShouldProcessCorrectly()
    {
        // Arrange
        string[] args = ["--IsFlag", "-uadmin", "--OtherFlag", "value"];
        string[] flagArgs = ["--IsFlag", "--OtherFlag"];
        string[] shortArgs = ["-u"];

        // Act
        var result = ArgsHelper.PreprocessArgs(args, flagArgs, shortArgs);

        // Assert
        Assert.Equal(["--IsFlag", "true", "-u", "admin", "--OtherFlag", "value"], result);
    }

    [Fact]
    public void PreprocessArgs_WithNullArgs_ShouldThrowArgumentNullException()
    {
        // Arrange
        string[] args = null!;
        string[] flagArgs = [];
        string[] shortArgs = [];

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => ArgsHelper.PreprocessArgs(args, flagArgs, shortArgs));
    }
} 