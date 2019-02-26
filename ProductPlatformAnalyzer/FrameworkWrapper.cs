using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;

namespace ProductPlatformAnalyzer
{
    public class FrameworkWrapper
    {
        private OutputHandler cOutputHandler;

        //private List<variant> cVariantList;
        private HashSet<variant> cVariantSet = new HashSet<variant>();
        private Dictionary<string, variant> cVariantNameLookup = new Dictionary<string, variant>();
        private Dictionary<variant, int> cVariantIndexLookup = new Dictionary<variant, int>();
        private Dictionary<int, variant> cIndexVariantLookup = new Dictionary<int, variant>();
        private Dictionary<string, variant> cVariantSymbolicNameLookup = new Dictionary<string, variant>();

        private HashSet<variantGroup> cVariantGroupSet;

        //private List<part> cPartList;
        private HashSet<part> cPartSet;
        private bool cUsePartInfo;
        private Dictionary<string, part> cPartNameLookup = new Dictionary<string, part>();
        private Dictionary<part, int> cPartIndexLookup = new Dictionary<part, int>();
        private Dictionary<int, part> cIndexPartLookup = new Dictionary<int, part>();
        private Dictionary<string, part> cPartSymbolicNameLookup = new Dictionary<string, part>();

        private HashSet<itemUsageRule> cItemUsageRuleSet;

        private HashSet<string> cConstraintSet;

        //private List<operation> cOperationList;
        private HashSet<Operation> cOperationSet;
        private Dictionary<string, Operation> cOperationNameLookup = new Dictionary<string, Operation>();
        private Dictionary<string, Operation> cOperationSymbolicNameLookup = new Dictionary<string, Operation>();

        private HashSet<OperationInstance> cOperationInstanceSet = new HashSet<OperationInstance>();
        private Dictionary<Tuple<string, int>, OperationInstance> cOperationInstanceDictionary = new Dictionary<Tuple<string, int>, OperationInstance>();
        //private List<KeyValuePair<string, OperationInstance>> cOperationInstanceNameLookup = new List<KeyValuePair<string, OperationInstance>>();
        //private Dictionary<string, OperationInstance> cOperationInstanceSymbolicNameLookup = new Dictionary<string, OperationInstance>();

        private HashSet<resource> cResourceSet;
        private Dictionary<string, resource> cResourceNameLookup = new Dictionary<string, resource>();
        private Dictionary<string, resource> cResourceSymbolicNameLookup = new Dictionary<string, resource>();

        private HashSet<trait> cTraitSet;
        private Dictionary<string, trait> cTraitNameLookup = new Dictionary<string, trait>();
        private Dictionary<string, trait> cTraitSymbolicNameLookup = new Dictionary<string, trait>();

        #region Getter-Setters
        public HashSet<variant> VariantSet
        {
            get { return this.cVariantSet; }
            set { this.cVariantSet = value; }
        }

        public Dictionary<string, variant> VariantNameLookup
        {
            get { return this.cVariantNameLookup; }
            set { this.cVariantNameLookup = value; }
        }

        public Dictionary<variant, int> VariantIndexLookup
        {
            get { return this.cVariantIndexLookup; }
            set { this.cVariantIndexLookup = value; }
        }

        public Dictionary<int, variant> IndexVariantLookup
        {
            get { return this.cIndexVariantLookup; }
            set { this.cIndexVariantLookup = value; }
        }

        public Dictionary<string, variant> VariantSymbolicNameLookup
        {
            get { return this.cVariantSymbolicNameLookup; }
            set { this.cVariantSymbolicNameLookup = value; }
        }

        public HashSet<variantGroup> VariantGroupSet
        {
            get { return this.cVariantGroupSet; }
            set { this.cVariantGroupSet = value; }
        }

        public HashSet<part> PartSet
        {
            get { return this.cPartSet; }
            set { this.cPartSet = value; }
        }

        public bool UsePartInfo
        {
            get { return this.cUsePartInfo; }
            set { this.cUsePartInfo = value; }
        }

        public Dictionary<string, part> PartNameLookup
        {
            get { return this.cPartNameLookup; }
            set { this.cPartNameLookup = value; }
        }

        public Dictionary<part, int> PartIndexLookup
        {
            get { return this.cPartIndexLookup; }
            set { this.cPartIndexLookup = value; }
        }

        public Dictionary<int, part> IndexPartLookup
        {
            get { return this.cIndexPartLookup; }
            set { this.cIndexPartLookup = value; }
        }

        public Dictionary<string, part> PartSymbolicNameLookup
        {
            get { return this.cPartSymbolicNameLookup; }
            set { this.cPartSymbolicNameLookup = value; }
        }

        public HashSet<Operation> OperationSet
        {
            get { return this.cOperationSet; }
            set { this.cOperationSet = value; }
        }

        public Dictionary<string, Operation> OperationNameLookup
        {
            get { return this.cOperationNameLookup; }
            set { this.cOperationNameLookup = value; }
        }

        public Dictionary<string, Operation> OperationSymbolicNameLookup
        {
            get { return this.cOperationSymbolicNameLookup; }
            set { this.cOperationSymbolicNameLookup = value; }
        }

        public Dictionary<Tuple<string, int>, OperationInstance> OperationInstanceDictionary
        {
            get { return this.cOperationInstanceDictionary; }
            set { this.OperationInstanceDictionary = value; }
        }

        public HashSet<OperationInstance> OperationInstanceSet
        {
            get { return this.cOperationInstanceSet; }
            set { this.cOperationInstanceSet = value; }
        }

        /*public List<KeyValuePair<string, OperationInstance>> OperationInstanceNameLookup
        {
            get { return this.cOperationInstanceNameLookup; }
            set { this.cOperationInstanceNameLookup = value; }
        }*/

        /*public Dictionary<string, OperationInstance> OperationInstanceSymbolicNameLookup
        {
            get { return this.cOperationInstanceSymbolicNameLookup; }
            set { this.cOperationInstanceSymbolicNameLookup = value; }
        }*/

        public HashSet<itemUsageRule> ItemUsageRuleSet
        {
            get { return this.cItemUsageRuleSet; }
            set { this.cItemUsageRuleSet = value; }
        }

        public HashSet<string> ConstraintSet
        {
            get { return this.cConstraintSet; }
            set { this.cConstraintSet = value; }
        }

        public HashSet<resource> ResourceSet
        {
            get { return this.cResourceSet; }
            set { this.cResourceSet = value; }
        }

        public Dictionary<string, resource> ResourceNameLookup
        {
            get { return this.cResourceNameLookup; }
            set { this.cResourceNameLookup = value; }
        }

        public Dictionary<string, resource> ResourceSymbolicNameLookup
        {
            get { return this.cResourceSymbolicNameLookup; }
            set { this.cResourceSymbolicNameLookup = value; }
        }

        public HashSet<trait> TraitSet
        {
            get { return this.cTraitSet; }
            set { this.cTraitSet = value; }
        }

        public Dictionary<string, trait> TraitNameLookup
        {
            get { return this.cTraitNameLookup; }
            set { this.cTraitNameLookup = value; }
        }

        public Dictionary<string, trait> TraitSymbolicNameLookup
        {
            get { return this.cTraitSymbolicNameLookup; }
            set { this.cTraitSymbolicNameLookup = value; }
        }
        #endregion

        public FrameworkWrapper(OutputHandler pOutputHandler)
        {
            cOutputHandler = pOutputHandler;

            cVariantSet = new HashSet<variant>();
            cVariantNameLookup = new Dictionary<string, variant>();
            cVariantIndexLookup = new Dictionary<variant, int>();
            cIndexVariantLookup = new Dictionary<int, variant>();
            cVariantSymbolicNameLookup = new Dictionary<string, variant>();

            cVariantGroupSet = new HashSet<variantGroup>();

            cPartSet = new HashSet<part>();
            cUsePartInfo = true;
            cPartNameLookup = new Dictionary<string, part>();
            cPartIndexLookup = new Dictionary<part, int>();
            cIndexPartLookup = new Dictionary<int, part>();
            cPartSymbolicNameLookup = new Dictionary<string, part>();

            cOperationSet = new HashSet<Operation>();
            cOperationNameLookup = new Dictionary<string, Operation>();
            cOperationSymbolicNameLookup = new Dictionary<string, Operation>();

            //cOperationInstanceNameLookup = new List<KeyValuePair<string, OperationInstance>>();
            //cOperationInstanceSymbolicNameLookup = new Dictionary<string, OperationInstance>();

            cConstraintSet = new HashSet<string>();

            cItemUsageRuleSet = new HashSet<itemUsageRule>();

            cResourceSet = new HashSet<resource>();
            cResourceNameLookup = new Dictionary<string, resource>();
            cResourceSymbolicNameLookup = new Dictionary<string, resource>();

            cTraitSet = new HashSet<trait>();
            cTraitNameLookup = new Dictionary<string, trait>();
            cTraitSymbolicNameLookup = new Dictionary<string, trait>();
        }

        public bool existVariantByName(string pVariantName)
        {
            bool lResult = false;
            try
            {
                if (cVariantNameLookup.ContainsKey(pVariantName))
                    lResult = true;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in existVariantByName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResult;
        }

        public variant variantLookupByName(string pVariantName)
        {
            variant lResultVariant = null;
            try
            {
                if (cVariantNameLookup.ContainsKey(pVariantName))
                    lResultVariant = cVariantNameLookup[pVariantName];
                else
                    cOutputHandler.printMessageToConsole("Variant " + pVariantName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in variantLookupByName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultVariant;
        }

        public variant variantLookupBySymbolicName(string pVariantSymbolicName)
        {
            variant lResultVariant = null;
            try
            {
                if (cVariantSymbolicNameLookup.ContainsKey(pVariantSymbolicName))
                    lResultVariant = cVariantSymbolicNameLookup[pVariantSymbolicName];
                else
                    cOutputHandler.printMessageToConsole("Variant " + pVariantSymbolicName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in variantLookupBySymbolicName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultVariant;
        }

        public bool haveVariantWithName(string pVariantName)
        {
            bool tempResult = false;
            try
            {
                tempResult = cVariantNameLookup.ContainsKey(pVariantName);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in haveVariantWithName, pVariantName: " + pVariantName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return tempResult;
        }

        public part partLookupByName(string pPartName, bool pWithUserMsg = true)
        {
            part lResultPart = null;
            try
            {
                if (cPartNameLookup.ContainsKey(pPartName))
                    lResultPart = cPartNameLookup[pPartName];
                else
                    if (pWithUserMsg)
                        cOutputHandler.printMessageToConsole("Part " + pPartName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in partLookupByName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultPart;
        }

        public part partLookupByIndex(int pPartIndex)
        {
            part lResultPart = null;
            try
            {
                if (cIndexPartLookup.ContainsKey(pPartIndex))
                    lResultPart = cIndexPartLookup[pPartIndex];
                else
                    cOutputHandler.printMessageToConsole("Part " + pPartIndex + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in partLookupByIndex");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultPart;
        }

        public int indexLookupByPart(part pPart)
        {
            int lResultIndex = 0;
            try
            {
                if (cPartIndexLookup.ContainsKey(pPart))
                    lResultIndex = cPartIndexLookup[pPart];
                else
                    cOutputHandler.printMessageToConsole("Part " + pPart.names + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in indexLookupByPart");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultIndex;
        }

        public variant variantLookupByIndex(int pVariantIndex)
        {
            variant lResultVariant = null;
            try
            {
                if (cIndexVariantLookup.ContainsKey(pVariantIndex))
                    lResultVariant = cIndexVariantLookup[pVariantIndex];
                else
                    cOutputHandler.printMessageToConsole("Variant " + pVariantIndex + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in variantLookupByIndex");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultVariant;
        }

        public int indexLookupByVariant(variant pVariant)
        {
            int lResultIndex = 0;
            try
            {
                if (cVariantIndexLookup.ContainsKey(pVariant))
                    lResultIndex = cVariantIndexLookup[pVariant];
                else
                    cOutputHandler.printMessageToConsole("Variant " + pVariant.names + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in indexLookupByVariant");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultIndex;
        }

        public bool havePartWithName(string pPartName)
        {
            bool tempResult = false;
            try
            {
                tempResult = cPartNameLookup.ContainsKey(pPartName);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in havePartWithName, pPartName: " + pPartName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return tempResult;
        }

        public part partLookupBySymbolicName(string pPartSymbolicName)
        {
            part lResultPart = null;
            try
            {
                if (cPartSymbolicNameLookup.ContainsKey(pPartSymbolicName))
                    lResultPart = cPartSymbolicNameLookup[pPartSymbolicName];
                else
                    cOutputHandler.printMessageToConsole("Part " + pPartSymbolicName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in partLookupBySymbolicName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultPart;
        }

        public Operation operationLookupByName(string pOperationName)
        {
            Operation lResultOperation = null;
            try
            {
                if (cOperationNameLookup.ContainsKey(pOperationName))
                    lResultOperation = cOperationNameLookup[pOperationName];
                else
                    cOutputHandler.printMessageToConsole("Operation " + pOperationName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in operationLookupByName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperation;
        }

        public Operation operationLookupBySymbolicName(string pOperationSymbolicName)
        {
            Operation lResultOperation = null;
            try
            {
                if (cOperationSymbolicNameLookup.ContainsKey(pOperationSymbolicName))
                    lResultOperation = cOperationSymbolicNameLookup[pOperationSymbolicName];
                else
                    cOutputHandler.printMessageToConsole("Operation " + pOperationSymbolicName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in operationLookupBySymbolicName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperation;
        }

        public OperationInstance operationInstanceLookup(string pOperationName, int pTransitionNumber)
        {
            OperationInstance lResultOperationInstance = null;
            try
            {
                Tuple<string, int> lKeyTuple = new Tuple<string,int>(pOperationName, pTransitionNumber);

                if (cOperationInstanceDictionary.ContainsKey(lKeyTuple))
                    lResultOperationInstance = cOperationInstanceDictionary[lKeyTuple];
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in operationInstanceLookup");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperationInstance;
        }

        public HashSet<OperationInstance> getOperationInstancesInOneTransition(int pTransitionNumber)
        {
            HashSet<OperationInstance> lResultList = new HashSet<OperationInstance>();
            try
            {
                foreach (var lOperationInstance in cOperationInstanceSet)
                {
                    if (lOperationInstance.TransitionNumber.Equals(pTransitionNumber))
                        lResultList.Add(lOperationInstance);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getOperationInstancesInOneTransition");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultList;
        }

        public HashSet<OperationInstance> getOperationInstancesForOneOperationInOneTrasition(Operation pOperation, int pTransitionNumber=-1)
        {
            HashSet<OperationInstance> lResultList = new HashSet<OperationInstance>();
            try
            {
                foreach (var lOperationnstance in cOperationInstanceSet)
                {
                    if (lOperationnstance.AbstractOperation.Equals(pOperation))
                        if (pTransitionNumber!=-1 && lOperationnstance.TransitionNumber.Equals(pTransitionNumber))
                            lResultList.Add(lOperationnstance);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getOperationInstancesForOneOperationInOneTrasition");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultList;

        }

        /*public List<OperationInstance> operationInstanceLookupByName(string pOperationInstanceName)
        {
            List<OperationInstance> lResultOperationInstance = new List<OperationInstance>();
            try
            {
                var result = cOperationInstanceNameLookup.Where(kvp => kvp.Key == pOperationInstanceName);

                if (result.Any())
                {
                    foreach (var kvp in result)
                    {
                        lResultOperationInstance.Add(new OperationInstance(kvp.Value.AbstractOperation
                                                                            , kvp.Value.TransitionNumber));
                    }
                }
                else
                    cOutputHandler.printMessageToConsole("Operation Instance " + pOperationInstanceName + " not found in Dictionary!");

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in operationInstanceLookupByName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperationInstance;
        }

        public OperationInstance operationInstanceLookupBySymbolicName(string pOperationInstanceSymbolicName)
        {
            OperationInstance lResultOperationInstance = null;
            try
            {
                if (cOperationInstanceSymbolicNameLookup.ContainsKey(pOperationInstanceSymbolicName))
                    lResultOperationInstance = cOperationInstanceSymbolicNameLookup[pOperationInstanceSymbolicName];
                else
                    cOutputHandler.printMessageToConsole("Operation Instance " + pOperationInstanceSymbolicName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in operationInstanceLookupBySymbolicName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperationInstance;
        }*/

        public resource resourceLookupByName(string pResourceName)
        {
            resource lResultResource = null;
            try
            {
                if (cResourceNameLookup.ContainsKey(pResourceName))
                    lResultResource = cResourceNameLookup[pResourceName];
                else
                    cOutputHandler.printMessageToConsole("Resource " + pResourceName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in resourceLookupByName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultResource;
        }

        public resource resourceLookupBySymbolicName(string pResourceSymbolicName)
        {
            resource lResultResource = null;
            try
            {
                if (cResourceSymbolicNameLookup.ContainsKey(pResourceSymbolicName))
                    lResultResource = cResourceSymbolicNameLookup[pResourceSymbolicName];
                else
                    cOutputHandler.printMessageToConsole("Resource " + pResourceSymbolicName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in resourceLookupBySymbolicName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultResource;
        }

        public trait traitLookupByName(string pTraitName)
        {
            trait lResultTrait = null;
            try
            {
                if (cTraitNameLookup.ContainsKey(pTraitName))
                    lResultTrait = cTraitNameLookup[pTraitName];
                else
                    cOutputHandler.printMessageToConsole("Trait " + pTraitName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in traitLookupByName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultTrait;
        }

        public trait traitLookupBySymbolicName(string pTraitSymbolicName)
        {
            trait lResultTrait = null;
            try
            {
                if (cTraitSymbolicNameLookup.ContainsKey(pTraitSymbolicName))
                    lResultTrait = cTraitSymbolicNameLookup[pTraitSymbolicName];
                else
                    cOutputHandler.printMessageToConsole("Trait " + pTraitSymbolicName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in traitLookupBySymbolicName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultTrait;
        }

        /// <summary>
        /// This function returns a list of operation names
        /// </summary>
        /// <returns>List of operation names</returns>
        public HashSet<string> getSetOfOperationNames()
        {
            HashSet<string> lOperationNames = new HashSet<string>();
            try
            {
                foreach (Operation lOperation in cOperationSet)
                {
                    if (!lOperationNames.Contains(lOperation.Name))
                        lOperationNames.Add(lOperation.Name);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getSetOfOperationNames");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lOperationNames;
        }

        public List<string> getPreconditionForOperation(string opName)
        {
            List<string> lCondition = new List<string>();
            try
            {
                Operation op = operationLookupByName(opName);
                lCondition = op.Precondition;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getPreconditionForOperation");                
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lCondition;
        }

        public String getOperationStateFromOperationName(String pOperationName)
        {
            String tempOperationState = "";
            try
            {
                String[] lOperationNameParts = pOperationName.Split('_');

                tempOperationState = lOperationNameParts[3];
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getOperationStateFromOperationName, pOperationName: " + pOperationName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return tempOperationState;
        }

        public string getVariantGroup(string varName)
        {
            string lVariantgroup = "";
            try
            {
                variant var = variantLookupByName(varName);
                variantGroup varGroup = getVariantGroup(var);
                if (varGroup != null)
                    lVariantgroup = varGroup.names;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getVariantGroup");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lVariantgroup;
        }

        public HashSet<variantGroup> getVariantGroupSet()
        {
            return VariantGroupSet;
        }

        public HashSet<string> getConstraintSet()
        {
            return ConstraintSet;
        }

        /*public HashSet<string> getvariantInstancesForOperation(string op)
        {
            HashSet<string> instances = new HashSet<string>();
            try
            {
                string[] opParts = new string[4];
                foreach (string iOp in ActiveOperationInstanceNamesSet)
                {
                    opParts = iOp.Split('_');
                    if (String.Equals(opParts[0], op) && String.Equals(opParts[1], "F") && String.Equals(opParts[3], "0"))
                        instances.Add(opParts[2]);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getvariantInstancesForOperation");                
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return instances;
        }*/

        public variantGroup getVariantGroup(variant var)
        {
            variantGroup lVariantGroup = null;
            try
            {
                foreach (variantGroup vg in VariantGroupSet)
                {

                    foreach (variant v in vg.variants)
                    {
                        if (v.Equals(var))
                            lVariantGroup = vg;
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getVariantGroup");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lVariantGroup;
        }

        public Operation getOperationFromOperationName(string pOperationName)
        {
            Operation resultOperation = null;
            try
            {
                string tempOperationName = "";
                if (pOperationName.Contains("_"))
                {
                    string[] pOperationNameParts = pOperationName.Split('_');
                    tempOperationName = pOperationNameParts[0];
                }
                else
                    tempOperationName = pOperationName;

                resultOperation = operationLookupByName(tempOperationName);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getOperationFromOperationName, pOperationName: " + pOperationName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return resultOperation;
        }

        /// <summary>
        /// This function checks the requirement field of the operations
        /// </summary>
        /// <returns>The result of the analysis of the requirment field of the operations</returns>
        public bool checkPreAnalysis()
        {
            bool lPreAnalysisResult = true;
            try
            {
                foreach (var operation in OperationSet)
                {
                    if (!checkOperationRequirementField(operation))
                    {
                        cOutputHandler.printMessageToConsole(operation.Name + " not executable!");
                        lPreAnalysisResult = false;
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in checkPreAnalysis");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lPreAnalysisResult;
        }

        public bool checkOperationRequirementField(Operation pOperation)
        {
            bool lCheckResult = false;
            try
            {
                if (pOperation.Requirement != null && pOperation.Requirement!="")
                {
                    //Check syntax of Requirement field
                    lCheckResult = CheckOperationsRequirementFieldSyntax(pOperation.Requirement);

                    //for each part of (Trait)+ check that the traits are existing objects
                    lCheckResult = CheckExistanceOfRequirementTraits(pOperation.Requirement);

                    //Check to find resource which inheritance field matches the  (Trait)+ part of the requirement field
                    lCheckResult = CheckValidityOfOperationRequirementsTraits(pOperation.Requirement);

                    //For the fields in the expression of the requirement add the found resource name as a prefix to fields in expression
                    AddRelevantResourceNameToOperationRequirementAttributes(pOperation);
                }
                else
                    lCheckResult = true;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in checkOperationRequirementField, pOperationName: " + pOperation.Name);
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lCheckResult;
        }

        //This function is no longer needed as it is assumed that there is only one requirement expression per operation
        /*
        public string ReturnOperationRequirements(string pOperationName)
        {
            string lResultOperationRequirement = "";
            try
            {
                Operation lResultingOperation = getOperationFromOperationName(pOperationName);

                foreach (string lRequirement in lResultingOperation.Requirement)
                {
                    if (lResultOperationRequirement != "")
                        lResultOperationRequirement += " && " + lRequirement;
                    else
                        lResultOperationRequirement += lRequirement;
                }
                
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnOperationRequirements");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperationRequirement;
        }
        */

        public HashSet<resource> ReturnOperationChosenResource(string pOperationName)
        {
            HashSet<resource> lResultResources = new HashSet<resource>();
            try
            {
                //IMPORTANT: Here we have assumed that the operation requirement part is in the Prefix format
                //Also remember that the traits have been replaced
                //The operation has the format "operand operator1 resource_name.attribute"
                Operation lOperation = operationLookupByName(pOperationName);

                string lRequirement = lOperation.Requirement;
                //foreach (string lRequirement in lOperation.requirements)
	            //{
                    int lLastSpaceIndex = lRequirement.LastIndexOf(' ');
                    string lLastOperand = lRequirement.Substring(lLastSpaceIndex + 1);
                    string[] lLastOperandParts = lLastOperand.Split('_');
                    string lResourceName = lLastOperandParts[0];
                    lResultResources.Add(resourceLookupByName(lResourceName));
	            //}
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnOperationChosenResource");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultResources;
        }

        private void AddRelevantResourceNameToOperationRequirementAttributes(Operation pOperation)
        {
            try
            {
                //IMPORTANT: We have the premise that the input is in the prefix format!!!
                string lRequirementField = pOperation.Requirement;

                HashSet<Tuple<string, string>> lChangeRequirementSet = new HashSet<Tuple<string, string>>();

                string lRequirement = lRequirementField;
                //foreach (string lRequirement in lRequirementField)
                //{
                    //Here for each requirement we look at its traits and see which resource can match them
                    resource lResultingResource = ReturnRequirementMatchingResource(lRequirement);

                    //We find the attibute part of the requirement
                    string[] lRequirementFieldParts = lRequirement.Split(':');
                    string lAttributePart = lRequirementFieldParts[1].Trim();

                    //We add that resource name to add it to the field name of that requirement
                    int lIndexOfFirstOperand = lAttributePart.LastIndexOf(' ') + 1;
                    lAttributePart = lAttributePart.Insert(lIndexOfFirstOperand, lResultingResource.names + "_");
//                    lAttributePart = lResultingResource.names + "_" + lAttributePart;
//                    lAttributePart = lAttributePart.Replace(", ", ", " + lResultingResource.names + ".");

                    lChangeRequirementSet.Add(new Tuple<string, string>(lRequirement, lAttributePart));
                //}

                foreach (Tuple<string,string> lChangeRequirement in lChangeRequirementSet)
                {
                    ChangeOperationRequirementField(pOperation, lChangeRequirement.Item1, lChangeRequirement.Item2);
                    
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("Error in AddRelevantResourceNameToOperationRequirementAttributes");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private void ChangeOperationRequirementField(Operation pOperation, string pOldRequirement, string pNewRequirement)
        {
            try
            {
                Operation lOperation = operationLookupByName(pOperation.Name);
                
                //Before the requirements was a list so it was replaced like this
                /*
                lOperation.requirements.Remove(pOldRequirement);

                lOperation.requirements.Add(pNewRequirement);
                 */
                //Now it is only one field hence this is how it is replaced
                lOperation.Requirement = pNewRequirement;

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ChangeOperationRequirementField");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private resource ReturnRequirementMatchingResource(string pRequirement)
        {
            resource lResultingResource = new resource();
            try
            {
                HashSet<trait> lRequirementTraits = ExtractRequirementFieldTraits(pRequirement);

                HashSet<resource> resources = new HashSet<resource>();
                foreach (resource lResource in cResourceSet)
                {
                    if (lResource.traits.SequenceEqual(lRequirementTraits))
                        resources.Add(lResource);
                }

                if (resources.Count != 0)
                    lResultingResource = resources.First();
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnRequirementMatchingResource");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultingResource;
        }

        private bool CheckExistanceOfRequirementTraits(string pRequirementField)
        {
            bool lSemanticCheck = true;
            try
            {
                var lRequirment = pRequirementField;
                //foreach (var lRequirment in pRequirementField)
                //{

                    string lTraitNamesStr = ExtractRequirementFieldTraitNames(lRequirment);

                    string[] lTraitNames = lTraitNamesStr.Split(',');

                    foreach (var lTraitName in lTraitNames)
                    {
                        var lTraits = traitLookupByName(lTraitName);

                        if (lTraits != null)
                            lSemanticCheck = lSemanticCheck && true;
                    }

                //}
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CheckExistanceOfRequirementTraits");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lSemanticCheck;
        }

        private bool CheckValidityOfOperationRequirementsTraits(string pRequirementField)
        {
            bool lSemanticCheck = true;
            try
            {
                var lRequirment = pRequirementField;
                //foreach (var lRequirment in pRequirementField)
                //{
                    resource lResultingResource = ReturnRequirementMatchingResource(lRequirment);

                    if (lResultingResource != null)
                        lSemanticCheck = lSemanticCheck && true;
                //}
            }
            catch (Exception ex)
            {
                lSemanticCheck = false;
                cOutputHandler.printMessageToConsole("error in CheckValidityOfOperationRequirementsTraits");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lSemanticCheck;
        }

        private string ExtractRequirementFieldTraitNames(string pRequirementField)
        {
            string requirementFieldTraitNames = "";
            try
            {
                string[] lRequirementFieldParts = pRequirementField.Split(':');
                requirementFieldTraitNames = lRequirementFieldParts[0].TrimEnd();
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ExtractRequirementFieldTraitNames");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return requirementFieldTraitNames;
        }

        private HashSet<trait> ExtractRequirementFieldTraits(string pRequirementField)
        {
            HashSet<trait> lRequirementFieldTraits = new HashSet<trait>();
            string requirementFieldTraitNames = "";
            try
            {
                string[] lRequirementFieldParts = pRequirementField.Split(':');
                requirementFieldTraitNames = lRequirementFieldParts[0].TrimEnd();

                string[] lTraitNames = requirementFieldTraitNames.Split(',');
                for (int i = 0; i < lTraitNames.Length; i++)
			    {
                			 
                    foreach (var lTrait in TraitSet)
                    {
                        if (lTrait.names == lTraitNames[i].Trim())
                        {
                            lRequirementFieldTraits.Add(lTrait);
                            break;
                        }
                    }
			    }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ExtractRequirementFieldTraits");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lRequirementFieldTraits;
        }

        private string ExtractResourceTraitNames(resource pResource)
        {
            string resourceTraitName = "";
            try
            {
                foreach (var trait in pResource.traits)
                {
                    if (resourceTraitName != "")
                        resourceTraitName += ",";
                    resourceTraitName += trait.names + " ";
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ExtractResourceTraitNames");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return resourceTraitName;
        }

        private bool CheckOperationsRequirementFieldSyntax(string pRequirementField)
        {
            bool lSyntaxCheck = true;
            try
            {
                string lRequirment = pRequirementField;
                //foreach (var lRequirment in pRequirementField)
                //{
                    
                    string lTraitPart = "";
                    string lExpressionPart = "";
                    //Requirement field grammar -> (Trait)+: expression
                    if (lRequirment.Contains(':'))
                    {
                        string[] lRequirementFieldParts = lRequirment.Split(':');
                        if (lRequirementFieldParts[0] != "" && lRequirementFieldParts[1] != "")
                        {
                            lTraitPart = lRequirementFieldParts[0].Trim();
                            lExpressionPart = lRequirementFieldParts[1].Trim();
                        }
                        else
                        {
                            lSyntaxCheck = false;
                            cOutputHandler.printMessageToConsole("error in CheckOperationsRequirementFieldSyntax, Operation requierment field should have the correct syntax");
                        }

                    }
                    else
                    {
                        lSyntaxCheck = false;
                        cOutputHandler.printMessageToConsole("error in CheckOperationsRequirementFieldSyntax, Operation requierment field should contain : character");
                    }
                //}

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CheckOperationsRequirementFieldSyntax");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lSyntaxCheck;
        }

        /*
        public operation findOperationWithName(String pOperationName)
        {
            operation tempResultOperation = null;
            try
            {
                foreach (operation lOperation in OperationList)
                {
                    if (lOperation.names.Equals(pOperationName))
                        tempResultOperation = lOperation;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in findOperationWithName, pOperationName: " + pOperationName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return tempResultOperation;
        }
        */

        public string ReturnStringElements(HashSet<String> pSet)
        {
            string lResultElements = "";
            try
            {
                //TODO: write with LINQ
                foreach (string lElement in pSet)
                {
                    lResultElements += lElement;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnStringElements");                
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultElements;
        }

        public void addVariantGroup(variantGroup pVariantGroup)
        {
            VariantGroupSet.Add(pVariantGroup);
        }

        /*public void addPart(part pPart)
        {
            PartList.Add(pPart);
        }*/

        /*public void addVariant(variant pVariant)
        {
            VariantList.Add(pVariant);
        }*/

        public void addConstraint(String pConstraint)
        {
            ConstraintSet.Add(pConstraint);
        }

        public void addResource(resource pResource)
        {
            ResourceSet.Add(pResource);
        }

        /*public resource findResourceWithName(String pResourceName)
        {
            resource tempResultResource = null;
            try
            {
                foreach (resource lResource in ResourceList)
                {
                    if (lResource.names.Equals(pResourceName))
                        tempResultResource = lResource;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in findResourceWithName, pStationName: " + pResourceName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return tempResultResource;
        }*/

        public void addTrait(trait pTrait)
        {
            TraitSet.Add(pTrait);
        }

        public String giveNextStateActiveOperationName(String pActiveOperationName)
        {
            String lNextStateActiveOperationName = "";
            try
            {
                String[] parts = pActiveOperationName.Split('_');
                if (parts[3] != null)
                {
                    int lCurrentActiveOperationIndex = Convert.ToInt32(parts[3]);
                    parts[3] = (lCurrentActiveOperationIndex + 1).ToString();
                }

                lNextStateActiveOperationName = String.Join("_", parts);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in giveNextStateActiveOperationName, pActiveOperationName: " + pActiveOperationName);                
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lNextStateActiveOperationName;
        }

        public string getOperationNameFromActiveOperation(string pActiveOperationName)
        {
            string lResultOperationName = "";
            try
            {
                string[] parts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Part_Transition
                if (parts.Length >= 1)
                    lResultOperationName = parts[0];
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getOperationNameFromActiveOperation");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperationName;
        }

        public Operation getOperationFromActiveOperation(string pActiveOperationName)
        {
            Operation lResultOperation = null;
            try
            {
                string[] parts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Part_Transition
                if (parts.Length >= 1)
                    lResultOperation = operationLookupByName(parts[0]);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getOperationFromActiveOperation");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperation;
        }

        public part getPartFromActiveOperationName(string pActiveOperationName)
        {
            part lResultPart = null;
            try
            {
                string[] parts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Part_Transition
                if (parts.Length >= 2)
                    lResultPart = partLookupByIndex(int.Parse(parts[2]));
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getPartFromActiveOperation");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultPart;
        }

        public int getPartIndexFromActiveOperation(string pActiveOperationName)
        {
            int lPartIndex = -1;
            try
            {
                string[] parts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Part_Transition
                if (parts.Length >= 3)
                {
                    if (parts[2] != null)
                        lPartIndex = Convert.ToInt32(parts[2]);
                }
                else
                    //This means that the variant for the active operation has not been mentioned so this operation should be considered for all active variants
                    lPartIndex = -1;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getPartIndexFromActiveOperation, pActiveOperationName: " + pActiveOperationName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }

            return lPartIndex;
        }

        public int getVariantIndexFromActiveOperation(string pActiveOperationName)
        {
            int lVariantIndex = -1;
            try
            {
                string[] parts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Variant_Transition
                if (parts.Length >= 3)
                {
                    if (parts[2] != null)
                        lVariantIndex = Convert.ToInt32(parts[2]);
                }
                else
                    //This means that the variant for the active operation has not been mentioned so this operation should be considered for all active variants
                    lVariantIndex = -1;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getVariantIndexFromActiveOperation, pActiveOperationName: " + pActiveOperationName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }

            return lVariantIndex;
        }

        public int getOperationTransitionNumberFromActiveOperation(string pActiveOperationName)
        {
            int lOpTransNum = 0;
            try
            {
                //TODO: To be corrected in the resource implementation
                /*if (!pActiveOperationName.Contains("Possible") && !pActiveOperationName.Contains("Use"))
                {
                    String[] parts = pActiveOperationName.Split('_');
                    //ActiveOperationInstance: OperationName_State_Part_Transition
                    if (parts.Length.Equals(4))
                    {
                        if (parts[3] != null)
                            lOpTransNum = Convert.ToInt32(parts[3]);
                    }
                    else
                    {
                        //This means that for the active operation the transition number has not been mentioned hence it should be considered from the first transition
                        lOpTransNum = calculateTransitionNumberForActiveOperation(pActiveOperationName);
                        setActiveOperationMissingTransitionNumber(pActiveOperationName, lOpTransNum);
                    }
                }*/

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getOperationTransitionNumberFromActiveOperation, pActiveOperationName: " + pActiveOperationName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lOpTransNum;
        }

        private void setActiveOperationMissingTransitionNumber(string pActiveOperationName, int pCalculatedTransitionNumber)
        {
            try
            {
                string lNewOperationName = pActiveOperationName + "_" + pCalculatedTransitionNumber;

                //Here we know that the operation does not have a transition number hence we set the transition number of the operation to 0
                updateOperationName(pActiveOperationName, lNewOperationName);

                refactorOperationName(pActiveOperationName, lNewOperationName);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in setActiveOperationMissingTransitionNumber");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private void refactorOperationName(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                //But as this operation name has changed we have to change any reference which is to this operation:
                //1. First in the local operation list
                updateOperationNameInLocalSet(pOldOperationName, pNewOperationName);

                //2. Second in the pre-condition or post condition of any of the other operations
                updateOperationNameInPrePostConditions(pOldOperationName, pNewOperationName);

                //3. Third in the variant-operation mappings
                //updateOperationNameInPartOperationMapping(pOldOperationName, pNewOperationName);

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in refactorOperationName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private void updateOperationName(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                Operation lFoundOperation = cOperationNameLookup[pOldOperationName];
                if (lFoundOperation != null)
                    lFoundOperation.Name = pNewOperationName;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in updateOperationName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        /*private void updateOperationNameInPartOperationMapping(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                //In this function the name of one of the operations in the local list has changed so we want to update any possible references to this operation in the variantOperationsSet
                foreach (partOperations lPartOperations in PartsOperationsSet)
                {
                    //var lOperation = lPartOperations.getOperations().Find(item => item.names.Contains(pOldOperationName));
                    foreach (operation lOperation in lPartOperations.getOperations())
                    {
                        if (lOperation.names.Contains(pOldOperationName))
                        {
                            lPartOperations.getOperations().Remove(lOperation);
                            lOperation.names = pNewOperationName;
                            lPartOperations.getOperations().Add(lOperation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in updateOperationNameInPartOperationMapping");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        private void updateOperationNameInPrePostConditions(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                
                //In this function the name of one of the operations in the local list has changed so we want to update the pre/post condition of any operation that references this operation
                foreach (Operation lOperation in cOperationSet)
                {
                    var lPrecondition = operationLookupByName(pOldOperationName);
                    if (lPrecondition != null)
                    {
                        lOperation.Precondition.Add(pNewOperationName);
                    }

                    /*var lPostCondition = operationLookupByName(pOldOperationName);
                    if (lPostCondition != null)
                    {
                        lOperation.postcondition.Remove(pOldOperationName);
                        lOperation.postcondition.Add(pNewOperationName);
                    }*/
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in updateOperationNameInPrePostConditions");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private void updateOperationNameInLocalSet(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                //In this function the name of one of the operations in the local list has changed so we want to update the local operation list
                Operation lOperationToChangeName = cOperationNameLookup[pOldOperationName];
                if (lOperationToChangeName != null)
                    lOperationToChangeName.Name = pNewOperationName;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in updateOperationNameInLocalSet");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void PrintDataSummary()
        {
            try
            {
                //In this function we will print a summary of the important information which is contained in the framework wrapper
                string lDataSummary = "";

                //Operations
                if (cOperationSet.Count > 0)
                {
                    lDataSummary += "Operations:" + System.Environment.NewLine;
                    foreach (Operation lOperation in OperationSet)
                    {
                        lDataSummary += "Operation Name: " + lOperation.Name + System.Environment.NewLine;
                        string lOperationPreconditions = GetOperationPreconditionsString(lOperation.Precondition);
                        lDataSummary += "Operation Precondition: " + lOperationPreconditions + System.Environment.NewLine;
                        lDataSummary += "Operation Trigger: " + lOperation.Trigger + System.Environment.NewLine;
                        string lOperationPostconditions = GetOperationPreconditionsString(lOperation.Postcondition);
                        lDataSummary += "Operation Postcondition: " + lOperationPostconditions + System.Environment.NewLine;

                        /*foreach (string lPostconditionOperationName in lOperation.postcondition)
                            lDataSummary += "Operation Postcondition: " + lPostconditionOperationName + System.Environment.NewLine;*/
                    }
                }

                //Variants
                if (cVariantNameLookup.Count > 0)
                {
                    lDataSummary += "Variants:" + System.Environment.NewLine;
                    foreach (variant lVariant in VariantSet)
                    {
                        lDataSummary += "Variant Name: " + lVariant.names + System.Environment.NewLine;
                    }

                    //Variantgroups
                    lDataSummary += "Variant Groups:" + System.Environment.NewLine;
                    foreach (variantGroup lVariantGroup in VariantGroupSet)
                    {
                        lDataSummary += "Variant Group Name: " + lVariantGroup.names + System.Environment.NewLine;
                        lDataSummary += "Variant Group Cardinality: " + lVariantGroup.gCardinality + System.Environment.NewLine;
                        foreach (variant lVariant in lVariantGroup.variants)
                            lDataSummary += "Variant Group Variant: " + lVariant.names + System.Environment.NewLine;

                    }
                }

                //Parts
                if (cPartNameLookup.Count > 0)
                {
                    lDataSummary += "Parts:" + System.Environment.NewLine;
                    foreach (part lPart in PartSet)
                    {
                        lDataSummary += "Part Name: " + lPart.names + System.Environment.NewLine;
                        /*                    foreach (operation lManOperation in lPart.manOperations)
                                                lDataSummary += "Part Manufacturing Operation: " + lManOperation.names + System.Environment.NewLine;*/
                    }
                }

                //Configuration Rules
                if (cConstraintSet.Count > 0)
                {
                    lDataSummary += "Configuration Rules:" + System.Environment.NewLine;
                    foreach (var lConfgurationRule in cConstraintSet)
                    {
                        lDataSummary += "Configuration Rule: " + lConfgurationRule + System.Environment.NewLine;
                        
                    }

                }

                //Traits
                if (cTraitSet.Count > 0)
                {
                    lDataSummary += "Traits:" + System.Environment.NewLine;
                    foreach (trait lTrait in cTraitSet)
                    {
                        lDataSummary += "Trait Name: " + lTrait.names + System.Environment.NewLine;
                        lDataSummary += "Attributes: " + System.Environment.NewLine;
                        foreach (Tuple<string, string> lAttribute in lTrait.attributes)
                        {
                            lDataSummary += "Attribute Name: " + lAttribute.Item2 + System.Environment.NewLine;
                            lDataSummary += "Attribute Type: " + lAttribute.Item1 + System.Environment.NewLine;
                        }
                    }
                }

                cOutputHandler.printMessageToConsole(lDataSummary);
                System.IO.File.WriteAllText("D:/LocalImplementation/GitHub/ProductPlatformAnalyzer/ProductPlatformAnalyzer/Output/Debug/DataSummary.txt", lDataSummary);

                //System.IO.File.WriteAllText("C:/Users/amir/Desktop/Output/Debug/DataSummary.txt", lDataSummary);

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in PrintDataSummary");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public string GetOperationPreconditionsString(List<string> pPreconditions, bool pPrefix = false)
        {
            string lResultPreconditionStr = "";
            try
            {

                foreach (var lPrecondition in pPreconditions)
                {
                    if (lResultPreconditionStr == "")
                        lResultPreconditionStr += lPrecondition;
                    else
                    {
                        if (!pPrefix)
                            lResultPreconditionStr += " and ";
                        else
                            lResultPreconditionStr = "and " + lResultPreconditionStr;

                        lResultPreconditionStr = lResultPreconditionStr + " " + lPrecondition;
                    }
                    
                }

                lResultPreconditionStr = lResultPreconditionStr.Trim();
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in GetOperationPreconditionsString");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultPreconditionStr;
        }

        public string GetOperationPostconditionsString(List<string> pPostconditions)
        {
            string lResultPostconditionStr = "";
            try
            {

                foreach (var lPostcondition in pPostconditions)
                {
                    if (lResultPostconditionStr != "")
                        lResultPostconditionStr += " and ";
                    lResultPostconditionStr += lPostcondition;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in GetOperationPostconditionsString");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultPostconditionStr;
        }

        public Operation CreateOperationInstance(string pName
                                            , string pTriggers
                                            , string pRequirements
                                            , string pPrecondition
                                            , string pPostcondition)
        {
            Operation lTempOperation = null;
            try
            {

                lTempOperation = new Operation(pName, pTriggers, pRequirements, pPrecondition,pPostcondition);

                cOperationSet.Add(lTempOperation);
                cOperationNameLookup.Add(pName, lTempOperation);
                cOperationSymbolicNameLookup.Add("O" + cOperationSymbolicNameLookup.Count + 1, lTempOperation);

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateOperationInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lTempOperation;
        }

        public void CreateOperationInstances4AllTransitions(Operation pOperation)
        {
            try
            {
                var lMaxTransitionNumber = (OperationSet.Count * 2) + 1;
                for (int lTransitionNumber = 0; lTransitionNumber < lMaxTransitionNumber; lTransitionNumber++)
                {
                    OperationInstance lTempOperationInstance = addOperationInstance(pOperation, lTransitionNumber);

                    //Placeholder to add any additional dictionaries on operation instances
                    var lKeyTuple = new Tuple<string, int>(pOperation.Name, lTransitionNumber);
                    if (!OperationInstanceDictionary.ContainsKey(lKeyTuple))
                    {
                        OperationInstanceDictionary.Add(lKeyTuple, lTempOperationInstance);
                    }

                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateOperationInstanceInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public OperationInstance addOperationInstance(Operation pOperation, int pTransitionNo)
        {
            OperationInstance lResultOperationInstance = null;
            try
            {
                lResultOperationInstance = new OperationInstance(pOperation, pTransitionNo, cOperationInstanceSet.Count + 1);
                cOperationInstanceSet.Add(lResultOperationInstance);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in addOperationInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperationInstance;
        }

        public part CreatePartInstance(string pName)
        {
            part lTempPart = new part();
            try
            {
                lTempPart.names = pName;
                //addPart(lTempPart);
                part lFoundPart = partLookupByName(pName, false);
                if (lFoundPart == null)
                {
                    cPartSet.Add(lTempPart);
                    cPartNameLookup.Add(pName, lTempPart);
                    cIndexPartLookup.Add(cPartIndexLookup.Count + 1, lTempPart);
                    cPartIndexLookup.Add(lTempPart, cPartIndexLookup.Count + 1);
                    cPartSymbolicNameLookup.Add("P" + cPartSymbolicNameLookup.Count + 1, lTempPart);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreatePartInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lTempPart;
        }

        public variant CreateVariantInstance(string pName)
        {
            variant lTempVariant = new variant();
            try
            {
                lTempVariant.names = pName;
                //addVariant(lTempVariant);

                cVariantSet.Add(lTempVariant);
                cVariantNameLookup.Add(pName, lTempVariant);
                cIndexVariantLookup.Add(cVariantIndexLookup.Count + 1, lTempVariant);
                cVariantIndexLookup.Add(lTempVariant, cVariantIndexLookup.Count + 1);
                cVariantSymbolicNameLookup.Add("V" + cVariantSymbolicNameLookup.Count + 1, lTempVariant);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateVariantInstance, pName: " + pName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lTempVariant;
        }

        public void CreateVariantGroupInstance(string pName, string pGroupCardinality, List<variant> pVariantSet)
        {
            try
            {
                variantGroup tempVariantGroup = new variantGroup();
                tempVariantGroup.names = pName;
                tempVariantGroup.gCardinality = pGroupCardinality;
                tempVariantGroup.variants = pVariantSet;
                addVariantGroup(tempVariantGroup);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateVariantGroupInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        //This function is no longer needed as the relation between part and operations is defined in the trigger field of the operation
        /*public partOperations CreatePartOperationMappingTemporaryInstance(string pPartName, List<string> pOperationSet)
        {
            partOperations lPartOperations = new partOperations();
            try
            {
                //Here we have to check if the part name is a single part or an expression over parts


                //lPartOperations.setPartExpr(partLoopupByName(pPartName));
                lPartOperations.setPartExpr(pPartName);

                if (pOperationSet != null)
                {
                    HashSet<operation> tempOperations = new HashSet<operation>();
                    foreach (string lOperationName in pOperationSet)
                    {
                        tempOperations.Add(getOperationFromOperationName(lOperationName));
                    }
                    lPartOperations.setOperations(tempOperations);
                    cPartOperationsPartExprLookup.Add(pPartName, tempOperations);
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreatePartOperationMappingInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lPartOperations;
        }*/

        //This function is no longer needed as the relation between part and operations is defined in the trigger field of the operation
        /*public void CreatePartOperationMappingInstance(string pPartName, HashSet<string> pOperationSet)
        {
            try
            {
                //Here we have to check if the part name is a single part or an expression over parts

                partOperations lPartOperations = new partOperations();

                //lVariantOperations.setVariantExpr(variantLookupByName(pVariantName));
                lPartOperations.setPartExpr(pPartName);

                if (pOperationSet != null)
                {
                    HashSet<operation> tempOperations = new HashSet<operation>();
                    foreach (String lOperationName in pOperationSet)
                    {
                        tempOperations.Add(getOperationFromOperationName(lOperationName));
                    }
                    lPartOperations.setOperations(tempOperations);
                }
                addPartsOperations(lPartOperations);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreatePartOperationMappingInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        public void CreateItemUsageRuleInstance(part pPart, string pVariantExp)
        {
            try
            {
                //Here we have to check if the part name is a single part or an expression over parts

                itemUsageRule lItemUsageRule = new itemUsageRule();

                //lVariantOperations.setVariantExpr(variantLookupByName(pVariantName));
                lItemUsageRule.setVariantExp(pVariantExp);
                lItemUsageRule.setPart(pPart);


                addItemUsageRule(lItemUsageRule);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateItemUsageRuleInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void addItemUsageRule(itemUsageRule pItemUsageRule)
        {
            ItemUsageRuleSet.Add(pItemUsageRule);
        }

        //This function is no longer needed as the relation between part and operations is defined in the trigger field of the operation
        /*public void CreatePartOperationMappingInstance(string pPartExpr, HashSet<operation> pOperationSet)
        {
            try
            {
                //Here we have to check if the part name is a single part or an expression over parts

                partOperations lPartOperations = new partOperations();

                //lPartOperations.setPartExpr(partLoopupByName(pPartName));
                lPartOperations.setPartExpr(pPartExpr);

                lPartOperations.setOperations(pOperationSet);

                addPartsOperations(lPartOperations);
                cPartOperationsPartExprLookup.Add(pPartExpr, pOperationSet);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreatePartOperationMappingInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        //This function is no longer needed as the relation between part and operations is defined in the trigger field of the operation
        /*public void CreateVariantOperationMappingInstance(string pVariantExpr, HashSet<operation> pOperationSet)
        {
            try
            {
                //Here we have to check if the part name is a single part or an expression over parts

                variantOperations lVariantOperations = new variantOperations();

                //lPartOperations.setPartExpr(partLoopupByName(pPartName));
                lVariantOperations.setVariantExpr(pVariantExpr);

                lVariantOperations.setOperations(pOperationSet);

                //The operations which are added as a result of variant - operation relation are infact active operations
                foreach (operation lOperation in pOperationSet)
                    addActiveOperation(lOperation);

                addVarantOperations(lVariantOperations);
                cVariantOperationsVariantExprLookup.Add(pVariantExpr, pOperationSet);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateVariantOperationMappingInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        /*private trait findTraitWithName(string pTraitName)
        {
            trait lResultTrait = new trait();
            try
            {
                List<trait> lTempResultTrait = (from trait in cTraitList
                                        where trait.names == pTraitName
                                        select trait).ToList();
                if (lTempResultTrait.Count == 1)
                    lResultTrait = lTempResultTrait[0];
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in findTraitWithName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultTrait;
        }*/

        public void createTraitInstance(string pName, HashSet<trait> pInherit, HashSet<Tuple<string,string>> pAttributes)
        {
            try
            {
                trait lTempTrait = new trait();

                lTempTrait.names = pName;
                lTempTrait.inherit = pInherit;
                lTempTrait.attributes = pAttributes;

                addTrait(lTempTrait);

                cTraitNameLookup.Add(pName, lTempTrait);
                cTraitSymbolicNameLookup.Add("T" + cTraitSymbolicNameLookup.Count + 1, lTempTrait);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createTraitInstance, pName: " + pName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void CreateResourceInstance(string pName, HashSet<trait> pTraits ,HashSet<Tuple<string,string,string>> pAttributes)
        {
            try
            {
                resource lTempResource = new resource();
                lTempResource.names = pName;
                lTempResource.traits = pTraits;
                lTempResource.attributes = pAttributes;
                addResource(lTempResource);

                cResourceNameLookup.Add(pName, lTempResource);
                cResourceSymbolicNameLookup.Add("R" + cResourceSymbolicNameLookup.Count + 1, lTempResource);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateResourceInstance, pName: " + pName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        //Virtual variants are no longer needed
        /*public variant createVirtualVariant(string lVariantExpr)
        {
            variant lResultVariant = new variant();
            try
            {
                //A new Virtual variant need to be build
                //AND
                //The virtual variant needs to be added to virtual variant and virtual variant group
                variant lVirtualVariant = createVirtualVariantInstance();

                //TODO: URGENT add a relevant virtual variant and an virtual variant group
                addVirtualVariantToGroup(lVirtualVariant);

                //A new constraint needs to be added relating the virtual variant to the variant expression
                addVirtualVariantConstaint(lVirtualVariant, lVariantExpr);

                //The new virtual variant and the exression it represents needs to be added to virtual variant list in frameworkwrapper
                createVirtualVariant2VariantExprInstance(lVirtualVariant, lVariantExpr);

                lResultVariant = lVirtualVariant;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createVirtualVariant");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultVariant;
        }*/

        //Virtual parts are no longer needed
        /*public part createVirtualPart(string lPartExpr)
        {
            part lResultPart = new part();
            try
            {
                //A new Virtual part need to be build
                //AND
                //The virtual part needs to be added to virtual variant and virtual variant group
                part lVirtualPart = createVirtualPartInstance();

                //TODO: URGENT add a relevant virtual variant and an virtual variant group
                //addVirtualVariantToGroup(lVirtualPart);

                //A new constraint needs to be added relating the virtual part to the part expression
                addVirtualPartConstaint(lVirtualPart, lPartExpr);

                //The new virtual variant and the exression it represents needs to be added to virtual variant list in frameworkwrapper
                createVirtualPart2PartExprInstance(lVirtualPart, lPartExpr);

                lResultPart = lVirtualPart;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createVirtualPart");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultPart;
        }*/

        //Part operations are not used anymore
        /*public part ReturnCurrentPart(partOperations pPartOperations)
        {
            part lResultPart = null;
            try
            {

                string lPartExpr = pPartOperations.getPartExpr();

                if (lPartExpr.Contains(' '))
                {
                    //Then this should be an expression on parts
                    lResultPart = createVirtualPart(lPartExpr);
                }
                else
                {
                    //Then this should be a single part
                    lResultPart = partLookupByName(lPartExpr);
                }


            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnCurrentPart");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultPart;
        }*/

        //Variant operations are not used anymore
        /*public variant ReturnCurrentVariant(variantOperations pVariantOperations)
        {
            variant lResultVariant = null;
            try
            {

                string lVariantExpr = pVariantOperations.getVariantExpr();

                if (lVariantExpr.Contains(' '))
                {
                    //Then this should be an expression on variants
                    lResultVariant = createVirtualVariant(lVariantExpr);
                }
                else
                {
                    //Then this should be a single vairant
                    lResultVariant = variantLookupByName(lVariantExpr);
                }


            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnCurrentVariant");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultVariant;
        }*/


        public void addVirtualPartConstaint(part pVirtualPart, string pPartExpr)
        {
            try
            {
                //A new constraint needs to be added relating the virtual part to the variant expression
                addConstraint("-> " + pVirtualPart.names + " (" + pPartExpr + ")");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in addVirtualPartConstaint");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void addVirtualVariantConstaint(variant pVirtualVariant, string pVariantExpr)
        {
            try
            {
                //A new constraint needs to be added relating the virtual variant to the variant expression
                addConstraint("-> " + pVirtualVariant.names + " (" + pVariantExpr + ")");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in addVirtualVariantConstaint");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        /*public void createVirtualPart2PartExprInstance(part pVirtualPart, string pPartExpr)
        {
            try
            {
                //The new virtual part and the exression it represents needs to be added to virtual part list in frameworkwrapper
                virtualPart2PartExpr lTempVirtualPart = new virtualPart2PartExpr();

                lTempVirtualPart.setVirtualPart(pVirtualPart);
                lTempVirtualPart.setPartExpr(pPartExpr);

                cVirtualPart2PartExprSet.Add(lTempVirtualPart);

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createVirtualPart2PartExprInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        /*public void createVirtualVariant2VariantExprInstance(variant pVirtualVariant, string pVariantExpr)
        {
            try
            {
                //The new virtual variant and the exression it represents needs to be added to virtual variant list in frameworkwrapper
                virtualVariant2VariantExpr lTempVirtualVariant = new virtualVariant2VariantExpr();

                lTempVirtualVariant.setVirtualVariant(pVirtualVariant);
                lTempVirtualVariant.setVariantExpr(pVariantExpr);

                cVirtualVariant2VariantExprSet.Add(lTempVirtualVariant);

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createVirtualVariant2VariantExprInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        /*public part createVirtualPartInstance()
        {
            part lVirtualPart = new part();
            try
            {
                string name = "VirtualP" + getNextVirtualPartIndex();
                CreatePartInstance(name);

                lVirtualPart = partLookupByName(name);

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createVirtualPartInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }


            return lVirtualPart;
        }*/

        /*public variant createVirtualVariantInstance()
        {
            variant lVirtualVariant = new variant();
            try
            {
                string name = "VirtualV" + getNextVirtualVariantIndex();
                CreateVariantInstance(name);

                lVirtualVariant = variantLookupByName(name);

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createVirtualVariantInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }


            return lVirtualVariant;
        }*/

        //Virtual variants are no longer needed
        /*public void addVirtualVariantToGroup(variant pVariant)
        {
            try
            {
                foreach (variantGroup vg in VariantGroupSet)
                {
                    if (string.Equals(vg.names, "Virtual-VG"))
                    {
                        vg.variants.Add(pVariant);
                        return;
                    }
                }

                HashSet<variant> lVariantSet = new HashSet<variant>();
                lVariantSet.Add(pVariant);

                CreateVariantGroupInstance("Virtual-VG", "choose any number", lVariantSet);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in addVirtualVariantToGroup");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/


        /*private void addVirtualConstraint(virtualConnection connection)
        {
            string constraint = "or";
            List<variant> variants = new List<variant>(connection.getVariants());
            string andVariants = variants.ElementAt(0).names;
            variants.RemoveAt(0);

            foreach (variant var in variants)
            {
                andVariants = "(and " + andVariants + " " + var.names + ")";
            }

            constraint = constraint + " (and " + andVariants + " " + connection.getVirtualVariant().names + ") (and (not " +
                            andVariants + ") (not " + connection.getVirtualVariant().names + "))";

            ConstraintList.Add(constraint);
        }*/

        /*internal virtualConnection findVirtualConnectionWithName(string p)
        {
            foreach (virtualConnection con in VirtualConnections)
            {
                if (String.Equals(con.getVirtualVariant().names, p))
                    return con;
            }

            return null;
        }*/

        /*private int getNextVariantIndex()
        {
            int index = 0;
            try
            {
                foreach (part var in PartList)
                {
                    if (var.index > index)
                        index = var.index;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getNextVariantIndex");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return index + 1;
        }*/

        /*private int getNextPartIndex()
        {
            int lResultIndex = 0;
            try
            {
                foreach (part var in PartList)
                {
                    if (var.index > index)
                        index = var.index;
                }
                lResultIndex = cPartIndexLookup.Count + 1;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getNextPartIndex");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultIndex;
        }*/

        private int getVirtualVariantIndex(variant pVirtualVariant)
        {
            int lVirtualVariantIndex = 0;
            try
            {
                string lVirtualVariantName = pVirtualVariant.names;
                int lStrartingIndex = lVirtualVariantName.IndexOf("VirtualVariant");
                lVirtualVariantIndex = int.Parse(lVirtualVariantName.Remove(lStrartingIndex + 14 + 1));
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getVirtualVariantIndex");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lVirtualVariantIndex;
        }

        private int getVirtualPartIndex(part pVirtualPart)
        {
            int lVirtualPartIndex = 0;
            try
            {
                string lVirtualPartName = pVirtualPart.names;
                int lStartingIndex = lVirtualPartName.IndexOf("VirtualPart");
                lVirtualPartIndex = int.Parse(lVirtualPartName.Remove(lStartingIndex + 14 + 1));
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getVirtualPartIndex");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lVirtualPartIndex;
        }

        /*private int getMaxVirtualVariantNumber()
        {
            int lResultIndex = 0; 
            try
            {
                foreach (virtualVariant2VariantExpr virtualVariant2VariantExpr in cVirtualVariant2VariantExprSet)
                {
                    variant virtualVariant = virtualVariant2VariantExpr.getVirtualVariant();

                    lResultIndex = getVirtualVariantIndex(virtualVariant) + 1;

                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getMaxVirtualVariantNumber");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultIndex;
        }*/

        /*private int getMaxVirtualPartNumber()
        {
            int lResultIndex = 0;
            try
            {
                foreach (virtualPart2PartExpr virtualPart2PartExpr in cVirtualPart2PartExprSet)
                {
                    part virtualPart = virtualPart2PartExpr.getVirtualPart();

                    lResultIndex = getVirtualPartIndex(virtualPart) + 1;

                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getMaxVirtualPartNumber");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultIndex;
        }*/

        /*private string getNextVirtualVariantName()
        {
            string lResultName = "";
            try
            {
                //Each virtual variant is named as "VirtualVariantX" which X is a number
                int lMaxVirtualVariantNumber = getMaxVirtualVariantNumber();

                lResultName = "VirtualVariant" + lMaxVirtualVariantNumber;
                
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getNextVirtualVariantName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultName;
        }*/

        /*private string getNextVirtualPartName()
        {
            string lResultName = "";
            try
            {
                //Each virtual part is named as "VirtualPartX" which X is a number
                int lMaxVirtualPartNumber = getMaxVirtualPartNumber();

                lResultName = "VirtualPart" + lMaxVirtualPartNumber;

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getNextVirtualPartName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultName;
        }*/

        /*public string findVirtualVariantExpression(string lVirtualVariant)
        {
            string lVirtualVariantExpr = "";
            try
            {
                //TODO: if possible rewrite with LINQ
                foreach (virtualVariant2VariantExpr virtualVariant in cVirtualVariant2VariantExprSet)
                {
                    if (virtualVariant.getVirtualVariant().names == lVirtualVariant)
                        lVirtualVariantExpr = virtualVariant.getVariantExpr();
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in findVirtualVariantExpression");                
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lVirtualVariantExpr;
        }*/

        /*public string findVirtualPartExpression(string lVirtualPart)
        {
            string lVirtualPartExpr = "";
            try
            {
                //TODO: if possible rewrite with LINQ
                foreach (virtualPart2PartExpr virtualPart in cVirtualPart2PartExprSet)
                {
                    if (virtualPart.getVirtualPart().names == lVirtualPart)
                        lVirtualPartExpr = virtualPart.getPartExpr();
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in findVirtualPartExpression");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lVirtualPartExpr;
        }*/


        public String getXMLNodeAttributeInnerText(XmlNode lNode, String lAttributeName)
        {
            String lResultAttributeText = "";

            try
            {
                lResultAttributeText =lNode[lAttributeName].InnerText;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getXMLNodeAttributeInnerText, node " + lNode.Name + " does not have attribute " + lAttributeName);
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultAttributeText;
        }

        //This function is no longer needed as the relation between part and operations is defined in the trigger field of the operation
        /*public HashSet<partOperations> createPartOperationTemporaryInstances(XmlDocument pXDoc)
        {
            HashSet<partOperations> lPartOperationsSet = new HashSet<partOperations>();
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//partOperationMapping");


                foreach (XmlNode lNode in nodeList)
                {
                    HashSet<string> lPartOperations = new HashSet<string>();

                    XmlNodeList partOperationsNodeList = lNode["operationRefs"].ChildNodes;
                    foreach (XmlNode lPartOperation in partOperationsNodeList)
                    {
                        lPartOperations.Add(lPartOperation.InnerText);
                    }


                    lPartOperationsSet.Add(CreatePartOperationMappingTemporaryInstance(getXMLNodeAttributeInnerText(lNode, "partRefs")
                                                                    , lPartOperations));


                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createPartOperationInstances");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lPartOperationsSet;
        }*/

        //TODO: rename this function to show that you are loading from input
        public bool createTraitInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//trait");

                if (nodeList.Count.Equals(0))
                    lDataLoaded = false;
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        HashSet<Tuple<string, string>> lAttributes = new HashSet<Tuple<string, string>>();

                        HashSet<trait> lInheritTraits = new HashSet<trait>();

                        XmlNodeList inheritList = lNode["inherit"].ChildNodes;
                        foreach (XmlNode lTraitName in inheritList)
                        {
                            lInheritTraits.Add(traitLookupByName(lTraitName.InnerText));
                        }

                        XmlNodeList attributeList = lNode["attributes"].ChildNodes;
                        foreach (XmlNode lAttribute in attributeList)
                        {
                            lAttributes.Add(new Tuple<string, string>(getXMLNodeAttributeInnerText(lAttribute, "attributeType")
                                            , getXMLNodeAttributeInnerText(lAttribute, "attributeName")));
                        }

                        createTraitInstance(getXMLNodeAttributeInnerText(lNode, "traitName")
                                                                , lInheritTraits
                                                                , lAttributes);
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createTraitInstances");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool createResourceInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//resource");

                if (nodeList.Count.Equals(0))
                    lDataLoaded = false;
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        HashSet<Tuple<string, string, string>> lAttributes = new HashSet<Tuple<string, string, string>>();
                        HashSet<trait> lTraits = new HashSet<trait>();
                        XmlNodeList traitNamesList = lNode["traits"].ChildNodes;
                        foreach (XmlNode lTraitRef in traitNamesList)
                        {
                            string lTraitName = lTraitRef.InnerText;
                            if (lTraitName != "")
                                lTraits.Add(traitLookupByName(lTraitName));
                        }

                        XmlNodeList attributeList = lNode["attributes"].ChildNodes;
                        foreach (XmlNode lAttribute in attributeList)
                        {
                            lAttributes.Add(new Tuple<string, string, string>(getXMLNodeAttributeInnerText(lAttribute, "attributeName")
                                                                            , getXMLNodeAttributeInnerText(lAttribute, "attributeType")
                                                                            , getXMLNodeAttributeInnerText(lAttribute, "attributeValue")));
                        }


                        CreateResourceInstance(getXMLNodeAttributeInnerText(lNode, "resourceName")
                                                                , lTraits
                                                                , lAttributes);
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createResourceInstances");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool createConstraintInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//constraint");

                if (nodeList.Count.Equals(0))
                    lDataLoaded = false;
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        string lPrefixFormat = getXMLNodeAttributeInnerText(lNode, "logic");
                        //TODO: if the constraint are converted into infix format this line has to be used
                        //string lPrefixFormat = GeneralUtilities.parseExpression(lPrefixFormat, "prefix");
                        addConstraint(lPrefixFormat);
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createConstraintInstances");                
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool createVariantGroupInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//variantGroup");

                if (nodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    //throw new InitialDataIncompleteException("Initial data did not contain variant group information! Variant groups not loaded.");
                    cOutputHandler.printMessageToConsole("Initial data did not contain variant group information! Variant groups not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        List<variant> lVariantGroupVariants = new List<variant>();

                        XmlNodeList variantGroupVariantNamesNodeList = lNode["variantRefs"].ChildNodes;
                        if (variantGroupVariantNamesNodeList.Count > 0)
                        {
                            foreach (XmlNode lVariantGroupVariantName in variantGroupVariantNamesNodeList)
                            {
                                string lVariantName = lVariantGroupVariantName.InnerText;
                                lVariantGroupVariants.Add(variantLookupByName(lVariantName));
                            }

                            CreateVariantGroupInstance(getXMLNodeAttributeInnerText(lNode, "variantGroupName")
                                                    , getXMLNodeAttributeInnerText(lNode, "groupCardinality")
                                                    , lVariantGroupVariants);

                            lDataLoaded = true;
                        }
                        else
                        {
                            lDataLoaded = false;
                            cOutputHandler.printMessageToConsole("Variant group defined without any variants! Data not loaded.");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createVariantGroupInstances");                
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool createVariantInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//variant");

                if (nodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    //throw new InitialDataIncompleteException("Initial data did not contain variant information! Variants not loaded.");
                    cOutputHandler.printMessageToConsole("Initial data did not contain variant information! Variants not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        /*List<string> lVariantManufacturingOperations = new List<string>();

                        XmlNodeList variantManufacturingOperationsNodeList = lNode["variantManufacturingOps"].ChildNodes;
                        foreach (XmlNode lManufacturingOp in variantManufacturingOperationsNodeList)
                        {
                            lVariantManufacturingOperations.Add(lManufacturingOp.InnerText);
                        }

                        CreateVariantInstance(getXMLNodeAttributeInnerText(lNode, "variantName")
                                                , int.Parse(getXMLNodeAttributeInnerText(lNode, "variantIndex"))
                                                , getXMLNodeAttributeInnerText(lNode, "variantDisplayName")
                                                , lVariantManufacturingOperations);*/
                        CreateVariantInstance(getXMLNodeAttributeInnerText(lNode, "variantName"));
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createVariantInstances");                
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool createPartInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//part");

                if (nodeList.Count.Equals(0))
                {
                    cUsePartInfo = false;
                    lDataLoaded = false;
                    //throw new InitialDataIncompleteException("Initial data did not contain part information! Parts not loaded.");
                    cOutputHandler.printMessageToConsole("Initial data did not contain variant information! Parts not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        CreatePartInstance(getXMLNodeAttributeInnerText(lNode, "partName"));
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createPartInstances");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        public bool createItemUsageRulesInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//itemusagerule");

                if (nodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    //throw new InitialDataIncompleteException("Initial data did not contain item usage rule information! Item usage rules not loaded.");
                    cOutputHandler.printMessageToConsole("Initial data did not contain variant information! Item usage rules not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        part lTempPart = partLookupByName(getXMLNodeAttributeInnerText(lNode, "partRef"));

                        string lTempVariantExp = getXMLNodeAttributeInnerText(lNode, "variantRef");

                        CreateItemUsageRuleInstance(lTempPart
                                                , lTempVariantExp);

                        lDataLoaded = true;

                        //HashSet<part> lParts = new HashSet<part>();

                        //XmlNodeList itemUsageRulePartsNodeList = lNode["partRefs"].ChildNodes;
                        //if (itemUsageRulePartsNodeList.Count > 0)
                        //{
                        //    foreach (XmlNode lItemUsageRuleItem in itemUsageRulePartsNodeList)
                        //    {
                        //        lParts.Add(partLookupByName(lItemUsageRuleItem.InnerText));
                        //    }

                        //    variant lTempVariant = variantLookupByName(getXMLNodeAttributeInnerText(lNode, "variantRef"));
                        //    CreateItemUsageRuleInstance(lTempVariant
                        //                            , lParts);

                        //    lDataLoaded = true;
                        //}
                        //else
                        //{
                        //    lDataLoaded = false;
                        //    cOutputHandler.printMessageToConsole("Item usage rules defined without any parts! Data not loaded.");

                        //}
                    }
                }


            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createItemUsageRulesInstances");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        //This function is no longer needed as the relation between part and operations is defined in the trigger field of the operation
        /*public bool createPartOperationsInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//partoperation");

                if (nodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    throw new InitialDataIncompleteException("Initial data did not contain part - operation information! Data not loaded.");
                    ////cOutputHandler.printMessageToConsole("Initial data did not contain variant information! Data not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        HashSet<operation> lOperations = new HashSet<operation>();

                        XmlNodeList lPartOperationMappingOperationNamesNodeList = lNode["operationRefs"].ChildNodes;
                        if (lPartOperationMappingOperationNamesNodeList.Count > 0)
                        {
                            foreach (XmlNode lPartOperationsMappingOperationName in lPartOperationMappingOperationNamesNodeList)
                                lOperations.Add(operationLookupByName(lPartOperationsMappingOperationName.InnerText));

                            CreatePartOperationMappingInstance(getXMLNodeAttributeInnerText(lNode, "partExpr")
                                                    , lOperations);

                            lDataLoaded = true;
                        }
                        else
                        {
                            lDataLoaded = false;
                            cOutputHandler.printMessageToConsole("Part-operations mapping defined without any operations! Data not loaded.");

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createPartOperationsInstances");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }*/

        //This function is no longer needed as the relation between part and operations is defined in the trigger field of the operation
        /*public bool createVariantOperationsInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//variantOperationMapping");

                if (nodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    throw new InitialDataIncompleteException("Initial data did not contain variant - operation information! Data not loaded.");
                    ////cOutputHandler.printMessageToConsole("Initial data did not contain variant information! Data not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        HashSet<operation> lOperations = new HashSet<operation>();

                        XmlNodeList lVariantOperationMappingOperationNamesNodeList = lNode["operationRefs"].ChildNodes;
                        if (lVariantOperationMappingOperationNamesNodeList.Count > 0)
                        {
                            foreach (XmlNode lVariantOperationsMappingOperationName in lVariantOperationMappingOperationNamesNodeList)
                                lOperations.Add(operationLookupByName(lVariantOperationsMappingOperationName.InnerText));

                            CreateVariantOperationMappingInstance(getXMLNodeAttributeInnerText(lNode, "variantRefs")
                                                    , lOperations);

                            lDataLoaded = true;
                        }
                        else
                        {
                            lDataLoaded = false;
                            cOutputHandler.printMessageToConsole("Variant-operations mapping defined without any operations! Data not loaded.");

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createVariantOperationsInstances");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }*/

        /// <summary>
        /// This function is used when you have the name of an operation instance (ONLY in the case of operation precondition)
        /// And this function will return the operation status which corresponds to this opertion instance name
        /// </summary>
        /// <param name="pOperationInstanceName"></param>
        /// <returns></returns>
        public Enumerations.OperationInstanceState ReturnOperationStateFromOperationInstanceName(string pOperationInstanceName)
        {
            Enumerations.OperationInstanceState lResultOperationState = Enumerations.OperationInstanceState.Unused;
            try
            {
                string[] lOperationInstanceParts = pOperationInstanceName.Split('_');

                if (lOperationInstanceParts.Length > 0)
                {
                    switch (lOperationInstanceParts[1])
                    {
                        case "I":
                            lResultOperationState = Enumerations.OperationInstanceState.Initial;
                            break;
                        case "E":
                            lResultOperationState = Enumerations.OperationInstanceState.Executing;
                            break;
                        case "F":
                            lResultOperationState = Enumerations.OperationInstanceState.Finished;
                            break;
                        default:
                            lResultOperationState = Enumerations.OperationInstanceState.Unused;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnOperationStatusFromOperationInstanceName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperationState;
        }

        /// <summary>
        /// This function is used when you have the name of an operation instance (ONLY in the case of operation precondition)
        /// And this function will return the operation which corresponds to this opertion instance name
        /// </summary>
        /// <param name="pOperationInstanceName"></param>
        /// <returns></returns>
        public Operation ReturnOperationFromOperationInstanceName(string pOperationInstanceName)
        {
            Operation lResultOperation = null;
            try
            {
                string[] lOperationInstanceParts = pOperationInstanceName.Split('_');

                if (lOperationInstanceParts.Length > 0)
                    lResultOperation = operationLookupByName(lOperationInstanceParts[0]);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnOperationFromOperationInstanceName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperation;
        }

        /// <summary>
        /// This function is used when you have the name of an operation instance (ONLY in the case of operation precondition)
        /// And this function will return the operation name which corresponds to this opertion instance name
        /// </summary>
        /// <param name="pOperationInstanceName"></param>
        /// <returns></returns>
        public string ReturnOperationNameFromOperationInstanceName(string pOperationInstanceName)
        {
            string lResultOperationName = "";
            try
            {
                string[] lOperationInstanceParts = pOperationInstanceName.Split('_');

                if (lOperationInstanceParts.Length > 0)
                    lResultOperationName = lOperationInstanceParts[0];
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnOperationNameFromOperationInstanceName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperationName;
        }

        //TODO: rename this function to show that you are loading from input
        public bool createOperationInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//operation");

                if (nodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    //throw new InitialDataIncompleteException("Initial data did not contain operation infor! Operations not loaded.");
                    cOutputHandler.printMessageToConsole("Initial data did not contain operation infor! Operations not loaded.");
                }
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        string lTriggers = "";
                        string lOperationPrecondition = "";
                        string lOperationRequirement = "";
                        string lOperationPostcondition = "";

                        if (lNode["trigger"] != null)
                        {
                            XmlNodeList opTriggersList = lNode["trigger"].ChildNodes;
                            lTriggers = opTriggersList[0].InnerText;
                        }

                        if (lNode["requirement"] != null)
                        {
                            XmlNodeList opRequirementsList = lNode["requirement"].ChildNodes;
                            lOperationRequirement = opRequirementsList[0].InnerText;
                        }

                        if (lNode["preconditions"] != null)
                        {
                            XmlNodeList opPreconditionNodeList = lNode["preconditions"].ChildNodes;
                            lOperationPrecondition=opPreconditionNodeList[0].InnerText;
                        }

                        if (lNode["postconditions"] != null)
                        {
                            XmlNodeList opPostconditionNodeList = lNode["postconditions"].ChildNodes;
                            lOperationPostcondition = opPostconditionNodeList[0].InnerText;
                        }

                        var lOperationName = getXMLNodeAttributeInnerText(lNode, "operationName");
                        
                        var lOperation  = CreateOperationInstance(lOperationName
                                                                , lTriggers
                                                                , lOperationRequirement
                                                                , lOperationPrecondition
                                                                , lOperationPostcondition);

                    }
                    
                    ///This should be done as late as possible and not when reading the XML files
                    //foreach (var lOperation in OperationSet)
                    //{
                    //    CreateOperationInstances4AllTransitions(lOperation);
                    //}

                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createOperationInstances");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }
        
    }

    public class InitialDataIncompleteException : Exception
    {
        public InitialDataIncompleteException() : base()
        {

        }

        public InitialDataIncompleteException(string message) : base(message)
        {

        }
    }
}
