using System;
using System.Data;
using NCalc;
namespace MauiApp2
{
    public partial class MainPage : ContentPage
    {
        private double _currentValue = 0;
        private string _currentExpression = "";
        private bool _isNewNumber = true;
        private bool _hasDecimal = false;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnNumberClicked(object sender, EventArgs e)
        {
            if(_currentExpression == "")
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

            /**/ExpressionLabel.Text = _currentExpression;

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
                /**/ExpressionLabel.Text = _currentExpression + " = ";
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
                var expr = new Expression("1.0*" + _currentExpression);
                // NCalc correctly interprets numbers to avoid overflow and integer division issues.
                var result = expr.Evaluate();
                // The result will be of the correct type (e.g., double, long, int)
                _currentValue = Convert.ToDouble(result);

                DisplayLabel.Text = FormatNumber(_currentValue);
                if(DisplayLabel.Text.Contains("."))
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
            catch (Exception e)
            {
                DisplayLabel.Text = "Error";
                ExpressionLabel.Text = $"{e.Message}";
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
            //// add a subtle animation for button
            //MainThread.BeginInvokeOnMainThread(() => 
            //{
            //    activeButton.ScaleTo(1.1, 100).ContinueWith(_ =>
            //    {
            //        activeButton.ScaleTo(1.0, 100);
            //    });
            //});

            // Alternatively, you can use a simple fade effect
            activeButton.ScaleTo(1.1, 100).ContinueWith(_ =>
                activeButton.ScaleTo(1.0, 100));
        }
    }
}
