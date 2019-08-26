using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class Action
    {
        public string Name { get; set; }
        public ProductPlatformAnalyzer.Enumerations.ActionType Type { get; set; }
        public List<string> Precondition { get; set; }
        public string Effect { get; set; }
        public OperationInstance MyOperationInstance { get; set; }
        public BoolExpr Variable { get; set; }

        public Action(string pName
                        , ProductPlatformAnalyzer.Enumerations.ActionType pType
                        , List<string> pPrecondition
                        , string pEffect
                        , OperationInstance pOperationInstance)
        {
            Name = pName;
            Type = pType;
            Precondition = pPrecondition;
            Effect = pEffect;
            MyOperationInstance = pOperationInstance;
        }

        public OperationInstance GetOperationInstanceForTransition(int pIndex)
        {
            return MyOperationInstance.getOperationInstanceForTransition(pIndex);
        }

        public Action GetActionForTransition(int pIndex)
        {
            if (Type == Enumerations.ActionType.I2E)
            {
                return GetOperationInstanceForTransition(pIndex).Action_I2E;
            }
            else
            {
                return GetOperationInstanceForTransition(pIndex).Action_E2F;
            }
        }

        public void AddPrecondition(string pPrecondition)
        {
            if (pPrecondition != null)
                Precondition.Add(pPrecondition);
        }

        public Action NextI2EAction()
        {
            return MyOperationInstance.Action_I2E;
        }

        public Action NextE2FAction()
        {
            return MyOperationInstance.Action_E2F;
        }
    }
}
