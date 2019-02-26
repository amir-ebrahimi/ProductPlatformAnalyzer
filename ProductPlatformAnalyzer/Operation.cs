using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class Operation
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _trigger;
        public string Trigger
        {
            get { return _trigger; }
            set { _trigger = value; }
        }

        private string _operationTriggerVariableName;
        public string OperationTriggerVariableName
        {
            get { return _operationTriggerVariableName; }
        }

        private List<string> _precondition;
        public List<string> Precondition
        {
            get { return _precondition; }
            set { _precondition = value; }
        }

        private List<string> _postcondition;
        public List<string> Postcondition
        {
            get { return _postcondition; }
            set { _postcondition = value; }
        }

        private string _requirement;
        public string Requirement
        {
            get { return _requirement; }
            set { _requirement = value; }
        }

        private string _operationRequirementVariableName;
        public string OperationRequirementVariableName
        {
            get { return _operationRequirementVariableName; }
        }

        /*private Z3Solver _z3Solver;
        public Z3Solver Z3Solver
        {
            get { return _z3Solver; }
            set { _z3Solver = value; }
        }*/

        public Operation(string pOperationName, string pTrigger, string pRequirement, string pPrecondition, string pPostcondition)
        {
            _name = pOperationName;
            _trigger = pTrigger;
            _requirement = pRequirement;
            _precondition = new List<string>();
            _postcondition = new List<string>();

            if (pPrecondition != null)
                if (pPrecondition != "")
                    _precondition.Add(pPrecondition);

            if (pPostcondition != null)
                if (pPostcondition != "")
                    _postcondition.Add(pPostcondition);

            //_z3Solver = pZ3Solver;
            createOperationVariableNames();
            //createOperationVariables();
        }

        public void AddPrecondition(string pPrecondition)
        {
            if (pPrecondition != null)
                _precondition.Add(pPrecondition);
        }

        public void AddPostcondition(string pPostcondition)
        {
            if (pPostcondition != null)
                _postcondition.Add(pPostcondition);
        }

        private void createOperationVariableNames()
        {
            _operationRequirementVariableName = String.Join("_", new String[] { _name, "Requirement" });
            _operationTriggerVariableName = String.Join("_", new String[] { _name, "Trigger" });
        }

        /*private void createOperationVariables()
        {
            try
            {
                if (_operationTriggerVariableName != null)
                    Z3Solver.AddBooleanExpression(_operationTriggerVariableName);
                if (_operationRequirementVariableName != null)
                    Z3Solver.AddBooleanExpression(_operationRequirementVariableName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }*/
    }
}
