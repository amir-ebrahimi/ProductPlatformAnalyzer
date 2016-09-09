using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    static class GeneralUtilities
    {
        public static string parseExpression(string pPrefixExpr, string pParseType)
        {
            string newExpr = null;

            try
            {
                //For each condition first we have to build its coresponding tree
                Parser lConditionParser = new Parser();
                Node<string> lExprTree = new Node<string>("root");

                lConditionParser.AddChild(lExprTree, pPrefixExpr);

                foreach (Node<string> item in lExprTree)
                {
                    //Then we have to traverse the tree
                    switch (pParseType)
                    {
                        case "infix":
                            newExpr = ParseInfix(item);
                            break;
                        case "prefix":
                            newExpr = ParsePrefix(item);
                            break;
                        default:
                            newExpr = ParseInfix(item);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in parseExpression in GeneralUtilities");
                Console.WriteLine(ex.Message);
            }

            return newExpr;
        }

        private static string ParseInfix(Node<string> pNode)
        {
            try
            {
                List<Node<string>> lChildren = new List<Node<string>>();
                string newCon = null;
                if ((pNode.Data != "and")
                    && (pNode.Data != "or")
                    && (pNode.Data != "not")
                    && (pNode.Data != "<=")
                    && (pNode.Data != ">=")
                    && (pNode.Data != "<")
                    && (pNode.Data != ">")
                    )
                {
                    //We have one operator
                    newCon = printStatus(pNode.Data);
                }
                else
                {
                    foreach (Node<string> lChild in pNode.Children)
                    {
                        lChildren.Add(lChild);
                    }
                    switch (pNode.Data)
                    {
                        case "and":
                            {
                                newCon = "(" + ParseInfix(lChildren[0]) + " and " + ParseInfix(lChildren[1]) + ")";
                                break;
                            }
                        case "or":
                            {
                                newCon = "(" + ParseInfix(lChildren[0]) + " or " + ParseInfix(lChildren[1]) + ")";
                                break;
                            }
                        case "<=":
                            {
                                newCon = "(" + ParseInfix(lChildren[0]) + " <= " + ParseInfix(lChildren[1]) + ")";
                                break;
                            }
                        case ">=":
                            {
                                newCon = "(" + ParseInfix(lChildren[0]) + " >= " + ParseInfix(lChildren[1]) + ")";
                                break;
                            }
                        case ">":
                            {
                                newCon = "(" + ParseInfix(lChildren[0]) + " > " + ParseInfix(lChildren[1]) + ")";
                                break;
                            }
                        case "<":
                            {
                                newCon = "(" + ParseInfix(lChildren[0]) + " < " + ParseInfix(lChildren[1]) + ")";
                                break;
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                newCon = "(not " + ParseInfix(lChildren[0]) + ")";
                                break;
                            }
                        default:
                            break;
                    }
                }
                return newCon;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ParseInfix in GeneralUtilities");
                Console.WriteLine(ex.Message);
                throw ex;
            }

        }

        private static string ParsePrefix(Node<string> pNode)
        {
            try
            {
                List<Node<string>> lChildren = new List<Node<string>>();
                string newCon = null;
                if ((pNode.Data != "and")
                    && (pNode.Data != "or")
                    && (pNode.Data != "not")
                    && (pNode.Data != "<=")
                    && (pNode.Data != ">=")
                    && (pNode.Data != "<")
                    && (pNode.Data != ">")
                    )
                {
                    //We have one operator
                    newCon = printStatus(pNode.Data);
                }
                else
                {
                    foreach (Node<string> lChild in pNode.Children)
                    {
                        lChildren.Add(lChild);
                    }
                    switch (pNode.Data)
                    {
                        case "and":
                            {
                                newCon = "(" + " and " + ParsePrefix(lChildren[0]) + ParsePrefix(lChildren[1]) + ")";
                                break;
                            }
                        case "or":
                            {
                                newCon = "(" + " or " + ParsePrefix(lChildren[0]) + ParsePrefix(lChildren[1]) + ")";
                                break;
                            }
                        case "<=":
                            {
                                newCon = "(" + " <= " + ParsePrefix(lChildren[0]) + ParsePrefix(lChildren[1]) + ")";
                                break;
                            }
                        case ">=":
                            {
                                newCon = "(" + " >= " + ParsePrefix(lChildren[0]) + ParsePrefix(lChildren[1]) + ")";
                                break;
                            }
                        case ">":
                            {
                                newCon = "(" + " > " + ParsePrefix(lChildren[0]) + ParsePrefix(lChildren[1]) + ")";
                                break;
                            }
                        case "<":
                            {
                                newCon = "(" + " < " + ParsePrefix(lChildren[0]) + ParsePrefix(lChildren[1]) + ")";
                                break;
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                newCon = "(not " + ParsePrefix(lChildren[0]) + ")";
                                break;
                            }
                        default:
                            break;
                    }
                }
                return newCon;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ParsePrefix in GeneralUtilities");
                Console.WriteLine(ex.Message);
                throw ex;
            }

        }

        private static string printStatus(string p)
        {
            string[] condition = new string[3];

            try
            {

                condition = p.Split('_');

                if (condition.Length == 1)
                    return p;
                else
                    switch (condition[1])
                    {
                        case "F":
                            return condition[0] + ".Finished";
                        case "I":
                            return condition[0] + ".Initial";
                        default:
                            return condition[0] + ".Executing";
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in printStatus in GeneralUtilities");
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
