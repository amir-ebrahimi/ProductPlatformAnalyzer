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
        public int cMaxVariantNumber { get; set; }
        public int cMaxPartNumber { get; set; }
        public int cMaxOperationNumber { get; set; }
        public int cMaxNoOfConfigurationRules { get; set; }
        public int cTrueProbability { get; set; }
        public int cFalseProbability { get; set; }
        public int cExpressionProbability { get; set; }
        public int cMaxTraitNumber { get; set; }
        public int cMaxNoOfTraitAttributes { get; set; }
        public int cMaxResourceNumber { get; set; }
        public int cMaxExpressionOperandNumber { get; set; }
        public Dictionary<int, Operation> cOperationCodeLookup { get; set; }
        public Dictionary<int, part> cPartCodeLookup { get; set; }
        public Dictionary<int, variant> cVariantCodeLookup { get; set; }
        public List<int> cOverallChosenVariantCodes { get; set; }
        public List<int> cOverallFreeVariantCodes { get; set; }
        public List<int> cOverallChosenOperationCodes { get; set; }
        public List<int> cOverallFreeOperationCodes { get; set; }
        public string[] cExpressionOperators = new string[] { "or" };

        private Random cMyRandom;
        private FrameworkWrapper cFrameworkWrapper;
        private OutputHandler cOutputHandler;
        private Z3SolverEngineer cZ3SolverEngineer;
        private Z3Solver cZ3Solver;

        public RandomTestCreator(OutputHandler pOutputHandler
                                , Z3SolverEngineer pZ3SolverEngineer
                                , FrameworkWrapper pFrameworkWrapper
                                , Z3Solver pZ3Solver)
        {
            cOutputHandler = pOutputHandler;
            cZ3SolverEngineer = pZ3SolverEngineer;
            cFrameworkWrapper = pFrameworkWrapper;
            cZ3Solver = pZ3Solver;
            

            cMaxVariantGroupNumber = 1;
            cMaxVariantNumber = 1;
            cMaxPartNumber = 0;
            cMaxOperationNumber = 1;
            cMaxNoOfConfigurationRules = 1;
            cTrueProbability = 100;
            cFalseProbability = 0;
            cExpressionProbability = 0;
            cMaxTraitNumber = 0;
            cMaxResourceNumber = 0;
            cMaxNoOfTraitAttributes = 0;
            cMaxExpressionOperandNumber = 3;

            cMyRandom = new Random();

            cOperationCodeLookup = new Dictionary<int, Operation>();
            cPartCodeLookup = new Dictionary<int, part>();
            cVariantCodeLookup = new Dictionary<int, variant>();

            cOverallChosenVariantCodes = new List<int>();
            cOverallFreeVariantCodes = new List<int>();
            cOverallChosenOperationCodes = new List<int>();
            cOverallFreeOperationCodes = new List<int>();
        }

        public Operation operationLookupByCode(int pOperationCode)
        {
            Operation lResultOperation = null;
            try
            {
                if (cOperationCodeLookup.ContainsKey(pOperationCode))
                    lResultOperation = cOperationCodeLookup[pOperationCode];
                else
                    cOutputHandler.printMessageToConsole("Operation " + pOperationCode + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in operationLookupByCode");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                    cOutputHandler.printMessageToConsole("Part " + pPartCode + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in partLookupByCode");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                    cOutputHandler.printMessageToConsole("Variant " + pVariantCode + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in variantLookupByCode");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                                    , int pMaxExpressionOperandNumber
                                    , int pMaxNoOfConfigurationRules)
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
                if (pMaxExpressionOperandNumber != 0)
                    cMaxExpressionOperandNumber = pMaxExpressionOperandNumber;
                if (pMaxNoOfConfigurationRules != 0)
                    cMaxNoOfConfigurationRules = pMaxNoOfConfigurationRules;
                /*if (pFrameworkWrapper != null)
                    cFrameworkWrapper = pFrameworkWrapper;*/


                createVariants();
                cZ3SolverEngineer.convertFVariants2Z3Variants();
                createVariantGroups();
                cZ3SolverEngineer.produceVariantGroupGCardinalityConstraints();

                createParts();
                cZ3SolverEngineer.convertFParts2Z3Parts();

                createItemUsageRules();
                cZ3SolverEngineer.convertItemUsageRulesConstraints();

                createConfigurationRules();
                //cZ3SolverEngineer.convertFConstraint2Z3Constraint();


                createOperations();
                updateOperationPrePostConditions();
                updateOperationTriggerConditions();
                //cZ3SolverEngineer.convertFOperations2Z3Operations(1);
                //cZ3SolverEngineer.convertOperationsPrecedenceRules(0);

                //createTraits();
                //createResources();
                Console.WriteLine("Random data summary: ");
                Console.WriteLine("---------------------------------------------------------------------------");
                cFrameworkWrapper.PrintDataSummary();

                lResult = true;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createRandomData");
                cOutputHandler.printMessageToConsole(ex.Message);
                
            }
            return lResult;
        }

        private void createConfigurationRules()
        {
            try
            {
                List<string> lSatisfiableConfigurationRules = new List<string>();

                //First build the maximum number of operands
                List<string> lOperands = new List<string>();
                foreach (var lPart in cFrameworkWrapper.PartSet)
                {
                    lOperands.Add(lPart.names);
                }
                foreach (var lVariant in cFrameworkWrapper.VariantSet)
                {
                    lOperands.Add(lVariant.names);
                }

                string lRandomConfigurationRule = "";

                cZ3Solver.SolverPushFunction();
                for (int i = 0; i < cMaxNoOfConfigurationRules; i++)
                {
                    lRandomConfigurationRule = buildRandomExpFromOperands(lOperands);
                    
                    Microsoft.Z3.BoolExpr lBoolExpr = cZ3SolverEngineer.convertComplexString2BoolExpr(lRandomConfigurationRule);
                    cZ3Solver.AddConstraintToSolver(lBoolExpr, "RandomConfigRule");
                    //We add a random constraint rule if it is satisfiable we keep it otherwise we remove it
                    
                    //This line checks if the constraints in the model have any conflicts with each other
                    int lTransitionNo = 0;
                    Microsoft.Z3.Status lAnalysisResult = cZ3SolverEngineer.AnalyzeModelConstraints(lTransitionNo);

                    if (lAnalysisResult.Equals(Microsoft.Z3.Status.SATISFIABLE))
                    {
                        lSatisfiableConfigurationRules.Add(lRandomConfigurationRule);
                    }
                    else
                    {
                        cZ3Solver.SolverPopFunction();
                        i--;
                        cZ3Solver.SolverPushFunction();
                    }


                    
                }
                cZ3Solver.SolverPopFunction();

                //Adding the satisfied set of configuration rules to the model
                if (lSatisfiableConfigurationRules.Count > 0)
                {
                    foreach (var lConfigRule in lSatisfiableConfigurationRules)
                    {
                        cFrameworkWrapper.ConstraintSet.Add(lRandomConfigurationRule);
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createConfigurationRules");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
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
                cOutputHandler.printMessageToConsole("error in createTraits");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in makeRandomSetOfTraitAttributes");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in createResources");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in resetOverallFreeVariantCodes");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in resetOverallFreeOperationCodes");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private void createItemUsageRules()
        {
            try
            {
                if (cMaxPartNumber > 0)
                {
                    List<string> lOperands = new List<string>();
                    foreach (variant lVariant in cFrameworkWrapper.VariantSet)
	                {
		                 lOperands.Add(lVariant.names);
	                }

                    foreach (part lPart in cFrameworkWrapper.PartSet)
                    {
                        string lTempVariantExpr = buildRandomExpFromOperands(lOperands);
                        cFrameworkWrapper.CreateItemUsageRuleInstance(lPart,lTempVariantExpr);
                    }
                    ////Previous version: when it was variant -> parts
                    //foreach (variant lVariant in cFrameworkWrapper.VariantSet)
                    //{
                    //    resetOverallFreeOperationCodes();
                    //    HashSet<part> lRandomParts = pickASeriesOfRandomParts(true);
                    //    cFrameworkWrapper.CreateItemUsageRuleInstance(lVariant, lRandomParts);
                    //}
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createItemUsageRules");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void createVariantGroups()
        {
            try
            {
                for (int i = 0; i < cMaxVariantGroupNumber; i++)
                    if (cOverallFreeVariantCodes.Count > 0)
                        cFrameworkWrapper.CreateVariantGroupInstance("VG-" + i
                                                                        , pickRandomGroupCardinality()
                                                                        , pickASeriesOfRandomVariants());
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createVariantGroups");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in makeItemListToBeChosenFrom");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultList;
        }

        private List<string> pickASeriesOfRandomOperationNames(bool pRepeatOperationsAllowed)
        {
            List<string> lChosenOperations = new List<string>();
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
                        lChosenOperations.Add(operationLookupByCode(lRandomOperationCode).Name);
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in pickASeriesOfRandomOperationNames");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lChosenOperations;
        }

        private List<Operation> pickASeriesOfRandomOperations(bool pRepeatOperationsAllowed)
        {
            List<Operation> lChosenOperations = new List<Operation>();
            try
            {
                if (!cOverallFreeOperationCodes.Count.Equals(0))
                {
                    int lNoOfOperations = cMyRandom.Next(1, cMaxOperationNumber);

                    List<int> lChosenOperationCodes = new List<int>();
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
                cOutputHandler.printMessageToConsole("error in pickASeriesOfRandomOperations");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in pickASeriesOfRandomVariants");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                    lRandomPartCode = cMyRandom.Next(0, cPartCodeLookup.Count);
                    lChosenParts.Add(partLookupByCode(lRandomPartCode));
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in pickASeriesOfRandomVariants");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in pickASeriesOfRandomOperationCodes");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in pickASeriesOfRandomOperations");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in pickRandomOperationCode");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in returnOperationNames");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultList;
        }

        private List<variant> pickASeriesOfRandomVariants()
        {
            List<variant> lChosenVariants = new List<variant>();
            try
            {
                if (cOverallFreeVariantCodes.Count > 0)
                {
                    //This function returns a series of randomly piched variants which will be bound to a specific variantgroup
                    int lNoOfVariants = cMyRandom.Next(1, cOverallFreeVariantCodes.Count);
                    List<int> lChosenVariantCodes = new List<int>();
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

                    List<string> lVariantNames = returnVariantNames(lChosenVariantCodes);

                    foreach (string lVariantName in lVariantNames)
                    {
                        lChosenVariants.Add(cFrameworkWrapper.variantLookupByName(lVariantName));
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in pickASeriesOfRandomVariants");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in returnVariantNames");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in returnRandomVariantCode");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in updateChosenVariantCode");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in pickRandomGroupCardinality");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in createVariants");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in createVariants");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void createOperations()
        {
            try
            {
                //TODO: we have to also make two random list fir preconditions and postconditions
                string lOperationPrecondition = "";
                string lOperationPostcondition = "";
                string lOperationTrigger = "";
                string lOperationRequiremnt = "";

                Operation lTempOperation = null;
                for (int i = 0; i < cMaxOperationNumber; i++)
                {
                    
                    lTempOperation = cFrameworkWrapper.CreateOperationInstance("O-" + i
                                                                            , lOperationTrigger
                                                                            , lOperationRequiremnt
                                                                            , lOperationPrecondition
                                                                            , lOperationPostcondition);
                    //Creating a list for operation lookup by code, ONLY for use with in this class
                    cOperationCodeLookup.Add(i, lTempOperation);

                }

                //foreach (var lOperation in cFrameworkWrapper.OperationSet)
                //{
                //    cFrameworkWrapper.CreateOperationInstances4AllTransitions(lOperation);
                //}
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createOperations");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in returnOperationCode");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lOperationCode;
        }

        private string returnRandomExpression(List<string> pOperands)
        {
            string lResultRandomExpression = "";
            try
            {
                List<string> lRandomNegatedOperands = new List<string>();
                foreach (var lOperand in pOperands)
                {
                    var lRandomNegatedOperand = randomNotOperator(lOperand);
                    lRandomNegatedOperands.Add(lRandomNegatedOperand);
                }

                //what ever number of operands we have we have to pick one less random operator for them to make the random expression
                int lNeededNumberOfOperators = pOperands.Count - 1;
                List<string> lRandomOperators = new List<string>();
                for (int i = 0; i < lNeededNumberOfOperators; i++)
                {
                    lRandomOperators.Add(randomOperator());
                }

                //Now making the overall random expression in preorder format
                foreach (var lOperator in lRandomOperators)
                {
                    lResultRandomExpression += lOperator + " ";
                }

                foreach (var lRandomNegatedOperand in lRandomNegatedOperands)
                {
                    lResultRandomExpression += lRandomNegatedOperand + " ";
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in returnRandomExpression");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultRandomExpression;
        }

        private string randomOperator()
        {
            string lResultRandomOperator = "";
            try
            {
                int lRandomOperatorIndex = cMyRandom.Next(0, cExpressionOperators.Count());

                lResultRandomOperator = cExpressionOperators[lRandomOperatorIndex];
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in randomOperator");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultRandomOperator;
        }

        private string randomNotOperator(string pOperand)
        {
            string lResultOperand = "";
            try
            {
                var lRandomOperatorIndex = cMyRandom.Next(0, 2);
                if (lRandomOperatorIndex.Equals(0))
                    lResultOperand = "not " + pOperand;
                else
                    lResultOperand = pOperand;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in randomNotOperator");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperand;
        }

        private string buildRandomExpFromOperands(List<string> pOperands)
        {
            string lResultRandomExp = "";
            try
            {
                //Then pick a random number of operands
                List<string> lRandomOperands = new List<string>();
                for (int i = 0; i < cMaxExpressionOperandNumber; i++)
                {
                    lRandomOperands.Add(pickRandomStringFromList(pOperands));
                }

                lResultRandomExp = returnRandomExpression(lRandomOperands);

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in buildRandomExpFromOperands");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultRandomExp;
        }

        private string pickRandomOperationState()
        {
            Enumerations.OperationInstanceState lResultOperationState = Enumerations.OperationInstanceState.Finished;
            string lReturnedOperationState = "F";
            try
            {
                int lRandomNumber = cMyRandom.Next(0, 3);

                switch (lRandomNumber)
                {
                    case 0:
                        lResultOperationState = Enumerations.OperationInstanceState.Initial;
                        break;
                    case 1:
                        lResultOperationState = Enumerations.OperationInstanceState.Executing;
                        break;
                    case 2:
                        lResultOperationState = Enumerations.OperationInstanceState.Finished;
                        break;
                    case 3:
                        lResultOperationState = Enumerations.OperationInstanceState.Unused;
                        break;
                    default:
                        lResultOperationState = Enumerations.OperationInstanceState.Finished;
                        break;
                }

                switch (lResultOperationState)
                {
                    case Enumerations.OperationInstanceState.Initial:
                        lReturnedOperationState = "I";
                        break;
                    case Enumerations.OperationInstanceState.Executing:
                        lReturnedOperationState = "E";
                        break;
                    case Enumerations.OperationInstanceState.Finished:
                        lReturnedOperationState = "F";
                        break;
                    case Enumerations.OperationInstanceState.Unused:
                        lReturnedOperationState = "U";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in pickRandomOperationState");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lReturnedOperationState;
        }

        private void updateOperationTriggerConditions()
        {
            try
            {
                //First build the maximum number of operands
                List<string> lOperands = new List<string>();
                foreach (var lPart in cFrameworkWrapper.PartSet)
                {
                    lOperands.Add(lPart.names);
                }
                foreach (var lVariant in cFrameworkWrapper.VariantSet)
                {
                    lOperands.Add(lVariant.names);
                }


                //the trigger is defined as an expression over parts and variants
                buildRandomExpFromOperands(lOperands);
                foreach (Operation lCurrentOperation in cFrameworkWrapper.OperationSet)
                {
                    lCurrentOperation.Trigger = buildRandomExpFromOperands(lOperands);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in updateOperationTriggerConditions");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private string pickRandomStringFromList(List<string> pStrings)
        {
            string lResultRandomString = "";
            try
            {
                var lIndex = cMyRandom.Next(0, pStrings.Count);
                string lTempString = pStrings.ElementAt<string>(lIndex);
                if (lTempString.StartsWith("O"))
                    lTempString = lTempString + "_" + pickRandomOperationState();
                lResultRandomString = lTempString;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in pickRandomStringFromList");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultRandomString;
        }

        private void updateOperationPrePostConditions()
        {
            try
            {
                foreach (Operation lCurrentOperation in cFrameworkWrapper.OperationSet)
                {
                    resetOverallFreeOperationCodes();

                    //This is because an operation can't be part of its own pre or post condition
                    cOverallFreeOperationCodes.Remove(returnOperationCode(lCurrentOperation.Name));

                    if (cOverallFreeOperationCodes.Count > 0)
                    {
                        List<string> lOperands = pickASeriesOfRandomOperationNames(false);
                        string lPrecondition = buildRandomExpFromOperands(lOperands);
                        //lCurrentOperation.Precondition.Clear();
                        lCurrentOperation.AddPrecondition(lPrecondition);

                        string lPostcondition = buildRandomExpFromOperands(lOperands);
                        lCurrentOperation.AddPostcondition(lPostcondition);
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in updateOperationPrePostConditions");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }
    }
}
