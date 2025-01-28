using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WorldsBelly.DataAccess.Entities.Constants;
using WorldsBelly.DataAccess.Entities.Filters;
using Builder = WorldsBelly.DataAccess.Entities.Filters.QueryBuilder;
using Query = WorldsBelly.DataAccess.Entities.Filters.Query;

namespace WorldsBelly.DataAccess.Utilities.Extensions
{
    public static class QueryBuilderExtensions
    {

        public static string Build(this ICollection<Builder> children, string logicalOperator)
        {
            if (children == null || children.Count == 0) return "";

            var rules = children.Where(_ => _.Type.Equals(QueryBuilderType.Rule)).ToList();
            var rulesString = "";
            if (rules.Count != 0)
            {
                rulesString = rules.BuildRules(logicalOperator);
            }

            var groupString = "";
            var groups = children.Where(_ => _.Type.Equals(QueryBuilderType.Group)).ToList();

            if (groups.Count != 0)
            {
                groupString = groups.BuildGroups(logicalOperator);
            }

            var queryBuilderOperator = logicalOperator.Equals(LogicalOperator.Any) ? "OR" : "AND";
            var operand = groupString.Equals("") ? "" : queryBuilderOperator;
            var finalString = $"(_ => {rulesString} {operand} {groupString})";

            return finalString;
        }

        public static string BuildGroups(this ICollection<Builder> children, string logicalOperator)
        {
            var queryBuilderOperator = logicalOperator.Equals(LogicalOperator.Any) ? "OR" : "AND";
            var final = children.Aggregate("(", (current, child) => current + (child.Query.Children.BuildRules(child.Query.LogicalOperator) + $") {queryBuilderOperator} ("));

            final = final.ReplaceLast($" {queryBuilderOperator} (", "");
            return final;
        }

        public static string BuildRules(this ICollection<Builder> children, string logicalOperator)
        {
            return logicalOperator.ToLower() switch
            {
                LogicalOperator.All => children.All(),
                LogicalOperator.Any => children.Any(),
                _ => ""
            };
        }

        public static string All(this ICollection<Builder> children)
        {
            var toReturn = children.Aggregate("", (current, child) => current + child.Query.BuildExpression() + " AND ");
            toReturn = toReturn.ReplaceLast(" AND ", "");
            return toReturn;
        }

        public static string Any(this ICollection<Builder> children)
        {
            var toReturn = children.Aggregate("", (current, child) => current + child.Query.BuildExpression() + " OR ");
            toReturn = toReturn.ReplaceLast(" OR ", "");
            return toReturn;
        }

        public static string BuildExpression(this Query query)
        {
            return query.Operator switch
            {
                Operator.DoesEqual => query.Equals(),
                Operator.DoesNotEqual => query.NotEquals(),
                Operator.IsNotEmpty => query.NotEmpty(),
                Operator.IsEmpty => query.Empty(),
                Operator.Contains => query.Contains(),
                Operator.DoesNotContain => query.NotContains(),
                Operator.BeginsWith => query.StartsWith(),
                Operator.EndsWith => query.EndsWith(),
                Operator.GreaterThan => query.GreaterThan(),
                Operator.LowerThan => query.LessThan(),
                Operator.Within => query.Within(),
                Operator.NotWithin => query.NotWithin(),
                _ => ""
            };
        }

        #region Builders

        private static string Equals(this Query query)
        {
            if (query.Operand.Contains("."))
            {
                var operands = query.Operand.Split(".");
                return $"_.{operands[0]}.Any(x => x.{operands[1]} = \"{query.Value as string}\") ";
            }
            else
            {
                return $"_.{query.Operand} = \"{query.Value as string}\" ";
            }
        }

        private static string NotEquals(this Query query)
        {
            if (query.Operand.Contains("."))
            {
                var operands = query.Operand.Split(".");
                return $"_.{operands[0]}.Any(x => x.{operands[1]} != \"{query.Value as string}\") ";
            }
            else
            {
                return $"_.{query.Operand} != \"{query.Value as string}\" ";
            }
        }

        private static string NotEmpty(this Query query)
        {
            if (query.Operand.Contains("."))
            {
                var operands = query.Operand.Split("."); 
                return $"_.{operands[0]}.Any(x => x.{operands[1]} != \"\") ";
            }
            else
            {
                return $"_.{query.Operand} != \"\" ";
            }

        }

        private static string Empty(this Query query)
        {
            if (query.Operand.Contains("."))
            {
                var operands = query.Operand.Split(".");
                return $"_.{operands[0]}.Any(x => !x.{operands[1]} == \"\") ";
            }
            else
            {
                return $"_.{query.Operand} == \"\" ";
            }

        }

        private static string Contains(this Query query)
        {
            if(query.Operand.Contains("."))
            {
               var operands = query.Operand.Split(".");
                return $"_.{operands[0]}.Any(x => x.{operands[1]}.Contains(\"{query.Value as string}\")) ";
            }
            else
            {
                return $"_.{query.Operand}.Contains(\"{query.Value as string}\") ";
            }
        }

        private static string NotContains(this Query query)
        {
            if (query.Operand.Contains("."))
            {
                var operands = query.Operand.Split(".");
                return $"_.{operands[0]}.Any(x => NOT x.{operands[1]}.Contains(\"{query.Value as string}\")) ";
            }
            else
            {
                return $"NOT _.{query.Operand}.Contains(\"{query.Value as string}\") ";
            }

        }

        private static string StartsWith(this Query query)
        {
            if (query.Operand.Contains("."))
            {
                var operands = query.Operand.Split(".");
                return $"_.{operands[0]}.Any(x => x.{operands[1]}.StartsWith(\"{query.Value as string}\")) ";
            }
            else
            {
                return $"_.{query.Operand}.StartsWith(\"{query.Value as string}\") ";
            }
        }

        private static string EndsWith(this Query query)
        {
            if (query.Operand.Contains("."))
            {
                var operands = query.Operand.Split(".");
                return $"_.{operands[0]}.Any(x => x.{operands[1]}.EndsWith(\"{query.Value as string}\")) ";
            }
            else
            {
                return $"_.{query.Operand}.EndsWith(\"{query.Value as string}\") ";
            }
        }

        private static string GreaterThan(this Query query)
        {
            if (query.Operand.Contains("."))
            {
                var operands = query.Operand.Split(".");
                return $"_.{operands[0]}.Any(x => x.{operands[1]} > {Convert.ToDouble((string)query.Value, CultureInfo.InvariantCulture)}) ";
            }
            else
            {
                return $"_.{query.Operand} > {Convert.ToDouble((string)query.Value, CultureInfo.InvariantCulture)}";
            }
        }

        private static string LessThan(this Query query)
        {
            if (query.Operand.Contains("."))
            {
                var operands = query.Operand.Split(".");
                return $"_.{operands[0]}.Any(x => x.{operands[1]} < {Convert.ToDouble((string)query.Value, CultureInfo.InvariantCulture)}) ";
            }
            else
            {
                return $"_.{query.Operand} < {Convert.ToDouble((string)query.Value, CultureInfo.InvariantCulture)}";
            }
        }

        private static string Within(this Query query)
        {
            var inBetween = JsonConvert.DeserializeObject<InBetweenObject>(JsonConvert.SerializeObject(query.Value));
            var dateAfter = DateTimeOffset.Parse(inBetween.After);
            var dateBefore = DateTimeOffset.Parse(inBetween.Before);
            return $"_.{query.Operand} >= {dateAfter.ToEpoch()} AND _.{query.Operand} <= {dateBefore.ToEpoch()} ";
        }

        private static string NotWithin(this Query query)
        {
            var inBetween = JsonConvert.DeserializeObject<InBetweenObject>(JsonConvert.SerializeObject(query.Value));
            var dateAfter = DateTimeOffset.Parse(inBetween.After);
            var dateBefore = DateTimeOffset.Parse(inBetween.Before);
            return $"_.{query.Operand} <= {dateAfter.ToEpoch()} AND _.{query.Operand} >= {dateBefore.ToEpoch()}";

        }

        #endregion
    }
}
