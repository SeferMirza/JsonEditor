using JsonEditor.Components;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace JsonEditor;

public partial class MainWindow : Window
{
    Dictionary<string, object> _jsonData = [];
    private readonly JsonService _jsonService;
    readonly EditProperty editProperty = new();
    public MainWindow(JsonService jsonService)
    {
        _jsonService = jsonService;

        InitializeComponent();
        ScrollViewer scrollViewer = new();
        scrollViewer.Content = editProperty.Root;
        propertyDetailBorder.Child = scrollViewer;
    }

    private void OpenNewJsonButtonClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new()
        {
            FileName = "Document",
            DefaultExt = ".json",
            Filter = "Json files (.json)|*.json",
            Multiselect = false
        };

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
                _jsonData = _jsonService.OpenJson(filename);

                JsonModelStackPanel jsonModel = new(_jsonData, _jsonService);
                jsonModel.SelectProperty += JsonModel_ValueSelected;
                modelScrollBar.Content= jsonModel.Base;
            }
        }
    }

    private void JsonModel_ValueSelected(string key, object value)
    {
        editProperty.FillTheProperties(key, value);
    }

    private void JsonModel_Update(string key, object value)
    {
        MessageBox.Show("Hi");
        _jsonData[key] = value;
    }
}
