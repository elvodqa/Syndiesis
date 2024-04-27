using Avalonia.Controls;
using Avalonia.Input;
using Syndiesis.Controls;
using Syndiesis.ViewModels;
using System;

namespace Syndiesis.Views;

public partial class MainView : UserControl
{
    public readonly AnalysisPipelineHandler AnalysisPipelineHandler = new();

    public readonly MainWindowViewModel ViewModel = new();

    public MainView()
    {
        InitializeComponent();
        InitializeEvents();

        Focusable = true;
    }

    private void InitializeEvents()
    {
        codeEditor.CodeChanged += TriggerPipeline;
        syntaxTreeView.listView.HoveredNode += HandleHoveredNode;
        syntaxTreeView.RegisterAnalysisPipelineHandler(AnalysisPipelineHandler);
    }

    private void HandleHoveredNode(SyntaxTreeListNode? obj)
    {
        codeEditor.ShowHoveredSyntaxNode(obj);
    }

    private void TriggerPipeline()
    {
        var currentSource = ViewModel.Editor.FullString();
        AnalysisPipelineHandler.InitiateAnalysis(currentSource);
    }

    public void Reset()
    {
        const string defaultCode = """
            #define SYNDIESIS

            using System;

            namespace Example;

            public class Program
            {
                public static void Main(string[] args)
                {
                    // using conditional compilation symbols is fun
                    const string greetings =
            #if SYNDIESIS
                        "Hello Syndiesis!"
            #else
                        "Hello World!"
            #endif
                        ;
                    Console.WriteLine(greetings);
                }
            }

            """;

        var analysisPipelineHandler = AnalysisPipelineHandler;
        var viewModel = ViewModel;

        codeEditor.Editor = viewModel.Editor;
        var previousDelay = analysisPipelineHandler.UserInputDelay;
        analysisPipelineHandler.UserInputDelay = TimeSpan.Zero;
        codeEditor.SetSource(defaultCode);
        analysisPipelineHandler.UserInputDelay = previousDelay;
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
        codeEditor.Focus();
    }
}
