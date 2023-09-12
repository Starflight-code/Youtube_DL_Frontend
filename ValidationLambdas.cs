using System.Text.RegularExpressions;

namespace Youtube_DL_Frontend
{
    internal class ValidationLambdas
    {
        public static Func<string, bool> exampleLambda = (x) =>
        {
            if (!x.Contains("lm"))
            {
                return false;
            }
            else
            {
                return true;
            }
        };

        public static Func<string, bool> isNumber = (x) =>
        {
            return Int32.TryParse(x, out _);
        };

        public static Func<string, bool> yesOrNo = (x) =>
        {
            return x == "y" ? true : x == "n" ? true : false;
        };
    }
}
