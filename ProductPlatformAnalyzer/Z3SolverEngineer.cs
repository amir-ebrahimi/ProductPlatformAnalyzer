using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Z3;
using System.Collections;
using System.IO;
using System.Xml;

namespace ProductPlatformAnalyzer
{
    class Z3SolverEngineer
    {
        private FrameworkWrapper lFrameworkWrapper;
        private Z3Solver lZ3Solver;
        private RandomTestCreator lRandomTestCreator;
        private bool lDebugMode;
        private bool lOpSeqAnalysis;
        private bool lNeedPreAnalysis;
        private bool lPreAnalysisResult;


        private BoolExpr lOp_I_CurrentState;
        private BoolExpr lOp_E_CurrentState;
        private BoolExpr lOp_F_CurrentState;
        private BoolExpr lOp_U_CurrentState;

        private BoolExpr lOp_I_NextState;
        private BoolExpr lOp_E_NextState;
        private BoolExpr lOp_F_NextState;
        private BoolExpr lOp_U_NextState;

        private BoolExpr lOpPrecondition;
        private BoolExpr lOpPostcondition;

        public Z3SolverEngineer()
        {
            lFrameworkWrapper = new FrameworkWrapper();
            lRandomTestCreator = new RandomTestCreator();
            lZ3Solver = new Z3Solver();
            lDebugMode = false;
            lOpSeqAnalysis = true;
            lNeedPreAnalysis = true;
            lPreAnalysisResult = true;
        }

        public void ResetAnalyzer()
        {
            lZ3Solver = new Z3Solver();
        }

        public Solver getZ3Solver()
        {
            throw new NotImplementedException();
        }

        public void makeZ3Solver()
        {
            throw new NotImplementedException();
        }

        public void makeExpressionListFromVariantGroupList()
        {
            List<variantGroup> localVariantGroupList = lFrameworkWrapper.VariantGroupList;

            foreach (variantGroup localVariantGroup in localVariantGroupList)
            {
                lZ3Solver.AddBooleanExpression(localVariantGroup.names);
            }
        }

        public void makeExpressionListFromVariantList()
        {
            List<variant> localVariantList = lFrameworkWrapper.VariantList;

            foreach (variant localVariant in localVariantList)
            {
                lZ3Solver.AddBooleanExpression(localVariant.names);
            }
        }

        public void loadInitialData(String pFile)
        {
            try
            {
                //hardwire input data
                //Reading data from hard code
                //lFrameworkWrapper.createTestData6();

                //Reading data from an XML file
                string exePath = Directory.GetCurrentDirectory();
                string endPath = null;
                if (pFile != null)
                    endPath = pFile;
                else
                    endPath = "";

                LoadInitialDataFromXMLFile(exePath + "../../../" + endPath);
   //             lFrameworkWrapper.LoadInitialDataFromXMLFile(endPath);


                //lFrameworkWrapper.LoadInitialDataFromXMLFile("C:/Users/Amir/Desktop/Output/InitialData/TestData1.xml");

                //Creating random data
                //lRandomTestCreator.createRandomData(2, 4, 3, 100, 0, 0, 4, 3, 2);
                //Console.WriteLine(lFrameworkWrapper.getConstraintList());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in loadInitialData");
                Console.WriteLine(ex.Message);
            }
        }

        public bool testConstraintConvertion(int pState, String pFile, bool pDone)
        {
            bool lTestResult = false;
            try
            {
                lOpSeqAnalysis = true;

                convertProductPlatformNConfigurationRules();

                convertOperationsNPrecedenceRulesNOperationVariantRelations(pState);

                convertResourcesNOperationResourceRelations();

                lTestResult = convertGoalNAnlyzeResults(pState, pDone);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in testConstraintConvertion");
                Console.WriteLine(ex.Message);
            }
            return lTestResult;
        }

        /// <summary>
        /// This function converts the static part of the product platform, i.e. variants, 
        /// and configuration rules, i.e. variant group cardinality and constraint rules
        /// </summary>
        private void convertProductPlatformNConfigurationRules()
        {
            try
            {
                convertFVariants2Z3Variants();
                //formula 2
                produceVariantGroupGCardinalityConstraints();
                //formula 3
                convertFConstraint2Z3Constraint();

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertProductPlatformNConfigurationRules");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// This function converts the dynamic part of the product platform, i.e. operations 
        /// and operation relations (precedence rules)
        /// and the relationship between operations and variants
        /// </summary>
        /// <param name="pState"></param>
        private void convertOperationsNPrecedenceRulesNOperationVariantRelations(int pState)
        {
            try
            {
                convertFOperations2Z3Operations(pState);

                if (lNeedPreAnalysis && pState == 0)
                    lPreAnalysisResult = lFrameworkWrapper.checkPreAnalysis();

                if (lPreAnalysisResult)
                {
                    //forula 4
                    initializeFVariantOperation2Z3Constraints();

                    //formula 5 and New Formula
                    convertFOperations2Z3ConstraintNewVersion(pState);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertOperationsNPrecedenceRulesNOperationVariantRelations");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// This function converts the resources and the operation resource relationships
        /// </summary>
        private void convertResourcesNOperationResourceRelations()
        {
            try
            {
                if (lPreAnalysisResult)
                {
                    //New formulas for implementing resources
                    if (lFrameworkWrapper.ResourceList.Count != 0)
                    {
                        convertFResource2Z3Constraints();
                        checkFOperationExecutabilityWithCurrentResourcesUsingZ3Constraints();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertResourcesNOperationResourceRelations");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// This function converts the goals and analyzes the result of the goals
        /// </summary>
        /// <param name="pState"></param>
        /// <param name="pDone"></param>
        /// <returns>Analysis result of the goal</returns>
        private bool convertGoalNAnlyzeResults(int pState, bool pDone)
        {
            bool lTestResult = false;
            try
            {
                if (lPreAnalysisResult)
                {
                    if (lOpSeqAnalysis)
                    {
                        if (!pDone)
                            //formula 7 and 8
                            convertFGoals2Z3GoalsVersion2(pState);
                    }

                    if (!pDone)
                    {
                        Console.WriteLine("Analysis No: " + pState);
                        lTestResult = analyseZ3Model(pState, pDone, lFrameworkWrapper);

                        lZ3Solver.WriteDebugFile(pState);
                    }
                    else
                    {
                        Console.WriteLine("Finished: ");
                        lTestResult = analyseZ3Model(pState, pDone, lFrameworkWrapper);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertGoalNAnlyzeResults");
                Console.WriteLine(ex.Message);
            }
            return lTestResult;
        }

        private void convertFResource2Z3Constraints()
        {
            try
            {
                List<resource> lResourceList = lFrameworkWrapper.ResourceList;
                foreach (resource lResource in lResourceList)
                {
                    string lResourceName = lResource.names;
                    foreach (Tuple<string,string,string> lAttribute in lResource.attributes)
                    {
                        convertFResourceAttribute2Z3Constraint(lResourceName, lAttribute);


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertFResource2Z3Constraints");
                Console.WriteLine(ex.Message);
            }
        }

        private void convertFResourceAttribute2Z3Constraint(string pResourceName, Tuple<string,string,string> pAttribute)
        {
            try
            {
                string lAttributeName = pResourceName + '_' + pAttribute.Item1;
                string lAttributeType = pAttribute.Item2;
                string lAttributeValue = pAttribute.Item3;

                switch (lAttributeType)
                {
                    case "string":
                        break;
                    case "int":
                        lZ3Solver.AddIntegerExpression(lAttributeName);
                        IntExpr lExprVariable = lZ3Solver.FindIntExpressionUsingName(lAttributeName);
                        lZ3Solver.AddEqualOperator2Constraints(lExprVariable
                                                                , int.Parse(lAttributeValue)
                                                                , "Attribute_Value");
                        break;
                    default:
                        //The default case is boolean variables
                        lZ3Solver.AddBooleanExpression(lAttributeName);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertFResourceAttribute2Z3Constraint");
                Console.WriteLine(ex.Message);
            }
        }

        private void checkFOperationExecutabilityWithCurrentResourcesUsingZ3Constraints()
        {
            try
            {
                List<string> lActiveOperationNames = lFrameworkWrapper.ActiveOperationNamesList;
                List<resource> lResourceList = lFrameworkWrapper.ResourceList;
//                List<string> lPossibleToRunOperationVariableNames = new List<string>();

                foreach (string lActiveOperationName in lActiveOperationNames)
                {
//                    List<string> lPossibleResourceVariablesForActiveOperation = new List<string>();
                    List<string> lUseResourceVariablesForActiveOperation = new List<string>();
                    
                    operation lActiveOperation = lFrameworkWrapper.getOperationFromOperationName(lActiveOperationName);

                    //This variable shows if the current operation can be run with at least one resource
                    string lPossibleToRunActiveOperationName = "Possible_to_run_" + lActiveOperationName;
                    lZ3Solver.AddBooleanExpression(lPossibleToRunActiveOperationName);

                    foreach (resource lActiveResource in lResourceList)
                    {
                        //This variable shows if the current operation CAN be run with the current resource
                        string lPossibleToUseResource4OperationName = "Possible_to_use_" + lActiveResource.names + "_for_" + lActiveOperationName;
                        lZ3Solver.AddBooleanExpression(lPossibleToUseResource4OperationName);
//                        lPossibleResourceVariablesForActiveOperation.Add(lPossibleToUseResource4OperationName);

                        //This variable shows if the current operation WILL be run with the current resource
                        string lUseResource4OperationName = "Use_" + lActiveResource.names + "_for_" + lActiveOperationName;
                        lZ3Solver.AddBooleanExpression(lUseResource4OperationName);
                        lUseResourceVariablesForActiveOperation.Add(lUseResource4OperationName);

                        //formula 6.1
                        //Possible_to_use_ActiveResource_for_ActiveOperation <-> Operation.Requirement
                        string lActiveOperationRequirements = lFrameworkWrapper.ReturnOperationRequirements(lActiveOperationName);
                        if (lActiveOperationRequirements != "")
                        {
                            //Active operation has requirements defined

                            List<resource> lOperationChosenResources = lFrameworkWrapper.ReturnOperationChosenResource(lActiveOperationName);

                            if (lOperationChosenResources.Contains(lActiveResource))
                            {
                                //This active resource is one of the resources that can run this operation
                                BoolExpr lActiveOperationRequirementExpr = returnFExpression2Z3Constraint(lActiveOperationRequirements);
                                lZ3Solver.AddTwoWayImpliesOperator2Constraints(lZ3Solver.FindBoolExpressionUsingName(lPossibleToUseResource4OperationName)
                                                                            , lActiveOperationRequirementExpr
                                                                            , "formula 6.1");
                            }
                            else
                            {
                                //This active resource is not one of the resources that can run this operation
                                lZ3Solver.AddNotOperator2Constraints(lPossibleToUseResource4OperationName
                                                                        , "formula 6.1");
                            }
                        }
                        else
                            //Active operation has no requirements defined, hence it is always possible to run
                            lZ3Solver.AddConstraintToSolver(lZ3Solver.FindBoolExpressionUsingName(lPossibleToRunActiveOperationName), "formula 6.1");


                        //formula 6.2
                        // Use_ActiveResource_ActiveOperation -> Possible_to_use_ActiveResource_for_ActiveOperation
                        lZ3Solver.AddImpliesOperator2Constraints(lZ3Solver.FindBoolExpressionUsingName(lUseResource4OperationName)
                                                                , lZ3Solver.FindBoolExpressionUsingName(lPossibleToUseResource4OperationName)
                                                                , "formula 6.2");
                    }

                    /////////////////////////////////////////////////////////
                    //formula 6.3
                    //This formula makes sure this operation can be run by ONLY one resource

                    //Possible_to_run_ActiveOperation -> Possible_to_use_Resource1_for_ActiveOperation or Possible_to_use_Resource2_for_ActiveOperation or ...

                    //                    lPossibleToRunOperationVariableNames.Add(lPossibleToRunActiveOperationName);
                    BoolExpr lPossibleToRunActiveOperation = lZ3Solver.FindBoolExpressionUsingName(lPossibleToRunActiveOperationName);
                    lZ3Solver.AddTwoWayImpliesOperator2Constraints(lPossibleToRunActiveOperation
                                                                , lZ3Solver.XorOperator(lUseResourceVariablesForActiveOperation)
                                                                , "formula6.3");
                    //formula 6.2
                    //This formula makes sure this operation can be executed by one resource
                    //Before
                    //BoolExpr lRightHandSide = lZ3Solver.OrOperator(lPossibleResourceVariablesForActiveOperation);
                    //Now
                    //lZ3Solver.AddImpliesOperator2Constraints(lPossibleToRunActiveOperation, lUseActiveResource4ActiveOperation, "formula6.2");

                    //formula 6.4
                    if (!lActiveOperation.precondition.Contains(lPossibleToRunActiveOperationName))
                        lActiveOperation.precondition.Add(lPossibleToRunActiveOperationName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertFResourceOperationMapping2Z3Constraints");
                Console.WriteLine(ex.Message);
            }
        }

        private bool analyseZ3Model(int pState, bool done, FrameworkWrapper wrapper)
        {
            //returns the result of checking the satisfiability;
            return lZ3Solver.CheckSatisfiability(pState, done, wrapper);
        }

        public void convertFVariants2Z3Variants()
        {
            try
            {
                List<variant> lVariantList = lFrameworkWrapper.VariantList;

                foreach (variant lVariant in lVariantList)
                    lZ3Solver.AddBooleanExpression(lVariant.names);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertFVariants2Z3Variants");
                Console.WriteLine(ex.Message);
            }

        }

        private variant ReturnCurrentVariant(string pVariantExpression)
        {
            variant lResultVariant = new variant();
            try
            {
                if (pVariantExpression.Trim().Contains(' '))
                {
                    //If the variant expression does contain any spaces other than the start or ending space then it is an expression

                    //A new Virtual variant need to be build

                    //The virtual variant needs to be added to virtual variant group

                    //A new constraint needs to be added relating the virtual variant to the variant expression

                    //The new virtual variant and the exression it represents needs to be added to virtual variant list in frameworkwrapper

                }
                else
                {
                    //If the variant expression does NOT contain any spaces other than the start or ending space then it is a variant
                    lResultVariant = lFrameworkWrapper.findVariantWithName(pVariantExpression);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnCurrentVariant");
                Console.WriteLine(ex.Message);
            }
            return lResultVariant;
        }

        public void convertFOperations2Z3Operations(int pState)
        {
            try
            {
                //Loop over variant list
                //For each variant find the variant in variant-operation mapping table
                //For each operation in this list make the following operation boolean variables
                List<variant> localVariantList = lFrameworkWrapper.VariantList;

                foreach (variant lCurrentVariant in localVariantList)
                {
                    List<operation> lOperationList = lFrameworkWrapper.getVariantExprOperations(lCurrentVariant.names);
                    if (lOperationList != null)
                    {
                        foreach (operation lOperation in lOperationList)
                        {

                            lFrameworkWrapper.addActiveOperationName(lOperation.names);

                            //Current state of operation
                            addCurrentVariantOperationInstanceVariables(lOperation, lCurrentVariant, pState);

                            //Next state of operation
                            int lNewState = pState + 1;
                            addCurrentVariantOperationInstanceVariables(lOperation, lCurrentVariant, lNewState);
                        }
                    }
                }

                //Here we want to list all operations which are not needed for any specific variant
                List<operation> lMasterOperationList = lFrameworkWrapper.OperationList;
                foreach (operation lOperation in lMasterOperationList)
                {
                    if (!lFrameworkWrapper.isActiveOperation(lOperation.names))
                    {
                        lFrameworkWrapper.addInActiveOperationName(lOperation.names);

                        //lFrameworkWrapper.addInActiveOperation(lOperation.names + "_I_0_" + pState.ToString());
                        lZ3Solver.AddBooleanExpression(lOperation.names + "_I_0_" + pState.ToString());
                        //lFrameworkWrapper.addInActiveOperation(lOperation.names + "_E_0_" + pState.ToString());
                        lZ3Solver.AddBooleanExpression(lOperation.names + "_E_0_" + pState.ToString());
                        //lFrameworkWrapper.addInActiveOperation(lOperation.names + "_F_0_" + pState.ToString());
                        lZ3Solver.AddBooleanExpression(lOperation.names + "_F_0_" + pState.ToString());
                        //lFrameworkWrapper.addInActiveOperation(lOperation.names + "_U_0_" + pState.ToString());
                        lZ3Solver.AddBooleanExpression(lOperation.names + "_U_0_" + pState.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertFOperations2Z3Operations");
                Console.WriteLine(ex.Message);
            }
        }

        
        private void addCurrentVariantOperationInstanceVariables(operation pOperation, variant pVariant, int pState)
        {
            try
            {
                lZ3Solver.AddBooleanExpression(pOperation.names + "_I_" + pVariant.index + "_" + pState.ToString());
                lZ3Solver.AddBooleanExpression(pOperation.names + "_E_" + pVariant.index + "_" + pState.ToString());
                lZ3Solver.AddBooleanExpression(pOperation.names + "_F_" + pVariant.index + "_" + pState.ToString());
                lZ3Solver.AddBooleanExpression(pOperation.names + "_U_" + pVariant.index + "_" + pState.ToString());

                lZ3Solver.AddBooleanExpression(pOperation.names + "_PreCondition_" + pVariant.index + "_" + pState.ToString());
                lZ3Solver.AddBooleanExpression(pOperation.names + "_PostCondition_" + pVariant.index + "_" + pState.ToString());

                lFrameworkWrapper.addActiveOperationInstanceName(pOperation.names + "_I_" + pVariant.index + "_" + pState.ToString());
                lFrameworkWrapper.addActiveOperationInstanceName(pOperation.names + "_E_" + pVariant.index + "_" + pState.ToString());
                lFrameworkWrapper.addActiveOperationInstanceName(pOperation.names + "_F_" + pVariant.index + "_" + pState.ToString());
                lFrameworkWrapper.addActiveOperationInstanceName(pOperation.names + "_U_" + pVariant.index + "_" + pState.ToString());

                lFrameworkWrapper.addActiveOperationInstanceName(pOperation.names + "_PreCondition_" + pVariant.index + "_" + pState.ToString());
                lFrameworkWrapper.addActiveOperationInstanceName(pOperation.names + "_PostCondition_" + pVariant.index + "_" + pState.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addCurrentVariantOperationInstances");
                Console.WriteLine(ex.Message);
            }
        }

        public void resetCurrentStateAndNewStateOperationVariables(operation pOperation, variant pVariant, int pState, string pConstraintSource)
        {
            lOp_I_CurrentState = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_I_" + pVariant.index + "_" + pState.ToString());
            lOp_E_CurrentState = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_E_" + pVariant.index + "_" + pState.ToString());
            lOp_F_CurrentState = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_F_" + pVariant.index + "_" + pState.ToString());
            lOp_U_CurrentState = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_U_" + pVariant.index + "_" + pState.ToString());

            int lNewState = pState + 1;

            lOp_I_NextState = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_I_" + pVariant.index + "_" + lNewState.ToString());
            lOp_E_NextState = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_E_" + pVariant.index + "_" + lNewState.ToString());
            lOp_F_NextState = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_F_" + pVariant.index + "_" + lNewState.ToString());
            lOp_U_NextState = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_U_" + pVariant.index + "_" + lNewState.ToString());

            resetOperationPrecondition(pOperation, pVariant, pState, pConstraintSource);
            resetOperationPostcondition(pOperation, pVariant, pState, pConstraintSource);
        }

        public void produceVariantGroupGCardinalityConstraints()
        {
            try
            {
                //Formula 2
                List<variantGroup> localVariantGroupsList = lFrameworkWrapper.VariantGroupList;

                foreach (variantGroup lVariantGroup in localVariantGroupsList)
                {
                    makeGCardinalityConstraint(lVariantGroup);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in produceVariantGroupGCardinalityConstraints");
                Console.WriteLine(ex.Message);
            }
        }

        enum VariantGroupCardinality
        {
            Choose_Exactly_One,
            Choose_At_Least_One,
            Choose_Zero_Or_More,
            Choose_All
        }

        //TODO: We need an enum for the Variant Group Cardinality to be used here and also used where the Variant Groups are created from the input files
        public void makeGCardinalityConstraint(variantGroup pVariantGroup)
        {
            try
            {
                //Formula 2
                List<variant> lVariants = pVariantGroup.variant;

                List<String> lVariantNames = new List<string>();
                foreach (variant lVariant in lVariants)
                    lVariantNames.Add(lVariant.names);

                switch (pVariantGroup.gCardinality)
                {
                    case "choose exactly one":
                        {
                            lZ3Solver.AddXorOperator2Constraints(lVariantNames, "GroupCardinality");

                            break;
                        }
                    case "choose at least one":
                        {
                            lZ3Solver.AddOrOperator2Constraints(lVariantNames, "GroupCardinality");

                            break;
                        }
                    case "choose all":
                        {
                            lZ3Solver.AddAndOperator2Constraints(lVariantNames, "GroupCardinality");

                            break;
                        }
                    case "choose zero or more":
                        {

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void initializeFVariantOperation2Z3Constraints()
        {
            try
            {
                //formula 4
                // C = (BIG AND) (O_I_j_0 <=> V_j) AND (! O_e_j_0) AND (! O_f_j_0) AND (O_u_j_0 <=> (! V_j))
                //This formula has to be applied to all operations, both operations which are active and operations which are inactive
                List<variantOperations> lVariantsOperationsList = lFrameworkWrapper.getVariantsOperationsList();

                foreach (variantOperations lVariantOperations in lVariantsOperationsList)
                {
                    variant currentVariant = lFrameworkWrapper.ReturnCurrentVariant(lVariantOperations);

                    List<operation> lOperationList = lVariantOperations.getOperations();
                    if (lOperationList != null)
                    {
                        //operation lOperation = lOperationList[0];
                        foreach (operation lOperation in lOperationList)
                        {
                            /*BoolExpr lCurrentOpNPreCondition;
                            if (lOperation.precondition != null)
                                lCurrentOpNPreCondition = lZ3Solver.AndOperator(lOperation.names + "_I_" + currentVariant.index + "_0", lOperation.precondition[0].names + "_F_" + currentVariant.index + "_0");
                            else
                                lCurrentOpNPreCondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_I_" + currentVariant.index + "_0");

                            BoolExpr lFirstPart = lZ3Solver.TwoWayImpliesOperator(lCurrentOpNPreCondition, lZ3Solver.FindBoolExpressionUsingName(currentVariant.names));*/

                            BoolExpr lFirstPart = lZ3Solver.TwoWayImpliesOperator(lOperation.names + "_I_" + currentVariant.index + "_0", currentVariant.names);
                            BoolExpr lSecondPart = lZ3Solver.NotOperator(lOperation.names + "_E_" + currentVariant.index + "_0");
                            BoolExpr lThirdPart = lZ3Solver.NotOperator(lOperation.names + "_F_" + currentVariant.index + "_0");

                            BoolExpr lFirstOperand = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_U_" + currentVariant.index + "_0");
                            BoolExpr lFourthPart = lZ3Solver.TwoWayImpliesOperator(lFirstOperand, lZ3Solver.NotOperator(currentVariant.names));

                            lZ3Solver.AddAndOperator2Constraints(new List<BoolExpr>() { lFirstPart, lSecondPart, lThirdPart, lFourthPart }, "formula4-ActiveOperations");

                        }
                    }
                }

                /*List<variant> localVariantList = lFrameworkWrapper.VariantList;

                foreach (variant lVariant in localVariantList)
                {
                    variantOperations lVariantOperations = lFrameworkWrapper.getVariantExprOperations(lVariant.names);
                    if (lVariantOperations != null)
                    {
                        variant currentVariant = returnCurrentVariant(lVariantOperations);
                        List<operation> lOperationList = lVariantOperations.getOperations();
                        if (lOperationList != null)
                        {
                            //operation lOperation = lOperationList[0];
                            foreach (operation lOperation in lOperationList)
                            {
                 //////////////////////////////OLDER CODE///////////////////////////////////
                                BoolExpr lCurrentOpNPreCondition;
                                if (lOperation.precondition != null)
                                    lCurrentOpNPreCondition = lZ3Solver.AndOperator(lOperation.names + "_I_" + currentVariant.index + "_0", lOperation.precondition[0].names + "_F_" + currentVariant.index + "_0");
                                else
                                    lCurrentOpNPreCondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_I_" + currentVariant.index + "_0");

                                BoolExpr lFirstPart = lZ3Solver.TwoWayImpliesOperator(lCurrentOpNPreCondition, lZ3Solver.FindBoolExpressionUsingName(currentVariant.names));
                 //////////////////////////////OLDER CODE///////////////////////////////////

                                BoolExpr lFirstPart = lZ3Solver.TwoWayImpliesOperator(lOperation.names + "_I_" + currentVariant.index + "_0", currentVariant.names);
                                BoolExpr lSecondPart = lZ3Solver.NotOperator(lOperation.names + "_E_" + currentVariant.index + "_0");
                                BoolExpr lThirdPart = lZ3Solver.NotOperator(lOperation.names + "_F_" + currentVariant.index + "_0");

                                BoolExpr lFirstOperand = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_U_" + currentVariant.index + "_0");
                                BoolExpr lFourthPart = lZ3Solver.TwoWayImpliesOperator(lFirstOperand, lZ3Solver.NotOperator(currentVariant.names));

                                lZ3Solver.AddAndOperator2Constraints(new List<BoolExpr>() { lFirstPart, lSecondPart, lThirdPart, lFourthPart }, "formula4-ActiveOperations");

                            }
                        }
                    }
                }*/

                //For operations which are inactive all states should be false except the unused state
                List<String> lInActiveOperationNames = lFrameworkWrapper.InActiveOperationNamesList;
                foreach (String lInActiveOperationName in lInActiveOperationNames)
                {
                    //String[] lInActiveOperationParts = lInActiveOperation.Split('_');
                    //String lInActiveOperationName = lInActiveOperationParts[0];
                    BoolExpr lFirstPart = lZ3Solver.NotOperator(lInActiveOperationName + "_I_0_0");
                    BoolExpr lSecondPart = lZ3Solver.NotOperator(lInActiveOperationName + "_E_0_0");
                    BoolExpr lThirdPart = lZ3Solver.NotOperator(lInActiveOperationName + "_F_0_0");
                    BoolExpr lFourthPart = lZ3Solver.FindBoolExpressionUsingName(lInActiveOperationName + "_U_0_0");

                    lZ3Solver.AddAndOperator2Constraints(new List<BoolExpr>() { lFirstPart, lSecondPart, lThirdPart, lFourthPart }, "formula4-InactiveOperations");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in initializeFVariantOperation2Z3Constraints");
                Console.WriteLine(ex.Message);
            }
        }

        /*public void convertFOperations2Z3Constraint(int pState)
        {
            //Loop over variant list
            //For each variant find the variant in variant-operation mapping table
            //For each operation in this list make the following operation boolean variables
            List<variant> localVariantList = lFrameworkWrapper.getVariantList();

            //Next state of operation
            int lNewState = pState + 1;

            foreach (variant lVariant in localVariantList)
            {
                variantOperations lVariantOperations = lFrameworkWrapper.getVariantOperations(lVariant.names);
                if (lVariantOperations != null)
                {
                    variant currentVariant = lVariantOperations.getVariant();
                    List<operation> lOperationList = lVariantOperations.getOperations();
                    if (lOperationList != null)
                    {
                        foreach (operation lOperation in lOperationList)
                        {
                            //formula 5
                            //THIS ONLY WORKS IS THE PRECONDITION OF THE OPERATION IS ONLY ONE OTHER OPERATION!!!!!!!
                            BoolExpr tempLeftHandSide;
                            if (lOperation.precondition != null)
                                tempLeftHandSide = lZ3Solver.AndOperator(lOperation.names + "_I_" + currentVariant.index + "_" + pState.ToString(), lOperation.precondition[0] + "_F_" + currentVariant.index + "_" + pState.ToString());
                            else
                                tempLeftHandSide = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_I_" + currentVariant.index + "_" + pState.ToString());

                            /*BoolExpr tempRightHandSide = lZ3Solver.AndOperator(lZ3Solver.NotOperator(lOperation.names + "_I_" + currentVariant.index + "_" + lNewState.ToString()),
                                                                                 lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_E_" + currentVariant.index + "_" + lNewState.ToString()),
                                                                                 lZ3Solver.NotOperator(lOperation.names + "_F_" + currentVariant.index + "_" + lNewState.ToString()),
                                                                                 lZ3Solver.NotOperator(lOperation.names + "_U_" + currentVariant.index + "_" + lNewState.ToString()));
//                            lZ3Solver.AddImpliesOperator2Constraints(tempLeftHandSide, tempRightHandSide);
                            lZ3Solver.AddTwoWayImpliesOperator2Constraints(tempLeftHandSide, tempRightHandSide);

                            lZ3Solver.AddTwoWayImpliesOperator2Constraints(tempLeftHandSide, lZ3Solver.NotOperator(lOperation.names + "_I_" + currentVariant.index + "_" + lNewState.ToString()));
                            lZ3Solver.AddTwoWayImpliesOperator2Constraints(tempLeftHandSide, lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_E_" + currentVariant.index + "_" + lNewState.ToString()));
                            lZ3Solver.AddTwoWayImpliesOperator2Constraints(tempLeftHandSide, lZ3Solver.NotOperator(lOperation.names + "_F_" + currentVariant.index + "_" + lNewState.ToString()));
                            lZ3Solver.AddTwoWayImpliesOperator2Constraints(tempLeftHandSide, lZ3Solver.NotOperator(lOperation.names + "_U_" + currentVariant.index + "_" + lNewState.ToString()));

                        }
                    }
                }
            }

            //NEW FORMULA
            //This new formula was not part of the ETFA paper but was added after that
            foreach (variant lVariant in localVariantList)
            {
                variantOperations lVariantOperations = lFrameworkWrapper.getVariantOperations(lVariant.names);
                if (lVariantOperations != null)
                {
                    variant currentVariant = lVariantOperations.getVariant();
                    List<operation> lOperationList = lVariantOperations.getOperations();
                    if (lOperationList != null)
                    {
                        foreach (operation lOperation in lOperationList)
                        {
                            BoolExpr leftHandSide;
                            BoolExpr rightHandSide;
                            leftHandSide = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_E_" + currentVariant.index + "_" + pState.ToString());
                            rightHandSide = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_F_" + currentVariant.index + "_" + lNewState.ToString());

                            lZ3Solver.AddTwoWayImpliesOperator2Constraints(leftHandSide, rightHandSide);
                        }
                    }
                }
            }


            //formula 6
            //In the list of operations, start with operations indexed for variant 0 and compare them with all operations indexed with one more
            //For each pair (e.g. 0 and 1, 0 and 2,...) compare its current state with its new state on the operation_I

            BoolExpr formulaSix = null;

            //List<String> lActiveOperationNamesList = lFrameworkWrapper.getActiveOperationNamesList();
            List<String> lActiveOperationNamesList = lFrameworkWrapper.getActiveOperationNamesList(pState);
            if (lActiveOperationNamesList != null)
            {
                foreach (String lFirstActiveOperation in lActiveOperationNamesList)
                {
                    int lFirstVariantIndex = lFrameworkWrapper.getVariantIndexFromActiveOperation(lFirstActiveOperation);

                    foreach (String lSecondActiveOperation in lActiveOperationNamesList)
                    {
                        int lSecondVariantIndex = lFrameworkWrapper.getVariantIndexFromActiveOperation(lSecondActiveOperation);

                        if (lFirstVariantIndex < lSecondVariantIndex)
                        {
                            BoolExpr lFirstOperand = lZ3Solver.FindBoolExpressionUsingName(lFirstActiveOperation);
                            BoolExpr lSecondOperand = lZ3Solver.NotOperator(lFrameworkWrapper.giveNextStateActiveOperationName(lFirstActiveOperation));
                            BoolExpr lFirstParantesis = lZ3Solver.AndOperator(new List<BoolExpr>() {lFirstOperand, lSecondOperand});

                            BoolExpr lThirdOperand = lZ3Solver.FindBoolExpressionUsingName(lSecondActiveOperation);
                            String lNextStateActiveOperationName = lFrameworkWrapper.giveNextStateActiveOperationName(lSecondActiveOperation);
                            if (lZ3Solver.FindBoolExpressionUsingName(lNextStateActiveOperationName) != null)
                            {
                                BoolExpr lFourthOperand = lZ3Solver.NotOperator(lNextStateActiveOperationName);
                                BoolExpr lSecondParantesis = lZ3Solver.AndOperator(new List<BoolExpr>() {lThirdOperand, lFourthOperand});

                                if (formulaSix == null)
                                    formulaSix = lZ3Solver.NotOperator(lZ3Solver.AndOperator(new List<BoolExpr>() {lFirstParantesis, lSecondParantesis}));
                                else
                                    formulaSix = lZ3Solver.AndOperator(new List<BoolExpr>() {formulaSix, lZ3Solver.NotOperator(lZ3Solver.AndOperator(new List<BoolExpr>() {lFirstParantesis, lSecondParantesis}))});
                            }
                        }
                    }
                }
            }
            if (formulaSix != null)
                lZ3Solver.AddConstraintToSolver(formulaSix);
        }*/

        public void resetOperationPrecondition(operation pOperation, variant pVariant, int pState, string pConstraintSource)
        {
            //RUNA Code maybe to be able to take more than one constraint
            /*
            List<BoolExpr> preconditionList = new List<BoolExpr>();
            BoolExpr lConstraintExpr = null;
            lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_PreCondition_" + pVariant.index + "_" + pState.ToString());

             if (pOperation.precondition.Count != 0)
             {
                foreach (string precon in pOperation.precondition)
                {
                    //For each precondition first we have to build its coresponding tree
                    Parser lConditionParser = new Parser();
                    Node<string> lCnstExprTree = new Node<string>("root");

                    lConditionParser.AddChild(lCnstExprTree, precon);

                    foreach (Node<string> item in lCnstExprTree)
                    {
                        //Then we have to traverse the tree and call the appropriate Z3Solver functionalities
                        lConstraintExpr = ParseCondition(item, pState);
                    }
                    preconditionList.Add(lConstraintExpr);
                }

                lZ3Solver.AddTwoWayImpliesOperator2Constraints(lZ3Solver.AndOperator(preconditionList), lOpPrecondition, pConstraintSource);
            }
            else
                //If the operation DOES NOT have a precondition hence
                //We want to force the precondition to be true
                lZ3Solver.AddConstraintToSolver(lOpPrecondition, pConstraintSource + "-Precondition");
             */

            //MY Code
            //BoolExpr lOpPrecondition = lZ3Solver.MakeBoolVariable("lOpPrecondition");
            lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_PreCondition_" + pVariant.index + "_" + pState.ToString());

            if (pOperation.precondition.Count != 0)
            {
                //If the operation HAS a precondition
                if (lFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(pOperation.precondition[0]) > pState)
                {
                    //This means the precondition is on a transition state which has not been reached yet!
                    //lOpPrecondition = lZ3Solver.NotOperator(lOpPrecondition);
                    lZ3Solver.AddConstraintToSolver(lZ3Solver.NotOperator(lOpPrecondition), pConstraintSource);
                }
                else
                {
                    if (pOperation.precondition[0].Contains('_'))
                        //This means the precondition includes an operation status
                        //lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0] + "_" + currentVariant.index  + "_" + pState.ToString());
                        lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(pOperation.precondition[0]);
                    else
                        //This means the precondition only includes an operation
                        lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(pOperation.precondition[0] + "_F_" + pVariant.index + "_" + pState.ToString());
                }

            }
            else
                //If the operation DOES NOT have a precondition hence
                //We want to force the precondition to be true
                lZ3Solver.AddConstraintToSolver(lOpPrecondition, pConstraintSource);
        }

        public void resetOperationPostcondition(operation pOperation, variant pVariant, int pState, String pPostconditionSource)
        {
            //RUNA Code maybe to be able to have more than one constraint
            /*
            List<BoolExpr> postconditionList = new List<BoolExpr>();
            BoolExpr lConstraintExpr = null;
            lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_PostCondition_" + pVariant.index + "_" + pState.ToString());

            if (pOperation.postcondition.Count != 0)
            {
                foreach (string postcon in pOperation.postcondition)
                {
                    //For each precondition first we have to build its coresponding tree
                    Parser lConditionParser = new Parser();
                    Node<string> lCnstExprTree = new Node<string>("root");

                    lConditionParser.AddChild(lCnstExprTree, postcon);

                    foreach (Node<string> item in lCnstExprTree)
                    {
                        //Then we have to traverse the tree and call the appropriate Z3Solver functionalities
                        lConstraintExpr = ParseCondition(item, pState);
                    }
                    postconditionList.Add(lConstraintExpr);
                }

                lZ3Solver.AddTwoWayImpliesOperator2Constraints(lZ3Solver.AndOperator(postconditionList), lOpPostcondition, pPostconditionSource);


            }
            else
                //We want to force the postcondition to be true
                //lOpPostcondition = lOpPostcondition;
                lZ3Solver.AddConstraintToSolver(lOpPostcondition, pPostconditionSource + "-Postcondition");
             */
            lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_PostCondition_" + pVariant.index + "_" + pState.ToString());

            if (pOperation.postcondition.Count != 0)
            {
                if (lFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(pOperation.postcondition[0]) > pState)
                {
                    //This means the postcondition is on a transition state which has not been reached yet!
                    lOpPostcondition = lZ3Solver.NotOperator(lOpPostcondition);
                    lZ3Solver.AddConstraintToSolver(lOpPostcondition, pPostconditionSource);
                }
                else
                {
                    if (pOperation.postcondition[0].Contains('_'))
                        //This means the postcondition includes an operation status
                        //lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.postcondition[0] + "_" + currentVariant.index  + "_" + pState.ToString());
                        lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(pOperation.postcondition[0]);
                    else
                        //This means the postcondition only includes an operation
                        lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(pOperation.postcondition[0] + "_F_" + pVariant.index + "_" + pState.ToString());
                }

            }
            else
                //We want to force the postcondition to be true
                //lOpPostcondition = lOpPostcondition;
                lZ3Solver.AddConstraintToSolver(lOpPostcondition, pPostconditionSource);
        }

        //Build together pre/postcondition conponentes according to parse tree
        private BoolExpr ParseCondition(Node<string> pNode, int pState)
        {
            try
            {
                List<Node<string>> lChildren = new List<Node<string>>();
                BoolExpr lResult = null;
                if (pNode.Data.Contains("Possible"))
                {
                    lResult = lZ3Solver.MakeBoolExprFromString(pNode.Data);
                }
                else if ((pNode.Data != "and") && (pNode.Data != "or") && (pNode.Data != "not"))
                {
                    //We have one operator
                    ////lResult = pNode.Data;
                    lResult = mkCondition(pNode.Data, pState);
                }
                else
                {
                    foreach (Node<string> lChild in pNode.Children)
                    {
                        lChildren.Add(lChild);
                    }
                    switch (pNode.Data)
                    {
                        case "and":
                            {
                                lResult = lZ3Solver.AndOperator(new List<BoolExpr>() { ParseCondition(lChildren[0], pState), ParseCondition(lChildren[1], pState) });
                                break;
                            }
                        case "or":
                            {
                                lResult = lZ3Solver.OrOperator(new List<BoolExpr>() { ParseCondition(lChildren[0], pState), ParseCondition(lChildren[1], pState) });
                                break;
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                lResult = lZ3Solver.NotOperator(ParseCondition(lChildren[0], pState));
                                break;
                            }
                        default:
                            break;
                    }
                }
                return lResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //construct pre/postcondition conponents
        private BoolExpr mkCondition(string pCon, int pState)
        {
            String[] lOperationNameParts;
            //If the operation HAS a precondition
            //if (lFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(pOperation.precondition[0]) > pState)
            if (pCon.Contains('_'))
            {
                //This means the precondition includes more than an operation
                lOperationNameParts = pCon.Split('_');
                if (lOperationNameParts.Length != 4)
                {
                    //This means the precondition does not include a state
                    if (lOperationNameParts.Length == 2)
                    {
                        //This means the precondition does not include a variant nor a state
                        List<string> vInstances = lFrameworkWrapper.getvariantInstancesForOperation(lOperationNameParts[0]);
                        List<BoolExpr> opExpr = new List<BoolExpr>();
                        foreach (string variant in vInstances)
                        {
                            opExpr.Add(lZ3Solver.FindBoolExpressionUsingName(pCon + "_" + variant + "_" + pState.ToString()));

                        }
                        return(lZ3Solver.OrOperator(opExpr));
                    }
                    else if (lOperationNameParts.Length == 3)
                    {
                        //This means the precondition does includes a variant but not a state
                        return(lZ3Solver.FindBoolExpressionUsingName(pCon + "_" + pState.ToString()));
                    }
                    else
                        //This means the precondition only includes an operation
                        throw new System.ArgumentException("Precondition did not include a status", pCon);
                 }
                 else
                    //This means the precondition includes a state and a variant
                    if (lFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(pCon) > pState)
                    {
                        //This means the precondition is on a transition state which has not been reached yet!
                        //lZ3Solver.AddConstraintToSolver(lZ3Solver.NotOperator(lOpPrecondition), pPreconditionSource);
                        //unsatisfiable = true;
                        //break;
                        return lZ3Solver.getFalseBoolExpr();
                    }
                    else
                    {
                        return lZ3Solver.FindBoolExpressionUsingName(pCon);
                    }
            }
            else
                //This means the postcondition only includes an operation
                throw new System.ArgumentException("Precondition did not include a status", pCon);

        }

        //TODO: I think this can be removed!!!
        private variant returnCurrentVariant(variantOperations pVariantOperations)
        {
            variant lResultVariant = new variant();
            try
            {
                variant lCurrentVariant = lFrameworkWrapper.ReturnCurrentVariant(pVariantOperations);

                //TODO: this should be done here for ALL types of variants
                if (lCurrentVariant.names.Contains("Virtual"))
                {
                    //The result variant is going to be a virtual variant hence for this variant we have to add the needed operations
                    addVirtualVariantOperationInstances(lResultVariant, pVariantOperations.getOperations());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in returnCurrentVariant");
                Console.WriteLine(ex.Message);
            }
            return lResultVariant;
        }

        public void convertFOperations2Z3ConstraintNewVersion(int pState)
        {
            try
            {
                //Loop over the variant-operation mappings
                //For each mapping find the current variant and current operations
                List<variantOperations> lVariantOperationsList = lFrameworkWrapper.getVariantsOperationsList();

                //Next state of operation
                int lNewState = pState + 1;

                foreach (variantOperations lCurrentVariantOperations in lVariantOperationsList)
                {
                    variant lCurrentVariant = lFrameworkWrapper.ReturnCurrentVariant(lCurrentVariantOperations);

                    List<operation> lOperationList = lCurrentVariantOperations.getOperations();
                    if (lOperationList != null)
                    {
                        foreach (operation lOperation in lOperationList)
                        {
                            resetCurrentStateAndNewStateOperationVariables(lOperation, lCurrentVariant, pState, "formula5");

                            resetOperationPrecondition(lOperation, lCurrentVariant, pState, "formula5-Precondition");

                            ////TODO: check this line, it might be that it is not needed considering that post conditions are set as part of the previous method.
                            BoolExpr lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_PostCondition_" + lCurrentVariant.index + "_" + pState.ToString());

                            //(O_I_k_j and Pre_k_j) => O_E_k_j+1
                            createFormula51();

                            // not (O_I_k_j and Pre_k_j) => (O_I_k_j <=> O_I_k_j+1)
                            createFormula52(lOperation);

                            //for this XOR O_I_k_j O_E_k_j O_F_k_j O_U_k_j
                            //We should show
                            //or O_I_k_j O_E_k_j O_F_k_j O_U_k_j
                            //and (=> O_I_k_j (and (not O_E_k_j) (not O_F_k_j) (not O_U_k_j)))
                            //    (=> O_E_k_j (and (not O_I_k_j) (not O_F_k_j) (not O_U_k_j)))
                            //    (=> O_F_k_j (and (not O_I_k_j) (not O_E_k_j) (not O_U_k_j)))
                            //    (=> O_U_k_j (and (not O_I_k_j) (not O_E_k_j) (not O_F_k_j)))
                            createFormula53();

                            //(O_E_k_j AND Post_k_j) => O_F_k_j+1
                            createFormula54(pState, lOperation, lCurrentVariant);

                            //O_U_k_j => O_U_k_j+1
                            createFormula56();

                            //O_F_k_j => O_F_k_j+1
                            createFormula57();

//                            //for all variants k, (O_I_k_j and Pre_k_j) => O_E_k_j+1
//                            createFormula58();

                            //formula 6
                            //In the list of operations, start with operations indexed for variant 0 and compare them with all operations indexed with one more
                            //For each pair (e.g. 0 and 1, 0 and 2,...) compare its current state with its new state on the operation_I
                            createFormula6(pState);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertFOperations2Z3ConstraintNewVersion");
                Console.WriteLine(ex.Message);
            }
        }


        private void createFormula6(int pState)
        {
            try
            {
                //formula 6
                //In the list of operations, start with operations indexed for variant 0 and compare them with all operations indexed with one more
                //For each pair (e.g. 0 and 1, 0 and 2,...) compare its current state with its new state on the operation_I

                BoolExpr formulaSix = null;

                //List<String> lActiveOperationList = lFrameworkWrapper.getActiveOperationList();
                List<String> lActiveOperationList = lFrameworkWrapper.getActiveOperationNamesList(pState);
                if (lActiveOperationList != null)
                {
                    foreach (String lFirstActiveOperation in lActiveOperationList)
                    {
                        int lFirstVariantIndex = lFrameworkWrapper.getVariantIndexFromActiveOperation(lFirstActiveOperation);

                        foreach (String lSecondActiveOperation in lActiveOperationList)
                        {
                            int lSecondVariantIndex = lFrameworkWrapper.getVariantIndexFromActiveOperation(lSecondActiveOperation);

                            if (lFirstVariantIndex < lSecondVariantIndex)
                            {
                                BoolExpr lFirstOperand = lZ3Solver.FindBoolExpressionUsingName(lFirstActiveOperation);
                                BoolExpr lSecondOperand = lZ3Solver.NotOperator(lFrameworkWrapper.giveNextStateActiveOperationName(lFirstActiveOperation));
                                BoolExpr lFirstParantesis = lZ3Solver.AndOperator(new List<BoolExpr>() { lFirstOperand, lSecondOperand });

                                BoolExpr lThirdOperand = lZ3Solver.FindBoolExpressionUsingName(lSecondActiveOperation);
                                String lNextStateActiveOperationName = lFrameworkWrapper.giveNextStateActiveOperationName(lSecondActiveOperation);
                                if (lZ3Solver.FindBoolExpressionUsingName(lNextStateActiveOperationName) != null)
                                {
                                    BoolExpr lFourthOperand = lZ3Solver.NotOperator(lNextStateActiveOperationName);
                                    BoolExpr lSecondParantesis = lZ3Solver.AndOperator(new List<BoolExpr>() { lThirdOperand, lFourthOperand });

                                    if (formulaSix == null)
                                        formulaSix = lZ3Solver.NotOperator(lZ3Solver.AndOperator(new List<BoolExpr>() { lFirstParantesis, lSecondParantesis }));
                                    else
                                        formulaSix = lZ3Solver.AndOperator(new List<BoolExpr>() { formulaSix, lZ3Solver.NotOperator(lZ3Solver.AndOperator(new List<BoolExpr>() { lFirstParantesis, lSecondParantesis })) });
                                }
                            }
                        }
                    }
                }
                if (formulaSix != null)
                    lZ3Solver.AddConstraintToSolver(formulaSix, "formula6");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateFormula6");
                Console.WriteLine(ex.Message);
            }
        }

        /*private void createFormula58()
        {
            try
            {
                //Formula 5.8
                //for all variants k, (O_I_k_j and Pre_k_j) => O_E_k_j+1
                BoolExpr temp58Expr = null;
                List<variant> localVariantList = lFrameworkWrapper.VariantList;

                foreach (variant lVariant in localVariantList)
                {
                    variantOperations lVariantOperations = lFrameworkWrapper.getVariantOperations(lVariant.names);
                    if (lVariantOperations != null)
                    {
                        variant currentVariant = lVariantOperations.getVariant();
                        List<operation> lOperationList = lVariantOperations.getOperations();
                        if (lOperationList != null)
                        {
                            foreach (operation lOperation in lOperationList)
                            {
                                resetCurrentStateAndNewStateOperationVariables(lOperation, currentVariant, 0, "formula5.8");

                                //Making formula:
                                //for all variants k, (O_I_k_j and Pre_k_j) => O_E_k_j+1
                                BoolExpr tempLeftHandSide;
                                tempLeftHandSide = ReturnOperationInitialStateNItsPrecondition();
                                BoolExpr tempExpr4CurrentOperation;
                                tempExpr4CurrentOperation = lZ3Solver.ImpliesOperator(new List<BoolExpr>() { tempLeftHandSide, lOp_E_NextState });
                                if (temp58Expr == null)
                                    temp58Expr = tempExpr4CurrentOperation;
                                else
                                    temp58Expr = lZ3Solver.XorOperator(new List<BoolExpr>() { temp58Expr, tempExpr4CurrentOperation });
                            }
                        }
                    }
                }

                lZ3Solver.AddConstraintToSolver(temp58Expr, "formula5.8");

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateFormula58");
                Console.WriteLine(ex.Message);
            }
        }*/

        private void createFormula57()
        {
            try
            {
                //Formula 5.7
                //O_F_k_j => O_F_k_j+1
                lZ3Solver.AddImpliesOperator2Constraints(lOp_F_CurrentState, lOp_F_NextState, "formula5.7");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateFormula57");
                Console.WriteLine(ex.Message);
            }
        }

        private void createFormula56()
        {
            try
            {
                //Formula 5.6
                //O_U_k_j => O_U_k_j+1
                lZ3Solver.AddTwoWayImpliesOperator2Constraints(lOp_U_CurrentState, lOp_U_NextState, "formula5.6");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateFormula56");
                Console.WriteLine(ex.Message);
            }
        }

        private void createFormula54(int pState, operation pOperation, variant pCurrentVariant)
        {
            try
            {
                //Formula 5.4
                //(O_E_k_j AND Post_k_j) => O_F_k_j+1

                ////TODO:Check this line, it might be the fact that this line is not needed as the post conditions are set outside this method in the mother method!!
                resetOperationPostcondition(pOperation, pCurrentVariant, pState, "formula5.4");

                BoolExpr lLeftHandSide = lZ3Solver.AndOperator(new List<BoolExpr>() { lOp_E_CurrentState, lOpPostcondition });
                lZ3Solver.AddImpliesOperator2Constraints(lLeftHandSide, lOp_F_NextState, "formula5.4");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateFormula54");
                Console.WriteLine(ex.Message);
            }
        }

        private void createFormula53()
        {
            try
            {
                //Fromula 5.3
                //for this XOR O_I_k_j O_E_k_j O_F_k_j O_U_k_j
                //We should show
                //or O_I_k_j O_E_k_j O_F_k_j O_U_k_j
                //and (=> O_I_k_j (and (not O_E_k_j) (not O_F_k_j) (not O_U_k_j)))
                //    (=> O_E_k_j (and (not O_I_k_j) (not O_F_k_j) (not O_U_k_j)))
                //    (=> O_F_k_j (and (not O_I_k_j) (not O_E_k_j) (not O_U_k_j)))
                //    (=> O_U_k_j (and (not O_I_k_j) (not O_E_k_j) (not O_F_k_j)))

                lZ3Solver.AddOrOperator2Constraints(new List<BoolExpr>() { lOp_I_CurrentState, lOp_E_CurrentState, lOp_F_CurrentState, lOp_U_CurrentState }, "formula5.3-Firstpart");

                BoolExpr lFirstPart = lZ3Solver.ImpliesOperator(new List<BoolExpr>() { lOp_I_CurrentState
                                                                , lZ3Solver.AndOperator(new List<BoolExpr>() {lZ3Solver.NotOperator(lOp_E_CurrentState)
                                                                                                    , lZ3Solver.NotOperator(lOp_F_CurrentState)
                                                                                                    , lZ3Solver.NotOperator(lOp_U_CurrentState)})});
                BoolExpr lSecondPart = lZ3Solver.ImpliesOperator(new List<BoolExpr>() { lOp_E_CurrentState
                                                                , lZ3Solver.AndOperator(new List<BoolExpr>() {lZ3Solver.NotOperator(lOp_I_CurrentState)
                                                                                                    , lZ3Solver.NotOperator(lOp_F_CurrentState)
                                                                                                    , lZ3Solver.NotOperator(lOp_U_CurrentState)})});
                BoolExpr lThirdPart = lZ3Solver.ImpliesOperator(new List<BoolExpr>() { lOp_F_CurrentState
                                                                , lZ3Solver.AndOperator(new List<BoolExpr>() {lZ3Solver.NotOperator(lOp_I_CurrentState)
                                                                                                    , lZ3Solver.NotOperator(lOp_E_CurrentState)
                                                                                                    , lZ3Solver.NotOperator(lOp_U_CurrentState)})});
                BoolExpr lFourthPart = lZ3Solver.ImpliesOperator(new List<BoolExpr>() { lOp_U_CurrentState
                                                                , lZ3Solver.AndOperator(new List<BoolExpr>() {lZ3Solver.NotOperator(lOp_I_CurrentState)
                                                                                                    , lZ3Solver.NotOperator(lOp_E_CurrentState)
                                                                                                    , lZ3Solver.NotOperator(lOp_F_CurrentState)})});

                lZ3Solver.AddAndOperator2Constraints(new List<BoolExpr>() { lFirstPart, lSecondPart, lThirdPart, lFourthPart }, "formula5.3-Secondpart");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateFormula53");
                Console.WriteLine(ex.Message);
            }
        }


        private void createFormula53GeneralVersion()
        {
            try
            {
                //Fromula 5.3
                //for this XOR O_I_k_j O_E_k_j O_F_k_j O_U_k_j

                lZ3Solver.AddXorOperator2Constraints(new List<BoolExpr>() { lOp_I_CurrentState, lOp_E_CurrentState, lOp_F_CurrentState, lOp_U_CurrentState }, "formula5.3");

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateFormula53");
                Console.WriteLine(ex.Message);
            }
        }

        private void createFormula52(operation pOperation)
        {
            try
            {
                //Formula 5.2
                // not (O_I_k_j and Pre_k_j) => (O_I_k_j <=> O_I_k_j+1)
                BoolExpr tempLeftHandSideTwo;

                tempLeftHandSideTwo = ReturnOperationInitialStateNItsPrecondition(pOperation);

                tempLeftHandSideTwo = lZ3Solver.NotOperator(tempLeftHandSideTwo);

                BoolExpr tempRightHandSideTwo;

                tempRightHandSideTwo = lZ3Solver.TwoWayImpliesOperator(lOp_I_CurrentState, lOp_I_NextState);

                lZ3Solver.AddImpliesOperator2Constraints(tempLeftHandSideTwo, tempRightHandSideTwo, "formula5.2");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateFormula52");
                Console.WriteLine(ex.Message);
            }
        }

        private void createFormula51()
        {
            try
            {
                //Formula 5.1
                //(O_I_k_j and Pre_k_j) => O_E_k_j+1
                BoolExpr tempLeftHandSideOne;
                //if (lOperation.precondition != null)
                tempLeftHandSideOne = lZ3Solver.AndOperator(new List<BoolExpr>() { lOp_I_CurrentState, lOpPrecondition });
                //else
                //    tempLeftHandSideOne = lOp_I_CurrentState;

                lZ3Solver.AddImpliesOperator2Constraints(tempLeftHandSideOne, lOp_E_NextState, "formula5.1");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateFormula51");
                Console.WriteLine(ex.Message);
            }
        }

        public BoolExpr ReturnOperationInitialStateNItsPrecondition(operation pOperation)
        {
            BoolExpr result;
            try
            {
                //If the operation has no precondition then the relevant variable is forced to be true, other wise it will be the precondition
                if (pOperation.precondition.Count != 0)
                    result = lZ3Solver.AndOperator(new List<BoolExpr>() { lOp_I_CurrentState, lOpPrecondition });
                else
                    result = lOp_I_CurrentState;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationInitialStateNItsPrecondition");
                Console.WriteLine(ex.Message);
                result = null;
            }
            return result;
        }

        //TODO: We need enum for operators to be used here
        public BoolExpr ParseConstraintExpression(Node<string> pNode)
        {
            BoolExpr lResult = null;
            try
            {
                if ((pNode.Data != "and") 
                    && (pNode.Data != "or") 
                    && (pNode.Data != "not")
                    && (pNode.Data != "->")
/*                    && (pNode.Data != "<")
                    && (pNode.Data != ">")
                    && (pNode.Data != "<=")
                    && (pNode.Data != ">=")
                    && (pNode.Data != "==")*/
                    )
                {
                    //We have one operator
                    ////lResult = pNode.Data;
                    lResult = (BoolExpr)lZ3Solver.FindBoolExpressionUsingName(pNode.Data);
                }
                else
                {
                    List<Node<string>> lChildren = new List<Node<string>>();
                    foreach (Node<string> lChild in pNode.Children)
                    {
                        lChildren.Add(lChild);
                    }
                    switch (pNode.Data)
                    {
                        case "and":
                            {
                                ////lResult = lZ3Solver.AndOperator(ParseConstraint(lChildren[0]), ParseConstraint(lChildren[1])).ToString();
                                lResult = lZ3Solver.AndOperator(new List<BoolExpr>() { ParseConstraintExpression(lChildren[0]), ParseConstraintExpression(lChildren[1]) });
                                break;
                            }
                        case "or":
                            {
                                ////lResult = lZ3Solver.OrOperator(ParseConstraint(lChildren[0]), ParseConstraint(lChildren[1])).ToString();
                                //lResult = lZ3Solver.OrOperator(ParseConstraint(lChildren[0]), ParseConstraint(lChildren[1]));
                                lResult = lZ3Solver.OrOperator(new List<BoolExpr>() { ParseConstraintExpression(lChildren[0]), ParseConstraintExpression(lChildren[1]) });
                                break;
                            }
                        case "->":
                            {
                                lResult = lZ3Solver.ImpliesOperator(new List<BoolExpr>() { ParseConstraintExpression(lChildren[0]), ParseConstraintExpression(lChildren[1]) });
                                break;
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                lResult = lZ3Solver.NotOperator(ParseConstraintExpression(lChildren[0]));
                                break;
                            }
/*                        case ">=":
                            {
                                lResult = lZ3Solver.GreaterOrEqualOperator(lChildren[0].Data, int.Parse(lChildren[1].Data));
                                break;
                            }
                        case "<=":
                            {
                                lResult = lZ3Solver.LessOrEqualOperator(lChildren[0].Data, int.Parse(lChildren[1].Data));
                                break;
                            }
                        case "<":
                            {
                                lResult = lZ3Solver.LessThanOperator(lChildren[0].Data, int.Parse(lChildren[1].Data));
                                break;
                            }
                        case ">":
                            {
                                lResult = lZ3Solver.GreaterThanOperator(lChildren[0].Data, int.Parse(lChildren[1].Data));
                                break;
                            }*/
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ParseExpression");
                Console.WriteLine(ex.Message);
            }
            return lResult;
        }

        //TODO: We need an enum for operators which should be used here
        public BoolExpr ParseExpression(Node<string> pNode)
        {
            BoolExpr lResult = null;
            try
            {
                if ((pNode.Data != "and")
                    && (pNode.Data != "or")
                    && (pNode.Data != "not")
                    && (pNode.Data != "<")
                    && (pNode.Data != ">")
                    && (pNode.Data != "<=")
                    && (pNode.Data != ">=")
                    && (pNode.Data != "==")
                    )
                {
                    //We have one operator
                    ////lResult = pNode.Data;
                    lResult = (BoolExpr)lZ3Solver.FindExpressionUsingName(pNode.Data);
                }
                else
                {
                    List<Node<string>> lChildren = new List<Node<string>>();
                    foreach (Node<string> lChild in pNode.Children)
                    {
                        lChildren.Add(lChild);
                    }
                    switch (pNode.Data)
                    {
                        case "and":
                            {
                                ////lResult = lZ3Solver.AndOperator(ParseConstraint(lChildren[0]), ParseConstraint(lChildren[1])).ToString();
                                lResult = lZ3Solver.AndOperator(new List<BoolExpr>() { ParseExpression(lChildren[0]), ParseExpression(lChildren[1]) });
                                break;
                            }
                        case "or":
                            {
                                ////lResult = lZ3Solver.OrOperator(ParseConstraint(lChildren[0]), ParseConstraint(lChildren[1])).ToString();
                                //lResult = lZ3Solver.OrOperator(ParseConstraint(lChildren[0]), ParseConstraint(lChildren[1]));
                                lResult = lZ3Solver.OrOperator(new List<BoolExpr>() { ParseExpression(lChildren[0]), ParseExpression(lChildren[1]) });
                                break;
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                lResult = lZ3Solver.NotOperator(ParseExpression(lChildren[0]));
                                break;
                            }
                        case ">=":
                            {
                                lResult = lZ3Solver.GreaterOrEqualOperator(lChildren[0].Data, int.Parse(lChildren[1].Data));
                                break;
                            }
                        case "<=":
                            {
                                lResult = lZ3Solver.LessOrEqualOperator(lChildren[0].Data, int.Parse(lChildren[1].Data));
                                break;
                            }
                        case "<":
                            {
                                lResult = lZ3Solver.LessThanOperator(lChildren[0].Data, int.Parse(lChildren[1].Data));
                                break;
                            }
                        case ">":
                            {
                                lResult = lZ3Solver.GreaterThanOperator(lChildren[0].Data, int.Parse(lChildren[1].Data));
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ParseExpression");
                Console.WriteLine(ex.Message);
            }
            return lResult;
        }

        public void convertFConstraint2Z3Constraint()
        {
            try
            {
                //formula 3
                //First we have to loop the constraint list
                List<string> localConstraintList = lFrameworkWrapper.ConstraintList;

                foreach (string lConstraint in localConstraintList)
                    lZ3Solver.AddConstraintToSolver(returnFBooleanExpression2Z3Constraint(lConstraint)
                                                    , "formula3");

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertFConstraint2Z3Constraint");
                Console.WriteLine(ex.Message);
            }
        }

        private BoolExpr returnFExpression2Z3Constraint(string pExpression)
        {
            BoolExpr lResultExpr = null;
            try
            {
                //For each expression first we have to build its coresponding tree
                Parser lExpressionParser = new Parser();
                Node<string> lExprTree = new Node<string>("root");

                lExpressionParser.AddChild(lExprTree, pExpression);

                foreach (Node<string> item in lExprTree)
                {
                    //Then we have to traverse the tree and call the appropriate Z3Solver functionalities
                    lResultExpr = ParseExpression(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addFExpression2Z3Constraint");
                Console.WriteLine(ex.Message);
            }
            return lResultExpr;
        }

        private BoolExpr returnFBooleanExpression2Z3Constraint(string pExpression)
        {
            BoolExpr lResultExpr = null;
            try
            {
                //For each expression first we have to build its coresponding tree
                Parser lExpressionParser = new Parser();
                Node<string> lExprTree = new Node<string>("root");

                lExpressionParser.AddChild(lExprTree, pExpression);

                foreach (Node<string> item in lExprTree)
                {
                    //Then we have to traverse the tree and call the appropriate Z3Solver functionalities
                    lResultExpr = ParseConstraintExpression(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addFExpression2Z3Constraint");
                Console.WriteLine(ex.Message);
            }
            return lResultExpr;
        }

        /*
        public BoolExpr createFormula7(List<variant> pVariantList, int pState)
        {
            //NEW formula 7
            //(Big And) ((! Pre_k_j AND O_I_k_j) OR (O_E_k_j => Post_k_j))

            BoolExpr lResultFormula7 = null;

            foreach (variant lVariant in pVariantList)
            {
                variantOperations lVariantOperations = lFrameworkWrapper.getVariantOperations(lVariant.names);
                if (lVariantOperations != null)
                {
                    variant currentVariant = lVariantOperations.getVariant();
                    List<operation> lOperationList = lVariantOperations.getOperations();
                    if (lOperationList != null)
                    {
                        foreach (operation lOperation in lOperationList)
                        {
                            resetCurrentStateAndNewStateOperationVariables(lOperation, currentVariant, pState, "formula7");

                            BoolExpr lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_PreCondition_" + currentVariant.index + "_" + pState.ToString());
                            BoolExpr lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_PostCondition_" + currentVariant.index + "_" + pState.ToString());
                            //String [] lOperationNameParts = new String[4];
                            //if (lOperation.precondition.Count != 0)
                            //{
                            //    if (lFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(lOperation.precondition[0]) > pState)
                            //        //This means the precondition is on a transition state which has not been reached yet!
                            //        //lOpPrecondition = lZ3Solver.NotOperator(lOpPrecondition);
                            //        lZ3Solver.AddConstraintToSolver(lZ3Solver.NotOperator(lOpPrecondition), "formula7-Precondition");
                            //    else
                            //    {
                            //        if (lOperation.precondition[0].Contains('_'))
                            //        {
                            //            //This means the precondition includes more than an operation
                            //            lOperationNameParts = lOperation.precondition[0].Split('_');
                            //            if (lOperationNameParts.Length == 3)
                            //            {
                            //                //This means the precondition does not include a state
                            //                if (lOperationNameParts.Length == 2)
                            //                    //This means the precondition does not include a variant nor a state
                            //                    //Needs to be properly implementet
                            //                    lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0] + currentVariant.index + "_" + pState.ToString());
                            //                else
                            //                    //This means the precondition does includes a variant but not a state
                            //                    lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0] + "_" + pState.ToString());
                            //            }
                            //            else
                            //                //This means the precondition includes a state
                            //                lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0]);
                            //        }
                            //        else
                            //            //This means the precondition only includes an operation
                            //            lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0] + "_F_" + currentVariant.index + "_" + pState.ToString());
                            //    }
                            //}
                            //else
                            //    //lOpPrecondition = lOpPrecondition;
                            //    lZ3Solver.AddConstraintToSolver(lOpPrecondition, "formula7-Precondition");

                            //if (lOperation.postcondition.Count != 0)
                            //{
                            //    if (lFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(lOperation.postcondition[0]) > pState)
                            //        //This means the postcondition is on a transition state which has not been reached yet!
                            //        lZ3Solver.AddConstraintToSolver(lZ3Solver.NotOperator(lOpPostcondition), "formula7-Postcondition");
                            //    else
                            //    {
                            //        if (lOperation.postcondition[0].Contains('_'))
                            //        {
                            //            //This means the postcondition includes more than an operation
                            //            lOperationNameParts = lOperation.postcondition[0].Split('_');
                            //            if (lOperationNameParts.Length == 3)
                            //            {
                            //                //This means the postcondition does not include a state
                            //                if (lOperationNameParts.Length == 2)
                            //                    //This means the postcondition does not include a variant nor a state
                            //                    //Needs to be properly implementet
                            //                    lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.postcondition[0] + currentVariant.index + "_" + pState.ToString());
                            //                else
                            //                    //This means the postcondition does includes a variant but not a state
                            //                    lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.postcondition[0] + "_" + pState.ToString());
                            //            }
                            //            else
                            //                //This means the postcondition includes a state
                            //                lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.postcondition[0]);
                            //        }
                            //        else
                            //            //This means the postcondition only includes an operation
                            //            lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.postcondition[0] + "_F_" + currentVariant.index + "_" + pState.ToString());
                            //        //Before it was this
                            //        //lOperationPrecondition = lZ3Solver.FindBoolExpressionUsingName(lPrecondition + "_I_" + currentVariant.index + "_" + pState.ToString());
                            //    }
                            //    //if (lFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(lOperation.postcondition[0]) > pState)
                            //    //    //This means the precondition is on a transition state which has not been reached yet!
                            //    //    //lOpPostcondition = lZ3Solver.NotOperator(lOpPostcondition);
                            //    //    lZ3Solver.AddConstraintToSolver(lZ3Solver.NotOperator(lOpPostcondition), "formula7-Postcondition");
                            //    //else
                            //    //{
                            //    //    if (lOperation.postcondition[0].Contains('_'))
                            //    //        //This means the precondition includes an operation status
                            //    //        lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.postcondition[0]);
                            //    //    else
                            //    //        //This means the precondition only includes an operation
                            //    //        lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.postcondition[0] + "_F_" + currentVariant.index + "_" + pState.ToString());
                            //    //    //Before it was this
                            //    //    //lOperationPrecondition = lZ3Solver.FindBoolExpressionUsingName(lPrecondition + "_I_" + currentVariant.index + "_" + pState.ToString());
                            //    //}
                            //}
                            //else
                            //    //lOpPostcondition = lOpPostcondition;
                            //    lZ3Solver.AddConstraintToSolver(lOpPostcondition, "formula7-Postcondition");
                            BoolExpr lNotPreCondition = lZ3Solver.NotOperator(lOpPrecondition);
                            BoolExpr lNotPostCondition = lZ3Solver.NotOperator(lOpPostcondition);

                            BoolExpr lFirstOperand = lZ3Solver.AndOperator(new List<BoolExpr>() { lNotPreCondition, lOp_I_CurrentState });
                            BoolExpr lSecondOperand = lZ3Solver.AndOperator(new List<BoolExpr>() { lNotPostCondition, lOp_E_CurrentState });

                            BoolExpr lOperand = lZ3Solver.OrOperator(new List<BoolExpr>() { lFirstOperand, lSecondOperand, lOp_F_CurrentState, lOp_U_CurrentState });

                            if (lResultFormula7 == null)
                                lResultFormula7 = lOperand;
                            else
                                lResultFormula7 = lZ3Solver.AndOperator(new List<BoolExpr>() { lResultFormula7, lOperand });
                        }
                    }
                }
            }
            //            if (formula7 != null)
            //                lZ3Solver.AddConstraintToSolver(formula7);
            return lResultFormula7;
        }*/

        public void resetCurrentStateOperationVariables(operation pOperation, variant pVariant, int pState)
        {
            lOp_I_CurrentState = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_I_" + pVariant.index + "_" + pState.ToString());
            lOp_E_CurrentState = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_E_" + pVariant.index + "_" + pState.ToString());
            lOp_F_CurrentState = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_F_" + pVariant.index + "_" + pState.ToString());
            lOp_U_CurrentState = lZ3Solver.FindBoolExpressionUsingName(pOperation.names + "_U_" + pVariant.index + "_" + pState.ToString());
        }

        public BoolExpr createFormula7(List<variant> pVariantList, int pState)
        {
            //NEW formula 7
            //No operation can proceed
            //(Big And) ((! Pre_k_j AND O_I_k_j) OR (! Post_k_j AND O_E_k_j) OR O_F_k_j OR O_U_k_j)

            BoolExpr lResultFormula7 = null;

            foreach (variant lCurrentVariant in pVariantList)
            {
                List<operation> lOperationList = lFrameworkWrapper.getVariantExprOperations(lCurrentVariant.names);
                if (lOperationList != null)
                {
                    foreach (operation lOperation in lOperationList)
                    {
                        resetCurrentStateOperationVariables(lOperation, lCurrentVariant, pState);

                        BoolExpr lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_PreCondition_" + lCurrentVariant.index + "_" + pState.ToString());
                        BoolExpr lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_PostCondition_" + lCurrentVariant.index + "_" + pState.ToString());

                        if (lOperation.precondition.Count != 0)
                        {
                            if (lFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(lOperation.precondition[0]) > pState)
                                //This means the precondition is on a transition state which has not been reached yet!
                                //lOpPrecondition = lZ3Solver.NotOperator(lOpPrecondition);
                                lZ3Solver.AddConstraintToSolver(lZ3Solver.NotOperator(lOpPrecondition), "formula7-Precondition");
                            else
                            {
                                if (lOperation.precondition[0].Contains('_'))
                                    //This means the precondition includes an operation status
                                    lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0]);
                                else
                                    //This means the precondition only includes an operation
                                    lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0] + "_F_" + lCurrentVariant.index + "_" + pState.ToString());
                                //Before it was this
                                //lOperationPrecondition = lZ3Solver.FindBoolExpressionUsingName(lPrecondition + "_I_" + currentVariant.index + "_" + pState.ToString());
                            }
                        }
                        else
                            //lOpPrecondition = lOpPrecondition;
                            lZ3Solver.AddConstraintToSolver(lOpPrecondition, "formula7-Precondition");

                        if (lOperation.postcondition.Count != 0)
                        {
                            if (lFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(lOperation.postcondition[0]) > pState)
                                //This means the precondition is on a transition state which has not been reached yet!
                                //lOpPostcondition = lZ3Solver.NotOperator(lOpPostcondition);
                                lZ3Solver.AddConstraintToSolver(lZ3Solver.NotOperator(lOpPostcondition), "formula7-Postcondition");
                            else
                            {
                                if (lOperation.postcondition[0].Contains('_'))
                                    //This means the precondition includes an operation status
                                    lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.postcondition[0]);
                                else
                                    //This means the precondition only includes an operation
                                    lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.postcondition[0] + "_F_" + lCurrentVariant.index + "_" + pState.ToString());
                                //Before it was this
                                //lOperationPrecondition = lZ3Solver.FindBoolExpressionUsingName(lPrecondition + "_I_" + currentVariant.index + "_" + pState.ToString());
                            }
                        }
                        else
                            //lOpPostcondition = lOpPostcondition;
                            lZ3Solver.AddConstraintToSolver(lOpPostcondition, "formula7-Postcondition");
                        BoolExpr lNotPreCondition = lZ3Solver.NotOperator(lOpPrecondition);
                        BoolExpr lNotPostCondition = lZ3Solver.NotOperator(lOpPostcondition);

                        BoolExpr lFirstOperand = lZ3Solver.AndOperator(new List<BoolExpr>() { lNotPreCondition, lOp_I_CurrentState });
                        BoolExpr lSecondOperand = lZ3Solver.AndOperator(new List<BoolExpr>() { lNotPostCondition, lOp_E_CurrentState });

                        BoolExpr lOperand = lZ3Solver.OrOperator(new List<BoolExpr>() { lFirstOperand, lSecondOperand, lOp_F_CurrentState, lOp_U_CurrentState });

                        if (lResultFormula7 == null)
                            lResultFormula7 = lOperand;
                        else
                            lResultFormula7 = lZ3Solver.AndOperator(new List<BoolExpr>() { lResultFormula7, lOperand });
                    }
                }
            }
            //            if (formula7 != null)
            //                lZ3Solver.AddConstraintToSolver(formula7);
            return lResultFormula7;
        }

        public BoolExpr createFormula8(List<variant> pVariantList, int pState)
        {
            //formula 8
            //At least one operation is in initial or executing state
            //(Big OR) (O_I_k_j OR O_E_k_j)
            BoolExpr lResultFormula8 = null;

            foreach (variant lCurrentVariant in pVariantList)
            {
                List<operation> lOperationList = lFrameworkWrapper.getVariantExprOperations(lCurrentVariant.names);
                if (lOperationList != null)
                {
                    foreach (operation lOperation in lOperationList)
                    {
                        BoolExpr lOp_I_CurrentState = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_I_" + lCurrentVariant.index + "_" + pState.ToString());
                        BoolExpr lOp_E_CurrentState = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_E_" + lCurrentVariant.index + "_" + pState.ToString());

                        BoolExpr lOperand = lZ3Solver.OrOperator(new List<BoolExpr>() { lOp_I_CurrentState, lOp_E_CurrentState });

                        if (lResultFormula8 == null)
                            lResultFormula8 = lOperand;
                        else
                            lResultFormula8 = lZ3Solver.OrOperator(new List<BoolExpr>() { lResultFormula8, lOperand });
                    }
                }
            }
            //            if (formula8 != null)
            //                lZ3Solver.AddConstraintToSolver(formula8);
            return lResultFormula8;
        }

        public void convertFGoals2Z3GoalsVersion2(int pState)
        {
            try
            {
                BoolExpr lOverallGoal = null;

                //This boolean expression is used to refer to this overall goal
                lZ3Solver.AddBooleanExpression("P" + pState);

                List<variant> localVariantList = lFrameworkWrapper.VariantList;

                //(Big And) ((! Pre_k_j AND O_I_k_j) OR (! Post_k_j AND O_E_k_j) OR O_F_k_j OR O_U_k_j)
                BoolExpr lFormula7 = createFormula7(localVariantList, pState);
                //(Big OR) (O_I_k_j OR O_E_k_j)
                BoolExpr lFormula8 = createFormula8(localVariantList, pState);


                lOverallGoal = lZ3Solver.AndOperator(new List<BoolExpr>() { lFormula7, lFormula8 });
                if (lOverallGoal != null)
                    //lZ3Solver.AddConstraintToSolver(lOverallGoal, "overallGoal");
                    lZ3Solver.AddImpliesOperator2Constraints(lZ3Solver.FindBoolExpressionUsingName("P" + pState)
                                                                , lOverallGoal
                                                                , "overallGoal");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in convertFGoals2Z3GoalsVersion2");
                Console.WriteLine(ex.Message);
            }
        }

        public void convertFGoals2Z3GoalsVersion1(int pState)
        {
            BoolExpr lOverallGoal = null;


            //formula 7

            BoolExpr formula7 = null;
            List<variant> localVariantList = lFrameworkWrapper.VariantList;

            foreach (variant lCurrentVariant in localVariantList)
            {
                List<operation> lOperationList = lFrameworkWrapper.getVariantExprOperations(lCurrentVariant.names);
                if (lOperationList != null)
                {
                    foreach (operation lOperation in lOperationList)
                    {
                        resetCurrentStateAndNewStateOperationVariables(lOperation, lCurrentVariant, pState, "formula7");

                        BoolExpr lOperand = lZ3Solver.OrOperator(new List<BoolExpr>() { lOp_F_CurrentState, lOp_U_CurrentState });
                        if (formula7 == null)
                            formula7 = lOperand;
                        else
                            formula7 = lZ3Solver.AndOperator(new List<BoolExpr>() { formula7, lOperand });
                    }
                }
            }
            //            if (formula7 != null)
            //                lZ3Solver.AddConstraintToSolver(formula7);

            //formula 8
            BoolExpr formula8 = null;

            foreach (variant lCurrentVariant in localVariantList)
            {
                List<operation> lOperationList = lFrameworkWrapper.getVariantExprOperations(lCurrentVariant.names);
                if (lOperationList != null)
                {
                    foreach (operation lOperation in lOperationList)
                    {
                        resetCurrentStateAndNewStateOperationVariables(lOperation, lCurrentVariant, pState, "formula8");

                        //BoolExpr lOpPrecondition = lZ3Solver.MakeBoolVariable("lOpPrecondition");
                        BoolExpr lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.names + "_PreCondition_" + lCurrentVariant.index + "_" + pState.ToString());

                        if (lOperation.precondition != null)
                        {
                            if (lFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(lOperation.precondition[0]) > pState)
                                //This means the precondition is on a transition state which has not been reached yet!
                                lOpPrecondition = lZ3Solver.NotOperator(lOpPrecondition);
                            else
                            {
                                if (lOperation.precondition[0].Contains('_'))
                                    //This means the precondition includes an operation status
                                    lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0]);
                                else
                                    //This means the precondition only includes an operation
                                    lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0] + "_F_" + lCurrentVariant.index + "_" + pState.ToString());
                                //Before it was this
                                //lOperationPrecondition = lZ3Solver.FindBoolExpressionUsingName(lPrecondition + "_I_" + currentVariant.index + "_" + pState.ToString());
                            }
                        }
                        else
                            lOpPrecondition = lOpPrecondition;

                        BoolExpr lFirstOperand = null;

                        lFirstOperand = lZ3Solver.AndOperator(new List<BoolExpr>() { lOp_I_CurrentState, lOpPrecondition });


                        BoolExpr lOperand = lZ3Solver.OrOperator(new List<BoolExpr>() { lFirstOperand, lOp_E_CurrentState });

                        if (formula8 == null)
                            formula8 = lOperand;
                        else
                            formula8 = lZ3Solver.OrOperator(new List<BoolExpr>() { formula8, lOperand });
                    }
                }
            }
            //            if (formula8 != null)
            //                lZ3Solver.AddConstraintToSolver(formula8);


            lOverallGoal = lZ3Solver.NotOperator(lZ3Solver.ImpliesOperator(new List<BoolExpr>() { lZ3Solver.NotOperator(formula7), formula8 }));
            if (lOverallGoal != null)
                lZ3Solver.AddConstraintToSolver(lOverallGoal, "overallGoal");
        }

        private void addVirtualVariantOperationInstances(variant pVirtualVariant, List<operation> pVariantOperations)
        {
            try
            {

                foreach (operation lCurrentOperation in pVariantOperations)
                {
                    //5. Add the VirtualVariant and its operations or variantOperationsList
                    lFrameworkWrapper.CreateVariantOperationMappingInstance(pVirtualVariant.names, pVariantOperations);
                    //6. For all the operations create their respective variables
                    addCurrentVariantOperationInstanceVariables(lCurrentOperation, pVirtualVariant, 0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addVirtualVariantOperationInstances");
                Console.WriteLine(ex.Message);
            }
        }

        public variant ParseVariantExpr(string pVariantExpr)
        {
            variant lResultVariant = null;
            try
            {

                if (pVariantExpr.Contains(' '))
                {
                    //Then this should be an expression on variants
                    lResultVariant = lFrameworkWrapper.createVirtualVariant(pVariantExpr);
                }
                else
                {
                    //Then this should be a single variant
                    lResultVariant = lFrameworkWrapper.findVariantWithName(pVariantExpr);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ParseVariantExpr");
                Console.WriteLine(ex.Message);
            }
            return lResultVariant;
        }

        public void ParsingVariantExpr2OperationsMapping(string pVariantExpr, List<operation> pOperations)
        {
            try
            {
                //First we have to see if our pVariantExpr is a single variant or an Expression of variants
                //incase the variantExpr is an expression on a couple of variants we do the following
                //1. Enter a new VirtualVariant to the variants list
                //2. Add the entered VirtualVariant to the VirtualVariantGroup
                //3. Add a constraint relating the newly created VirtualVariant to the VariantExpr
                //4. Add the VirtualVariant and the VariantExpr to the local virtualVariant2VariantExprList
                variant lCurrentVariant = ParseVariantExpr(pVariantExpr);

                //5. Add the VirtualVariant and its operations or variantOperationsList
                //6. For all the operations create their respective variables
                addVirtualVariantOperationInstances(lCurrentVariant, pOperations);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ParsingVariantExpr2OperationsMapping");
                Console.WriteLine(ex.Message);
            }
        }

        public void createVariantOperationInstances(XmlDocument pXDoc)
        {
            try
            {
                //First we extract the current set of variant operations from the input file
                List<variantOperations> lTemporaryVariantOperationsList = new List<variantOperations>();
                lTemporaryVariantOperationsList = lFrameworkWrapper.createVariantOperationTemporaryInstances(pXDoc);
                foreach (variantOperations lVariantOperation in lTemporaryVariantOperationsList)
                {
                    string lCurrentVariantExpr = lVariantOperation.getVariantExpr();
                    List<operation> lCurrentOperation = lVariantOperation.getOperations();

                    //Now we parse each instace of variant operation so for those who have a variant expression instead of variant we can create Virtual variants
                    ParsingVariantExpr2OperationsMapping(lCurrentVariantExpr, lCurrentOperation);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariantOperationInstances");
                Console.WriteLine(ex.Message);
            }
        }

        public void LoadInitialDataFromXMLFile(string pFilePath)
        {
            try
            {
                //new instance of xdoc
                XmlDocument xDoc = new XmlDocument();

                //First load the XML file from the file path
                xDoc.Load(pFilePath);

                lFrameworkWrapper.createOperationInstances(xDoc);
                lFrameworkWrapper.createVariantInstances(xDoc);
                lFrameworkWrapper.createVariantGroupInstances(xDoc);
                lFrameworkWrapper.createConstraintInstances(xDoc);
                createVariantOperationInstances(xDoc);
                //createStationInstances(xDoc);
                lFrameworkWrapper.createTraitInstances(xDoc);
                lFrameworkWrapper.createResourceInstances(xDoc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in LoadInitialDataFromXMLFile, FilePath: " + pFilePath);
                Console.WriteLine(ex.Message);
            }
        }

        public void ProductPlatformAnalysis(string[] args)
        {
            String file = null;
            if (args.Length > 0)
                file = args[0];

            try
            {
                // These examples need proof generation turned on.
                using (Context ctx = new Context(new Dictionary<string, string>() { { "proof", "true" } }))
                {
                    lZ3Solver.setDebugMode(true);

                    loadInitialData(file);

                    //NoOfCycles = NumOfOperations * NumOfTransitions
                    int lNoOfOperations = lFrameworkWrapper.getNumberOfOperations();

                    //Each operation will have two transitions
                    int lNoOfCycles = lNoOfOperations * 2;

                    bool done = false;

                    bool lTestResult = false;
                    lZ3Solver.PrepareDebugDirectory();
                    for (int i = 0; i < lNoOfCycles; i++)
                    {
                        lTestResult = testConstraintConvertion(i, file, done);

                        if (lTestResult)
                            break;
                        //                        lZ3SolverEngineer.ResetAnalyzer();
                        Console.ReadKey();
                        //ResetAnalyzer();

                        if (i == lNoOfCycles - 1)
                        {
                            done = true;
                            lTestResult = testConstraintConvertion(i, file, done);
                        }

                    }
                    Console.ReadKey();
                }
            }
            catch (Z3Exception ex)
            {
                Console.WriteLine("Z3 Managed Exception: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

                lZ3SolverEngineer.ProductPlatformAnalysis(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
