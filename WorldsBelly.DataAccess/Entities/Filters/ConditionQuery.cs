using System.Collections.Generic;

namespace WorldsBelly.DataAccess.Entities.Filters
{
    public class ConditionQuery
    {
        public string LogicalOperator { get; set; }
        public ICollection<QueryBuilder> Children { get; set; }
    }

    public class QueryBuilder
    {
        public string Type { get; set; }
        public Query Query { get; set; }
    }


    public class Query
    {
        public string Rule { get; set; }
        public string Operator { get; set; }
        public string Operand { get; set; }
        public object Value { get; set; }
        public string LogicalOperator { get; set; }
        public ICollection<QueryBuilder> Children { get; set; }
    }

    public class InBetweenObject
    {
        public string After { get; set; }
        public string Before { get; set; }
    }
}
