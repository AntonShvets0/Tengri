using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TengriLang.Exceptions;
using TengriLang.Language.Model;
using TengriLang.Language.Model.Lexeme;
using TengriLang.Reader;
using TengriLang.Reader.Model;

namespace TengriLang.Language
{
    public class Lexer
    {
        private StringReader _reader;
        private string _file;

        private string _operatorChars = "+-*/%=|&<>^";
        private string _specialChars = "(){}[],.:@";
        private string _keywords = "if else elif for while do in static var dynamic return import this break continue export fun try catch finally";

        public Lexer(string file, string content)
        {
            _file = file;
            _reader = new StringReader(content);
        }
        
        public List<TreeElement> GetTokens()
        {
            

            var tokens = new List<TreeElement>();

            while (!_reader.IsEmpty())
            {
                var token = GetToken(_reader.Read());
                if (token == null) break;
                tokens.Add(token);
            }

            return tokens;
        }

        private TreeElement GetToken(char ch)
        {
            if (ch == '\0') return null;

            if (ch == '\n')
            {
                _reader.Next();
                return new NewLineLexeme(_file, _reader);
            }
            
            // If whitespace
            if (ch == '\r' || ch == '\t' || ch == ' ')
            {
                _reader.Next();
                return GetToken(_reader.Read());
            }
            
            // If comment
            if (ch == '/' && _reader.Read(1) == '/')
            {
                ReadToEndLine();
                return GetToken(_reader.Read());
            }

            if (ch == '/' && _reader.Read(1) == '*')
            {
                ReadToEndComment();
                return GetToken(_reader.Read());
            }

            if (ch == '"')
            {
                _reader.Next();
                return ReadString();
            }

            if (_operatorChars.Contains(ch.ToString()))
            {
                return ReadOperator();
            }

            if (_specialChars.Contains(ch.ToString()))
            {
                _reader.Next();
                return new SpecialLexeme(ch.ToString(), _file, _reader);
            }
            if (int.TryParse(ch.ToString(), out var result)) return ReadNumeric();
            if (Regex.IsMatch(ch.ToString(), "^([a-zA-Z_]*)$")) return ReadKeyword();

            throw new TokenizerException(
                _file, _reader.Line, 
                _reader.CharIndex, 
                $"Unknown char '{ch}' in current context"
                );
        }

        private TreeElement ReadOperator()
        {
            var data = _reader.ReadWhile(ch =>
            {
                if (_operatorChars.Contains(ch.ToString())) return ch;
                return false;
            });
            
            return new OperatorLexeme(data, _file, _reader);
        }

        private TreeElement ReadKeyword()
        {
            var data = _reader.ReadWhile(ch =>
            {
                if (Regex.IsMatch(ch.ToString(), "^([a-zA-Z0-9_]*)$")) return ch;
                return false;
            });

            var keywords = _keywords.Split(' ');
            if (keywords.Contains(data)) return new KeywordLexeme(data, _file, _reader);
            if (data == "false" || data == "true") return new BoolLexeme(data == "true", _file, _reader);
            if (data == "null") return new NullLexeme(_file, _reader);
            
            return new VariableLexeme(data, _file, _reader);
        }

        private TreeElement ReadNumeric()
        {
            var isDotUsed = false;
            var isFloat = false;
            var isLong = false;
            
            var data = _reader.ReadWhile(ch =>
            {
                if (ch == '.' && !isDotUsed)
                {
                    if (!int.TryParse(_reader.Read(1).ToString(), out var intResult)) return false;
                    isDotUsed = true;
                    return ch;
                }

                if (ch == 'f' && !int.TryParse(_reader.Read(1).ToString(), out var floatResult))
                {
                    isFloat = true;
                    
                    return true;
                }
                
                if (ch == 'f' && !int.TryParse(_reader.Read(1).ToString(), out var longResult))
                {
                    isLong = true;
                    
                    return true;
                }

                if (int.TryParse(ch.ToString(), out int result)) return ch;
                return false;
            });

            if (isLong) return new LongLexeme(long.Parse(data), _file, _reader);
            if (isFloat) return new FloatLexeme(float.Parse(data), _file, _reader);

            return new IntegerLexeme(int.Parse(data), _file, _reader);
        }

        private TreeElement ReadString()
        {
            var isEscaped = false;
            var data = _reader.ReadWhile((ch) =>
            {
                if (ch == '"' && !isEscaped)
                {
                    return false;
                }

                if (ch == '\\' && !isEscaped)
                {
                    isEscaped = true;
                    return true;
                }

                if (isEscaped)
                {
                    isEscaped = false;
                    return new ReadWhileResponse(true, $"\\{ch}");
                }

                return ch;
            });
            _reader.Next();

            return new StringLexeme(data, _file, _reader);
        }

        private void ReadToEndLine()
        {
            _reader.ReadWhile((ch) =>
            {
                if (ch != '\n' || ch != '\r') return true;
                return false;
            });
        }

        private void ReadToEndComment()
        {
            _reader.Next(2);
            var startComment = false;

            _reader.ReadWhile((ch) =>
            {
                if (ch == '*' && !startComment)
                {
                    startComment = true;
                    return true;
                }

                if (ch == '/' && startComment)
                {
                    _reader.Next();
                    return false;
                }

                startComment = false;
                return true;
            });
        }
    }
}