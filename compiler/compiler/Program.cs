using Gee.CodeAnalysis;
using Gee.CodeAnalysis.Syntax;

var showTree = false;
var variables = new Dictionary<VariableSymbol, object>();
while (true)
{
    Console.Write("> ");
    var line = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(line))
        return;

    switch (line)
    {
        case "#st":
            showTree = !showTree;
            Console.WriteLine(showTree ? "Showing parse: trees" : "Not showing parse tree");
            continue;
        case "clear" or "cls":
            Console.Clear();
            continue;
    }

    var syntaxTree = SyntaxTree.Parse(line);
    var compilation = new Compilation(syntaxTree);
    var result = compilation.Evaluate(variables);

    var diagnostics = result.Diagnostics;
    if (showTree)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        PrettyPrint(syntaxTree.Root);
        Console.ResetColor();
    }

    if (!diagnostics.Any())
        Console.WriteLine(result.Value);
    else
    {
        foreach (var diagnostic in diagnostics)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(diagnostic);
            Console.ResetColor();

            var prefix = line[..diagnostic.Span.Start];
            var error = line[diagnostic.Span.Start..diagnostic.Span.End];
            var suffix = line[diagnostic.Span.End..];

            Console.Write("    " + prefix);

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(error);
            Console.ResetColor();

            Console.Write(suffix);
            Console.WriteLine();
        }
    }
}

static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
{
    var marker = isLast ? "└──" : "├──";
    Console.Write(indent);
    Console.Write(marker);
    Console.Write(node.Kind);

    if (node is SyntaxToken { Value: not null } t)
        Console.Write($" {t.Value}");


    Console.WriteLine();
    indent += isLast ? "   " : "│  ";

    var lastChild = node.GetChildren().LastOrDefault();
    foreach (var child in node.GetChildren())
        PrettyPrint(child, indent, child == lastChild);
}