using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace TestEval
{
    class Program
    {
        static void Main(string[] args)
        {
            var parameters = new Dictionary<string, double> { {"i_MACD_10_24_3_6", 1 },
                { "i_MACD_10_24_5_5", 2 },
                { "i_RSI_14_3_6", 3 },
                { "i_RSI_14", 4 },
                {"i_RSI_14_5", 5 }};
            while (true)
            {
                Console.WriteLine("type equatoin:");
                var equation = Console.ReadLine();
                var expressionParameters = parameters.Select(r => Expression.Parameter(typeof(double), r.Key)).ToArray();
                var expression = DynamicExpressionParser.ParseLambda(expressionParameters, typeof(bool), equation);
                var result = (bool)expression.Compile().DynamicInvoke(parameters.Values.Cast<object>().ToArray());
                Console.WriteLine($"result: {result}");
            }
        }
    }
}
