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

        PopulateGridWithDictionary(_jsonData, propertiesStackPanel, 0);
    }

    private void PopulateGridWithDictionary(Dictionary<string, object> data, StackPanel stackPanel, int indent = 0)
    {
        foreach (var kvp in data)
        {
            Label indentLabel = new()
            {
                Content = new string(' ', indent)
            };
            Grid wrapper = new();
            wrapper.ColumnDefinitions.Add(new() { Width = new GridLength(5 * (indent + 1), GridUnitType.Star) });
            wrapper.ColumnDefinitions.Add(new() { Width = new GridLength(45 - indent, GridUnitType.Star) });
            wrapper.ColumnDefinitions.Add(new() { Width = new GridLength(3, GridUnitType.Star) });
            wrapper.ColumnDefinitions.Add(new() { Width = new GridLength(40 - (indent * 4), GridUnitType.Star) });
            wrapper.ColumnDefinitions.Add(new() { Width = new GridLength(12, GridUnitType.Star) });
            wrapper.RowDefinitions.Add(new());
            wrapper.RowDefinitions.Add(new());

            Grid.SetColumn(indentLabel, 0);
            Grid.SetRow(indentLabel, 0);
            wrapper.Children.Add(indentLabel);

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
            Grid.SetRow(keyLabel, 0);
            Grid.SetColumn(keyLabel, 1);
            wrapper.Children.Add(keyLabel);

            Label valueLabel = new()
            {
                Content = ":",
                Width = 15,
                Padding = new Thickness(0, 0, 10, 0),
                HorizontalContentAlignment = HorizontalAlignment.Left
            };
            Grid.SetColumn(valueLabel, 2);
            Grid.SetRow(valueLabel, 0);
            wrapper.Children.Add(valueLabel);

            var typeofProp = kvp.Value.GetType();
            Label typeLabel = new();
            if (typeofProp.Name == typeof(Dictionary<string, object>).Name)
            {
                StackPanel childStackPanel = new();
                Grid.SetRow(childStackPanel, 1);
                Grid.SetColumnSpan(childStackPanel, 5);
                wrapper.Children.Add(childStackPanel);

                typeLabel.Content = typeof(object).Name;
                Button expandButton = new()
                {
                    Content = ">"
                };
                expandButton.Click += (sender, e) =>
                {
                    if (expandButton.Content.ToString() == ">")
                    {
                        PopulateGridWithDictionary((Dictionary<string, object>)kvp.Value, childStackPanel, indent + 1);
                        expandButton.Content = "v";
                    }
                    else
                    {
                        childStackPanel.Children.Clear();
                        expandButton.Content = ">";
                    }
                };
                Grid.SetColumn(expandButton, 4);
                Grid.SetRow(expandButton, 0);
                wrapper.Children.Add(expandButton);
            }
            else
            {
                if (typeofProp.FullName == typeof(List<object>).FullName)
                {
                    StackPanel childStackPanel = new();
                    Grid.SetRow(childStackPanel, 1);
                    Grid.SetColumnSpan(childStackPanel, 5);
                    wrapper.Children.Add(childStackPanel);

                    typeLabel.Content = "List<Object>";
                    Button expandButton = new()
                    {
                        Content = ">"
                    };
                    expandButton.Click += (sender, e) =>
                    {
                        if (expandButton.Content.ToString() == ">")
                        {
                            PopulateGridWithDictionary((Dictionary<string, object>)kvp.Value, childStackPanel, indent + 1);
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

            Grid.SetColumn(typeLabel, 3);
            Grid.SetRow(typeLabel, 0);
            wrapper.Children.Add(typeLabel);

            stackPanel.Children.Add(wrapper);
        }
    }
}
