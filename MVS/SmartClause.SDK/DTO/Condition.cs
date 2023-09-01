using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Smartclause.SDK.DTO
{
    public class Condition
    {
        public string Variable { get; set; }
        public int Operation { get; set; }
        public string Value { get; set; }
    }

    public enum ConditionOperations
    {
        [Description("EqualTo")]
        Equal = ExpressionType.Equal,
        [Description("GreaterThanOrEqualTo")]
        GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,
        [Description("LessThan")]
        LessThan = ExpressionType.LessThan,
    }

    public enum ConditionTypeEnum
    {
        OPERATOR = 0,
        VARIABLE = 1,
        VALUE = 2,
    }

    public class ConditionNode
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public string LeftNodeId { get; set; }
        public string RightNodeId { get; set; }
        public string Value { get; set; }

        public ConditionNode LeftChild { get; set; }
        public ConditionNode RightChild { get; set; }
    }
}
