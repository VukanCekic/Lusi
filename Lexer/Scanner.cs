    namespace Lusio.Lexer
    {
        public class Scanner {
            
            private readonly string _source;
            private readonly List<Token> _tokens = new List<Token>();
            
            private int _start = 0;
            private int _current = 0;
            private int _line = 1;
            
            private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType> {
                { "and", TokenType.AND },
                { "class", TokenType.CLASS },
                { "else", TokenType.ELSE },
                { "false", TokenType.FALSE },
                { "for", TokenType.FOR },
                { "method", TokenType.METHOD },
                { "if", TokenType.IF },
                { "null", TokenType.NULL },
                { "or", TokenType.OR },
                { "write", TokenType.WRITE },
                { "return", TokenType.RETURN },
                { "super", TokenType.SUPER },
                { "this", TokenType.THIS },
                { "true", TokenType.TRUE },
                { "variable", TokenType.VARIABLE },
                { "while", TokenType.WHILE }
            };
            
            public Scanner(string source) {
                _source = source;
            }
            
            private char advance()
            {
                var nextChar = _source[_current++];
                return nextChar;
            }

            private void addToken(TokenType type) {
                addToken(type, null);
            }

            private void addToken(TokenType type, Object? literal) {
                 string text = _source.Substring(_start, _current - _start);
                _tokens.Add(new Token(type, text, literal, _line));
            }
            
            private void scanToken() {
                char c = advance();
                
                switch (c) {
                    
                    case '(': addToken(TokenType.LEFT_PARENT); break;
                    case ')': addToken(TokenType.RIGHT_PARENT); break;
                    case '{': addToken(TokenType.LEFT_BRACE); break;
                    case '}': addToken(TokenType.RIGHT_BRACE); break;
                    case ',': addToken(TokenType.COMMA); break;
                    case '.': addToken(TokenType.DOT); break;
                    case '-': addToken(TokenType.MINUS); break;
                    case '+': addToken(TokenType.PLUS); break;
                    case ';': addToken(TokenType.SEMICOLON); break;
                    case '*': addToken(TokenType.STAR); break; 
                    
                    case '!':
                        addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                        break;
                    case '=':
                        addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                        break;
                    case '<':
                        addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                        break;
                    case '>':
                        addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                        break;
                    
                    case '/':
                        if (match('/')) {
                            // A comment goes until the end of the line.
                            while (peek() != '\n' && !isAtEnd()) advance();
                        } else {
                            addToken(TokenType.SLASH);
                        }
                        break;
                    
                    case ' ':
                    case '\r':
                    case '\t':
                        break;
                    
                    case '\n':
                        _line++;
                        break;
                    
                    case '"': stringLiteral(); break;
                    
                    default:
                        if (isDigit(c)) {
                            number();
                          
                        }    else if (isAlpha(c)) {
                            identifier();
                        }   
                        else {
                            Lusi.error(_line, "Unexpected character.");
                        }
                        break;
                }
            }
            private void identifier() {
                while (isAlphaNumeric(peek())) advance();
                string text = _source.Substring(_start, _current - _start);
                TokenType type = keywords.ContainsKey(text) ? keywords[text] : TokenType.IDENTIFIER;
                addToken(type);
            }
            
            private bool isAlpha(char c) {
                return (c >= 'a' && c <= 'z') ||
                       (c >= 'A' && c <= 'Z') ||
                       c == '_';
            }

            private bool isAlphaNumeric(char c) {
                return isAlpha(c) || isDigit(c);
            }
            private void number() {
                while (isDigit(peek())) advance();

                // Look for a fractional part.
                if (peek() == '.' && isDigit(peekNext())) {
                    // Consume the "."
                    advance();

                    while (isDigit(peek())) advance();
                }

                addToken(TokenType.NUMBER,
                    Double.Parse(_source.Substring(_start, _current - _start)));
            }
            
            private bool isDigit(char c) {
                return c >= '0' && c <= '9';
            } 
            
            private void stringLiteral()
            {
                
                while (peek() != '"' && !isAtEnd())
                {                
                    if (peek() == '\n') _line++;
                    advance();
                }

                if (isAtEnd())
                {
                    Lusi.error(_line,"Unterminated string literal");
                }

                advance();
                
                string text = _source.Substring(_start + 1, _current - 2);
                _tokens.Add(new Token(TokenType.STRING, text, null, _line));

            }


            private char peek() {
                if (isAtEnd()) return '\0';
                return _source[_current];
            }
            
            private char peekNext() {
                if (_current + 1 >= _source.Length) return '\0';
                return _source[_current + 1];
            } 
            
            
            private bool match(char expected) {
                if (isAtEnd()) return false;
                if (_source[_current] != expected) return false;

                _current++;
                return true;
            }
            
            private bool isAtEnd()
            {
                return _current >= _source.Length;
            }
            
            public  List<Token> scanTokens() {
                while (!isAtEnd()) {
                    // We are at the beginning of the next lexeme.
                    _start = _current;
                    scanToken();
                }

                _tokens.Add(new Token(TokenType.EOF, "", null, _line));
                return _tokens;
            }
        }

    }

