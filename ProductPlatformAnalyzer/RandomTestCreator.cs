using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    class RandomTestCreator
    {
        public int cMaxVariantGroupNumber { get; set; }
        public List<int> cVariantGroupList { get; set; }
        public int cMaxVariantNumber { get; set; }
        public List<int> cVariantList { get; set; }
        public int cMaxPartNumber { get; set; }
        public int cMaxOperationNumber { get; set; }
        public List<int> cOperationList { get; set; }
        public int cTrueProbability { get; set; }
        public int cFalseProbability { get; set; }
        public int cExpressionProbability { get; set; }
        public int cMaxTraitNumber { get; set; }
        public int cMaxNoOfTraitAttributes { get; set; }
        public int cMaxResourceNumber { get; set; }
        public Dictionary<int, operation> cOperationCodeLookup { get; set; }
        public Dictionary<int, part> cPartCodeLookup { get; set; }
        public Dictionary<int, variant> cVariantCodeLookup { get; set; }
        public List<int> cOverallChosenVariantCodes { get; set; }
        public List<int> cOverallFreeVariantCodes { get; set; }
        public List<int> cOverallChosenOperationCodes { get; set; }
        public List<int> cOverallFreeOperationCodes { get; set; }

        private Random cMyRandom;
        private FrameworkWrapper cFrameworkWrapper;
        public RandomTestCreator()
        {
            cMaxVariantGroupNumber = 1;
            cMaxVariantNumber = 1;
            cMaxPartNumber = 0;
            cMaxOperationNumber = 1;
            cTrueProbability = 100;
            cFalseProbability = 0;
            cExpressionProbability = 0;
            cMaxTraitNumber = 0;
            cMaxResourceNumber = 0;
            cMaxNoOfTraitAttributes = 0;

            cMyRandom = new Random();

            cOperationCodeLookup = new Dictionary<int, operation>();
            cPartCodeLookup = new Dictionary<int, part>();
            cVariantCodeLookup = new Dictionary<int, variant>();

            cVariantGroupList = new List<int>();
            cOverallChosenVariantCodes = new List<int>();
            cOverallFreeVariantCodes = new List<int>();
            cOverallChosenOperationCodes = new List<int>();
            cOverallFreeOperationCodes = new List<int>();
        }

        public operation operationLookupByCode(int pOperationCode)
        {
            operation lResultOperation = null;
            try
            {
                if (cOperationCodeLookup.ContainsKey(pOperationCode))
                    lResultOperation = cOperationCodeLookup[pOperationCode];
                else
                    Console.WriteLine("Operation " + pOperationCode + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in operationLookupByCode");
                Console.WriteLine(ex.Message);
            }
            return lResultOperation;
        }

        public part partLookupByCode(int pPartCode)
        {
            part lResultPart = null;
            try
            {
                if (cPartCodeLookup.ContainsKey(pPartCode))
                    lResultPart = cPartCodeLookup[pPartCode];
                else
                    Console.WriteLine("Part " + pPartCode + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in partLookupByCode");
                Console.WriteLine(ex.Message);
            }
            return lResultPart;
        }

        public variant variantLookupByCode(int pVariantCode)
        {
            variant lResultVariant = null;
            try
            {
                if (cVariantCodeLookup.ContainsKey(pVariantCode))
                    lResultVariant = cVariantCodeLookup[pVariantCode];
                else
                    Console.WriteLine("Variant " + pVariantCode + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in variantLookupByCode");
                Console.WriteLine(ex.Message);
            }
            return lResultVariant;
        }

        public bool createRandomData(int pMaxVariantGroupNumber
                                    , int pMaxVariantNumber
                                    , int pMaxPartNumber
                                    , int pMaxOperationNumber
                                    , int pTrueProbability
                                    , int pFalseProbability
                                    , int pExpressionProbability
                                    , int pMaxTraitNumber
                                    , int pMaxNoOfTraitAttributes
                                    , int pMaxResourceNumber
                                    , FrameworkWrapper pFrameworkWrapper)
        {
            bool lResult = false;
            try
            {
                if (pMaxVariantGroupNumber != 0)
                    cMaxVariantGroupNumber = pMaxVariantGroupNumber;
                if (pMaxVariantNumber != 0)
                {
                    cMaxVariantNumber = pMaxVariantNumber;
                    resetOverallFreeVariantCodes();
                }
                if (pMaxPartNumber != 0)
                    cMaxPartNumber = pMaxPartNumber;

                if (pMaxOperationNumber != 0)
                {
                    cMaxOperationNumber = pMaxOperationNumber;
                    resetOverallFreeOperationCodes();
                }
                if (pTrueProbability != 0)
                    cTrueProbability = pTrueProbability;
                if (pFalseProbability != 0)
                    cFalseProbability = pFalseProbability;
                if (pExpressionProbability != 0)
                    cExpressionProbability = pExpressionProbability;
                if (pMaxTraitNumber != 0)
                    cMaxTraitNumber = pMaxTraitNumber;
                if (pMaxNoOfTraitAttributes != 0)
                    cMaxNoOfTraitAttributes = pMaxNoOfTraitAttributes;
                if (pMaxResourceNumber != 0)
                    cMaxResourceNumber = pMaxResourceNumber;
                if (pFrameworkWrapper != null)
                    cFrameworkWrapper = pFrameworkWrapper;


                createVariants();
                createVariantGroups();
                createParts();

                createOperations();
                updateOperationPrePostConditions();

                createVariantOperationMapping();
                createPartOperationMapping();
                createItemUsageRules();

                //createTraits();
                //createResources();
                cFrameworkWrapper.PrintDataSummary();

                lResult = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createRandomData");
                Console.WriteLine(ex.Message);
                
            }
            return lResult;
        }

        private void createTraits()
        {
            try
            {
                HashSet<trait> lInheritedTraits = new HashSet<trait>();
                HashSet<Tuple<string,string>> lTraitAttributes = new HashSet<Tuple<string,string>>();
                for (int i = 0; i < cMaxTraitNumber; i++)
                {
                    string lTraitName = "T" + i;
                    lTraitAttributes = makeRandomSetOfTraitAttributes(lTraitName);
                    cFrameworkWrapper.createTraitInstance(lTraitName
                                                            , lInheritedTraits
                                                            , lTraitAttributes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createTraits");
                Console.WriteLine(ex.Message);
            }
        }

        private HashSet<Tuple<string, string>> makeRandomSetOfTraitAttributes(string pTraitName)
        {
            HashSet<Tuple<string, string>> lTraitAttributes = new HashSet<Tuple<string, string>>();
            try
            {
                int lNoOfAttributes = cMyRandom.Next(1, cMaxNoOfTraitAttributes);

                for (int i = 0; i < lNoOfAttributes; i++)
                {
                    Tuple<string, string> lAttributeTuple = new Tuple<string, string>("int", pTraitName + "Att" + i);
                    lTraitAttributes.Add(lAttributeTuple);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in makeRandomSetOfTraitAttributes");
                Console.WriteLine(ex.Message);
            }
            return lTraitAttributes;
        }

        private void createResources()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createResources");
                Console.WriteLine(ex.Message);
            }
        }

        private void resetOverallFreeVariantCodes()
        {
            try
            {
                for (int i = 0; i < cMaxVariantNumber; i++)
                    cOverallFreeVariantCodes.Add(i);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in resetOverallFreeVariantCodes");
                Console.WriteLine(ex.Message);
            }
        }

        private void resetOverallFreeOperationCodes()
        {
            try
            {
                cOverallFreeOperationCodes.RemoveRange(0,cOverallFreeOperationCodes.Count);

                for (int i = 0; i < cMaxOperationNumber; i++)
                    cOverallFreeOperationCodes.Add(i);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in resetOverallFreeOperationCodes");
                Console.WriteLine(ex.Message);
            }
        }

        private void createVariantOperationMapping()
        {
            try
            {
                foreach (variant lVariant in cFrameworkWrapper.VariantSet)
                {
                    resetOverallFreeOperationCodes();
                    HashSet<operation> lRandomOperations = pickASeriesOfRandomOperations(true);
                    cFrameworkWrapper.CreateVariantOperationMappingInstance(lVariant.names, lRandomOperations);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createPartOperationMapping");
                Console.WriteLine(ex.Message);
            }
        }

        private void createPartOperationMapping()
        {
            try
            {
                foreach (part lPart in cFrameworkWrapper.PartSet)
                {
                    resetOverallFreeOperationCodes();
                    HashSet<operation> lRandomOperations = pickASeriesOfRandomOperations(true);
                    cFrameworkWrapper.CreatePartOperationMappingInstance(lPart.names, lRandomOperations);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createPartOperationMapping");
                Console.WriteLine(ex.Message);
            }
        }

        private void createItemUsageRules()
        {
            try
            {
                if (cMaxPartNumber > 0)
                {
                    foreach (variant lVariant in cFrameworkWrapper.VariantSet)
                    {
                        resetOverallFreeOperationCodes();
                        HashSet<part> lRandomParts = pickASeriesOfRandomParts(true);
                        cFrameworkWrapper.CreateItemUsageRuleInstance(lVariant, lRandomParts);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createItemUsageRules");
                Console.WriteLine(ex.Message);
            }
        }

        public void createVariantGroups()
        {
            try
            {
                for (int i = 0; i < cMaxVariantGroupNumber; i++)
                    cFrameworkWrapper.CreateVariantGroupInstance("VG-" + i
                                                                    , pickRandomGroupCardinality()
                                                                    , pickASeriesOfRandomVariants());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariantGroups");
                Console.WriteLine(ex.Message);
            }
        }

        private List<int> makeItemListToBeChosenFrom(int pMaxItemNumber, List<int> pChosenItems)
        {
            List<int> lResultList = null;
            List<int> lItemIndexToBeChosen = new List<int>();
            try
            {

                for (int i = 0; i < pMaxItemNumber; i++)
                {
                    if (!pChosenItems.Contains(i))
                        lItemIndexToBeChosen.Add(i);
                }
                lResultList = lItemIndexToBeChosen;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in makeItemListToBeChosenFrom");
                Console.WriteLine(ex.Message);
            }
            return lResultList;
        }

        private HashSet<string> pickASeriesOfRandomOperationNames(bool pRepeatOperationsAllowed)
        {
            HashSet<string> lChosenOperations = new HashSet<string>();
            try
            {
                if (!cOverallFreeOperationCodes.Count.Equals(0))
                {
                    int lNoOfOperations = cMyRandom.Next(1, cOverallFreeOperationCodes.Count);

                    HashSet<int> lChosenOperationCodes = new HashSet<int>();
                    int lRandomOperationCode = 0;
                    for (int i = 0; i < lNoOfOperations; i++)
                    {
                        lRandomOperationCode = cMyRandom.Next(1, cMaxOperationNumber);
                        lChosenOperations.Add(operationLookupByCode(lRandomOperationCode).names);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in pickASeriesOfRandomOperationNames");
                Console.WriteLine(ex.Message);
            }
            return lChosenOperations;
        }

        private HashSet<operation> pickASeriesOfRandomOperations(bool pRepeatOperationsAllowed)
        {
            HashSet<operation> lChosenOperations = new HashSet<operation>();
            try
            {
                if (!cOverallFreeOperationCodes.Count.Equals(0))
                {
                    int lNoOfOperations = cMyRandom.Next(1, cMaxOperationNumber);

                    HashSet<int> lChosenOperationCodes = new HashSet<int>();
                    int lRandomOperationCode = 0;
                    for (int i = 0; i < lNoOfOperations; i++)
                    {
                        lRandomOperationCode = cMyRandom.Next(1, cOperationCodeLookup.Count);
                        lChosenOperations.Add(operationLookupByCode(lRandomOperationCode));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in pickASeriesOfRandomOperations");
                Console.WriteLine(ex.Message);
            }
            return lChosenOperations;
        }

        private HashSet<variant> pickASeriesOfRandomVariants(bool pRepeatVariantsAllowed)
        {
            HashSet<variant> lChosenVariants = new HashSet<variant>();
            try
            {
                int lNoOfVariants = cMyRandom.Next(1, cMaxVariantNumber);

                int lRandomVariantCode = 0;
                for (int i = 0; i < lNoOfVariants; i++)
                {
                    lRandomVariantCode = cMyRandom.Next(1, cVariantCodeLookup.Count);
                    lChosenVariants.Add(variantLookupByCode(lRandomVariantCode));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in pickASeriesOfRandomVariants");
                Console.WriteLine(ex.Message);
            }
            return lChosenVariants;
        }

        private HashSet<part> pickASeriesOfRandomParts(bool pRepeatPartsAllowed)
        {
            HashSet<part> lChosenParts = new HashSet<part>();
            try
            {
                int lNoOfParts = cMyRandom.Next(1, cMaxPartNumber);

                int lRandomPartCode = 0;
                for (int i = 0; i < lNoOfParts; i++)
                {
                    lRandomPartCode = cMyRandom.Next(1, cPartCodeLookup.Count);
                    lChosenParts.Add(partLookupByCode(lRandomPartCode));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in pickASeriesOfRandomVariants");
                Console.WriteLine(ex.Message);
            }
            return lChosenParts;
        }

        /*private HashSet<int> pickASeriesOfRandomOperationCodes(bool pRepeatOperationsAllowed)
        {
            HashSet<int> lChosenOperationCodes = new HashSet<int>();
            try
            {
                if (!cOverallFreeOperationCodes.Count.Equals(0))
                {
                    int lNoOfOperations = cMyRandom.Next(1, cOverallFreeOperationCodes.Count);

                    for (int i = 0; i < lNoOfOperations; i++)
                        lChosenOperationCodes.Add(pickRandomOperationCode(pRepeatOperationsAllowed));

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in pickASeriesOfRandomOperationCodes");
                Console.WriteLine(ex.Message);
            }
            return lChosenOperationCodes;
        }*/

        /*private HashSet<operation> pickASeriesOfRandomOperations(bool pRepeatOperationsAllowed)
        {
            HashSet<operation> lChosenOperations = new HashSet<operation>();
            try
            {
                HashSet<string> lChosenOperationNames = new HashSet<string>();
                lChosenOperationNames = pickASeriesOfRandomOperationNames(pRepeatOperationsAllowed);

                foreach (var lOperationName in lChosenOperationNames)
                {
                    lChosenOperations.Add(cFrameworkWrapper.operationLookupByName(lOperationName));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in pickASeriesOfRandomOperations");
                Console.WriteLine(ex.Message);
            }
            return lChosenOperations;
        }*/

        /*private int pickRandomOperationCode(bool pRepeatOperationsAllowed)
        {
            int lResultOperationCode = 0;
            try
            {
                int lOperationIndex = cMyRandom.Next(0, cOverallFreeOperationCodes.Count);
                int lOperationCode = cOverallFreeOperationCodes[lOperationIndex];

                cOverallChosenOperationCodes.Add(lOperationCode);
                if (!pRepeatOperationsAllowed)
                    cOverallFreeOperationCodes.Remove(lOperationCode);

                lResultOperationCode = lOperationCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in pickRandomOperationCode");
                Console.WriteLine(ex.Message);
            }
            return lResultOperationCode;
        }*/

        private HashSet<string> returnOperationNames(HashSet<int> pOperationCodes)
        {
            HashSet<string> lResultList = new HashSet<string>();
            try
            {
                foreach (int lOperationCode in pOperationCodes)
                {
                    lResultList.Add("O-" + lOperationCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in returnOperationNames");
                Console.WriteLine(ex.Message);
            }
            return lResultList;
        }

        private HashSet<variant> pickASeriesOfRandomVariants()
        {
            HashSet<variant> lChosenVariants = new HashSet<variant>();
            try
            {
                //This function returns a series of randomly piched variants which will be bound to a specific variantgroup
                int lNoOfVariants = cMyRandom.Next(1, cOverallFreeVariantCodes.Count);
                HashSet<int> lChosenVariantCodes = new HashSet<int>();
                int lChosenVariantCode;

                for (int i = 0; i < lNoOfVariants; i++)
                {

                    lChosenVariantCode = returnRandomVariantCode();

                    //We don't want a group to have zero variants
                    if (!cOverallChosenVariantCodes.Contains(lChosenVariantCode))
                    {
                        updateChosenVariantCode(lChosenVariantCode);
                        lChosenVariantCodes.Add(lChosenVariantCode);
                    }
                }

                HashSet<string> lVariantNames = returnVariantNames(lChosenVariantCodes);

                foreach (string lVariantName in lVariantNames)
	            {
                    lChosenVariants.Add(cFrameworkWrapper.variantLookupByName(lVariantName));		 
	            }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in pickASeriesOfRandomVariants");
                Console.WriteLine(ex.Message);
            }
            return lChosenVariants;
        }

        private HashSet<string> returnVariantNames(HashSet<int> pVariantCodes)
        {
            HashSet<string> lResultList = new HashSet<string>();
            try
            {
                foreach (int lVariantCode in pVariantCodes)
                {
                    lResultList.Add("V-" + lVariantCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in returnVariantNames");
                Console.WriteLine(ex.Message);
            }
            return lResultList;
        }

        private int returnRandomVariantCode()
        {
            int lResultVariantCode = 0;
            try
            {
                int lRandomVariantIndex = cMyRandom.Next(0, cOverallFreeVariantCodes.Count);
                lResultVariantCode = cOverallFreeVariantCodes[lRandomVariantIndex];
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in returnRandomVariantCode");
                Console.WriteLine(ex.Message);
            }
            return lResultVariantCode;
        }

        private void updateChosenVariantCode(int pChosenVariantCode)
        {
            try
            {
                cOverallChosenVariantCodes.Add(pChosenVariantCode);
                cOverallFreeVariantCodes.Remove(pChosenVariantCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in updateChosenVariantCode");
                Console.WriteLine(ex.Message);
            }
        }

        private string pickRandomGroupCardinality()
        {
            string lResultGroupCardinality = "";
            try
            {
                int lGroupcardinality = cMyRandom.Next(1, 2);

                if (lGroupcardinality == 1)
                    lResultGroupCardinality = "choose exactly one";
                else
                    lResultGroupCardinality = "choose at least one";
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in pickRandomGroupCardinality");
                Console.WriteLine(ex.Message);
            }
            return lResultGroupCardinality;
        }

        public void createVariants()
        {
            try
            {
                variant lTempVariant = null;
                for (int i = 0; i < cMaxVariantNumber; i++)
                {
                    lTempVariant = cFrameworkWrapper.CreateVariantInstance("V-" + i);
                    cVariantCodeLookup.Add(i, lTempVariant);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariants");
                Console.WriteLine(ex.Message);
            }
        }

        public void createParts()
        {
            try
            {
                part lTempPart = null;
                for (int i = 0; i < cMaxPartNumber; i++)
                {
                    lTempPart = cFrameworkWrapper.CreatePartInstance("P-" + i);
                    cPartCodeLookup.Add(i, lTempPart);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariants");
                Console.WriteLine(ex.Message);
            }
        }

        public void createOperations()
        {
            try
            {
                //TODO: we have to also make two random list fir preconditions and postconditions
                HashSet<string> lOperationPrecondition = new HashSet<string>();
                HashSet<string> lOperationPostcondition = new HashSet<string>();
                HashSet<string> lOperationRequiremnt = new HashSet<string>();

                operation lTempOperation = null;
                for (int i = 0; i < cMaxOperationNumber; i++)
                {
                    
                    lTempOperation = cFrameworkWrapper.CreateOperationInstance("O-" + i
                                                                            , lOperationRequiremnt
                                                                            , lOperationPrecondition
                                                                            , lOperationPostcondition);
                    //Creating a list for operation lookup by code, ONLY for use with in this class
                    cOperationCodeLookup.Add(i, lTempOperation);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createOperations");
                Console.WriteLine(ex.Message);
            }
        }

        private int returnOperationCode(string pOperationName)
        {
            int lOperationCode = 0;
            try
            {
                string[] lOperationNameParts = pOperationName.Split('-');
                lOperationCode = int.Parse(lOperationNameParts[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in returnOperationCode");
                Console.WriteLine(ex.Message);
            }
            return lOperationCode;
        }

        private void updateOperationPrePostConditions()
        {
            try
            {
                foreach (operation lCurrentOperation in cFrameworkWrapper.OperationSet)
                {
                    resetOverallFreeOperationCodes();

                    //This is because an operation can't be part of its own pre or post condition
                    cOverallFreeOperationCodes.Remove(returnOperationCode(lCurrentOperation.names));

                    lCurrentOperation.precondition = pickASeriesOfRandomOperationNames(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in updateOperationPrePostConditions");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
