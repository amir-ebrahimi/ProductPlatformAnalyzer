using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Z3;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ProductPlatformAnalyzer
{
    class Z3Solver
    {
        private ArrayExpr cExpressions;
        private Dictionary<string, Expr> cExpressionDictionary;
        private BoolExpr cConstraints;
        private ArrayList cConstraintList;
        private Solver cISolver; 
        private Context cICtx;
        private int cConstraintCounter;
        private int cBooleanExpressionCounter;
        private bool cIDebugMode;
        private StringBuilder cIDebugText;
        private Model cResultModel;
        private OutputHandler cOutputHandler;

        public Z3Solver(OutputHandler pOutputHandler)
        {
            cOutputHandler = pOutputHandler;

            cIDebugText = new StringBuilder();
            cICtx = new Context(new Dictionary<string, string>() { { "proof", "false" } });
            using (cICtx)
            {
                this.cISolver = cICtx.MkSolver("QF_FD");
                this.cExpressionDictionary = new Dictionary<string, Expr>();
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

        public void setExpressionList(Dictionary<string, Expr> pExpressionDictionary)
        {
            cExpressionDictionary = pExpressionDictionary;
        }

        public ArrayExpr getExpression()
        {
            return cExpressions;
        }

        public Dictionary<string, Expr> getExpressionList()
        {
            return cExpressionDictionary;
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
                cOutputHandler.printMessageToConsole("Error in getNextBooleanExpressionCounter");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("Error in getNextConstraintCounter");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return newCounter;
        }

        public BoolExpr getFalseBoolExpr()
        {
            return cICtx.MkFalse();
        }

        public string ReturnStringElements(HashSet<String> pList)
        {
            string lResultElements = "";
            try
            {
                foreach (string lElement in pList)
                    lResultElements += lElement;

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("Error in ReturnStringElements");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultElements;
        }

        public string ReturnBoolExprElementNames(HashSet<BoolExpr> pList)
        {
            string lResultElementNames = "";
            try
            {
                foreach (BoolExpr lElement in pList)
                    lResultElementNames += lElement.ToString();
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("Error in ReturnBoolExprElementNames");
                cOutputHandler.printMessageToConsole(ex.Message);
            }

            return lResultElementNames;
        }

        public void AddAndOperator2Constraints(HashSet<string> pOperandSet, string pConstraintSource)
        {
            try
            {
                BoolExpr lConstraint = null;

                foreach (string lOperand in pOperandSet)
                {
                    if (lConstraint == null)
                        lConstraint = (BoolExpr)FindExprInExprSet(lOperand);
                    else
                        lConstraint = cICtx.MkAnd(lConstraint, (BoolExpr)FindExprInExprSet(lOperand));
                }

                AddConstraintToSolver(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddAndOperator2Constraints" );
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
                cOutputHandler.printMessageToConsole("error in AddGreaterOperator2Constraints");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in AddEqualOperator2Constraints");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void AddAndOperator2Constraints(HashSet<BoolExpr> pOperandSet, string pConstraintSource)
        {
            try
            {
                BoolExpr lConstraint = null;

                foreach (BoolExpr lOperand in pOperandSet)
                {
                    if (lConstraint == null)
                        lConstraint = lOperand;
                    else
                        lConstraint = cICtx.MkAnd(lConstraint, lOperand);
                }

                AddConstraintToSolver(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddAndOperator2Constraints");
                throw ex;
            }
        }

        public BoolExpr AndOperator(HashSet<string> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpression = null;

                foreach (string lOperand in pOperandSet)
                {
                    if (lResultExpression == null)
                        lResultExpression = (BoolExpr)FindExprInExprSet(lOperand);
                    else
                        lResultExpression = cICtx.MkAnd(lResultExpression, (BoolExpr)FindExprInExprSet(lOperand));
                }

                return lResultExpression;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AndOperator");
                throw ex;
            }
        }

        public BoolExpr AndOperator(HashSet<BoolExpr> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpression = null;

                foreach (BoolExpr lOperand in pOperandSet)
                {
                    if (lResultExpression ==null)
                        lResultExpression = lOperand;
                    else
                        lResultExpression = cICtx.MkAnd(lResultExpression, lOperand);
                }

                return lResultExpression;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AndOperator");
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
                cOutputHandler.printMessageToConsole("error in GreaterThanOperator");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr GreaterThanOperator(string pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                //Expr lOperand0 = FindExpressionUsingName(pOperand0);
                Expr lOperand0 = FindExprInExprSet(pOperand0);
                lResultExpression = cICtx.MkGt((ArithExpr)lOperand0, cICtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in GreaterThanOperator");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in LessThanOperator");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr LessThanOperator(string pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                //Expr lOperand0 = FindExpressionUsingName(pOperand0);
                Expr lOperand0 = FindExprInExprSet(pOperand0);
                lResultExpression = cICtx.MkLt((ArithExpr)lOperand0, cICtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in LessThanOperator");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in GreaterOrEqualOperator");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr GreaterOrEqualOperator(string pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                //Expr lOperand0 = FindExpressionUsingName(pOperand0);
                Expr lOperand0 = FindExprInExprSet(pOperand0);
                lResultExpression = cICtx.MkGe((ArithExpr)lOperand0, cICtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in GreaterOrEqualOperator");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in LessOrEqualOperator");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr LessOrEqualOperator(string pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                //Expr lOperand0 = FindExpressionUsingName(pOperand0);
                Expr lOperand0 = FindExprInExprSet(pOperand0);
                lResultExpression = cICtx.MkLe((ArithExpr)lOperand0, cICtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in LessOrEqualOperator");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr IffOperator(string pOperand1, string pOperand2)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lFirstOperand = FindExprInExprSet(pOperand1);
                Expr lSecondOperand = FindExprInExprSet(pOperand2);

                BoolExpr Expression = cICtx.MkIff((BoolExpr)lFirstOperand, (BoolExpr)lSecondOperand);

                return Expression;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in IffOperator, " + pOperand1 + " , " + pOperand2);
                throw ex;
            }
        }

        public void AddImpliesOperator2Constraints(BoolExpr pOperand1, BoolExpr pOperand2, string pConstraintSource)
        {
            try
            {
                BoolExpr Constraint = cICtx.MkImplies(pOperand1, pOperand2);

                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddImpliesOperator2Constraints, " + pOperand1.ToString() + " , " + pOperand2.ToString());
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
                cOutputHandler.printMessageToConsole("error in ImpliesOperator");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }

        public void AddSimpleConstraint(string pConstraint, string pConstraintSource)
        {
            try
            {
                BoolExpr lConstraint = (BoolExpr)FindExprInExprSet(pConstraint);

                AddConstraintToSolver(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddSimpleConstraint, " + pConstraint);
                throw ex;
            }
        }

        public BoolExpr TwoWayImpliesOperator(string pOperand1, string pOperand2)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lFirstOperand = FindExprInExprSet(pOperand1);
                
                Expr lSecondOperand = FindExprInExprSet(pOperand2);

                BoolExpr Expression2 = cICtx.MkImplies((BoolExpr)lSecondOperand, (BoolExpr)lFirstOperand);

                BoolExpr Expression1 = cICtx.MkImplies((BoolExpr)lFirstOperand, (BoolExpr)lSecondOperand);

                BoolExpr Expression = cICtx.MkAnd(Expression1, Expression2);
                return Expression;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in TwoWayImpliesOperator, " + pOperand1 + " , " + pOperand2);
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
                cOutputHandler.printMessageToConsole("error in TwoWayImpliesOperator, " + pOperand1.ToString() + " , " + pOperand2.ToString());
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
                cOutputHandler.printMessageToConsole("error in AddTwoWayImpliesOperator2Constraints");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void AddTwoWayImpliesOperator2Constraints(BoolExpr pOperand1, BoolExpr pOperand2, string pConstraintSource)
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
                cOutputHandler.printMessageToConsole("error in AddTwoWayImpliesOperator2Constraints, " + pOperand1.ToString() + " , " + pOperand2.ToString());
                throw ex;
            }
        }

        public BoolExpr IffOperator(HashSet<BoolExpr> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpression = null ;

                foreach (BoolExpr lOperand in pOperandSet)
                {
                    if (lResultExpression == null)
                        lResultExpression = lOperand;
                    else
                        lResultExpression = cICtx.MkIff(lResultExpression, lOperand);
                }

                return lResultExpression;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in IffOperator");
                throw ex;
            }
        }

        public BoolExpr ImpliesOperator(HashSet<BoolExpr> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpression = null;

                foreach (BoolExpr lOperand in pOperandSet)
                {
                    if (lResultExpression == null)
                        lResultExpression = lOperand;
                    else
                        lResultExpression = cICtx.MkImplies(lResultExpression, lOperand);
                }

                return lResultExpression;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ImpliesOperator");
                throw ex;
            }
        }

        public void AddOrOperator2Constraints(HashSet<BoolExpr> pOperandSet, string pConstraintSource)
        {
            try
            {
                BoolExpr lConstraint = null;

                foreach (BoolExpr lOperand in pOperandSet)
                {
                    if (lConstraint == null)
                        lConstraint = lOperand;
                    else
                        lConstraint = cICtx.MkOr(lConstraint, lOperand);
                }

                AddConstraintToSolver(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddOrOperator2Constraints");
                throw ex;
            }
        }

        public void AddOrOperator2Constraints(HashSet<string> pOperandSet, string pConstraintSource)
        {
            try
            {
                BoolExpr lConstraint = null;

                foreach (string lOperand in pOperandSet)
                {
                    if (lConstraint == null)
                        lConstraint = (BoolExpr)FindExprInExprSet(lOperand);
                    else
                        lConstraint = cICtx.MkOr(lConstraint, (BoolExpr)FindExprInExprSet(lOperand));
                }

                AddConstraintToSolver(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddOrOperator2Constraints");
                throw ex;
            }
        }

        public BoolExpr OrOperator(HashSet<string> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpression = null;

                foreach (string lOperand in pOperandSet)
                {
                    if (lResultExpression == null)
                        lResultExpression = (BoolExpr)FindExprInExprSet(lOperand);
                    else
                        lResultExpression = cICtx.MkOr(lResultExpression, (BoolExpr)FindExprInExprSet(lOperand));
                }
                return lResultExpression;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in OrOperator");
                throw ex;
            }
        }

        public BoolExpr OrOperator(HashSet<BoolExpr> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpr = null;

                foreach (BoolExpr lOperand in pOperandSet)
                {
                    if (lResultExpr == null)
                        lResultExpr = lOperand;
                    else
                        lResultExpr = cICtx.MkOr(lResultExpr, lOperand);
                }
                return lResultExpr;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in OrOperator");
                throw ex;
            }
        }

        public void AddPickOneOperator2Constraints(HashSet<string> pOperandSet, string pConstraintSource)
        {
            try
            {
                BoolExpr lConstraint = null;

                foreach (string lOperand in pOperandSet)
                {
                    if (lConstraint == null)
                        lConstraint = (BoolExpr)FindExprInExprSet(lOperand);
                    else
                        lConstraint = cICtx.MkXor(lConstraint, (BoolExpr)FindExprInExprSet(lOperand));

                }
                AddConstraintToSolver(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddPickOneOperator2Constraints");
                throw ex;
            }
        }

        public BoolExpr PickOneOperator(HashSet<string> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpression = null;
                int lCounter = 1;
                foreach (string lOperand in pOperandSet)
                {
                    if (lResultExpression == null)
                        lResultExpression = (BoolExpr)FindExprInExprSet(lOperand);
                    else
                    {
                        AddBooleanExpression("Xor-helper" + lCounter);

                        BoolExpr lTempImplies;
                        BoolExpr lHelperBoolExpr;

                        if (lCounter.Equals(1))
                            lTempImplies = ImpliesOperator(new HashSet<BoolExpr>() { lResultExpression, (BoolExpr)FindExprInExprSet(lOperand) });
                        else
                            lTempImplies = ImpliesOperator(new HashSet<BoolExpr>() { (BoolExpr)FindExprInExprSet("Xor-helper" + (lCounter - 1)), (BoolExpr)FindExprInExprSet(lOperand) });

                        lHelperBoolExpr = ImpliesOperator((BoolExpr)FindExprInExprSet("Xor-helper" + lCounter), lTempImplies);
                        AddConstraintToSolver(lHelperBoolExpr, "Building Xor Operator");

                        AddConstraintToSolver((BoolExpr)FindExprInExprSet("Xor-helper" + lCounter), "Building Xor Operator");

                        lCounter++;
                    }
                }
                return lResultExpression;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in PickOneOperator");
                throw ex;
            }
        }

        public void AddPickOneOperator2Constraints(HashSet<BoolExpr> pOperandSet, string pConstraintSource)
        {
            try
            {
                BoolExpr Constraint;
                int[] lCoeffecient = new int[pOperandSet.Count];
                for (int i = 0; i < lCoeffecient.Length; i++)
			    {
                    lCoeffecient[i] = 1;			 
			    }
                BoolExpr[] lOperandsArray = new BoolExpr[pOperandSet.Count];

                int lCounter = 0;
                foreach (BoolExpr lOperand in pOperandSet)
                {
                    lOperandsArray[lCounter] = lOperand;
                    lCounter++;
                }
                Constraint = cICtx.MkPBEq(lCoeffecient, lOperandsArray, 1);
                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddPickOneOperator2Constraints");
                throw ex;
            }
        }

        public BoolExpr PickOneOperator(HashSet<BoolExpr> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpression;
                int[] lCoeffecient = new int[pOperandSet.Count];
                for (int i = 0; i < lCoeffecient.Length; i++)
                {
                    lCoeffecient[i] = 1;
                }
                BoolExpr[] lOperandsArray = new BoolExpr[pOperandSet.Count];

                int lCounter = 0;
                foreach (BoolExpr lOperand in pOperandSet)
                {
                    lOperandsArray[lCounter] = lOperand;
                    lCounter++;
                }
                lResultExpression = cICtx.MkPBEq(lCoeffecient, lOperandsArray, 1);

                return lResultExpression;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in PickOneOperator");
                throw ex;
            }
        }

        public void AddNotOperator2Constraints(string pOperand, string pConstraintSource)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lOperand = FindExprInExprSet(pOperand);

                BoolExpr Constraint = cICtx.MkNot((BoolExpr)lOperand);

                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddNotOperator2Constraints, " + pOperand);
                throw ex;
            }
        }

        public BoolExpr NotOperator(string pOperand)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lOperand = FindExprInExprSet(pOperand);

                BoolExpr Expression = cICtx.MkNot((BoolExpr)lOperand);

                return Expression;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in NotOperator, " + pOperand);
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
                cOutputHandler.printMessageToConsole("error in NotOperator, " + pOperand.ToString());
                throw ex;
            }
        }

        public BoolExpr MakeBoolVariable(string pOperand)
        {
            try
            {
                BoolExpr lResult = cICtx.MkBoolConst(pOperand);

                return lResult;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in MakeBoolVariable, " + pOperand);
                throw ex;
            }
        }

        public void AddConstraintToSolver(BoolExpr pConstraint, string pConstraintSource)
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
                    cIDebugText.Append("(assert " + pConstraint.ToString() + "); Constraint " + lConstraintIndex + " , Source: " + pConstraintSource + "\r\n");
                //cOutputHandler.printMessageToConsole("Constraint " + lConstraintIndex + ":" + pConstraint.ToString());
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddConstraintToSolver, " + pConstraint.ToString());
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
                    cOutputHandler.printMessageToConsole("error in FindExprInExprList, Variable " + pExprName + " could not be found");

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in FindExpressionUsingName, " + pExprName);
                cOutputHandler.printMessageToConsole(ex.Message);                
            }
            return resultExpr;
        }*/

        //TODO: Urgent make sure when this code is used
        /*public Expr FindExpressionUsingName(String pExprName)
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
                    foreach (Expr currentExpr in cExpressionDictionary)
                    {
                        if (currentExpr.ToString().Equals(pExprName))
                        {
                            resultExpr = currentExpr;
                            break;
                        }
                    }
                }
                if (resultExpr == null)
                    cOutputHandler.printMessageToConsole("error in FindExpressionUsingName, Variable " + pExprName + " could not be found");

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in FindExpressionUsingName, " + pExprName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return resultExpr;
        }*/

        public BoolExpr MakeBoolExprFromString(string pExpression)
        {
            BoolExpr lResultExpr = null;
            try
            {
                lResultExpr = cICtx.MkBoolConst(pExpression);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in MakeBoolExprFromString");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }

        /*public BoolExpr FindExprInExprList(String pExprName)
        {
            Expr resultExpr = null;
            try
            {
                Expr tempExpr = cICtx.MkConst(pExprName, cICtx.MkBoolSort());

                HashSet<Expr> lFoundExpr = (from Expr in cExpressionDictionary
                                 where Expr == tempExpr
                                 select Expr).ToList();
                if (lFoundExpr != null && lFoundExpr.Count != 0)
                    resultExpr = lFoundExpr[0];

                //if (resultExpr == null || lFoundExpr.Count.Equals(0))
                //    cOutputHandler.printMessageToConsole("error in FindExprInExprList, Variable " + pExprName + " could not be found");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in FindExprInExprList, " + pExprName);
                throw ex;
            }
            return (BoolExpr)resultExpr;
        }*/

        /*public IntExpr FindIntExpressionUsingName(String pExprName)
        {
            Expr resultExpr = null;
            try
            {
                Expr tempExpr = cICtx.MkConst(pExprName, cICtx.MkIntSort());
                
                HashSet<Expr> lFoundExpr = (from Expr in cExpressionDictionary
                                         where Expr == tempExpr
                                         select Expr).ToList();
                if (lFoundExpr != null)
                    resultExpr = lFoundExpr[0];

                //if (resultExpr == null)
                //    cOutputHandler.printMessageToConsole("error in FindExpressionUsingName, Variable " + pExprName + " could not be found");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in FindIntExpressionUsingName, " + pExprName);
                throw ex;
            }
            return (IntExpr)resultExpr;
        }*/

        public void AddBooleanExpressionWithIndex(string pExprName)
        {
            try
            {
                int newBooleanExpressionCounter = getNextBooleanExpressionCounter();

                Expr tempExpr = cICtx.MkConst(pExprName + "-V" + newBooleanExpressionCounter, cICtx.MkBoolSort());
                setBooleanExpressionCounter(newBooleanExpressionCounter);
                cExpressionDictionary.Add(tempExpr.ToString(), tempExpr);

                if (cIDebugMode)
                    cIDebugText.Append("(declare-const " + pExprName + " Bool)" + "\r\n");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddBooleanExpressionWithIndex, " + pExprName);
                throw ex;
            }
        }

        public void AddBooleanExpression(string pExprName)
        {
            try
            {
                Expr tempExpr = cICtx.MkConst(pExprName, cICtx.MkBoolSort());
                if (!cExpressionDictionary.ContainsKey(tempExpr.ToString()))
                {
                    cExpressionDictionary.Add(tempExpr.ToString(), tempExpr);

                    if (cIDebugMode)
                        cIDebugText.Append("(declare-const " + pExprName + " Bool)" + "\r\n");
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddBooleanExpression, " + pExprName);
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
                cOutputHandler.printMessageToConsole("error in AddStringExpression");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }
        public void AddIntegerExpression(string pExprName)
        {
            try
            {
                Expr tempExpr = cICtx.MkConst(pExprName, cICtx.MkIntSort());
                if (!cExpressionDictionary.ContainsKey(tempExpr.ToString()))
                {
                    cExpressionDictionary.Add(tempExpr.ToString(), tempExpr);

                    if (cIDebugMode)
                        cIDebugText.Append("(declare-const " + pExprName + " Int)" + "\r\n");
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddIntegerExpression, " + pExprName);
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

                foreach (System.IO.FileInfo lFile in directory.GetFiles()) lFile.Delete();

                endPath = "Output/Debug";
                System.IO.Directory.CreateDirectory(exePath + "../../../" + endPath);


                System.IO.DirectoryInfo directoryDebug = new System.IO.DirectoryInfo(exePath + "../../../" + endPath);
                
                foreach (System.IO.FileInfo lFile in directoryDebug.GetFiles())
                {
                    if (!lFile.Name.Contains("DataSummary"))
                        lFile.Delete();
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in PrepareDebugDirectory");
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

                System.IO.File.WriteAllText(exePath + "../../../" + endPath, cIDebugText.ToString());
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
                        cOutputHandler.printMessageToConsole("Neither a counterexample nor a satisfiable finished-result could be produced.");
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
                            cOutputHandler.printMessageToConsole("Time: " + stopwatch.Elapsed);

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
                            cOutputHandler.printMessageToConsole("Satisfiable");

                        if (pReportAnalysisTiming)
                            cOutputHandler.printMessageToConsole("Time: " + stopwatch.Elapsed);

                        if (pAnalysisDetail)
                            output.printCounterExample();
                        output.writeCounterExample();
                        output.writeCounterExampleNoPost();
                    }*/
//                    output.writeDebugFile();

                    //foreach (Expr lExpression in ExpressionList)
                    //    cOutputHandler.printMessageToConsole(lExpression.ToString() + " = " + resultModel.Evaluate(lExpression));

                    ////Taken this to the reporting function in Z3SolverEngineer
                    /*if (pReportAnalysisDetail)
                        cOutputHandler.printMessageToConsole("Satisfiable");*/

                    //Adding this model value to the assertions
                    ////AddModelItem2SolverAssertion(pWrapper, resultModel);
                    //CheckSatisfiability();
                }
                else
                {
                    //lSatisfiabilityResult = false;

                    ////Taken this to the reporting function in Z3SolverEngineer
                    /*if (pReportAnalysisDetail)
                        cOutputHandler.printMessageToConsole("Unsatisfiable");*/

/*                    //cOutputHandler.printMessageToConsole("proof: {0}", iSolver.Proof);
                    //cOutputHandler.printMessageToConsole("core: ");
                    if (pReportUnsatCore)
                    {
                        foreach (Expr c in iSolver.UnsatCore)
                        {
                            cOutputHandler.printMessageToConsole("{0}", c);
                        }
                    }*/
                }

                //TODO: in the best case all the details about the analysis including the time of the analysis 
                //should be in one class instance which can be reached by the reporting procedure in the Z3SolverEngineer
                if (pReportAnalysisDetail && pReportAnalysisTiming)
                    cOutputHandler.printMessageToConsole("Time: " + stopwatch.Elapsed);


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
                cOutputHandler.printMessageToConsole("error in CheckSatisfiability");               
                cOutputHandler.printMessageToConsole(ex.Message);
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

                    cOutputHandler.setFrameworkWrapper(pFrameworkWrapper);

                    //adding expressions from model to outputhandler
                    foreach (FuncDecl lFunctionDecleration in resultModel.ConstDecls)
                    {
                        //Expr lCurrentExpr = FindExprInExprListWithNull(lFunctionDecleration.Name.ToString());
                        Expr lCurrentExpr = FindExprInExprSet(lFunctionDecleration.Name.ToString());
                        if (lCurrentExpr != null && !lCurrentExpr.GetType().Name.Equals("IntExpr"))
                        {
                            string value = "" + resultModel.Evaluate(lCurrentExpr);
                            cOutputHandler.addExp(lCurrentExpr.ToString(), value, pState);
                        }
                    }

                    if (pDone)
                    {
                        //Print and writes an output file showing the result of a finished test
                        if (pReportAnalysisDetail)
                        {
                            cOutputHandler.printMessageToConsole("Model No " + pState + ":");
                            if (pReportVariants)
                            {
                                cOutputHandler.printChosenVariants();
                            }
                            if (pReportTransitions)
                                cOutputHandler.printOperationsTransitions();
                        }
                        cOutputHandler.writeFinished();
                        cOutputHandler.writeFinishedNoPost();
                    }
                    else
                    {
                        //Print and writes an output file showing the result of a deadlocked test
                        if (pReportAnalysisDetail)
                            cOutputHandler.printCounterExample();
                        cOutputHandler.writeCounterExample();
                        cOutputHandler.writeCounterExampleNoPost();
                    }
                    cOutputHandler.writeDebugFile();
                }
                else
                {
                    //cOutputHandler.printMessageToConsole("proof: {0}", iSolver.Proof);
                    //cOutputHandler.printMessageToConsole("core: ");
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
                cOutputHandler.printMessageToConsole("error in ReportSolverResults");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                    //Expr lCurrentExpr = FindExprInExprListWithNull(lFunctionDecleration.Name.ToString());
                    Expr lCurrentExpr = FindExprInExprSet(lFunctionDecleration.Name.ToString());
                    if (lCurrentExpr != null && !lCurrentExpr.GetType().Name.Equals("IntExpr"))
                    {
                        string value = "" + resultModel.Evaluate(lCurrentExpr);
                        lOutputHandler.addExp(lCurrentExpr.ToString(), value, pState);
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in PopulateOutputHandler");                
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in ConsoleWriteUnsatCore");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void SolverPushFunction()
        {
            try
            {
                cISolver.Push();
                if (cIDebugMode)
                    cIDebugText.Append("(push); \r\n");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in SolverPushFunction");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void SolverPopFunction()
        {
            try
            {
                cISolver.Pop();
                if (cIDebugMode)
                    cIDebugText.Append("(pop); \r\n");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in SolverPopFunction");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                    Expr lExprToCheck = FindExprInExprSet(pStrExprToCheck);
                    lReturnStatus = cISolver.Check(lExprToCheck);
                }


            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CheckModelSatisfiability");               
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lReturnStatus;
        }

        public Expr FindExprInExprSet(string pExprName)
        {
            Expr lResultExpr = null;
            try
            {
                /*HashSet<Expr> lFoundExpr = (from Expr in cExpressionDictionary
                                         where Expr.ToString().Equals(pExprName)
                                         select Expr).ToList();

                if (lFoundExpr.Count != 0)
                    lResultExpr = lFoundExpr[0];*/
                if (cExpressionDictionary.ContainsKey(pExprName))
                    lResultExpr = cExpressionDictionary[pExprName];
                //else
                //    cOutputHandler.printMessageToConsole("Error in FindExprInExprList: " + pExprName + " not found in expression list!");                
                //TODO: terminate program

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("Error in FindExprInExprSet!");                
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }


        /*public Expr FindExprInExprListWithNull(String pExprName)
        {
            Expr lResultExpr = null;
            try
            {
                HashSet<Expr> lFoundExpr = (from Expr in cExpressionDictionary
                                         where Expr.ToString().Equals(pExprName)
                                         select Expr).ToList();
                if (lFoundExpr.Count != 0)
                    lResultExpr = lFoundExpr[0];

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("Error in FindExprInExprList!");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }*/


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
                    Expr lCurrentExpr = FindExprInExprSet(lFunctionDecleration.Name.ToString());
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
                    //cOutputHandler.printMessageToConsole(lCurrentExpr.ToString() + " = " + pResultModel.Evaluate(lCurrentExpr));
                }

                //tempExpression = iCtx.MkNot(tempExpression);
                cISolver.AssertAndTrack(tempExpression, addedConstraint);
                cIDebugText.Append("(assert " + lLocalDebugText + ")\r\n");

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("Error in AddModelItem2SolverAssertion!");
                cOutputHandler.printMessageToConsole(ex.Message);
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
