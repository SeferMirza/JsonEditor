using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace JsonEditor;

public partial class MainWindow : Window
{
    Dictionary<string, object> _jsonData = [];

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OpenNewJsonButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog();
        dialog.FileName = "Document";
        dialog.DefaultExt = ".json";
        dialog.Filter = "Json files (.json)|*.json";
        dialog.Multiselect = false;

        bool? result = dialog.ShowDialog();

        if (result == true)
        {
            string filename = dialog.FileName;

            string caption = "Json file path";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult mboxResult;
            mboxResult = MessageBox.Show($"Is file path '{filename}' correct?", caption, button, icon, MessageBoxResult.Yes);
            if (mboxResult == MessageBoxResult.Yes)
            {
                using StreamReader r = new(filename);
                string jsonString = r.ReadToEnd();

                using var document = JsonDocument.Parse(jsonString);
                var elements = document.RootElement.ParseToDictionary();

                _jsonData = elements;
            }
        }

        ShowJsonModel(_jsonData, propertiesStackPanel, 0);
    }

    private void ShowJsonModel(Dictionary<string, object> data, StackPanel stackPanel, int indent = 0)
    {
        foreach (var kvp in data)
        {
            JsonModelPropertyGrid wrapper = new(columnCount: 5, rowCount: 2);
            wrapper.SetColumnWithWidthPercent(5 * (indent + 1), 40 - indent, 3, 40 - (indent * 4), 12);
            wrapper.SetRows(2);

            Label indentLabel = new()
            {
                Content = new string(' ', indent)
            };
            wrapper[0, 0] = indentLabel;

            Label keyLabel = new()
            {
                Content = kvp.Key,
                Cursor = Cursors.Hand,
                FontWeight = FontWeights.Medium,
                Background = Brushes.Transparent
            };
            keyLabel.MouseEnter += (sender, e) =>
            {
                keyLabel.FontWeight = FontWeights.Bold;
                keyLabel.Background = Brushes.LightGray;
            };
            keyLabel.MouseLeave += (sender, e) =>
            {
                keyLabel.FontWeight = FontWeights.Medium;
                keyLabel.Background = Brushes.Transparent;
            };
            wrapper[1, 0] = keyLabel;

            Label valueLabel = new()
            {
                Content = ":",
                Width = 15,
                Padding = new Thickness(0, 0, 10, 0),
                HorizontalContentAlignment = HorizontalAlignment.Left
            };
            wrapper[2, 0] = valueLabel;

            var typeofProp = kvp.Value.GetType();
            Label typeLabel = new();
            if (typeofProp.Name == typeof(Dictionary<string, object>).Name)
            {
                StackPanel childStackPanel = new();
                wrapper[0, 1] = childStackPanel;
                Grid.SetColumnSpan(childStackPanel, 5);

                typeLabel.Content = typeof(object).Name;
                Button expandButton = new()
                {
                    Content = ">"
                };
                expandButton.Click += (sender, e) =>
                {
                    if (expandButton.Content.ToString() == ">")
                    {
                        ShowJsonModel((Dictionary<string, object>)kvp.Value, childStackPanel, indent + 1);
                        expandButton.Content = "v";
                    }
                    else
                    {
                        childStackPanel.Children.Clear();
                        expandButton.Content = ">";
                    }
                };
                wrapper[4, 0] = expandButton;
            }
            else
            {
                if (typeofProp.FullName == typeof(List<object>).FullName)
                {
                    StackPanel childStackPanel = new();
                    wrapper[0, 1] = childStackPanel;
                    Grid.SetColumnSpan(childStackPanel, 5);

                    typeLabel.Content = "List<Object>";
                    Button expandButton = new()
                    {
                        Content = ">"
                    };
                    expandButton.Click += (sender, e) =>
                    {
                        if (expandButton.Content.ToString() == ">")
                        {
                            ShowJsonModel((Dictionary<string, object>)kvp.Value, childStackPanel, indent + 1);
                            expandButton.Content = "v";
                        }
                        else
                        {
                            childStackPanel.Children.Clear();
                            expandButton.Content = ">";
                        }
                    };
                }
                else if (typeofProp.FullName == typeof(List<object>).FullName)
                {
                    typeLabel.Content = "List<string>";
                }
                else if (typeofProp.FullName == typeof(List<int>).FullName)
                {
                    typeLabel.Content = "List<int>";
                }
                else
                {
                    typeLabel.Content = typeofProp.Name;
                }
            }

            wrapper[3, 0] = typeLabel;

            stackPanel.Children.Add(wrapper.GetGrid);
        }
    }
}
