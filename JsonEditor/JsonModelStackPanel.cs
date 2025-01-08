using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace JsonEditor;

public class JsonModelStackPanel
{
    readonly StackPanel _base = new();

    public StackPanel Base => _base;

    public JsonModelStackPanel(Dictionary<string, object> _json)
    {
        GenerateModel(_json, _base, 0);
    }

    void GenerateModel(Dictionary<string, object> data, StackPanel stackPanel, int indent = 0)
    {
        foreach ((string key, object value) in data)
        {
            JsonModelPropertyGrid wrapper = new(columnCount: 5, rowCount: 2);
            wrapper.SetColumnWithWidthPercent(5 * (indent + 1), 40 - indent, 3, 40 - (indent * 4), 12);
            wrapper.SetRows(2);

            StackPanel childStackPanel = new();
            wrapper[0, 1] = childStackPanel;
            Grid.SetColumnSpan(childStackPanel, 5);

            Label indentLabel = new()
            {
                Content = new string(' ', indent)
            };
            wrapper[0, 0] = indentLabel;

            Label keyLabel = new()
            {
                Content = key,
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


            void ExpandButton(Dictionary<string, object> json)
            {
                Button expandButton = new()
                {
                    Content = ">"
                };
                expandButton.Click += (sender, e) =>
                {
                    if (expandButton.Content.ToString() == ">")
                    {
                        GenerateModel(json, childStackPanel, indent + 1);
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

            Label typeLabel = new();
            if (value is Dictionary<string, object> dic)
            { 
                typeLabel.Content = "Object";
                ExpandButton(dic);
            }
            else if (value is List<object> list)
            {
                typeLabel.Content = "List<Object>";
                var newDic = new Dictionary<string, object>();
                newDic["undefined"] = list[0];
                ExpandButton(newDic);
            }
            else
            {
                typeLabel.Content = value.GetType().GetFriendlyTypeName();
            }

            wrapper[3, 0] = typeLabel;

            stackPanel.Children.Add(wrapper.Base);
        }
    }
}
