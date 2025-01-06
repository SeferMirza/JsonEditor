using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace JsonEditor;

public partial class MainWindow : Window
{
    Dictionary<string, object> _jsonData = [];
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Refresh()
    {
        
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

        addNewPropertyToModel.IsEnabled = true;
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

            TextBox keyTextBox = new() { Text = kvp.Key };
            Grid.SetRow(keyTextBox, 0);
            Grid.SetColumn(keyTextBox, 1);
            wrapper.Children.Add(keyTextBox);

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

            var typeofProp = kvp.Value.GetType().Name;
            Label typeLabel = new();
            if (typeofProp == typeof(Dictionary<string, object>).Name)
            {
                typeLabel.Content = typeof(object).Name;
                Button expandButton = new()
                {
                    Content = "..."
                };
                expandButton.Click += (sender, e) =>
                {
                    StackPanel childStackPanel = new();
                    Grid.SetRow(childStackPanel, 1);
                    Grid.SetColumnSpan(childStackPanel, 5);
                    wrapper.Children.Add(childStackPanel);
                    PopulateGridWithDictionary((Dictionary<string, object>)kvp.Value, childStackPanel, indent + 1);
                };
                Grid.SetColumn(expandButton , 4);
                Grid.SetRow(expandButton, 0);
                wrapper.Children.Add(expandButton);
            }
            else
            {
                typeLabel.Content = typeofProp;
            }

            Grid.SetColumn(typeLabel, 3);
            Grid.SetRow(typeLabel, 0);
            wrapper.Children.Add(typeLabel);

            stackPanel.Children.Add(wrapper);
        }
    }
}
