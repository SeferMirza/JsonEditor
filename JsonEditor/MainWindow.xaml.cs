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
                var element = document.RootElement.ParseToDictionary();

                _jsonData = element;
            }
        }

        addNewPropertyToModel.IsEnabled = true;
    }

    private void AddNewPropertyToModel(object sender, RoutedEventArgs e)
    {
    }
}
