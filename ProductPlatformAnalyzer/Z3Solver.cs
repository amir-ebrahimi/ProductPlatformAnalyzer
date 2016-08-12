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
        private ArrayExpr Expressions;
        private List<Expr> ExpressionList;
        private BoolExpr Constraints;
        private ArrayList ConstraintList;
        private Solver iSolver; 
        private Context iCtx;
        private int ConstraintCounter;
        private int BooleanExpressionCounter;
        private bool iDebugMode;
        private string iDebugText;

        public Z3Solver()
        {
            iDebugText = "";
            iCtx = new Context(new Dictionary<string, string>() { { "proof", "true" } });
            using (iCtx)
            {
                this.iSolver = iCtx.MkSolver();
                this.ExpressionList = new List<Expr>();
                this.ConstraintList = new ArrayList();
            }
        }

        public void setDebugMode(bool pDebugMode)
        {
            iDebugMode = pDebugMode;
        }

        public bool getDebugMode()
        {
            return iDebugMode;
        }

        public void setExpressions(ArrayExpr pExpressions)
        {
            Expressions = pExpressions;
        }

        public void setExpressionList(List<Expr> pExpressionList)
        {
            ExpressionList = pExpressionList;
        }

        public ArrayExpr getExpression()
        {
            return Expressions;
        }

        public List<Expr> getExpressionList()
        {
            return ExpressionList;
        }

        public void setConstraints(BoolExpr pConstraints)
        {
            Constraints = pConstraints;
        }

        public void setConstraintList(ArrayList pConstraintList)
        {
            ConstraintList = pConstraintList;
        }

        public BoolExpr getConstraints()
        {
            return Constraints;
        }

        public ArrayList getConstraintList()
        {
            return ConstraintList;
        }

        public int getConstraintCounter()
        {
            return ConstraintCounter;
        }

        public void setConstraintCounter(int pCounter)
        {
            ConstraintCounter = pCounter;
        }

        public int getBooleanExpressionCounter()
        {
            return BooleanExpressionCounter;
        }

        public void setBooleanExpressionCounter(int pCounter)
        {
            BooleanExpressionCounter = pCounter;
        }

        public int getNextBooleanExpressionCounter()
        {
            int newCounter = getBooleanExpressionCounter() + 1;
            setBooleanExpressionCounter(newCounter);
            return newCounter;
        }

        public int getNextConstraintCounter()
        {
            int newCounter = getConstraintCounter() + 1;
            setConstraintCounter(newCounter);
            return newCounter;
        }

        public BoolExpr getTrueBoolExpr()
        {
            Expr one = iCtx.MkConst("1", iCtx.MkIntSort());
            Expr zero = iCtx.MkConst("0", iCtx.MkIntSort());
            return iCtx.MkGt((ArithExpr)one, (ArithExpr)zero);
        }

        public string ReturnStringElements(List<String> pList)
        {
            string lResultElements = "";

            foreach (string lElement in pList)
            {
                lResultElements += lElement;
            }
            return lResultElements;
        }

        public string ReturnBoolExprElementNames(List<BoolExpr> pList)
        {
            string lResultElementNames = "";

            foreach (BoolExpr lElement in pList)
            {
                lResultElementNames += lElement.ToString();
            }

            return lResultElementNames;
        }

        public void AddAndOperator2Constraints(List<String> pOperandList, String pConstraintSource)
        {
            try
            {
                BoolExpr Constraint = (BoolExpr)FindExpressionUsingName(pOperandList[0]);

                for (int i = 1; i < pOperandList.Count; i++)
                    Constraint = iCtx.MkAnd(Constraint, (BoolExpr)FindExpressionUsingName(pOperandList[i]));

                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddAndOperator2Constraints, params: " + ReturnStringElements(pOperandList) );
                throw ex;
            }
        }

        public void AddAndOperator2Constraints(List<BoolExpr> pOperandList, String pConstraintSource)
        {
            try
            {
                BoolExpr Constraint = pOperandList[0];

                for (int i = 1; i < pOperandList.Count; i++)
                    Constraint = iCtx.MkAnd(Constraint, pOperandList[i]);

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
                BoolExpr lResultExpression = (BoolExpr)FindExpressionUsingName(pOperandList[0]);

                for (int i = 1; i < pOperandList.Count; i++)
                    lResultExpression = iCtx.MkAnd(lResultExpression, (BoolExpr)FindExpressionUsingName(pOperandList[i]));

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
                    lResultExpression = iCtx.MkAnd(lResultExpression, pOperandList[i]);

                return lResultExpression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AndOperator, params: " + ReturnBoolExprElementNames(pOperandList));
                throw ex;
            }
        }

        public BoolExpr IffOperator(String pOperand1, String pOperand2)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lFirstOperand = FindExpressionUsingName(pOperand1);
                Expr lSecondOperand = FindExpressionUsingName(pOperand2);

                BoolExpr Expression = iCtx.MkIff((BoolExpr)lFirstOperand, (BoolExpr)lSecondOperand);

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
                BoolExpr Constraint = iCtx.MkImplies(pOperand1, pOperand2);

                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddImpliesOperator2Constraints, " + pOperand1.ToString() + " , " + pOperand2.ToString());
                throw ex;
            }
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
                Expr lFirstOperand = FindExpressionUsingName(pOperand1);
                Expr lSecondOperand = FindExpressionUsingName(pOperand2);

                BoolExpr Expression1 = iCtx.MkImplies((BoolExpr)lFirstOperand, (BoolExpr)lSecondOperand);
                BoolExpr Expression2 = iCtx.MkImplies((BoolExpr)lSecondOperand, (BoolExpr)lFirstOperand);

                BoolExpr Expression = iCtx.MkAnd(Expression1, Expression2);
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
                BoolExpr Expression1 = iCtx.MkImplies(pOperand1, pOperand2);
                BoolExpr Expression2 = iCtx.MkImplies(pOperand2, pOperand1);

                BoolExpr Expression = iCtx.MkAnd(Expression1, Expression2);
                return Expression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in TwoWayImpliesOperator, " + pOperand1.ToString() + " , " + pOperand2.ToString());
                throw ex;
            }
        }

        public void AddTwoWayImpliesOperator2Constraints(BoolExpr pOperand1, BoolExpr pOperand2, String pConstraintSource)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                BoolExpr Expression1 = iCtx.MkImplies(pOperand1, pOperand2);
                BoolExpr Expression2 = iCtx.MkImplies(pOperand2, pOperand1);

                BoolExpr Expression = iCtx.MkAnd(Expression1, Expression2);

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
                    lResultExpression = iCtx.MkIff(lResultExpression, pOperandList[i]);

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
                    lResultExpression = iCtx.MkImplies(lResultExpression, pOperandList[i]);

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
                    Constraint = iCtx.MkOr(Constraint, pOperandList[i]);

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
                    Constraint = iCtx.MkOr(Constraint, (BoolExpr)FindBoolExpressionUsingName(pOperandList[i]));

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
                    lResultExpression = iCtx.MkOr(lResultExpression, (BoolExpr)FindExpressionUsingName(pOperandList[i]));
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
                    lResultExpr = iCtx.MkOr(lResultExpr, pOperandList[i]);

                return lResultExpr;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in OrOperator, params: " + ReturnBoolExprElementNames(pOperandList));
                throw ex;
            }
        }

        public void AddXorOperator2Constraints(List<String> pOperandList, String pConstraintSource)
        {
            try
            {
                BoolExpr Constraint = (BoolExpr)FindExpressionUsingName(pOperandList[0]);

                for (int i = 1; i < pOperandList.Count; i++)
                {
                    Expr lOperand = FindExpressionUsingName(pOperandList[i]);
                    
                    Constraint = iCtx.MkOr(iCtx.MkAnd(Constraint, iCtx.MkNot((BoolExpr)lOperand))
                                            , iCtx.MkAnd(iCtx.MkNot(Constraint), (BoolExpr)lOperand));
                }

                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddXorOperator2Constraints, " + ReturnStringElements(pOperandList));
                throw ex;
            }
        }

        public BoolExpr XorOperator(List<String> pOperandList)
        {
            try
            {
                BoolExpr lResultExpression = (BoolExpr)FindBoolExpressionUsingName(pOperandList[0]);

                for (int i = 1; i < pOperandList.Count; i++)
                {
                    Expr lOperand = FindExpressionUsingName(pOperandList[i]);

                    lResultExpression = iCtx.MkOr(iCtx.MkAnd(lResultExpression, iCtx.MkNot((BoolExpr)lOperand))
                                            , iCtx.MkAnd(iCtx.MkNot(lResultExpression), (BoolExpr)lOperand));
                }
                return lResultExpression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in XorOperator, " + ReturnStringElements(pOperandList));
                throw ex;
            }
        }

        public void AddXorOperator2Constraints(List<BoolExpr> pOperandList, String pConstraintSource)
        {
            try
            {
                //We should show
                //or pOperand1 pOperand2 pOperand3 ...
                //and (=> pOperand1 (and (not pOperand2) (not pOperand3) ...))
                //    (=> pOperand2 (and (not pOperand1) (not pOperand3) ...))
                //    (=> pOperand3 (and (not pOperand1) (not pOperand2) ...))

                BoolExpr Constraint = pOperandList[0];

                for (int i = 1; i < pOperandList.Count; i++)
                {
                    BoolExpr lOperand = pOperandList[i];

                    Constraint = iCtx.MkOr(iCtx.MkAnd(Constraint, iCtx.MkNot(lOperand))
                                            , iCtx.MkAnd(iCtx.MkNot(Constraint), lOperand));
                }

                AddConstraintToSolver(Constraint, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddXorOperator2Constraints, " + ReturnBoolExprElementNames(pOperandList));
                throw ex;
            }
        }

        public BoolExpr XorOperator(List<BoolExpr> pOperandList)
        {
            try
            {
                BoolExpr lResultExpression = pOperandList[0];

                for (int i = 1; i < pOperandList.Count; i++)
                {
                    Expr lOperand = pOperandList[i];

                    lResultExpression = iCtx.MkOr(iCtx.MkAnd(lResultExpression, iCtx.MkNot((BoolExpr)lOperand))
                                            , iCtx.MkAnd(iCtx.MkNot(lResultExpression), (BoolExpr)lOperand));
                }
                return lResultExpression;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in XorOperator, " + ReturnBoolExprElementNames(pOperandList));
                throw ex;
            }
        }

        public void AddNotOperator2Constraints(String pOperand, String pConstraintSource)
        {
            try
            {
                //We assume that both operands are part of the previously defined expressions
                //Hence we don't need to find them in the array of expressions
                Expr lOperand = FindExpressionUsingName(pOperand);

                BoolExpr Constraint = iCtx.MkNot((BoolExpr)lOperand);

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
                Expr lOperand = FindExpressionUsingName(pOperand);

                BoolExpr Expression = iCtx.MkNot((BoolExpr)lOperand);

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
                BoolExpr Expression = iCtx.MkNot(pOperand);

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
                BoolExpr lResult = iCtx.MkBoolConst(pOperand);

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
                BoolExpr ConstraintTracker = iCtx.MkBoolConst("Constraint" + lConstraintIndex);
                iSolver.AssertAndTrack(pConstraint, ConstraintTracker);

                if (iDebugMode)
                    iDebugText += "(assert " + pConstraint.ToString() + "); Constraint " + lConstraintIndex + " , Source: " + pConstraintSource + "\r\n";
                //Console.WriteLine("Constraint " + lConstraintIndex + ":" + pConstraint.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddConstraintToSolver, " + pConstraint.ToString());
                throw ex;
            }
        }

        public Expr FindExpressionUsingName(String pExprName)
        {
            Expr resultExpr = null;
            try
            {
                Expr tempExpr = iCtx.MkConst(pExprName, iCtx.MkBoolSort());
                foreach (Expr currentExpr in ExpressionList)
                {
                    if (currentExpr == tempExpr)
                        resultExpr = currentExpr;
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

        public BoolExpr FindBoolExpressionUsingName(String pExprName)
        {
            Expr resultExpr = null;
            try
            {
                Expr tempExpr = iCtx.MkConst(pExprName, iCtx.MkBoolSort());

                List<Expr> lFoundExpr = (from Expr in ExpressionList
                                 where Expr == tempExpr
                                 select Expr).ToList();
                if (lFoundExpr != null)
                    resultExpr = lFoundExpr[0];

                if (resultExpr == null)
                    Console.WriteLine("error in FindExpressionUsingName, Variable " + pExprName + " could not be found");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in FindBoolExpressionUsingName, " + pExprName);
                throw ex;
            }
            return (BoolExpr)resultExpr;
        }

        public void AddBooleanExpressionWithIndex(String pExprName)
        {
            try
            {
                int newBooleanExpressionCounter = getNextBooleanExpressionCounter();

                Expr tempExpr = iCtx.MkConst(pExprName + "-V" + newBooleanExpressionCounter, iCtx.MkBoolSort());
                setBooleanExpressionCounter(newBooleanExpressionCounter);
                ExpressionList.Add(tempExpr);

                if (iDebugMode)
                    iDebugText += "(declare-const " + pExprName + " Bool)" + "\r\n";
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
                Expr tempExpr = iCtx.MkConst(pExprName, iCtx.MkBoolSort());
                if (!ExpressionList.Contains(tempExpr))
                {
                    ExpressionList.Add(tempExpr);

                    if (iDebugMode)
                        iDebugText += "(declare-const " + pExprName + " Bool)" + "\r\n";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddBooleanExpression, " + pExprName);
                throw ex;
            }
        }

        public void AddIntegerExpression(String pExprName)
        {
            try
            {
                Expr tempExpr = iCtx.MkConst(pExprName, iCtx.MkIntSort());
                if (!ExpressionList.Contains(tempExpr))
                {
                    ExpressionList.Add(tempExpr);

                    if (iDebugMode)
                        iDebugText += "(declare-const " + pExprName + " Int)" + "\r\n";
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

                endPath = "Debug";

                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(exePath + "../../../" + endPath);
                //System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(@"C:\Users\amir\Desktop\Output\Debug");

                foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteDebugFile(int pState)
        {
            try
            {
                string exePath = Directory.GetCurrentDirectory();
                string endPath = null;

                endPath = "Debug/Debug" + pState + ".txt";

                System.IO.File.WriteAllText(exePath + "../../../" + endPath, iDebugText);
                //System.IO.File.WriteAllText("C:/Users/amir/Desktop/Output/Debug/Debug" + pState + ".txt",iDebugText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

        public static void MicrosoftCheckSatisfiability(Context pCtx, Solver pSolver, Expr pX, Expr pY,Expr pZ)
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

        public bool CheckSatisfiability(int pState, bool done)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Status sat;

            if (done)
            {
                sat = iSolver.Check();
            }
            else
            {
                Expr lExprToCheck = FindBoolExpressionUsingName("P" + pState);
                sat = iSolver.Check(lExprToCheck);
            }

            

            stopwatch.Stop();

            bool lSatisfiabilityResult = false;

            if (sat == Status.SATISFIABLE)
            {
                lSatisfiabilityResult = true;
                Model resultModel = iSolver.Model;

                OutputHandler output = new OutputHandler();

                foreach (FuncDecl lFunctionDecleration in resultModel.ConstDecls)
                {
                    Expr lCurrentExpr = FindExprInExprList(lFunctionDecleration.Name.ToString());
                    if (lCurrentExpr != null)
                    {
                        string value = "" + resultModel.Evaluate(lCurrentExpr);
                        output.addExp(lCurrentExpr.ToString(), value, pState);
                    }
                }

                if (done)
                {
                    Console.WriteLine("Time: " + stopwatch.Elapsed);
                    output.printFinished();
                    output.writeFinished();
                }
                else
                {
                    Console.WriteLine("Satisfiable");
                    Console.WriteLine("Time: " + stopwatch.Elapsed);
                    output.printCounterExample();
                    output.writeCounterExample();
                }

                //foreach (Expr lExpression in ExpressionList)
                //    Console.WriteLine(lExpression.ToString() + " = " + resultModel.Evaluate(lExpression));

                //Adding this model value to the assertions
                AddModelItem2SolverAssertion(resultModel);
                //CheckSatisfiability();
            }
            else
            {
                lSatisfiabilityResult = false;
                Console.WriteLine("Unsatisfiable");
                Console.WriteLine("Time: " + stopwatch.Elapsed);
                //Console.WriteLine("proof: {0}", iSolver.Proof);
                //Console.WriteLine("core: ");
                foreach (Expr c in iSolver.UnsatCore)
                {
                    Console.WriteLine("{0}", c);
                }
            }
            return lSatisfiabilityResult;
        }

        public Expr FindExprInExprList(String pExprName)
        {
            Expr lResultExpr = null;
            try
            {
                List<Expr> lFoundExpr = (from Expr in ExpressionList
                                         where Expr.ToString().Equals(pExprName)
                                         select Expr).ToList();
                if (lFoundExpr.Count != 0)
                    lResultExpr = lFoundExpr[0];
                else
                    Console.WriteLine("Error in FindExprInExprList: " + pExprName + " not found in expression list!");                

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in FindExprInExprList!");                
                Console.WriteLine(ex.Message);
            }
            return lResultExpr;
        }

        public void AddModelItem2SolverAssertion(Model pResultModel)
        {
            //Adding this model value to the assertions
            BoolExpr addedConstraint = iCtx.MkBoolConst("AddedConstraint");

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
            foreach (FuncDecl lFunctionDecleration in pResultModel.ConstDecls)
            {
                Expr lCurrentExpr = FindExprInExprList(lFunctionDecleration.Name.ToString());
                if (lCurrentExpr != null)
                {
                    if (tempExpression == null)
                    {
                        if (pResultModel.Evaluate(lCurrentExpr).IsTrue)
                            tempExpression = (BoolExpr)lCurrentExpr;
                        else
                            tempExpression = iCtx.MkNot((BoolExpr)lCurrentExpr);

                    }
                    else
                    {
                        if (pResultModel.Evaluate(lCurrentExpr).IsTrue)
                            tempExpression = iCtx.MkAnd(tempExpression, (BoolExpr)lCurrentExpr);
                        else
                            tempExpression = iCtx.MkAnd(tempExpression, iCtx.MkNot((BoolExpr)lCurrentExpr));
                    }
                }
                //Console.WriteLine(lCurrentExpr.ToString() + " = " + pResultModel.Evaluate(lCurrentExpr));
            }

            //tempExpression = iCtx.MkNot(tempExpression);
            iSolver.AssertAndTrack(tempExpression, addedConstraint);


        }
        
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
