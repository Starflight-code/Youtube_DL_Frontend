namespace Youtube_DL_Frontnend {
    internal class ValidationLambdas {
        public static Func<string, bool> exampleLambda = (x) => {
            if (!x.Contains("lm")) {
                return false;
            } else {
                return true;
            }
        };

        public static Func<string, bool> isNumber = (x) => {
            return Int32.TryParse(x, out _);
        };
    }
}
