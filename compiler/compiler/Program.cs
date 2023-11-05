﻿// See https://aka.ms/new-console-template for more information

using compiler.CodeAnalysis;
using compiler.CodeAnalysis.Binding;
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
    var binder = new Binder();
    var boundExpression = binder.BindExpression(syntaxTree.Root);
    var diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();
    if (showTree)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        PrettyPrint(syntaxTree.Root);
        Console.ResetColor();
    }
    
    if (!diagnostics.Any())
    {
        var e = new Evaluator(boundExpression);
        var result = e.Evaluate();
        Console.WriteLine(result);
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        foreach (var diagnostic in diagnostics) 
            Console.WriteLine(diagnostic);
        Console.ResetColor();
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