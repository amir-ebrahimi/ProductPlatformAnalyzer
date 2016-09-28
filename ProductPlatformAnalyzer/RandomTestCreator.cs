using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    class RandomTestCreator
    {
        public int maxVariantGroupNumber { get; set; }
        public List<int> variantGroupList { get; set; }
        public int maxVariantNumber { get; set; }
        public List<int> variantList { get; set; }
        public int maxOperationNumber { get; set; }
        public List<int> operationList { get; set; }
        public int trueProbability { get; set; }
        public int falseProbability { get; set; }
        public int expressionProbability { get; set; }
        public int maxTraitNumber { get; set; }
        public int maxNoOfTraitAttributes { get; set; }
        public int maxResourceNumber { get; set; }
        public List<int> lOverallChosenVariantCodes { get; set; }
        public List<int> lOverallFreeVariantCodes { get; set; }
        public List<int> lOverallChosenOperationCodes { get; set; }
        public List<int> lOverallFreeOperationCodes { get; set; }

        private Random myRandom;
        private FrameworkWrapper lFrameworkWrapper;
        public RandomTestCreator()
        {
            maxVariantGroupNumber = 1;
            maxVariantNumber = 1;
            maxOperationNumber = 1;
            trueProbability = 100;
            falseProbability = 0;
            expressionProbability = 0;
            maxTraitNumber = 0;
            maxResourceNumber = 0;
            maxNoOfTraitAttributes = 0;

            myRandom = new Random();
            lFrameworkWrapper = new FrameworkWrapper();

            variantGroupList = new List<int>();
            lOverallChosenVariantCodes = new List<int>();
            lOverallFreeVariantCodes = new List<int>();
            lOverallChosenOperationCodes = new List<int>();
            lOverallFreeOperationCodes = new List<int>();
        }

        public void createRandomData(int pMaxVariantGroupNumber
                                    , int pMaxVariantNumber
                                    , int pMaxOperationNumber
                                    , int pTrueProbability
                                    , int pFalseProbability
                                    , int pExpressionProbability
                                    , int pMaxTraitNumber
                                    , int pMaxNoOfTraitAttributes
                                    , int pMaxResourceNumber)
        {
            if (pMaxVariantGroupNumber != 0)
                maxVariantGroupNumber = pMaxVariantGroupNumber;
            if (pMaxVariantNumber != 0)
            {
                maxVariantNumber = pMaxVariantNumber;
                resetOverallFreeVariantCodes();
            }
            if (pMaxOperationNumber != 0)
            {
                maxOperationNumber = pMaxOperationNumber;
                resetOverallFreeOperationCodes();
            }
            if (pTrueProbability != 0)
                trueProbability = pTrueProbability;
            if (pFalseProbability != 0)
                falseProbability = pFalseProbability;
            if (pExpressionProbability != 0)
                expressionProbability = pExpressionProbability;
            if (pMaxTraitNumber != 0)
                maxTraitNumber = pMaxTraitNumber;
            if (pMaxNoOfTraitAttributes != 0)
                maxNoOfTraitAttributes = pMaxNoOfTraitAttributes;
            if (pMaxResourceNumber != 0)
                maxResourceNumber = pMaxResourceNumber;

            createVariants();
            createVariantGroups();

            createOperations();
            updateOperationPrePostConditions();

            createVariantOperationMapping();

            //createTraits();
            //createResources();
            lFrameworkWrapper.PrintDataSummary();
        }

        private void createTraits()
        {
            try
            {
                List<trait> lInheritedTraits = new List<trait>();
                List<Tuple<string,string>> lTraitAttributes = new List<Tuple<string,string>>();
                for (int i = 0; i < maxTraitNumber; i++)
                {
                    string lTraitName = "T" + i;
                    lTraitAttributes = makeRandomSetOfTraitAttributes(lTraitName);
                    lFrameworkWrapper.createTraitInstance(lTraitName
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

        private List<Tuple<string, string>> makeRandomSetOfTraitAttributes(string pTraitName)
        {
            List<Tuple<string, string>> lTraitAttributes = new List<Tuple<string, string>>();
            try
            {
                int lNoOfAttributes = myRandom.Next(1, maxNoOfTraitAttributes);

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
                for (int i = 0; i < maxVariantNumber; i++)
                    lOverallFreeVariantCodes.Add(i);
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
                lOverallFreeOperationCodes.RemoveRange(0,lOverallFreeOperationCodes.Count);

                for (int i = 0; i < maxOperationNumber; i++)
                    lOverallFreeOperationCodes.Add(i);
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
                foreach (variant lVariant in lFrameworkWrapper.VariantList)
                {
                    resetOverallFreeOperationCodes();
                    List<string> lRandomOperationCodes = pickASeriesOfRandomOperations(true);
                    lFrameworkWrapper.CreateVariantOperationMappingInstance(lVariant.names, lRandomOperationCodes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariantOperationMapping");
                Console.WriteLine(ex.Message);
            }
        }

        public void createVariantGroups()
        {
            try
            {
                List<string> lVariantList = new List<string>();

                for (int i = 0; i < maxVariantGroupNumber; i++)
                    lFrameworkWrapper.CreateVariantGroupInstance("VG-" + i
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

        private List<string> pickASeriesOfRandomOperations(bool pRepeatOperationsAllowed)
        {
            List<string> lChosenOperations = new List<string>();
            try
            {
                if (!lOverallFreeOperationCodes.Count.Equals(0))
                {
                    int lNoOfOperations = myRandom.Next(1, lOverallFreeOperationCodes.Count);

                    List<int> lChosenOperationCodes = new List<int>();
                    for (int i = 0; i < lNoOfOperations; i++)
                        lChosenOperationCodes.Add(pickRandomOperationCode(pRepeatOperationsAllowed));

                    lChosenOperations = returnOperationNames(lChosenOperationCodes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in pickASeriesOfRandomOperations");
                Console.WriteLine(ex.Message);
            }
            return lChosenOperations;
        }

        private int pickRandomOperationCode(bool pRepeatOperationsAllowed)
        {
            int lResultOperationCode = 0;
            try
            {
                int lOperationIndex = myRandom.Next(0, lOverallFreeOperationCodes.Count);
                int lOperationCode = lOverallFreeOperationCodes[lOperationIndex];

                lOverallChosenOperationCodes.Add(lOperationCode);
                if (!pRepeatOperationsAllowed)
                    lOverallFreeOperationCodes.Remove(lOperationCode);

                lResultOperationCode = lOperationCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in pickRandomOperationCode");
                Console.WriteLine(ex.Message);
            }
            return lResultOperationCode;
        }

        private List<string> returnOperationNames(List<int> pOperationCodes)
        {
            List<string> lResultList = new List<string>();
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

        private List<string> pickASeriesOfRandomVariants()
        {
            List<string> lChosenVariants = new List<string>();
            try
            {
                //This function returns a series of randomly piched variants which will be bound to a specific variantgroup
                int lNoOfVariants = myRandom.Next(1, lOverallFreeVariantCodes.Count);
                List<int> lChosenVariantCodes = new List<int>();
                int lChosenVariantCode;

                for (int i = 0; i < lNoOfVariants; i++)
                {

                    lChosenVariantCode = returnRandomVariantCode();

                    //We don't want a group to have zero variants
                    if (!lOverallChosenVariantCodes.Contains(lChosenVariantCode))
                    {
                        updateChosenVariantCode(lChosenVariantCode);
                        lChosenVariantCodes.Add(lChosenVariantCode);
                    }
                }

                lChosenVariants = returnVariantNames(lChosenVariantCodes);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in pickASeriesOfRandomVariants");
                Console.WriteLine(ex.Message);
            }
            return lChosenVariants;
        }

        private List<string> returnVariantNames(List<int> pVariantCodes)
        {
            List<string> lResultList = new List<string>();
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
                int lRandomVariantIndex = myRandom.Next(0, lOverallFreeVariantCodes.Count);
                lResultVariantCode = lOverallFreeVariantCodes[lRandomVariantIndex];
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
                lOverallChosenVariantCodes.Add(pChosenVariantCode);
                lOverallFreeVariantCodes.Remove(pChosenVariantCode);
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
                int lGroupcardinality = myRandom.Next(1, 2);

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
                for (int i = 0; i < maxVariantNumber; i++)
                {
                    lFrameworkWrapper.CreateVariantInstance("V-" + i
                                                            , i
                                                            , "V-" + i);
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
                List<string> lOperationPrecondition = new List<string>();
                List<string> lOperationPostcondition = new List<string>();
                List<string> lOperationRequiremnt = new List<string>();

                for (int i = 0; i < maxOperationNumber; i++)
                {
                    lFrameworkWrapper.CreateOperationInstance("O-" + i
                                                            , "O-" + i
                                                            , lOperationRequiremnt
                                                            , lOperationPrecondition
                                                            , lOperationPostcondition);

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
                foreach (operation lCurrentOperation in lFrameworkWrapper.OperationList)
                {
                    resetOverallFreeOperationCodes();

                    //This is because an operation can't be part of its own pre or post condition
                    lOverallFreeOperationCodes.Remove(returnOperationCode(lCurrentOperation.names));

                    lCurrentOperation.precondition = pickASeriesOfRandomOperations(false);
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
