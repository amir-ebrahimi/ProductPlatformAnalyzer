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
                        lResult[1] = lRemainderString.Substring(0, 1);
                    else
                        lResult[1] = lRemainderString;
                }
            }
            return lResult;
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
