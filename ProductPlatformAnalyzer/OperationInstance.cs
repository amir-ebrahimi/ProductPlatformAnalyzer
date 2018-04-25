using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class OperationInstance
    {
        private Enumerations.OperationStatus _status;
        private Operation _abstractOperation;
        private int _transitionNumber;
        private string _initialVariableName;
        private string _executingVariableName;
        private string _finishedVariableName;
        private string _unusedVariableName;
        private string _operationPreconditionVariableName;

        #region 
        public Enumerations.OperationStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public Operation AbstractOperation
        {
            get { return _abstractOperation; }
            set { _abstractOperation = value; }
        }

        public int TransitionNumber
        {
            get { return _transitionNumber; }
            set { _transitionNumber = value; }
        }

        public string InitialVariableName
        {
            get { return _initialVariableName; }
        }

        public string ExecutingVariableName
        {
            get { return _executingVariableName; }
        }

        public string FinishedVariableName
        {
            get { return _finishedVariableName; }
        }

        public string UnusedVariableName
        {
            get { return _unusedVariableName; }
        }

        public string OperationPreconditionVariableName
        {
            get { return _operationPreconditionVariableName; }
        }
        #endregion

        public OperationInstance(Operation pOperation, int pTransitionNumber)
        {
            _abstractOperation = pOperation;
            _transitionNumber = pTransitionNumber;
            _status = Enumerations.OperationStatus.Inactive;
            CreateOperationInstanceVariableNames();
        }

        private void CreateOperationInstanceVariableNames()
        {
            _initialVariableName = String.Join("_", new String[] { _abstractOperation.Name, "I", _transitionNumber.ToString() });
            _executingVariableName = String.Join("_", new String[] { _abstractOperation.Name, "E", _transitionNumber.ToString() });
            _finishedVariableName = String.Join("_", new String[] { _abstractOperation.Name, "F", _transitionNumber.ToString() });
            _unusedVariableName = String.Join("_", new String[] { _abstractOperation.Name, "U", _transitionNumber.ToString() });
            _operationPreconditionVariableName = String.Join("_", new String[] { _abstractOperation.Name, "PreCondition", _transitionNumber.ToString() });
        }
    }
}
