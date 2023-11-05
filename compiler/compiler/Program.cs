// See https://aka.ms/new-console-template for more information

using compiler.CodeAnalysis;
using compiler.CodeAnalysis.Syntax;

var showTree = false;
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
        // case "exit":
        //     Environment.Exit(0);
        //     break;
    }
        
    var syntaxTree = SyntaxTree.Parse(line);
    if (showTree)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        PrettyPrint(syntaxTree.Root);
        Console.ResetColor();
    }
    
    if (syntaxTree.Diagnostics.Any())
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        foreach (var diagnostic in syntaxTree.Diagnostics) 
            Console.WriteLine(diagnostic);
        Console.ResetColor();
    }
    else
    {
        var e = new Evaluator(syntaxTree.Root);
        var result = e.Evaluate();
        Console.WriteLine(result);
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
    indent += isLast ? "   ": "│   ";
    
    var lastChild = node.GetChildren().LastOrDefault();
    foreach (var child in node.GetChildren())
        PrettyPrint(child, indent, child == lastChild);
}