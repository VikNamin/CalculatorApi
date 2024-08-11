using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using NCalc;

namespace CalculatorApi.Services
{
    public class CalculatorService : CalculatorInterface
    {
        // Метод для сложения двух чисел
        public double Add(double a, double b)
        {
            return a + b;
        }

        // Метод для вычитания одного числа из другого
        public double Subtract(double a, double b)
        {
            return a - b;
        }

        // Метод для деления одного числа на другое
        // В случае деления на ноль выбрасывается исключение DivideByZeroException
        public double Divide(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException("Деление на ноль запрещено.");
            return a / b;
        }

        // Метод для умножения двух чисел
        public double Multiply(double a, double b)
        {
            return a * b;
        }

        // Метод для возведения числа в степень
        public double Power(double a, double b)
        {
            return Math.Pow(a, b);
        }

        // Метод для вычисления корня степени b от числа a
        // В случае некорректных аргументов выбрасывается исключение ArgumentException
        public double Root(double a, double b)
        {
            if (a < 0 || b <= 0) throw new ArgumentException("Неверное выражение.");
            return Math.Pow(a, 1.0 / b);
        }

        // Метод для вычисления выражения, переданного в виде строки
        // Используется библиотека NCalc для обработки выражения
        // Если выражение некорректно, выбрасывается исключение ArgumentException
        public double EvaluateExpression(string expression)
        {
            try
            {
                var ncalcExpression = new Expression(FormatExpression(expression));
                var result = ncalcExpression.Evaluate();
                return Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid expression: " + ex.Message);
            }
        }

        // Приватный метод для форматирования выражения
        // В данном методе производится замена всех операторов возведения в степень "^"
        // на функцию Pow(base, exponent)
        private string FormatExpression(string exp)
        {
            while (exp.Contains("^"))
            {
                // Находим позицию оператора возведения в степень "^"
                int indexOfPower = exp.IndexOf('^');
                bool isBaseFound = false;
                int baseEnd = indexOfPower - 1;
                int baseStart = baseEnd;
                int baseBracketCounter = 0;
                string baseExpression = "";

                bool isExponentFound = false;
                int exponentStart = indexOfPower + 1;
                int exponentEnd = exponentStart;
                int exponentBracketCounter = 0;
                string exponentExpression = "";

                // Если база является числом, определяем её начало
                if (char.IsDigit(exp[baseEnd]))
                {
                    while (baseStart != 0 && char.IsDigit(exp[baseStart - 1]))
                    {
                        baseStart--;
                    }
                    baseExpression = exp.Substring(baseStart, baseEnd - baseStart + 1);
                    isBaseFound = true;
                }

                // Если база выражение в скобках, находим её границы
                while (!isBaseFound)
                {
                    baseStart--;
                    if (exp[baseStart].ToString().Equals("("))
                    {
                        if (baseBracketCounter == 0)
                        {
                            baseExpression = exp.Substring(baseStart + 1, baseEnd - baseStart - 1);
                            isBaseFound = true;
                        }
                        else
                        {
                            baseBracketCounter--;
                        }
                    }
                    if (exp[baseStart].ToString().Equals(")"))
                    {
                        baseBracketCounter++;
                    }
                }

                // Если показатель степени является числом, определяем его конец
                if (char.IsDigit(exp[exponentStart]))
                {
                    while (exponentEnd != exp.Length - 1 && char.IsDigit(exp[exponentEnd + 1]))
                    {
                        exponentEnd++;
                    }
                    exponentExpression = exp.Substring(exponentStart, exponentEnd - exponentStart + 1);
                    isExponentFound = true;
                }

                // Если показатель степени выражение в скобках, находим его границы
                while (!isExponentFound)
                {
                    exponentEnd++;
                    if (exp[exponentEnd].ToString().Equals(")"))
                    {
                        if (exponentBracketCounter == 0)
                        {
                            exponentExpression = exp.Substring(exponentStart + 1, exponentEnd - exponentStart - 1);
                            isExponentFound = true;
                        }
                        else
                        {
                            exponentBracketCounter--;
                        }
                    }
                    if (exp[exponentEnd].ToString().Equals("("))
                    {
                        exponentBracketCounter++;
                    }
                }

                // Формируем строку для функции возведения в степень Pow(base, exponent)
                string powString = $"Pow({baseExpression},{exponentExpression})";

                // Заменяем "^" на вызов функции Pow
                exp = exp.Substring(0, baseStart) + powString + exp.Substring(exponentEnd + 1);
            }
            return exp;
        }
    }
}
