namespace Lusio.Lexer
{
    
    public class Token {
        public readonly TokenType type;
        public readonly string lexeme;
        public readonly object literal;
        public readonly int line;

        public Token(TokenType type, string lexeme, object? literal, int line) {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }
        
        public override string ToString()
        {
            return type + " " + lexeme + " " + literal;
        }
    }
    
    public enum TokenType
    {
        // Single-character tokens.
        LEFT_PARENT, RIGHT_PARENT, LEFT_BRACE, RIGHT_BRACE,
        COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR,
        
        // One or two character tokens.
        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,

        // Literals.
        IDENTIFIER, STRING, NUMBER,

        // Keywords.
        AND, CLASS, ELSE, FALSE, METHOD, FOR, IF, NULL, OR,
        WRITE, RETURN, SUPER, THIS, TRUE, VARIABLE, WHILE,

        EOF
    }
}


