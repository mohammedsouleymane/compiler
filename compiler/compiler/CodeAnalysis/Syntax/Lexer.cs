namespace compiler.CodeAnalysis.Syntax;

internal sealed class Lexer
{
    private readonly string _text;
    private int _position;
    private List<string> _diagnostics = new();

    public Lexer(string text)
    {
        _text = text;
    }

    private char Current => Peek(0);
    private char Lookahead => Peek(1);
    private char Peek(int offset) => _position + offset >= _text.Length ? '\0' : _text[_position + offset];


    private void Next()
    {
        _position++;
    }

    public IEnumerable<string> Diagnostics => _diagnostics;
    public SyntaxToken Lex()
    {
        if (_position >= _text.Length)
            return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
        if (char.IsDigit(Current))
        {
            var start = _position;
            while (char.IsDigit(Current))
                Next();
            var length = _position - start;
            var text = _text.AsSpan(start, length).ToString();
            if (!int.TryParse(text, out var value))
            {
                _diagnostics.Add($"The number {_text} isn't valid Int32");
            }
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
        }

        if (char.IsWhiteSpace(Current))
        {
            var start = _position;
            while (char.IsWhiteSpace(Current))
                Next();
            var length = _position - start;
            var text = _text.AsSpan(start, length).ToString();
            return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
        }

        if (char.IsLetter(Current))
        {
            var start = _position;
            while (char.IsLetter(Current))
                Next();
            var length = _position - start;
            var text = _text.AsSpan(start, length).ToString();
            var kind = SyntaxFacts.GetKeywordKind(text);
            return new SyntaxToken(kind, start, text, null);
        }
        switch (Current)
        {
            case '+':
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
            case '-':
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
            case '*':
                return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
            case '/':
                return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
            case '(':
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
            case ')':
                return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
            case '!':
                return new SyntaxToken(SyntaxKind.BangToken, _position++, "!", null);
            case '&':
                if(Lookahead == '&')
                    return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, _position += 2, "&&", null);
                break;
            case '|':
                if(Lookahead == '|')
                    return new SyntaxToken(SyntaxKind.PipePipeToken, _position += 2, "||", null);
                break;
        }
        _diagnostics.Add($"ERROR: bad character in input: {Current}");
        return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.AsSpan(_position - 1, 1).ToString(), null);
    }
}
