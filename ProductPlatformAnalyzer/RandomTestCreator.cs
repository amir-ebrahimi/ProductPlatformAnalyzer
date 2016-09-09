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
        public List<int> lOverallChosenVariantCodes { get; set; }        

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

            myRandom = new Random();
            lFrameworkWrapper = new FrameworkWrapper();

            variantGroupList = new List<int>();
            lOverallChosenVariantCodes = new List<int>();
        }

        public void createRandomData(int pMaxVariantGroupNumber
                                    , int pMaxVariantNumber
                                    , int pMaxOperationNumber
                                    , int pTrueProbability
                                    , int pFalseProbability
                                    , int pExpressionProbability)
        {
            if (pMaxVariantGroupNumber != 0)
                maxVariantGroupNumber = pMaxVariantGroupNumber;
            if (pMaxVariantNumber != 0)
                maxVariantNumber = pMaxVariantNumber;
            if (pMaxOperationNumber != 0)
                maxOperationNumber = pMaxOperationNumber;
            if (pTrueProbability != 0)
                trueProbability = pTrueProbability;
            if (pFalseProbability != 0)
                falseProbability = pFalseProbability;
            if (pExpressionProbability != 0)
                expressionProbability = pExpressionProbability;

            createOperations();
            createVariants();
            createVariantGroups();
            lFrameworkWrapper.PrintDataSummary();
        }

        public void createVariantGroups()
        {
            List<string> lVariantList = new List<string>();

            for (int i = 0; i < maxVariantGroupNumber; i++)
                lFrameworkWrapper.CreateVariantGroupInstance("VG-" + i
                                                                , pickRandomGroupCardinality()
                                                                , pickASeriesOfRandomVariants());
        }

        private List<string> pickASeriesOfRandomVariants()
        {
            //This function returns a series of randomly piched variants which will be bound to a specific variantgroup
            int lNoOfVariants = myRandom.Next(1, maxVariantNumber);
            List<int> lChosenVariantCodes = new List<int>();
            List<string> lChosenVariants = new List<string>();
            int lChosenVariantCode;

            for (int i = 0; i < lNoOfVariants; i++)
            {
                lChosenVariantCode = myRandom.Next(1, maxVariantNumber);

                if (!lOverallChosenVariantCodes.Contains(lChosenVariantCode))
                {
                    lOverallChosenVariantCodes.Add(lChosenVariantCode);
                    lChosenVariantCodes.Add(lChosenVariantCode);
                }
            }

            foreach (int lVariantCode in lChosenVariantCodes)
                lChosenVariants.Add("V-" + lVariantCode);

            return lChosenVariants;
        }

        private string pickRandomGroupCardinality()
        {
            string lResultGroupCardinality = "";
            int lGroupcardinality = myRandom.Next(1, 2);

            if (lGroupcardinality == 1)
                lResultGroupCardinality = "choose exactly one";
            else
                lResultGroupCardinality = "choose at least one";

            return lResultGroupCardinality;
        }

        public void createVariants()
        {
            for (int i = 0; i < maxVariantNumber; i++)
            {
/*                lFrameworkWrapper.CreateVariantInstance("V-" + i
                                                        , i
                                                        , "V-" + i
                                                        , pickASeriesOfRandomOperations());*/
                lFrameworkWrapper.CreateVariantInstance("V-" + i
                                                        , i
                                                        , "V-" + i);
            }
        }

        private List<string> pickASeriesOfRandomOperations()
        {
            List<string> lRandomOperations = new List<string>();

            int lNoOfOperations = myRandom.Next(1, maxOperationNumber - 1);

            for (int i = 0; i < lNoOfOperations; i++)
            {
                lRandomOperations.Add("O-" + myRandom.Next(maxOperationNumber - 1));
            }

            return lRandomOperations;
        }

        public void createOperations()
        {
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
    }
}
