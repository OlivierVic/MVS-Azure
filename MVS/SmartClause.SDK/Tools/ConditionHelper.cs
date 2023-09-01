using Smartclause.SDK.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartClause.SDK.Tools
{
    public static class ConditionHelper
    {

        public static ConditionNode ConditionListToConditionTree(List<Condition> conditionList)
        {
            if (conditionList == null || conditionList.Count == 0)
                return null;
            
            ConditionNode currentNode = ConditionToConditionNode(conditionList[conditionList.Count - 1]); ;
            for (int i = conditionList.Count - 2 ; i >= 0; --i)
            {
                currentNode = new ConditionNode()
                {
                    Type = (int)ConditionTypeEnum.OPERATOR,
                    Value = ((int)ExpressionType.And).ToString(),
                    RightChild = currentNode,
                    LeftChild = ConditionToConditionNode(conditionList[i]),
                };
            }
            return currentNode;
        }

        private static ConditionNode ConditionToConditionNode(Condition cond)
        {
            ConditionNode leftChild = new()
            {
                Value = cond.Variable,
                Type = (int)ConditionTypeEnum.VARIABLE,
            };

            ConditionNode rightChild = new()
            {
                Value = cond.Value,
                Type = (int)ConditionTypeEnum.VALUE,
            };

            return new ConditionNode()
            {
                LeftChild = leftChild,
                RightChild = rightChild,
                Type = (int)ConditionTypeEnum.OPERATOR,
                Value = cond.Operation.ToString(),
            };

        }

        // Case where you use a list to handle conditions like this :
        // COND && COND && COND && COND ...
        public static List<Condition> ConditionTreeToConditionList(ConditionNode root)
        {
            List<Condition> conditionList = new();
            return ConditionTreeToConditionListRec(root, conditionList);
        }

        private static List<Condition> ConditionTreeToConditionListRec(ConditionNode root, List<Condition> conditionList)
        {
            if (root != null)
            {
                if (root.Type == (int)ConditionTypeEnum.OPERATOR)
                {
                    if (int.Parse(root.Value) != (int)ExpressionType.And)
                    {
                        if (root.LeftChild.Type != (int)ConditionTypeEnum.VARIABLE
                            || root.RightChild.Type != (int)ConditionTypeEnum.VALUE)
                            throw new InvalidOperationException("Condition tree not valid");
                        conditionList.Add(new Condition()
                        {
                            Value = root.RightChild.Value,
                            Variable = root.LeftChild.Value,
                            Operation = int.Parse(root.Value)
                        });
                    }
                    else
                    {
                        conditionList = ConditionTreeToConditionListRec(root.LeftChild, conditionList);
                        conditionList = ConditionTreeToConditionListRec(root.RightChild, conditionList);
                    }
                }
                else
                    throw new InvalidOperationException("Condition tree not valid");
            }

            return conditionList;
        }
    }
}
