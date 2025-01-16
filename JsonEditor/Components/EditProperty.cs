using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JsonEditor.Components;

public class EditProperty
{
    readonly Grid _grid = new();
    readonly TextBox keyTextBox = new()
    {
        HorizontalAlignment = HorizontalAlignment.Left,
        TextWrapping = TextWrapping.Wrap,
        VerticalAlignment = VerticalAlignment.Center,
        VerticalContentAlignment = VerticalAlignment.Center,
        Width = 120,
        Margin = new Thickness(5, 10, 0, 10),
        IsEnabled = false
    };
    readonly TextBox valueTextBox = new()
    {
        HorizontalAlignment = HorizontalAlignment.Left,
        TextWrapping = TextWrapping.Wrap,
        VerticalAlignment = VerticalAlignment.Center,
        VerticalContentAlignment = VerticalAlignment.Center,
        Width = 120,
        Margin = new Thickness(5, 10, 0, 10),
        IsEnabled = false
    };
    readonly WrapPanel wrapPanel = new()
    {
        Orientation = Orientation.Horizontal,
    };
    readonly Button saveBtn = new()
    {
        Content = "Save",
        HorizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment = VerticalAlignment.Center,
        FontSize = 15,
        IsEnabled = false,
        Width = 50,
        Height = 25,
        Margin = new Thickness(10, 10, 0, 10)
    };

    public EditProperty()
    {
        Default();
    }

    void Default()
    {
        _grid.RowDefinitions.Clear();
        _grid.ColumnDefinitions.Clear();
        _grid.Children.Clear();

        keyTextBox.IsEnabled = false;
        keyTextBox.Text = string.Empty;

        valueTextBox.Text = string.Empty;
        valueTextBox.IsEnabled = false;

        saveBtn.IsEnabled = false;

        wrapPanel.Children.Clear();

        _grid.ColumnDefinitions.Add(new());
        _grid.ColumnDefinitions.Add(new());
        _grid.ColumnDefinitions.Add(new());
        _grid.ColumnDefinitions.Add(new());
        _grid.ColumnDefinitions.Add(new());
        _grid.ColumnDefinitions.Add(new());
        _grid.RowDefinitions.Add(new() { Height = new GridLength(50, GridUnitType.Pixel) });

        _grid.Children.Add(wrapPanel);
        Grid.SetColumnSpan(wrapPanel, 6);
        Grid.SetRow(wrapPanel, 0);

        Label keyLabel = new()
        {
            Content = "Key :",
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(10, 10, 0, 10)
        };
        wrapPanel.Children.Add(keyLabel);

        wrapPanel.Children.Add(keyTextBox);

        Label valueLabel = new()
        {
            Content = "Value :",
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(10, 10, 0, 10)
        };
        wrapPanel.Children.Add(valueLabel);

        wrapPanel.Children.Add(valueTextBox);

        wrapPanel.Children.Add(saveBtn);
    }

    public UIElement Root => _grid;

    void PropertyView(Action<WrapPanel, Label, TextBox, Label, Label> action)
    {
        WrapPanel wrapPanel = new()
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Center,
            UseLayoutRounding = false,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        _grid.Children.Add(wrapPanel);
        Grid.SetColumnSpan(wrapPanel, 6);

        Label keyLabel = new()
        {
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(10, 10, 0, 10)
        };
        wrapPanel.Children.Add(keyLabel);

        TextBox keyTextBox = new()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            TextWrapping = TextWrapping.Wrap,
            VerticalAlignment = VerticalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
            Width = 120,
            Margin = new Thickness(5, 10, 0, 10),
        };
        wrapPanel.Children.Add(keyTextBox);

        Label valueLabel = new()
        {
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(10, 10, 0, 10)
        };
        wrapPanel.Children.Add(valueLabel);

        Label valueTypeLabel = new()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
            Width = 80,
            Background = Brushes.AliceBlue,
            Margin = new Thickness(5, 10, 0, 10)
        };
        wrapPanel.Children.Add(valueTypeLabel);

        action(wrapPanel, keyLabel, keyTextBox, valueLabel, valueTypeLabel);
    }

    public void FillTheProperties(string key, object property)
    {
        Default();
        keyTextBox.IsEnabled = true;
        keyTextBox.Text = key;
        saveBtn.IsEnabled = true;
        valueTextBox.Text = "-";

        if (property is Dictionary<string, object> dic)
        {
            int i = 1;
            foreach ((string dicKey, object value) in dic)
            {
                _grid.RowDefinitions.Add(new() { Height = new GridLength(50, GridUnitType.Pixel) });

                PropertyView((wrapPanel, keyLabel, keyTextBox, valueLabel, valueTypeLabel) =>
                {
                    Grid.SetRow(wrapPanel, i);

                    keyLabel.Content = $"Property {i} :";

                    keyTextBox.Text = dicKey;

                    valueLabel.Content = "Value Type :";

                    valueTypeLabel.Content = value.GetType().GetFriendlyTypeName();
                });

                i++;
            }
        }

        if (property is List<string> strList)
        {
            int i = 1;
            foreach (string str in strList)
            {
                _grid.RowDefinitions.Add(new() { Height = new GridLength(50, GridUnitType.Pixel) });

                PropertyView((wrapPanel, keyLabel, keyTextBox, valueLabel, valueTypeLabel) =>
                {
                    Grid.SetRow(wrapPanel, i);

                    keyLabel.Content = $"Index {i} :";

                    keyTextBox.Text = str;

                    valueLabel.Content = "Value Type :";

                    valueTypeLabel.Content = "String";
                });

                i++;
            }
        }

        if (property is List<int> intList)
        {
            int i = 1;
            foreach (int integer in intList)
            {
                _grid.RowDefinitions.Add(new() { Height = new GridLength(50, GridUnitType.Pixel) });

                PropertyView((wrapPanel, keyLabel, keyTextBox, valueLabel, valueTypeLabel) =>
                {
                    Grid.SetRow(wrapPanel, i);

                    keyLabel.Content = $"Index {i} :";

                    keyTextBox.Text = integer.ToString();

                    valueLabel.Content = "Value Type :";

                    valueTypeLabel.Content = "Integer";
                });

                i++;
            }
        }

        if (property is List<object> objList)
        {
            int i = 1;
            foreach (Dictionary<string, object> obj in objList)
            {
                _grid.RowDefinitions.Add(new());

                WrapPanel wrapPanel = new()
                {
                    Orientation = Orientation.Horizontal,
                    UseLayoutRounding = false,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                _grid.Children.Add(wrapPanel);
                Grid.SetRow(wrapPanel, i);
                Grid.SetColumnSpan(wrapPanel, 6);
                Border border = new()
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new(0, 0, 0, 1),
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 3, 0, 3)
                };
                _grid.Children.Add(border);
                Grid.SetRow(border, i);
                Grid.SetColumnSpan(border, 6);

                Label keyLabel = new()
                {
                    Content = $"Index {i} :",
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 10, 0, 10)
                };
                wrapPanel.Children.Add(keyLabel);

                Grid objGrid = new();
                objGrid.ColumnDefinitions.Add(new() { Width = new GridLength(1, GridUnitType.Star) });
                objGrid.ColumnDefinitions.Add(new() { Width = new GridLength(2, GridUnitType.Star) });
                objGrid.ColumnDefinitions.Add(new() { Width = new GridLength(1, GridUnitType.Star) });
                objGrid.ColumnDefinitions.Add(new() { Width = new GridLength(2, GridUnitType.Star) });

                int j = 0;
                foreach((string objKey, object objValue) in obj)
                {
                    objGrid.RowDefinitions.Add(new());

                    Label objKeyLabel = new()
                    {
                        Content = "Key: ",
                        FontWeight = FontWeights.Bold,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(10, 10, 0, 10)
                    };

                    objGrid.Children.Add(objKeyLabel);
                    Grid.SetColumn(objKeyLabel, 0);
                    Grid.SetRow(objKeyLabel, j);

                    TextBox objKeyTextBox = new()
                    {
                        Text = objKey,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        TextWrapping = TextWrapping.Wrap,
                        VerticalAlignment = VerticalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Width = 120,
                        Margin = new Thickness(5, 10, 0, 10),
                    };
                    objGrid.Children.Add(objKeyTextBox);
                    Grid.SetColumn(objKeyTextBox, 1);
                    Grid.SetRow(objKeyTextBox, j);

                    Label objValueLabel = new()
                    {
                        Content = "Value: ",
                        FontWeight = FontWeights.Bold,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(10, 10, 0, 10)
                    };

                    objGrid.Children.Add(objValueLabel);
                    Grid.SetColumn(objValueLabel, 2);
                    Grid.SetRow(objValueLabel, j);

                    if(objValue is Dictionary<string, object> || objValue is List<int> || objValue is List<string> || objValue is List<int>)
                    {
                        Label objValueTypeLabel = new()
                        {
                            Content = objValue.GetType().GetFriendlyTypeName(),
                            FontWeight = FontWeights.Bold,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(10, 10, 0, 10)
                        };

                        objGrid.Children.Add(objValueTypeLabel);
                        Grid.SetColumn(objValueTypeLabel, 3);
                        Grid.SetRow(objValueTypeLabel, j);
                    }
                    else
                    {
                        TextBox objValueTextBox = new()
                        {
                            Text = objValue.ToString(),
                            HorizontalAlignment = HorizontalAlignment.Left,
                            TextWrapping = TextWrapping.Wrap,
                            VerticalAlignment = VerticalAlignment.Center,
                            VerticalContentAlignment = VerticalAlignment.Center,
                            Width = 120,
                            Margin = new Thickness(5, 10, 0, 10),
                        };
                        objGrid.Children.Add(objValueTextBox);
                        Grid.SetColumn(objValueTextBox, 3);
                        Grid.SetRow(objValueTextBox, j);
                    }

                    j++;
                }
                
                wrapPanel.Children.Add(objGrid);
                
                i++;
            }
        }

        if (property is int|| property is string)
        {
            valueTextBox.IsEnabled = true;
            valueTextBox.Text = property.ToString();
        }
    }

    public void Empty()
    {
        Default();
    }
}
