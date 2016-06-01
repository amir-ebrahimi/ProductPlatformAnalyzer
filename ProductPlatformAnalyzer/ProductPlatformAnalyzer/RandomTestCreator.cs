using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    class RandomTestCreator
    {
        private int maxVariantGroupNumber;
        private List<int> variantGroupList;
        private int maxVariantNumber;
        private List<int> variantList;
        private int maxOperationNumber;
        private List<int> operationList;
        private int trueProbability;
        private int falseProbability;
        private int expressionProbability;
        private List<int> lOverallChosenVariantCodes;

        public int getMaxVariantGroupNumber()
        {
            return maxVariantGroupNumber;
        }

        public void setMaxVariantGroupNumber(int value)
        {
            maxVariantGroupNumber = value;
        }

        public List<int> getVariantGroupList()
        {
            return variantGroupList;
        }

        public void setVariantGroupList(List<int> value)
        {
            variantGroupList = value;
        }

        public int getMaxVariantNumber()
        {
            return maxVariantNumber;
        }

        public void setMaxVariantNumber(int value)
        {
            maxVariantNumber = value;
        }

        public List<int> getVariantList()
        {
            return variantList;
        }

        public void setVariantList(List<int> value)
        {
            variantList = value;
        }

        public int getMaxOperationNumber()
        {
            return maxOperationNumber;
        }

        public void setMaxOperationNumber(int value)
        {
            maxOperationNumber = value;
        }

        public List<int> getOperationList()
        {
            return operationList;
        }

        public void setOperationNumber(List<int> value)
        {
            operationList = value;
        }

        public int getTrueProbability()
        {
            return trueProbability;
        }

        public void setTrueProbability(int value)
        {
            trueProbability = value;
        }

        public int getFalseProbability()
        {
            return falseProbability;
        }

        public void setFalseProbability(int value)
        {
            falseProbability = value;
        }

        public int getExpressionProbability()
        {
            return expressionProbability;
        }

        public void setExpressionProbability(int value)
        {
            expressionProbability = value;
        }

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

        public void createRandomData(int pMaxVariantGroupNumber, int pMaxVariantNumber, int pMaxOperationNumber, int pTrueProbability, int pFalseProbability, int pExpressionProbability)
        {
            if (pMaxVariantGroupNumber != null)
                setMaxVariantGroupNumber(pMaxVariantGroupNumber);
            if (pMaxVariantNumber != null)
                setMaxVariantNumber(pMaxVariantNumber);
            if (pMaxOperationNumber != null)
                setMaxOperationNumber(pMaxOperationNumber);
            if (pTrueProbability != null)
                setTrueProbability(pTrueProbability);
            if (pFalseProbability != null)
                setFalseProbability(pFalseProbability);
            if (pExpressionProbability != null)
                setExpressionProbability(pExpressionProbability);

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

            for (int i = 0; i < maxOperationNumber; i++)
            {

                lFrameworkWrapper.CreateOperationInstance("O-" + i
                                                        , "O-" + i
                                                        , lOperationPrecondition
                                                        , lOperationPostcondition);
                
            }
        }
    }
}
