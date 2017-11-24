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

        private HashSet<string> cConstraintSet;

        //private List<operation> cOperationList;
        private HashSet<operation> cOperationSet;
        private Dictionary<string, operation> cOperationNameLookup = new Dictionary<string, operation>();
        private Dictionary<string, operation> cOperationSymbolicNameLookup = new Dictionary<string, operation>();

        private HashSet<itemUsageRule> cItemUsageRuleSet;

        private HashSet<partOperations> cPartsOperationsSet;
        private Dictionary<string, HashSet<operation>> cPartOperationsPartExprLookup = new Dictionary<string, HashSet<operation>>();
        
        private HashSet<variantOperations> cVariantsOperationsSet;
        private Dictionary<string, HashSet<operation>> cVariantOperationsVariantExprLookup = new Dictionary<string, HashSet<operation>>();

        private HashSet<string> cActiveOperationInstanceNamesSet;

        private HashSet<virtualPart2PartExpr> cVirtualPart2PartExprSet;
        private HashSet<virtualVariant2VariantExpr> cVirtualVariant2VariantExprSet;

        private HashSet<operation> cActiveOperationSet;
        private HashSet<string> cInActiveOperationNamesSet;
        private HashSet<string> cOperationInstanceSet;

        private HashSet<resource> cResourceSet;
        private Dictionary<string, resource> cResourceNameLookup = new Dictionary<string, resource>();
        private Dictionary<string, resource> cResourceSymbolicNameLookup = new Dictionary<string, resource>();

        private HashSet<trait> cTraitSet;
        private Dictionary<string, trait> cTraitNameLookup = new Dictionary<string, trait>();
        private Dictionary<string, trait> cTraitSymbolicNameLookup = new Dictionary<string, trait>();

        private int cVirtualVariantCounter;
        private int cVirtualPartCounter;

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

        public HashSet<operation> OperationSet
        {
            get { return this.cOperationSet; }
            set { this.cOperationSet = value; }
        }

        public Dictionary<string, operation> OperationNameLookup
        {
            get { return this.cOperationNameLookup; }
            set { this.cOperationNameLookup = value; }
        }

        public Dictionary<string, operation> OperationSymbolicNameLookup
        {
            get { return this.cOperationSymbolicNameLookup; }
            set { this.cOperationSymbolicNameLookup = value; }
        }

        public HashSet<itemUsageRule> ItemUsageRuleSet
        {
            get { return this.ItemUsageRuleSet; }
            set { this.ItemUsageRuleSet = value; }
        }

        private int getNextVirtualVariantIndex()
        {
            return cVirtualVariantCounter++;
        }

        private int getNextVirtualPartIndex()
        {
            return cVirtualPartCounter++;
        }

        public HashSet<string> ConstraintSet
        {
            get { return this.cConstraintSet; }
            set { this.cConstraintSet = value; }
        }

        public HashSet<partOperations> PartsOperationsSet
        {
            get { return this.cPartsOperationsSet; }
            set { this.cPartsOperationsSet = value; }
        }

        public Dictionary<string, HashSet<operation>> PartOperationsPartExprLookup
        {
            get { return this.cPartOperationsPartExprLookup; }
            set { this.cPartOperationsPartExprLookup = value; }
        }

        public HashSet<variantOperations> VariantsOperationsSet
        {
            get { return this.cVariantsOperationsSet; }
            set { this.cVariantsOperationsSet = value; }
        }

        public Dictionary<string, HashSet<operation>> VariantOperationsVariantExprLookup
        {
            get { return this.cVariantOperationsVariantExprLookup; }
            set { this.cVariantOperationsVariantExprLookup = value; }
        }

        public HashSet<operation> ActiveOperationSet
        {
            get { return this.cActiveOperationSet; }
            set { this.cActiveOperationSet = value; }
        }

        public HashSet<string> ActiveOperationInstanceNamesSet
        {
            get { return this.cActiveOperationInstanceNamesSet; }
            set { this.cActiveOperationInstanceNamesSet = value; }
        }

        public HashSet<string> InActiveOperationNamesSet
        {
            get { return this.cInActiveOperationNamesSet; }
            set { this.cInActiveOperationNamesSet = value; }
        }

        public HashSet<string> OperationInstanceSet
        {
            get { return this.cOperationInstanceSet; }
            set { this.cOperationInstanceSet = value; }
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

        public FrameworkWrapper()
        {
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

            cOperationSet = new HashSet<operation>();
            cOperationNameLookup = new Dictionary<string, operation>();
            cOperationSymbolicNameLookup = new Dictionary<string, operation>();

            cConstraintSet = new HashSet<string>();
            cActiveOperationInstanceNamesSet = new HashSet<string>();
            cActiveOperationSet = new HashSet<operation>();
            cInActiveOperationNamesSet = new HashSet<string>();
            cOperationInstanceSet = new HashSet<string>();

            cItemUsageRuleSet = new HashSet<itemUsageRule>();
            cPartsOperationsSet = new HashSet<partOperations>();
            cPartOperationsPartExprLookup = new Dictionary<string, HashSet<operation>>();
            cVariantsOperationsSet = new HashSet<variantOperations>();
            cVariantOperationsVariantExprLookup = new Dictionary<string, HashSet<operation>>();

            cResourceSet = new HashSet<resource>();
            cResourceNameLookup = new Dictionary<string, resource>();
            cResourceSymbolicNameLookup = new Dictionary<string, resource>();

            cTraitSet = new HashSet<trait>();
            cTraitNameLookup = new Dictionary<string, trait>();
            cTraitSymbolicNameLookup = new Dictionary<string, trait>();

            cVirtualPart2PartExprSet = new HashSet<virtualPart2PartExpr>();
            cVirtualVariant2VariantExprSet = new HashSet<virtualVariant2VariantExpr>();
            cVirtualVariantCounter = 0;
            cVirtualPartCounter = 0;
        }

        public variant variantLookupByName(string pVariantName)
        {
            variant lResultVariant = null;
            try
            {
                if (cVariantNameLookup.ContainsKey(pVariantName))
                    lResultVariant = cVariantNameLookup[pVariantName];
                else
                    Console.WriteLine("Variant " + pVariantName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in variantLookupByName");
                Console.WriteLine(ex.Message);
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
                    Console.WriteLine("Variant " + pVariantSymbolicName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in variantLookupBySymbolicName");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in haveVariantWithName, pVariantName: " + pVariantName);
                Console.WriteLine(ex.Message);
            }
            return tempResult;
        }

        public part partLookupByName(string pPartName)
        {
            part lResultPart = null;
            try
            {
                if (cPartNameLookup.ContainsKey(pPartName))
                    lResultPart = cPartNameLookup[pPartName];
                else
                    Console.WriteLine("Part " + pPartName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in partLookupByName");
                Console.WriteLine(ex.Message);
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
                    Console.WriteLine("Part " + pPartIndex + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in partLookupByIndex");
                Console.WriteLine(ex.Message);
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
                    Console.WriteLine("Part " + pPart.names + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in indexLookupByPart");
                Console.WriteLine(ex.Message);
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
                    Console.WriteLine("Variant " + pVariantIndex + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in variantLookupByIndex");
                Console.WriteLine(ex.Message);
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
                    Console.WriteLine("Variant " + pVariant.names + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in indexLookupByVariant");
                Console.WriteLine(ex.Message);
            }
            return lResultIndex;
        }

        public bool havePartWithName(string pPartName)
        {
            bool tempResult = false;
            try
            {
                cPartNameLookup.ContainsKey(pPartName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in havePartWithName, pPartName: " + pPartName);
                Console.WriteLine(ex.Message);
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
                    Console.WriteLine("Part " + pPartSymbolicName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in partLookupBySymbolicName");
                Console.WriteLine(ex.Message);
            }
            return lResultPart;
        }

        public operation operationLookupByName(string pOperationName)
        {
            operation lResultOperation = null;
            try
            {
                if (cOperationNameLookup.ContainsKey(pOperationName))
                    lResultOperation = cOperationNameLookup[pOperationName];
                else
                    Console.WriteLine("Operation " + pOperationName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in operationLookupByName");
                Console.WriteLine(ex.Message);
            }
            return lResultOperation;
        }

        public operation operationLookupBySymbolicName(string pOperationSymbolicName)
        {
            operation lResultOperation = null;
            try
            {
                if (cOperationSymbolicNameLookup.ContainsKey(pOperationSymbolicName))
                    lResultOperation = cOperationSymbolicNameLookup[pOperationSymbolicName];
                else
                    Console.WriteLine("Operation " + pOperationSymbolicName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in operationLookupBySymbolicName");
                Console.WriteLine(ex.Message);
            }
            return lResultOperation;
        }

        public HashSet<operation> partOperationsLookupByPartExpr(string pPartExpr)
        {
            HashSet<operation> lResultOperationSet = null;
            try
            {
                if (cPartOperationsPartExprLookup.ContainsKey(pPartExpr))
                    lResultOperationSet = cPartOperationsPartExprLookup[pPartExpr];
                else
                    Console.WriteLine("Part-Operations does not contain a relation for part expression: " + pPartExpr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in partOperationsLookupByPartExpr");
                Console.WriteLine(ex.Message);
            }
            return lResultOperationSet;
        }

        public HashSet<operation> variantOperationsLookupByVariantExpr(string pVariantExpr)
        {
            HashSet<operation> lResultOperationSet = null;
            try
            {
                if (cVariantOperationsVariantExprLookup.ContainsKey(pVariantExpr))
                    lResultOperationSet = cVariantOperationsVariantExprLookup[pVariantExpr];
                else
                    Console.WriteLine("Variant-Operations does not contain a relation for variant expression: " + pVariantExpr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in variantOperationsLookupByVariantExpr");
                Console.WriteLine(ex.Message);
            }
            return lResultOperationSet;
        }

        public resource resourceLookupByName(string pResourceName)
        {
            resource lResultResource = null;
            try
            {
                if (cResourceNameLookup.ContainsKey(pResourceName))
                    lResultResource = cResourceNameLookup[pResourceName];
                else
                    Console.WriteLine("Resource " + pResourceName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in resourceLookupByName");
                Console.WriteLine(ex.Message);
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
                    Console.WriteLine("Resource " + pResourceSymbolicName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in resourceLookupBySymbolicName");
                Console.WriteLine(ex.Message);
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
                    Console.WriteLine("Trait " + pTraitName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in traitLookupByName");
                Console.WriteLine(ex.Message);
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
                    Console.WriteLine("Trait " + pTraitSymbolicName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in traitLookupBySymbolicName");
                Console.WriteLine(ex.Message);
            }
            return lResultTrait;
        }

        /// <summary>
        /// Returns the number of operations which are active, meaning they are related to a variant
        /// </summary>
        /// <returns>Number of active operations</returns>
        public int getNumberOfActiveOperations()
        {
            return cActiveOperationSet.Count();
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
                foreach (operation lOperation in cOperationSet)
                {
                    if (!lOperationNames.Contains(lOperation.names))
                        lOperationNames.Add(lOperation.names);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getSetOfOperationNames");
                Console.WriteLine(ex.Message);
            }
            return lOperationNames;
        }

        /// <summary>
        /// Returns the list of part operations corresponding to a specific part
        /// </summary>
        /// <param name="pVariantExpr">The part which we want to return its part operations</param>
        /// <returns>The list of part operations corresponding to that part</returns>
        public HashSet<operation> getPartExprOperations(string pPartExpr)
        {
            HashSet<operation> lResultOperations = null;
            try
            {
                lResultOperations = partOperationsLookupByPartExpr(pPartExpr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getPartExprOperations, pPartExpr: " + pPartExpr);
                Console.WriteLine(ex.Message);
            }
            return lResultOperations;
        }

        /// <summary>
        /// Returns the list of variant operations corresponding to a specific variant
        /// </summary>
        /// <param name="pVariantExpr">The variant which we want to return its variant operations</param>
        /// <returns>The list of variant operations corresponding to that variant</returns>
        public HashSet<operation> getVariantExprOperations(string pVariantExpr)
        {
            HashSet<operation> lResultOperations = null;
            try
            {
                lResultOperations = variantOperationsLookupByVariantExpr(pVariantExpr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getVariantExprOperations");
                Console.WriteLine(ex.Message);
            }
            return lResultOperations;
        }

        /*public operation findOperationWithName(String pOperationName)
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
                Console.WriteLine("error in findOperationWithName, pOperationName: " + pOperationName);
                Console.WriteLine(ex.Message);
            }
            return tempResultOperation;
        }*/

        public HashSet<string> getPreconditionForOperation(string opName)
        {
            HashSet<string> con = null;
            try
            {
                operation op = operationLookupByName(opName);
                con = new HashSet<string>(op.precondition);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getPreconditionForOperation");                
                Console.WriteLine(ex.Message);
            }
            return con;
        }

        /*public HashSet<string> getPostconditionForOperation(string opName)
        {
            HashSet<string> con = null;
            try
            {
                operation op = operationLookupByName(opName);
                con = new HashSet<string>(op.postcondition);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getPostconditionForOperation");                
                Console.WriteLine(ex.Message);
            }
            return con;
        }*/

        public HashSet<String> getActiveOperationNamesSet(int pState, string pOperationStateToFilter = "")
        {
            //TODO: Optimize
            HashSet<String> tempActiveOperationNamesSet = new HashSet<string>();
            try
            {
                foreach (String operationName in ActiveOperationInstanceNamesSet)
                {
                    if (getOperationStateFromOperationName(operationName).Equals(pState.ToString()))
                        if (pOperationStateToFilter!="" && operationName.Contains("_"+ pOperationStateToFilter + "_"))
                            tempActiveOperationNamesSet.Add(operationName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getActiveOperationNamesSet, pState: " + pState);
                Console.WriteLine(ex.Message);
            }
            return tempActiveOperationNamesSet;
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
                Console.WriteLine("error in getOperationStateFromOperationName, pOperationName: " + pOperationName);
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in getVariantGroup");
                Console.WriteLine(ex.Message);
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

        public HashSet<string> getvariantInstancesForOperation(string op)
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
                Console.WriteLine("error in getvariantInstancesForOperation");                
                Console.WriteLine(ex.Message);
            }
            return instances;
        }

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
                Console.WriteLine("error in getVariantGroup");
                Console.WriteLine(ex.Message);
            }
            return lVariantGroup;
        }

        public void setActiveOperationInstanceNamesSet(HashSet<String> pActiveOperationInstanceNamesSet)
        {
            ActiveOperationInstanceNamesSet = pActiveOperationInstanceNamesSet;
        }

        public void setInActiveOperationNamesSet(HashSet<String> pInActiveOperationNamesSet)
        {
            InActiveOperationNamesSet = pInActiveOperationNamesSet;
        }


        public operation getOperationFromOperationName(string pOperationName)
        {
            operation resultOperation = null;
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
                Console.WriteLine("error in getOperationFromOperationName, pOperationName: " + pOperationName);
                Console.WriteLine(ex.Message);
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
                        Console.WriteLine(operation.names + " not executable!");
                        lPreAnalysisResult = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in checkPreAnalysis");
                Console.WriteLine(ex.Message);
            }
            return lPreAnalysisResult;
        }

        public bool checkOperationRequirementField(operation pOperation)
        {
            bool lCheckResult = false;
            try
            {
                if (pOperation.requirements != null)
                {
                    //Check syntax of Requirement field
                    lCheckResult = CheckOperationsRequirementFieldSyntax(pOperation.requirements);

                    //for each part of (Trait)+ check that the traits are existing objects
                    lCheckResult = CheckExistanceOfRequirementTraits(pOperation.requirements);

                    //Check to find resource which inheritance field matches the  (Trait)+ part of the requirement field
                    lCheckResult = CheckValidityOfOperationRequirementsTraits(pOperation.requirements);

                    //For the fields in the expression of the requirement add the found resource name as a prefix to fields in expression
                    AddRelevantResourceNameToOperationRequirementAttributes(pOperation);
                }
                else
                    lCheckResult = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in checkOperationRequirementField, pOperationName: " + pOperation.names);
                Console.WriteLine(ex.Message);
            }
            return lCheckResult;
        }

        public string ReturnOperationRequirements(string pOperationName)
        {
            string lResultOperationRequirement = "";
            try
            {
                operation lResultingOperation = getOperationFromOperationName(pOperationName);

                foreach (string lRequirement in lResultingOperation.requirements)
                {
                    if (lResultOperationRequirement != "")
                        lResultOperationRequirement += " && " + lRequirement;
                    else
                        lResultOperationRequirement += lRequirement;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationRequirements");
                Console.WriteLine(ex.Message);
            }
            return lResultOperationRequirement;
        }

        public HashSet<resource> ReturnOperationChosenResource(string pOperationName)
        {
            HashSet<resource> lResultResources = new HashSet<resource>();
            try
            {
                //IMPORTANT: Here we have assumed that the operation requirement part is in the Prefix format
                //Also remember that the traits have been replaced
                //The operation has the format "operand operator1 resource_name.attribute"
                operation lOperation = operationLookupByName(pOperationName);

                foreach (string lRequirement in lOperation.requirements)
	            {
                    int lLastSpaceIndex = lRequirement.LastIndexOf(' ');
                    string lLastOperand = lRequirement.Substring(lLastSpaceIndex + 1);
                    string[] lLastOperandParts = lLastOperand.Split('_');
                    string lResourceName = lLastOperandParts[0];
                    lResultResources.Add(resourceLookupByName(lResourceName));
	            }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationChosenResource");
                Console.WriteLine(ex.Message);
            }
            return lResultResources;
        }

        private void AddRelevantResourceNameToOperationRequirementAttributes(operation pOperation)
        {
            try
            {
                //IMPORTANT: We have the premise that the input is in the prefix format!!!
                HashSet<string> lRequirementField = pOperation.requirements;

                HashSet<Tuple<string, string>> lChangeRequirementSet = new HashSet<Tuple<string, string>>();

                foreach (string lRequirement in lRequirementField)
                {
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
                }

                foreach (Tuple<string,string> lChangeRequirement in lChangeRequirementSet)
                {
                    ChangeOperationRequirementField(pOperation, lChangeRequirement.Item1, lChangeRequirement.Item2);
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AddRelevantResourceNameToOperationRequirementAttributes");
                Console.WriteLine(ex.Message);
            }
        }

        private void ChangeOperationRequirementField(operation pOperation, string pOldRequirement, string pNewRequirement)
        {
            try
            {
                operation lOperation = operationLookupByName(pOperation.names);
                
                lOperation.requirements.Remove(pOldRequirement);

                lOperation.requirements.Add(pNewRequirement);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ChangeOperationRequirementField");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in ReturnRequirementMatchingResource");
                Console.WriteLine(ex.Message);
            }
            return lResultingResource;
        }

        private bool CheckExistanceOfRequirementTraits(HashSet<string> pRequirementField)
        {
            bool lSemanticCheck = true;
            try
            {
                foreach (var lRequirment in pRequirementField)
                {

                    string lTraitNamesStr = ExtractRequirementFieldTraitNames(lRequirment);

                    string[] lTraitNames = lTraitNamesStr.Split(',');

                    foreach (var lTraitName in lTraitNames)
                    {
                        var lTraits = traitLookupByName(lTraitName);

                        if (lTraits != null)
                            lSemanticCheck = lSemanticCheck && true;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CheckExistanceOfRequirementTraits");
                Console.WriteLine(ex.Message);
            }
            return lSemanticCheck;
        }

        private bool CheckValidityOfOperationRequirementsTraits(HashSet<string> pRequirementField)
        {
            bool lSemanticCheck = true;
            try
            {
                foreach (var lRequirment in pRequirementField)
                {
                    resource lResultingResource = ReturnRequirementMatchingResource(lRequirment);

                    if (lResultingResource != null)
                        lSemanticCheck = lSemanticCheck && true;
                }
            }
            catch (Exception ex)
            {
                lSemanticCheck = false;
                Console.WriteLine("error in CheckValidityOfOperationRequirementsTraits");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in ExtractRequirementFieldTraitNames");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in ExtractRequirementFieldTraits");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in ExtractResourceTraitNames");
                Console.WriteLine(ex.Message);
            }
            return resourceTraitName;
        }

        private bool CheckOperationsRequirementFieldSyntax(HashSet<string> pRequirementField)
        {
            bool lSyntaxCheck = true;
            try
            {
                foreach (var lRequirment in pRequirementField)
                {
                    
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
                            Console.WriteLine("error in CheckOperationsRequirementFieldSyntax, Operation requierment field should have the correct syntax");
                        }

                    }
                    else
                    {
                        lSyntaxCheck = false;
                        Console.WriteLine("error in CheckOperationsRequirementFieldSyntax, Operation requierment field should contain : character");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CheckOperationsRequirementFieldSyntax");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in findOperationWithName, pOperationName: " + pOperationName);
                Console.WriteLine(ex.Message);
            }
            return tempResultOperation;
        }
        */

        public HashSet<partOperations> getPartsOperationsSet()
        {
            return PartsOperationsSet;
        }

        public void setPartsOperationsSet(HashSet<partOperations> pPartsOperationsSet)
        {
            PartsOperationsSet = pPartsOperationsSet;
        }

        public HashSet<variantOperations> getVariantsOperationsSet()
        {
            return VariantsOperationsSet;
        }

        public void setVariantsOperationsSet(HashSet<variantOperations> pVariantsOperationsSet)
        {
            VariantsOperationsSet = pVariantsOperationsSet;
        }

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
                Console.WriteLine("error in ReturnStringElements");                
                Console.WriteLine(ex.Message);
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

        public void addActiveOperationInstanceName(String pOperationInstanceName)
        {
            try
            {
                //TODO: for now just to be simple we will make the ActiveOperationNamesSet just the names of the operations
                if (!ActiveOperationInstanceNamesSet.Contains(pOperationInstanceName))
                    ActiveOperationInstanceNamesSet.Add(pOperationInstanceName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addActiveOperationInstanceName");
                Console.WriteLine(ex.Message);
            }
        }

        public void addActiveOperation(string pOperationName)
        {
            try
            {
                operation lTempOperation = operationLookupByName(pOperationName);
                if (!ActiveOperationSet.Contains(lTempOperation))
                    ActiveOperationSet.Add(lTempOperation);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addActiveOperationName");
                Console.WriteLine(ex.Message);
            }
        }

        public void addActiveOperation(operation pOperation)
        {
            try
            {
                if (!ActiveOperationSet.Contains(pOperation))
                    ActiveOperationSet.Add(pOperation);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addActiveOperationName");
                Console.WriteLine(ex.Message);
            }
        }

        public void addOperationInstance(string pOperationInstance)
        {
            try
            {
                if (!cOperationInstanceSet.Contains(pOperationInstance))
                    cOperationInstanceSet.Add(pOperationInstance);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addOperationInstance");
                Console.WriteLine(ex.Message);
            }
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
                Console.WriteLine("error in findResourceWithName, pStationName: " + pResourceName);
                Console.WriteLine(ex.Message);
            }
            return tempResultResource;
        }*/

        public void addTrait(trait pTrait)
        {
            TraitSet.Add(pTrait);
        }

        //TODO: Either this function or the next function is not needed
        /// <summary>
        /// Takes a specific operation and looks in the list of variant operations to see if this operation is part of any variant operation list
        /// </summary>
        /// <param name="pOperation">The operation we want to check if it is active</param>
        /// <returns>If an operation is active or not</returns>
        public bool isOperationActive(operation pOperation)
        {
            bool lOperationActive = false;
            try
            {
                if (cUsePartInfo)
                {
                    foreach (partOperations lPartOperations in PartsOperationsSet)
                    {
                        foreach (operation lOperation in lPartOperations.getOperations())
                        {
                            if (lOperation.Equals(pOperation))
                                lOperationActive = true;
                        }
                    }
                }
                else
                {
                    foreach (variantOperations lVariantOperations in VariantsOperationsSet)
                    {
                        foreach (operation lOperation in lVariantOperations.getOperations())
                        {
                            if (lOperation.Equals(pOperation))
                                lOperationActive = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in isOperationActive");
                Console.WriteLine(ex.Message);
            }
            return lOperationActive;
        }

        //TODO: Either this function or the previous function is not needed
        /// <summary>
        /// In this function we have an operation name which we want to know if it is an active operation or not?
        /// </summary>
        /// <param name="pOperationName">Operation name which we want to check</param>
        /// <returns>If the operation is active or not</returns>
        public bool isActiveOperation(string pOperationName)
        {
            bool lResult = false;
            try
            {
                operation lTempOperation = operationLookupByName(pOperationName);
                lResult = ActiveOperationSet.Contains(lTempOperation);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in isActiveOperation, pOperationName: " + pOperationName);
                Console.WriteLine(ex.Message);
            }
            return lResult;
        }

        /// <summary>
        /// this function takes an operation instance variable name and looks at the variant code 
        /// if the variant code is 0 this means the operation instance is inactive hence the function will return false, otherwise it will return true
        /// </summary>
        /// <param name="pOperationInstanceVariableName">operation instance variable name</param>
        /// <returns>if the operation instance is active</returns>
        public bool isOperationInstanceActive(string pOperationInstanceVariableName)
        {
            bool lResult = false;

            try
            {
                string[] lOperationInstanceParts = pOperationInstanceVariableName.Split('_');
                if (!lOperationInstanceParts[2].Equals("0"))
                    lResult = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in isOperationInstanceActive");
                Console.WriteLine(ex.Message);
            }

            return lResult;
        }

        /// <summary>
        /// This function takes an instace variable of an operation and checks if it is in the initial state or not
        /// </summary>
        /// <param name="pOperationInstanceVariableName">Operaton instance variable name</param>
        /// <returns>If the operation instance variable is in the intial state or not</returns>
        public bool isOperationInstanceInitialState(string pOperationInstanceVariableName)
        {
            bool lResult = false;
            try
            {
                if (pOperationInstanceVariableName.Contains("_I_"))
                    lResult = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in isOperationInstanceInitialState");
                Console.WriteLine(ex.Message);
            }
            return lResult;
        }

        /// <summary>
        /// This function takes an instace variable of an operation and checks if it is in the unused state or not
        /// </summary>
        /// <param name="pOperationInstanceVariableName">Operaton instance variable name</param>
        /// <returns>If the operation instance variable is in the unused state or not</returns>
        public bool isOperationInstanceUnusedState(string pOperationInstanceVariableName)
        {
            bool lResult = false;
            try
            {
                if (pOperationInstanceVariableName.Contains("_U_"))
                    lResult = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in isOperationInstanceUnusedState");
                Console.WriteLine(ex.Message);
            }
            return lResult;
        }

        /// <summary>
        /// This funcion takes an operation instance and returns the operation name
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        public string ReturnOperationNameFromOperationInstance(string pOperationInstance)
        {
            string lOperationName = "";
            try
            {
                string[] lOperationInstanceParts = pOperationInstance.Split('_');

                lOperationName = lOperationInstanceParts[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in returnOperationNameFromOperationInstance");
                Console.WriteLine(ex.Message);
            }
            return lOperationName;
        }

        /// <summary>
        /// This funcion takes an operation instance and returns the operation
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        public operation ReturnOperationFromOperationInstance(string pOperationInstance)
        {
            operation lResultOperation = null;
            try
            {
                string lOperationName = "";

                string[] lOperationInstanceParts = pOperationInstance.Split('_');

                lOperationName = lOperationInstanceParts[0];

                lResultOperation = operationLookupByName(lOperationName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in returnOperationNameFromOperationInstance");
                Console.WriteLine(ex.Message);
            }
            return lResultOperation;
        }

        /// <summary>
        /// This function takes an operation instance and returns the operation Status from that operation instance
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        public string ReturnOperationStatusFromOperationInstance(string pOperationInstance)
        {
            string lOperationStatus = "";
            try
            {
                string[] lOperationInstanceParts = pOperationInstance.Split('_');
                lOperationStatus = lOperationInstanceParts[1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationStatusFromOperationInstance");
                Console.WriteLine(ex.Message);
            }
            return lOperationStatus;
        }

        public HashSet<operation> ReturnOnePartsOperations(part pPart)
        {
            HashSet<operation> lResultOperationSet = new HashSet<operation>();
            try
            {
                foreach (var lPartOperations in cPartsOperationsSet)
                {
                    if (lPartOperations.getPartExpr().Equals(pPart.names))
                        lResultOperationSet = lPartOperations.getOperations();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOnePartsOperations");
                Console.WriteLine(ex.Message);
            }
            return lResultOperationSet;
        }

        public HashSet<operation> ReturnOneVariantsOperations(variant pVariant)
        {
            HashSet<operation> lResultOperationSet = new HashSet<operation>();
            try
            {
                foreach (var lVariantOperations in cVariantsOperationsSet)
                {
                    if (lVariantOperations.getVariantExpr().Equals(pVariant.names))
                        lResultOperationSet = lVariantOperations.getOperations();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOneVariantsOperations");
                Console.WriteLine(ex.Message);
            }
            return lResultOperationSet;
        }

        /// <summary>
        /// This function takes an operation instance and returns the operation part from that operation instance
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        public part ReturnOperationPartFromOperationInstance(string pOperationInstance)
        {
            part lResultPart;
            try
            {
                int lOperationPartIndex;
                string[] lOperationInstanceParts = pOperationInstance.Split('_');
                lOperationPartIndex = int.Parse(lOperationInstanceParts[2]);
                lResultPart = partLookupByIndex(lOperationPartIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationPartFromOperationInstance");
                Console.WriteLine(ex.Message);
                throw;
            }
            return lResultPart;
        }

        /// <summary>
        /// This function takes an operation instance and returns the operation variant from that operation instance
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        public variant ReturnOperationVariantFromOperationInstance(string pOperationInstance)
        {
            variant lResultVariant;
            try
            {
                int lOperationVariantIndex;
                string[] lOperationInstanceVariants = pOperationInstance.Split('_');
                lOperationVariantIndex = int.Parse(lOperationInstanceVariants[2]);
                lResultVariant = variantLookupByIndex(lOperationVariantIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationVariantFromOperationInstance");
                Console.WriteLine(ex.Message);
                throw;
            }
            return lResultVariant;
        }

        /// <summary>
        /// This function takes an operation instance and returns the operation transition from that operation instance
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        public string ReturnOperationTransitionFromOperationInstance(string pOperationInstance)
        {
            string lOperationState = "";
            try
            {
                string[] lOperationInstanceParts = pOperationInstance.Split('_');
                lOperationState = lOperationInstanceParts[3];
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationTransitionFromOperationInstance");
                Console.WriteLine(ex.Message);
                throw;
            }
            return lOperationState;
        }

        public void addInActiveOperationName(String pOperationName)
        {
            try
            {
                //TODO: for now just to be simple we will make the ActiveOperationNamesSet just the names of the operations
                if (!InActiveOperationNamesSet.Contains(pOperationName))
                    InActiveOperationNamesSet.Add(pOperationName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addInActiveOperationName");
                Console.WriteLine(ex.Message);
            }
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
                Console.WriteLine("error in giveNextStateActiveOperationName, pActiveOperationName: " + pActiveOperationName);                
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in getOperationNameFromActiveOperation");
                Console.WriteLine(ex.Message);
            }
            return lResultOperationName;
        }

        public operation getOperationFromActiveOperation(string pActiveOperationName)
        {
            operation lResultOperation = null;
            try
            {
                string[] parts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Part_Transition
                if (parts.Length >= 1)
                    lResultOperation = operationLookupByName(parts[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getOperationFromActiveOperation");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in getPartFromActiveOperation");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in getPartIndexFromActiveOperation, pActiveOperationName: " + pActiveOperationName);
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in getVariantIndexFromActiveOperation, pActiveOperationName: " + pActiveOperationName);
                Console.WriteLine(ex.Message);
            }

            return lVariantIndex;
        }

        public int getOperationTransitionNumberFromActiveOperation(string pActiveOperationName)
        {
            int lOpTransNum = 0;
            try
            {
                if (!pActiveOperationName.Contains("Possible") && !pActiveOperationName.Contains("Use"))
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
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getOperationTransitionNumberFromActiveOperation, pActiveOperationName: " + pActiveOperationName);
                Console.WriteLine(ex.Message);
            }
            return lOpTransNum;
        }

        private void calculatePartIndexForActiveOperation(string pActiveOperationName)
        {
            try
            {
                part lActivePart = getPartFromActiveOperationName(pActiveOperationName);
                operation lActiveOperation = getOperationFromActiveOperation(pActiveOperationName);
                //Here we have to find any variantOperations which ourActive Operation is part of
                foreach (partOperations lPartOperations in PartsOperationsSet)
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in calculatePartIndexForActiveOperation");
                Console.WriteLine(ex.Message);
            }
        }

        private int calculateTransitionNumberForActiveOperation(string pActiveOperationName)
        {
            int lTransitionNo = 0;
            try
            {
                part lActivePart = getPartFromActiveOperationName(pActiveOperationName);
                operation lActiveOperation = getOperationFromActiveOperation(pActiveOperationName);
                //Here we have to find any variantOperations which ourActive Operation is part of
                foreach (partOperations lPartOperations in PartsOperationsSet)
                {
                    string lPartExpression = lPartOperations.getPartExpr();

                    if (lPartExpression.Contains(lActivePart.names))
                    {
                        lTransitionNo = 0;
                        foreach (operation lOperation in lPartOperations.getOperations())
                        {
                            lTransitionNo += 2;
                            if (lOperation.Equals(lActiveOperation))
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in calculateTransitionNumberForActiveOperation");
                Console.WriteLine(ex.Message);
            }
            return lTransitionNo;
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
                Console.WriteLine("error in setActiveOperationMissingTransitionNumber");
                Console.WriteLine(ex.Message);
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
                updateOperationNameInPartOperationMapping(pOldOperationName, pNewOperationName);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in refactorOperationName");
                Console.WriteLine(ex.Message);
            }
        }

        private void updateOperationName(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                operation lFoundOperation = cOperationNameLookup[pOldOperationName];
                if (lFoundOperation != null)
                    lFoundOperation.names = pNewOperationName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in updateOperationName");
                Console.WriteLine(ex.Message);
            }
        }

        private void updateOperationNameInPartOperationMapping(string pOldOperationName, string pNewOperationName)
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
                Console.WriteLine("error in updateOperationNameInPartOperationMapping");
                Console.WriteLine(ex.Message);
            }
        }

        private void updateOperationNameInPrePostConditions(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                
                //In this function the name of one of the operations in the local list has changed so we want to update the pre/post condition of any operation that references this operation
                foreach (operation lOperation in cOperationSet)
                {
                    var lPrecondition = operationLookupByName(pOldOperationName);
                    if (lPrecondition != null)
                    {
                        lOperation.precondition.Remove(pOldOperationName);
                        lOperation.precondition.Add(pNewOperationName);
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
                Console.WriteLine("error in updateOperationNameInPrePostConditions");
                Console.WriteLine(ex.Message);
            }
        }

        private void updateOperationNameInLocalSet(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                //In this function the name of one of the operations in the local list has changed so we want to update the local operation list
                operation lOperationToChangeName = cOperationNameLookup[pOldOperationName];
                if (lOperationToChangeName != null)
                    lOperationToChangeName.names = pNewOperationName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in updateOperationNameInLocalSet");
                Console.WriteLine(ex.Message);
            }
        }

        public void addPartsOperations(partOperations pPartOperations)
        {
            PartsOperationsSet.Add(pPartOperations);
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
                    foreach (operation lOperation in OperationSet)
                    {
                        lDataSummary += "Operation Name: " + lOperation.names + System.Environment.NewLine;
                        foreach (string lPreconditionOperationName in lOperation.precondition)
                            lDataSummary += "Operation Precondition: " + lPreconditionOperationName + System.Environment.NewLine;

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

                if (VariantsOperationsSet.Count > 0)
                {
                    //VariantOperationMappings
                    lDataSummary += "Variant Operation Mappings:" + System.Environment.NewLine;
                    foreach (variantOperations lVariantOperations in VariantsOperationsSet)
                    {
                        lDataSummary += "Variant Name: " + lVariantOperations.getVariantExpr() + System.Environment.NewLine;
                        lDataSummary += "Operations: " + System.Environment.NewLine;
                        foreach (operation lOperation in lVariantOperations.getOperations())
                        {
                            lDataSummary += "Operation Name: " + lOperation.names + System.Environment.NewLine;
                        }
                    }
                }

                if (PartsOperationsSet.Count > 0)
                {
                    //PartOperationMappings
                    lDataSummary += "Part Operation Mappings:" + System.Environment.NewLine;
                    foreach (partOperations lPartOperations in PartsOperationsSet)
                    {
                        lDataSummary += "Part Name: " + lPartOperations.getPartExpr() + System.Environment.NewLine;
                        lDataSummary += "Operations: " + System.Environment.NewLine;
                        foreach (operation lOperation in lPartOperations.getOperations())
                        {
                            lDataSummary += "Operation Name: " + lOperation.names + System.Environment.NewLine;
                        }
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

                System.Console.WriteLine(lDataSummary);
                System.IO.File.WriteAllText("D:/LocalImplementation/GitHub/ProductPlatformAnalyzer/ProductPlatformAnalyzer/Output/Debug/DataSummary.txt", lDataSummary);

                //System.IO.File.WriteAllText("C:/Users/amir/Desktop/Output/Debug/DataSummary.txt", lDataSummary);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in PrintDataSummary");
                Console.WriteLine(ex.Message);
            }
        }

        public operation CreateOperationInstance(string pName
                                            , HashSet<string> pRequirements
                                            , HashSet<string> pPreconditions
                                            , HashSet<string> pPostconditions)
        {
            operation lTempOperation = new operation();
            try
            {

                lTempOperation.names = pName;
                lTempOperation.requirements = pRequirements;
                lTempOperation.precondition = pPreconditions;
                //lTempOperation.postcondition = pPostconditions;
                //addOperation(lTempOperation);

                cOperationSet.Add(lTempOperation);
                cOperationNameLookup.Add(pName, lTempOperation);
                cOperationSymbolicNameLookup.Add("O" + cOperationSymbolicNameLookup.Count + 1, lTempOperation);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateOperationInstance");
                Console.WriteLine(ex.Message);
            }
            return lTempOperation;
        }

        public part CreatePartInstance(string pName)
        {
            part lTempPart = new part();
            try
            {
                lTempPart.names = pName;
                //addPart(lTempPart);

                cPartSet.Add(lTempPart);
                cPartNameLookup.Add(pName, lTempPart);
                cIndexPartLookup.Add(cPartIndexLookup.Count + 1, lTempPart);
                cPartIndexLookup.Add(lTempPart, cPartIndexLookup.Count + 1);
                cPartSymbolicNameLookup.Add("P" + cPartSymbolicNameLookup.Count + 1, lTempPart);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreatePartInstance");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in CreateVariantInstance, pName: " + pName);
                Console.WriteLine(ex.Message);
            }
            return lTempVariant;
        }

        public void CreateVariantGroupInstance(string pName, string pGroupCardinality, HashSet<variant> pVariantSet)
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
                Console.WriteLine("error in CreateVariantGroupInstance");
                Console.WriteLine(ex.Message);
            }
        }

        public partOperations CreatePartOperationMappingTemporaryInstance(string pPartName, HashSet<string> pOperationSet)
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
                Console.WriteLine("error in CreatePartOperationMappingInstance");
                Console.WriteLine(ex.Message);
            }
            return lPartOperations;
        }

        public void CreatePartOperationMappingInstance(string pPartName, HashSet<string> pOperationSet)
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
                Console.WriteLine("error in CreatePartOperationMappingInstance");
                Console.WriteLine(ex.Message);
            }
        }

        public void CreateItemUsageRuleInstance(variant pVariant, HashSet<part> pPartSet)
        {
            try
            {
                //Here we have to check if the part name is a single part or an expression over parts

                itemUsageRule lItemUsageRule = new itemUsageRule();

                //lVariantOperations.setVariantExpr(variantLookupByName(pVariantName));
                lItemUsageRule.setVariant(pVariant);
                lItemUsageRule.setParts(pPartSet);


                addItemUsageRule(lItemUsageRule);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateItemUsageRuleInstance");
                Console.WriteLine(ex.Message);
            }
        }

        public void addItemUsageRule(itemUsageRule pItemUsageRule)
        {
            ItemUsageRuleSet.Add(pItemUsageRule);
        }

        public void CreatePartOperationMappingInstance(string pPartExpr, HashSet<operation> pOperationSet)
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
                Console.WriteLine("error in CreatePartOperationMappingInstance");
                Console.WriteLine(ex.Message);
            }
        }

        public void CreateVariantOperationMappingInstance(string pVariantExpr, HashSet<operation> pOperationSet)
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
                Console.WriteLine("error in CreateVariantOperationMappingInstance");
                Console.WriteLine(ex.Message);
            }
        }

        public void addVarantOperations(variantOperations pVariantOperations)
        {
            VariantsOperationsSet.Add(pVariantOperations);
        }

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
                Console.WriteLine("error in findTraitWithName");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in createTraitInstance, pName: " + pName);
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in CreateResourceInstance, pName: " + pName);
                Console.WriteLine(ex.Message);
            }
        }

        public variant createVirtualVariant(string lVariantExpr)
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
                Console.WriteLine("error in createVirtualVariant");
                Console.WriteLine(ex.Message);
            }
            return lResultVariant;
        }

        public part createVirtualPart(string lPartExpr)
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
                Console.WriteLine("error in createVirtualPart");
                Console.WriteLine(ex.Message);
            }
            return lResultPart;
        }

        //TODO: Do we need this anymore?
        public part ReturnCurrentPart(partOperations pPartOperations)
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
                Console.WriteLine("error in ReturnCurrentPart");
                Console.WriteLine(ex.Message);
            }
            return lResultPart;
        }

        //TODO: Do we need this anymore?
        public variant ReturnCurrentVariant(variantOperations pVariantOperations)
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
                Console.WriteLine("error in ReturnCurrentVariant");
                Console.WriteLine(ex.Message);
            }
            return lResultVariant;
        }


        public void addVirtualPartConstaint(part pVirtualPart, string pPartExpr)
        {
            try
            {
                //A new constraint needs to be added relating the virtual part to the variant expression
                addConstraint("-> " + pVirtualPart.names + " (" + pPartExpr + ")");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addVirtualPartConstaint");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in addVirtualVariantConstaint");
                Console.WriteLine(ex.Message);
            }
        }

        public void createVirtualPart2PartExprInstance(part pVirtualPart, string pPartExpr)
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
                Console.WriteLine("error in createVirtualPart2PartExprInstance");
                Console.WriteLine(ex.Message);
            }
        }

        public void createVirtualVariant2VariantExprInstance(variant pVirtualVariant, string pVariantExpr)
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
                Console.WriteLine("error in createVirtualVariant2VariantExprInstance");
                Console.WriteLine(ex.Message);
            }
        }

        public part createVirtualPartInstance()
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
                Console.WriteLine("error in createVirtualPartInstance");
                Console.WriteLine(ex.Message);
            }


            return lVirtualPart;
        }

        public variant createVirtualVariantInstance()
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
                Console.WriteLine("error in createVirtualVariantInstance");
                Console.WriteLine(ex.Message);
            }


            return lVirtualVariant;
        }

        public void addVirtualVariantToGroup(variant pVariant)
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
                Console.WriteLine("error in addVirtualVariantToGroup");
                Console.WriteLine(ex.Message);
            }
        }


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
                Console.WriteLine("error in getNextVariantIndex");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in getNextPartIndex");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in getVirtualVariantIndex");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in getVirtualPartIndex");
                Console.WriteLine(ex.Message);
            }
            return lVirtualPartIndex;
        }

        private int getMaxVirtualVariantNumber()
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
                Console.WriteLine("error in getMaxVirtualVariantNumber");
                Console.WriteLine(ex.Message);
            }
            return lResultIndex;
        }

        private int getMaxVirtualPartNumber()
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
                Console.WriteLine("error in getMaxVirtualPartNumber");
                Console.WriteLine(ex.Message);
            }
            return lResultIndex;
        }

        private string getNextVirtualVariantName()
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
                Console.WriteLine("error in getNextVirtualVariantName");
                Console.WriteLine(ex.Message);
            }
            return lResultName;
        }

        private string getNextVirtualPartName()
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
                Console.WriteLine("error in getNextVirtualPartName");
                Console.WriteLine(ex.Message);
            }
            return lResultName;
        }

        public string findVirtualVariantExpression(string lVirtualVariant)
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
                Console.WriteLine("error in findVirtualVariantExpression");                
                Console.WriteLine(ex.Message);
            }
            return lVirtualVariantExpr;
        }

        public string findVirtualPartExpression(string lVirtualPart)
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
                Console.WriteLine("error in findVirtualPartExpression");
                Console.WriteLine(ex.Message);
            }
            return lVirtualPartExpr;
        }


        public String getXMLNodeAttributeInnerText(XmlNode lNode, String lAttributeName)
        {
            String lResultAttributeText = "";

            try
            {
                lResultAttributeText =lNode[lAttributeName].InnerText;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getXMLNodeAttributeInnerText, node " + lNode.Name + " does not have attribute " + lAttributeName);
                Console.WriteLine(ex.Message);
            }
            return lResultAttributeText;
        }

        //TODO: rename this function to show that you are loading from input
        public HashSet<partOperations> createPartOperationTemporaryInstances(XmlDocument pXDoc)
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
                Console.WriteLine("error in createPartOperationInstances");
                Console.WriteLine(ex.Message);
            }
            return lPartOperationsSet;
        }

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
                Console.WriteLine("error in createTraitInstances");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in createResourceInstances");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("error in createConstraintInstances");                
                Console.WriteLine(ex.Message);
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
                    throw new InitialDataIncompleteException("Initial data did not contain variant group information! Data not loaded.");
                    ////Console.WriteLine("Initial data did not contain variant group information! Data not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        HashSet<variant> lVariantGroupVariants = new HashSet<variant>();

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
                            Console.WriteLine("Variant group defined without any variants! Data not loaded.");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariantGroupInstances");                
                Console.WriteLine(ex.Message);
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
                    throw new InitialDataIncompleteException("Initial data did not contain variant information! Data not loaded.");
                    ////Console.WriteLine("Initial data did not contain variant information! Data not loaded.");

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
                Console.WriteLine("error in createVariantInstances");                
                Console.WriteLine(ex.Message);
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
                    throw new InitialDataIncompleteException("Initial data did not contain part information! Data not loaded.");
                    ////Console.WriteLine("Initial data did not contain variant information! Data not loaded.");

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
                        CreatePartInstance(getXMLNodeAttributeInnerText(lNode, "partName"));
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createPartInstances");
                Console.WriteLine(ex.Message);
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
                    throw new InitialDataIncompleteException("Initial data did not contain item usage rule information! Data not loaded.");
                    ////Console.WriteLine("Initial data did not contain variant information! Data not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        HashSet<part> lParts = new HashSet<part>();

                        XmlNodeList itemUsageRulePartsNodeList = lNode["partRefs"].ChildNodes;
                        if (itemUsageRulePartsNodeList.Count > 0)
                        {
                            foreach (XmlNode lItemUsageRuleItem in itemUsageRulePartsNodeList)
                            {
                                lParts.Add(partLookupByName(lItemUsageRuleItem.InnerText));
                            }

                            variant lTempVariant = variantLookupByName(getXMLNodeAttributeInnerText(lNode, "variantRefs"));
                            CreateItemUsageRuleInstance(lTempVariant
                                                    , lParts);

                            lDataLoaded = true;
                        }
                        else
                        {
                            lDataLoaded = false;
                            Console.WriteLine("Item usage rules defined without any parts! Data not loaded.");

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createItemUsageRulesInstances");
                Console.WriteLine(ex.Message);
            }
            return lDataLoaded;
        }
        public bool createPartOperationsInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//partoperation");

                if (nodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    throw new InitialDataIncompleteException("Initial data did not contain part - operation information! Data not loaded.");
                    ////Console.WriteLine("Initial data did not contain variant information! Data not loaded.");

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
                            Console.WriteLine("Part-operations mapping defined without any operations! Data not loaded.");

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createPartOperationsInstances");
                Console.WriteLine(ex.Message);
            }
            return lDataLoaded;
        }

        public bool createVariantOperationsInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//variantOperationMapping");

                if (nodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    throw new InitialDataIncompleteException("Initial data did not contain variant - operation information! Data not loaded.");
                    ////Console.WriteLine("Initial data did not contain variant information! Data not loaded.");

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
                            Console.WriteLine("Variant-operations mapping defined without any operations! Data not loaded.");

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariantOperationsInstances");
                Console.WriteLine(ex.Message);
            }
            return lDataLoaded;
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
                    throw new InitialDataIncompleteException("Initial data did not contain operation infor! Data not loaded.");
                    ////Console.WriteLine("Initial data did not contain operation infor! Data not loaded.");
                }
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        HashSet<string> lOperationPrecondition = new HashSet<string>();
                        HashSet<string> lOperationPostcondition = new HashSet<string>();
                        HashSet<string> lOperationRequirement = new HashSet<string>();

                        if (lNode["requirements"] != null)
                        {
                            XmlNodeList opRequirementsList = lNode["requirements"].ChildNodes;
                            foreach (XmlNode lOpRequirement in opRequirementsList)
                            {
                                lOperationRequirement.Add(lOpRequirement.InnerText);
                            }
                        }

                        if (lNode["operationPrecondition"] != null)
                        {
                            XmlNodeList opPreconditionNodeList = lNode["operationPrecondition"].ChildNodes;
                            foreach (XmlNode lOpPrecondition in opPreconditionNodeList)
                            {
                                lOperationPrecondition.Add(lOpPrecondition.InnerText);
                            }
                        }

                        if (lNode["operationPostcondition"] != null)
                        {
                            XmlNodeList opPostconditionNodeList = lNode["operationPostcondition"].ChildNodes;
                            foreach (XmlNode lOpPostcondition in opPostconditionNodeList)
                            {
                                lOperationPostcondition.Add(lOpPostcondition.InnerText);
                            }

                        }

                        CreateOperationInstance(getXMLNodeAttributeInnerText(lNode, "operationName")
                                                , lOperationRequirement
                                                , lOperationPrecondition
                                                , lOperationPostcondition);
                        lDataLoaded = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createOperationInstances");
                Console.WriteLine(ex.Message);
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
