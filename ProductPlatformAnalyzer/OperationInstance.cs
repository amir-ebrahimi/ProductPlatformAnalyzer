using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class OperationInstance
    {
        #region 
        public Enumerations.OperationStatus Status { get; set; }
        public Operation AbstractOperation { get; set; }
        public string TransitionNumber { get; set; }
        //public int Index { get; set; }
        public string InitialVariableName { get; private set; }
        public BoolExpr InitialVariable { get; set; }
        public string ExecutingVariableName { get; private set; }
        public BoolExpr ExecutingVariable { get; set; }
        public string FinishedVariableName { get; private set; }
        public BoolExpr FinishedVariable { get; set; }
        public string UnusedVariableName { get; private set; }
        public BoolExpr UnusedVariable { get; set; }
        public Action Action_I2E { get; set; }
        public string Action_I2E_VariableName { get; private set; }
        public Action Action_E2F { get; set; }
        public string Action_E2F_VariableName { get; private set; }
        public string OperationPreconditionVariableName { get; private set; }
        public BoolExpr OperationPreconditionVariable { get; set; }
        public string OperationPostconditionVariableName { get; private set; }
        public BoolExpr OperationPostconditionVariable { get; set; }

        OperationInstance MyNextOperationInstance = null;

        #endregion

        public OperationInstance CreateNextOperationInstance(bool pAddToList = true, bool pIncludeResources = false)
        {
            string lNextTransition = "";

            if (TransitionNumber.Equals("K"))
                lNextTransition = "K+1";
            else
                lNextTransition = (int.Parse(TransitionNumber) + 1).ToString();

            //MyNextOperationInstance = new OperationInstance(AbstractOperation, lNextTransition, Index, pAddToList);
            MyNextOperationInstance = new OperationInstance(AbstractOperation, lNextTransition, pAddToList, pIncludeResources);
            
            return MyNextOperationInstance;

        }

        public BoolExpr GetResourceExpression()
        {
            return ExecutingVariable;
        }

        public OperationInstance NextOperationInstance()
        {
            return MyNextOperationInstance;
        }

        public OperationInstance GetOperationInstanceForTransition(int pIndex)
        {
            return AbstractOperation.GetOperationInstanceForTransition(pIndex);
        }

        public OperationInstance(Operation pOperation
                                , string pTransitionNumber
                                , bool add = true
                                , bool pIncludeResources = false)
        {
            AbstractOperation = pOperation;
            TransitionNumber = pTransitionNumber;
            //Index = pIndex;
            Status = Enumerations.OperationStatus.Inactive;

            if (add)
            {
                AbstractOperation.AddOperationInstance(this);
            }
            InitialVariableName = String.Join("_", new String[] { AbstractOperation.Name, "I", TransitionNumber });
            ExecutingVariableName = String.Join("_", new String[] { AbstractOperation.Name, "E", TransitionNumber });
            FinishedVariableName = String.Join("_", new String[] { AbstractOperation.Name, "F", TransitionNumber });
            UnusedVariableName = String.Join("_", new String[] { AbstractOperation.Name, "U", TransitionNumber });

            //Here we have to create the two Action objects Action_I2E and Action_E2F

            //AbstractOperation_I_TransitionNumber should be Part of the precondition
            //pOperation.Precondition.Add(InitialVariableName);

            List<string> lTempPrecondition = new List<string>();
            foreach (var lPrecondition in pOperation.Precondition)
	        {
                lTempPrecondition.Add(lPrecondition);
	        }
            lTempPrecondition.Add(InitialVariableName);

            if (pIncludeResources)
                if (pOperation.Resource != null)
                    if (!pOperation.Resource.Equals(""))
                    {
                        lTempPrecondition.Add(pOperation.Resource);
                    }

            string lAction_I2E_Name = String.Join("_", new String[] { AbstractOperation.Name, "I2E", TransitionNumber });

            string lAction_I2E_Effect = "";
            if (pIncludeResources)
                if (pOperation.Resource != null)
                    if (!pOperation.Resource.Equals(""))
                    {
                        lAction_I2E_Effect = "and and not " + InitialVariableName + " " + ExecutingVariableName + " not " + pOperation.Resource;
                        
                    }
                    else
                        lAction_I2E_Effect = "and not " + InitialVariableName + " " + ExecutingVariableName;
            else
                lAction_I2E_Effect = "and not " + InitialVariableName + " " + ExecutingVariableName;

            Action lAction_I2E = new Action(lAction_I2E_Name
                                            , Enumerations.ActionType.I2E
                                            , lTempPrecondition
                                            , lAction_I2E_Effect
                                            , this);
            Action_I2E = lAction_I2E;

            //AbstractOperation_E_TransitionNumber should be Part of the postcondition
            //pOperation.Postcondition.Add(ExecutingVariableName);

            List<string> lTempPostcondition = new List<string>();
            foreach (var lPostcondition in pOperation.Postcondition)
            {
                lTempPostcondition.Add(lPostcondition);
            }
            lTempPostcondition.Add(ExecutingVariableName);

            if (pIncludeResources)
                if (pOperation.Resource != null)
                    if (!pOperation.Resource.Equals(""))
                    {
                        lTempPostcondition.Add("not " + pOperation.Resource);
                    }

            string lAction_E2F_Name = String.Join("_", new String[]{AbstractOperation.Name, "E2F", TransitionNumber});

            string lAction_E2F_Effect = "";

            if (pIncludeResources)
                if (pOperation.Resource!= null)
                    if (!pOperation.Resource.Equals(""))
                        lAction_E2F_Effect = "and and not " + ExecutingVariableName + " " + FinishedVariableName + " " + pOperation.Resource;
                    else
                        lAction_E2F_Effect = "and not " + ExecutingVariableName + " " + FinishedVariableName;
            else
                lAction_E2F_Effect = "and not " + ExecutingVariableName + " " + FinishedVariableName;

            Action lAction_E2F = new Action(lAction_E2F_Name
                                            , Enumerations.ActionType.E2F
                                            , lTempPostcondition
                                            , lAction_E2F_Effect
                                            , this);
            Action_E2F = lAction_E2F;

            //TODO: Maybe these two variables are redundant now with the correction to action name!
            Action_I2E_VariableName = Action_I2E.Name;
            Action_E2F_VariableName = Action_E2F.Name;

            //The calculated precondition and postcondition has to update the operation corresponding variables as well
            //First we clear the previous pre and post conditions and then we add the new calculated pre and post conditions
            //pOperation.Precondition.Clear();
            //pOperation.AddPrecondition(lTempPrecondition);

            //pOperation.Postcondition.Clear();
            //pOperation.AddPostcondition(lTempPostcondition);
            
            OperationPreconditionVariableName = String.Join("_", new String[] { AbstractOperation.Name, "PreCondition", TransitionNumber });
            OperationPostconditionVariableName = String.Join("_", new String[] { AbstractOperation.Name, "PostCondition", TransitionNumber });
        }
    }
}
