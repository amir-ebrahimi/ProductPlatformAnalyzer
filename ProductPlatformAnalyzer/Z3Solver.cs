using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Z3;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ProductPlatformAnalyzer
{
    class Z3Solver
    {
        private ArrayExpr cExpressions;
        private List<Expr> cExpressionList;
        private BoolExpr cConstraints;
        private ArrayList cConstraintList;
        private Solver cISolver; 
        private Context cICtx;
        private int cConstraintCounter;
        private int cBooleanExpressionCounter;
        private bool cIDebugMode;
        private string cIDebugText;
        private Model cResultModel;

        public Z3Solver()
        {
            cIDebugText = "";
            cICtx = new Context(new Dictionary<string, string>() { { "proof", "true" } });
            using (cICtx)
            {
                this.cISolver = cICtx.MkSolver("QF_FD");
                this.cExpressionList = new List<Expr>();
                this.cConstraintList = new ArrayList();
            }
        }

        public void setDebugMode(bool pDebugMode)
        {
            cIDebugMode = pDebugMode;
        }

        public bool getDebugMode()
        {
            return cIDebugMode;
        }

        public void setExpressions(ArrayExpr pExpressions)
        {
            cExpressions = pExpressions;
        }

        public void setExpressionList(List<Expr> pExpressionList)
        {
            cExpressionList = pExpressionList;
        }

        public ArrayExpr getExpression()
        {
            return cExpressions;
        }

        public List<Expr> getExpressionList()
        {
            return cExpressionList;
        }

        public void setConstraints(BoolExpr pConstraints)
        {
            cConstraints = pConstraints;
        }

        public void setConstraintList(ArrayList pConstraintList)
        {
            cConstraintList = pConstraintList;
        }

        public BoolExpr getConstraints()
        {
            return cConstraints;
        }

        public ArrayList getConstraintList()
        {
            return cConstraintList;
        }

        public int getConstraintCounter()
        {
            return cConstraintCounter;
        }

        public void setConstraintCounter(int pCounter)
        {
            cConstraintCounter = pCounter;
        }

        public int getBooleanExpressionCounter()
        {
            return cBooleanExpressionCounter;
        }

        public void setBooleanExpressionCounter(int pCounter)
        {
            cBooleanExpressionCounter = pCounter;
        }

        public int getNextBooleanExpressionCounter()
        {
            int newCounter = 0;
            try
            {
                newCounter = getBooleanExpressionCounter() + 1;
                setBooleanExpressionCounter(newCounter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in getNextBooleanExpressionCounter");
                Console.WriteLine(ex.Message);
            }
            return newCounter;
        }

        public int getNextConstraintCounter()
        {
            int newCounter = 0;
            try
            {
                newCounter = getConstraintCounter() + 1;
                setConstraintCounter(newCounter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in getNextConstraintCounter");
                Console.WriteLine(ex.Message);
            }
            return newCounter;
        }

        public BoolExpr getFalseBoolExpr()
        {
            return cICtx.MkFalse();
        }

        public string ReturnStringElements(List<String> pList)
        {
            string lResultElements = "";
            try
            {
                foreach (string lElement in pList)
                    lResultElements += lElement;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ReturnStringElements");
                Console.WriteLine(ex.Message);
            }
            return lResultElements;
        }

        public string ReturnBoolExprElementNames(List<BoolExpr> pList)
        {
            string lResultElementNames = "";
            try
            {
                foreach (BoolExpr lElement in pList)
                    lResultElementNames += lElement.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ReturnBoolExprElementNames");
                Console.WriteLine(ex.Message);
            }

            return lResultElementNames;
        }

        public void AddAndOperator2Constraints(List<String> pOperandList, String pConstraintSource)
        {
            try
            {
                BoolExpr Constraint = (BoolExpr)FindBoolExpressionUsingName(pOperandList[0]);

                for (int i = 1; i < pOperandList.Count; i++)
                    Constraint = cICtx.MkAnd(Constraint, (BoolExpr)FindBoolExpressionUsingName(pOperandList[i]));

                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddAndOperator2Constraints, params: " + ReturnStringElements(pOperandList) );
                throw ex;
            }
        }

        public void AddGreaterOperator2Constraints(Expr pOperand1, int pOperand2, string pConstraintSource)
        {
            try
            {
                Expr lOperand2 = cICtx.MkConst("pOperand2", cICtx.MkIntSort());
                BoolExpr lConstraint = cICtx.MkGt((ArithExpr)pOperand1, (ArithExpr)lOperand2);

                AddConstraintToSolver(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddGreaterOperator2Constraints");
                Console.WriteLine(ex.Message);
            }
        }

        public void AddEqualOperator2Constraints(Expr pOperand1, int pOperand2, string pConstraintSource)
        {
            try
            {
                //Expr lOperand2 = iCtx.MkConst(pOperand2, iCtx.MkIntSort());
                BoolExpr lConstraint = cICtx.MkEq((ArithExpr)pOperand1, cICtx.MkInt(pOperand2));

                AddConstraintToSolver(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddEqualOperator2Constraints");
                Console.WriteLine(ex.Message);
            }
        }

        public void AddAndOperator2Constraints(List<BoolExpr> pOperandList, String pConstraintSource)
        {
            try
            {
                BoolExpr Constraint = pOperandList[0];

                for (int i = 1; i < pOperandList.Count; i++)
                    Constraint = cICtx.MkAnd(Constraint, pOperandList[i]);

                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddAndOperator2Constraints, params: " + ReturnBoolExprElementNames(pOperandList));
                throw ex;
            }
        }

        public BoolExpr AndOperator(List<String> pOperandList)
        {
            try
            {
                BoolExpr lResultExpression = (BoolExpr)FindBoolExpressionUsingName(pOperandList[0]);

                for (int i = 1; i < pOperandList.Count; i++)
                    lResultExpression = cICtx.MkAnd(lResultExpression, (BoolExpr)FindBoolExpressionUsingName(pOperandList[i]));

                return lResultExpression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AndOperator, params: " + ReturnStringElements(pOperandList));
                throw ex;
            }
        }

        public BoolExpr AndOperator(List<BoolExpr> pOperandList)
        {
            try
            {
                BoolExpr lResultExpression = pOperandList[0];

                for (int i = 1; i < pOperandList.Count; i++)
                    lResultExpression = cICtx.MkAnd(lResultExpression, pOperandList[i]);

                return lResultExpression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AndOperator, params: " + ReturnBoolExprElementNames(pOperandList));
                throw ex;
            }
        }

        public BoolExpr GreaterThanOperator(Expr pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                lResultExpression = cICtx.MkGt((ArithExpr)pOperand0, cICtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in GreaterThanOperator");
                Console.WriteLine(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr GreaterThanOperator(string pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                Expr lOperand0 = FindExpressionUsingName(pOperand0);
                lResultExpression = cICtx.MkGt((ArithExpr)lOperand0, cICtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in GreaterThanOperator");
                Console.WriteLine(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr LessThanOperator(Expr pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                lResultExpression = cICtx.MkLt((ArithExpr)pOperand0, cICtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in LessThanOperator");
                Console.WriteLine(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr LessThanOperator(string pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                Expr lOperand0 = FindExpressionUsingName(pOperand0);
                lResultExpression = cICtx.MkLt((ArithExpr)lOperand0, cICtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in LessThanOperator");
                Console.WriteLine(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr GreaterOrEqualOperator(Expr pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                lResultExpression = cICtx.MkGe((ArithExpr)pOperand0, cICtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in GreaterOrEqualOperator");
                Console.WriteLine(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr GreaterOrEqualOperator(string pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                Expr lOperand0 = FindExpressionUsingName(pOperand0);
                lResultExpression = cICtx.MkGe((ArithExpr)lOperand0, cICtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in GreaterOrEqualOperator");
                Console.WriteLine(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr LessOrEqualOperator(Expr pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                lResultExpression = cICtx.MkLe((ArithExpr)pOperand0, cICtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in LessOrEqualOperator");
                Console.WriteLine(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr LessOrEqualOperator(string pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                Expr lOperand0 = FindExpressionUsingName(pOperand0);
                lResultExpression = cICtx.MkLe((ArithExpr)lOperand0, cICtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in LessOrEqualOperator");
                Console.WriteLine(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr IffOperator(String pOperand1, String pOperand2)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lFirstOperand = FindBoolExpressionUsingName(pOperand1);
                Expr lSecondOperand = FindBoolExpressionUsingName(pOperand2);

                BoolExpr Expression = cICtx.MkIff((BoolExpr)lFirstOperand, (BoolExpr)lSecondOperand);

                return Expression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in IffOperator, " + pOperand1 + " , " + pOperand2);
                throw ex;
            }
        }

        public void AddImpliesOperator2Constraints(BoolExpr pOperand1, BoolExpr pOperand2, String pConstraintSource)
        {
            try
            {
                BoolExpr Constraint = cICtx.MkImplies(pOperand1, pOperand2);

                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddImpliesOperator2Constraints, " + pOperand1.ToString() + " , " + pOperand2.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// This function takes two operand and returns the expression which is the implecation of these two operands
        /// </summary>
        /// <param name="pOperand1">Left hand side of the implecation</param>
        /// <param name="pOperand2">Right hand side of the implecation</param>
        /// <returns>Implecation expression</returns>
        public BoolExpr ImpliesOperator(BoolExpr pOperand1, BoolExpr pOperand2)
        {
            BoolExpr lResultExpr = null;
            try
            {
                lResultExpr = cICtx.MkImplies(pOperand1, pOperand2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ImpliesOperator");
                Console.WriteLine(ex.Message);
            }
            return lResultExpr;
        }

        public void AddSimpleConstraint(String pConstraint, String pConstraintSource)
        {
            try
            {
                BoolExpr lConstraint = FindBoolExpressionUsingName(pConstraint);

                AddConstraintToSolver(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddSimpleConstraint, " + pConstraint);
                throw ex;
            }
        }

        public BoolExpr TwoWayImpliesOperator(String pOperand1, String pOperand2)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lFirstOperand = FindBoolExpressionUsingName(pOperand1);
                
                Expr lSecondOperand = FindBoolExpressionUsingName(pOperand2);

                BoolExpr Expression2 = cICtx.MkImplies((BoolExpr)lSecondOperand, (BoolExpr)lFirstOperand);

                BoolExpr Expression1 = cICtx.MkImplies((BoolExpr)lFirstOperand, (BoolExpr)lSecondOperand);

                BoolExpr Expression = cICtx.MkAnd(Expression1, Expression2);
                return Expression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in TwoWayImpliesOperator, " + pOperand1 + " , " + pOperand2);
                throw ex;
            }
        }

        public BoolExpr TwoWayImpliesOperator(BoolExpr pOperand1, BoolExpr pOperand2)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                BoolExpr Expression1 = cICtx.MkImplies(pOperand1, pOperand2);
                BoolExpr Expression2 = cICtx.MkImplies(pOperand2, pOperand1);

                BoolExpr Expression = cICtx.MkAnd(Expression1, Expression2);
                return Expression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in TwoWayImpliesOperator, " + pOperand1.ToString() + " , " + pOperand2.ToString());
                throw ex;
            }
        }

        public void AddTwoWayImpliesOperator2Constraints(string pOperand1, string pOperand2,string pConstraintSource)
        {
            try
            {
                BoolExpr lConstraint = TwoWayImpliesOperator(pOperand1, pOperand2);
                AddConstraintToSolver(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddTwoWayImpliesOperator2Constraints");
                Console.WriteLine(ex.Message);
            }
        }

        public void AddTwoWayImpliesOperator2Constraints(BoolExpr pOperand1, BoolExpr pOperand2, String pConstraintSource)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                BoolExpr Expression1 = cICtx.MkImplies(pOperand1, pOperand2);
                BoolExpr Expression2 = cICtx.MkImplies(pOperand2, pOperand1);

                BoolExpr Expression = cICtx.MkAnd(Expression1, Expression2);

                AddConstraintToSolver(Expression, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddTwoWayImpliesOperator2Constraints, " + pOperand1.ToString() + " , " + pOperand2.ToString());
                throw ex;
            }
        }

        public BoolExpr IffOperator(List<BoolExpr> pOperandList)
        {
            try
            {
                BoolExpr lResultExpression = pOperandList[0];

                for (int i = 1; i < pOperandList.Count; i++)
                    lResultExpression = cICtx.MkIff(lResultExpression, pOperandList[i]);

                return lResultExpression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in IffOperator, params: " + ReturnBoolExprElementNames(pOperandList));
                throw ex;
            }
        }

        public BoolExpr ImpliesOperator(List<BoolExpr> pOperandList)
        {
            try
            {
                BoolExpr lResultExpression = pOperandList[0];

                for (int i = 1; i < pOperandList.Count; i++)
                    lResultExpression = cICtx.MkImplies(lResultExpression, pOperandList[i]);

                return lResultExpression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ImpliesOperator, params: " + ReturnBoolExprElementNames(pOperandList));
                throw ex;
            }
        }

        public void AddOrOperator2Constraints(List<BoolExpr> pOperandList, String pConstraintSource)
        {
            try
            {
                BoolExpr Constraint = pOperandList[0];

                for (int i = 1; i < pOperandList.Count; i++)
                    Constraint = cICtx.MkOr(Constraint, pOperandList[i]);

                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddOrOperator2Constraints, params: " + ReturnBoolExprElementNames(pOperandList));
                throw ex;
            }
        }

        public void AddOrOperator2Constraints(List<String> pOperandList, String pConstraintSource)
        {
            try
            {
                BoolExpr Constraint = (BoolExpr)FindBoolExpressionUsingName(pOperandList[0]);

                for (int i = 1; i < pOperandList.Count; i++)
                    Constraint = cICtx.MkOr(Constraint, (BoolExpr)FindBoolExpressionUsingName(pOperandList[i]));

                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddOrOperator2Constraints, params: " + ReturnStringElements(pOperandList));
                throw ex;
            }
        }

        public BoolExpr OrOperator(List<String> pOperandList)
        {
            try
            {
                BoolExpr lResultExpression = (BoolExpr)FindExpressionUsingName(pOperandList[0]);

                for (int i = 1; i < pOperandList.Count; i++)
                {
                    lResultExpression = cICtx.MkOr(lResultExpression, (BoolExpr)FindBoolExpressionUsingName(pOperandList[i]));
                }
                return lResultExpression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in OrOperator, params: " + ReturnStringElements(pOperandList));
                throw ex;
            }
        }

        public BoolExpr OrOperator(List<BoolExpr> pOperandList)
        {
            try
            {
                BoolExpr lResultExpr = pOperandList[0];

                for (int i = 1; i < pOperandList.Count; i++)
                    lResultExpr = cICtx.MkOr(lResultExpr, pOperandList[i]);

                return lResultExpr;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in OrOperator, params: " + ReturnBoolExprElementNames(pOperandList));
                throw ex;
            }
        }

        public void AddPickOneOperator2Constraints(List<String> pOperandList, String pConstraintSource)
        {
            try
            {
                BoolExpr Constraint = (BoolExpr)FindBoolExpressionUsingName(pOperandList[0]);

                for (int i = 1; i < pOperandList.Count; i++)
                {
                    BoolExpr lOperand = FindBoolExpressionUsingName(pOperandList[i]);
                    
                    //Optimized   Constraint = iCtx.MkOr(iCtx.MkAnd(Constraint, iCtx.MkNot((BoolExpr)lOperand))
                    //                            , iCtx.MkAnd(iCtx.MkNot(Constraint), (BoolExpr)lOperand));

                    Constraint = cICtx.MkXor(Constraint, lOperand);
                }

                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddPickOneOperator2Constraints, " + ReturnStringElements(pOperandList));
                throw ex;
            }
        }

        public BoolExpr PickOneOperator(List<String> pOperandList)
        {
            try
            {
                BoolExpr lResultExpression = (BoolExpr)FindBoolExpressionUsingName(pOperandList[0]);

                for (int i = 1; i < pOperandList.Count; i++)
                {
                    BoolExpr lOperand = FindBoolExpressionUsingName(pOperandList[i]);

                    //TODO: it is impossible to use the xor operator straight because with three operands it will not give the right answer

                    AddBooleanExpression("Xor-helper" + i);

                    BoolExpr lTempImplies;
                    BoolExpr lHelperBoolExpr;

                    if (i.Equals(1))
                        lTempImplies = ImpliesOperator(new List<BoolExpr>() { lResultExpression, (BoolExpr)lOperand });
                    else
                        lTempImplies = ImpliesOperator(new List<BoolExpr>() { FindBoolExpressionUsingName("Xor-helper" + (i - 1)), (BoolExpr)lOperand });

                    lHelperBoolExpr = ImpliesOperator(FindBoolExpressionUsingName("Xor-helper" + i), lTempImplies);
                    AddConstraintToSolver(lHelperBoolExpr, "Building Xor Operator");

                    AddConstraintToSolver(FindBoolExpressionUsingName("Xor-helper" + i), "Building Xor Operator");

                    
                    //lResultExpression = cICtx.MkOr(cICtx.MkAnd(lResultExpression, cICtx.MkNot((BoolExpr)lOperand))
                    //                        , cICtx.MkAnd(cICtx.MkNot(lResultExpression), (BoolExpr)lOperand));

                    //lResultExpression = cICtx.MkXor(lResultExpression, lOperand);
                }
                return lResultExpression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in PickOneOperator, " + ReturnStringElements(pOperandList));
                throw ex;
            }
        }

        public void AddPickOneOperator2Constraints(List<BoolExpr> pOperandList, String pConstraintSource)
        {
            try
            {
                /* Optmized:
                //We should show
                //or pOperand1 pOperand2 pOperand3 ...
                //and (=> pOperand1 (and (not pOperand2) (not pOperand3) ...))
                //    (=> pOperand2 (and (not pOperand1) (not pOperand3) ...))
                //    (=> pOperand3 (and (not pOperand1) (not pOperand2) ...))

                BoolExpr lOrPartConstraint = pOperandList[0];
                BoolExpr lAndPartConstraint = null;

                for (int i = 1; i < pOperandList.Count; i++)
                {
                    BoolExpr lOperand = pOperandList[i];

                    lOrPartConstraint = iCtx.MkOr(lOrPartConstraint, lOperand);

                    //Temporarilly remove the active operand to make the rest of the list 
                    List<BoolExpr> lTempRestOfOperandList = pOperandList;
                    lTempRestOfOperandList.Remove(lOperand);
                    lAndPartConstraint = iCtx.MkAnd(iCtx.MkImplies(lOperand, XorHelper(lTempRestOfOperandList)));

                    //This line was the previous implementation which now should be removed
                    //Constraint = iCtx.MkOr(iCtx.MkAnd(Constraint, iCtx.MkNot(lOperand))
                    //                        , iCtx.MkAnd(iCtx.MkNot(Constraint), lOperand));
                }

                AddConstraintToSolver(lOrPartConstraint, pConstraintSource);
                AddConstraintToSolver(lAndPartConstraint, pConstraintSource);*/

                //This implementation was not working either
                /*
                BoolExpr Constraint = pOperandList[0];

                for (int i = 1; i < pOperandList.Count; i++)
                {
                    BoolExpr lOperand = pOperandList[i];

                    //Optimized   Constraint = iCtx.MkOr(iCtx.MkAnd(Constraint, iCtx.MkNot((BoolExpr)lOperand))
                    //                            , iCtx.MkAnd(iCtx.MkNot(Constraint), (BoolExpr)lOperand));

                    Constraint = cICtx.MkXor(Constraint, lOperand);
                }
                 */

                BoolExpr Constraint;
                int[] lCoeffecient = new int[pOperandList.Count];
                for (int i = 0; i < lCoeffecient.Length; i++)
			    {
                    lCoeffecient[i] = 1;			 
			    }
                BoolExpr[] lOperandsArray = new BoolExpr[pOperandList.Count];
                for (int i = 0; i < lOperandsArray.Length; i++)
                {
                    lOperandsArray[i] = pOperandList[i];
                }
                Constraint = cICtx.MkPBEq(lCoeffecient, lOperandsArray, 1);
                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddPickOneOperator2Constraints, " + ReturnBoolExprElementNames(pOperandList));
                throw ex;
            }
        }

        /*Optimized:
        private BoolExpr XorHelper(List<BoolExpr> pOperandList)
        {
            BoolExpr lResultExpr = iCtx.MkNot(pOperandList[0]);
            try
            {
                foreach (BoolExpr lOperand in pOperandList)
                {
                    lResultExpr = iCtx.MkAnd(lResultExpr, iCtx.MkNot(lOperand));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in XorHelper");
                Console.WriteLine(ex.Message);
            }
            return lResultExpr;
        }*/

        public BoolExpr PickOneOperator(List<BoolExpr> pOperandList)
        {
            try
            {
                BoolExpr lResultExpression;
                int[] lCoeffecient = new int[pOperandList.Count];
                for (int i = 0; i < lCoeffecient.Length; i++)
                {
                    lCoeffecient[i] = 1;
                }
                BoolExpr[] lOperandsArray = new BoolExpr[pOperandList.Count];
                for (int i = 0; i < lOperandsArray.Length; i++)
                {
                    lOperandsArray[i] = pOperandList[i];
                }
                lResultExpression = cICtx.MkPBEq(lCoeffecient, lOperandsArray, 1);

                //This implementation did not work because it had to have one of the first two operands true and the rest could not become true
                /*
                //BoolExpr lResultExpression = pOperandList[0];
                List<BoolExpr> lResultExpressionList = new List<BoolExpr>();

                for (int i = 1; i < pOperandList.Count; i++)
                {
                    BoolExpr lOperand = pOperandList[i];
                    //TODO: it is impossible to use the xor operator straight because with three operands it will not give the right answer

                    AddBooleanExpression("Xor-helper" + i);

                    BoolExpr lTempImplies;
                    BoolExpr lHelperBoolExpr;

                    if (i.Equals(1))
                        //lTempImplies = ImpliesOperator(new List<BoolExpr>(){ pOperandList[0], pOperandList[i] });
                        lTempImplies = cICtx.MkXor( pOperandList[0], pOperandList[i] );
                    else
                        //lTempImplies = ImpliesOperator(new List<BoolExpr>(){ FindBoolExpressionUsingName("Xor-helper" + (i-1)), pOperandList[i]});
                        lTempImplies = cICtx.MkXor( FindBoolExpressionUsingName("Xor-helper" + (i - 1)), pOperandList[i] );

                    lHelperBoolExpr = ImpliesOperator(FindBoolExpressionUsingName("Xor-helper" + i), lTempImplies);
                    //AddConstraintToSolver(lHelperBoolExpr, "Building Xor Operator");
                    lResultExpressionList.Add(lHelperBoolExpr);

                    //AddConstraintToSolver(FindBoolExpressionUsingName("Xor-helper" + i), "Building Xor Operator");
                    lResultExpressionList.Add(FindBoolExpressionUsingName("Xor-helper" + i));

                    //Tried this it did not work for more thand two operands, it can make more than one true at the same time
                    //lResultExpression = cICtx.MkOr(cICtx.MkAnd(lResultExpression, cICtx.MkNot((BoolExpr)lOperand))
                    //                        , cICtx.MkAnd(cICtx.MkNot(lResultExpression), (BoolExpr)lOperand));

                    //Tried this it did not work for more thand two operands, it can make more than one true at the same time
                    //lResultExpression = cICtx.MkXor(lResultExpression, lOperand);
                }*/
                return lResultExpression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in PickOneOperator, " + ReturnBoolExprElementNames(pOperandList));
                throw ex;
            }
        }

        public void AddNotOperator2Constraints(String pOperand, String pConstraintSource)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lOperand = FindBoolExpressionUsingName(pOperand);

                BoolExpr Constraint = cICtx.MkNot((BoolExpr)lOperand);

                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddNotOperator2Constraints, " + pOperand);
                throw ex;
            }
        }

        public BoolExpr NotOperator(String pOperand)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lOperand = FindBoolExpressionUsingName(pOperand);

                BoolExpr Expression = cICtx.MkNot((BoolExpr)lOperand);

                return Expression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in NotOperator, " + pOperand);
                throw ex;
            }
        }

        public BoolExpr NotOperator(BoolExpr pOperand)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                BoolExpr Expression = cICtx.MkNot(pOperand);

                return Expression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in NotOperator, " + pOperand.ToString());
                throw ex;
            }
        }

        public BoolExpr MakeBoolVariable(String pOperand)
        {
            try
            {
                BoolExpr lResult = cICtx.MkBoolConst(pOperand);

                return lResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in MakeBoolVariable, " + pOperand);
                throw ex;
            }
        }

        public void AddConstraintToSolver(BoolExpr pConstraint, String pConstraintSource)
        {
            try
            {
                //Here we have to add this constraint to the solver which is previously defined
                //Also using the solver.AssertAndTrack function which requires to use another named
                //Boolean expression to track this constraint
                int lConstraintIndex = getNextConstraintCounter();
                BoolExpr ConstraintTracker = cICtx.MkBoolConst("Constraint" + lConstraintIndex);
                cISolver.AssertAndTrack(pConstraint, ConstraintTracker);

                if (cIDebugMode)
                    cIDebugText += "(assert " + pConstraint.ToString() + "); Constraint " + lConstraintIndex + " , Source: " + pConstraintSource + "\r\n";
                //Console.WriteLine("Constraint " + lConstraintIndex + ":" + pConstraint.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddConstraintToSolver, " + pConstraint.ToString());
                throw ex;
            }
        }

        /*public Expr FindBooleanExpressionUsingName(String pExprName)
        {
            Expr resultExpr = null;
            try
            {
                //TODO: this is just for one requirement not including the &&
                if (pExprName.Contains('<') || pExprName.Contains('>') || pExprName.Contains(">=") || pExprName.Contains("<=") || pExprName.Contains("=="))
                    resultExpr = iCtx.MkBoolConst(pExprName);
                else
                {
                    Expr tempExpr = iCtx.MkConst(pExprName, iCtx.MkBoolSort());
                    foreach (Expr currentExpr in ExpressionList)
                    {
                        if (currentExpr.Equals(tempExpr))
                        {
                            resultExpr = currentExpr;
                            break;
                        }
                    }
                }
                if (resultExpr == null)
                    Console.WriteLine("error in FindBoolExpressionUsingName, Variable " + pExprName + " could not be found");

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in FindExpressionUsingName, " + pExprName);
                Console.WriteLine(ex.Message);                
            }
            return resultExpr;
        }*/

        public Expr FindExpressionUsingName(String pExprName)
        {
            Expr resultExpr = null;
            try
            {
                //TODO: this is just for one requirement not including the &&
                if (pExprName.Contains('<') || pExprName.Contains('>') || pExprName.Contains(">=") || pExprName.Contains("<=") || pExprName.Contains("=="))
                    resultExpr = cICtx.MkBoolConst(pExprName);
                else
                {
                    Expr tempExpr = cICtx.MkConst(pExprName, cICtx.MkIntSort());
                    foreach (Expr currentExpr in cExpressionList)
                    {
                        if (currentExpr.Equals(tempExpr))
                        {
                            resultExpr = currentExpr;
                            break;
                        }
                    }
                }
                if (resultExpr == null)
                    Console.WriteLine("error in FindExpressionUsingName, Variable " + pExprName + " could not be found");

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in FindExpressionUsingName, " + pExprName);
                Console.WriteLine(ex.Message);
            }
            return resultExpr;
        }

        public BoolExpr MakeBoolExprFromString(string pExpression)
        {
            BoolExpr lResultExpr = null;
            try
            {
                lResultExpr = cICtx.MkBoolConst(pExpression);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in MakeBoolExprFromString");
                Console.WriteLine(ex.Message);
            }
            return lResultExpr;
        }

        public BoolExpr FindBoolExpressionUsingName(String pExprName)
        {
            Expr resultExpr = null;
            try
            {
                Expr tempExpr = cICtx.MkConst(pExprName, cICtx.MkBoolSort());

                List<Expr> lFoundExpr = (from Expr in cExpressionList
                                 where Expr == tempExpr
                                 select Expr).ToList();
                if (lFoundExpr != null && lFoundExpr.Count != 0)
                    resultExpr = lFoundExpr[0];

                //if (resultExpr == null || lFoundExpr.Count.Equals(0))
                //    Console.WriteLine("error in FindBoolExpressionUsingName, Variable " + pExprName + " could not be found");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in FindBoolExpressionUsingName, " + pExprName);
                throw ex;
            }
            return (BoolExpr)resultExpr;
        }

        public IntExpr FindIntExpressionUsingName(String pExprName)
        {
            Expr resultExpr = null;
            try
            {
                Expr tempExpr = cICtx.MkConst(pExprName, cICtx.MkIntSort());
                
                List<Expr> lFoundExpr = (from Expr in cExpressionList
                                         where Expr == tempExpr
                                         select Expr).ToList();
                if (lFoundExpr != null)
                    resultExpr = lFoundExpr[0];

                //if (resultExpr == null)
                //    Console.WriteLine("error in FindExpressionUsingName, Variable " + pExprName + " could not be found");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in FindIntExpressionUsingName, " + pExprName);
                throw ex;
            }
            return (IntExpr)resultExpr;
        }

        public void AddBooleanExpressionWithIndex(String pExprName)
        {
            try
            {
                int newBooleanExpressionCounter = getNextBooleanExpressionCounter();

                Expr tempExpr = cICtx.MkConst(pExprName + "-V" + newBooleanExpressionCounter, cICtx.MkBoolSort());
                setBooleanExpressionCounter(newBooleanExpressionCounter);
                cExpressionList.Add(tempExpr);

                if (cIDebugMode)
                    cIDebugText += "(declare-const " + pExprName + " Bool)" + "\r\n";
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddBooleanExpressionWithIndex, " + pExprName);
                throw ex;
            }
        }

        public void AddBooleanExpression(String pExprName)
        {
            try
            {
                Expr tempExpr = cICtx.MkConst(pExprName, cICtx.MkBoolSort());
                if (!cExpressionList.Contains(tempExpr))
                {
                    cExpressionList.Add(tempExpr);

                    if (cIDebugMode)
                        cIDebugText += "(declare-const " + pExprName + " Bool)" + "\r\n";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddBooleanExpression, " + pExprName);
                throw ex;
            }
        }

        public void AddStringExpression(string pExprName)
        {
            try
            {
                //TODO: this has to be implemented
                //Expr tempExpr = iCtx.MkConst(pExprName, iCtx.mkC
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddStringExpression");
                Console.WriteLine(ex.Message);
            }
        }
        public void AddIntegerExpression(String pExprName)
        {
            try
            {
                Expr tempExpr = cICtx.MkConst(pExprName, cICtx.MkIntSort());
                if (!cExpressionList.Contains(tempExpr))
                {
                    cExpressionList.Add(tempExpr);

                    if (cIDebugMode)
                        cIDebugText += "(declare-const " + pExprName + " Int)" + "\r\n";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddIntegerExpression, " + pExprName);
                throw ex;
            }
        }

        public void PrepareDebugDirectory()
        {
            try
            {
                string exePath = Directory.GetCurrentDirectory();
                string endPath = null;

                endPath = "Output";

                System.IO.Directory.CreateDirectory(exePath + "../../../" + endPath);
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(exePath + "../../../" + endPath);
                //System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(@"C:\Users\amir\Desktop\Output\Debug");

                foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();

                endPath = "Output/Debug";
                System.IO.Directory.CreateDirectory(exePath + "../../../" + endPath);


                System.IO.DirectoryInfo directoryDebug = new System.IO.DirectoryInfo(exePath + "../../../" + endPath);
                
                foreach (System.IO.FileInfo file in directoryDebug.GetFiles()) file.Delete();

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in PrepareDebugDirectory");
                throw ex;
            }
        }


        /// <summary>
        /// This function is usd to output the previously filled DebugText to a external file with the name Debug indexed by the transition number which it was in
        /// </summary>
        /// <param name="pState">Transition number</param>
        public void WriteDebugFile(int pState, int pModelIndex)
        {
            try
            {
                string exePath = Directory.GetCurrentDirectory();
                string endPath = null;

                if (pState != -1)
                    endPath = "Output/Debug/Transition" + pState + ".txt";
                else
                    endPath = "Output/Debug/Model" + pModelIndex + ".txt";

                System.IO.File.WriteAllText(exePath + "../../../" + endPath, cIDebugText);
                //System.IO.File.WriteAllText("C:/Users/amir/Desktop/Output/Debug/Debug" + pState + ".txt",iDebugText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Status CheckSatisfiability(int pState
                                        , bool pDone
                                        , FrameworkWrapper pWrapper
                                        , bool pReportGoalAnalysis
                                        , bool pReportAnalysisDetail
                                        , bool pReportVariants
                                        , bool pReportTransitions
                                        , bool pReportAnalysisTiming
                                        , bool pReportUnsatCore
                                        , string pStrExprToCheck = "")
        {
            //Status lSatisfiabilityResult =  Status.UNKNOWN;
            Status lSat = Status.UNKNOWN;

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();


/*                if (pDone)
                {
                    //Check the whole model
                    lSat = CheckModelSatisfiability();
                    if (lSat == Status.UNSATISFIABLE)
                    {
                        Console.WriteLine("Neither a counterexample nor a satisfiable finished-result could be produced.");
                    }
                }
                else
                {
                    if (pReportGoalAnalysis)
                        //Check one specific expression
                        lSat = CheckModelSatisfiability(pStrExprToCheck);
                    else if (pStrExprToCheck != "")
                        //Check one specific expression
                        lSat = CheckModelSatisfiability(pStrExprToCheck);
                    else
                        lSat = CheckModelSatisfiability();

                }*/

                if (pStrExprToCheck != "")
                    //Check one specific expression
                    lSat = CheckModelSatisfiability(pStrExprToCheck);
                else
                    lSat = CheckModelSatisfiability();

                stopwatch.Stop();

                if (lSat == Status.SATISFIABLE)
                {
                    //lSatisfiabilityResult = true;
                    cResultModel = cISolver.Model;
/*                    OutputHandler output = new OutputHandler(wrapper);

                                        //adding expressions from model to outputhandler
                                        foreach (FuncDecl lFunctionDecleration in resultModel.ConstDecls)
                                        {
                                            Expr lCurrentExpr = FindExprInExprListWithNull(lFunctionDecleration.Name.ToString());
                                            if (lCurrentExpr != null && !lCurrentExpr.GetType().Name.Equals("IntExpr"))
                                            {
                                                string value = "" + resultModel.Evaluate(lCurrentExpr);
                                                output.addExp(lCurrentExpr.ToString(), value, pState);
                                            }
                                        }*/


/*                    if (pDone)
                    {
                        //Print and writes an output file showing the result of a finished test
                        if (pReportAnalysisTiming)
                            Console.WriteLine("Time: " + stopwatch.Elapsed);

                        if (pAnalysisDetail)
                        {
                            output.printChosenVariants();
                            output.printOperationsTransitions();
                        }
                        output.writeFinished();
                        output.writeFinishedNoPost();
                    }
                    else
                    {
                        //Print and writes an output file showing the result of a deadlocked test
                        if (pReportAnalysisDetail)
                            Console.WriteLine("Satisfiable");

                        if (pReportAnalysisTiming)
                            Console.WriteLine("Time: " + stopwatch.Elapsed);

                        if (pAnalysisDetail)
                            output.printCounterExample();
                        output.writeCounterExample();
                        output.writeCounterExampleNoPost();
                    }*/
//                    output.writeDebugFile();

                    //foreach (Expr lExpression in ExpressionList)
                    //    Console.WriteLine(lExpression.ToString() + " = " + resultModel.Evaluate(lExpression));

                    ////Taken this to the reporting function in Z3SolverEngineer
                    /*if (pReportAnalysisDetail)
                        Console.WriteLine("Satisfiable");*/

                    //Adding this model value to the assertions
                    ////AddModelItem2SolverAssertion(pWrapper, resultModel);
                    //CheckSatisfiability();
                }
                else
                {
                    //lSatisfiabilityResult = false;

                    ////Taken this to the reporting function in Z3SolverEngineer
                    /*if (pReportAnalysisDetail)
                        Console.WriteLine("Unsatisfiable");*/

/*                    //Console.WriteLine("proof: {0}", iSolver.Proof);
                    //Console.WriteLine("core: ");
                    if (pReportUnsatCore)
                    {
                        foreach (Expr c in iSolver.UnsatCore)
                        {
                            Console.WriteLine("{0}", c);
                        }
                    }*/
                }

                //TODO: in the best case all the details about the analysis including the time of the analysis 
                //should be in one class instance which can be reached by the reporting procedure in the Z3SolverEngineer
                if (pReportAnalysisDetail && pReportAnalysisTiming)
                    Console.WriteLine("Time: " + stopwatch.Elapsed);


/*                ReportSolverResult(pState
                                    , pDone
                                    , pWrapper
                                    , lSat
                                    , pReportAnalysisDetail
                                    , pReportVariants
                                    , pReportTransitions
                                    , pReportUnsatCore);*/

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CheckSatisfiability");               
                Console.WriteLine(ex.Message);
            }
            return lSat;
        }

        public void ReportSolverResult(int pState
                                        , bool pDone
                                        , FrameworkWrapper pFrameworkWrapper
                                        , Status pSatResult
                                        , bool pReportAnalysisDetail
                                        , bool pReportVariants
                                        , bool pReportTransitions
                                        , bool pReportUnsatCore)
        {
            try
            {
                if (pSatResult.Equals(Status.SATISFIABLE))
                {
                    Model resultModel = cISolver.Model;

                    OutputHandler output = new OutputHandler(pFrameworkWrapper);

                    //adding expressions from model to outputhandler
                    foreach (FuncDecl lFunctionDecleration in resultModel.ConstDecls)
                    {
                        Expr lCurrentExpr = FindExprInExprListWithNull(lFunctionDecleration.Name.ToString());
                        if (lCurrentExpr != null && !lCurrentExpr.GetType().Name.Equals("IntExpr"))
                        {
                            string value = "" + resultModel.Evaluate(lCurrentExpr);
                            output.addExp(lCurrentExpr.ToString(), value, pState);
                        }
                    }

                    if (pDone)
                    {
                        //Print and writes an output file showing the result of a finished test
                        if (pReportAnalysisDetail)
                        {
                            Console.WriteLine("Model No " + pState + ":");
                            if (pReportVariants)
                            {
                                output.printChosenVariants();
                            }
                            if (pReportTransitions)
                                output.printOperationsTransitions();
                        }
                        output.writeFinished();
                        output.writeFinishedNoPost();
                    }
                    else
                    {
                        //Print and writes an output file showing the result of a deadlocked test
                        if (pReportAnalysisDetail)
                            output.printCounterExample();
                        output.writeCounterExample();
                        output.writeCounterExampleNoPost();
                    }
                    output.writeDebugFile();
                }
                else
                {
                    //Console.WriteLine("proof: {0}", iSolver.Proof);
                    //Console.WriteLine("core: ");
                    if (pReportUnsatCore)
                    {
                        foreach (Expr c in cISolver.UnsatCore)
                        {
                            Console.WriteLine("{0}", c);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReportSolverResults");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Uses the local solver to populate the output handler
        /// </summary>
        /// <param name="pState"></param>
        /// <param name="pOutputHandler"></param>
        /// <returns></returns>
        public OutputHandler PopulateOutputHandler(int pState, OutputHandler pOutputHandler)
        {
            OutputHandler lOutputHandler = pOutputHandler;
            try
            {
                Model resultModel = cISolver.Model;

                //adding expressions from model to outputhandler
                foreach (FuncDecl lFunctionDecleration in resultModel.ConstDecls)
                {
                    Expr lCurrentExpr = FindExprInExprListWithNull(lFunctionDecleration.Name.ToString());
                    if (lCurrentExpr != null && !lCurrentExpr.GetType().Name.Equals("IntExpr"))
                    {
                        string value = "" + resultModel.Evaluate(lCurrentExpr);
                        lOutputHandler.addExp(lCurrentExpr.ToString(), value, pState);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in PopulateOutputHandler");                
                Console.WriteLine(ex.Message);
            }
            return lOutputHandler;
        }

        /// <summary>
        /// This function writes the unsat core of the solver to the console screen
        /// </summary>
        public void ConsoleWriteUnsatCore()
        {
            try
            {
                foreach (Expr c in cISolver.UnsatCore)
                    Console.WriteLine("{0}", c);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ConsoleWriteUnsatCore");
                Console.WriteLine(ex.Message);
            }
        }

        public void SolverPushFunction()
        {
            try
            {
                cISolver.Push();
                if (cIDebugMode)
                    cIDebugText += "(push); \r\n";
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in SolverPushFunction");
                Console.WriteLine(ex.Message);
            }
        }

        public void SolverPopFunction()
        {
            try
            {
                cISolver.Pop();
                if (cIDebugMode)
                    cIDebugText += "(pop); \r\n";
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in SolverPopFunction");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// This function checks the model for the satisfiability either for one specific expression or for the whole model
        /// </summary>
        /// <param name="pExprToCheck">The specific expression which needs to be checked</param>
        /// <returns>The result of checking the model</returns>
        public Status CheckModelSatisfiability(string pStrExprToCheck = "")
        {
            Status lReturnStatus = Status.UNKNOWN;
            try
            {
                if (pStrExprToCheck == "")
                    lReturnStatus = cISolver.Check();
                else
                {
                    Expr lExprToCheck = FindBoolExpressionUsingName(pStrExprToCheck);
                    lReturnStatus = cISolver.Check(lExprToCheck);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CheckModelSatisfiability");               
                Console.WriteLine(ex.Message);
            }
            return lReturnStatus;
        }

        public Expr FindExprInExprList(String pExprName)
        {
            Expr lResultExpr = null;
            try
            {
                List<Expr> lFoundExpr = (from Expr in cExpressionList
                                         where Expr.ToString().Equals(pExprName)
                                         select Expr).ToList();
                if (lFoundExpr.Count != 0)
                    lResultExpr = lFoundExpr[0];
                //else
                  //  Console.WriteLine("Error in FindExprInExprList: " + pExprName + " not found in expression list!");                

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in FindExprInExprList!");                
                Console.WriteLine(ex.Message);
            }
            return lResultExpr;
        }


        public Expr FindExprInExprListWithNull(String pExprName)
        {
            Expr lResultExpr = null;
            try
            {
                List<Expr> lFoundExpr = (from Expr in cExpressionList
                                         where Expr.ToString().Equals(pExprName)
                                         select Expr).ToList();
                if (lFoundExpr.Count != 0)
                    lResultExpr = lFoundExpr[0];

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in FindExprInExprList!");
                Console.WriteLine(ex.Message);
            }
            return lResultExpr;
        }


        //public void AddModelItem2SolverAssertion(FrameworkWrapper pFrameworkWrapper, Model pResultModel)
        public void AddModelItem2SolverAssertion(FrameworkWrapper pFrameworkWrapper)
        {
            try
            {
                //Adding this model value to the assertions
                BoolExpr addedConstraint = cICtx.MkBoolConst("AddedConstraint");

                //should we concentrate on the expressions JUST used in the given model?
                //We should negate the model itself and add it again to the constraint list
                //should we not care about the other expressions which are not used in the model?

                /////////////////////SHOULD BE REMOVED//////////////////////////////
                //Expr one = iCtx.MkNumeral(1, iCtx.MkRealSort());
                //BoolExpr tempExpression = iCtx.MkEq((ArithExpr)one, (ArithExpr)one);
                //foreach (Expr lExpression in ExpressionList)
                //{

                //    tempExpression = iCtx.MkAnd(iCtx.MkEq(lExpression, pResultModel.Evaluate(lExpression))
                //                                , tempExpression);
                //}
                /////////////////////SHOULD BE REMOVED//////////////////////////////

                BoolExpr tempExpression = null;
                string lLocalDebugText = "";
                //foreach (FuncDecl lFunctionDecleration in pResultModel.ConstDecls)
                foreach (FuncDecl lFunctionDecleration in cResultModel.ConstDecls)
                    {
                    Expr lCurrentExpr = FindExprInExprList(lFunctionDecleration.Name.ToString());
                    if (lCurrentExpr != null && !lCurrentExpr.GetType().Name.Equals("IntExpr"))  
                    {
                        if (tempExpression == null)
                        {
                            //if (pResultModel.Evaluate(lCurrentExpr).IsTrue)
                            if (cResultModel.Evaluate(lCurrentExpr).IsTrue)
                            {
                                //If the value of the variable is true
                                tempExpression = cICtx.MkNot((BoolExpr)lCurrentExpr);
                                lLocalDebugText += "(not " + lCurrentExpr.ToString() + " ) ";
                            }
                            else
                            { 
                                //If the value of the variable is false
                                tempExpression = (BoolExpr)lCurrentExpr;
                                lLocalDebugText += "( " + lCurrentExpr.ToString() + " ) ";
                            }

                        }
                        else
                        {
                            //if (!pResultModel.Evaluate(lCurrentExpr).IsTrue && !pResultModel.Evaluate(lCurrentExpr).IsFalse)
                            if (!cResultModel.Evaluate(lCurrentExpr).IsTrue && !cResultModel.Evaluate(lCurrentExpr).IsFalse)
                            {
                                //If the value of the variable is neither true nor false (it is don't care, meaning both true and false values for this variable will satisfy the model)
                                tempExpression = cICtx.MkAnd(tempExpression, cICtx.MkNot((BoolExpr)lCurrentExpr));
                                lLocalDebugText = "(and " + lLocalDebugText + " (not " + lCurrentExpr.ToString() + " ))";
                            }
                            //else if (pResultModel.Evaluate(lCurrentExpr).IsTrue)
                            else if (cResultModel.Evaluate(lCurrentExpr).IsTrue)
                            {
                                tempExpression = cICtx.MkAnd(tempExpression, cICtx.MkNot((BoolExpr)lCurrentExpr));
                                lLocalDebugText = "(and " + lLocalDebugText + " (not " + lCurrentExpr.ToString() + " )) ";
                            }
                            else
                            { 
                                tempExpression = cICtx.MkAnd(tempExpression, (BoolExpr)lCurrentExpr);
                                lLocalDebugText = "(and " + lLocalDebugText + " " + lCurrentExpr.ToString() + " ) ";
                            }
                        }
                    }
                    //Console.WriteLine(lCurrentExpr.ToString() + " = " + pResultModel.Evaluate(lCurrentExpr));
                }

                //tempExpression = iCtx.MkNot(tempExpression);
                cISolver.AssertAndTrack(tempExpression, addedConstraint);
                cIDebugText += "(assert " + lLocalDebugText + ")\r\n";

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AddModelItem2SolverAssertion!");
                Console.WriteLine(ex.Message);
            }

        }

        public BoolExpr MakeTrueNFalseExpr(bool pTrue)
        {
            BoolExpr lResultExpr;
            Expr l1 = cICtx.MkNumeral(1, cICtx.MkIntSort());
            Expr l2 = cICtx.MkNumeral(2, cICtx.MkIntSort());
            if (pTrue)
                lResultExpr = cICtx.MkEq(l1, l1);
            else
                lResultExpr = cICtx.MkEq(l1, l2);

            return lResultExpr;
        }
        /// <summary>
        /// First piece of code to get used to Z3 API
        /// </summary>
        /// <param name="ctx"></param>
        public static void MicrosoftMyGetAllModelExample(Context ctx)
        {
            Console.WriteLine("Z3Solver.MyGetAllModelExample");

            Solver solver = ctx.MkSolver();

            Expr x = ctx.MkConst("x", ctx.MkIntSort());
            Expr y = ctx.MkConst("y", ctx.MkIntSort());
            Expr z = ctx.MkConst("z", ctx.MkIntSort());
            Expr zero = ctx.MkNumeral(0, ctx.MkRealSort());
            Expr one = ctx.MkNumeral(1, ctx.MkRealSort());
            Expr five = ctx.MkNumeral(5, ctx.MkRealSort());

            BoolExpr constraint1 = ctx.MkBoolConst("Constraint1");
            solver.AssertAndTrack(ctx.MkGt((ArithExpr)x, (ArithExpr)zero), constraint1);
            BoolExpr constraint2 = ctx.MkBoolConst("Constraint2");
            solver.AssertAndTrack(ctx.MkEq((ArithExpr)y, ctx.MkAdd((ArithExpr)x, (ArithExpr)one)), constraint2);
            BoolExpr constraint3 = ctx.MkBoolConst("Constraint3");
            solver.AssertAndTrack(ctx.MkLt((ArithExpr)y, (ArithExpr)five), constraint3);

            MicrosoftCheckSatisfiability(ctx, solver, x, y, z);
        }

        /// <summary>
        /// First Piece of code just to get used to coding Z3 API
        /// </summary>
        /// <param name="pCtx"></param>
        /// <param name="pSolver"></param>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pZ"></param>
        public static void MicrosoftCheckSatisfiability(Context pCtx, Solver pSolver, Expr pX, Expr pY, Expr pZ)
        {
            Status sat = pSolver.Check();

            if (sat == Status.SATISFIABLE)
            {
                Console.WriteLine("Satisfiable");
                Model resultModel = pSolver.Model;
                Console.WriteLine("x = {0}, y = {1}, z = {2}",
                    resultModel.Evaluate(pX),
                    resultModel.Evaluate(pY),
                    resultModel.Evaluate(pZ));

                //Adding this model value to the assertions
                pSolver = MicrosoftAddModelItem2SolverAssertion(pCtx, pSolver, resultModel, pX, pY);
                MicrosoftCheckSatisfiability(pCtx, pSolver, pX, pY, pZ);
            }
            else
            {
                Console.WriteLine("Unsatisfiable");
                Console.WriteLine("proof: {0}", pSolver.Proof);
                Console.WriteLine("core: ");
                foreach (Expr c in pSolver.UnsatCore)
                {
                    Console.WriteLine("{0}", c);
                }
            }
        }

        /// <summary>
        /// First piece of code to get used to Z3 API
        /// </summary>
        /// <param name="pctx"></param>
        /// <param name="pSolver"></param>
        /// <param name="pResultModel"></param>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <returns></returns>
        public static Solver MicrosoftAddModelItem2SolverAssertion(Context pctx, Solver pSolver, Model pResultModel, Expr pX, Expr pY)
        {
            Solver resultSolver = pSolver;

            //Adding this model value to the assertions
            BoolExpr addedConstraint = pctx.MkBoolConst("AddedConstraint");
            resultSolver.AssertAndTrack(pctx.MkNot(pctx.MkAnd(pctx.MkEq((ArithExpr)pX, pResultModel.Evaluate(pX))
                                                            , pctx.MkEq((ArithExpr)pY, pResultModel.Evaluate(pY)))), addedConstraint);

            return resultSolver;
        }

    }
}
