using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace TestExpressions
{
    public class ExpressionsProcessing
    {
        public bool ConditionCalc(string condition, SortedDictionary<string, double> parameters)
        {
            if (string.IsNullOrWhiteSpace(condition))
            {
                throw new ArgumentNullException($"Параметр {nameof(condition)} не может быть пустым или равным null");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException($"Параметр {nameof(parameters)} не может быть равным null");
            }

            var expressionParameters = parameters.Select(r => Expression.Parameter(typeof(double), r.Key)).ToArray();
            var expression = DynamicExpressionParser.ParseLambda(expressionParameters, typeof(bool), condition);
            return (bool)expression.Compile().DynamicInvoke(parameters.Values.Cast<object>().ToArray());
        }
    }
}
