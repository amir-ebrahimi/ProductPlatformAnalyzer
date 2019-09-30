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
        #region Private_fields
//        private OutputHandler cOutputHandler;

//        //private List<variant> cVariantList;
//        private HashSet<variant> cVariantSet = new HashSet<variant>();
//        private Dictionary<string, variant> cVariantNameLookup = new Dictionary<string, variant>();
//        private Dictionary<variant, int> cVariantIndexLookup = new Dictionary<variant, int>();
//        private Dictionary<int, variant> cIndexVariantLookup = new Dictionary<int, variant>();
//        private Dictionary<string, variant> cVariantSymbolicNameLookup = new Dictionary<string, variant>();

//        private HashSet<variantGroup> cVariantGroupSet;

//        //private List<part> cPartList;
//        private HashSet<part> cPartSet;
//        private bool cUsePartInfo;
//        private Dictionary<string, part> cPartNameLookup = new Dictionary<string, part>();
//        private Dictionary<part, int> cPartIndexLookup = new Dictionary<part, int>();
//        private Dictionary<int, part> cIndexPartLookup = new Dictionary<int, part>();
//        private Dictionary<string, part> cPartSymbolicNameLookup = new Dictionary<string, part>();

//        private HashSet<itemUsageRule> cItemUsageRuleSet;

//        private HashSet<string> cConstraintSet;

//        //private List<operation> cOperationList;
//        private HashSet<Operation> cOperationSet;
//        private Dictionary<string, Operation> cOperationNameLookup = new Dictionary<string, Operation>();
//        private Dictionary<string, Operation> cOperationSymbolicNameLookup = new Dictionary<string, Operation>();

////        private HashSet<OperationInstance> cOperationInstanceSet = new HashSet<OperationInstance>();
//        //private Dictionary<Tuple<string, string>, OperationInstance> cOperationInstanceDictionary = new Dictionary<Tuple<string, string>, OperationInstance>();
//        //private List<KeyValuePair<string, OperationInstance>> cOperationInstanceNameLookup = new List<KeyValuePair<string, OperationInstance>>();
//        //private Dictionary<string, OperationInstance> cOperationInstanceSymbolicNameLookup = new Dictionary<string, OperationInstance>();

//        private HashSet<resource> cResourceSet;
//        private Dictionary<string, resource> cResourceNameLookup = new Dictionary<string, resource>();
//        private Dictionary<string, resource> cResourceSymbolicNameLookup = new Dictionary<string, resource>();

//        private HashSet<trait> cTraitSet;
//        private Dictionary<string, trait> cTraitNameLookup = new Dictionary<string, trait>();
//        private Dictionary<string, trait> cTraitSymbolicNameLookup = new Dictionary<string, trait>();
        #endregion

        #region Getter-Setters
        //public HashSet<variant> VariantSet
        //{
        //    get { return this.cVariantSet; }
        //    set { this.cVariantSet = value; }
        //}

        //public Dictionary<string, variant> VariantNameLookup
        //{
        //    get { return this.cVariantNameLookup; }
        //    set { this.cVariantNameLookup = value; }
        //}

        //public Dictionary<variant, int> VariantIndexLookup
        //{
        //    get { return this.cVariantIndexLookup; }
        //    set { this.cVariantIndexLookup = value; }
        //}

        //public Dictionary<int, variant> IndexVariantLookup
        //{
        //    get { return this.cIndexVariantLookup; }
        //    set { this.cIndexVariantLookup = value; }
        //}

        //public Dictionary<string, variant> VariantSymbolicNameLookup
        //{
        //    get { return this.cVariantSymbolicNameLookup; }
        //    set { this.cVariantSymbolicNameLookup = value; }
        //}

        //public HashSet<variantGroup> VariantGroupSet
        //{
        //    get { return this.cVariantGroupSet; }
        //    set { this.cVariantGroupSet = value; }
        //}

        //public HashSet<part> PartSet
        //{
        //    get { return this.cPartSet; }
        //    set { this.cPartSet = value; }
        //}

        //public bool UsePartInfo
        //{
        //    get { return this.cUsePartInfo; }
        //    set { this.cUsePartInfo = value; }
        //}

        //public Dictionary<string, part> PartNameLookup
        //{
        //    get { return this.cPartNameLookup; }
        //    set { this.cPartNameLookup = value; }
        //}

        //public Dictionary<part, int> PartIndexLookup
        //{
        //    get { return this.cPartIndexLookup; }
        //    set { this.cPartIndexLookup = value; }
        //}

        //public Dictionary<int, part> IndexPartLookup
        //{
        //    get { return this.cIndexPartLookup; }
        //    set { this.cIndexPartLookup = value; }
        //}

        //public Dictionary<string, part> PartSymbolicNameLookup
        //{
        //    get { return this.cPartSymbolicNameLookup; }
        //    set { this.cPartSymbolicNameLookup = value; }
        //}

        //public HashSet<Operation> OperationSet
        //{
        //    get { return this.cOperationSet; }
        //    set { this.cOperationSet = value; }
        //}

        //public Dictionary<string, Operation> OperationNameLookup
        //{
        //    get { return this.cOperationNameLookup; }
        //    set { this.cOperationNameLookup = value; }
        //}

        //public Dictionary<string, Operation> OperationSymbolicNameLookup
        //{
        //    get { return this.cOperationSymbolicNameLookup; }
        //    set { this.cOperationSymbolicNameLookup = value; }
        //}

        ////public Dictionary<Tuple<string, string>, OperationInstance> OperationInstanceDictionary
        ////{
        ////    get { return this.cOperationInstanceDictionary; }
        ////    set { this.OperationInstanceDictionary = value; }
        ////}

        ////public HashSet<OperationInstance> OperationInstanceSet
        ////{
        ////    get { return this.cOperationInstanceSet; }
        ////    set { this.cOperationInstanceSet = value; }
        ////}

        ///*public List<KeyValuePair<string, OperationInstance>> OperationInstanceNameLookup
        //{
        //    get { return this.cOperationInstanceNameLookup; }
        //    set { this.cOperationInstanceNameLookup = value; }
        //}*/

        ///*public Dictionary<string, OperationInstance> OperationInstanceSymbolicNameLookup
        //{
        //    get { return this.cOperationInstanceSymbolicNameLookup; }
        //    set { this.cOperationInstanceSymbolicNameLookup = value; }
        //}*/

        //public HashSet<itemUsageRule> ItemUsageRuleSet
        //{
        //    get { return this.cItemUsageRuleSet; }
        //    set { this.cItemUsageRuleSet = value; }
        //}

        //public HashSet<string> ConstraintSet
        //{
        //    get { return this.cConstraintSet; }
        //    set { this.cConstraintSet = value; }
        //}

        //public HashSet<resource> ResourceSet
        //{
        //    get { return this.cResourceSet; }
        //    set { this.cResourceSet = value; }
        //}

        //public Dictionary<string, resource> ResourceNameLookup
        //{
        //    get { return this.cResourceNameLookup; }
        //    set { this.cResourceNameLookup = value; }
        //}

        //public Dictionary<string, resource> ResourceSymbolicNameLookup
        //{
        //    get { return this.cResourceSymbolicNameLookup; }
        //    set { this.cResourceSymbolicNameLookup = value; }
        //}

        //public HashSet<trait> TraitSet
        //{
        //    get { return this.cTraitSet; }
        //    set { this.cTraitSet = value; }
        //}

        //public Dictionary<string, trait> TraitNameLookup
        //{
        //    get { return this.cTraitNameLookup; }
        //    set { this.cTraitNameLookup = value; }
        //}

        //public Dictionary<string, trait> TraitSymbolicNameLookup
        //{
        //    get { return this.cTraitSymbolicNameLookup; }
        //    set { this.cTraitSymbolicNameLookup = value; }
        //}
        #endregion

        #region Props
        public OutputHandler OutputHandler { get; set; }
        public HashSet<Variant> VariantSet { get; set; }
        public Dictionary<string, Variant> VariantNameLookup { get; set; }
        //public Dictionary<Variant, int> VariantIndexLookup { get; set; }
        //public Dictionary<int, Variant> IndexVariantLookup { get; set; }
        //public Dictionary<string, Variant> VariantSymbolicNameLookup { get; set; }
        public HashSet<VariantGroup> VariantGroupSet { get; set; }
        public HashSet<Part> PartSet { get; set; }
        public bool UsePartInfo { get; set; }
        public Dictionary<string, Part> PartNameLookup { get; set; }
        //public Dictionary<Part, int> PartIndexLookup { get; set; }
        //public Dictionary<int, Part> IndexPartLookup { get; set; }
        //public Dictionary<string, Part> PartSymbolicNameLookup { get; set; }
        public HashSet<Operation> OperationSet { get; set; }
        public Dictionary<string, Operation> OperationNameLookup { get; set; }
        //public Dictionary<string, Operation> OperationSymbolicNameLookup { get; set; }
        public HashSet<PartUsageRule> PartUsageRuleSet { get; set; }
        public HashSet<string> ConstraintSet { get; set; }
        public HashSet<Resource> ResourceSet { get; set; }
        public Dictionary<string, Resource> ResourceNameLookup { get; set; }
        //public Dictionary<string, Resource> ResourceSymbolicNameLookup { get; set; }
        public HashSet<Trait> TraitSet { get; set; }
        public Dictionary<string, Trait> TraitNameLookup { get; set; }
        //public Dictionary<string, Trait> TraitSymbolicNameLookup { get; set; }
        #endregion

        public FrameworkWrapper(OutputHandler pOutputHandler)
        {
            OutputHandler = pOutputHandler;

            VariantSet = new HashSet<Variant>();
            VariantNameLookup = new Dictionary<string, Variant>();
            //VariantIndexLookup = new Dictionary<Variant, int>();
            //IndexVariantLookup = new Dictionary<int, Variant>();
            //VariantSymbolicNameLookup = new Dictionary<string, Variant>();

            VariantGroupSet = new HashSet<VariantGroup>();

            PartSet = new HashSet<Part>();
            UsePartInfo = true;
            PartNameLookup = new Dictionary<string, Part>();
            //PartIndexLookup = new Dictionary<Part, int>();
            //IndexPartLookup = new Dictionary<int, Part>();
            //PartSymbolicNameLookup = new Dictionary<string, Part>();

            OperationSet = new HashSet<Operation>();
            OperationNameLookup = new Dictionary<string, Operation>();
            //OperationSymbolicNameLookup = new Dictionary<string, Operation>();

            //OperationInstanceNameLookup = new List<KeyValuePair<string, OperationInstance>>();
            //OperationInstanceSymbolicNameLookup = new Dictionary<string, OperationInstance>();

            ConstraintSet = new HashSet<string>();

            PartUsageRuleSet = new HashSet<PartUsageRule>();

            ResourceSet = new HashSet<Resource>();
            ResourceNameLookup = new Dictionary<string, Resource>();
            //ResourceSymbolicNameLookup = new Dictionary<string, Resource>();

            TraitSet = new HashSet<Trait>();
            TraitNameLookup = new Dictionary<string, Trait>();
            //TraitSymbolicNameLookup = new Dictionary<string, Trait>();
        }

        public bool ExistVariantByName(string pVariantName)
        {
            bool lResult = false;
            try
            {
                if (VariantNameLookup.ContainsKey(pVariantName))
                    lResult = true;
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in existVariantByName");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResult;
        }

        public Variant VariantLookupByName(string pVariantName)
        {
            Variant lResultVariant = null;
            try
            {
                if (VariantNameLookup.ContainsKey(pVariantName))
                    lResultVariant = VariantNameLookup[pVariantName];
                else
                    OutputHandler.PrintMessageToConsole("Variant " + pVariantName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in variantLookupByName");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultVariant;
        }

        //MIGHT BE NEEDED LATER
        //public Variant VariantLookupBySymbolicName(string pVariantSymbolicName)
        //{
        //    Variant lResultVariant = null;
        //    try
        //    {
        //        if (VariantSymbolicNameLookup.ContainsKey(pVariantSymbolicName))
        //            lResultVariant = VariantSymbolicNameLookup[pVariantSymbolicName];
        //        else
        //            OutputHandler.PrintMessageToConsole("Variant " + pVariantSymbolicName + " not found in Dictionary!");
        //    }
        //    catch (Exception ex)
        //    {
        //        OutputHandler.PrintMessageToConsole("error in variantLookupBySymbolicName");
        //        OutputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lResultVariant;
        //}

        public bool HaveVariantWithName(string pVariantName)
        {
            bool lTempResult = false;
            try
            {
                lTempResult = VariantNameLookup.ContainsKey(pVariantName);
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in haveVariantWithName, pVariantName: " + pVariantName);
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lTempResult;
        }

        public Part PartLookupByName(string pPartName, bool pWithUserMsg = true)
        {
            Part lResultPart = null;
            try
            {
                if (PartNameLookup.ContainsKey(pPartName))
                    lResultPart = PartNameLookup[pPartName];
                else
                    if (pWithUserMsg)
                        OutputHandler.PrintMessageToConsole("Part " + pPartName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in partLookupByName");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultPart;
        }

        //public Part PartLookupByIndex(int pPartIndex)
        //{
        //    Part lResultPart = null;
        //    try
        //    {
        //        if (IndexPartLookup.ContainsKey(pPartIndex))
        //            lResultPart = IndexPartLookup[pPartIndex];
        //        else
        //            OutputHandler.PrintMessageToConsole("Part " + pPartIndex + " not found in Dictionary!");
        //    }
        //    catch (Exception ex)
        //    {
        //        OutputHandler.PrintMessageToConsole("error in partLookupByIndex");
        //        OutputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lResultPart;
        //}

        //public int IndexLookupByPart(Part pPart)
        //{
        //    int lResultIndex = 0;
        //    try
        //    {
        //        if (PartIndexLookup.ContainsKey(pPart))
        //            lResultIndex = PartIndexLookup[pPart];
        //        else
        //            OutputHandler.PrintMessageToConsole("Part " + pPart.Names + " not found in Dictionary!");
        //    }
        //    catch (Exception ex)
        //    {
        //        OutputHandler.PrintMessageToConsole("error in indexLookupByPart");
        //        OutputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lResultIndex;
        //}

        //public Variant VariantLookupByIndex(int pVariantIndex)
        //{
        //    Variant lResultVariant = null;
        //    try
        //    {
        //        if (IndexVariantLookup.ContainsKey(pVariantIndex))
        //            lResultVariant = IndexVariantLookup[pVariantIndex];
        //        else
        //            OutputHandler.PrintMessageToConsole("Variant " + pVariantIndex + " not found in Dictionary!");
        //    }
        //    catch (Exception ex)
        //    {
        //        OutputHandler.PrintMessageToConsole("error in variantLookupByIndex");
        //        OutputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lResultVariant;
        //}

        //public int IndexLookupByVariant(Variant pVariant)
        //{
        //    int lResultIndex = 0;
        //    try
        //    {
        //        if (VariantIndexLookup.ContainsKey(pVariant))
        //            lResultIndex = VariantIndexLookup[pVariant];
        //        else
        //            OutputHandler.PrintMessageToConsole("Variant " + pVariant.Names + " not found in Dictionary!");
        //    }
        //    catch (Exception ex)
        //    {
        //        OutputHandler.PrintMessageToConsole("error in indexLookupByVariant");
        //        OutputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lResultIndex;
        //}

        public bool HavePartWithName(string pPartName)
        {
            bool lTempResult = false;
            try
            {
                lTempResult = PartNameLookup.ContainsKey(pPartName);
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in havePartWithName, pPartName: " + pPartName);
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lTempResult;
        }

        //public Part PartLookupBySymbolicName(string pPartSymbolicName)
        //{
        //    Part lResultPart = null;
        //    try
        //    {
        //        if (PartSymbolicNameLookup.ContainsKey(pPartSymbolicName))
        //            lResultPart = PartSymbolicNameLookup[pPartSymbolicName];
        //        else
        //            OutputHandler.PrintMessageToConsole("Part " + pPartSymbolicName + " not found in Dictionary!");
        //    }
        //    catch (Exception ex)
        //    {
        //        OutputHandler.PrintMessageToConsole("error in partLookupBySymbolicName");
        //        OutputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lResultPart;
        //}

        public Operation OperationLookupByName(string pOperationName)
        {
            Operation lResultOperation = null;
            try
            {
                if (OperationNameLookup.ContainsKey(pOperationName))
                    lResultOperation = OperationNameLookup[pOperationName];
                else
                    OutputHandler.PrintMessageToConsole("Operation " + pOperationName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in operationLookupByName");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultOperation;
        }

        //public Operation OperationLookupBySymbolicName(string pOperationSymbolicName)
        //{
        //    Operation lResultOperation = null;
        //    try
        //    {
        //        if (OperationSymbolicNameLookup.ContainsKey(pOperationSymbolicName))
        //            lResultOperation = OperationSymbolicNameLookup[pOperationSymbolicName];
        //        else
        //            OutputHandler.PrintMessageToConsole("Operation " + pOperationSymbolicName + " not found in Dictionary!");
        //    }
        //    catch (Exception ex)
        //    {
        //        OutputHandler.PrintMessageToConsole("error in operationLookupBySymbolicName");
        //        OutputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lResultOperation;
        //}

        //public OperationInstance operationInstanceLookup(string pOperationName, string pTransitionNumber)
        //{
        //    OperationInstance lResultOperationInstance = null;
        //    try
        //    {
        //        Tuple<string, string> lKeyTuple = new Tuple<string,string>(pOperationName, pTransitionNumber);

        //        if (OperationInstanceDictionary.ContainsKey(lKeyTuple))
        //            lResultOperationInstance = OperationInstanceDictionary[lKeyTuple];
        //    }
        //    catch (Exception ex)
        //    {
        //        cOutputHandler.printMessageToConsole("error in operationInstanceLookup");
        //        cOutputHandler.printMessageToConsole(ex.Message);
        //    }
        //    return lResultOperationInstance;
        //}

        public HashSet<OperationInstance> GetOperationInstancesInOneTransition(int pTransitionNumber)
        {
            
            HashSet<OperationInstance> lResultList = new HashSet<OperationInstance>();
            
            foreach (Operation lCurrentAbstractOperation in OperationSet)
            {
                OperationInstance lTempOperationInstance = lCurrentAbstractOperation.GetOperationInstanceForTransition(pTransitionNumber);
                if (lTempOperationInstance!=null)
                    lResultList.Add(lTempOperationInstance);
            }
            /*
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
            }*/
            return lResultList;
        }

        public HashSet<OperationInstance> GetOperationInstancesForOneOperationInOneTrasition(Operation pOperation, int pTransitionNumber=-1)
        {
            HashSet<OperationInstance> lResultList = new HashSet<OperationInstance>();
            try
            {
                HashSet<OperationInstance> lOperationInstancesInOneTransitionNo = GetOperationInstancesInOneTransition(pTransitionNumber);
                foreach (var lOperationInstance in lOperationInstancesInOneTransitionNo)
                {
                    if (lOperationInstance.AbstractOperation.Equals(pOperation))
                        if (pTransitionNumber!=-1 && lOperationInstance.TransitionNumber.Equals(pTransitionNumber.ToString()))
                            lResultList.Add(lOperationInstance);
                }
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in getOperationInstancesForOneOperationInOneTrasition");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultList;

        }

        /*public List<OperationInstance> operationInstanceLookupByName(string pOperationInstanceName)
        {
            List<OperationInstance> lResultOperationInstance = new List<OperationInstance>();
            try
            {
                var result = OperationInstanceNameLookup.Where(kvp => kvp.Key == pOperationInstanceName);

                if (result.Any())
                {
                    foreach (var kvp in result)
                    {
                        lResultOperationInstance.Add(new OperationInstance(kvp.Value.AbstractOperation
                                                                            , kvp.Value.TransitionNumber));
                    }
                }
                else
                    OutputHandler.printMessageToConsole("Operation Instance " + pOperationInstanceName + " not found in Dictionary!");

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

        public Resource ResourceLookupByName(string pResourceName)
        {
            Resource lResultResource = null;
            try
            {
                if (ResourceNameLookup.ContainsKey(pResourceName))
                    lResultResource = ResourceNameLookup[pResourceName];
                else
                    OutputHandler.PrintMessageToConsole("Resource " + pResourceName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in resourceLookupByName");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultResource;
        }

        //public Resource ResourceLookupBySymbolicName(string pResourceSymbolicName)
        //{
        //    Resource lResultResource = null;
        //    try
        //    {
        //        if (ResourceSymbolicNameLookup.ContainsKey(pResourceSymbolicName))
        //            lResultResource = ResourceSymbolicNameLookup[pResourceSymbolicName];
        //        else
        //            OutputHandler.PrintMessageToConsole("Resource " + pResourceSymbolicName + " not found in Dictionary!");
        //    }
        //    catch (Exception ex)
        //    {
        //        OutputHandler.PrintMessageToConsole("error in resourceLookupBySymbolicName");
        //        OutputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lResultResource;
        //}

        public Trait TraitLookupByName(string pTraitName)
        {
            Trait lResultTrait = null;
            try
            {
                if (TraitNameLookup.ContainsKey(pTraitName))
                    lResultTrait = TraitNameLookup[pTraitName];
                else
                    OutputHandler.PrintMessageToConsole("Trait " + pTraitName + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in traitLookupByName");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultTrait;
        }

        //public Trait TraitLookupBySymbolicName(string pTraitSymbolicName)
        //{
        //    Trait lResultTrait = null;
        //    try
        //    {
        //        if (TraitSymbolicNameLookup.ContainsKey(pTraitSymbolicName))
        //            lResultTrait = TraitSymbolicNameLookup[pTraitSymbolicName];
        //        else
        //            OutputHandler.PrintMessageToConsole("Trait " + pTraitSymbolicName + " not found in Dictionary!");
        //    }
        //    catch (Exception ex)
        //    {
        //        OutputHandler.PrintMessageToConsole("error in traitLookupBySymbolicName");
        //        OutputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lResultTrait;
        //}

        /// <summary>
        /// This function returns a list of operation names
        /// </summary>
        /// <returns>List of operation names</returns>
        public List<string> GetSetOfOperationNames()
        {
            //HashSet<string> lOperationNames = new HashSet<string>();
            //try
            //{
                //foreach (Operation lOperation in OperationSet)
                //{
                //    if (!lOperationNames.Contains(lOperation.Name))
                //        lOperationNames.Add(lOperation.Name);
                //}

                var lOperationNames =
                    from operation in OperationSet
                    select operation.Name;

                return lOperationNames.ToList();

            //}
            //catch (Exception ex)
            //{
            //    OutputHandler.PrintMessageToConsole("error in getSetOfOperationNames");
            //    OutputHandler.PrintMessageToConsole(ex.Message);
            //}
        }

        public List<string> GetPreconditionForOperation(string opName)
        {
            List<string> lCondition = new List<string>();
            try
            {
                Operation op = OperationLookupByName(opName);
                lCondition = op.Precondition;
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in getPreconditionForOperation");                
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lCondition;
        }

        public String GetOperationStateFromOperationName(String pOperationName)
        {
            String tempOperationState = "";
            try
            {
                String[] lOperationNameParts = pOperationName.Split('_');

                tempOperationState = lOperationNameParts[3];
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in getOperationStateFromOperationName, pOperationName: " + pOperationName);
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return tempOperationState;
        }

        public string GetVariantGroup(string pVarName)
        {
            string lVariantgroup = "";
            try
            {
                Variant lVar = VariantLookupByName(pVarName);
                VariantGroup lVarGroup = GetVariantGroup(lVar);
                if (lVarGroup != null)
                    lVariantgroup = lVarGroup.Names;
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in getVariantGroup");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lVariantgroup;
        }

        public HashSet<VariantGroup> GetVariantGroupSet()
        {
            return VariantGroupSet;
        }

        public HashSet<string> GetConstraintSet()
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

        public VariantGroup GetVariantGroup(Variant pVar)
        {
            VariantGroup lVariantGroup = null;
            try
            {
                foreach (VariantGroup lVg in VariantGroupSet)
                {

                    foreach (Variant lV in lVg.Variants)
                    {
                        if (lV.Equals(pVar))
                            lVariantGroup = lVg;
                    }
                }
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in getVariantGroup");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lVariantGroup;
        }

        public Operation GetOperationFromOperationName(string pOperationName)
        {
            Operation lResultOperation = null;
            try
            {
                string lTempOperationName = "";
                if (pOperationName.Contains("_"))
                {
                    string[] pOperationNameParts = pOperationName.Split('_');
                    lTempOperationName = pOperationNameParts[0];
                }
                else
                    lTempOperationName = pOperationName;

                lResultOperation = OperationLookupByName(lTempOperationName);
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in getOperationFromOperationName, pOperationName: " + pOperationName);
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultOperation;
        }

        /// <summary>
        /// This function checks the requirement field of the operations
        /// </summary>
        /// <returns>The result of the analysis of the requirment field of the operations</returns>
        public bool CheckPreAnalysis()
        {
            bool lPreAnalysisResult = true;
            try
            {
                foreach (var lOperation in OperationSet)
                {
                    if (!CheckOperationRequirementField(lOperation))
                    {
                        OutputHandler.PrintMessageToConsole(lOperation.Name + " not executable!");
                        lPreAnalysisResult = false;
                    }
                }
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in checkPreAnalysis");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lPreAnalysisResult;
        }

        public bool CheckOperationRequirementField(Operation pOperation)
        {
            bool lCheckResult = false;
            try
            {
                if (pOperation.Requirement != null && pOperation.Requirement!="")
                {
                    //Check syntax of Requirement field
                    lCheckResult = CheckOperationsRequirementFieldSyntax(pOperation.Requirement);

                    //for each Part of (Trait)+ check that the traits are existing objects
                    lCheckResult = CheckExistanceOfRequirementTraits(pOperation.Requirement);

                    //Check to find resource which inheritance field matches the  (Trait)+ Part of the requirement field
                    lCheckResult = CheckValidityOfOperationRequirementsTraits(pOperation.Requirement);

                    //For the fields in the expression of the requirement add the found resource name as a prefix to fields in expression
                    AddRelevantResourceNameToOperationRequirementAttributes(pOperation);
                }
                else
                    lCheckResult = true;
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in checkOperationRequirementField, pOperationName: " + pOperation.Name);
                OutputHandler.PrintMessageToConsole(ex.Message);
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

        public HashSet<Resource> ReturnOperationChosenResource(string pOperationName)
        {
            HashSet<Resource> lResultResources = new HashSet<Resource>();
            try
            {
                //IMPORTANT: Here we have assumed that the operation requirement Part is in the Prefix format
                //Also remember that the traits have been replaced
                //The operation has the format "operand operator1 resource_name.attribute"
                Operation lOperation = OperationLookupByName(pOperationName);

                string lRequirement = lOperation.Requirement;
                //foreach (string lRequirement in lOperation.requirements)
	            //{
                    int lLastSpaceIndex = lRequirement.LastIndexOf(' ');
                    string lLastOperand = lRequirement.Substring(lLastSpaceIndex + 1);
                    string[] lLastOperandParts = lLastOperand.Split('_');
                    string lResourceName = lLastOperandParts[0];
                    lResultResources.Add(ResourceLookupByName(lResourceName));
	            //}
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in ReturnOperationChosenResource");
                OutputHandler.PrintMessageToConsole(ex.Message);
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
                    Resource lResultingResource = ReturnRequirementMatchingResource(lRequirement);

                    //We find the attibute Part of the requirement
                    string[] lRequirementFieldParts = lRequirement.Split(':');
                    string lAttributePart = lRequirementFieldParts[1].Trim();

                    //We add that resource name to add it to the field name of that requirement
                    int lIndexOfFirstOperand = lAttributePart.LastIndexOf(' ') + 1;
                    lAttributePart = lAttributePart.Insert(lIndexOfFirstOperand, lResultingResource.Name + "_");
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
                OutputHandler.PrintMessageToConsole("Error in AddRelevantResourceNameToOperationRequirementAttributes");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void ChangeOperationRequirementField(Operation pOperation, string pOldRequirement, string pNewRequirement)
        {
            try
            {
                Operation lOperation = OperationLookupByName(pOperation.Name);
                
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
                OutputHandler.PrintMessageToConsole("error in ChangeOperationRequirementField");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private Resource ReturnRequirementMatchingResource(string pRequirement)
        {
            Resource lResultingResource = new Resource();
            try
            {
                HashSet<Trait> lRequirementTraits = ExtractRequirementFieldTraits(pRequirement);

                HashSet<Resource> lResources = new HashSet<Resource>();
                //foreach (Resource lResource in ResourceSet)
                //{
                //    if (lResource.traits.SequenceEqual(lRequirementTraits))
                //        lResources.Add(lResource);
                //}

                if (lResources.Count != 0)
                    lResultingResource = lResources.First();
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in ReturnRequirementMatchingResource");
                OutputHandler.PrintMessageToConsole(ex.Message);
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
                        var lTraits = TraitLookupByName(lTraitName);

                        if (lTraits != null)
                            lSemanticCheck = lSemanticCheck && true;
                    }

                //}
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in CheckExistanceOfRequirementTraits");
                OutputHandler.PrintMessageToConsole(ex.Message);
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
                    Resource lResultingResource = ReturnRequirementMatchingResource(lRequirment);

                    if (lResultingResource != null)
                        lSemanticCheck = lSemanticCheck && true;
                //}
            }
            catch (Exception ex)
            {
                lSemanticCheck = false;
                OutputHandler.PrintMessageToConsole("error in CheckValidityOfOperationRequirementsTraits");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lSemanticCheck;
        }

        private string ExtractRequirementFieldTraitNames(string pRequirementField)
        {
            string lRequirementFieldTraitNames = "";
            try
            {
                string[] lRequirementFieldParts = pRequirementField.Split(':');
                lRequirementFieldTraitNames = lRequirementFieldParts[0].TrimEnd();
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in ExtractRequirementFieldTraitNames");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lRequirementFieldTraitNames;
        }

        private HashSet<Trait> ExtractRequirementFieldTraits(string pRequirementField)
        {
            HashSet<Trait> lRequirementFieldTraits = new HashSet<Trait>();
            string lRequirementFieldTraitNames = "";
            try
            {
                string[] lRequirementFieldParts = pRequirementField.Split(':');
                lRequirementFieldTraitNames = lRequirementFieldParts[0].TrimEnd();

                string[] lTraitNames = lRequirementFieldTraitNames.Split(',');
                for (int i = 0; i < lTraitNames.Length; i++)
			    {
                			 
                    foreach (var lTrait in TraitSet)
                    {
                        if (lTrait.Names == lTraitNames[i].Trim())
                        {
                            lRequirementFieldTraits.Add(lTrait);
                            break;
                        }
                    }
			    }
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in ExtractRequirementFieldTraits");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lRequirementFieldTraits;
        }

        //private string ExtractResourceTraitNames(Resource pResource)
        //{
        //    string lResourceTraitName = "";
        //    try
        //    {
        //        foreach (var lTrait in pResource.traits)
        //        {
        //            if (lResourceTraitName != "")
        //                lResourceTraitName += ",";
        //            lResourceTraitName += lTrait.names + " ";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        OutputHandler.PrintMessageToConsole("error in ExtractResourceTraitNames");
        //        OutputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lResourceTraitName;
        //}

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
                            OutputHandler.PrintMessageToConsole("error in CheckOperationsRequirementFieldSyntax, Operation requierment field should have the correct syntax");
                        }

                    }
                    else
                    {
                        lSyntaxCheck = false;
                        OutputHandler.PrintMessageToConsole("error in CheckOperationsRequirementFieldSyntax, Operation requierment field should contain : character");
                    }
                //}

            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in CheckOperationsRequirementFieldSyntax");
                OutputHandler.PrintMessageToConsole(ex.Message);
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

        //Never used
        //public string ReturnStringElements(HashSet<String> pSet)
        //{
        //    string lResultElements = "";
        //    try
        //    {
        //        //TODO: write with LINQ
        //        foreach (string lElement in pSet)
        //        {
        //            lResultElements += lElement;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return lResultElements;
        //}

        public string ReturnListAsString(List<string> pOperands)
        {
            string lResultString = "";
            try
            {
                foreach (string lOperand in pOperands)
                {
                    if (lResultString.Equals(""))
                        lResultString += lOperand;
                    else
                        lResultString = String.Join(" ", new string[] { "and", lResultString, lOperand });
                }
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in ReturnListAsString");                
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultString;
        }

        public void AddVariantGroup(VariantGroup pVariantGroup)
        {
            VariantGroupSet.Add(pVariantGroup);
        }

        /*public void addPart(Part pPart)
        {
            PartList.Add(pPart);
        }*/

        /*public void addVariant(VariantpVariant)
        {
            VariantList.Add(pVariant);
        }*/

        public void AddConstraint(String pConstraint)
        {
            ConstraintSet.Add(pConstraint);
        }

        public void AddResource(Resource pResource)
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

        public void AddTrait(Trait pTrait)
        {
            TraitSet.Add(pTrait);
        }

        public String GiveNextStateActiveOperationName(String pActiveOperationName)
        {
            String lNextStateActiveOperationName = "";
            try
            {
                String[] lParts = pActiveOperationName.Split('_');
                if (lParts[3] != null)
                {
                    int lCurrentActiveOperationIndex = Convert.ToInt32(lParts[3]);
                    lParts[3] = (lCurrentActiveOperationIndex + 1).ToString();
                }

                lNextStateActiveOperationName = String.Join("_", lParts);
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in giveNextStateActiveOperationName, pActiveOperationName: " + pActiveOperationName);                
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lNextStateActiveOperationName;
        }

        public string GetOperationNameFromActiveOperation(string pActiveOperationName)
        {
            string lResultOperationName = "";
            try
            {
                string[] lParts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Part_Transition
                if (lParts.Length >= 1)
                    lResultOperationName = lParts[0];
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in getOperationNameFromActiveOperation");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultOperationName;
        }

        public Operation GetOperationFromActiveOperation(string pActiveOperationName)
        {
            Operation lResultOperation = null;
            try
            {
                string[] lParts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Part_Transition
                if (lParts.Length >= 1)
                    lResultOperation = OperationLookupByName(lParts[0]);
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in getOperationFromActiveOperation");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultOperation;
        }

        //public Part GetPartFromActiveOperationName(string pActiveOperationName)
        //{
        //    Part lResultPart = null;
        //    try
        //    {
        //        string[] lParts = pActiveOperationName.Split('_');
        //        //ActiveOperationInstance: OperationName_State_Part_Transition
        //        if (lParts.Length >= 2)
        //            lResultPart = PartLookupByIndex(int.Parse(lParts[2]));
        //    }
        //    catch (Exception ex)
        //    {
        //        OutputHandler.PrintMessageToConsole("error in getPartFromActiveOperation");
        //        OutputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lResultPart;
        //}

        public int GetPartIndexFromActiveOperation(string pActiveOperationName)
        {
            int lPartIndex = -1;
            try
            {
                string[] lParts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Part_Transition
                if (lParts.Length >= 3)
                {
                    if (lParts[2] != null)
                        lPartIndex = Convert.ToInt32(lParts[2]);
                }
                else
                    //This means that the Variantfor the active operation has not been mentioned so this operation should be considered for all active variants
                    lPartIndex = -1;
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in getPartIndexFromActiveOperation, pActiveOperationName: " + pActiveOperationName);
                OutputHandler.PrintMessageToConsole(ex.Message);
            }

            return lPartIndex;
        }

        public int GetVariantIndexFromActiveOperation(string pActiveOperationName)
        {
            int lVariantIndex = -1;
            try
            {
                string[] lParts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Variant_Transition
                if (lParts.Length >= 3)
                {
                    if (lParts[2] != null)
                        lVariantIndex = Convert.ToInt32(lParts[2]);
                }
                else
                    //This means that the Variantfor the active operation has not been mentioned so this operation should be considered for all active variants
                    lVariantIndex = -1;
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in getVariantIndexFromActiveOperation, pActiveOperationName: " + pActiveOperationName);
                OutputHandler.PrintMessageToConsole(ex.Message);
            }

            return lVariantIndex;
        }

        public int GetOperationTransitionNumberFromOperationInstanceString(string pOperationInstanceString)
        {
            //This function is ONLY for operation instances which are mentioned in the pre and post condition
            //Because these operation instances are mentioned in the pre or post condition hence they are in a string format
            int lOpTransNum = 0;
            try
            {
                //TODO: To be corrected in the resource implementation
                /*if (!pActiveOperationName.Contains("Possible") && !pActiveOperationName.Contains("Use"))
                {
                    String[] parts = pOperationInstanceString.Split('_');
                    //pOperationInstanceString: OperationName_State_Transition
                    if (parts.Length.Equals(3))
                    {

                        if (parts[2] != null)
                            lOpTransNum = parts[2];
                    }
                    else
                    {
                        //This means that for the operation instance the transition number has not been mentioned
                        lOpTransNum = "";
                    }
                }*/

            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in getOperationTransitionNumberFromOperationInstanceString");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lOpTransNum;
        }

        private void SetActiveOperationMissingTransitionNumber(string pActiveOperationName, int pCalculatedTransitionNumber)
        {
            try
            {
                string lNewOperationName = pActiveOperationName + "_" + pCalculatedTransitionNumber;

                //Here we know that the operation does not have a transition number hence we set the transition number of the operation to 0
                UpdateOperationName(pActiveOperationName, lNewOperationName);

                RefactorOperationName(pActiveOperationName, lNewOperationName);
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in setActiveOperationMissingTransitionNumber");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void RefactorOperationName(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                //But as this operation name has changed we have to change any reference which is to this operation:
                //1. First in the local operation list
                UpdateOperationNameInLocalSet(pOldOperationName, pNewOperationName);

                //2. Second in the pre-condition or post condition of any of the other operations
                UpdateOperationNameInPrePostConditions(pOldOperationName, pNewOperationName);

                //3. Third in the variant-operation mappings
                //updateOperationNameInPartOperationMapping(pOldOperationName, pNewOperationName);

            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in refactorOperationName");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void UpdateOperationName(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                Operation lFoundOperation = OperationNameLookup[pOldOperationName];
                if (lFoundOperation != null)
                    lFoundOperation.Name = pNewOperationName;
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in updateOperationName");
                OutputHandler.PrintMessageToConsole(ex.Message);
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

        private void UpdateOperationNameInPrePostConditions(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                
                //In this function the name of one of the operations in the local list has changed so we want to update the pre/post condition of any operation that references this operation
                foreach (Operation lOperation in OperationSet)
                {
                    var lPrecondition = OperationLookupByName(pOldOperationName);
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
                OutputHandler.PrintMessageToConsole("error in updateOperationNameInPrePostConditions");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void UpdateOperationNameInLocalSet(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                //In this function the name of one of the operations in the local list has changed so we want to update the local operation list
                Operation lOperationToChangeName = OperationNameLookup[pOldOperationName];
                if (lOperationToChangeName != null)
                    lOperationToChangeName.Name = pNewOperationName;
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in updateOperationNameInLocalSet");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void PrintDataFileXML()
        {
            try
            {
                string lDataFileXML = "";
                //<testData>
                lDataFileXML += "<testData>" + System.Environment.NewLine;

                //Operations
                if (OperationSet.Count > 0)
                {
                    /*
                      <operations>
                        <operation>
                          <operationName>O1</operationName>
                          <trigger>V1</trigger>
                          <preconditions>
                            <operationRef>true</operationRef>
                          </preconditions>
                        </operation>
                      </operations>
                     */
                    lDataFileXML += "   <operations>" + System.Environment.NewLine;
                    foreach (Operation lOperation in OperationSet)
                    {
                        lDataFileXML += "       <operation>" + System.Environment.NewLine;
                        lDataFileXML += "           <operationName>" + lOperation.Name + "</operationName>" + System.Environment.NewLine;
                        if (lOperation.Trigger != "")
                            lDataFileXML += "           <trigger>" + lOperation.Trigger + "</trigger>" + System.Environment.NewLine;

                        if (lOperation.Requirement != "")
                            lDataFileXML += "           <requirement>" + lOperation.Requirement + "</requirement>" + System.Environment.NewLine;

                        if (lOperation.Precondition != null)
                        {
                            lDataFileXML += "           <preconditions>" + System.Environment.NewLine;
                            foreach (string lPrecondition in lOperation.Precondition)
                            {
                                lDataFileXML += "               <operationRef>" + lPrecondition + "</operationRef>" + System.Environment.NewLine;
                            }
                            lDataFileXML += "           </preconditions>" + System.Environment.NewLine;

                        }

                        if (lOperation.Postcondition != null)
                        {
                            lDataFileXML += "           <postconditions>" + System.Environment.NewLine;
                            foreach (string lPostcondition in lOperation.Postcondition)
                            {
                                lDataFileXML += "               <operationRef>" + lPostcondition + "</operationRef>" + System.Environment.NewLine;
                            }
                            lDataFileXML += "           </postconditions>" + System.Environment.NewLine;

                        }
                        lDataFileXML += "       </operation>" + System.Environment.NewLine;
                    }
                    lDataFileXML += "   </operations>" + System.Environment.NewLine;
                }

                //Variants
                if (VariantSet.Count > 0)
                {
                    //Variants and Variant Groups
                    /*
                      <variants>
                        <variant>
                          <variantName>V1</variantName>
                        </variant>
                      </variants>
                      <variantGroups>
                        <variantGroup>
                          <variantGroupName>VG1</variantGroupName>
                          <groupCardinality>choose exactly one</groupCardinality>
                          <variantRefs>
                            <variantRef>V1</variantRef>
                          </variantRefs>
                        </variantGroup>
                      </variantGroups>
                     */
                    lDataFileXML += "   <variants>" + System.Environment.NewLine;
                    foreach (Variant lVariant in VariantSet)
                    {
                        lDataFileXML += "       <variant>" + System.Environment.NewLine;
                        lDataFileXML += "           <variantName>" + lVariant.Names + "</variantName>" + System.Environment.NewLine;
                        lDataFileXML += "       </variant>" + System.Environment.NewLine;
                    }
                    lDataFileXML += "   </variants>" + System.Environment.NewLine;

                    lDataFileXML += "   <variantGroups>" + System.Environment.NewLine;
                    foreach (VariantGroup lVariantGroup in VariantGroupSet)
                    {
                        lDataFileXML += "       <variantGroup>" + System.Environment.NewLine;
                        lDataFileXML += "           <variantGroupName>" + lVariantGroup.Names + "</variantGroupName>" + System.Environment.NewLine;
                        lDataFileXML += "           <groupCardinality>" + lVariantGroup.GCardinality + "</groupCardinality>" + System.Environment.NewLine;
                        if (lVariantGroup.Variants.Count > 0)
                        {
                            lDataFileXML += "               <variantRefs>" + System.Environment.NewLine;
                            foreach (Variant lVariant in lVariantGroup.Variants)
                            {
                                lDataFileXML += "                   <variantRef>" + lVariant.Names + "</variantRef>"+ System.Environment.NewLine;
                            }
                            lDataFileXML += "               </variantRefs>" + System.Environment.NewLine;

                        }
                        lDataFileXML += "       </variantGroup>" + System.Environment.NewLine;
                    }
                    lDataFileXML += "   </variantGroups>" + System.Environment.NewLine;

                }

                //Parts
                if (PartSet.Count > 0)
                {
                    /*
                      <parts>
                        <part>
                          <partName>Medium-cab</partName>
                        </part>
                      </parts>
                      <itemusagerules>
                        <itemusagerule>
                          <partRef>Medium-cab</partRef>
                          <variantRef>Medium-cab</variantRef>
                        </itemusagerule>
                      </itemusagerules>
                     */
                    lDataFileXML += "   <parts>" + System.Environment.NewLine;
                    foreach (Part lPart in PartSet)
                    {
                        lDataFileXML += "       <part>" + System.Environment.NewLine;
                        lDataFileXML += "           <partName>" + lPart.Names + "</partName>" + System.Environment.NewLine;
                        lDataFileXML += "       </part>" + System.Environment.NewLine;
                    }
                    lDataFileXML += "   </parts>" + System.Environment.NewLine;

                    lDataFileXML += "   <itemusagerules>" + System.Environment.NewLine;
                    foreach (PartUsageRule lPartUsageRule in PartUsageRuleSet)
                    {
                        lDataFileXML += "       <itemusagerule>" + System.Environment.NewLine;
                        lDataFileXML += "           <partRef>" + lPartUsageRule.Part.Names + "</partRef>" + System.Environment.NewLine;
                        lDataFileXML += "           <variantRef>" + lPartUsageRule.VariantExpression + "</variantRef>" + System.Environment.NewLine;
                        lDataFileXML += "       </itemusagerule>" + System.Environment.NewLine;
                    }
                    lDataFileXML += "   </itemusagerules>" + System.Environment.NewLine;
                }

                //Configuration Rules
                if (ConstraintSet.Count > 0)
                {
                    /*
                      <constraints>
                        <logic>axle1</logic>
                      </constraints>
                     */
                    lDataFileXML += "   <constraints>" + System.Environment.NewLine;
                    foreach (string lConstraint in ConstraintSet)
                    {
                        lDataFileXML += "       <logic>" + lConstraint + "</logic>" + System.Environment.NewLine; ;
                    }
                    lDataFileXML += "   </constraints>" + System.Environment.NewLine;
                }

                //Traits
                if (TraitSet.Count > 0)
                {
                    /*
                    <traits>
                        <trait>
                            <traitName>TLevel</traitName>
                            <inherit></inherit>
                            <attributes>
                                <attribute>
                                    <attributeType>int</attributeType>
                                    <attributeName>level</attributeName>
                                </attribute>
                            </attributes>
                        </trait>
                    </traits>
                     */
                    lDataFileXML += "   <traits>" + System.Environment.NewLine;
                    foreach (Trait lTrait in TraitSet)
                    {
                        lDataFileXML += "       <trait>" + System.Environment.NewLine;
                        lDataFileXML += "           <traitName>" + lTrait.Names + "</traitName>" + System.Environment.NewLine; ;
                        if (lTrait.Inherit != null)
                        {
                            foreach (Trait lInherit in lTrait.Inherit)
                            {
                                lDataFileXML += "           <inherit>" + lInherit.Names + "</inherit>" + System.Environment.NewLine; ;
                            }
                        }
                        if (lTrait.Attributes != null)
                        {
                            lDataFileXML += "           <attributes>" + System.Environment.NewLine; ;
                            foreach (var lAttribute in lTrait.Attributes)
                            {
                                lDataFileXML += "               <attribute>" + System.Environment.NewLine; ;
                                lDataFileXML += "                   <attributeType>" + lAttribute.Item1 + "</attributeType>" + System.Environment.NewLine; ;
                                lDataFileXML += "                   <attributeName>" + lAttribute.Item2 + "</attributeName>" + System.Environment.NewLine; ;
                                lDataFileXML += "               </attribute>" + System.Environment.NewLine; ;
                            }
                            lDataFileXML += "           </attributes>" + System.Environment.NewLine; ;
                        }

                        lDataFileXML += "       </trait>" + System.Environment.NewLine;
                        
                    }
                    lDataFileXML += "   </traits>" + System.Environment.NewLine;
                }

                //</testData>
                lDataFileXML += "</testData>" + System.Environment.NewLine;

                //cOutputHandler.printMessageToConsole(lDataFileXML);
                System.IO.File.WriteAllText("D:/LocalImplementation/GitHub/ProductPlatformAnalyzer/ProductPlatformAnalyzer/Output/Debug/DataFileXML.xml", lDataFileXML);

                //System.IO.File.WriteAllText("C:/Users/amir/Desktop/Output/Debug/DataSummary.txt", lDataSummary);


            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in PrintDataFileXML");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void PrintDataSummary()
        {
            try
            {
                //In this function we will print a summary of the important information which is contained in the framework wrapper
                string lDataSummary = "";

                //Operations
                if (OperationSet.Count > 0)
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
                if (VariantNameLookup.Count > 0)
                {
                    lDataSummary += "Variants:" + System.Environment.NewLine;
                    foreach (Variant lVariant in VariantSet)
                    {
                        lDataSummary += "Variant Name: " + lVariant.Names + System.Environment.NewLine;
                    }

                    //Variantgroups
                    lDataSummary += "Variant Groups:" + System.Environment.NewLine;
                    foreach (VariantGroup lVariantGroup in VariantGroupSet)
                    {
                        lDataSummary += "Variant Group Name: " + lVariantGroup.Names + System.Environment.NewLine;
                        lDataSummary += "Variant Group Cardinality: " + lVariantGroup.GCardinality + System.Environment.NewLine;
                        foreach (Variant lVariant in lVariantGroup.Variants)
                            lDataSummary += "Variant Group Variant: " + lVariant.Names + System.Environment.NewLine;

                    }
                }

                //Parts
                if (PartNameLookup.Count > 0)
                {
                    lDataSummary += "Parts:" + System.Environment.NewLine;
                    foreach (Part lPart in PartSet)
                    {
                        lDataSummary += "Part Name: " + lPart.Names + System.Environment.NewLine;
                        /*                    foreach (operation lManOperation in lPart.manOperations)
                                                lDataSummary += "Part Manufacturing Operation: " + lManOperation.names + System.Environment.NewLine;*/
                    }
                }

                //Configuration Rules
                if (ConstraintSet.Count > 0)
                {
                    lDataSummary += "Configuration Rules:" + System.Environment.NewLine;
                    foreach (var lConfgurationRule in ConstraintSet)
                    {
                        lDataSummary += "Configuration Rule: " + lConfgurationRule + System.Environment.NewLine;
                        
                    }

                }

                //Traits
                if (TraitSet.Count > 0)
                {
                    lDataSummary += "Traits:" + System.Environment.NewLine;
                    foreach (Trait lTrait in TraitSet)
                    {
                        lDataSummary += "Trait Name: " + lTrait.Names + System.Environment.NewLine;
                        lDataSummary += "Attributes: " + System.Environment.NewLine;
                        foreach (Tuple<string, string> lAttribute in lTrait.Attributes)
                        {
                            lDataSummary += "Attribute Name: " + lAttribute.Item2 + System.Environment.NewLine;
                            lDataSummary += "Attribute Type: " + lAttribute.Item1 + System.Environment.NewLine;
                        }
                    }
                }

                OutputHandler.PrintMessageToConsole(lDataSummary);
                System.IO.File.WriteAllText("D:/LocalImplementation/GitHub/ProductPlatformAnalyzer/ProductPlatformAnalyzer/Output/Debug/DataSummary.txt", lDataSummary);
                
                PrintDataFileXML();

                //System.IO.File.WriteAllText("C:/Users/amir/Desktop/Output/Debug/DataSummary.txt", lDataSummary);

            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in PrintDataSummary");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public string GetOperationPreconditionsString(List<string> pPreconditions, bool pPrefix = false)
        {
            string lResultPreconditionStr = "";
            try
            {

                foreach (var lPrecondition in pPreconditions)
                {
                    if (lResultPreconditionStr.Equals(""))
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
                OutputHandler.PrintMessageToConsole("error in GetOperationPreconditionsString");
                OutputHandler.PrintMessageToConsole(ex.Message);
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
                OutputHandler.PrintMessageToConsole("error in GetOperationPostconditionsString");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultPostconditionStr;
        }

        public Operation CreateOperationInstanceNLocalSets(string pName
                                            , string pTriggers
                                            , string pRequirements
                                            , string pPrecondition
                                            , string pPostcondition
                                            , string pResource)
        {
            Operation lTempOperation = null;
            try
            {

                lTempOperation = new Operation(pName, pTriggers, pRequirements, pPrecondition, pPostcondition, pResource);

                OperationSet.Add(lTempOperation);
                OperationNameLookup.Add(pName, lTempOperation);
                //OperationSymbolicNameLookup.Add("O" + OperationSymbolicNameLookup.Count + 1, lTempOperation);

            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in CreateOperationInstanceNLocalSets");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lTempOperation;
        }

        //public void CreateOperationInstances4AllTransitions(Operation pOperation)
        //{
        //    try
        //    {
        //        var lMaxTransitionNumber = (OperationSet.Count * 2) + 1;

        //        OperationInstance lTempOperationInstance = null;

        //        for (int lTransitionNumber = 0; lTransitionNumber < lMaxTransitionNumber; lTransitionNumber++)
        //        {
        //            if (lTransitionNumber == 0)
        //                lTempOperationInstance = new OperationInstance(pOperation, lTransitionNumber.ToString());
        //            else
        //                lTempOperationInstance = lTempOperationInstance.createNextOperationInstance();

        //            //Placeholder to add any additional dictionaries on operation instances
        //            var lKeyTuple = new Tuple<string, string>(pOperation.Name, lTransitionNumber.ToString());
        //            if (!OperationInstanceDictionary.ContainsKey(lKeyTuple))
        //            {
        //                OperationInstanceDictionary.Add(lKeyTuple, lTempOperationInstance);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cOutputHandler.printMessageToConsole("error in CreateOperationInstanceInstance");
        //        cOutputHandler.printMessageToConsole(ex.Message);
        //    }
        //}

        //public OperationInstance addOperationInstance(Operation pOperation, String pTransitionNo, bool addToListOfOperationInstances = true)
        //{
        //    OperationInstance lResultOperationInstance = null;
        //    try
        //    {
        //        lResultOperationInstance = new OperationInstance(pOperation, pTransitionNo, addToListOfOperationInstances);
        //        if (addToListOfOperationInstances)
        //        {
        //            cOperationInstanceSet.Add(lResultOperationInstance);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cOutputHandler.printMessageToConsole("error in addOperationInstance");
        //        cOutputHandler.printMessageToConsole(ex.Message);
        //    }
        //    return lResultOperationInstance;
        //}

        public Part CreatePartInstanceNLocalSets(string pName)
        {
            Part lTempPart = null;
            try
            {
                lTempPart = new Part(pName);

                //TODO: This lookup might not be needed later on
                Part lFoundPart = PartLookupByName(pName, false);
                if (lFoundPart == null)
                {
                    PartSet.Add(lTempPart);
                    PartNameLookup.Add(pName, lTempPart);
                    //IndexPartLookup.Add(PartIndexLookup.Count + 1, lTempPart);
                    //PartIndexLookup.Add(lTempPart, PartIndexLookup.Count + 1);
                    //PartSymbolicNameLookup.Add("P" + PartSymbolicNameLookup.Count + 1, lTempPart);
                }
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in CreatePartInstanceNLocalSets");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lTempPart;
        }

        public Variant CreateVariantInstanceNLocalSets(string pName)
        {
            Variant lTempVariant = null;
            try
            {
                lTempVariant = new Variant(pName);

                VariantSet.Add(lTempVariant);
                VariantNameLookup.Add(pName, lTempVariant);
                //IndexVariantLookup.Add(VariantIndexLookup.Count + 1, lTempVariant);
                //VariantIndexLookup.Add(lTempVariant, VariantIndexLookup.Count + 1);
                //VariantSymbolicNameLookup.Add("V" + VariantSymbolicNameLookup.Count + 1, lTempVariant);
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in CreateVariantInstanceNLocalSets");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lTempVariant;
        }

        //public VariantGroup CreateVariantGroupInstance(string pName, string pGroupCardinality, List<Variant> pVariantSet)
        //{
        //    VariantGroup lTempVariantGroup = new VariantGroup();
        //    try
        //    {
        //        lTempVariantGroup.Names = pName;
        //        lTempVariantGroup.GCardinality = pGroupCardinality;
        //        lTempVariantGroup.Variants = pVariantSet;
        //        AddVariantGroup(lTempVariantGroup);
        //    }
        //    catch (Exception ex)
        //    {
        //        OutputHandler.PrintMessageToConsole("error in CreateVariantGroupInstance");
        //        OutputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lTempVariantGroup;
        //}

        //This function is no longer needed as the relation between Part and operations is defined in the trigger field of the operation
        /*public partOperations CreatePartOperationMappingTemporaryInstance(string pPartName, List<string> pOperationSet)
        {
            partOperations lPartOperations = new partOperations();
            try
            {
                //Here we have to check if the Part name is a single Part or an expression over parts


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

        //This function is no longer needed as the relation between Part and operations is defined in the trigger field of the operation
        /*public void CreatePartOperationMappingInstance(string pPartName, HashSet<string> pOperationSet)
        {
            try
            {
                //Here we have to check if the Part name is a single Part or an expression over parts

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

        public void CreateItemUsageRuleInstance(Part pPart, string pVariantExp)
        {
            try
            {
                //Here we have to check if the Part name is a single Part or an expression over parts

                PartUsageRule lPartUsageRule = new PartUsageRule();

                //lVariantOperations.setVariantExpr(variantLookupByName(pVariantName));
                lPartUsageRule.VariantExpression = pVariantExp;
                lPartUsageRule.Part = pPart;


                AddPartUsageRule(lPartUsageRule);
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in CreateItemUsageRuleInstance");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void AddPartUsageRule(PartUsageRule pPartUsageRule)
        {
            PartUsageRuleSet.Add(pPartUsageRule);
        }

        //This function is no longer needed as the relation between Part and operations is defined in the trigger field of the operation
        /*public void CreatePartOperationMappingInstance(string pPartExpr, HashSet<operation> pOperationSet)
        {
            try
            {
                //Here we have to check if the Part name is a single Part or an expression over parts

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

        //This function is no longer needed as the relation between Part and operations is defined in the trigger field of the operation
        /*public void CreateVariantOperationMappingInstance(string pVariantExpr, HashSet<operation> pOperationSet)
        {
            try
            {
                //Here we have to check if the Part name is a single Part or an expression over parts

                variantOperations lVariantOperations = new variantOperations();

                //lPartOperations.setPartExpr(partLoopupByName(pPartName));
                lVariantOperations.setVariantExpr(pVariantExpr);

                lVariantOperations.setOperations(pOperationSet);

                //The operations which are added as a result of Variant- operation relation are infact active operations
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

        public Trait CreateTraitInstanceNLocalSets(string pName, HashSet<Trait> pInherit, HashSet<Tuple<string,string>> pAttributes)
        {
            Trait lTempTrait = null;
            try
            {
                lTempTrait = new Trait(pName, pAttributes, pInherit);

                AddTrait(lTempTrait);

                TraitNameLookup.Add(pName, lTempTrait);
                //TraitSymbolicNameLookup.Add("T" + TraitSymbolicNameLookup.Count + 1, lTempTrait);
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in createTraitInstance, pName: " + pName);
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lTempTrait;
        }

        public Resource CreateResourceInstanceNLocalSets(string pName, HashSet<Trait> pTraits ,HashSet<Tuple<string,string,string>> pAttributes)
        {
            Resource lTempResource = null;
            try
            {
                lTempResource = new Resource(pName);

                AddResource(lTempResource);

                ResourceNameLookup.Add(pName, lTempResource);
                //ResourceSymbolicNameLookup.Add("R" + ResourceSymbolicNameLookup.Count + 1, lTempResource);
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in CreateResourceInstance, pName: " + pName);
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lTempResource;
        }

        //Virtual variants are no longer needed
        /*public VariantcreateVirtualVariant(string lVariantExpr)
        {
            VariantlResultVariant = new variant();
            try
            {
                //A new Virtual Variantneed to be build
                //AND
                //The virtual Variantneeds to be added to virtual Variantand virtual Variantgroup
                VariantlVirtualVariant = createVirtualVariantInstance();

                //TODO: URGENT add a relevant virtual Variantand an virtual Variantgroup
                addVirtualVariantToGroup(lVirtualVariant);

                //A new constraint needs to be added relating the virtual Variantto the Variantexpression
                addVirtualVariantConstaint(lVirtualVariant, lVariantExpr);

                //The new virtual Variantand the exression it represents needs to be added to virtual Variantlist in frameworkwrapper
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
        /*public Part createVirtualPart(string lPartExpr)
        {
            Part lResultPart = new part();
            try
            {
                //A new Virtual Part need to be build
                //AND
                //The virtual Part needs to be added to virtual Variantand virtual Variantgroup
                Part lVirtualPart = createVirtualPartInstance();

                //TODO: URGENT add a relevant virtual Variantand an virtual Variantgroup
                //addVirtualVariantToGroup(lVirtualPart);

                //A new constraint needs to be added relating the virtual Part to the Part expression
                addVirtualPartConstaint(lVirtualPart, lPartExpr);

                //The new virtual Variantand the exression it represents needs to be added to virtual Variantlist in frameworkwrapper
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
        /*public Part ReturnCurrentPart(partOperations pPartOperations)
        {
            Part lResultPart = null;
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
        /*public VariantReturnCurrentVariant(variantOperations pVariantOperations)
        {
            VariantlResultVariant = null;
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


        public void AddVirtualPartConstaint(Part pVirtualPart, string pPartExpr)
        {
            try
            {
                //A new constraint needs to be added relating the virtual Part to the Variantexpression
                AddConstraint("-> " + pVirtualPart.Names + " (" + pPartExpr + ")");
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in addVirtualPartConstaint");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void AddVirtualVariantConstaint(Variant pVirtualVariant, string pVariantExpr)
        {
            try
            {
                //A new constraint needs to be added relating the virtual Variantto the Variantexpression
                AddConstraint("-> " + pVirtualVariant.Names + " (" + pVariantExpr + ")");
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in addVirtualVariantConstaint");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        /*public void createVirtualPart2PartExprInstance(Part pVirtualPart, string pPartExpr)
        {
            try
            {
                //The new virtual Part and the exression it represents needs to be added to virtual Part list in frameworkwrapper
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

        /*public void createVirtualVariant2VariantExprInstance(VariantpVirtualVariant, string pVariantExpr)
        {
            try
            {
                //The new virtual Variantand the exression it represents needs to be added to virtual Variantlist in frameworkwrapper
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

        /*public Part createVirtualPartInstance()
        {
            Part lVirtualPart = new part();
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

        /*public VariantcreateVirtualVariantInstance()
        {
            VariantlVirtualVariant = new variant();
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
        /*public void addVirtualVariantToGroup(VariantpVariant)
        {
            try
            {
                foreach (VariantGroup vg in VariantGroupSet)
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

            foreach (Variantvar in variants)
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
                foreach (Part var in PartList)
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
                foreach (Part var in PartList)
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

        private int GetVirtualVariantIndex(Variant pVirtualVariant)
        {
            int lVirtualVariantIndex = 0;
            try
            {
                string lVirtualVariantName = pVirtualVariant.Names;
                int lStrartingIndex = lVirtualVariantName.IndexOf("VirtualVariant");
                lVirtualVariantIndex = int.Parse(lVirtualVariantName.Remove(lStrartingIndex + 14 + 1));
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in getVirtualVariantIndex");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lVirtualVariantIndex;
        }

        //private int GetVirtualPartIndex(Part pVirtualPart)
        //{
        //    int lVirtualPartIndex = 0;
        //    try
        //    {
        //        string lVirtualPartName = pVirtualPart.Names;
        //        int lStartingIndex = lVirtualPartName.IndexOf("VirtualPart");
        //        lVirtualPartIndex = int.Parse(lVirtualPartName.Remove(lStartingIndex + 14 + 1));
        //    }
        //    catch (Exception ex)
        //    {
        //        OutputHandler.PrintMessageToConsole("error in getVirtualPartIndex");
        //        OutputHandler.PrintMessageToConsole(ex.Message);
        //    }
        //    return lVirtualPartIndex;
        //}

        /*private int getMaxVirtualVariantNumber()
        {
            int lResultIndex = 0; 
            try
            {
                foreach (virtualVariant2VariantExpr virtualVariant2VariantExpr in cVirtualVariant2VariantExprSet)
                {
                    VariantvirtualVariant = virtualVariant2VariantExpr.getVirtualVariant();

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
                    Part virtualPart = virtualPart2PartExpr.getVirtualPart();

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
                //Each virtual Variantis named as "VirtualVariantX" which X is a number
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
                //Each virtual Part is named as "VirtualPartX" which X is a number
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


        public String GetXMLNodeAttributeInnerText(XmlNode lNode, String lAttributeName)
        {
            String lResultAttributeText = "";

            try
            {
                lResultAttributeText =lNode[lAttributeName].InnerText;
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in getXMLNodeAttributeInnerText, node " + lNode.Name + " does not have attribute " + lAttributeName);
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultAttributeText;
        }

        //This function is no longer needed as the relation between Part and operations is defined in the trigger field of the operation
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
        public bool CreateTraitInstances(XmlDocument pXDoc)
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

                        HashSet<Trait> lInheritTraits = new HashSet<Trait>();

                        XmlNodeList lInheritList = lNode["inherit"].ChildNodes;
                        foreach (XmlNode lTraitName in lInheritList)
                        {
                            lInheritTraits.Add(TraitLookupByName(lTraitName.InnerText));
                        }

                        XmlNodeList lAttributeList = lNode["attributes"].ChildNodes;
                        foreach (XmlNode lAttribute in lAttributeList)
                        {
                            lAttributes.Add(new Tuple<string, string>(GetXMLNodeAttributeInnerText(lAttribute, "attributeType")
                                            , GetXMLNodeAttributeInnerText(lAttribute, "attributeName")));
                        }

                        CreateTraitInstanceNLocalSets(GetXMLNodeAttributeInnerText(lNode, "traitName")
                                                                , lInheritTraits
                                                                , lAttributes);
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in createTraitInstances");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool CreateResourceInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList lNodeList = pXDoc.DocumentElement.SelectNodes("//resource");

                if (lNodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    //throw new InitialDataIncompleteException("Initial data did not contain Variantinformation! Variants not loaded.");
                    OutputHandler.PrintMessageToConsole("Initial data did not contain resource information! Resources not loaded.");
                }
                else
                {
                    foreach (XmlNode lNode in lNodeList)
                    {
                        HashSet<Tuple<string, string, string>> lAttributes = new HashSet<Tuple<string, string, string>>();
                        HashSet<Trait> lTraits = new HashSet<Trait>();
                        //if (lNode["traits"] != null)
                        //{
                        //    XmlNodeList traitNamesList = lNode["traits"].ChildNodes;
                        //    foreach (XmlNode lTraitRef in traitNamesList)
                        //    {
                        //        string lTraitName = lTraitRef.InnerText;
                        //        if (lTraitName != "")
                        //            lTraits.Add(traitLookupByName(lTraitName));
                        //    }

                        //}
                        //if (lNode["attributes"] != null)
                        //{
                        //    XmlNodeList attributeList = lNode["attributes"].ChildNodes;
                        //    foreach (XmlNode lAttribute in attributeList)
                        //    {
                        //        lAttributes.Add(new Tuple<string, string, string>(getXMLNodeAttributeInnerText(lAttribute, "attributeName")
                        //                                                        , getXMLNodeAttributeInnerText(lAttribute, "attributeType")
                        //                                                        , getXMLNodeAttributeInnerText(lAttribute, "attributeValue")));
                        //    }


                        //}

                        //var lResourceName = getXMLNodeAttributeInnerText(lNode, "resourceName");

                        CreateResourceInstanceNLocalSets(GetXMLNodeAttributeInnerText(lNode, "resourceName")
                                            , lTraits
                                            , lAttributes);

                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in createResourceInstances");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool CreateConstraintInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList lNodeList = pXDoc.DocumentElement.SelectNodes("//constraint");

                if (lNodeList.Count.Equals(0))
                    lDataLoaded = false;
                else
                {
                    foreach (XmlNode lNode in lNodeList)
                    {
                        string lPrefixFormat = GetXMLNodeAttributeInnerText(lNode, "logic");
                        //TODO: if the constraint are converted into infix format this line has to be used
                        //string lPrefixFormat = GeneralUtilities.parseExpression(lPrefixFormat, "prefix");
                        AddConstraint(lPrefixFormat);
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in createConstraintInstances");                
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool CreateVariantGroupInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList lNodeList = pXDoc.DocumentElement.SelectNodes("//variantGroup");

                if (lNodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    //throw new InitialDataIncompleteException("Initial data did not contain Variantgroup information! Variant groups not loaded.");
                    OutputHandler.PrintMessageToConsole("Initial data did not contain Variantgroup information! Variant groups not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in lNodeList)
                    {
                        List<Variant> lVariantGroupVariants = new List<Variant>();

                        XmlNodeList lVariantGroupVariantNamesNodeList = lNode["variantRefs"].ChildNodes;
                        if (lVariantGroupVariantNamesNodeList.Count > 0)
                        {
                            var lVariantGroupName = GetXMLNodeAttributeInnerText(lNode, "variantGroupName");
                            var lVariantGroupCardinality = GetXMLNodeAttributeInnerText(lNode, "groupCardinality");

                            foreach (XmlNode lVariantGroupVariantName in lVariantGroupVariantNamesNodeList)
                            {
                                string lVariantName = lVariantGroupVariantName.InnerText;
                                var lVariant = VariantLookupByName(lVariantName);

                                lVariantGroupVariants.Add(lVariant);
                            }

                            var lVariantGroup = new VariantGroup(lVariantGroupName
                                                    , lVariantGroupCardinality
                                                    , lVariantGroupVariants);

                            foreach (var lVariant in lVariantGroupVariants)
                                lVariant.MyVariantGroup = lVariantGroup;

                            lDataLoaded = true;
                        }
                        else
                        {
                            lDataLoaded = false;
                            OutputHandler.PrintMessageToConsole("Variant group defined without any variants! Data not loaded.");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in createVariantGroupInstances");                
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool CreateVariantInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList lNodeList = pXDoc.DocumentElement.SelectNodes("//variant");

                if (lNodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    //throw new InitialDataIncompleteException("Initial data did not contain Variantinformation! Variants not loaded.");
                    OutputHandler.PrintMessageToConsole("Initial data did not contain Variantinformation! Variants not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in lNodeList)
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
                        CreateVariantInstanceNLocalSets(GetXMLNodeAttributeInnerText(lNode, "variantName"));
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in createVariantInstances");                
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool CreatePartInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList lNodeList = pXDoc.DocumentElement.SelectNodes("//part");

                if (lNodeList.Count.Equals(0))
                {
                    UsePartInfo = false;
                    lDataLoaded = false;
                    //throw new InitialDataIncompleteException("Initial data did not contain Part information! Parts not loaded.");
                    OutputHandler.PrintMessageToConsole("Initial data did not contain Variantinformation! Parts not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in lNodeList)
                    {
                        CreatePartInstanceNLocalSets(GetXMLNodeAttributeInnerText(lNode, "partName"));
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in createPartInstances");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        public bool CreateItemUsageRulesInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList lNodeList = pXDoc.DocumentElement.SelectNodes("//itemusagerule");

                if (lNodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    //throw new InitialDataIncompleteException("Initial data did not contain item usage rule information! Item usage rules not loaded.");
                    OutputHandler.PrintMessageToConsole("Initial data did not contain Variantinformation! Item usage rules not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in lNodeList)
                    {
                        Part lTempPart = PartLookupByName(GetXMLNodeAttributeInnerText(lNode, "partRef"));

                        string lTempVariantExp = GetXMLNodeAttributeInnerText(lNode, "variantRef");

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

                        //    VariantlTempVariant = variantLookupByName(getXMLNodeAttributeInnerText(lNode, "variantRef"));
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
                OutputHandler.PrintMessageToConsole("error in createItemUsageRulesInstances");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        //This function is no longer needed as the relation between Part and operations is defined in the trigger field of the operation
        /*public bool createPartOperationsInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//partoperation");

                if (nodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    throw new InitialDataIncompleteException("Initial data did not contain Part - operation information! Data not loaded.");
                    ////cOutputHandler.printMessageToConsole("Initial data did not contain Variantinformation! Data not loaded.");

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

        //This function is no longer needed as the relation between Part and operations is defined in the trigger field of the operation
        /*public bool createVariantOperationsInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//variantOperationMapping");

                if (nodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    throw new InitialDataIncompleteException("Initial data did not contain Variant- operation information! Data not loaded.");
                    ////cOutputHandler.printMessageToConsole("Initial data did not contain Variantinformation! Data not loaded.");

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
                OutputHandler.PrintMessageToConsole("error in ReturnOperationStatusFromOperationInstanceName");
                OutputHandler.PrintMessageToConsole(ex.Message);
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
                    lResultOperation = OperationLookupByName(lOperationInstanceParts[0]);
            }
            catch (Exception ex)
            {
                OutputHandler.PrintMessageToConsole("error in ReturnOperationFromOperationInstanceName");
                OutputHandler.PrintMessageToConsole(ex.Message);
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
                OutputHandler.PrintMessageToConsole("error in ReturnOperationNameFromOperationInstanceName");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultOperationName;
        }

        //TODO: rename this function to show that you are loading from input
        public bool CreateOperationInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList lNodeList = pXDoc.DocumentElement.SelectNodes("//operation");

                if (lNodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    //throw new InitialDataIncompleteException("Initial data did not contain operation infor! Operations not loaded.");
                    OutputHandler.PrintMessageToConsole("Initial data did not contain operation infor! Operations not loaded.");
                }
                else
                {
                    foreach (XmlNode lNode in lNodeList)
                    {
                        string lTriggers = "";
                        string lOperationPrecondition = "";
                        string lOperationRequirement = "";
                        string lOperationPostcondition = "";
                        string lOperationResource = "";

                        if (lNode["trigger"] != null)
                        {
                            XmlNodeList lOpTriggersList = lNode["trigger"].ChildNodes;
                            lTriggers = lOpTriggersList[0].InnerText;
                        }

                        if (lNode["requirement"] != null)
                        {
                            XmlNodeList lOpRequirementsList = lNode["requirement"].ChildNodes;
                            lOperationRequirement = lOpRequirementsList[0].InnerText;
                        }

                        if (lNode["preconditions"] != null)
                        {
                            XmlNodeList lOpPreconditionNodeList = lNode["preconditions"].ChildNodes;
                            lOperationPrecondition=lOpPreconditionNodeList[0].InnerText;
                        }

                        if (lNode["postconditions"] != null)
                        {
                            XmlNodeList lOpPostconditionNodeList = lNode["postconditions"].ChildNodes;
                            lOperationPostcondition = lOpPostconditionNodeList[0].InnerText;
                        }

                        if (lNode["resourceRef"] != null)
                        {
                            XmlNodeList lOpResourceList = lNode["resourceRef"].ChildNodes;
                            lOperationResource = lOpResourceList[0].InnerText;
                        }

                        var lOperationName = GetXMLNodeAttributeInnerText(lNode, "operationName");
                        
                        //var lOperation  = CreateOperationInstance(lOperationName
                        //                                        , lTriggers
                        //                                        , lOperationRequirement
                        //                                        , lOperationPrecondition
                        //                                        , lOperationPostcondition
                        //                                        , lOperationResource);
                        var lOperation = new Operation(lOperationName
                                                    , lTriggers
                                                    , lOperationRequirement
                                                    , lOperationPrecondition
                                                    , lOperationPostcondition
                                                    , lOperationResource);

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
                OutputHandler.PrintMessageToConsole("error in createOperationInstances");
                OutputHandler.PrintMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        //public Action FindActionWithName(string pActionName)
        //{
        //    Action lResultAction = null;
        //    try
        //    {
        //        foreach (OperationInstance lOperationInstance in OperationInstanceSet)
        //        {
        //            if (lOperationInstance.Action_I2E.Name.Equals(pActionName))
        //                lResultAction = lOperationInstance.Action_I2E;
        //            else if (lOperationInstance.Action_E2F.Name.Equals(pActionName))
        //                lResultAction = lOperationInstance.Action_E2F;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cOutputHandler.printMessageToConsole("error in FindActionWithName");
        //        cOutputHandler.printMessageToConsole(ex.Message);
        //    }
        //    return lResultAction;
        //}
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
