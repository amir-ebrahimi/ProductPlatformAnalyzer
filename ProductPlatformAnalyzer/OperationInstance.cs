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
        public int TransitionNumber { get; set; }
        public int Index { get; set; }
        public string InitialVariableName { get; private set; }
        public string ExecutingVariableName { get; private set; }
        public string FinishedVariableName { get; private set; }
        public string UnusedVariableName { get; private set; }
        public Action Action_I2E { get; set; }
        public string Action_I2E_VariableName { get; private set; }
        public Action Action_E2F { get; set; }
        public string Action_E2F_VariableName { get; private set; }
        public string OperationPreconditionVariableName { get; private set; }
        public string OperationPostconditionVariableName { get; private set; }

        #endregion

        public OperationInstance(Operation pOperation
                                , int pTransitionNumber
                                , int pIndex)
        {
            AbstractOperation = pOperation;
            TransitionNumber = pTransitionNumber;
            Index = pIndex;
            Status = Enumerations.OperationStatus.Inactive;

            InitialVariableName = String.Join("_", new String[] { AbstractOperation.Name, "I", TransitionNumber.ToString() });
            ExecutingVariableName = String.Join("_", new String[] { AbstractOperation.Name, "E", TransitionNumber.ToString() });
            FinishedVariableName = String.Join("_", new String[] { AbstractOperation.Name, "F", TransitionNumber.ToString() });
            UnusedVariableName = String.Join("_", new String[] { AbstractOperation.Name, "U", TransitionNumber.ToString() });

            //Here we have to create the two Action objects Action_I2E and Action_E2F

            //AbstractOperation_I_TransitionNumber should be part of the precondition
            pOperation.Precondition.Add(InitialVariableName);

            Action lAction_I2E = new Action(AbstractOperation.Name + "_I2E", pOperation.Precondition, "and not " + InitialVariableName + " " + ExecutingVariableName);
            Action_I2E = lAction_I2E;

            //AbstractOperation_E_TransitionNumber should be part of the postcondition
            pOperation.Postcondition.Add(ExecutingVariableName);

            Action lAction_E2F = new Action(AbstractOperation.Name + "_E2F", pOperation.Postcondition, "and not " + ExecutingVariableName + " " + FinishedVariableName);
            Action_E2F = lAction_E2F;

            Action_I2E_VariableName = String.Join("_", new String[] { Action_I2E.Name, TransitionNumber.ToString() });
            Action_E2F_VariableName = String.Join("_", new String[] { Action_E2F.Name, TransitionNumber.ToString() });
            OperationPreconditionVariableName = String.Join("_", new String[] { AbstractOperation.Name, "PreCondition", TransitionNumber.ToString() });
            OperationPostconditionVariableName = String.Join("_", new String[] { AbstractOperation.Name, "PostCondition", TransitionNumber.ToString() });
        }
    }
}
