﻿using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class Operation
    {
        #region Props
        public string Name { get; set; }
        public string Trigger { get; set; }
        public string OperationTriggerVariableName { get; set; } //set was not there, but it was setting the object so I added it
        public BoolExpr OperationTriggerVariable { get; set; }
        public List<string> Precondition { get; set; }
        public List<string> Postcondition { get; set; }
        public string Requirement { get; set; }
        public string OperationRequirementVariableName { get; set; } //set was not there, but it was setting the object so I added it
        public BoolExpr OperationRequirementVariable { get; set; }
        public string Resource { get; set; }
        #endregion

        #region Getter-Setter
        //private string _name;
        //public string Name
        //{
        //    get { return _name; }
        //    set { _name = value; }
        //}

        //private string _trigger;
        //public string Trigger
        //{
        //    get { return _trigger; }
        //    set { _trigger = value; }
        //}

        //private string _operationTriggerVariableName;
        //public string OperationTriggerVariableName
        //{
        //    get { return _operationTriggerVariableName; }
        //}

        //private BoolExpr _operationTriggerVariable;
        //public BoolExpr OperationTriggerVariable
        //{
        //    get { return _operationTriggerVariable; }
        //    set { _operationTriggerVariable = value; }
        //}

        //private List<string> _precondition;
        //public List<string> Precondition
        //{
        //    get { return _precondition; }
        //    set { _precondition = value; }
        //}

        //private List<string> _postcondition;
        //public List<string> Postcondition
        //{
        //    get { return _postcondition; }
        //    set { _postcondition = value; }
        //}

        //private string _requirement;
        //public string Requirement
        //{
        //    get { return _requirement; }
        //    set { _requirement = value; }
        //}

        //private string _operationRequirementVariableName;
        //public string OperationRequirementVariableName
        //{
        //    get { return _operationRequirementVariableName; }
        //}

        //private BoolExpr _operationRequirementVariable;
        //public BoolExpr OperationRequirementVariable
        //{
        //    get { return _operationRequirementVariable; }
        //    set { _operationRequirementVariable = value; }
        //}

        //private string _resource;
        //public string Resource
        //{
        //    get { return _resource; }
        //    set { _resource = value; }
        //}

        /*private Z3Solver _z3Solver;
        public Z3Solver Z3Solver
        {
            get { return _z3Solver; }
            set { _z3Solver = value; }
        }*/
        #endregion


        public List<OperationInstance> MyOperationInstances = new List<OperationInstance>();

        public Operation(string pOperationName, string pTrigger, string pRequirement, string pPrecondition, string pPostcondition, string pResource)
        {
            Name = pOperationName;
            Trigger = pTrigger;
            Requirement = pRequirement;
            Precondition = new List<string>();
            Postcondition = new List<string>();
            Resource = pResource;

            if (pPrecondition != null)
                if (pPrecondition != "")
                    Precondition.Add(pPrecondition);

            if (pPostcondition != null)
                if (pPostcondition != "")
                    Postcondition.Add(pPostcondition);

            //_z3Solver = pZ3Solver;
            CreateOperationVariableNames();
            //createOperationVariables();
        }

        public void AddOperationInstance(OperationInstance currOperation)
        {
            MyOperationInstances.Add(currOperation);

        }

        public OperationInstance GetOperationInstanceForTransition(int pIndex)
        {
            return MyOperationInstances[pIndex];
        }

        public BoolExpr GetResourceExpression(int pIndex)
        {
            return MyOperationInstances[pIndex].GetResourceExpression();
        }

        public void AddPrecondition(string pPrecondition)
        {
            if (pPrecondition != null)
                if (!ContainPrecondition(pPrecondition))
                    Precondition.Add(pPrecondition);
        }

        public void RemovePrecondition(string pPrecondition)
        {
            if (pPrecondition != null)
                if (ContainPrecondition(pPrecondition))
                    Precondition.Remove(pPrecondition);
        }

        public void AddPrecondition(List<string> pPreconditions)
        {
            foreach (string lPrecondition in pPreconditions)
            {
                AddPrecondition(lPrecondition);
            }
        }

        private bool ContainPrecondition(string pPrecondition)
        {
            bool lResult = false;

            foreach (string lPrecondition in Precondition)
            {
                if (lPrecondition.Equals(pPrecondition))
                    lResult = true;
            }
            return lResult;
        }

        private bool ContainPostcondition(string pPostcondition)
        {
            bool lResult = false;

            foreach (string lPostcondition in Postcondition)
            {
                if (lPostcondition.Equals(pPostcondition))
                    lResult = true;
            }
            return lResult;
        }

        public void AddPostcondition(string pPostcondition)
        {
            if (pPostcondition != null)
                if (!ContainPostcondition(pPostcondition))
                    Postcondition.Add(pPostcondition);
        }

        public void RemovePostcondition(string pPostcondition)
        {
            if (pPostcondition != null)
                if (ContainPostcondition(pPostcondition))
                    Postcondition.Remove(pPostcondition);
        }

        public void AddPostcondition(List<string> pPostconditions)
        {
            foreach (string lPostcondition in pPostconditions)
            {
                AddPostcondition(lPostcondition);
            }
        }

        private void CreateOperationVariableNames()
        {
            OperationRequirementVariableName = String.Join("_", new String[] { Name, "Requirement" });
            OperationTriggerVariableName = String.Join("_", new String[] { Name, "Trigger" });
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
