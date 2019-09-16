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
    public class Z3Solver
    {
        private Solver _iSolver;
        private Optimize _iOptimize;
        private Context _iCtx;
        private StringBuilder _iDebugText;
        private StringBuilder _iDebugOptimizerText;
        private Model _resultModel;
        private OutputHandler _outputHandler;

        public ArrayExpr Expressions { get; set; }
        public Dictionary<string, Expr> ExpressionDictionary { get; set; }
        public Dictionary<string, Expr> _optimizerExpressionDictionary;
        public BoolExpr Constraints { get; set; }
        public ArrayList ConstraintList { get; set; }
        public int ConstraintCounter { get; set; }
        public int BooleanExpressionCounter { get; set; }
        public bool DebugMode { get; set; }

        public Z3Solver(OutputHandler pOutputHandler)
        {
            _outputHandler = pOutputHandler;
            
            _iDebugText = new StringBuilder();
            _iDebugOptimizerText = new StringBuilder();
            _iCtx = new Context(new Dictionary<string, string>() { { "proof", "false" } });
            using (_iCtx)
            {
                this._iSolver = _iCtx.MkSolver("QF_FD");
                this._iOptimize = _iCtx.MkOptimize();

                this.ExpressionDictionary = new Dictionary<string, Expr>();
                this._optimizerExpressionDictionary = new Dictionary<string, Expr>();
                this.ConstraintList = new ArrayList();
            }
        }

        public int GetNextBooleanExpressionCounter()
        {
            int lNewCounter = 0;
            try
            {
                lNewCounter = BooleanExpressionCounter + 1;
                BooleanExpressionCounter = lNewCounter;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("Error in getNextBooleanExpressionCounter");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lNewCounter;
        }

        public int GetNextConstraintCounter()
        {
            int lNewCounter = 0;
            try
            {
                lNewCounter = ConstraintCounter + 1;
                ConstraintCounter = lNewCounter;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("Error in getNextConstraintCounter");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lNewCounter;
        }

        public BoolExpr GetFalseBoolExpr()
        {
            return _iCtx.MkFalse();
        }

        //Never Used
        //public string ReturnStringElements(string[] pList)
        //{
        //    string lResultElements = "";
        //    try
        //    {
        //        foreach (string lElement in pList)
        //            lResultElements += lElement;

        //    }
        //    catch (Exception ex)
        //    {
        //        _outputHandler.PrintMessageToConsole("Error in ReturnStringElements");
        //        _outputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lResultElements;
        //}

        //Never Used
        //public string ReturnBoolExprElementNames(HashSet<BoolExpr> pList)
        //{
        //    string lResultElementNames = "";
        //    try
        //    {
        //        foreach (BoolExpr lElement in pList)
        //            lResultElementNames += lElement.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        _outputHandler.PrintMessageToConsole("Error in ReturnBoolExprElementNames");
        //        _outputHandler.PrintMessageToConsole(ex.Message);
        //    }

        //    return lResultElementNames;
        //}


        public void AddAndOperator2Constraints(List<string> pOperandSet, string pConstraintSource, bool pOptimizer = false)
        {
            try
            {
                BoolExpr lConstraint = null;

                foreach (string lOperand in pOperandSet)
                {
                    if (lConstraint == null)
                        lConstraint = (BoolExpr)FindExprInExprSet(lOperand);
                    else
                        lConstraint = _iCtx.MkAnd(lConstraint, (BoolExpr)FindExprInExprSet(lOperand));
                }

                AddConstraintToSolver(lConstraint, pConstraintSource);
                if (pOptimizer)
                    AddConstraintToOptimizer(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddAndOperator2Constraints");
                throw ex;
            }
        }

        public void AddAndOperator2Constraints(List<BoolExpr> pOperandSet, string pConstraintSource)
        {
            try
            {
                BoolExpr lConstraint = null;

                foreach (BoolExpr lOperand in pOperandSet)
                {
                    if (lConstraint == null)
                        lConstraint = lOperand;
                    else
                        lConstraint = _iCtx.MkAnd(lConstraint, lOperand);
                }

                AddConstraintToSolver(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddAndOperator2Constraints");
                throw ex;
            }
        }

        //Never Used
        //public void AddGreaterOperator2Constraints(Expr pOperand1, int pOperand2, string pConstraintSource)
        //{
        //    try
        //    {
        //        Expr lOperand2 = _iCtx.MkConst("pOperand2", _iCtx.MkIntSort());
        //        BoolExpr lConstraint = _iCtx.MkGt((ArithExpr)pOperand1, (ArithExpr)lOperand2);

        //        AddConstraintToSolver(lConstraint, pConstraintSource);
        //    }
        //    catch (Exception ex)
        //    {
        //        _outputHandler.PrintMessageToConsole("error in AddGreaterOperator2Constraints");
        //        _outputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //}

        public void AddEqualOperator2Constraints(Expr pOperand1, int pOperand2, string pConstraintSource, bool pOptimizer = false)
        {
            try
            {
                //Expr lOperand2 = iCtx.MkConst(pOperand2, iCtx.MkIntSort());
                BoolExpr lConstraint = _iCtx.MkEq((ArithExpr)pOperand1, _iCtx.MkInt(pOperand2));

                AddConstraintToSolver(lConstraint, pConstraintSource);
                if (pOptimizer)
                    AddConstraintToOptimizer(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddEqualOperator2Constraints");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public BoolExpr AndOperator(List<string> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpression = null;

                foreach (string lOperand in pOperandSet)
                {
                    if (lResultExpression == null)
                        lResultExpression = (BoolExpr)FindExprInExprSet(lOperand);
                    else
                        lResultExpression = _iCtx.MkAnd(lResultExpression, (BoolExpr)FindExprInExprSet(lOperand));
                }

                return lResultExpression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AndOperator");
                throw ex;
            }
        }

        public BoolExpr AndOperator(List<BoolExpr> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpression = null;

                foreach (BoolExpr lOperand in pOperandSet)
                {
                    if (lResultExpression ==null)
                        lResultExpression = lOperand;
                    else
                        lResultExpression = _iCtx.MkAnd(lResultExpression, lOperand);
                }

                return lResultExpression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AndOperator");
                throw ex;
            }
        }

        public BoolExpr GreaterThanOperator(Expr pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                lResultExpression = _iCtx.MkGt((ArithExpr)pOperand0, _iCtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in GreaterThanOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                lResultExpression = _iCtx.MkGt((ArithExpr)lOperand0, _iCtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in GreaterThanOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr EqualOperator(Expr pOperand0, IntExpr pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                lResultExpression = _iCtx.MkEq(pOperand0, pOperand1);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in EqualOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr EqualOperator(Expr pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                lResultExpression = _iCtx.MkEq(pOperand0, _iCtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in EqualOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultExpression;
        }

        public int MaximizeExpression(Expr pArithExpr)
        {
            int lResultExpr = 0;
            try
            {
                var lExpX = _iOptimize.MkMaximize((ArithExpr)pArithExpr);
                Status lResultStatus = _iOptimize.Check();

                if (lResultStatus.Equals(Status.SATISFIABLE))
                {
                    Model lResultModel = _iOptimize.Model;
                    lResultExpr = int.Parse(lResultModel.Evaluate(pArithExpr).ToString());
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in MaximizeExpression");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }

        public BoolExpr LessThanOperator(Expr pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                lResultExpression = _iCtx.MkLt((ArithExpr)pOperand0, _iCtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in LessThanOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                lResultExpression = _iCtx.MkLt((ArithExpr)lOperand0, _iCtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in LessThanOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr GreaterOrEqualOperator(Expr pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                lResultExpression = _iCtx.MkGe((ArithExpr)pOperand0, _iCtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in GreaterOrEqualOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                lResultExpression = _iCtx.MkGe((ArithExpr)lOperand0, _iCtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in GreaterOrEqualOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr LessOrEqualOperator(Expr pOperand0, int pOperand1)
        {
            BoolExpr lResultExpression = null;
            try
            {
                lResultExpression = _iCtx.MkLe((ArithExpr)pOperand0, _iCtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in LessOrEqualOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                lResultExpression = _iCtx.MkLe((ArithExpr)lOperand0, _iCtx.MkInt(pOperand1));

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in LessOrEqualOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultExpression;
        }

        public BoolExpr IffOperator(string pOperand1, string pOperand2)
        {
            try
            {
                //We assume that both operands are Part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lFirstOperand = FindExprInExprSet(pOperand1);
                Expr lSecondOperand = FindExprInExprSet(pOperand2);

                BoolExpr Expression = _iCtx.MkIff((BoolExpr)lFirstOperand, (BoolExpr)lSecondOperand);

                return Expression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in IffOperator, " + pOperand1 + " , " + pOperand2);
                throw ex;
            }
        }

        public void AddImpliesOperator2Constraints(BoolExpr pOperand1, BoolExpr pOperand2, string pConstraintSource, bool pOptimizer = false)
        {
            try
            {
                BoolExpr Constraint = _iCtx.MkImplies(pOperand1, pOperand2);

                AddConstraintToSolver(Constraint, pConstraintSource);
                if (pOptimizer)
                    AddConstraintToOptimizer(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddImpliesOperator2Constraints, " + pOperand1.ToString() + " , " + pOperand2.ToString());
                throw ex;
            }
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
                _outputHandler.PrintMessageToConsole("error in AddSimpleConstraint, " + pConstraint);
                throw ex;
            }
        }

        public BoolExpr TwoWayImpliesOperator(string pOperand1, string pOperand2)
        {
            try
            {
                //We assume that both operands are Part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lFirstOperand = FindExprInExprSet(pOperand1);
                
                Expr lSecondOperand = FindExprInExprSet(pOperand2);

                BoolExpr Expression2 = _iCtx.MkImplies((BoolExpr)lSecondOperand, (BoolExpr)lFirstOperand);

                BoolExpr Expression1 = _iCtx.MkImplies((BoolExpr)lFirstOperand, (BoolExpr)lSecondOperand);

                BoolExpr Expression = _iCtx.MkAnd(Expression1, Expression2);
                return Expression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in TwoWayImpliesOperator, " + pOperand1 + " , " + pOperand2);
                throw ex;
            }
        }

        public BoolExpr TwoWayImpliesOperator(BoolExpr pOperand1, BoolExpr pOperand2)
        {
            try
            {
                //We assume that both operands are Part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                BoolExpr Expression1 = _iCtx.MkImplies(pOperand1, pOperand2);
                BoolExpr Expression2 = _iCtx.MkImplies(pOperand2, pOperand1);

                BoolExpr Expression = _iCtx.MkAnd(Expression1, Expression2);
                return Expression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in TwoWayImpliesOperator, " + pOperand1.ToString() + " , " + pOperand2.ToString());
                throw ex;
            }
        }

        //TODO: Should be replaced with a function which takes a list of operands!!
        public void AddTwoWayImpliesOperator2Constraints(string pOperand1, string pOperand2, string pConstraintSource)
        {
            try
            {
                BoolExpr lConstraint = TwoWayImpliesOperator(pOperand1, pOperand2);
                AddConstraintToSolver(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddTwoWayImpliesOperator2Constraints");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        //TODO: Should be replaced with a function which takes a list of operands!!
        public void AddTwoWayImpliesOperator2Constraints(BoolExpr pOperand1, BoolExpr pOperand2, string pConstraintSource, bool pOptimizer = false)
        {
            try
            {
                //We assume that both operands are Part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                BoolExpr Expression1 = _iCtx.MkImplies(pOperand1, pOperand2);
                BoolExpr Expression2 = _iCtx.MkImplies(pOperand2, pOperand1);

                BoolExpr Expression = _iCtx.MkAnd(Expression1, Expression2);

                if (pOptimizer)
                    AddConstraintToOptimizer(Expression, pConstraintSource);
                else
                    AddConstraintToSolver(Expression, pConstraintSource);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddTwoWayImpliesOperator2Constraints, " + pOperand1.ToString() + " , " + pOperand2.ToString());
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
                        lResultExpression = _iCtx.MkIff(lResultExpression, lOperand);
                }

                return lResultExpression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in IffOperator");
                throw ex;
            }
        }

        public BoolExpr ImpliesOperator(List<string> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpression = null;

                foreach (string lOperand in pOperandSet)
                {
                    if (lResultExpression == null)
                        lResultExpression = (BoolExpr)FindExprInExprSet(lOperand);
                    else
                        lResultExpression = _iCtx.MkImplies(lResultExpression, (BoolExpr)FindExprInExprSet(lOperand));
                }

                return lResultExpression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ImpliesOperator");
                throw ex;
            }
        }

        public BoolExpr ImpliesOperator(List<BoolExpr> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpression = null;

                foreach (BoolExpr lOperand in pOperandSet)
                {
                    if (lResultExpression == null)
                        lResultExpression = lOperand;
                    else
                        lResultExpression = _iCtx.MkImplies(lResultExpression, lOperand);
                }

                return lResultExpression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ImpliesOperator");
                throw ex;
            }
        }

        public void AddOrOperator2Constraints(List<BoolExpr> pOperandSet, string pConstraintSource)
        {
            try
            {
                BoolExpr lConstraint = null;

                foreach (BoolExpr lOperand in pOperandSet)
                {
                    if (lConstraint == null)
                        lConstraint = lOperand;
                    else
                        lConstraint = _iCtx.MkOr(lConstraint, lOperand);
                }

                AddConstraintToSolver(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddOrOperator2Constraints");
                throw ex;
            }
        }

        public void AddOrOperator2Constraints(List<string> pOperandSet, string pConstraintSource, bool pOptimizer = false)
        {
            try
            {
                BoolExpr lConstraint = null;

                foreach (string lOperand in pOperandSet)
                {
                    if (lConstraint == null)
                        lConstraint = (BoolExpr)FindExprInExprSet(lOperand);
                    else
                        lConstraint = _iCtx.MkOr(lConstraint, (BoolExpr)FindExprInExprSet(lOperand));
                }

                AddConstraintToSolver(lConstraint, pConstraintSource);

                if (pOptimizer)
                    AddConstraintToOptimizer(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddOrOperator2Constraints");
                throw ex;
            }
        }

        public BoolExpr OrOperator(List<string> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpression = null;

                foreach (string lOperand in pOperandSet)
                {
                    if (lResultExpression == null)
                        lResultExpression = (BoolExpr)FindExprInExprSet(lOperand);
                    else
                        lResultExpression = _iCtx.MkOr(lResultExpression, (BoolExpr)FindExprInExprSet(lOperand));
                }
                return lResultExpression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in OrOperator");
                throw ex;
            }
        }

        public BoolExpr OrOperator(List<BoolExpr> pOperandSet)
        {
            try
            {
                BoolExpr lResultExpr = null;

                foreach (BoolExpr lOperand in pOperandSet)
                {
                    if (lResultExpr == null)
                        lResultExpr = lOperand;
                    else
                        lResultExpr = _iCtx.MkOr(lResultExpr, lOperand);
                }
                return lResultExpr;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in OrOperator");
                throw ex;
            }
        }

        public void AddPickOneOperator2Constraints(List<string> pOperandSet, string pConstraintSource, bool pOptimizer = false)
        {
            try
            {
                BoolExpr lConstraint = null;

                /*foreach (string lOperand in pOperandSet)
                {
                    if (lConstraint == null)
                        lConstraint = (BoolExpr)FindExprInExprSet(lOperand);
                    else
                        lConstraint = cICtx.MkXor(lConstraint, (BoolExpr)FindExprInExprSet(lOperand));

                }*/
                lConstraint = PickOneOperator(pOperandSet);

                AddConstraintToSolver(lConstraint, pConstraintSource);
                if (pOptimizer)
                    AddConstraintToOptimizer(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddPickOneOperator2Constraints");
                throw ex;
            }
        }

        public void AddPickZeroOrOneOperator2Constraints(List<string> pOperandSet, string pConstraintSource, bool pOptimizer = false)
        {
            try
            {
                BoolExpr lConstraint = null;
                BoolExpr lZeroConstraint = null;
                BoolExpr lOneConstraint = null;

                //This is for the zero part
                lZeroConstraint = PickZeroOperand(pOperandSet);

                //This is for the one part
                lOneConstraint = PickOneOperator(pOperandSet);

                lConstraint = OrOperator(new List<BoolExpr> { lZeroConstraint, lOneConstraint });

                AddConstraintToSolver(lConstraint, pConstraintSource);
                if (pOptimizer)
                    AddConstraintToOptimizer(lConstraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddPickZeroOrOneOperator2Constraints");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public BoolExpr PickZeroOperand(List<string> pOperandNameList)
        {
            BoolExpr lResultConstraint = null;
            try
            {
                foreach (var lOperandName in pOperandNameList)
                {
                    BoolExpr lOperand = (BoolExpr)FindExprInExprSet(lOperandName);
                    BoolExpr lNegatedOperand = NotOperator(lOperand);
                    if (lResultConstraint == null)
                        lResultConstraint = lNegatedOperand;
                    else
                        lResultConstraint = AndOperator(new List<BoolExpr>{ lResultConstraint, lNegatedOperand});
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in PickZeroOperand");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultConstraint;
        }

        public BoolExpr PickZeroOperand(List<BoolExpr> pOperandList)
        {
            BoolExpr lResultConstraint = null;
            try
            {
                foreach (var lOperand in pOperandList)
                {
                    BoolExpr lNegatedOperand = NotOperator(lOperand);
                    if (lResultConstraint == null)
                        lResultConstraint = lNegatedOperand;
                    else
                        lResultConstraint = AndOperator(new List<BoolExpr> { lResultConstraint, lNegatedOperand });
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in PickZeroOperand");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultConstraint;
        }

        public BoolExpr PickZeroResource(List<Operation> pOperandList, int pTransition)
        {
            BoolExpr lResultConstraint = null;
            try
            {
                foreach (var lOperand in pOperandList)
                {
                    BoolExpr lNegatedOperand = NotOperator(lOperand.GetResourceExpression(pTransition));
                    if (lResultConstraint == null)
                        lResultConstraint = lNegatedOperand;
                    else
                        lResultConstraint = AndOperator(new List<BoolExpr> { lResultConstraint, lNegatedOperand });
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in PickZeroResource");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultConstraint;
        }

        public BoolExpr PickOneResource(List<Operation> pOperandSet, int pTransition)
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
               foreach (Operation lOperand in pOperandSet)
                {
                    lOperandsArray[lCounter] = lOperand.GetResourceExpression(pTransition);
                    lCounter++;
                }
                lResultExpression = _iCtx.MkPBEq(lCoeffecient, lOperandsArray, 1);

                return lResultExpression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in PickOneResource");
                throw ex;
            }
        }

        public void AddPickOneOperator2Constraints(List<BoolExpr> pOperandSet, string pConstraintSource)
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
                Constraint = _iCtx.MkPBEq(lCoeffecient, lOperandsArray, 1);
                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddPickOneOperator2Constraints");
                throw ex;
            }
        }

        public BoolExpr PickOneOperator(List<string> pOperandNameSet)
        {
            try
            {
                BoolExpr lResultExpression;
                int[] lCoeffecient = new int[pOperandNameSet.Count];
                for (int i = 0; i < lCoeffecient.Length; i++)
                {
                    lCoeffecient[i] = 1;
                }
                BoolExpr[] lOperandsArray = new BoolExpr[pOperandNameSet.Count];

                int lCounter = 0;
                foreach (string lOperandName in pOperandNameSet)
                {
                    lOperandsArray[lCounter] = (BoolExpr)FindExprInExprSet(lOperandName);
                    lCounter++;
                }
                lResultExpression = _iCtx.MkPBEq(lCoeffecient, lOperandsArray, 1);

                return lResultExpression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in PickOneOperator");
                throw ex;
            }
        }

        public BoolExpr PickOneOperator(List<BoolExpr> pOperandSet)
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
                lResultExpression = _iCtx.MkPBEq(lCoeffecient, lOperandsArray, 1);

                return lResultExpression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in PickOneOperator");
                throw ex;
            }
        }

        public void AddNotOperator2Constraints(string pOperand, string pConstraintSource, bool pOptimizer = false)
        {
            try
            {
                //We assume that both operands are Part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lOperand = FindExprInExprSet(pOperand);

                BoolExpr Constraint = _iCtx.MkNot((BoolExpr)lOperand);
                
                AddConstraintToSolver(Constraint, pConstraintSource);
                if (pOptimizer)
                    AddConstraintToOptimizer(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddNotOperator2Constraints, " + pOperand);
                throw ex;
            }
        }

        public IntExpr Number2Expr(int pNumber)
        {
            IntExpr lResultNum = null;

            try
            {
                lResultNum = _iCtx.MkIntConst(pNumber.ToString());
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in Number2Expr");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultNum;
        }

        public IntExpr AddOperator(List<IntExpr> pOperands)
        {
            IntExpr lResultNum = null;
            try
            {
                List<ArithExpr> lConvertedList = new List<ArithExpr>();
                foreach(IntExpr lCurrentOperand in pOperands)
                {
                    lConvertedList.Add((ArithExpr)lCurrentOperand);
                }
                lResultNum = (IntExpr)_iCtx.MkAdd(lConvertedList);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultNum;
        }

        public IntExpr MulOperator(List<IntExpr> pOperands)
        {
            IntExpr lResultNum = null;
            try
            {
                List<ArithExpr> lConvertedList = new List<ArithExpr>();
                foreach (IntExpr lCurrentOperand in pOperands)
                {
                    lConvertedList.Add((ArithExpr)lCurrentOperand);
                }
                lResultNum = (IntExpr)_iCtx.MkMul(lConvertedList);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in MulOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultNum;
        }

        public BoolExpr NotOperator(string pOperand)
        {
            try
            {
                //We assume that both operands are Part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lOperand = FindExprInExprSet(pOperand);

                BoolExpr Expression = _iCtx.MkNot((BoolExpr)lOperand);

                return Expression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in NotOperator, " + pOperand);
                throw ex;
            }
        }

        public BoolExpr NotOperator(BoolExpr pOperand)
        {
            try
            {
                //We assume that both operands are Part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                BoolExpr Expression = _iCtx.MkNot(pOperand);

                return Expression;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in NotOperator, " + pOperand.ToString());
                throw ex;
            }
        }

        public BoolExpr MakeBoolVariable(string pOperand)
        {
            try
            {
                BoolExpr lResult = _iCtx.MkBoolConst(pOperand);

                return lResult;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in MakeBoolVariable, " + pOperand);
                throw ex;
            }
        }

        public void AddConstraintToOptimizer(BoolExpr pConstraint, string pConstraintSource)
        {
            try
            {
                //Here we have to add this constraint to the solver which is previously defined
                //Also using the solver.AssertAndTrack function which requires to use another named
                //Boolean expression to track this constraint
                _iOptimize.Assert(pConstraint);

                if (DebugMode)
                    _iDebugOptimizerText.Append("(assert " + pConstraint.ToString() + "); Optimizer - " + pConstraintSource + "\r\n");
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddConstraintToOptimizer");
                throw ex;
            }
        }

        public void AddConstraintToSolver(BoolExpr pConstraint, string pConstraintSource, bool pOptimizer = false)
        {
            try
            {
                //Here we have to add this constraint to the solver which is previously defined
                //Also using the solver.AssertAndTrack function which requires to use another named
                //Boolean expression to track this constraint
                int lConstraintIndex = GetNextConstraintCounter();
                BoolExpr ConstraintTracker = _iCtx.MkBoolConst("Constraint" + lConstraintIndex);

                _iSolver.AssertAndTrack(pConstraint, ConstraintTracker);
                if (pOptimizer)
                    AddConstraintToOptimizer(pConstraint, pConstraintSource);
                
                if (DebugMode)
                    _iDebugText.Append("(assert " + pConstraint.ToString() + "); Constraint " + lConstraintIndex + " , Source: " + pConstraintSource + "\r\n");
                //cOutputHandler.printMessageToConsole("Constraint " + lConstraintIndex + ":" + pConstraint.ToString());
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddConstraintToSolver, " + pConstraint.ToString());
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
                lResultExpr = _iCtx.MkBoolConst(pExpression);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in MakeBoolExprFromString");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                int newBooleanExpressionCounter = GetNextBooleanExpressionCounter();

                Expr tempExpr = _iCtx.MkConst(pExprName + "-V" + newBooleanExpressionCounter, _iCtx.MkBoolSort());
                BooleanExpressionCounter = newBooleanExpressionCounter;
                ExpressionDictionary.Add(tempExpr.ToString(), tempExpr);

                if (DebugMode)
                    _iDebugText.Append("(declare-const " + pExprName + " Bool)" + "\r\n");
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddBooleanExpressionWithIndex, " + pExprName);
                throw ex;
            }
        }

        public BoolExpr AddBooleanExpression(string pExprName, string pExtraDescription = "")
        {
            BoolExpr lResult = null;
            try
            {
                Expr tempExpr = _iCtx.MkConst(pExprName, _iCtx.MkBoolSort());
                if (pExtraDescription.Equals("Optimizer"))
                {
                    if (!_optimizerExpressionDictionary.ContainsKey(tempExpr.ToString()))
                    {
                        _optimizerExpressionDictionary.Add(tempExpr.ToString(), tempExpr);

                        if (DebugMode)
                            _iDebugOptimizerText.Append("(declare-const " + pExprName + " Bool);" + pExtraDescription + "\r\n");
                    }
                }
                else
                {
                    if (!ExpressionDictionary.ContainsKey(tempExpr.ToString()))
                    {
                        ExpressionDictionary.Add(tempExpr.ToString(), tempExpr);

                        if (DebugMode)
                            _iDebugText.Append("(declare-const " + pExprName + " Bool);" + pExtraDescription + "\r\n");
                    }
                }
                lResult = (BoolExpr)tempExpr;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddBooleanExpression, " + pExprName);
                throw ex;
            }
            return lResult;
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
                _outputHandler.PrintMessageToConsole("error in AddStringExpression");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public IntExpr AddIntegerExpression(string pExprName, string pExtraDescription = "")
        {
            IntExpr lReturnedVariable = null;
            try
            {
                //Expr tempExpr = cICtx.MkConst(pExprName, cICtx.MkIntSort());
                lReturnedVariable = (IntExpr)_iCtx.MkIntConst(pExprName);

                if (!_optimizerExpressionDictionary.ContainsKey(lReturnedVariable.ToString()))
                {
                    _optimizerExpressionDictionary.Add(lReturnedVariable.ToString(), lReturnedVariable);

                    if (DebugMode)
                        _iDebugOptimizerText.Append("(declare-const " + pExprName + " Int) ;"+ pExtraDescription + "\r\n");
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddIntegerExpression, " + pExprName);
                throw ex;
            }
            return lReturnedVariable;
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
                    if (!lFile.Name.Contains("DataSummary") && !lFile.Name.Contains("DataFileXML"))
                        lFile.Delete();
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in PrepareDebugDirectory");
                throw ex;
            }
        }


        /// <summary>
        /// This function is usd to output the previously filled DebugText to a external file with the name Debug indexed by the transition number which it was in
        /// </summary>
        /// <param name="pState">Transition number</param>
        public void WriteDebugFile(String pState, int pModelIndex, string pCustomFileName = "", string pCustomFileData = "")
        {
            try
            {
                string exePath = Directory.GetCurrentDirectory();
                string endPath = null;

                if (pCustomFileName != "")
                    endPath = "Output/Debug/" + pCustomFileName + ".txt";
                else
                {
                    if (pState != "-1")
                        endPath = "Output/Debug/Transition" + pState + ".txt";
                    else
                        endPath = "Output/Debug/Model" + pModelIndex + ".txt";
                }

                if (pCustomFileData== "")
                {
                    System.IO.File.WriteAllText(exePath + "../../../" + endPath, _iDebugText.ToString());

                    //endPath.Replace("Transition", "Optimizer");
                    //System.IO.File.WriteAllText(exePath + "../../../" + endPath, cIDebugOptimizerText.ToString());

                }
                else
                    System.IO.File.WriteAllText(exePath + "../../../" + endPath, pCustomFileData);

                //System.IO.File.WriteAllText("C:/Users/amir/Desktop/Output/Debug/Debug" + pState + ".txt",iDebugText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Status CheckSatisfiability(string pState
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
                    _resultModel = _iSolver.Model;
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
                    _outputHandler.PrintMessageToConsole("Time: " + stopwatch.Elapsed);


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
                _outputHandler.PrintMessageToConsole("error in CheckSatisfiability");               
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lSat;
        }

        public void ReportSolverResult(string pState
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
                    Model resultModel = _iSolver.Model;

                    _outputHandler.SetFrameworkWrapper(pFrameworkWrapper);

                    //adding expressions from model to outputhandler
                    foreach (FuncDecl lFunctionDecleration in resultModel.ConstDecls)
                    {
                        //Expr lCurrentExpr = FindExprInExprListWithNull(lFunctionDecleration.Name.ToString());
                        Expr lCurrentExpr = FindExprInExprSet(lFunctionDecleration.Name.ToString());
                        if (lCurrentExpr != null && !lCurrentExpr.GetType().Name.Equals("IntExpr"))
                        {
                            string value = "" + resultModel.Evaluate(lCurrentExpr);
                            _outputHandler.AddExp(lCurrentExpr.ToString(), value, pState);
                        }
                    }

                    if (pDone)
                    {
                        //Print and writes an output file showing the result of a finished test
                        if (pReportAnalysisDetail)
                        {
                            _outputHandler.PrintMessageToConsole("Model No " + pState + ":");
                            if (pReportVariants)
                            {
                                _outputHandler.PrintChosenVariants();
                            }
                            if (pReportTransitions)
                                _outputHandler.PrintOperationsTransitions();
                        }
                        _outputHandler.WriteFinished();
                        _outputHandler.WriteFinishedNoPost();
                    }
                    else
                    {
                        //Print and writes an output file showing the result of a deadlocked test
                        if (pReportAnalysisDetail)
                            _outputHandler.PrintCounterExample();
                        _outputHandler.WriteCounterExample();
                        _outputHandler.WriteCounterExampleNoPost();
                    }
                    _outputHandler.WriteDebugFile();
                }
                else
                {
                    //cOutputHandler.printMessageToConsole("proof: {0}", iSolver.Proof);
                    //cOutputHandler.printMessageToConsole("core: ");
                    if (pReportUnsatCore)
                    {
                        foreach (Expr c in _iSolver.UnsatCore)
                        {
                            Console.WriteLine("{0}", c);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ReportSolverResults");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// Uses the local solver to populate the output handler
        /// </summary>
        /// <param name="pState"></param>
        /// <param name="pOutputHandler"></param>
        /// <returns></returns>
        public OutputHandler PopulateOutputHandler(string pState, OutputHandler pOutputHandler)
        {
            OutputHandler lOutputHandler = pOutputHandler;
            try
            {
                Model resultModel = _iSolver.Model;

                //adding expressions from model to outputhandler
                foreach (FuncDecl lFunctionDecleration in resultModel.ConstDecls)
                {
                    //Expr lCurrentExpr = FindExprInExprListWithNull(lFunctionDecleration.Name.ToString());
                    Expr lCurrentExpr = FindExprInExprSet(lFunctionDecleration.Name.ToString(), true);
                    if (lCurrentExpr != null 
                        && !lCurrentExpr.GetType().Name.Equals("IntExpr")
                        )
                    {
                        string value = "" + resultModel.Evaluate(lCurrentExpr);
                        lOutputHandler.AddExp(lCurrentExpr.ToString(), value, pState);
                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in PopulateOutputHandler");                
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                foreach (Expr c in _iSolver.UnsatCore)
                    Console.WriteLine("{0}", c);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ConsoleWriteUnsatCore");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void SolverPushFunction()
        {
            try
            {
                _iSolver.Push();
                if (DebugMode)
                    _iDebugText.Append("(push); \r\n");
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in SolverPushFunction");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void SolverPopFunction()
        {
            try
            {
                _iSolver.Pop();
                if (DebugMode)
                    _iDebugText.Append("(pop); \r\n");
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in SolverPopFunction");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                if (pStrExprToCheck.Equals(""))
                    lReturnStatus = _iSolver.Check();
                    
                    
                else
                {
                    Expr lExprToCheck = FindExprInExprSet(pStrExprToCheck);
                    lReturnStatus = _iSolver.Check(lExprToCheck);
                }


            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CheckModelSatisfiability");               
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lReturnStatus;
        }

        public Expr FindExprInExprSet(string pExprName, bool pJustFind = false, string pExtraDescription = "")
        {
            Expr lResultExpr = null;
            try
            {
                /*HashSet<Expr> lFoundExpr = (from Expr in cExpressionDictionary
                                         where Expr.ToString().Equals(pExprName)
                                         select Expr).ToList();

                if (lFoundExpr.Count != 0)
                    lResultExpr = lFoundExpr[0];*/
                if (pExtraDescription.Equals("Optimizer"))
                {
                    if (_optimizerExpressionDictionary.ContainsKey(pExprName))
                        lResultExpr = _optimizerExpressionDictionary[pExprName];
                    else
                    {
                        if (!pJustFind)
                            lResultExpr = AddBooleanExpression(pExprName, pExtraDescription);
                    }
                }
                else
                {
                    if (ExpressionDictionary.ContainsKey(pExprName))
                        lResultExpr = ExpressionDictionary[pExprName];
                    else
                    {
                        if (!pJustFind)
                            lResultExpr = AddBooleanExpression(pExprName, pExtraDescription);
                    }
                }
                //    cOutputHandler.printMessageToConsole("Error in FindExprInExprList: " + pExprName + " not found in expression list!");                
                //TODO: terminate program

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("Error in FindExprInExprSet!");                
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                BoolExpr addedConstraint = _iCtx.MkBoolConst("AddedConstraint");

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

                BoolExpr lTempExpression = null;
                string lLocalDebugText = "";
                //foreach (FuncDecl lFunctionDecleration in pResultModel.ConstDecls)
                foreach (FuncDecl lFunctionDecleration in _resultModel.ConstDecls)
                    {
                    Expr lCurrentExpr = FindExprInExprSet(lFunctionDecleration.Name.ToString());
                    if (lCurrentExpr != null && !lCurrentExpr.GetType().Name.Equals("IntExpr"))  
                    {
                        if (lTempExpression == null)
                        {
                            //if (pResultModel.Evaluate(lCurrentExpr).IsTrue)
                            if (_resultModel.Evaluate(lCurrentExpr).IsTrue)
                            {
                                //If the value of the variable is true
                                lTempExpression = _iCtx.MkNot((BoolExpr)lCurrentExpr);
                                lLocalDebugText += "(not " + lCurrentExpr.ToString() + " ) ";
                            }
                            else
                            { 
                                //If the value of the variable is false
                                lTempExpression = (BoolExpr)lCurrentExpr;
                                if (lTempExpression.ToString().Contains(' '))
                                    lLocalDebugText += "( " + lCurrentExpr.ToString() + " ) ";
                                else
                                    lLocalDebugText += lCurrentExpr.ToString();
                            }

                        }
                        else
                        {
                            //if (!pResultModel.Evaluate(lCurrentExpr).IsTrue && !pResultModel.Evaluate(lCurrentExpr).IsFalse)
                            if (!_resultModel.Evaluate(lCurrentExpr).IsTrue && !_resultModel.Evaluate(lCurrentExpr).IsFalse)
                            {
                                //If the value of the variable is neither true nor false (it is don't care, meaning both true and false values for this variable will satisfy the model)
                                lTempExpression = _iCtx.MkOr(lTempExpression, _iCtx.MkNot((BoolExpr)lCurrentExpr));
                                lLocalDebugText = "(or " + lLocalDebugText + " (not " + lCurrentExpr.ToString() + " ))";
                            }
                            //else if (pResultModel.Evaluate(lCurrentExpr).IsTrue)
                            else if (_resultModel.Evaluate(lCurrentExpr).IsTrue)
                            {
                                lTempExpression = _iCtx.MkOr(lTempExpression, _iCtx.MkNot((BoolExpr)lCurrentExpr));
                                lLocalDebugText = "(or " + lLocalDebugText + " (not " + lCurrentExpr.ToString() + " )) ";
                            }
                            else
                            { 
                                lTempExpression = _iCtx.MkOr(lTempExpression, (BoolExpr)lCurrentExpr);
                                lLocalDebugText = "(or " + lLocalDebugText + " " + lCurrentExpr.ToString() + " ) ";
                            }
                        }
                    }
                    //cOutputHandler.printMessageToConsole(lCurrentExpr.ToString() + " = " + pResultModel.Evaluate(lCurrentExpr));
                }

                //tempExpression = iCtx.MkNot(tempExpression);
                _iSolver.AssertAndTrack(lTempExpression, addedConstraint);
                _iDebugText.Append("(assert " + lLocalDebugText + ")\r\n");

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("Error in AddModelItem2SolverAssertion!");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public BoolExpr MakeTrueExpr()
        {
            BoolExpr lResultExpr;
            //Expr l1 = cICtx.MkNumeral(1, cICtx.MkIntSort());
            //Expr l2 = cICtx.MkNumeral(2, cICtx.MkIntSort());
            //if (pTrue)
            //    lResultExpr = cICtx.MkEq(l1, l1);
            //else
            //    lResultExpr = cICtx.MkEq(l1, l2);

            lResultExpr = _iCtx.MkTrue();

            return lResultExpr;
        }

        public BoolExpr MakeFalseExpr()
        {
            BoolExpr lResultExpr;
            //Expr l1 = cICtx.MkNumeral(1, cICtx.MkIntSort());
            //Expr l2 = cICtx.MkNumeral(2, cICtx.MkIntSort());
            //if (pTrue)
            //    lResultExpr = cICtx.MkEq(l1, l1);
            //else
            //    lResultExpr = cICtx.MkEq(l1, l2);

            lResultExpr = _iCtx.MkFalse();

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
