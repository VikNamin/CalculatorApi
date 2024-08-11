namespace CalculatorApi.Services
{
    public interface CalculatorInterface
    {
        double Add(double a, double b);
        double Subtract(double a, double b);
        double Multiply(double a, double b);
        double Divide(double a, double b);
        double Power(double a, double b);
        double Root(double a, double b);
        double EvaluateExpression(string expression);
    }
}
