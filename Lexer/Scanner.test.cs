using NUnit.Framework;
using Lusio.Lexer;
namespace Lusio.Lexer;

[TestFixture]
public class ScannerTests
{
    [Test]
    public void ScanTokens_SingleLineComment_NoTokens()
    {
        // Arrange
        string source = "// This is a comment";
        var scanner = new Scanner(source);

        // Act
        List<Token> tokens = scanner.scanTokens();

        // Assert
        Assert.AreEqual(1, tokens.Count);
        Assert.AreEqual(TokenType.EOF, tokens[0].type);
    }

    [Test]
    public void ScanTokens_StringLiteral_Success()
    {
        // Arrange
        string source = "\"hello, world\"";
        var scanner = new Scanner(source);

        // Act
        List<Token> tokens = scanner.scanTokens();

        // Assert
        Assert.AreEqual(2, tokens.Count);
        Assert.AreEqual(TokenType.STRING, tokens[0].type);
        Assert.AreEqual("hello, world", tokens[0].lexeme);
        Assert.AreEqual(TokenType.EOF, tokens[1].type);
    }

    [Test]
    public void ScanTokens_NumberLiteral_Success()
    {
        // Arrange
        string source = "123.45";
        var scanner = new Scanner(source);

        // Act
        List<Token> tokens = scanner.scanTokens();

        // Assert
        Assert.AreEqual(2, tokens.Count);
        Assert.AreEqual(TokenType.NUMBER, tokens[0].type);
        Assert.AreEqual(123.45, tokens[0].literal);
        Assert.AreEqual(TokenType.EOF, tokens[1].type);
    }

    [Test]
    public void ScanTokens_Identifier_Success()
    {
        // Arrange
        string source = "x = 42;";
        var scanner = new Scanner(source);

        // Act
        List<Token> tokens = scanner.scanTokens();

        // Assert
        Assert.AreEqual(5, tokens.Count);
        Assert.AreEqual(TokenType.IDENTIFIER, tokens[0].type);
        Assert.AreEqual("x", tokens[0].lexeme);
        Assert.AreEqual(TokenType.EQUAL, tokens[1].type);
        Assert.AreEqual(TokenType.NUMBER, tokens[2].type);
        Assert.AreEqual(42.0, tokens[2].literal);
        Assert.AreEqual(TokenType.SEMICOLON, tokens[3].type);
        Assert.AreEqual(TokenType.EOF, tokens[4].type);

    }
}
