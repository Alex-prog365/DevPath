using DevPath.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using DevPath.Services;
using System.Linq;
using System;

namespace DevPath.Services
{
    public static class ContentRenderer
    {
        public static void Render(Panel targetPanel, TopicContent content)
        {
            targetPanel.Children.Clear();

            var contentColumn = new StackPanel
            {
                MaxWidth = 1000,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            targetPanel.Children.Add(contentColumn);

            if (content == null || content.Blocks == null)
                return;

            foreach (var block in content.Blocks)
            {
                if (block.Type == "Title")
                {
                    var titleBlock = new TextBlock
                    {
                        Text = block.Content,
                        FontSize = 30,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 20),
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };

                    contentColumn.Children.Add(titleBlock);
                }
                else if (block.Type == "Code")
                {
                    var codeHeader = new TextBlock
                    {
                        Text = "Code Example",
                        FontSize = 22,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 10, 0, 8)
                    };

                    var codeBorder = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(55, 55, 65)),
                        CornerRadius = new CornerRadius(8),
                        Padding = new Thickness(15),
                        Margin = new Thickness(0, 0, 0, 15),
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };

                    var codeText = new TextBlock
                    {
                        Text = block.Content,
                        FontSize = 18,
                        Foreground = Brushes.White,
                        FontFamily = new FontFamily("Consolas"),
                        TextWrapping = TextWrapping.Wrap
                    };

                    codeBorder.Child = codeText;
                    contentColumn.Children.Add(codeHeader);
                    contentColumn.Children.Add(codeBorder);
                    
                }
                else if (block.Type == "TaskRetype" || block.Type == "TaskSolve")
                {
                    bool isRetypeTask = block.Type == "TaskRetype";
                    bool isSolveTask = block.Type == "TaskSolve";

                    var taskBorder = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(255, 248, 220)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(220, 180, 80)),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(8),
                        Padding = new Thickness(12),
                        Margin = new Thickness(0, 0, 0, 15),
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };

                    var taskPanel = new StackPanel
                    {
                        Margin = new Thickness(0, 4, 0, 0)
                    };

                    var taskHeader = new TextBlock
                    {
                        Text = isRetypeTask ? "Retype Task" : "Solve Task",
                        FontSize = 22,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 10)
                    };

                    var taskText = new TextBlock
                    {
                        Text = block.Content,
                        FontSize = 20,
                        FontWeight = FontWeights.SemiBold,
                        LineHeight = 28,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 12)
                    };

                    var showExampleButton = new Button
                    {
                        Content = isRetypeTask ? "Show example" : "Show solution",
                        Width = 130,
                        Height = 30,
                        Margin = new Thickness(0, 0, 0, 10),
                        IsEnabled = isRetypeTask
                    };

                    var exampleCodeBox = new TextEditor
                    {
                        Text = block.Code ?? "",
                        IsReadOnly = true,
                        ShowLineNumbers = false,
                        SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#"),
                        FontFamily = new FontFamily("Consolas"),
                        FontSize = 15,
                        MinHeight = 70,
                        Margin = new Thickness(0),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Visibility = Visibility.Visible,
                        Background = new SolidColorBrush(Color.FromRgb(226, 230, 236)),
                        Foreground = Brushes.Black
                    };

                    var exampleBorder = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(220, 225, 232)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(150, 160, 175)),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(6),
                        Padding = new Thickness(10),
                        Margin = new Thickness(0, 0, 0, 10),
                        Child = exampleCodeBox,
                        Visibility = Visibility.Collapsed
                    };

                    showExampleButton.Click += (s, e) =>
                    {
                        if (exampleBorder.Visibility == Visibility.Collapsed)
                        {
                            exampleBorder.Visibility = Visibility.Visible;
                            showExampleButton.Content = isRetypeTask ? "Hide example" : "Hide solution";
                        }
                        else
                        {
                            exampleBorder.Visibility = Visibility.Collapsed;
                            showExampleButton.Content = isRetypeTask ? "Show example" : "Show solution";
                        }
                    };

                    var answerBox = new TextEditor
                    {
                        ShowLineNumbers = false,
                        SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#"),
                        FontFamily = new FontFamily("Consolas"),
                        FontSize = 16,
                        MinHeight = 80,
                        Margin = new Thickness(0, 0, 0, 10),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Top,
                        WordWrap = false,
                        Options =
                        {
                            ConvertTabsToSpaces = true,
                            IndentationSize = 4,
                        }
                    };

                    var checkButton = new Button
                    {
                        Content = "Check",
                        Width = 110,
                        Height = 34,
                        Margin = new Thickness(0, 0, 0, 8)
                    };

                    var resultText = new TextBlock
                    {
                        FontSize = 18,
                        FontWeight = FontWeights.SemiBold,
                        Margin = new Thickness(0, 12, 0, 0)
                    };

                    var outputText = new TextBlock
                    {
                        FontSize = 16,
                        Margin = new Thickness(0, 10, 0, 0),
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = Brushes.DarkSlateBlue
                    };

                    var inputText = new TextBlock
                    {
                        FontSize = 16,
                        Margin = new Thickness(0, 6, 0, 0),
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = Brushes.DarkGoldenrod
                    };

                    checkButton.Click += (sender, e) =>
                    {
                       
                        if (isSolveTask)
                        {
                            showExampleButton.IsEnabled = true;
                        }

                        var context = new CodeAnalysisContext
                        {
                            UserCode = answerBox.Text,
                            ExpectedCode = block.ExpectedAnswer,
                            TaskType = block.Type,
                            TopicTitle = "",
                            RequiredVariableName = block.RequiredVariableName ?? "",
                            RequiredVariableType = block.RequiredVariableType ?? "",
                            RequiredVariableValue = block.RequiredVariableValue ?? "",
                            ValidationProfile = block.ValidationProfile ?? "",
                            FakeInput = block.FakeInput ?? "",
                            ExecutionMode = Enum.TryParse<ExecutionMode>(
                                block.ExecutionMode,
                                true,
                                out var executionMode
                            )
                                ? executionMode
                                : ExecutionMode.MainBody
                        };

                        var result = CodeValidator.Validate(context);

                        resultText.Text = result.Details != null && result.Details.Any()
                             ? string.Join("\n", result.Details) : result.Message; 

                        resultText.Foreground = result.IsPassed
                            ? Brushes.DarkGreen
                            : Brushes.DarkRed;

                        if (result.ExecutionOutput != null && result.ExecutionOutput.Any())
                        {
                            outputText.Text = "Program Output:\n" + string.Join("\n", result.ExecutionOutput);
                        }
                        else
                        {
                            outputText.Text = "";
                        }
                    };

                    taskPanel.Children.Add(taskHeader);
                    taskPanel.Children.Add(taskText);
                    taskPanel.Children.Add(showExampleButton);
                    taskPanel.Children.Add(exampleBorder);
                    taskPanel.Children.Add(answerBox);
                    taskPanel.Children.Add(checkButton);
                    taskPanel.Children.Add(resultText);
                    taskPanel.Children.Add(outputText);
                    taskPanel.Children.Add(inputText);

                    taskBorder.Child = taskPanel;
                    contentColumn.Children.Add(taskBorder);
                }

                else if (block.Type == "Tip")
                {
                    var tipBorder = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(230, 245, 255)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(120, 180, 230)),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(8),
                        Padding = new Thickness(12),
                        Margin = new Thickness(0, 0, 0, 16)
                    };

                    var tipText = new TextBlock
                    {
                        Text = block.Content,
                        FontSize = 17,
                        TextWrapping = TextWrapping.Wrap
                    };

                    tipBorder.Child = tipText;
                    contentColumn.Children.Add(tipBorder);
                }

                else if (block.Type == "Warning")
                {
                    var warningBorder = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(255, 240, 230)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(230, 120, 80)),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(8),
                        Padding = new Thickness(12),
                        Margin = new Thickness(0, 0, 0, 16)
                    };

                    var warningText = new TextBlock
                    {
                        Text = block.Content,
                        FontSize = 17,
                        TextWrapping = TextWrapping.Wrap
                    };

                    warningBorder.Child = warningText;
                    contentColumn.Children.Add(warningBorder);
                }

                else if (block.Type == "Note")
                {
                    var noteBorder = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(245, 245, 245)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(8),
                        Padding = new Thickness(12),
                        Margin = new Thickness(0, 0, 0, 16)
                    };

                    var noteText = new TextBlock
                    {
                        Text = block.Content,
                        FontSize = 17,
                        TextWrapping = TextWrapping.Wrap
                    };

                    noteBorder.Child = noteText;
                    contentColumn.Children.Add(noteBorder);
                }

                else if (block.Type == "Step")
                {
                    var stepText = new TextBlock
                    {
                        Text = block.Content,
                        FontSize = 18,
                        FontWeight = FontWeights.SemiBold,
                        Margin = new Thickness(0, 10, 0, 10),
                        TextWrapping = TextWrapping.Wrap
                    };

                    contentColumn.Children.Add(stepText);
                }

                else if (block.Type == "Image")
                {
                    var image = new Image
                    {
                        Source = new BitmapImage(new Uri(block.Content, UriKind.RelativeOrAbsolute)),
                        Margin = new Thickness(0, 10, 0, 16),
                        Height = 300,
                        Stretch = Stretch.Uniform
                    };

                    contentColumn.Children.Add(image);
                }

                else if (block.Type == "Divider")
                {
                    var line = new Separator
                    {
                        Margin = new Thickness(0, 10, 0, 16)
                    };

                    contentColumn.Children.Add(line);
                }


                else
                {
                    var textBlock = new TextBlock
                    {
                        Text = block.Content,
                        FontSize = 22,
                        LineHeight = 28,
                        Foreground = new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                        Margin = new Thickness(0, 0, 0, 18),
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };

                    contentColumn.Children.Add(textBlock);
                }
            }
        }
    }

}
