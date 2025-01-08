using System.Windows;
using System.Windows.Controls;

namespace JsonEditor;

public class JsonModelPropertyGrid
{
    readonly Grid _base;
    readonly UIElement[,] _elements;

    public JsonModelPropertyGrid(int columnCount = 1, int rowCount = 1)
    {
        _base = new Grid();
        _elements = new UIElement[columnCount, rowCount];
    }

    public Grid Base => _base;

    public void SetColumnWithWidthPercent(params int[] percents)
    {
        for (int i = 0; i < percents.Length; i++)
        {
            _base.ColumnDefinitions.Add(new() { Width = new GridLength(percents[i], GridUnitType.Star) });
        }
    }

    public void SetRowWithHeightPercent(params int[] percents)
    {
        for (int i = 0; i < percents.Length; i++)
        {
            _base.RowDefinitions.Add(new() { Height = new GridLength(percents[i], GridUnitType.Star) });
        }
    }

    public void SetRows(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _base.RowDefinitions.Add(new());
        }
    }

    public void SetColumns(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _base.ColumnDefinitions.Add(new());
        }
    }

    public UIElement this[int columnIndex, int rowIndex]
    {
        get
        {
            if (columnIndex < 0 || columnIndex >= _elements.GetLength(0) ||
                rowIndex < 0 || rowIndex >= _elements.GetLength(1))
            {
                throw new IndexOutOfRangeException("Index out of range.");
            }

            return _elements[columnIndex, rowIndex];
        }
        set
        {
            if (columnIndex < 0 || columnIndex >= _elements.GetLength(0) ||
                rowIndex < 0 || rowIndex >= _elements.GetLength(1))
            {
                throw new IndexOutOfRangeException("Index out of range.");
            }

            Grid.SetColumn(value, columnIndex);
            Grid.SetRow(value, rowIndex);
            _base.Children.Add(value);
            _elements[columnIndex, rowIndex] = value;
        }
    }
}