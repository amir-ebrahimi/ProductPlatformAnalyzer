using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    static class GeneralUtilities
    {
        public static string ParseExpression(string pPrefixExpr, string pParseType)
        {
            string lNewExpr = null;

            try
            {
                //For each condition first we have to build its coresponding tree
                Parser lConditionParser = new Parser();
                Node<string> lExprTree = new Node<string>("root");

                lConditionParser.AddChild(lExprTree, pPrefixExpr);

                foreach (Node<string> lItem in lExprTree)
                {
                    //Then we have to traverse the tree
                    switch (pParseType)
                    {
                        case "infix":
                            lNewExpr = ParseInfix(lItem);
                            break;
                        case "prefix":
                            lNewExpr = ParsePrefix(lItem);
                            break;
                        default:
                            lNewExpr = ParseInfix(lItem);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in parseExpression in GeneralUtilities");
                Console.WriteLine(ex.Message);
            }

            return lNewExpr;
        }

        private static string ParseInfix(Node<string> pNode)
        {
            try
            {
                List<Node<string>> lChildren = new List<Node<string>>();
                string lNewCon = null;
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
                    lNewCon = PrintStatus(pNode.Data);
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
                                lNewCon = "(" + ParseInfix(lChildren[0]) + " and " + ParseInfix(lChildren[1]) + ")";
                                break;
                            }
                        case "or":
                            {
                                lNewCon = "(" + ParseInfix(lChildren[0]) + " or " + ParseInfix(lChildren[1]) + ")";
                                break;
                            }
                        case "<=":
                            {
                                lNewCon = "(" + ParseInfix(lChildren[0]) + " <= " + ParseInfix(lChildren[1]) + ")";
                                break;
                            }
                        case ">=":
                            {
                                lNewCon = "(" + ParseInfix(lChildren[0]) + " >= " + ParseInfix(lChildren[1]) + ")";
                                break;
                            }
                        case ">":
                            {
                                lNewCon = "(" + ParseInfix(lChildren[0]) + " > " + ParseInfix(lChildren[1]) + ")";
                                break;
                            }
                        case "<":
                            {
                                lNewCon = "(" + ParseInfix(lChildren[0]) + " < " + ParseInfix(lChildren[1]) + ")";
                                break;
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                lNewCon = "(not " + ParseInfix(lChildren[0]) + ")";
                                break;
                            }
                        default:
                            break;
                    }
                }
                return lNewCon;
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
                string lNewCon = null;
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
                    lNewCon = PrintStatus(pNode.Data);
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
                                lNewCon = "(" + " and " + ParsePrefix(lChildren[0]) + ParsePrefix(lChildren[1]) + ")";
                                break;
                            }
                        case "or":
                            {
                                lNewCon = "(" + " or " + ParsePrefix(lChildren[0]) + ParsePrefix(lChildren[1]) + ")";
                                break;
                            }
                        case "<=":
                            {
                                lNewCon = "(" + " <= " + ParsePrefix(lChildren[0]) + ParsePrefix(lChildren[1]) + ")";
                                break;
                            }
                        case ">=":
                            {
                                lNewCon = "(" + " >= " + ParsePrefix(lChildren[0]) + ParsePrefix(lChildren[1]) + ")";
                                break;
                            }
                        case ">":
                            {
                                lNewCon = "(" + " > " + ParsePrefix(lChildren[0]) + ParsePrefix(lChildren[1]) + ")";
                                break;
                            }
                        case "<":
                            {
                                lNewCon = "(" + " < " + ParsePrefix(lChildren[0]) + ParsePrefix(lChildren[1]) + ")";
                                break;
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                lNewCon = "(not " + ParsePrefix(lChildren[0]) + ")";
                                break;
                            }
                        default:
                            break;
                    }
                }
                return lNewCon;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ParsePrefix in GeneralUtilities");
                Console.WriteLine(ex.Message);
                throw ex;
            }

        }

        public static string RemoveSpecialCharsFromString(string p, char[] pSpecialChars)
        {
            string lResultString = "";
            try
            {
                StringBuilder lSb = new StringBuilder();
                foreach (char lCharacter in p)
                {
                    if (!pSpecialChars.Contains(lCharacter))
                        lSb.Append(lCharacter);
                }
                lResultString = lSb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in RemoveSpecialCharsFromString");
                Console.WriteLine(ex.Message);
            }
            return lResultString;
        }

        private static string PrintStatus(string p)
        {
            string[] lCondition = new string[3];

            try
            {

                lCondition = p.Split('_');

                if (lCondition.Length == 1)
                    return p;
                else
                    switch (lCondition[1])
                    {
                        case "F":
                            return lCondition[0] + ".Finished";
                        case "I":
                            return lCondition[0] + ".Initial";
                        default:
                            return lCondition[0] + ".Executing";
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
