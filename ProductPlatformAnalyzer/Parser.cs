using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    class Parser
    {
        public string[] FindFirstInstance(string pData, char pCharacter)
        {
            //looks in the string and returns the first substring which is seperated by the specified character
            string lRemainderString = "";
            string[] lResult = new string[2];

            if (pData.StartsWith("("))
            {

                //find the corresponding close brackets and feed the new string again to the parsestring
                int lParentisisCount = 0;
                for (int i = 1; i < pData.Length; i++)
                {
                    if (pData[i] == '(')
                        lParentisisCount = lParentisisCount + 1;
                    else if (pData[i] == ')')
                        lParentisisCount = lParentisisCount - 1;

                    if (lParentisisCount == -1)
                    {
                        lResult[0] = pData.Substring(1, i - 1);
                        if (pData[i] == ')')
                        {
                            lResult[1] = pData.Substring(i + 1).TrimEnd();
                            lResult[1] = pData.Substring(i + 1).TrimStart();
                        }
                        else
                            lResult[1] = pData.Substring(i, pData.Length - i - 1);
                        break;
                    }
                    else
                    {
                        lResult[0] = "";
                        lResult[1] = pData;
                    }
                }
            }
            else
            {
                int lCharacterIndex = pData.IndexOf(pCharacter);
                if (lCharacterIndex == -1)
                {
                    //when the input data is just one string with no seperating character, hence it should be just one operand
                    lResult[0] = pData;
                    lResult[1] = "";
                }
                else
                {
                    lResult[0] = pData.Substring(0, lCharacterIndex);
                    lRemainderString = pData.Substring(lCharacterIndex + 1);

                    if (lRemainderString.StartsWith(pCharacter.ToString()))
                        lResult[1] = lRemainderString.Substring(1);
                    else
                        lResult[1] = lRemainderString;
                }
            }
            return lResult;
        }

        public Tuple<Node<string>, string> MakeChildNode(Node<string> pParent, string pData)
        {
            Node<string> lResultNode = null;
            string lUpdatedExp = "";
            Tuple<Node<string>, string> lReturnedResult = null;

            if (pParent == null)
            {
                //if no node has been entered in then first a dumy node named root will be entered
                Node<string> newNode = new Node<string>("root");
                lResultNode = newNode;
                //pParent.AddChildNode(newNode);
            }

            /*
             function MakeBinaryTree(expr):
                element = next element in expr
                if element is a number:
                    return a leaf node of that number
                else: // element is an operator
                    left = MakeBinaryTree(expr)
                    right = MakeBinaryTree(expr)
                    return a binary tree with subtrees left and right and with operator element
             */
            string[] lElements = new string[2];

            lElements = FindFirstInstance(pData, ' ');

            //If the first element does not contain a space
            if (!pData.Contains(' '))
            {
                //termination condition
                //the input does not contain any spaces hence it is just one operand
                Node<string> newNode = new Node<string>(pData);
                lResultNode = newNode;
                //pParent.AddChildNode(newNode);
            }
            else // first element is an operator
            {
                string lOperator = lElements[0];
                string lRemainderExp = lElements[1];
                lUpdatedExp = lRemainderExp;

                Node<string> lNewOperatorNode = new Node<string>(lOperator);

                switch (lOperator)
                {
                    case "or":
                    case "and":
                    case "=>":
                    case "<=":
                    case ">=":
                    case "<":
                    case ">":
                    case "==":
                        {
                            //return a binary tree with subtrees left and right and with operator element
                            Node<string> lNewLeftNode = null;
                            Node<string> lNewRightNode = null;

                            //lElements = FindFirstInstance(pData, ' ');
                            //string lOperand1 = lElements[0].ToLower();
                            //string lRemainderExp1 = lElements[1];
                            lReturnedResult = MakeChildNode(pParent, lUpdatedExp);
                            lNewLeftNode = lReturnedResult.Item1;
                            lUpdatedExp = lReturnedResult.Item2;

                            //lElements = FindFirstInstance(pData, ' ');
                            //string lOperand2 = lElements[0].ToLower();
                            //string lRemainderExp2 = lElements[1];
                            lReturnedResult = MakeChildNode(pParent, lUpdatedExp);
                            lNewRightNode = lReturnedResult.Item1;
                            lUpdatedExp = lReturnedResult.Item2;

                            lNewOperatorNode.AddChildNode(lNewLeftNode);
                            lNewOperatorNode.AddChildNode(lNewRightNode);
                            lResultNode = lNewOperatorNode;
                            break;
                        }
                    case "not":
                        {
                            Node<string> lNewOperandNode = null;
                            lReturnedResult = MakeChildNode(pParent, lUpdatedExp);
                            lNewOperandNode = lReturnedResult.Item1;
                            lUpdatedExp = lReturnedResult.Item2;
                            lNewOperatorNode.AddChildNode(lNewOperandNode);
                            lResultNode = lNewOperatorNode;
                            break;
                        }
                    default:
                        {
                            //The next item is a operand
                            //lElements = FindFirstInstance(pData, ' ');
                            //string lOperand = lElements[0].ToLower();
                            Node<string> lNewNode = new Node<string>(lOperator);
                            lResultNode = lNewNode;
                            break;

                        }
                }

            }
            return new Tuple<Node<string>,string>(lResultNode, lUpdatedExp);

        }

        //The version which Knut gave the algorithm
        public void AddChild(Node<string> pParent, string pData)
        {
            if (pParent == null)
            {
                //if no node has been entered in then first a dumy node named root will be entered
                Node<string> newNode = new Node<string>("root");
                pParent.AddChildNode(newNode);
            }

            if (pData.Contains(' '))
            {
                string lOperator = "";
                string lOperand1 = "";
                string lOperand2 = "";

                string lSubString = "";
                string lRemainderString = "";

                string[] lResult = new string[2];

                lResult = FindFirstInstance(pData, ' ');
                lSubString = lResult[0];
                lRemainderString = lResult[1];

                lOperator = lSubString;
                Node<string> newNode = new Node<string>(lOperator);
                pParent.AddChildNode(newNode);

                switch (lOperator)
                {
                    case "or":
                    case "and":
                    case "->":
                    case "<=":
                    case ">=":
                    case "<":
                    case ">":
                    case "==":
                        {
                            lResult = FindFirstInstance(lRemainderString, ' ');
                            lOperand1 = lResult[0];
                            lRemainderString = lResult[1];

                            lResult = FindFirstInstance(lRemainderString, ' ');
                            lOperand2 = lResult[0];
                            lRemainderString = lResult[1];

                            AddChild(newNode, lOperand1);
                            AddChild(newNode, lOperand2); 
                            //AddChild(newNode, lRemainderString);
                            break;
                        }
                    case "not":
                        {
                            lResult = FindFirstInstance(lRemainderString, ' ');
                            lOperand1 = lResult[0];
                            lRemainderString = lResult[1];
                            AddChild(newNode, lOperand1);
                            break;
                        }
                    default:
                        break;
                }
            }
            else
            {
                //termination condition
                //the input does not contain any spaces hence it is just one operand
                Node<string> newNode = new Node<string>(pData);
                pParent.AddChildNode(newNode);
            }
        }

        //My old original version which was bugged
 /*       public void ParseString(string pExpr, NTree<string> pCurrentNode)
        {
            if (pExpr.StartsWith("("))
            {

                //find the corresponding close brackets and feed the new string again to the parsestring
                int lParentisisCount = 0;
                for (int i = 1; i < pExpr.Length; i++)
                {
                    if (pExpr[i] == '(')
                        lParentisisCount = lParentisisCount + 1;
                    else if (pExpr[i] == ')')
                        lParentisisCount = lParentisisCount - 1;

                    if (lParentisisCount == -1)
                    {
                        string lSubString = pExpr.Substring(1, i - 1);
                        ParseString(lSubString, pCurrentNode);
                        break;
                    }
                }
            }
            else
            {
                if (pExpr.Contains(' '))
                {
                    int lSpaceIndex = pExpr.IndexOf(' ');
                    string lSubString = pExpr.Substring(0, lSpaceIndex);
                    string lRemainderString = pExpr.Substring(lSpaceIndex + 1);

                    //if the expression starts with "and", "or", any variant name
                    if (pCurrentNode == null)
                    {
                        pCurrentNode = new NTree<string>(lSubString);
                    }
                    else
                    {
                        NTree<string> newNode = new NTree<string>(lSubString);
                        pCurrentNode.AddChildNode(newNode);
                    }

                    if (lRemainderString.Length > 0)
                        ParseString(lRemainderString, pCurrentNode);
                }
                else
                {
                    if (pCurrentNode == null)
                        pCurrentNode = new NTree<string>(pExpr);
                    else
                        pCurrentNode.AddChild(pExpr);
                }
            }

        }*/


    }
}
