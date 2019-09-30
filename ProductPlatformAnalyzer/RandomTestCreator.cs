using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    class RandomTestCreator
    {
        public int MaxVariantGroupNumber { get; set; }
        public int MaxVariantNumber { get; set; }
        public int MaxPartNumber { get; set; }
        public int MaxOperationNumber { get; set; }
        public int MaxNoOfConfigurationRules { get; set; }
        public int TrueProbability { get; set; }
        public int FalseProbability { get; set; }
        public int ExpressionProbability { get; set; }
        public int MaxTraitNumber { get; set; }
        public int MaxNoOfTraitAttributes { get; set; }
        public int MaxResourceNumber { get; set; }
        public int MaxExpressionOperandNumber { get; set; }
        public Dictionary<int, Operation> OperationCodeLookup { get; set; }
        public Dictionary<int, Part> PartCodeLookup { get; set; }
        public Dictionary<int, Variant> VariantCodeLookup { get; set; }
        public List<int> OverallChosenVariantCodes { get; set; }
        public List<int> OverallFreeVariantCodes { get; set; }
        public List<int> OverallChosenOperationCodes { get; set; }
        public List<int> OverallFreeOperationCodes { get; set; }
        public string[] ExpressionOperators = new string[] { "or" };

        private Random _myRandom;
        private FrameworkWrapper _frameworkWrapper;
        private OutputHandler _outputHandler;
        private Z3SolverEngineer _z3SolverEngineer;
        private Z3Solver _z3Solver;

        public RandomTestCreator(OutputHandler pOutputHandler
                                , Z3SolverEngineer pZ3SolverEngineer
                                , FrameworkWrapper pFrameworkWrapper
                                , Z3Solver pZ3Solver)
        {
            _outputHandler = pOutputHandler;
            _z3SolverEngineer = pZ3SolverEngineer;
            _frameworkWrapper = pFrameworkWrapper;
            _z3Solver = pZ3Solver;
            

            MaxVariantGroupNumber = 1;
            MaxVariantNumber = 1;
            MaxPartNumber = 0;
            MaxOperationNumber = 1;
            MaxNoOfConfigurationRules = 1;
            TrueProbability = 100;
            FalseProbability = 0;
            ExpressionProbability = 0;
            MaxTraitNumber = 0;
            MaxResourceNumber = 0;
            MaxNoOfTraitAttributes = 0;
            MaxExpressionOperandNumber = 3;

            _myRandom = new Random();

            OperationCodeLookup = new Dictionary<int, Operation>();
            PartCodeLookup = new Dictionary<int, Part>();
            VariantCodeLookup = new Dictionary<int, Variant>();

            OverallChosenVariantCodes = new List<int>();
            OverallFreeVariantCodes = new List<int>();
            OverallChosenOperationCodes = new List<int>();
            OverallFreeOperationCodes = new List<int>();
        }

        public Operation operationLookupByCode(int pOperationCode)
        {
            Operation lResultOperation = null;
            try
            {
                if (OperationCodeLookup.ContainsKey(pOperationCode))
                    lResultOperation = OperationCodeLookup[pOperationCode];
                else
                    _outputHandler.PrintMessageToConsole("Operation " + pOperationCode + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in operationLookupByCode");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultOperation;
        }

        public Part partLookupByCode(int pPartCode)
        {
            Part lResultPart = null;
            try
            {
                if (PartCodeLookup.ContainsKey(pPartCode))
                    lResultPart = PartCodeLookup[pPartCode];
                else
                    _outputHandler.PrintMessageToConsole("Part " + pPartCode + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in partLookupByCode");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultPart;
        }

        public Variant VariantLookupByCode(int pVariantCode)
        {
            Variant lResultVariant = null;
            try
            {
                if (VariantCodeLookup.ContainsKey(pVariantCode))
                    lResultVariant = VariantCodeLookup[pVariantCode];
                else
                    _outputHandler.PrintMessageToConsole("Variant " + pVariantCode + " not found in Dictionary!");
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in variantLookupByCode");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                if (pMaxVariantGroupNumber != -1)
                    MaxVariantGroupNumber = pMaxVariantGroupNumber;
                if (pMaxVariantNumber != -1)
                {
                    MaxVariantNumber = pMaxVariantNumber;
                    resetOverallFreeVariantCodes();
                }
                if (pMaxPartNumber != -1)
                    MaxPartNumber = pMaxPartNumber;

                if (pMaxOperationNumber != -1)
                {
                    MaxOperationNumber = pMaxOperationNumber;
                    resetOverallFreeOperationCodes();
                }
                if (pTrueProbability != -1)
                    TrueProbability = pTrueProbability;
                if (pFalseProbability != -1)
                    FalseProbability = pFalseProbability;
                if (pExpressionProbability != -1)
                    ExpressionProbability = pExpressionProbability;
                if (pMaxTraitNumber != -1)
                    MaxTraitNumber = pMaxTraitNumber;
                if (pMaxNoOfTraitAttributes != -1)
                    MaxNoOfTraitAttributes = pMaxNoOfTraitAttributes;
                if (pMaxResourceNumber != -1)
                    MaxResourceNumber = pMaxResourceNumber;
                if (pMaxExpressionOperandNumber != -1)
                    MaxExpressionOperandNumber = pMaxExpressionOperandNumber;
                if (pMaxNoOfConfigurationRules != -1)
                    MaxNoOfConfigurationRules = pMaxNoOfConfigurationRules;
                /*if (pFrameworkWrapper != null)
                    cFrameworkWrapper = pFrameworkWrapper;*/


                createVariants();
                _z3SolverEngineer.ConvertFVariants2Z3Variants();
                createVariantGroups();
                _z3SolverEngineer.ProduceVariantGroupGCardinalityConstraints();

                createParts();
                _z3SolverEngineer.ConvertFParts2Z3Parts();

                createItemUsageRules();
                _z3SolverEngineer.ConvertItemUsageRulesConstraints();

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
                _frameworkWrapper.PrintDataSummary();

                lResult = true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in createRandomData");
                _outputHandler.PrintMessageToConsole(ex.Message);
                
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
                foreach (var lPart in _frameworkWrapper.PartSet)
                {
                    lOperands.Add(lPart.Names);
                }
                foreach (var lVariant in _frameworkWrapper.VariantSet)
                {
                    lOperands.Add(lVariant.Names);
                }

                string lRandomConfigurationRule = "";

                _z3Solver.SolverPushFunction();
                for (int i = 0; i < MaxNoOfConfigurationRules; i++)
                {
                    lRandomConfigurationRule = buildRandomExpFromOperands(lOperands);
                    
                    Microsoft.Z3.BoolExpr lBoolExpr = _z3SolverEngineer.ConvertComplexString2BoolExpr(lRandomConfigurationRule);
                    _z3Solver.AddConstraintToSolver(lBoolExpr, "RandomConfigRule");
                    //We add a random constraint rule if it is satisfiable we keep it otherwise we remove it
                    
                    //This line checks if the constraints in the model have any conflicts with each other
                    int lTransitionNo = 0;
                    Microsoft.Z3.Status lAnalysisResult = _z3SolverEngineer.AnalyzeModelConstraints(lTransitionNo);

                    if (lAnalysisResult.Equals(Microsoft.Z3.Status.SATISFIABLE))
                    {
                        lSatisfiableConfigurationRules.Add(lRandomConfigurationRule);
                    }
                    else
                    {
                        _z3Solver.SolverPopFunction();

                        i--;
                        _z3Solver.SolverPushFunction();
                        foreach (string lRandomConfigRule in lSatisfiableConfigurationRules)
                        {
                            Microsoft.Z3.BoolExpr lBoolExpr1 = _z3SolverEngineer.ConvertComplexString2BoolExpr(lRandomConfigurationRule);
                            _z3Solver.AddConstraintToSolver(lBoolExpr1, "RandomConfigRule");
                        }


                    }


                    
                }
                _z3Solver.SolverPopFunction();

                //Adding the satisfied set of configuration rules to the model
                if (lSatisfiableConfigurationRules.Count > 0)
                {
                    foreach (var lConfigRule in lSatisfiableConfigurationRules)
                    {
                        _frameworkWrapper.ConstraintSet.Add(lConfigRule);
                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in createConfigurationRules");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void createTraits()
        {
            try
            {
                HashSet<Trait> lInheritedTraits = new HashSet<Trait>();
                HashSet<Tuple<string,string>> lTraitAttributes = new HashSet<Tuple<string,string>>();
                for (int i = 0; i < MaxTraitNumber; i++)
                {
                    string lTraitName = "T" + i;
                    lTraitAttributes = makeRandomSetOfTraitAttributes(lTraitName);
                    _frameworkWrapper.CreateTraitInstanceNLocalSets(lTraitName
                                                            , lInheritedTraits
                                                            , lTraitAttributes);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in createTraits");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private HashSet<Tuple<string, string>> makeRandomSetOfTraitAttributes(string pTraitName)
        {
            HashSet<Tuple<string, string>> lTraitAttributes = new HashSet<Tuple<string, string>>();
            try
            {
                int lNoOfAttributes = _myRandom.Next(1, MaxNoOfTraitAttributes);

                for (int i = 0; i < lNoOfAttributes; i++)
                {
                    Tuple<string, string> lAttributeTuple = new Tuple<string, string>("int", pTraitName + "Att" + i);
                    lTraitAttributes.Add(lAttributeTuple);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in makeRandomSetOfTraitAttributes");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _outputHandler.PrintMessageToConsole("error in createResources");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void resetOverallFreeVariantCodes()
        {
            try
            {
                for (int i = 0; i < MaxVariantNumber; i++)
                    OverallFreeVariantCodes.Add(i);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in resetOverallFreeVariantCodes");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void resetOverallFreeOperationCodes()
        {
            try
            {
                OverallFreeOperationCodes.RemoveRange(0,OverallFreeOperationCodes.Count);

                for (int i = 0; i < MaxOperationNumber; i++)
                    OverallFreeOperationCodes.Add(i);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in resetOverallFreeOperationCodes");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void createItemUsageRules()
        {
            try
            {
                if (MaxPartNumber > 0)
                {
                    List<string> lOperands = new List<string>();
                    foreach (Variant lVariant in _frameworkWrapper.VariantSet)
	                {
		                 lOperands.Add(lVariant.Names);
	                }

                    foreach (Part lPart in _frameworkWrapper.PartSet)
                    {
                        string lTempVariantExpr = buildRandomExpFromOperands(lOperands);
                        _frameworkWrapper.CreateItemUsageRuleInstance(lPart,lTempVariantExpr);
                    }
                    ////Previous version: when it was Variant-> parts
                    //foreach (VariantlVariant in cFrameworkWrapper.VariantSet)
                    //{
                    //    resetOverallFreeOperationCodes();
                    //    HashSet<part> lRandomParts = pickASeriesOfRandomParts(true);
                    //    cFrameworkWrapper.CreateItemUsageRuleInstance(lVariant, lRandomParts);
                    //}
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in createItemUsageRules");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void createVariantGroups()
        {
            try
            {
                for (int i = 0; i < MaxVariantGroupNumber; i++)
                {
                    VariantGroup lVariantGroup = null;
                    if (OverallFreeVariantCodes.Count > 0)
                    {
                        var lRandomGroupCardinality = pickRandomGroupCardinality();
                        var lRandomVariants = pickASeriesOfRandomVariants();

                        lVariantGroup = new VariantGroup("VG-" + i
                                                        , lRandomGroupCardinality
                                                        , lRandomVariants);

                        foreach (var lVariant in lRandomVariants)
                            lVariant.MyVariantGroup = lVariantGroup;
                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in createVariantGroups");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _outputHandler.PrintMessageToConsole("error in makeItemListToBeChosenFrom");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultList;
        }

        private List<string> pickASeriesOfRandomOperationNames(bool pRepeatOperationsAllowed)
        {
            List<string> lChosenOperations = new List<string>();
            try
            {
                if (!OverallFreeOperationCodes.Count.Equals(0))
                {
                    int lNoOfOperations = _myRandom.Next(1, OverallFreeOperationCodes.Count);

                    HashSet<int> lChosenOperationCodes = new HashSet<int>();
                    int lRandomOperationCode = 0;
                    for (int i = 0; i < lNoOfOperations; i++)
                    {
                        lRandomOperationCode = _myRandom.Next(1, MaxOperationNumber);
                        lChosenOperations.Add(operationLookupByCode(lRandomOperationCode).Name);
                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in pickASeriesOfRandomOperationNames");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lChosenOperations;
        }

        private List<Operation> pickASeriesOfRandomOperations(bool pRepeatOperationsAllowed)
        {
            List<Operation> lChosenOperations = new List<Operation>();
            try
            {
                if (!OverallFreeOperationCodes.Count.Equals(0))
                {
                    int lNoOfOperations = _myRandom.Next(1, MaxOperationNumber);

                    List<int> lChosenOperationCodes = new List<int>();
                    int lRandomOperationCode = 0;
                    for (int i = 0; i < lNoOfOperations; i++)
                    {
                        lRandomOperationCode = _myRandom.Next(1, OperationCodeLookup.Count);
                        lChosenOperations.Add(operationLookupByCode(lRandomOperationCode));
                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in pickASeriesOfRandomOperations");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lChosenOperations;
        }

        private HashSet<Variant> pickASeriesOfRandomVariants(bool pRepeatVariantsAllowed)
        {
            HashSet<Variant> lChosenVariants = new HashSet<Variant>();
            try
            {
                int lNoOfVariants = _myRandom.Next(1, MaxVariantNumber);

                int lRandomVariantCode = 0;
                for (int i = 0; i < lNoOfVariants; i++)
                {
                    lRandomVariantCode = _myRandom.Next(1, VariantCodeLookup.Count);
                    lChosenVariants.Add(VariantLookupByCode(lRandomVariantCode));
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in pickASeriesOfRandomVariants");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lChosenVariants;
        }

        private HashSet<Part> pickASeriesOfRandomParts(bool pRepeatPartsAllowed)
        {
            HashSet<Part> lChosenParts = new HashSet<Part>();
            try
            {
                int lNoOfParts = _myRandom.Next(1, MaxPartNumber);

                int lRandomPartCode = 0;
                for (int i = 0; i < lNoOfParts; i++)
                {
                    lRandomPartCode = _myRandom.Next(0, PartCodeLookup.Count);
                    lChosenParts.Add(partLookupByCode(lRandomPartCode));
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in pickASeriesOfRandomVariants");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                    int lNoOfOperations = _myRandom.Next(1, cOverallFreeOperationCodes.Count);

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
                int lOperationIndex = _myRandom.Next(0, cOverallFreeOperationCodes.Count);
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
                _outputHandler.PrintMessageToConsole("error in returnOperationNames");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultList;
        }

        private List<Variant> pickASeriesOfRandomVariants()
        {
            List<Variant> lChosenVariants = new List<Variant>();
            try
            {
                if (OverallFreeVariantCodes.Count > 0)
                {
                    //This function returns a series of randomly piched variants which will be bound to a specific variantgroup
                    int lNoOfVariants = _myRandom.Next(1, OverallFreeVariantCodes.Count);
                    List<int> lChosenVariantCodes = new List<int>();
                    int lChosenVariantCode;

                    for (int i = 0; i < lNoOfVariants; i++)
                    {

                        lChosenVariantCode = returnRandomVariantCode();

                        //We don't want a group to have zero variants
                        if (!OverallChosenVariantCodes.Contains(lChosenVariantCode))
                        {
                            updateChosenVariantCode(lChosenVariantCode);
                            lChosenVariantCodes.Add(lChosenVariantCode);
                        }
                    }

                    List<string> lVariantNames = returnVariantNames(lChosenVariantCodes);

                    foreach (string lVariantName in lVariantNames)
                    {
                        lChosenVariants.Add(_frameworkWrapper.VariantLookupByName(lVariantName));
                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in pickASeriesOfRandomVariants");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _outputHandler.PrintMessageToConsole("error in returnVariantNames");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultList;
        }

        private int returnRandomVariantCode()
        {
            int lResultVariantCode = 0;
            try
            {
                int lRandomVariantIndex = _myRandom.Next(0, OverallFreeVariantCodes.Count);
                lResultVariantCode = OverallFreeVariantCodes[lRandomVariantIndex];
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in returnRandomVariantCode");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultVariantCode;
        }

        private void updateChosenVariantCode(int pChosenVariantCode)
        {
            try
            {
                OverallChosenVariantCodes.Add(pChosenVariantCode);
                OverallFreeVariantCodes.Remove(pChosenVariantCode);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in updateChosenVariantCode");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private string pickRandomGroupCardinality()
        {
            string lResultGroupCardinality = "";
            try
            {
                int lGroupcardinality = _myRandom.Next(1, 2);

                if (lGroupcardinality == 1)
                    lResultGroupCardinality = "choose exactly one";
                else
                    lResultGroupCardinality = "choose at least one";
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in pickRandomGroupCardinality");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultGroupCardinality;
        }

        public void createVariants()
        {
            try
            {
                Variant lTempVariant = null;
                for (int i = 0; i < MaxVariantNumber; i++)
                {
                    lTempVariant = _frameworkWrapper.CreateVariantInstanceNLocalSets("V-" + i);
                    VariantCodeLookup.Add(i, lTempVariant);
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in createVariants");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void createParts()
        {
            try
            {
                Part lTempPart = null;
                for (int i = 0; i < MaxPartNumber; i++)
                {
                    lTempPart = _frameworkWrapper.CreatePartInstanceNLocalSets("P-" + i);
                    PartCodeLookup.Add(i, lTempPart);
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in createVariants");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                string lResource = "";

                Operation lTempOperation = null;
                for (int i = 0; i < MaxOperationNumber; i++)
                {
                    
                    //lTempOperation = _frameworkWrapper.CreateOperationInstance("O-" + i
                    //                                                        , lOperationTrigger
                    //                                                        , lOperationRequiremnt
                    //                                                        , lOperationPrecondition
                    //                                                        , lOperationPostcondition
                    //                                                        , lResource);
                    lTempOperation = new Operation("O-" + i
                                                , lOperationTrigger
                                                , lOperationRequiremnt
                                                , lOperationPrecondition
                                                , lOperationPostcondition
                                                , lResource);

                    //Creating a list for operation lookup by code, ONLY for use with in this class
                    OperationCodeLookup.Add(i, lTempOperation);

                }

                //foreach (var lOperation in cFrameworkWrapper.OperationSet)
                //{
                //    cFrameworkWrapper.CreateOperationInstances4AllTransitions(lOperation);
                //}
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in createOperations");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _outputHandler.PrintMessageToConsole("error in returnOperationCode");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _outputHandler.PrintMessageToConsole("error in returnRandomExpression");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultRandomExpression;
        }

        private string randomOperator()
        {
            string lResultRandomOperator = "";
            try
            {
                int lRandomOperatorIndex = _myRandom.Next(0, ExpressionOperators.Count());

                lResultRandomOperator = ExpressionOperators[lRandomOperatorIndex];
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in randomOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultRandomOperator;
        }

        private string randomNotOperator(string pOperand)
        {
            string lResultOperand = "";
            try
            {
                var lRandomOperatorIndex = _myRandom.Next(0, 2);
                if (lRandomOperatorIndex.Equals(0))
                    lResultOperand = "not " + pOperand;
                else
                    lResultOperand = pOperand;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in randomNotOperator");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                for (int i = 0; i < MaxExpressionOperandNumber; i++)
                {
                    lRandomOperands.Add(pickRandomStringFromList(pOperands));
                }

                lResultRandomExp = returnRandomExpression(lRandomOperands);

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in buildRandomExpFromOperands");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultRandomExp;
        }

        private string pickRandomOperationState()
        {
            Enumerations.OperationInstanceState lResultOperationState = Enumerations.OperationInstanceState.Finished;
            string lReturnedOperationState = "F";
            try
            {
                int lRandomNumber = _myRandom.Next(0, 3);

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
                _outputHandler.PrintMessageToConsole("error in pickRandomOperationState");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lReturnedOperationState;
        }

        private void updateOperationTriggerConditions()
        {
            try
            {
                //First build the maximum number of operands
                List<string> lOperands = new List<string>();
                foreach (var lPart in _frameworkWrapper.PartSet)
                {
                    lOperands.Add(lPart.Names);
                }
                foreach (var lVariant in _frameworkWrapper.VariantSet)
                {
                    lOperands.Add(lVariant.Names);
                }


                //the trigger is defined as an expression over parts and variants
                buildRandomExpFromOperands(lOperands);
                foreach (Operation lCurrentOperation in _frameworkWrapper.OperationSet)
                {
                    lCurrentOperation.Trigger = buildRandomExpFromOperands(lOperands);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in updateOperationTriggerConditions");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private string pickRandomStringFromList(List<string> pStrings)
        {
            string lResultRandomString = "";
            try
            {
                var lIndex = _myRandom.Next(0, pStrings.Count);
                string lTempString = pStrings.ElementAt<string>(lIndex);
                if (lTempString.StartsWith("O"))
                    lTempString = lTempString + "_" + pickRandomOperationState();
                lResultRandomString = lTempString;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in pickRandomStringFromList");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultRandomString;
        }

        private void updateOperationPrePostConditions()
        {
            try
            {
                foreach (Operation lCurrentOperation in _frameworkWrapper.OperationSet)
                {
                    resetOverallFreeOperationCodes();

                    //This is because an operation can't be Part of its own pre or post condition
                    OverallFreeOperationCodes.Remove(returnOperationCode(lCurrentOperation.Name));

                    if (OverallFreeOperationCodes.Count > 0)
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
                _outputHandler.PrintMessageToConsole("error in updateOperationPrePostConditions");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }
    }
}
