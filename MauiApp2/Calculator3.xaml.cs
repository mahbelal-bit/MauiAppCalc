using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Data;
using NCalc;    

namespace MauiApp2;

public partial class Calculator3 : ContentPage
{
    private double _currentValue = 0;
    private string _currentExpression = "";
    private bool _isNewNumber = true;
    private bool _hasDecimal = false;

    private Label ExpressionLabel = null!;
    private Label DisplayLabel = null!;
    public Calculator3()
    {
	    InitializeComponent();
        Title = "Calculator";
        BackgroundColor = Color.FromArgb("#1e1e1e");

        Content = CreateLayout();
    }
    private View CreateLayout()
    {
        // Create the main grid
        var mainGrid = new Grid
        {
            RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
            ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
        };

        // Create display area
        CreateDisplayArea(mainGrid);

        // Create button rows
        CreateButtonRow1(mainGrid);
        CreateButtonRow2(mainGrid);
        CreateButtonRow3(mainGrid);
        CreateButtonRow4(mainGrid);
        CreateButtonRow5(mainGrid);

        return mainGrid;
    }

    private void CreateDisplayArea(Grid mainGrid)
    {
        var displayBorder = new Border
        {
            BackgroundColor = Color.FromArgb("#2d2d2d"),
            Stroke = Colors.Transparent,
            StrokeThickness = 0,
            Margin = new Thickness(10, 20, 10, 10),
            Padding = new Thickness(20),
            StrokeShape = new RoundRectangle { CornerRadius = 8 }
        };

        var stackLayout = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            VerticalOptions = LayoutOptions.End//?
        };

        ExpressionLabel = new Label
        {
            Text = "",
            FontSize = DeviceInfo.Platform == DevicePlatform.Android ? 20 : 16,
            TextColor = Colors.Gray,
            HorizontalTextAlignment = TextAlignment.End,
            VerticalTextAlignment = TextAlignment.Start,
            LineBreakMode = LineBreakMode.NoWrap
        };

        DisplayLabel = new Label
        {
            Text = "0",
            FontSize = DeviceInfo.Platform == DevicePlatform.Android ? 24 : 48,
            TextColor = Colors.White,
            HorizontalTextAlignment = TextAlignment.End,
            VerticalTextAlignment = TextAlignment.End,
            LineBreakMode = LineBreakMode.NoWrap
        };

        stackLayout.Children.Add(ExpressionLabel);
        stackLayout.Children.Add(DisplayLabel);
        displayBorder.Content = stackLayout;

        Grid.SetRow(displayBorder, 0);
        Grid.SetColumnSpan(displayBorder, 4);
        mainGrid.Children.Add(displayBorder);
    }

    private void CreateButtonRow1(Grid mainGrid)
    {
        // Clear button
        var clearButton = CreateButton("C", Colors.Black, Color.FromArgb("#a6a6a6"), 24, OnClearClicked);
        Grid.SetRow(clearButton, 1);
        Grid.SetColumn(clearButton, 0);
        mainGrid.Children.Add(clearButton);

        // Delete button
        var deleteButton = CreateButton("DEL", Colors.Black, Color.FromArgb("#a6a6a6"), 24, OnDeleteClicked);
        Grid.SetRow(deleteButton, 1);
        Grid.SetColumn(deleteButton, 1);
        mainGrid.Children.Add(deleteButton);

        // Percent button
        var percentButton = CreateButton("%", Colors.Black, Color.FromArgb("#a6a6a6"), 24, OnPercentClicked);
        Grid.SetRow(percentButton, 1);
        Grid.SetColumn(percentButton, 2);
        mainGrid.Children.Add(percentButton);

        // Divide button
        var divideButton = CreateButton("÷", Colors.White, Color.FromArgb("#ff9500"), 32, OnOperatorClicked);
        Grid.SetRow(divideButton, 1);
        Grid.SetColumn(divideButton, 3);
        mainGrid.Children.Add(divideButton);
    }

    private void CreateButtonRow2(Grid mainGrid)
    {
        // Numbers 7, 8, 9
        var button7 = CreateButton("7", Colors.White, Color.FromArgb("#333333"), 32, OnNumberClicked);
        Grid.SetRow(button7, 2);
        Grid.SetColumn(button7, 0);
        mainGrid.Children.Add(button7);

        var button8 = CreateButton("8", Colors.White, Color.FromArgb("#333333"), 32, OnNumberClicked);
        Grid.SetRow(button8, 2);
        Grid.SetColumn(button8, 1);
        mainGrid.Children.Add(button8);

        var button9 = CreateButton("9", Colors.White, Color.FromArgb("#333333"), 32, OnNumberClicked);
        Grid.SetRow(button9, 2);
        Grid.SetColumn(button9, 2);
        mainGrid.Children.Add(button9);

        // Multiply button
        var multiplyButton = CreateButton("×", Colors.White, Color.FromArgb("#ff9500"), 32, OnOperatorClicked);
        Grid.SetRow(multiplyButton, 2);
        Grid.SetColumn(multiplyButton, 3);
        mainGrid.Children.Add(multiplyButton);
    }

    private void CreateButtonRow3(Grid mainGrid)
    {
        // Numbers 4, 5, 6
        var button4 = CreateButton("4", Colors.White, Color.FromArgb("#333333"), 32, OnNumberClicked);
        Grid.SetRow(button4, 3);
        Grid.SetColumn(button4, 0);
        mainGrid.Children.Add(button4);

        var button5 = CreateButton("5", Colors.White, Color.FromArgb("#333333"), 32, OnNumberClicked);
        Grid.SetRow(button5, 3);
        Grid.SetColumn(button5, 1);
        mainGrid.Children.Add(button5);

        var button6 = CreateButton("6", Colors.White, Color.FromArgb("#333333"), 32, OnNumberClicked);
        Grid.SetRow(button6, 3);
        Grid.SetColumn(button6, 2);
        mainGrid.Children.Add(button6);

        // Subtract button
        var subtractButton = CreateButton("-", Colors.White, Color.FromArgb("#ff9500"), 40, OnOperatorClicked);
        Grid.SetRow(subtractButton, 3);
        Grid.SetColumn(subtractButton, 3);
        mainGrid.Children.Add(subtractButton);
    }

    private void CreateButtonRow4(Grid mainGrid)
    {
        // Numbers 1, 2, 3
        var button1 = CreateButton("1", Colors.White, Color.FromArgb("#333333"), 32, OnNumberClicked);
        Grid.SetRow(button1, 4);
        Grid.SetColumn(button1, 0);
        mainGrid.Children.Add(button1);

        var button2 = CreateButton("2", Colors.White, Color.FromArgb("#333333"), 32, OnNumberClicked);
        Grid.SetRow(button2, 4);
        Grid.SetColumn(button2, 1);
        mainGrid.Children.Add(button2);

        var button3 = CreateButton("3", Colors.White, Color.FromArgb("#333333"), 32, OnNumberClicked);
        Grid.SetRow(button3, 4);
        Grid.SetColumn(button3, 2);
        mainGrid.Children.Add(button3);

        // Add button
        var addButton = CreateButton("+", Colors.White, Color.FromArgb("#ff9500"), 32, OnOperatorClicked);
        Grid.SetRow(addButton, 4);
        Grid.SetColumn(addButton, 3);
        mainGrid.Children.Add(addButton);
    }

    private void CreateButtonRow5(Grid mainGrid)
    {
        // Zero button
        var button0 = CreateButton("0", Colors.White, Color.FromArgb("#333333"), 32, OnNumberClicked);
        Grid.SetRow(button0, 5);
        Grid.SetColumn(button0, 0);
        mainGrid.Children.Add(button0);

        // Plus/Minus button
        var plusMinusButton = CreateButton("±", Colors.White, Color.FromArgb("#333333"), 32, OnPlusMinusClicked);
        Grid.SetRow(plusMinusButton, 5);
        Grid.SetColumn(plusMinusButton, 1);
        mainGrid.Children.Add(plusMinusButton);

        // Decimal button
        var decimalButton = CreateButton(".", Colors.White, Color.FromArgb("#333333"), 32, OnDecimalClicked);
        Grid.SetRow(decimalButton, 5);
        Grid.SetColumn(decimalButton, 2);
        mainGrid.Children.Add(decimalButton);

        // Equals button
        var equalsButton = CreateButton("=", Colors.White, Color.FromArgb("#ff9500"), 32, OnEqualsClicked);
        Grid.SetRow(equalsButton, 5);
        Grid.SetColumn(equalsButton, 3);
        mainGrid.Children.Add(equalsButton);
    }

    private Button CreateButton(string text, Color textColor, Color backgroundColor, double fontSize, EventHandler clickHandler)
    {
        var button = new Button
        {
            Text = text,
            TextColor = textColor,
            BackgroundColor = backgroundColor,
            FontSize = fontSize,
            Margin = new Thickness(5)
        };
        button.Clicked += clickHandler;
        return button;
    }

    // Event handlers 
    private void OnNumberClicked(object sender, EventArgs e)
    {
        if (_currentExpression == "")
            ExpressionLabel.Text = "";

        var button = (Button)sender;
        var number = button.Text;

        if (_isNewNumber)
        {
            DisplayLabel.Text = number;
            _isNewNumber = false;
            _hasDecimal = false;
        }
        else
        {
            if (DisplayLabel.Text == "0")
                DisplayLabel.Text = number;
            else
                DisplayLabel.Text += number;
        }

        _currentValue = double.Parse(DisplayLabel.Text);
    }

    private void OnDecimalClicked(object sender, EventArgs e)
    {
        if (_currentExpression == "")
            ExpressionLabel.Text = "";

        if (_hasDecimal) return;

        if (_isNewNumber)
        {
            DisplayLabel.Text = "0.";
            _isNewNumber = false;
        }
        else
        {
            DisplayLabel.Text += ".";
        }
        _currentValue = double.Parse(DisplayLabel.Text);

        _hasDecimal = true;
    }

    private void OnOperatorClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var op = button.Text;
        if (!_isNewNumber || string.IsNullOrEmpty(_currentExpression))
        {
            _currentExpression += $" {_currentValue}";
        }

        if (!char.IsDigit(_currentExpression.Last()))
            _currentExpression = _currentExpression.Substring(0, _currentExpression.Length - 2);
        _currentExpression += $" {op}";

        ExpressionLabel.Text = _currentExpression;

        _isNewNumber = true;
        _hasDecimal = false;

        // animation for the operator button
        HighlightOperatorButton(button);
    }

    private void OnEqualsClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_currentExpression))
        {
            _currentExpression += $" {_currentValue}";
            ExpressionLabel.Text = _currentExpression + " = ";
            PerformCalculation();

            _currentExpression = "";
            _isNewNumber = true;
        }
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        _currentValue = 0;
        _currentExpression = "";
        _isNewNumber = true;
        _hasDecimal = false;
        DisplayLabel.Text = "0";
        ExpressionLabel.Text = "";
    }

    private void OnPlusMinusClicked(object sender, EventArgs e)
    {
        if (_currentExpression == "")
            ExpressionLabel.Text = "";

        if (_currentValue != 0)
        {
            _currentValue = -_currentValue;
            DisplayLabel.Text = FormatNumber(_currentValue);
        }
    }

    private void OnPercentClicked(object sender, EventArgs e)
    {
        if (_currentExpression == "")
            ExpressionLabel.Text = "";

        _currentValue = _currentValue / 100;
        DisplayLabel.Text = FormatNumber(_currentValue);
        _isNewNumber = true;
    }

    private void PerformCalculation()
    {
        _currentExpression = _currentExpression.Trim().Replace("×", "*").Replace("÷", "/");
        
        try
        {
            // Create a new Expression object
            var expr = new Expression(_currentExpression);
            // NCalc correctly interprets numbers to avoid overflow and integer division issues.
            var result = expr.Evaluate();
            // The result will be of the correct type (e.g., double, long, int)
            _currentValue = Convert.ToDouble(result);

            DisplayLabel.Text = FormatNumber(_currentValue);
            if (DisplayLabel.Text.Contains("."))
                _hasDecimal = true;
            else
                _hasDecimal = false;
        }
        catch (DivideByZeroException)
        {
            DisplayLabel.Text = "Error";
            ExpressionLabel.Text = "Cannot divide by zero";
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(1500);
                OnClearClicked(null, null);
            });
        }
        catch (Exception)
        {
            DisplayLabel.Text = "Error";
            ExpressionLabel.Text = "Invalid operation";
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(1500);
                OnClearClicked(null, null);
            });
        }
    }

    private string FormatNumber(double number)
    {
        // Handle very large or very small numbers
        if (Math.Abs(number) >= 1e10 || (Math.Abs(number) <= 1e-6 && number != 0))
        {
            return number.ToString("E2");
        }

        // Remove unnecessary decimal places
        if (number == Math.Floor(number))
        {
            return number.ToString("F0");
        }

        // Limit decimal places to prevent overflow
        return number.ToString("G9");
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (!_isNewNumber)//?
        {
            int len = DisplayLabel.Text.Length;
            if (len > 1)
            {
                DisplayLabel.Text = DisplayLabel.Text.Remove(len - 1);
                if (DisplayLabel.Text != "-" && DisplayLabel.Text != "-0")
                    _currentValue = double.Parse(DisplayLabel.Text);
                else
                    len = 1;
            }
            if (len == 1)
            {
                _isNewNumber = true;
                _hasDecimal = false;
                DisplayLabel.Text = "0";
                _currentValue = 0;
            }
        }
    }

    private void HighlightOperatorButton(Button activeButton)
    {
        // Alternatively, you can use a simple fade effect
        activeButton.ScaleTo(1.1, 100).ContinueWith(_ =>
            activeButton.ScaleTo(1.0, 100));
    }
}
