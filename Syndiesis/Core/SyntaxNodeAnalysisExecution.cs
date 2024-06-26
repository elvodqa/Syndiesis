﻿using Microsoft.CodeAnalysis.CSharp;
using Syndiesis.Controls.SyntaxVisualization.Creation;
using System.Threading;
using System.Threading.Tasks;

namespace Syndiesis.Core;

public class SyntaxNodeAnalysisExecution : IAnalysisExecution
{
    public NodeLineCreationOptions NodeLineOptions { get; set; } = new();

    public Task<AnalysisResult> Execute(
        string source,
        CancellationToken token)
    {
        var creator = new NodeLineCreator(NodeLineOptions);

        var syntaxTree = CSharpSyntaxTree.ParseText(source, cancellationToken: token);
        if (token.IsCancellationRequested)
            return Task.FromCanceled<AnalysisResult>(token);

        var compilationUnitRoot = syntaxTree.GetCompilationUnitRoot(token);
        if (token.IsCancellationRequested)
            return Task.FromCanceled<AnalysisResult>(token);

        var nodeRoot = creator.CreateRootNode(compilationUnitRoot);
        if (token.IsCancellationRequested)
            return Task.FromCanceled<AnalysisResult>(token);

        var result = new SyntaxNodeAnalysisResult(nodeRoot);
        return Task.FromResult<AnalysisResult>(result);
    }
}
