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
    public class Z3SolverEngineer
    {
        private FrameworkWrapper cFrameworkWrapper;
        private Z3Solver cZ3Solver;
        private RandomTestCreator cRandomTestCreator;
        private bool cDebugMode;
        private bool cOpSeqAnalysis;
        private bool cNeedPreAnalysis;
        private bool cPreAnalysisResult;


        private BoolExpr cOp_I_CurrentState;
        private BoolExpr cOp_E_CurrentState;
        private BoolExpr cOp_F_CurrentState;
        private BoolExpr cOp_U_CurrentState;

        private BoolExpr cOp_I_NextState;
        private BoolExpr cOp_E_NextState;
        private BoolExpr cOp_F_NextState;
        private BoolExpr cOp_U_NextState;

        private BoolExpr cOpPrecondition;
        private BoolExpr cOpPostcondition;

        private List<BoolExpr> cConfigurationConstraints;
        private List<BoolExpr> cPConstraints;

        //boolean variation point variables for model building
        private Enumerations.GeneralAnalysisType cGeneralAnalysisType;
        private Enumerations.AnalysisType cAnalysisType;

        private bool cConvertVariants;
        private bool cConvertConfigurationRules;
        private bool cConvertOperations;
        private bool cConvertOperationPrecedenceRules;
        private bool cConvertVariantOperationRelations;
        private bool cConvertResources;
        private bool cConvertGoal;
        private bool cBuildPConstraints;

        //boolean variation point variables for reporting in output
        private bool cReportAnalysisResult;
        private bool cReportAnalysisDetailResult;
        private bool cReportVariantsResult;
        private bool cReportTransitionsResult;
        private bool cReportAnalysisTiming;
        private bool cReportUnsatCore;
        private bool cReportStopBetweenEachTransition;
        private bool cStopAEndOfAnalysis;
        private int cNoOfModelsRequired;

        /// <summary>
        /// This is the creator for the class which initializes the class variables
        /// </summary>
        public Z3SolverEngineer()
        {
            cFrameworkWrapper = new FrameworkWrapper();
            cRandomTestCreator = new RandomTestCreator();
            cZ3Solver = new Z3Solver();
            cDebugMode = false;
            cOpSeqAnalysis = true;
            cNeedPreAnalysis = true;
            cPreAnalysisResult = true;
            cConfigurationConstraints = new List<BoolExpr>();
            cPConstraints = new List<BoolExpr>();

            //boolean variation point variables
            setVariationPoints(Enumerations.GeneralAnalysisType.Static, Enumerations.AnalysisType.CompleteAnalysis);
            setReportType(true, true, true, true, true, true, true, true);
        }

        /// <summary>
        /// This function sets how the outputs should be reported, it does this by setting the output variation points
        /// </summary>
        public void setReportType(bool pAnalysisResult
                                    , bool pAnalysisDetailResult
                                    , bool pVariantsResult
                                    , bool pTransitionsResult
                                    , bool pAnalysisTiming
                                    , bool pUnsatCore
                                    , bool pStopBetweenEachTransition
                                    , bool pStopAtEndOfAnalysis)
        {
            try
            {
                cReportAnalysisResult = pAnalysisResult;
                cReportAnalysisDetailResult = pAnalysisDetailResult;
                cReportVariantsResult = pVariantsResult;
                cReportTransitionsResult = pTransitionsResult;
                cReportAnalysisTiming = pAnalysisTiming;
                cReportUnsatCore = pUnsatCore;
                cReportStopBetweenEachTransition = pStopBetweenEachTransition;
                cStopAEndOfAnalysis = pStopAtEndOfAnalysis;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in setReportType");
                Console.WriteLine(ex.Message);
            }
        }

        public void DefaultModelEnumerationAnalysisVariationPointSetting(int pOverrideNoOfModelsRequired = 1
                                        , bool pOverrideConvertVariants = false
                                        , bool pOverrideConvertConfigurationRules = false
                                        , bool pOverrideConvertOperations = false
                                        , bool pOverrideConvertOperationPrecedenceRules = false
                                        , bool pOverrideConvertVariantOperationRelations = false
                                        , bool pOverrideConvertResources = false
                                        , bool pOverrideConvertGoal = false
                                        , bool pOverrideBuildPConstaints = false)
        {
            try
            {
                cConvertVariants = (!pOverrideConvertVariants) ? true : false;
                cConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                cConvertOperations = (!pOverrideConvertOperations) ? true : false;
                cConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                cConvertVariantOperationRelations = (!pOverrideConvertVariantOperationRelations) ? true : false;
                cConvertResources = (!pOverrideConvertResources) ? false : true;
                cConvertGoal = (!pOverrideConvertGoal) ? true : false;
                cBuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DefaultModelEnumerationAnalysisVariationPointSetting");
                Console.WriteLine(ex.Message);
            }
        }

        public void DefaultVariantSelectabilityAnalysisVariationPointSetting(int pOverrideNoOfModelsRequired = 1
                                        , bool pOverrideConvertVariants = false
                                        , bool pOverrideConvertConfigurationRules = false
                                        , bool pOverrideConvertOperations = false
                                        , bool pOverrideConvertOperationPrecedenceRules = false
                                        , bool pOverrideConvertVariantOperationRelations = false
                                        , bool pOverrideConvertResources = false
                                        , bool pOverrideConvertGoal = false
                                        , bool pOverrideBuildPConstaints = false)
        {
            try
            {
                cConvertVariants = (!pOverrideConvertVariants) ? true : false;
                cConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                cConvertOperations = (!pOverrideConvertOperations) ? true : false;
                cConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                cConvertVariantOperationRelations = (!pOverrideConvertVariantOperationRelations) ? true : false;
                cConvertResources = (!pOverrideConvertResources) ? false : true;
                cConvertGoal = (!pOverrideConvertGoal) ? false : true;
                cBuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DefaultVariantSelectabilityAnalysisVariationPointSetting");
                Console.WriteLine(ex.Message);
            }
        }

        public void DefaultAlwaysSelectedVariantAnalysisVariationPointSetting(int pOverrideNoOfModelsRequired = 1
                                        , bool pOverrideConvertVariants = false
                                        , bool pOverrideConvertConfigurationRules = false
                                        , bool pOverrideConvertOperations = false
                                        , bool pOverrideConvertOperationPrecedenceRules = false
                                        , bool pOverrideConvertVariantOperationRelations = false
                                        , bool pOverrideConvertResources = false
                                        , bool pOverrideConvertGoal = false
                                        , bool pOverrideBuildPConstaints = false)
        {
            try
            {
                cConvertVariants = (!pOverrideConvertVariants) ? true : false;
                cConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                cConvertOperations = (!pOverrideConvertOperations) ? true : false;
                cConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                cConvertVariantOperationRelations = (!pOverrideConvertVariantOperationRelations) ? true : false;
                cConvertResources = (!pOverrideConvertResources) ? false : true;
                cConvertGoal = (!pOverrideConvertGoal) ? false : true;
                cBuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DefaultAlwaysSelectedVariantAnalysisVariationPointSetting");
                Console.WriteLine(ex.Message);
            }
        }

        public void DefaultOperationSelectabilityAnalysisVariationPointSetting(int pOverrideNoOfModelsRequired = 1
                                        , bool pOverrideConvertVariants = false
                                        , bool pOverrideConvertConfigurationRules = false
                                        , bool pOverrideConvertOperations = false
                                        , bool pOverrideConvertOperationPrecedenceRules = false
                                        , bool pOverrideConvertVariantOperationRelations = false
                                        , bool pOverrideConvertResources = false
                                        , bool pOverrideConvertGoal = false
                                        , bool pOverrideBuildPConstaints = false)
        {
            try
            {
                cConvertVariants = (!pOverrideConvertVariants) ? true : false;
                cConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                cConvertOperations = (!pOverrideConvertOperations) ? true : false;
                cConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                cConvertVariantOperationRelations = (!pOverrideConvertVariantOperationRelations) ? true : false;
                cConvertResources = (!pOverrideConvertResources) ? false : true;
                cConvertGoal = (!pOverrideConvertGoal) ? false : true;
                cBuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DefaultOperationSelectabilityAnalysisVariationPointSetting");
                Console.WriteLine(ex.Message);
            }
        }

        public void DefaultAlwaysSelectedOperationAnalysisVariationPointSetting(int pOverrideNoOfModelsRequired = 1
                                        , bool pOverrideConvertVariants = false
                                        , bool pOverrideConvertConfigurationRules = false
                                        , bool pOverrideConvertOperations = false
                                        , bool pOverrideConvertOperationPrecedenceRules = false
                                        , bool pOverrideConvertVariantOperationRelations = false
                                        , bool pOverrideConvertResources = false
                                        , bool pOverrideConvertGoal = false
                                        , bool pOverrideBuildPConstaints = false)
        {
            try
            {
                cConvertVariants = (!pOverrideConvertVariants) ? true : false;
                cConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                cConvertOperations = (!pOverrideConvertOperations) ? true : false;
                cConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                cConvertVariantOperationRelations = (!pOverrideConvertVariantOperationRelations) ? true : false;
                cConvertResources = (!pOverrideConvertResources) ? false : true;
                cConvertGoal = (!pOverrideConvertGoal) ? false : true;
                cBuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DefaultAlwaysSelectedOperationAnalysisVariationPointSetting");
                Console.WriteLine(ex.Message);
            }
        }

        public void DefaultExistanceOfDeadlockAnalysisVariationPointSetting(int pOverrideNoOfModelsRequired = 1
                                        , bool pOverrideConvertVariants = false
                                        , bool pOverrideConvertConfigurationRules = false
                                        , bool pOverrideConvertOperations = false
                                        , bool pOverrideConvertOperationPrecedenceRules = false
                                        , bool pOverrideConvertVariantOperationRelations = false
                                        , bool pOverrideConvertResources = false
                                        , bool pOverrideConvertGoal = false
                                        , bool pOverrideBuildPConstaints = false)
        {
            try
            {
                cConvertVariants = (!pOverrideConvertVariants) ? true : false;
                cConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                cConvertOperations = (!pOverrideConvertOperations) ? true : false;
                cConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? true : false;
                cConvertVariantOperationRelations = (!pOverrideConvertVariantOperationRelations) ? true : false;
                cConvertResources = (!pOverrideConvertResources) ? false : true;
                cConvertGoal = (!pOverrideConvertGoal) ? true : false;
                cBuildPConstraints = (!pOverrideBuildPConstaints) ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DefaultExistanceOfDeadlockAnalysisVariationPointSetting");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// This function sets the variations points which determine how the analysis is carried out
        /// </summary>
        /// <param name="pGeneralAnalysisType">Sets if the analysis is static or dynamic</param>
        /// <param name="pAnalysisType">Sets which type of analysis is going to be chosen, the variations include both static and dynamic variation points</param>
        /// <param name="pConvertVariants">If the variants should be converted or not</param>
        /// <param name="pConvertConfigurationRules">If the Configuration rules should be converted or not</param>
        /// <param name="pConvertOperations">If the operations should be converted or not</param>
        /// <param name="pConvertOperationPrecedenceRules">If the operation precedence rules should be converted or not</param>
        /// <param name="pConvertVariantOperationRelations">If the variant operation mappings should be converted or not</param>
        /// <param name="pConvertResources">If the resources should be converted or not</param>
        /// <param name="pConvertGoal">If the goal should be converted or not</param>
        /// <param name="pBuildPConstaints">If the precedence rules constraints should be kept in a local list or not</param>
        /// <param name="pNoOfModelsRequired">In case models needs to be created how many models are needed</param>
        public void setVariationPoints(Enumerations.GeneralAnalysisType pGeneralAnalysisType
                                        , Enumerations.AnalysisType pAnalysisType
                                        , int pOverrideNoOfModelsRequired = 1
                                        , bool pOverrideConvertVariants = false
                                        , bool pOverrideConvertConfigurationRules = false
                                        , bool pOverrideConvertOperations = false
                                        , bool pOverrideConvertOperationPrecedenceRules = false
                                        , bool pOverrideConvertVariantOperationRelations = false
                                        , bool pOverrideConvertResources = false
                                        , bool pOverrideConvertGoal = false
                                        , bool pOverrideBuildPConstaints = false)
        {
            try
            {
                cGeneralAnalysisType = pGeneralAnalysisType;
                cAnalysisType = pAnalysisType;
                switch (cAnalysisType)
                {
                    case Enumerations.AnalysisType.ModelEnumerationAnalysis:
                        {
                            DefaultModelEnumerationAnalysisVariationPointSetting(pOverrideNoOfModelsRequired
                                                                                , pOverrideConvertVariants
                                                                                , pOverrideConvertConfigurationRules
                                                                                , pOverrideConvertOperations
                                                                                , pOverrideConvertOperationPrecedenceRules
                                                                                , pOverrideConvertVariantOperationRelations
                                                                                , pOverrideConvertResources
                                                                                , pOverrideConvertGoal
                                                                                , pOverrideBuildPConstaints);
                            break;
                        }
                    case Enumerations.AnalysisType.VariantSelectabilityAnalysis:
                        {
                            DefaultVariantSelectabilityAnalysisVariationPointSetting(pOverrideNoOfModelsRequired
                                                                                , pOverrideConvertVariants
                                                                                , pOverrideConvertConfigurationRules
                                                                                , pOverrideConvertOperations
                                                                                , pOverrideConvertOperationPrecedenceRules
                                                                                , pOverrideConvertVariantOperationRelations
                                                                                , pOverrideConvertResources
                                                                                , pOverrideConvertGoal
                                                                                , pOverrideBuildPConstaints);
                            break;
                        }
                    case Enumerations.AnalysisType.AlwaysSelectedVariantAnalysis:
                        {
                            DefaultAlwaysSelectedVariantAnalysisVariationPointSetting(pOverrideNoOfModelsRequired
                                                                                , pOverrideConvertVariants
                                                                                , pOverrideConvertConfigurationRules
                                                                                , pOverrideConvertOperations
                                                                                , pOverrideConvertOperationPrecedenceRules
                                                                                , pOverrideConvertVariantOperationRelations
                                                                                , pOverrideConvertResources
                                                                                , pOverrideConvertGoal
                                                                                , pOverrideBuildPConstaints);
                            break;
                        }
                    case Enumerations.AnalysisType.OperationSelectabilityAnalysis:
                        {
                            DefaultOperationSelectabilityAnalysisVariationPointSetting(pOverrideNoOfModelsRequired
                                                                                , pOverrideConvertVariants
                                                                                , pOverrideConvertConfigurationRules
                                                                                , pOverrideConvertOperations
                                                                                , pOverrideConvertOperationPrecedenceRules
                                                                                , pOverrideConvertVariantOperationRelations
                                                                                , pOverrideConvertResources
                                                                                , pOverrideConvertGoal
                                                                                , pOverrideBuildPConstaints);
                            break;
                        }
                    case Enumerations.AnalysisType.AlwaysSelectedOperationAnalysis:
                        {
                            DefaultAlwaysSelectedOperationAnalysisVariationPointSetting(pOverrideNoOfModelsRequired
                                                                                , pOverrideConvertVariants
                                                                                , pOverrideConvertConfigurationRules
                                                                                , pOverrideConvertOperations
                                                                                , pOverrideConvertOperationPrecedenceRules
                                                                                , pOverrideConvertVariantOperationRelations
                                                                                , pOverrideConvertResources
                                                                                , pOverrideConvertGoal
                                                                                , pOverrideBuildPConstaints);
                            break;
                        }
                    case Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis:
                        {
                            DefaultExistanceOfDeadlockAnalysisVariationPointSetting(pOverrideNoOfModelsRequired
                                                                                , pOverrideConvertVariants
                                                                                , pOverrideConvertConfigurationRules
                                                                                , pOverrideConvertOperations
                                                                                , pOverrideConvertOperationPrecedenceRules
                                                                                , pOverrideConvertVariantOperationRelations
                                                                                , pOverrideConvertResources
                                                                                , pOverrideConvertGoal
                                                                                , pOverrideBuildPConstaints);
                            break;
                        }
                    case Enumerations.AnalysisType.AlwaysDeadlockAnalysis:
                        {
                            break;
                        }
                    case Enumerations.AnalysisType.CompleteAnalysis:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in setVariationPoints");
                Console.WriteLine(ex.Message);
            }
        }

/*        public void ResetAnalyzer()
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
        }*/

        /// <summary>
        /// NOT USED ANYWHERE IN PROJECT
        /// According to the variant group least for each variant group a boolean expression is created.
        /// </summary>
        public void makeExpressionListFromVariantGroupList()
        {
            try
            {
                List<variantGroup> localVariantGroupList = cFrameworkWrapper.VariantGroupList;

                foreach (variantGroup localVariantGroup in localVariantGroupList)
                    cZ3Solver.AddBooleanExpression(localVariantGroup.names);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in makeExpressionListFromVariantGroupList");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// NOT USED ANYWHERE IN THE PROJECT
        /// For each variant in the local list a boolean variable is created
        /// </summary>
        public void makeExpressionListFromVariantList()
        {
            try
            {
                List<variant> localVariantList = cFrameworkWrapper.VariantList;

                foreach (variant localVariant in localVariantList)
                    cZ3Solver.AddBooleanExpression(localVariant.names);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in makeExpressionListFromVariantList");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// This function loads the initial data from the external fle to internal lists
        /// </summary>
        /// <param name="pInitialData"></param>
        /// <param name="pFile"></param>
        /// <returns></returns>
        public bool loadInitialData(Enumerations.InitializerSource pInitialData, String pFile)
        {
            bool lDataLoaded = false;
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

                switch (pInitialData)
                {
                    case Enumerations.InitializerSource.ExternalFile:
                        {
//                            LoadInitialDataFromXMLFile(exePath + "../../../" + endPath);
                            lDataLoaded = LoadInitialDataFromXMLFile("../../Test/" + endPath);
                            //             lFrameworkWrapper.LoadInitialDataFromXMLFile(endPath);
                            break;
                        }
                    case Enumerations.InitializerSource.InternalFile:
                        {
                            //LoadInitialDataFromXMLFile("D:/LocalImplementation/GitHub/ProductPlatformAnalyzer/ProductPlatformAnalyzer/Test/1V2O1PreNoTransitions.xml");
                            lDataLoaded = LoadInitialDataFromXMLFile("D:/LocalImplementation/GitHub/ProductPlatformAnalyzer/ProductPlatformAnalyzer/Test/" + endPath);
                            break;
                        }
                    case Enumerations.InitializerSource.RandomData:
                        {
                            //Creating random data
                            //lRandomTestCreator.createRandomData(2, 4, 3, 100, 0, 0, 4, 3, 2);
                            //Console.WriteLine(lFrameworkWrapper.getConstraintList());
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in loadInitialData");
                Console.WriteLine(ex.Message);
            }
            return lDataLoaded;
        }

        ////New Version Commented out
        /// <summary>
        /// Uses the stated variation points that have been set according to the analysis type to build the model
        /// </summary>
        /*public void MakeProdutPlatformModel()
        {
            try
            {
                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                int lNoOfCycles = CalculateAnalysisNoOfCycles();

                //This is to initiate the debuging directory which will be created and filled during the analysis
                lZ3Solver.PrepareDebugDirectory();

                //If it is asked for the model to include the variants they are added to the created model
                //Variation Point
                if (lConvertVariants)
                    //formula 2
                    convertProductPlatform();

                //If it is asked for the configuration rules to be added to the model then they are added
                //Variation Point
                if (lConvertConfigurationRules)
                    //formula 3
                    convertFConstraint2Z3Constraint();

                //If it is asked for the resources to be added to the model then they are added
                //Variation Point
                if (lConvertResources)
                    convertResourcesNOperationResourceRelations();

                //Now we loop for the maximum number of transitions
                for (int i = 0; i < lNoOfCycles; i++)
                {
                    //If it is asked for the model to include the operations then they are added to the model
                    //Variation Point
                    if (lConvertOperations)
                        convertFOperations2Z3Operations(i);

                    //If it is asked for the operation precedence rues to be added to the model then they are added
                    //Variation Point
                    if (lConvertOperationPrecedenceRules)
                        convertOperationsPrecedenceRulesNOperationVariantRelations(i);

                    //Now the goal is added to the model
                    //Variation Point
                    if (lConvertGoal)
                        convertGoal(i);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in MakeProdutPlatformModel");
                Console.WriteLine(ex.Message);
            }
        }*/

        /// <summary>
        /// This function creates the static parts of the product platform model, which includes variants, group cardinality, configuration rules, and resources info
        /// </summary>
        /// <param name="pExtraConfigurationRule">If an extra configuration rule needs to be added to the information which comes from the file</param>
        public void MakeStaticPartOfProductPlatformModel(string pExtraConfigurationRule = "")
        {
            try
            {
                //If it is asked for the model to include the variants they are added to the created model
                //Variation Point
                if (cConvertVariants)
                    convertProductPlatform();

                //If it is asked for the configuration rules to be added to the model then they are added
                //Variation Point
                
                if (cConvertConfigurationRules)
                    convertFConstraint2Z3Constraint(pExtraConfigurationRule);

                ////I am not sure this part would be needed here, hence I added it to the previous part.
                ////if (lConvertConfigurationRules)
                ////    AddExtraConstraint2Z3Constraint(pExtraConfigurationRule);

                //If it is asked for the resources to be added to the model then they are added
                //Variation Point
                if (cConvertResources)
                    convertResourcesNOperationResourceRelations();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in MakeStaticPartOfProductPlatformModel");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// This function creates the dynamic part of the product platform model which includes the opertions, the precedence rules among the operations and the goals
        /// </summary>
        /// <param name="pAnalysisComplete">If the analysis is concluded, finished</param>
        /// <param name="pTransitionNo"></param>
        public void MakeDynamicPartOfProductPlatformModel(bool pAnalysisComplete, int pTransitionNo)
        {
            try
            {
                //TODO: write what formulas each of these lines include

                //If it is asked for the model to include the operations then they are added to the model
                //Variation Point
                if (cConvertOperations)
                    convertFOperations2Z3Operations(pTransitionNo);

                //If it is asked for the operation precedence rues to be added to the model then they are added
                //Variation Point
                if (cConvertOperationPrecedenceRules || cBuildPConstraints)
                    convertOperationsPrecedenceRules(pTransitionNo);

                //Now the goal is added to the model
                //Variation Point
                if (cConvertGoal && !pAnalysisComplete)
                    convertGoal(pTransitionNo);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in MakeDynamicPartOfProductPlatformModel");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// This function uses the variation points to set up the model for the product platform
        /// Then calculated how many times the analysis needs to be carried out
        /// Then carries out the analysis for each transition (the number of which was calculated in the previous step)
        /// </summary>
        /// <param name="pAnalysisComplete">Variable which is used to anounce if the analysis has been completed or not</param>
        /// <param name="pExtraConfigurationRule">Any extra configuration rules which need to be added to the product platform configuration rule set</param>
        /// <returns>The result of the analysis</returns>
        public Status AnalyzeProductPlatform(int pTransitionNo
                                            , int pModelIndex
                                            , bool pAnalysisComplete
                                            , string pExtraConfigurationRule = "")
        {
            //This is the variable which holds the result of the analysis
            Status lTestResult = Status.UNKNOWN;

            ////TODO: Removed as new version
            //Each analysis is initially going to be incomplete
            ////pAnalysisComplete = false;
            try
            {

                cOpSeqAnalysis = true;

                ////TODO: Removed as new version
                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                ////int lNoOfCycles = CalculateAnalysisNoOfCycles();

                ////TODO: Removed as new version
                //This is to initiate the debuging directory which will be created and filled during the analysis
                ////lZ3Solver.PrepareDebugDirectory();

                ////TODO: Removed as new version
                //If it is asked for the model to include the variants they are added to the created model
                //Variation Point
                ////if (lConvertVariants)
                ////    convertProductPlatform();

                ////TODO: Removed as new version
                //If it is asked for the configuration rules to be added to the model then they are added
                //Variation Point
                ////if (lConvertConfigurationRules)
                ////    convertConfigurationRules(pExtraConfigurationRule);
                ////if (lConvertConfigurationRules)
                ////    AddExtraConstraint2Z3Constraint(pExtraConfigurationRule);

                ////TODO: Removed as new version
                //If it is asked for the resources to be added to the model then they are added
                //Variation Point
                ////if (lConvertResources)
                ////    convertResourcesNOperationResourceRelations();

                ////TODO: Removed as new version
                //Now we loop for the maximum number of transitions
                ////for (int i = 0; i < lNoOfCycles; i++)
                ////{
                    ////TODO: Removed as new version
                    //If it is asked for the model to include the operations then they are added to the model
                    //Variation Point
                    ////if (lConvertOperations)
                    ////    convertFOperations2Z3Operations(i);

                    ////TODO: Removed as new version
                    //If it is asked for the operation precedence rues to be added to the model then they are added
                    //Variation Point
                    ////if (lConvertOperationPrecedenceRules)
                    ////    convertOperationsPrecedenceRulesNOperationVariantRelations(i);

                    ////TODO: Removed as new version
                    //Now the goal is added to the model
                    //Variation Point
                    ////if (lConvertGoal && !pAnalysisComplete)
                    ////    convertGoal(i);

                    //The model is analyzed and both the result and whether the analysis is complete or not is returned

                    string lStrExprToBeAnalyzed = "";
                    if (cAnalysisType == Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis)
                        lStrExprToBeAnalyzed = "P" + pTransitionNo;
                    else if (cAnalysisType == Enumerations.AnalysisType.OperationSelectabilityAnalysis)
                        lStrExprToBeAnalyzed = pExtraConfigurationRule;

                    ////Previous version: lTestResult = anlyzeModel(pTransitionNo, pAnalysisComplete, lStrExprToBeAnalyzed);
                    if (cPreAnalysisResult)
                    {
                        if (!pAnalysisComplete)
                        {
                            if (cGeneralAnalysisType.Equals(Enumerations.GeneralAnalysisType.Dynamic))
                                Console.WriteLine("Analysis No: " + pTransitionNo);
                            ////Removing unwanted code, this function was pointing to a one line function
                            ////lTestResult = analyseZ3Model(pState, pDone, lFrameworkWrapper, pStrExprToCheck);
                            lTestResult = cZ3Solver.CheckSatisfiability(pTransitionNo
                                                                        , pAnalysisComplete
                                                                        , cFrameworkWrapper
                                                                        , cConvertGoal
                                                                        , cReportAnalysisDetailResult
                                                                        , cReportVariantsResult
                                                                        , cReportTransitionsResult
                                                                        , cReportAnalysisTiming
                                                                        , cReportUnsatCore
                                                                        , lStrExprToBeAnalyzed);

                            if (cAnalysisType.Equals(Enumerations.AnalysisType.ModelEnumerationAnalysis))
                                cZ3Solver.WriteDebugFile(-1, pModelIndex);
                            else
                                cZ3Solver.WriteDebugFile(pTransitionNo, -1);

                            
                            if (lTestResult.Equals(Status.SATISFIABLE))
                                cZ3Solver.AddModelItem2SolverAssertion(cFrameworkWrapper);
                        }
/*                        else
                        {
                            ////TODO: If the analysis is finished why should it check the satisfiablity again???????
                            ////TODO: Have to remember the reason behind this else.

                            Console.WriteLine("Finished: ");
                            ////Removing unwanted code, this function was pointing to a one line function
                            ////lTestResult = analyseZ3Model(pState, pDone, lFrameworkWrapper, pStrExprToCheck);
                            lTestResult = lZ3Solver.CheckSatisfiability(pTransitionNo
                                                                        , pAnalysisComplete
                                                                        , lFrameworkWrapper
                                                                        , lConvertGoal
                                                                        , lReportAnalysisDetailResult
                                                                        , lReportAnalysisTiming
                                                                        , lReportUnsatCore
                                                                        , pExtraConfigurationRule);

                            lZ3Solver.WriteDebugFile(pTransitionNo);
                        }*/
                    }


                    ////TODO: Removed as new version
                    //If the result of the analysis is false then we should stop the analysis
                    ////if (lTestResult)
                    ////    break;

                    //variation point
                    if (cReportStopBetweenEachTransition)
                        Console.ReadKey();

                    ////TODO: Removed as new version
                    //If all the transition cycles are completed then the analysis is completed
                    ////if (pTransitionNo == lNoOfCycles - 1)
                    ////    pAnalysisComplete = true;

                ////}
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AnalyzeProductPlatform");
                Console.WriteLine(ex.Message);
            }
            return lTestResult;
        }

        /// <summary>
        /// This function reports the findings of the analysis
        /// </summary>
        /// <param name="pState"></param>
        /// <param name="pDone"></param>
        /// <param name="pFrameworkWrapper"></param>
        /// <param name="pSatResult"></param>
        public void ReportSolverResult(int pState
                                        , bool pDone
                                        , FrameworkWrapper pFrameworkWrapper
                                        , Status pSatResult
                                        , object pExtraField)
        {
            try
            {
                if (pSatResult.Equals(Status.SATISFIABLE))
                {
                    OutputHandler output = new OutputHandler(pFrameworkWrapper);

                    output = cZ3Solver.PopulateOutputHandler(pState, output);

                    if (cAnalysisType.Equals(Enumerations.AnalysisType.ModelEnumerationAnalysis))
                    {
                        //What does the satisfiable result in this analysis mean?
                        if (cReportAnalysisResult)
                            Console.WriteLine("The ProductPlatform has a model to satisfy it.");

                        //Means that there has been a model which has been found
                        Console.WriteLine("Model No " + pState + ":");
                        if (cReportVariantsResult)
                            output.printChosenVariants();
                        if (cReportTransitionsResult)
                            output.printOperationsTransitions();

                    }
                    else if (cAnalysisType.Equals(Enumerations.AnalysisType.VariantSelectabilityAnalysis))
                    {
                        if (pExtraField != null)
                        {
                            variant lAnalyzedVariant = (variant) pExtraField;
                            //What does the satisfiable result in this analysis mean?
                            if (cReportAnalysisResult)
                            {
                                Console.WriteLine("---------------------------------------------------------------------");
                                Console.WriteLine("Selected Variant: " + lAnalyzedVariant.names);
                                Console.WriteLine(lAnalyzedVariant.names + " is Selectable.");

                            }

                        }

                    }
                    else if (cAnalysisType.Equals(Enumerations.AnalysisType.OperationSelectabilityAnalysis))
                    {
                        if (pExtraField != null)
                        {
                            string lAnalyzedOperationName = (string)pExtraField;
                            //What does the satisfiable result in this analysis mean?
                            if (cReportAnalysisResult)
                            {
                                //Initial info
                                Console.WriteLine("----------------------------------------------------------------");
                                Console.WriteLine("Analysing operation named: " + lAnalyzedOperationName);

                                //Analysis Result
                                Console.WriteLine(lAnalyzedOperationName + " is selectable.");
                            }
                        }
                    }
                    else if (cAnalysisType.Equals(Enumerations.AnalysisType.AlwaysSelectedVariantAnalysis))
                    {
                        if (pExtraField != null)
                        {
                            variant lAnalyzedVariant = (variant)pExtraField;
                            if (cReportAnalysisResult)
                            {
                                //Initial info
                                Console.WriteLine("---------------------------------------------------------------------");
                                Console.WriteLine("Selected Variant: " + lAnalyzedVariant.names);

                                //Analysis Result
                                //if it does hold, then there is a configuration which is valid and this current variant is not present in it
                                Console.WriteLine("There DOES exist a valid configuration which does not include " + lAnalyzedVariant.names + ".");

                            }

                        }
                    }
                    else if (cAnalysisType.Equals(Enumerations.AnalysisType.AlwaysSelectedOperationAnalysis))
                    { 
                        if (pExtraField != null)
                        {
                            string lOperationName = cFrameworkWrapper.ReturnOperationNameFromOperationInstance(pExtraField.ToString());

                            //if it does hold, then there exists a valid configuration in which the current operation is UNUSED!
                            Console.WriteLine("There DOES exist a configuration in which " + lOperationName + " is in an UNUSED state!");
                        }
                    }
                    else
                    {
                        if (pDone)
                        {
                            //Print and writes an output file showing the result of a finished test
                            if (cReportAnalysisDetailResult)
                            {
                                Console.WriteLine("Model No " + pState + ":");
                                if (cReportVariantsResult)
                                {
                                    output.printChosenVariants();
                                }
                                if (cReportTransitionsResult)
                                    output.printOperationsTransitions();
                            }
                            output.writeFinished();
                            output.writeFinishedNoPost();
                        }
                        else
                        {
                            //Print and writes an output file showing the result of a deadlocked test
                            if (cReportAnalysisDetailResult)
                                output.printCounterExample();
                            output.writeCounterExample();
                            output.writeCounterExampleNoPost();
                        }
                    }
                    output.writeDebugFile();
                }
                else
                {
                    if (cAnalysisType.Equals(Enumerations.AnalysisType.ModelEnumerationAnalysis))
                    {
                        //What does the unsatisfiable result in this analysis mean?
                        if (cReportAnalysisResult)
                            Console.WriteLine("The ProductPlatform has no more valid models.");
                    }
                    else if (cAnalysisType.Equals(Enumerations.AnalysisType.VariantSelectabilityAnalysis))
                    {

                    }
                    else if (cAnalysisType.Equals(Enumerations.AnalysisType.OperationSelectabilityAnalysis))
                    {
                        //Here we check if this operation is in active operation or not, meaning does a variant use this operation for its assembly or not
                        if (!cFrameworkWrapper.isOperationInstanceActive(pExtraField.ToString()))
                        {

                            //This means the operation instance is inactive hence it should be mentioned

                            //This is when we want to report only the operation name
                            Console.WriteLine("Operation " + cFrameworkWrapper.ReturnOperationNameFromOperationInstance(pExtraField.ToString()) + " is inactive!");
                        }

                    }
                    else if (cAnalysisType.Equals(Enumerations.AnalysisType.AlwaysSelectedVariantAnalysis))
                    {

                        if (pExtraField != null)
                        {
                            variant lAnalyzedVariant = (variant)pExtraField;
                            Console.WriteLine("All valid configurations DO include " + lAnalyzedVariant.names + ".");
                        }

                    }
                    else if (cAnalysisType.Equals(Enumerations.AnalysisType.AlwaysSelectedOperationAnalysis))
                    {
                        if (pExtraField != null)
                        {
                            string lOperationName = cFrameworkWrapper.ReturnOperationNameFromOperationInstance(pExtraField.ToString());

                            Console.WriteLine("All valid configurations DO include " + lOperationName + ".");
                        }
                    }

                    //Console.WriteLine("proof: {0}", iSolver.Proof);
                    //Console.WriteLine("core: ");
                    if (cReportUnsatCore)
                        cZ3Solver.ConsoleWriteUnsatCore();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReportSolverResults");
                Console.WriteLine(ex.Message);
            }
        }
        
        /// <summary>
        /// This function converts the static part of the product platform, i.e. variants, followed by the group cardinalities
        /// </summary>
        private void convertProductPlatform()
        {
            try
            {
                convertFVariants2Z3Variants();

                //formula 2
                produceVariantGroupGCardinalityConstraints();

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertProductPlatform");
                Console.WriteLine(ex.Message);
            }
        }

        ////TODO: If every analysis is correct then it should be removed!
        /// <summary>
        /// This function converts the configuration rules, i.e. variant group cardinality and constraint rules
        /// </summary>
        /*private void convertConfigurationRules(string pExtraConfigurationRule = "")
        {
            try
            {
                //formula 3
                convertFConstraint2Z3Constraint(pExtraConfigurationRule);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertConfigurationRules");
                Console.WriteLine(ex.Message);
            }
        }*/


        ////TODO: If every thing is correct should be removed!!
        /// <summary>
        /// This function converts the dynamic part of the product platform, i.e. operations 
        /// </summary>
        /// <param name="pState"></param>
        /*private void convertOperations(int pState)
        {
            try
            {
                convertFOperations2Z3Operations(pState);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertOperations");
                Console.WriteLine(ex.Message);
            }
        }*/


        /// <summary>
        /// This function converts operation relations (precedence rules)
        /// and the relationship between operations and variants
        /// </summary>
        /// <param name="pState"></param>
        private void convertOperationsPrecedenceRules(int pState)
        {
            try
            {
                //TODO: Here we have to check if the operations have really been converted.

                if (cNeedPreAnalysis && pState == 0)
                    cPreAnalysisResult = cFrameworkWrapper.checkPreAnalysis();

                if (cPreAnalysisResult)
                {
                    //formula 5 and New Formula
                    //5.1: (O_I_k_j and Pre_k_j) => O_E_k_j+1
                    //5.2: not (O_I_k_j and Pre_k_j) => (O_I_k_j <=> O_I_k_j+1)
                    //5.3: XOR O_I_k_j O_E_k_j O_F_k_j O_U_k_j
                    //5.4: (O_E_k_j AND Post_k_j) => O_F_k_j+1
                    //5.6: O_U_k_j => O_U_k_j+1
                    //5.7: O_F_k_j => O_F_k_j+1
                    //TODO: Bring the comment for formula 6 from the follwing function
                    convertFOperationsPrecedenceRulesNStatusControl2Z3ConstraintNewVersion(pState);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertOperationsPrecedenceRules");
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
                if (cPreAnalysisResult)
                {
                    //New formulas for implementing resources
                    if (cFrameworkWrapper.ResourceList.Count != 0)
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
        /// This function converts the goal(s)
        /// </summary>
        /// <param name="pState"></param>
        /// <param name="pDone"></param>
        ////private void convertGoal(int pState, bool pDone)
        private void convertGoal(int pState)
        {
            try
            {
                //TODO: these two conditions have to be moved to one level higher and this function should be removed.
                if (cPreAnalysisResult)
                {
                    if (cOpSeqAnalysis)
                    {
                        ////Removed when seperating the model building from model analysis
                        ////Might be added by runa, don't know why!
                        ////if (!pDone)
                            //formula 7 and 8
                            convertFGoals2Z3GoalsVersion2(pState);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertGoal");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// This function analyzes the result of the goals
        /// </summary>
        /// <param name="pState">The transition number which we are in</param>
        /// <param name="pDone">If the analysis is finished or not</param>
        /// <param name="pStrExprToCheck">If a special epression needs to be checked</param>
        /// <returns>Analysis result of the goal</returns>
        private Status anlyzeModel(int pState, bool pDone, string pStrExprToCheck = "")
        {
            Status lTestResult = Status.UNKNOWN;
            try
            {
                if (cPreAnalysisResult)
                {
                    if (!pDone)
                    {
                        if (cGeneralAnalysisType.Equals(Enumerations.GeneralAnalysisType.Dynamic))
                            Console.WriteLine("Analysis No: " + pState);
                        ////Removing unwanted code, this function was pointing to a one line function
                        ////lTestResult = analyseZ3Model(pState, pDone, lFrameworkWrapper, pStrExprToCheck);
                        lTestResult = cZ3Solver.CheckSatisfiability(pState
                                                                    , pDone
                                                                    , cFrameworkWrapper
                                                                    , cConvertGoal
                                                                    , cReportAnalysisDetailResult
                                                                    , cReportVariantsResult
                                                                    , cReportTransitionsResult
                                                                    , cReportAnalysisTiming
                                                                    , cReportUnsatCore
                                                                    , pStrExprToCheck);

                        cZ3Solver.WriteDebugFile(pState, -1);
                    }
                    else
                    {
                        ////TODO: If the analysis is finished why should it check the satisfiablity again???????
                        ////TODO: Have to remember the reason behind this else.

                        Console.WriteLine("Finished: ");
                        ////Removing unwanted code, this function was pointing to a one line function
                        ////lTestResult = analyseZ3Model(pState, pDone, lFrameworkWrapper, pStrExprToCheck);
                        lTestResult = cZ3Solver.CheckSatisfiability(pState
                                                                    , pDone
                                                                    , cFrameworkWrapper
                                                                    , cConvertGoal
                                                                    , cReportAnalysisDetailResult
                                                                    , cReportVariantsResult
                                                                    , cReportTransitionsResult
                                                                    , cReportAnalysisTiming
                                                                    , cReportUnsatCore
                                                                    , pStrExprToCheck);

                        cZ3Solver.WriteDebugFile(pState, -1);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in anlyzeModel");
                Console.WriteLine(ex.Message);
            }
            return lTestResult;
        }

        private void convertFResource2Z3Constraints()
        {
            try
            {
                List<resource> lResourceList = cFrameworkWrapper.ResourceList;
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
                        cZ3Solver.AddIntegerExpression(lAttributeName);
                        IntExpr lExprVariable = cZ3Solver.FindIntExpressionUsingName(lAttributeName);
                        cZ3Solver.AddEqualOperator2Constraints(lExprVariable
                                                                , int.Parse(lAttributeValue)
                                                                , "Attribute_Value");
                        break;
                    default:
                        //The default case is boolean variables
                        cZ3Solver.AddBooleanExpression(lAttributeName);
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
                List<string> lActiveOperationNames = cFrameworkWrapper.ActiveOperationNamesList;
                List<resource> lResourceList = cFrameworkWrapper.ResourceList;
//                List<string> lPossibleToRunOperationVariableNames = new List<string>();

                foreach (string lActiveOperationName in lActiveOperationNames)
                {
//                    List<string> lPossibleResourceVariablesForActiveOperation = new List<string>();
                    List<string> lUseResourceVariablesForActiveOperation = new List<string>();
                    
                    operation lActiveOperation = cFrameworkWrapper.getOperationFromOperationName(lActiveOperationName);

                    //This variable shows if the current operation can be run with at least one resource
                    string lPossibleToRunActiveOperationName = "Possible_to_run_" + lActiveOperationName;
                    cZ3Solver.AddBooleanExpression(lPossibleToRunActiveOperationName);

                    foreach (resource lActiveResource in lResourceList)
                    {
                        //This variable shows if the current operation CAN be run with the current resource
                        string lPossibleToUseResource4OperationName = "Possible_to_use_" + lActiveResource.names + "_for_" + lActiveOperationName;
                        cZ3Solver.AddBooleanExpression(lPossibleToUseResource4OperationName);
//                        lPossibleResourceVariablesForActiveOperation.Add(lPossibleToUseResource4OperationName);

                        //This variable shows if the current operation WILL be run with the current resource
                        string lUseResource4OperationName = "Use_" + lActiveResource.names + "_for_" + lActiveOperationName;
                        cZ3Solver.AddBooleanExpression(lUseResource4OperationName);
                        lUseResourceVariablesForActiveOperation.Add(lUseResource4OperationName);

                        //formula 6.1
                        //Possible_to_use_ActiveResource_for_ActiveOperation <-> Operation.Requirement
                        string lActiveOperationRequirements = cFrameworkWrapper.ReturnOperationRequirements(lActiveOperationName);
                        if (lActiveOperationRequirements != "")
                        {
                            //Active operation has requirements defined

                            List<resource> lOperationChosenResources = cFrameworkWrapper.ReturnOperationChosenResource(lActiveOperationName);

                            if (lOperationChosenResources.Contains(lActiveResource))
                            {
                                //This active resource is one of the resources that can run this operation
                                BoolExpr lActiveOperationRequirementExpr = returnFExpression2Z3Constraint(lActiveOperationRequirements);
                                cZ3Solver.AddTwoWayImpliesOperator2Constraints(cZ3Solver.FindBoolExpressionUsingName(lPossibleToUseResource4OperationName)
                                                                            , lActiveOperationRequirementExpr
                                                                            , "formula 6.1");
                            }
                            else
                            {
                                //This active resource is not one of the resources that can run this operation
                                cZ3Solver.AddNotOperator2Constraints(lPossibleToUseResource4OperationName
                                                                        , "formula 6.1");
                            }
                        }
                        else
                            //Active operation has no requirements defined, hence it is always possible to run
                            cZ3Solver.AddConstraintToSolver(cZ3Solver.FindBoolExpressionUsingName(lPossibleToRunActiveOperationName), "formula 6.1");


                        //formula 6.2
                        // Use_ActiveResource_ActiveOperation -> Possible_to_use_ActiveResource_for_ActiveOperation
                        cZ3Solver.AddImpliesOperator2Constraints(cZ3Solver.FindBoolExpressionUsingName(lUseResource4OperationName)
                                                                , cZ3Solver.FindBoolExpressionUsingName(lPossibleToUseResource4OperationName)
                                                                , "formula 6.2");
                    }

                    /////////////////////////////////////////////////////////
                    //formula 6.3
                    //This formula makes sure this operation can be run by ONLY one resource

                    //Possible_to_run_ActiveOperation -> Possible_to_use_Resource1_for_ActiveOperation or Possible_to_use_Resource2_for_ActiveOperation or ...

                    //                    lPossibleToRunOperationVariableNames.Add(lPossibleToRunActiveOperationName);
                    BoolExpr lPossibleToRunActiveOperation = cZ3Solver.FindBoolExpressionUsingName(lPossibleToRunActiveOperationName);
                    cZ3Solver.AddTwoWayImpliesOperator2Constraints(lPossibleToRunActiveOperation
                                                                , cZ3Solver.PickOneOperator(lUseResourceVariablesForActiveOperation)
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

        ////Removing unwanted code
        /// <summary>
        /// This function calls the solver function of the Z3Solver layer. 
        /// </summary>
        /// <param name="pState">The transition number which we are in</param>
        /// <param name="done">If the analysis is done or not</param>
        /// <param name="wrapper">Variable to the framework wrapper</param>
        /// <param name="pStrExprToCheck">If we want to check a specific expression in the model</param>
        /// <returns></returns>
        /*private bool analyseZ3Model(int pState, bool done, FrameworkWrapper wrapper, string pStrExprToCheck = "")
        {
            //returns the result of checking the satisfiability;
            return lZ3Solver.CheckSatisfiability(pState, done, wrapper, lConvertGoal, lReportAnalysisTiming, lReportUnsatCore, pStrExprToCheck);
        }*/

        public void convertFVariants2Z3Variants()
        {
            try
            {
                List<variant> lVariantList = cFrameworkWrapper.VariantList;

                foreach (variant lVariant in lVariantList)
                    cZ3Solver.AddBooleanExpression(lVariant.names);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertFVariants2Z3Variants");
                Console.WriteLine(ex.Message);
            }

        }

        //TODO: is it needed?
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
                    lResultVariant = cFrameworkWrapper.findVariantWithName(pVariantExpression);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnCurrentVariant");
                Console.WriteLine(ex.Message);
            }
            return lResultVariant;
        }

        /// <summary>
        /// In this function we go over all the defined operations and for the active operations we define current state and next state variables in Z3 model
        /// For the inactive operations we only define current state variables in the Z3 model
        /// </summary>
        /// <param name="pState"></param>
        public void convertFOperations2Z3Operations(int pState)
        {
            try
            {
                //Here we aim at the variant operation mapping table
                //We loop over each variant operation mapping, and for each case make the needed operation instances


                //Loop over variant list
                //For each variant find the variant in variant-operation mapping table
                //For each operation in this list make the following operation boolean variables
                List<variant> localVariantList = cFrameworkWrapper.VariantList;

                foreach (variant lCurrentVariant in localVariantList)
                {
                    //Before I added the different types of analysis it was this line which only looked at the active operations and only for them it created variables to be set
                    //List<operation> lOperationList = lFrameworkWrapper.getVariantExprOperations(lCurrentVariant.names);
                    
                    //But then after the analysis types were added there was a need for inactive operations to have variables as well so in the analysis we can analyze them as well
                    List<operation> lOperationList = cFrameworkWrapper.OperationList;
                    if (lOperationList != null)
                    {
                        foreach (operation lOperation in lOperationList)
                        {
                            bool lActiveOperation = cFrameworkWrapper.isOperationActive(lOperation);

                            if (lActiveOperation)
                                cFrameworkWrapper.addActiveOperationName(lOperation.names);
                            else
                                cFrameworkWrapper.addInActiveOperationName(lOperation.names);


                            if (lActiveOperation)
                            {
                                //Current state of operation
                                addCurrentVariantOperationInstanceVariables(lOperation.names, lCurrentVariant.index, pState);
                                addCurrentActiveVariantToActiveVariantList(lOperation.names, lCurrentVariant.index, pState);
                            }
                            else
                                //Current state of operation
                                addCurrentVariantOperationInstanceVariables(lOperation.names, 0, 0);


                            if (lActiveOperation)
                            {
                                //Next state of operation
                                int lNewState = pState + 1;
                                addCurrentVariantOperationInstanceVariables(lOperation.names, lCurrentVariant.index, lNewState);
                            }
                        }
                    }
                }

                if (pState.Equals(0))
                {
                    //forula 4 - Setting operation status if they were picked or not
                    // C = (BIG AND) (O_I_j_0 <=> V_j) AND (! O_e_j_0) AND (! O_f_j_0) AND (O_u_j_0 <=> (! V_j))
                    initializeFVariantOperation2Z3Constraints();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertFOperations2Z3Operations");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// In this function the instance variables for each operation are created, in these variables the operation will be assigned to a state, variant, and transitionNo
        /// </summary>
        /// <param name="pOperationName">The operation for which the instance variables will be defined</param>
        /// <param name="pVariantIndex">The variant which will be assigned to this operation instance variables</param>
        /// <param name="pState">The transitionNo which will be assigned to this operation instance variable</param>
        private void addCurrentVariantOperationInstanceVariables(string pOperationName, int pVariantIndex, int pState)
        {
            try
            {
                string lOperationInitialVariableName = ReturnOperationInstanceName(pOperationName,"I",pVariantIndex,pState);
                cZ3Solver.AddBooleanExpression(lOperationInitialVariableName);
                cFrameworkWrapper.addOperationInstance(lOperationInitialVariableName);

                string lOperationExecutingVariableName = ReturnOperationInstanceName(pOperationName,"E",pVariantIndex,pState);
                cZ3Solver.AddBooleanExpression(lOperationExecutingVariableName);
                cFrameworkWrapper.addOperationInstance(lOperationExecutingVariableName);

                string lOperationFinishedVariableName = ReturnOperationInstanceName(pOperationName,"F",pVariantIndex,pState);
                cZ3Solver.AddBooleanExpression(lOperationFinishedVariableName);
                cFrameworkWrapper.addOperationInstance(lOperationFinishedVariableName);

                string lOperationUnusedVariableName = ReturnOperationInstanceName(pOperationName,"U",pVariantIndex,pState);
                cZ3Solver.AddBooleanExpression(lOperationUnusedVariableName);
                cFrameworkWrapper.addOperationInstance(lOperationUnusedVariableName);

                string lOperationPreConditionName = ReturnOperationInstanceName(pOperationName,"PreCondition",pVariantIndex,pState);
                cZ3Solver.AddBooleanExpression(lOperationPreConditionName);
//                lFrameworkWrapper.addOperationInstance(lOperationPreConditionName);

                string lOperationPostConditionName = ReturnOperationInstanceName(pOperationName,"PostCondition",pVariantIndex,pState);
                cZ3Solver.AddBooleanExpression(lOperationPostConditionName);
//                lFrameworkWrapper.addOperationInstance(lOperationPostConditionName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addCurrentVariantOperationInstances");
                Console.WriteLine(ex.Message);
            }
        }

        private void addCurrentActiveVariantToActiveVariantList(string pOperationName, int pVariantIndex, int pState)
        {
            try
            {
                string lOperationInitialVariableName = ReturnOperationInstanceName(pOperationName,"I",pVariantIndex,pState);
                cFrameworkWrapper.addActiveOperationInstanceName(lOperationInitialVariableName);

                string lOperationExecutingVariableName = ReturnOperationInstanceName(pOperationName,"E",pVariantIndex,pState);
                cFrameworkWrapper.addActiveOperationInstanceName(lOperationExecutingVariableName);

                string lOperationFinishedVariableName = ReturnOperationInstanceName(pOperationName,"F",pVariantIndex,pState);
                cFrameworkWrapper.addActiveOperationInstanceName(lOperationFinishedVariableName);

                string lOperationUnusedVariableName = ReturnOperationInstanceName(pOperationName,"U",pVariantIndex,pState);
                cFrameworkWrapper.addActiveOperationInstanceName(lOperationUnusedVariableName);

                string lOperationPreConditionName = ReturnOperationInstanceName(pOperationName,"PreCondition",pVariantIndex,pState);
                cFrameworkWrapper.addActiveOperationInstanceName(lOperationPreConditionName);

                string lOperationPostConditionName = ReturnOperationInstanceName(pOperationName,"PostCondition",pVariantIndex,pState);
                cFrameworkWrapper.addActiveOperationInstanceName(lOperationPostConditionName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addCurrentActiveVariantToActiveVariantList");
                Console.WriteLine(ex.Message);
            }
        }

        public void resetCurrentStateAndNewStateOperationVariables(operation pOperation, variant pVariant, int pState, string pConstraintSource)
        {
            try
            {
                cOp_I_CurrentState = ReturnOperationInstanceVariable(pOperation.names,"I",pVariant.index,pState);
                cOp_E_CurrentState = ReturnOperationInstanceVariable(pOperation.names,"E",pVariant.index,pState);
                cOp_F_CurrentState = ReturnOperationInstanceVariable(pOperation.names,"F",pVariant.index,pState);
                cOp_U_CurrentState = ReturnOperationInstanceVariable(pOperation.names,"U",pVariant.index,pState);

                int lNewState = pState + 1;

                cOp_I_NextState = ReturnOperationInstanceVariable(pOperation.names,"I",pVariant.index,lNewState);
                cOp_E_NextState = ReturnOperationInstanceVariable(pOperation.names,"E",pVariant.index,lNewState);
                cOp_F_NextState = ReturnOperationInstanceVariable(pOperation.names,"F",pVariant.index,lNewState);
                cOp_U_NextState = ReturnOperationInstanceVariable(pOperation.names,"U",pVariant.index,lNewState);

                resetOperationPrecondition(pOperation, pVariant, pState, pConstraintSource);
                resetOperationPostcondition(pOperation, pVariant, pState, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in resetCurrentStateAndNewStateOperationVariables");
                Console.WriteLine(ex.Message);
            }
        }

        public void resetCurrentStateOperationVariables(operation pOperation, variant pVariant, int pState)
        {
            try
            {
                cOp_I_CurrentState = ReturnOperationInstanceVariable(pOperation.names, "I", pVariant.index, pState);
                cOp_E_CurrentState = ReturnOperationInstanceVariable(pOperation.names, "E", pVariant.index, pState);
                cOp_F_CurrentState = ReturnOperationInstanceVariable(pOperation.names, "F", pVariant.index, pState);
                cOp_U_CurrentState = ReturnOperationInstanceVariable(pOperation.names, "U", pVariant.index, pState);

                resetOperationPrecondition(pOperation, pVariant, pState, "Don't know");
                resetOperationPostcondition(pOperation, pVariant, pState, "Don't know");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in resetCurrentStateOperationVariables");
                Console.WriteLine(ex.Message);
            }
        }

        public void produceVariantGroupGCardinalityConstraints()
        {
            try
            {
                //Formula 2
                List<variantGroup> localVariantGroupsList = cFrameworkWrapper.VariantGroupList;

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
                            cZ3Solver.AddPickOneOperator2Constraints(lVariantNames, "GroupCardinality");

                            break;
                        }
                    case "choose at least one":
                        {
                            cZ3Solver.AddOrOperator2Constraints(lVariantNames, "GroupCardinality");

                            break;
                        }
                    case "choose all":
                        {
                            cZ3Solver.AddAndOperator2Constraints(lVariantNames, "GroupCardinality");

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
                List<variantOperations> lVariantsOperationsList = cFrameworkWrapper.getVariantsOperationsList();

                foreach (variantOperations lVariantOperations in lVariantsOperationsList)
                {
                    variant currentVariant = cFrameworkWrapper.ReturnCurrentVariant(lVariantOperations);

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

                            //(O_I_j_0 <=> V_j)
                            BoolExpr lFirstPart = cZ3Solver.TwoWayImpliesOperator(ReturnOperationInstanceName(lOperation.names,"I",currentVariant.index,0)
                                                                                , currentVariant.names);
                            //(! O_e_j_0)
                            BoolExpr lSecondPart = cZ3Solver.NotOperator(ReturnOperationInstanceName(lOperation.names,"E",currentVariant.index,0));
                            //(! O_f_j_0)
                            BoolExpr lThirdPart = cZ3Solver.NotOperator(ReturnOperationInstanceName(lOperation.names,"F",currentVariant.index,0));

                            //(O_u_j_0 <=> (! V_j))
                            BoolExpr lFirstOperand = cZ3Solver.FindBoolExpressionUsingName(ReturnOperationInstanceName(lOperation.names,"U",currentVariant.index,0));
                            BoolExpr lFourthPart = cZ3Solver.TwoWayImpliesOperator(lFirstOperand, cZ3Solver.NotOperator(currentVariant.names));

                            //(O_I_j_0 <=> V_j) AND (! O_e_j_0) AND (! O_f_j_0) AND (O_u_j_0 <=> (! V_j))
                            BoolExpr lWholeFormula = cZ3Solver.AndOperator(new List<BoolExpr>() { lFirstPart, lSecondPart, lThirdPart, lFourthPart });

                            //(BIG AND) (O_I_j_0 <=> V_j) AND (! O_e_j_0) AND (! O_f_j_0) AND (O_u_j_0 <=> (! V_j))
                            if (cConvertOperations)
                                cZ3Solver.AddAndOperator2Constraints(new List<BoolExpr>() {lWholeFormula}, "formula4-ActiveOperations");

                            if (cBuildPConstraints)
                                AddPrecedanceConstraintToLocalList(lWholeFormula);

                        }
                    }
                }

                //For operations which are inactive all states should be false except the unused state
                List<String> lInActiveOperationNames = cFrameworkWrapper.InActiveOperationNamesList;
                foreach (String lInActiveOperationName in lInActiveOperationNames)
                {
                    //String[] lInActiveOperationParts = lInActiveOperation.Split('_');
                    //String lInActiveOperationName = lInActiveOperationParts[0];

                    //(! O_I_0_0)
                    BoolExpr lFirstPart = cZ3Solver.NotOperator(ReturnOperationInstanceName(lInActiveOperationName,"I",0,0));
                    //(! O_E_0_0)
                    BoolExpr lSecondPart = cZ3Solver.NotOperator(ReturnOperationInstanceName(lInActiveOperationName,"E",0,0));
                    //(! O_F_0_0)
                    BoolExpr lThirdPart = cZ3Solver.NotOperator(ReturnOperationInstanceName(lInActiveOperationName,"F",0,0));
                    //(O_U_0_0)
                    BoolExpr lFourthPart = ReturnOperationInstanceVariable(lInActiveOperationName,"U",0,0);
                    //(! O_I_0_0) AND (! O_E_0_0) AND (! O_F_0_0) AND (O_U_0_0)
                    cZ3Solver.AddAndOperator2Constraints(new List<BoolExpr>() { lFirstPart, lSecondPart, lThirdPart, lFourthPart }, "formula4-InactiveOperations");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in initializeFVariantOperation2Z3Constraints");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// This function adds the precedence constraint to the local list of precedence constraints which will be used for one type of analysis
        /// </summary>
        /// <param name="pPrecedanceConstraint">Precedence constraint which will be added</param>
        private void AddPrecedanceConstraintToLocalList(BoolExpr pPrecedanceConstraint)
        {
            try
            {
                if (!cPConstraints.Contains(pPrecedanceConstraint))
                cPConstraints.Add(pPrecedanceConstraint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddPrecedanceConstraintToLocalList");
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
            try
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
                cOpPrecondition = ReturnOperationInstanceVariable(pOperation.names,"PreCondition",pVariant.index,pState);

                if (pOperation.precondition.Count != 0)
                {
                    //If the operation HAS a precondition
                    if (IsOperationInstanceComplete(pOperation.precondition[0]))
                    {
                        //If the precondition has all parts mentioned, i.e. operation name, operation state, operation variant, operation transition no
                        if (cFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(pOperation.precondition[0]) > pState)
                        {
                            //This means the precondition is on a transition state which has not been reached yet!
                            //lOpPrecondition = lZ3Solver.NotOperator(lOpPrecondition);
                            cZ3Solver.AddConstraintToSolver(cZ3Solver.NotOperator(cOpPrecondition), pConstraintSource);
                        }
                        else
                        {
                            if (pOperation.precondition[0].Contains('_'))
                                //This means the precondition includes an operation status
                                //lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0] + "_" + currentVariant.index  + "_" + pState.ToString());
                                cOpPrecondition = cZ3Solver.FindBoolExpressionUsingName(pOperation.precondition[0]);
                            else
                                //This means the precondition only includes an operation
                                cOpPrecondition = ReturnOperationInstanceVariable(pOperation.precondition[0], "F", pVariant.index, pState);
                        }
                    }
                    else
                    {
                        //If some part of the precondition are missing, i.e. operation name, operation state, operation variant, operation transition no
                        if (IsOperationInstanceMissingState(pOperation.precondition[0]))
                        {
                            //This preconditon should consider state FINISHED and any variants and any transitions
                            operation lOperation = cFrameworkWrapper.ReturnOperationFromOperationInstance(pOperation.precondition[0]);

                            cOpPrecondition = OperationForAnyVariantsInAnyTransitions(lOperation, "F");
                        }
                        else if (IsOperationInstanceMissingVariant(pOperation.precondition[0]))
                        {
                            //This preconditon should consider any transitions and any variants
                            operation lOperation = cFrameworkWrapper.ReturnOperationFromOperationInstance(pOperation.precondition[0]);
                            string lOperationState = cFrameworkWrapper.ReturnOperationStateFromOperationInstance(pOperation.precondition[0]);

                            cOpPrecondition = OperationForAnyVariantsInAnyTransitions(lOperation, lOperationState);
                        }
                        else if (IsOperationInstanceMissingTransitionNo(pOperation.precondition[0]))
                        {
                            //This precondition should consider any transitions
                            operation lOperation = cFrameworkWrapper.ReturnOperationFromOperationInstance(pOperation.precondition[0]);
                            variant lVariant = cFrameworkWrapper.ReturnOperationVariantFromOperationInstance(pOperation.precondition[0]);
                            string lOperationState = cFrameworkWrapper.ReturnOperationStateFromOperationInstance(pOperation.precondition[0]);

                            cOpPrecondition = OperationInAnyTransitions(lOperation, lVariant, lOperationState);
                        }
                    }

                }
                else
                    //If the operation DOES NOT have a precondition hence
                    //We want to force the precondition to be true
                    cZ3Solver.AddConstraintToSolver(cOpPrecondition, pConstraintSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in resetOperationPrecondition");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// This function takes an operation instance and looks at the parts (operation name, operation state, operation variant, operation transition no)
        /// and makes sure all parts are mentioned in an operation instance or not
        /// </summary>
        /// <param name="pOperationInstance"></param>
        private bool IsOperationInstanceComplete(string pOperationInstance)
        {
            bool lResult = false;
            try
            {
                string[] lOperationInstanceParts = pOperationInstance.Split('_');
                if (lOperationInstanceParts.Length.Equals(4))
                    lResult = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in IsOperationInstanceComplete");
                Console.WriteLine(ex.Message);
                lResult = false;
            }
            return lResult;
        }

        /// <summary>
        /// This function checks if an operation instance has the transition part missing
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        private bool IsOperationInstanceMissingTransitionNo(string pOperationInstance)
        {
            bool lResult = true;
            try
            {
                string[] lOperationInstanceParts = pOperationInstance.Split('_');
                if (lOperationInstanceParts.Length.Equals(4))
                    lResult = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in IsOperationInstanceMissingTransitionNo");
                Console.WriteLine(ex.Message);
            }
            return lResult;
        }

        /// <summary>
        /// This function checks if an operation instance has the variant part missing
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        private bool IsOperationInstanceMissingVariant(string pOperationInstance)
        {
            bool lResult = true;
            try
            {
                string[] lOperationInstanceParts = pOperationInstance.Split('_');
                if (lOperationInstanceParts.Length.Equals(3))
                    lResult = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in IsOperationInstanceMissingVariant");
                Console.WriteLine(ex.Message);
            }
            return lResult;
        }

        /// <summary>
        /// This function checks if an operation instance has the state part missing
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        private bool IsOperationInstanceMissingState(string pOperationInstance)
        {
            bool lResult = true;
            try
            {
                string[] lOperationInstanceParts = pOperationInstance.Split('_');
                if (lOperationInstanceParts.Length.Equals(2))
                    lResult = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in IsOperationInstanceMissingState");
                Console.WriteLine(ex.Message);
            }
            return lResult;
        }

        public void resetOperationPostcondition(operation pOperation, variant pVariant, int pState, String pPostconditionSource)
        {
            try
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
                cOpPostcondition = ReturnOperationInstanceVariable(pOperation.names,"PostCondition",pVariant.index,pState);

                if (pOperation.postcondition.Count != 0)
                {
                    if (IsOperationInstanceComplete(pOperation.postcondition[0]))
                    {
                        if (cFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(pOperation.postcondition[0]) > pState)
                        {
                            //This means the postcondition is on a transition state which has not been reached yet!
                            cOpPostcondition = cZ3Solver.NotOperator(cOpPostcondition);
                            cZ3Solver.AddConstraintToSolver(cOpPostcondition, pPostconditionSource);
                        }
                        else
                        {
                            if (pOperation.postcondition[0].Contains('_'))
                                //This means the postcondition includes an operation status
                                //lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.postcondition[0] + "_" + currentVariant.index  + "_" + pState.ToString());
                                cOpPostcondition = cZ3Solver.FindBoolExpressionUsingName(pOperation.postcondition[0]);
                            else
                                //This means the postcondition only includes an operation
                                cOpPostcondition = ReturnOperationInstanceVariable(pOperation.postcondition[0], "F", pVariant.index, pState);
                        }
                    }
                    else
                    {
                        //If some part of the postcondition are missing, i.e. operation name, operation state, operation variant, operation transition no
                        if (IsOperationInstanceMissingTransitionNo(pOperation.postcondition[0]))
                        {
                            //This postcondition should consider any transitions
                            operation lOperation = cFrameworkWrapper.ReturnOperationFromOperationInstance(pOperation.postcondition[0]);
                            variant lVariant = cFrameworkWrapper.ReturnOperationVariantFromOperationInstance(pOperation.postcondition[0]);
                            string lOperationState = cFrameworkWrapper.ReturnOperationTransitionFromOperationInstance(pOperation.postcondition[0]);

                            cOpPostcondition = OperationInAnyTransitions(lOperation, lVariant, lOperationState);
                        }
                        else if (IsOperationInstanceMissingVariant(pOperation.postcondition[0]))
                        {
                            //This postconditon should consider any transitions and any variants
                            operation lOperation = cFrameworkWrapper.ReturnOperationFromOperationInstance(pOperation.postcondition[0]);
                            string lOperationState = cFrameworkWrapper.ReturnOperationTransitionFromOperationInstance(pOperation.postcondition[0]);

                            cOpPostcondition = OperationForAnyVariantsInAnyTransitions(lOperation, lOperationState);
                        }
                        else if (IsOperationInstanceMissingState(pOperation.postcondition[0]))
                        {
                            //This postconditon should consider state FINISHED and any variants and any transitions
                            operation lOperation = cFrameworkWrapper.ReturnOperationFromOperationInstance(pOperation.postcondition[0]);

                            cOpPostcondition = OperationForAnyVariantsInAnyTransitions(lOperation, "F");
                        }
                    }

                }
                else
                    //We want to force the postcondition to be true
                    //lOpPostcondition = lOpPostcondition;
                    cZ3Solver.AddConstraintToSolver(cOpPostcondition, pPostconditionSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in resetOperationPostcondition");
                Console.WriteLine(ex.Message);
            }
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
                    lResult = cZ3Solver.MakeBoolExprFromString(pNode.Data);
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
                                lResult = cZ3Solver.AndOperator(new List<BoolExpr>() { ParseCondition(lChildren[0], pState), ParseCondition(lChildren[1], pState) });
                                break;
                            }
                        case "or":
                            {
                                lResult = cZ3Solver.OrOperator(new List<BoolExpr>() { ParseCondition(lChildren[0], pState), ParseCondition(lChildren[1], pState) });
                                break;
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                lResult = cZ3Solver.NotOperator(ParseCondition(lChildren[0], pState));
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
            BoolExpr lResult = null;
            try
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
                            List<string> vInstances = cFrameworkWrapper.getvariantInstancesForOperation(lOperationNameParts[0]);
                            List<BoolExpr> opExpr = new List<BoolExpr>();
                            foreach (string variant in vInstances)
                            {
                                opExpr.Add(cZ3Solver.FindBoolExpressionUsingName(pCon + "_" + variant + "_" + pState.ToString()));

                            }
                            lResult = (cZ3Solver.OrOperator(opExpr));
                        }
                        else if (lOperationNameParts.Length == 3)
                        {
                            //This means the precondition does includes a variant but not a state
                            lResult = (cZ3Solver.FindBoolExpressionUsingName(pCon + "_" + pState.ToString()));
                        }
                        else
                            //This means the precondition only includes an operation
                            throw new System.ArgumentException("Precondition did not include a status", pCon);
                    }
                    else
                        //This means the precondition includes a state and a variant
                        if (cFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(pCon) > pState)
                        {
                            //This means the precondition is on a transition state which has not been reached yet!
                            //lZ3Solver.AddConstraintToSolver(lZ3Solver.NotOperator(lOpPrecondition), pPreconditionSource);
                            //unsatisfiable = true;
                            //break;
                            lResult = cZ3Solver.getFalseBoolExpr();
                        }
                        else
                        {
                            lResult = cZ3Solver.FindBoolExpressionUsingName(pCon);
                        }
                }
                else
                    //This means the postcondition only includes an operation
                    throw new System.ArgumentException("Precondition did not include a status", pCon);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in mkCondition");
                Console.WriteLine(ex.Message);
            }
            return lResult;
        }

        //TODO: I think this can be removed!!!
        private variant returnCurrentVariant(variantOperations pVariantOperations)
        {
            variant lResultVariant = new variant();
            try
            {
                variant lCurrentVariant = cFrameworkWrapper.ReturnCurrentVariant(pVariantOperations);

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

        public void convertFOperationsPrecedenceRulesNStatusControl2Z3ConstraintNewVersion(int pState)
        {
            try
            {
                //Loop over the variant-operation mappings
                //For each mapping find the current variant and current operations
                List<variantOperations> lVariantOperationsList = cFrameworkWrapper.getVariantsOperationsList();

                //Next state of operation
                int lNewState = pState + 1;

                foreach (variantOperations lCurrentVariantOperations in lVariantOperationsList)
                {
                    variant lCurrentVariant = cFrameworkWrapper.ReturnCurrentVariant(lCurrentVariantOperations);

                    List<operation> lOperationList = lCurrentVariantOperations.getOperations();
                    if (lOperationList != null)
                    {
                        foreach (operation lOperation in lOperationList)
                        {
                            resetCurrentStateAndNewStateOperationVariables(lOperation, lCurrentVariant, pState, "formula5");

                            //TODO: Maybe not needed. Verify??????
                            //resetOperationPrecondition(lOperation, lCurrentVariant, pState, "formula5-Precondition");

                            ////TODO: check this line, it might be that it is not needed considering that post conditions are set as part of the previous method.
                            BoolExpr lOpPostcondition = ReturnOperationInstanceVariable(lOperation.names,"PostCondition",lCurrentVariant.index,pState);

                            //5.1: (O_I_k_j and Pre_k_j) => O_E_k_j+1
                            createFormula51(lOperation);

                            //5.2: not (O_I_k_j and Pre_k_j) => (O_I_k_j <=> O_I_k_j+1)
                            createFormula52(lOperation);

                            //5.3: XOR O_I_k_j O_E_k_j O_F_k_j O_U_k_j
                            createFormula53();

                            //5.4: (O_E_k_j AND Post_k_j) => O_F_k_j+1
                            createFormula54(lOperation);

                            //5.6: O_U_k_j => O_U_k_j+1
                            createFormula56();

                            //5.7: O_F_k_j => O_F_k_j+1
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

                //Reminder: Before it was this line of code but this only gave us operation instances from this current transition and for that set the following condition on the transition index was always false
                List<String> lActiveOperationList = cFrameworkWrapper.getActiveOperationNamesList(pState);
                //List<String> lActiveOperationList = cFrameworkWrapper.ActiveOperationInstanceNamesList;

                if (lActiveOperationList != null)
                {
                    foreach (String lFirstActiveOperation in lActiveOperationList)
                    {
                        int lFirstVariantIndex = cFrameworkWrapper.getVariantIndexFromActiveOperation(lFirstActiveOperation);

                        foreach (String lSecondActiveOperation in lActiveOperationList)
                        {
                            int lSecondVariantIndex = cFrameworkWrapper.getVariantIndexFromActiveOperation(lSecondActiveOperation);

                            if (lFirstVariantIndex < lSecondVariantIndex)
                            {
                                BoolExpr lFirstOperand = cZ3Solver.FindBoolExpressionUsingName(lFirstActiveOperation);
                                BoolExpr lSecondOperand = cZ3Solver.NotOperator(cFrameworkWrapper.giveNextStateActiveOperationName(lFirstActiveOperation));
                                BoolExpr lFirstParantesis = cZ3Solver.AndOperator(new List<BoolExpr>() { lFirstOperand, lSecondOperand });

                                BoolExpr lThirdOperand = cZ3Solver.FindBoolExpressionUsingName(lSecondActiveOperation);
                                String lNextStateActiveOperationName = cFrameworkWrapper.giveNextStateActiveOperationName(lSecondActiveOperation);
                                if (cZ3Solver.FindBoolExpressionUsingName(lNextStateActiveOperationName) != null)
                                {
                                    BoolExpr lFourthOperand = cZ3Solver.NotOperator(lNextStateActiveOperationName);
                                    BoolExpr lSecondParantesis = cZ3Solver.AndOperator(new List<BoolExpr>() { lThirdOperand, lFourthOperand });

                                    if (formulaSix == null)
                                        formulaSix = cZ3Solver.NotOperator(cZ3Solver.AndOperator(new List<BoolExpr>() { lFirstParantesis, lSecondParantesis }));
                                    else
                                        formulaSix = cZ3Solver.AndOperator(new List<BoolExpr>() { formulaSix, cZ3Solver.NotOperator(cZ3Solver.AndOperator(new List<BoolExpr>() { lFirstParantesis, lSecondParantesis })) });
                                }
                            }
                        }
                    }
                }
                if (formulaSix != null)
                {
                    if (cConvertOperationPrecedenceRules)
                        cZ3Solver.AddConstraintToSolver(formulaSix, "formula6");

                    if (cBuildPConstraints)
                        AddPrecedanceConstraintToLocalList(formulaSix);
                }
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

                BoolExpr lWholeFormula = cZ3Solver.ImpliesOperator(new List<BoolExpr>() { cOp_F_CurrentState, cOp_F_NextState });

                if (cConvertOperationPrecedenceRules)
                    cZ3Solver.AddConstraintToSolver(lWholeFormula, "formula5.7");

                if (cBuildPConstraints)
                    AddPrecedanceConstraintToLocalList(lWholeFormula);
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

                BoolExpr lWholeFormula = cZ3Solver.TwoWayImpliesOperator(cOp_U_CurrentState, cOp_U_NextState);

                if (cConvertOperationPrecedenceRules)
                    cZ3Solver.AddConstraintToSolver(lWholeFormula, "formula5.6");

                if (cBuildPConstraints)
                    AddPrecedanceConstraintToLocalList(lWholeFormula);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateFormula56");
                Console.WriteLine(ex.Message);
            }
        }

        private void createFormula54(operation pOperation)
        {
            try
            {
                //Formula 5.4
                //(O_E_k_j AND Post_k_j) => O_F_k_j+1

                // Optimized //TODO:Check this line, it might be the fact that this line is not needed as the post conditions are set outside this method in the mother method!!
                //resetOperationPostcondition(pOperation, pCurrentVariant, pState, "formula5.4");

                //Optimized:  BoolExpr lLeftHandSide = lZ3Solver.AndOperator(new List<BoolExpr>() { lOp_E_CurrentState, lOpPostcondition });
                BoolExpr lLeftHandSide = ReturnOperationExecutingStateNItsPostcondition(pOperation);

                BoolExpr lWholeFormula = cZ3Solver.ImpliesOperator(new List<BoolExpr>() { lLeftHandSide, cOp_F_NextState });

                if (cConvertOperationPrecedenceRules)
                    cZ3Solver.AddConstraintToSolver(lWholeFormula, "formula5.4");

                if (cBuildPConstraints)
                    AddPrecedanceConstraintToLocalList(lWholeFormula);
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

                /* Optimized:
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

                BoolExpr lWholeFormula = lZ3Solver.AndOperator(new List<BoolExpr>() { lFirstPart, lSecondPart, lThirdPart, lFourthPart });*/

                //BoolExpr lWholeFormula = cZ3Solver.XorOperator(new List<BoolExpr>() { cOp_I_CurrentState, cOp_E_CurrentState, cOp_F_CurrentState, cOp_U_CurrentState });
                BoolExpr lWholeFormula = cZ3Solver.PickOneOperator(new List<BoolExpr>() { cOp_I_CurrentState, cOp_E_CurrentState, cOp_F_CurrentState, cOp_U_CurrentState });

                if (cConvertOperationPrecedenceRules)
                    cZ3Solver.AddConstraintToSolver(lWholeFormula, "formula5.3");

                if (cBuildPConstraints)
                    AddPrecedanceConstraintToLocalList(lWholeFormula);
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

                cZ3Solver.AddPickOneOperator2Constraints(new List<BoolExpr>() { cOp_I_CurrentState, cOp_E_CurrentState, cOp_F_CurrentState, cOp_U_CurrentState }, "formula5.3");

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

                tempLeftHandSideTwo = cZ3Solver.NotOperator(tempLeftHandSideTwo);

                BoolExpr tempRightHandSideTwo;

                tempRightHandSideTwo = cZ3Solver.TwoWayImpliesOperator(cOp_I_CurrentState, cOp_I_NextState);

                BoolExpr lWholeFormula = cZ3Solver.ImpliesOperator(new List<BoolExpr>() { tempLeftHandSideTwo, tempRightHandSideTwo });

                if (cConvertOperationPrecedenceRules)
                    cZ3Solver.AddConstraintToSolver(lWholeFormula, "formula5.2");

                if (cBuildPConstraints)
                    AddPrecedanceConstraintToLocalList(lWholeFormula);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateFormula52");
                Console.WriteLine(ex.Message);
            }
        }

        private void createFormula51(operation pOperation)
        {
            try
            {
                //Formula 5.1
                //(O_I_k_j and Pre_k_j) => O_E_k_j+1
                BoolExpr tempLeftHandSideOne;
                //if (lOperation.precondition != null)

                //(O_I_k_j and Pre_k_j)
                //Not optimized! tempLeftHandSideOne = lZ3Solver.AndOperator(new List<BoolExpr>() { lOp_I_CurrentState, lOpPrecondition });
                tempLeftHandSideOne = ReturnOperationInitialStateNItsPrecondition(pOperation);
                //else
                //    tempLeftHandSideOne = lOp_I_CurrentState;

                //(O_I_k_j and Pre_k_j) => O_E_k_j+1
                BoolExpr lWholeFormula = cZ3Solver.ImpliesOperator(new List<BoolExpr>() { tempLeftHandSideOne, cOp_E_NextState });

                if (cConvertOperationPrecedenceRules)
                    cZ3Solver.AddConstraintToSolver(lWholeFormula, "formula5.1");

                if (cBuildPConstraints)
                    AddPrecedanceConstraintToLocalList(lWholeFormula);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateFormula51");
                Console.WriteLine(ex.Message);
            }
        }

        public BoolExpr ReturnOperationExecutingStateNItsPostcondition(operation pOperation)
        {
            BoolExpr result;
            try
            {
                //If the operation has no precondition then the relevant variable is forced to be true, other wise it will be the precondition
                if (pOperation.postcondition.Count != 0)
                    result = cZ3Solver.AndOperator(new List<BoolExpr>() { cOp_E_CurrentState, cOpPostcondition });
                else
                    result = cOp_E_CurrentState;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationExecutingStateNItsPostcondition");
                Console.WriteLine(ex.Message);
                result = null;
            }
            return result;
        }

        public BoolExpr ReturnOperationInitialStateNItsPrecondition(operation pOperation)
        {
            BoolExpr result;
            try
            {
                //If the operation has no precondition then the relevant variable is forced to be true, other wise it will be the precondition
                if (pOperation.precondition.Count != 0)
                    result = cZ3Solver.AndOperator(new List<BoolExpr>() { cOp_I_CurrentState, cOpPrecondition });
                else
                    result = cOp_I_CurrentState;
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
                    lResult = (BoolExpr)cZ3Solver.FindBoolExpressionUsingName(pNode.Data);
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
                                lResult = cZ3Solver.AndOperator(new List<BoolExpr>() { ParseConstraintExpression(lChildren[0]), ParseConstraintExpression(lChildren[1]) });
                                break;
                            }
                        case "or":
                            {
                                ////lResult = lZ3Solver.OrOperator(ParseConstraint(lChildren[0]), ParseConstraint(lChildren[1])).ToString();
                                //lResult = lZ3Solver.OrOperator(ParseConstraint(lChildren[0]), ParseConstraint(lChildren[1]));
                                lResult = cZ3Solver.OrOperator(new List<BoolExpr>() { ParseConstraintExpression(lChildren[0]), ParseConstraintExpression(lChildren[1]) });
                                break;
                            }
                        case "->":
                            {
                                lResult = cZ3Solver.ImpliesOperator(new List<BoolExpr>() { ParseConstraintExpression(lChildren[0]), ParseConstraintExpression(lChildren[1]) });
                                break;
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                lResult = cZ3Solver.NotOperator(ParseConstraintExpression(lChildren[0]));
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
                    lResult = (BoolExpr)cZ3Solver.FindExpressionUsingName(pNode.Data);
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
                                lResult = cZ3Solver.AndOperator(new List<BoolExpr>() { ParseExpression(lChildren[0]), ParseExpression(lChildren[1]) });
                                break;
                            }
                        case "or":
                            {
                                ////lResult = lZ3Solver.OrOperator(ParseConstraint(lChildren[0]), ParseConstraint(lChildren[1])).ToString();
                                //lResult = lZ3Solver.OrOperator(ParseConstraint(lChildren[0]), ParseConstraint(lChildren[1]));
                                lResult = cZ3Solver.OrOperator(new List<BoolExpr>() { ParseExpression(lChildren[0]), ParseExpression(lChildren[1]) });
                                break;
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                lResult = cZ3Solver.NotOperator(ParseExpression(lChildren[0]));
                                break;
                            }
                        case ">=":
                            {
                                lResult = cZ3Solver.GreaterOrEqualOperator(lChildren[0].Data, int.Parse(lChildren[1].Data));
                                break;
                            }
                        case "<=":
                            {
                                lResult = cZ3Solver.LessOrEqualOperator(lChildren[0].Data, int.Parse(lChildren[1].Data));
                                break;
                            }
                        case "<":
                            {
                                lResult = cZ3Solver.LessThanOperator(lChildren[0].Data, int.Parse(lChildren[1].Data));
                                break;
                            }
                        case ">":
                            {
                                lResult = cZ3Solver.GreaterThanOperator(lChildren[0].Data, int.Parse(lChildren[1].Data));
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

        public string returnConstraintsString()
        {
            string lConstraintsString = "";
            try
            {
                //First we have to loop the constraint list
                List<string> localConstraintList = cFrameworkWrapper.ConstraintList;

                foreach (string lConstraint in localConstraintList)
                {
                    if (lConstraintsString != "")
                        lConstraintsString += " AND ";
                    lConstraintsString += lConstraint;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in returnConstraintsString");
                Console.WriteLine(ex.Message);
            }
            return lConstraintsString;
        }

        /// <summary>
        /// This function uses the constraint set to return all the constraints in one boolExpr
        /// </summary>
        /// <returns>The one boolExpr which represents the constraint set</returns>
        public BoolExpr returnBoolExprOfConstraintsSet()
        {
            BoolExpr lReturnBoolExpr = null;
            try
            {
                //First we have to loop the constraint list
                List<string> localConstraintList = cFrameworkWrapper.ConstraintList;

                ////foreach (string lConstraint in localConstraintList)

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in returnBoolExprOfConstraintsSet");
                Console.WriteLine(ex.Message);
            }
            return lReturnBoolExpr;
        }

/*        public BoolExpr returnBoolExprOfConstraint(string pConstraint)
        {
            BoolExpr lReturnExpr = null;
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in returnBoolExprOfConstraint");
                Console.WriteLine(ex.Message);
            }
            return lReturnExpr;
        }*/

        /// <summary>
        /// This function adds a stand alone constraint to the Z3 model which the user wants to add directly
        /// </summary>
        /// <param name="pStandAloneConstraint">User specified stand alone constraint</param>
        public void addStandAloneConstraint2Z3Solver(BoolExpr pStandAloneConstraint)
        {
            try
            {
                if (pStandAloneConstraint != null)
                    cZ3Solver.AddConstraintToSolver(pStandAloneConstraint, "StandAlone Constraint");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addStandAloneConstraint2Z3Solver");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// This function just adds the extra confguration rule to the Z3 model
        /// </summary>
        /// <param name="pExtraConfigurationRule">Extra confguration rule which should be added</param>
        public void AddExtraConstraint2Z3Constraint(string pExtraConfigurationRule)
        {
            try
            {
                cZ3Solver.AddConstraintToSolver(returnFBooleanExpression2Z3Constraint(pExtraConfigurationRule)
                                                , "formula3-extra configuration rule");

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddExtraConstraint2Z3Constraint");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Adds all the framework constrints to the Z3 model
        /// Also adds any given extra constraints to the Z3 model
        /// </summary>
        /// <param name="pExtraConfigurationRule"></param>
        public void convertFConstraint2Z3Constraint(string pExtraConfigurationRule = "")
        {
            try
            {
                //formula 3
                //First we have to loop the constraint list
                List<string> localConstraintList = cFrameworkWrapper.ConstraintList;

                foreach (string lConstraint in localConstraintList)
                {
                    BoolExpr lBoolExprConstraint = returnFBooleanExpression2Z3Constraint(lConstraint);
                    cZ3Solver.AddConstraintToSolver(lBoolExprConstraint
                                                    , "formula3");
                    if (cAnalysisType == Enumerations.AnalysisType.AlwaysSelectedVariantAnalysis 
                        || cAnalysisType == Enumerations.AnalysisType.AlwaysSelectedOperationAnalysis)
                        AddConfigurationConstraintToLocalList(lBoolExprConstraint);

                }

                //TODO: by default this extra configuration rule can be an array
                if (pExtraConfigurationRule != "")
                    cZ3Solver.AddConstraintToSolver(returnFBooleanExpression2Z3Constraint(pExtraConfigurationRule)
                                                    , "formula3-ExtraConfigRule");

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in convertFConstraint2Z3Constraint");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// This function populates the local list of configuration constraints which is used for one of the analysis
        /// </summary>
        /// <param name="pConfigurationConstraint">Configuration constraint which is to be added to the list</param>
        private void AddConfigurationConstraintToLocalList(BoolExpr pConfigurationConstraint)
        {
            try
            {
                if (!cConfigurationConstraints.Contains(pConfigurationConstraint))
                    cConfigurationConstraints.Add(pConfigurationConstraint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AddConfigurationConstraintToLocalList");
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

        public BoolExpr createFormula7(List<variant> pVariantList, int pState)
        {
            BoolExpr lResultFormula7 = null;
            try
            {
                //NEW formula 7
                //No operation can proceed
                //(Big And) ((! Pre_k_j AND O_I_k_j) OR (! Post_k_j AND O_E_k_j) OR O_F_k_j OR O_U_k_j)

                //This boolean expression is used to refer to this overall goal
                cZ3Solver.AddBooleanExpression("F7_" + pState);

                foreach (variant lCurrentVariant in pVariantList)
                {
                    List<operation> lOperationList = cFrameworkWrapper.getVariantExprOperations(lCurrentVariant.names);
                    if (lOperationList != null)
                    {
                        foreach (operation lCurrentOperation in lOperationList)
                        {
                            //Optimize: resetCurrentStateOperationVariables(lOperation, lCurrentVariant, pState);

                            /*Optimize:
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
                             */
                            resetCurrentStateOperationVariables(lCurrentOperation, lCurrentVariant, pState);

                            BoolExpr lNotPreCondition = cZ3Solver.NotOperator(cOpPrecondition);
                            BoolExpr lNotPostCondition = cZ3Solver.NotOperator(cOpPostcondition);

                            BoolExpr lFirstOperand = cZ3Solver.AndOperator(new List<BoolExpr>() { lNotPreCondition, cOp_I_CurrentState });
                            BoolExpr lSecondOperand = cZ3Solver.AndOperator(new List<BoolExpr>() { lNotPostCondition, cOp_E_CurrentState });

                            BoolExpr lOperand = cZ3Solver.OrOperator(new List<BoolExpr>() { lFirstOperand, lSecondOperand, cOp_F_CurrentState, cOp_U_CurrentState });

                            if (lResultFormula7 == null)
                                lResultFormula7 = lOperand;
                            else
                                lResultFormula7 = cZ3Solver.AndOperator(new List<BoolExpr>() { lResultFormula7, lOperand });
                        }
                    }
                }
                if (lResultFormula7 != null)
                    //lZ3Solver.AddConstraintToSolver(lOverallGoal, "overallGoal");
                    cZ3Solver.AddImpliesOperator2Constraints(cZ3Solver.FindBoolExpressionUsingName("F7_" + pState)
                                                                , lResultFormula7
                                                                , "Formula7");
                //            if (formula7 != null)
                //                lZ3Solver.AddConstraintToSolver(formula7);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createFormula7");
                Console.WriteLine(ex.Message);
            }
            return lResultFormula7;
        }

        public BoolExpr createFormula8(List<variant> pVariantList, int pState)
        {
            BoolExpr lResultFormula8 = null;
            try
            {
                //formula 8
                //At least one operation is in initial or executing state
                //(Big OR) (O_I_k_j OR O_E_k_j)

                //This boolean expression is used to refer to this formula 8
                cZ3Solver.AddBooleanExpression("F8_" + pState);

                foreach (variant lCurrentVariant in pVariantList)
                {
                    List<operation> lOperationList = cFrameworkWrapper.getVariantExprOperations(lCurrentVariant.names);
                    if (lOperationList != null)
                    {
                        foreach (operation lOperation in lOperationList)
                        {
                            BoolExpr lOp_I_CurrentState = ReturnOperationInstanceVariable(lOperation.names,"I",lCurrentVariant.index,pState);
                            BoolExpr lOp_E_CurrentState = ReturnOperationInstanceVariable(lOperation.names,"E",lCurrentVariant.index,pState);

                            BoolExpr lOperand = cZ3Solver.OrOperator(new List<BoolExpr>() { lOp_I_CurrentState, lOp_E_CurrentState });

                            if (lResultFormula8 == null)
                                lResultFormula8 = lOperand;
                            else
                                lResultFormula8 = cZ3Solver.OrOperator(new List<BoolExpr>() { lResultFormula8, lOperand });
                        }
                    }
                }
                if (lResultFormula8 != null)
                    //lZ3Solver.AddConstraintToSolver(lOverallGoal, "overallGoal");
                    cZ3Solver.AddImpliesOperator2Constraints(cZ3Solver.FindBoolExpressionUsingName("F8_" + pState)
                                                                , lResultFormula8
                                                                , "Formula8");
                //            if (formula8 != null)
                //                lZ3Solver.AddConstraintToSolver(formula8);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createFormula8");
                Console.WriteLine(ex.Message);
            }
            return lResultFormula8;
        }

        /// <summary>
        /// This analysis checks if there are any Deadlock
        /// </summary>
        public bool ExistanceOfDeadlockAnalysis(bool pVariationPointsSet
                                                            , bool pReportTypeSet)
        {
            //This is the result of the analysis we give the user
            bool lAnalysisResult = false;

            //This is the result of the internal analysis we get
            Status lInternalAnalysisResult = Status.UNKNOWN;
            try
            {
                //TODO: Only should be done when a flag is set
                Console.WriteLine("Existance Of Valid Production Path Analysis:");

                //This variable controls if the analysis has been completed or not
                bool lAnalysisComplete = false;

                //This is the list of all the variants in the product platform
                List<variant> lVariantList = cFrameworkWrapper.VariantList;
                
                ////TODO: Added as new version
                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                int lMaxNoOfTransitions = CalculateAnalysisNoOfCycles();

                ////TODO: Added as new version
                cZ3Solver.PrepareDebugDirectory();

                if (!pVariationPointsSet)
                    setVariationPoints(Enumerations.GeneralAnalysisType.Dynamic
                                        , Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis);

                if (!pReportTypeSet)
                    setReportType(true, true, true, true, false, false, false, true);

                ///??????????????????????????
                //Then we have to go over all variants one by one, for this we use the list we previously filled
//                foreach (variant lCurrentVariant in lVariantList)
//                {
//                    Console.WriteLine("---------------------------------------------------------------------");
//                    Console.WriteLine("Selected Variant: " + lCurrentVariant.names);

                    ////Making the static part of the model
                    MakeStaticPartOfProductPlatformModel();

                    for (int lTransitionNo = 0; lTransitionNo < lMaxNoOfTransitions; lTransitionNo++)
                    {

                        Console.WriteLine("--------------------Transition: " + lTransitionNo + " --------------------");
                        //For this new variant the analysis has just started, hence it is not complete
                        lAnalysisComplete = false;

                        ////Making the dynamic part of the model (Operations and transition between operation status)
                        MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, lTransitionNo);

                        //TODO: Do we need a stand alone constraint????????? Considering that this line comes with a stand alone constraint!!!
                        //lZ3Solver.SolverPushFunction();

                        //TODO: Do we need a stand alone constraint?????????
                        //addStandAloneConstraint2Z3Solver(lZ3Solver.FindBoolExpressionUsingName(lCurrentVariant.names));

                        //For each variant check if this statement holds - carry out the analysis
                        lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                                , 0
                                                                , lAnalysisComplete);

                        //if the result of the previous analysis is true then we go to the next analysis part
                        if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                        {
                            //If the result is true it means we have found a deadlock!
                            Console.WriteLine("A deadlock was found!");
                            break;
                        }
                        if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                        {
                            //If the result is false it means no deadlock can be found! HENCE DEADLOCK FREE!!
                        }

                        //TODO: Do we need a stand alone constraint????????? Considering that this line comes with a stand alone constraint!!!
//                        lZ3Solver.SolverPopFunction();

                        //TODO: Is this the best way to end the loop on the transiton numbers???????
                        //If all the transition cycles are completed then the analysis is completed
                        if (lTransitionNo == lMaxNoOfTransitions - 1)
                            lAnalysisComplete = true;
                    }
//                }

                    //Translating the internal analysis result to the user specific analysis result 
                    if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                        lAnalysisResult = true;
                    else
                        lAnalysisResult = false;

                    Console.WriteLine("Analysis Report: ");
                //TODO: How to give a analysis report?????
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ExistanceOfDeadlockAnalysis");
                Console.WriteLine(ex.Message);
            }
            //Returns the result of the analysis
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis finds models that satisfy the set of formulas
        /// </summary>
        public bool ModelEnumerationAnalysis(bool pVariationPointsSet
                                              , bool pReportTypeSet)
        {
            //This is the result of the analysis we give the user
            bool lAnalysisResult = false;

            //This is the result of the internal analysis we get
            Status lInternalAnalysisResult = Status.UNKNOWN;
            try
            {
                //Let the user know what analysis is being done
                Console.WriteLine("Model enumeration Analysis:");

                //This variable controls if the analysis has been completed or not
                bool lAnalysisComplete = false;
                //This is the list of all the variants in the product platform
                List<variant> lVariantList = cFrameworkWrapper.VariantList;

                ////TODO: Added as new version
                cZ3Solver.PrepareDebugDirectory();

                //This analysis is going to be carried out in two steps:
                //Step A, check the product platform structurally without the goal
                //        this check is done for each seperate variant from the variant group
                //        this checks to see if any of the rules are conflicting each other
                if (!pVariationPointsSet)
                    setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                        , Enumerations.AnalysisType.ModelEnumerationAnalysis);

                if (!pReportTypeSet)
                    setReportType(false, false, true, true, false, false, false, true);

                ////TODO: Added as new version
                MakeStaticPartOfProductPlatformModel();

                for (int i = 0; i < cNoOfModelsRequired; i++)
                {
                    //For each variant check if this statement holds - carry out the analysis
                    lInternalAnalysisResult = AnalyzeProductPlatform(0
                                                            , i
                                                            , lAnalysisComplete);

                    //if the result of the previous analysis is true then we go to the next analysis part
                    if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    {
                        //??addStandAloneConstraint2Z3Solver(lZ3Solver.FindBoolExpressionUsingName(lCurrentVariant.names));
                        ReportSolverResult(i + 1, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, null);
                    }
                    else if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                    {
                        //If the product platform did not have any more models!
                        if (i > 0)
                            Console.WriteLine("This Product Platform only had " + i + " Models.");
                        break;
                    }

                }

                //Translating the internal analysis result to the user specific analysis result 
                if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    lAnalysisResult = true;
                else
                    lAnalysisResult = false;


                //In this analysis there was not a meaningful analysis report!
                //Console.WriteLine("Analysis Report: ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ModelEnumerationAnalysis");
                Console.WriteLine(ex.Message);
            }
            //Returns the result of the analysis
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis checks if there are any variants which are not able to be selected and manufactured
        /// </summary>
        public bool VariantSelectabilityAnalysis(bool pVariationPointsSet
                                                    , bool pReportTypeSet)
        {
            //This is the result of the analysis we give the user
            bool lAnalysisResult = false;

            //This is the result of the internal analysis we get
            Status lInternalAnalysisResult = Status.UNKNOWN;
            try
            {
                //TODO: Only should be done when a flag is set
                Console.WriteLine("Variant Selectability Analysis:");

                //This variable controls if the analysis has been completed or not
                bool lAnalysisComplete = false;


                //This is the list of all the variants in the product platform
                List<variant> lVariantList = cFrameworkWrapper.VariantList;
                //This is an empty list which is going to be filled by all he variants which are not able to be picked
                List<variant> lUnselectableVariantList = new List<variant>();

/////                ////TODO: Added as new version
/////                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
/////                int lMaxNoOfTransitions = CalculateAnalysisNoOfCycles();

                ////TODO: Added as new version
                cZ3Solver.PrepareDebugDirectory();

                //This analysis is going to be carried out in two steps:
                //Step A, check the product platform structurally without the goal
                //        this check is done for each seperate variant from the variant group
                //        this checks to see if any of the rules are conflicting each other

                if (!pVariationPointsSet)
                    setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                        , Enumerations.AnalysisType.VariantSelectabilityAnalysis);

                if (!pReportTypeSet)
                    setReportType(false, false, true, true, false, false, false, true);

                ////As this analysis type is a static analysis we only carry t out for the first transition
                int lTransitionNo = 0;

                //Then we have to go over all variants one by one, for this we use the list we previously filled
                foreach (variant lCurrentVariant in lVariantList)
                {
                    //This line is moved to the reporting procedure in this class.
                    //Console.WriteLine("---------------------------------------------------------------------");
                    //Console.WriteLine("Selected Variant: " + lCurrentVariant.names);


                    ////TODO: Added as new version
                    MakeStaticPartOfProductPlatformModel();

                    /////TODO: Removed in the new version
                    /////for (int lTransitionNo = 0; lTransitionNo < lMaxNoOfTransitions; lTransitionNo++)
                    /////{

/////                        Console.WriteLine("--------------------Transition: " + lTransitionNo + " --------------------");
                        //For this new variant the analysis has just started, hence it is not complete
/////                        lAnalysisComplete = false;

                        ////TODO: Added as new version
                        MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, lTransitionNo);

                        cZ3Solver.SolverPushFunction();

                        addStandAloneConstraint2Z3Solver(cZ3Solver.FindBoolExpressionUsingName(lCurrentVariant.names));

                        //For each variant check if this statement holds - carry out the analysis
                        lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                                , 0
                                                                , lAnalysisComplete);

                        //if the result of the previous analysis is true then we go to the next analysis part
                        if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                        {
                            ReportSolverResult(lTransitionNo, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, lCurrentVariant);

                            //This line is moved to the reporting procedure in this class.
                            //Console.WriteLine(lCurrentVariant.names + " is Selectable.");
                        }
                        else if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                        {
                            //If the result of the first analysis was false that means the selected variant is in conflict with the rest of the product platform
                            if (!lUnselectableVariantList.Contains(lCurrentVariant))
                                lUnselectableVariantList.Add(lCurrentVariant);
                            break;
                        }

                        cZ3Solver.SolverPopFunction();

                        /////TODO: Removed in the new version
                        /////If all the transition cycles are completed then the analysis is completed
                        /////if (lTransitionNo == lMaxNoOfTransitions - 1)
                        /////    lAnalysisComplete = true;


                    /////}

                    //Here according to the number of unselectable variants which we have found we will give the coresponding report

                }

                Console.WriteLine("Analysis Report: ");
                if (lUnselectableVariantList.Count != 0)
                {
                    Console.WriteLine("Variats which are not selectable are: " + ReturnVariantNamesFromList(lUnselectableVariantList));
                    lAnalysisResult = false;
                }
                else
                {
                    Console.WriteLine("All Variats are selectable!");
                    lAnalysisResult = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in VariantSelectabilityGoal");
                Console.WriteLine(ex.Message);
            }
            //Returns the result of the analysis
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis checks if there are any variants which are always selected
        /// </summary>
        public bool AlwaysSelectedVariantAnalysis(bool pVariationPointsSet
                                                    , bool pReportTypeSet)
        {
            //This is the result of the analysis we give the user
            bool lAnalysisResult = false;

            //This is the result of the internal analysis we get
            Status lInternalAnalysisResult = Status.UNKNOWN;
            try
            {
                //TODO: Only should be done when a flag is set
                Console.WriteLine("Always selected variant Analysis:");

                bool lAnalysisComplete = false;
                List<variant> lVariantList = cFrameworkWrapper.VariantList;
                List<variant> lNotAlwaysSelectedVariantList = new List<variant>();

                ////TODO: Added as new version
                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                int lMaxNoOfTransitions = CalculateAnalysisNoOfCycles();

                ////TODO: Added as new version
                cZ3Solver.PrepareDebugDirectory();

                //2.Is there any variant that will always be selected
                //C and P => V_i

                //To start empty the constraint set to initialize the analysiss
                //In order to analyze this goal we have first add the C (configuration rules) and P (Operation precedence rules) to the constraints
                //In order to add the C and the P we set the coresponding variation points
                if (!pVariationPointsSet)
                    setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                        , Enumerations.AnalysisType.AlwaysSelectedVariantAnalysis);

                if (!pReportTypeSet)
                    setReportType(true, true, true, true, false, false, false, true);

                //This analysis only has meaning in the first transition, as it is a static analysis
                int lTransitionNo = 0;

                //This line is moved to the reporting procedure in this class
                //Console.WriteLine("---------------------------------------------------------------------");
                //Console.WriteLine("Selected Variant: " + lCurrentVariant.names);

                ////TODO: Added as new version
                lAnalysisComplete = false;

                ////TODO: Added as new version
                MakeStaticPartOfProductPlatformModel();

                /////                    for (int lTransitionNo = 0; lTransitionNo < lMaxNoOfTransitions; lTransitionNo++)
                /////                    {
                /////                        Console.WriteLine("--------------------Transition: " + lTransitionNo + " --------------------");

                ////TODO: Added as new version
                MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, lTransitionNo);

                //Then we have to go over all variants one by one
                foreach (variant lCurrentVariant in lVariantList)
                {

                        cZ3Solver.SolverPushFunction();

                        BoolExpr lStandAloneConstraint = null;
/////                        //Here we have to build an expression which shows C and P => V_i and assign it to the variable just defined
                        //Here we have to build an expression which shows C => ! V_i and assign it to the variable just defined
                        BoolExpr lRightHandSide = null;
                        BoolExpr lLeftHandSide = null;

/////                        ////TODO: only do this if the two list are not empty!
/////                        BoolExpr lP = lZ3Solver.AndOperator(lPConstraints);

                        lLeftHandSide = cZ3Solver.NotOperator(cZ3Solver.FindBoolExpressionUsingName(lCurrentVariant.names));

                        if (cConfigurationConstraints.Count > 0)
                        {
                            BoolExpr lC = cZ3Solver.AndOperator(cConfigurationConstraints);
/////                            lRightHandSide = lZ3Solver.AndOperator(new List<BoolExpr>() { lC, lP });
                            lRightHandSide = lC;
                            /////lStandAloneConstraint = lZ3Solver.ImpliesOperator(lRightHandSide, lZ3Solver.FindBoolExpressionUsingName(lCurrentVariant.names));
                            lStandAloneConstraint = cZ3Solver.AndOperator(new List<BoolExpr>() { lRightHandSide, lLeftHandSide });
                        }
                        else
                        {
                            /////                            lRightHandSide = lP;
                            lStandAloneConstraint = lLeftHandSide;
                        }


                        addStandAloneConstraint2Z3Solver(lStandAloneConstraint);

                        //For each variant check if this statement holds - carry out the analysis
                        lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                                , 0
                                                                , lAnalysisComplete);

/////                        if (!lAnalysisResult && !lAnalysisComplete)
                        if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                        {
                            //This line is moved to the reporting procedure in this class
                            //if it does hold, then there is a configuration which is valid and this current variant is not present in it
                            //Console.WriteLine("There DOES exist a valid configuration which does not include " + lCurrentVariant.names + ".");

                            if (!lNotAlwaysSelectedVariantList.Contains(lCurrentVariant))
                                lNotAlwaysSelectedVariantList.Add(lCurrentVariant);
/////                            break;
                        }
                        else if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                        {
                            //This line is moved to the reporting procedure in this class
                            //Console.WriteLine("All valid configurations DO include " + lCurrentVariant.names + ".");
                        }

                        //if it does hold we go to the next variant
                        cZ3Solver.SolverPopFunction();

                        ////TODO: Added as new version
                        //If all the transition cycles are completed then the analysis is completed
/////                        if (lTransitionNo == lMaxNoOfTransitions - 1)
/////                            lAnalysisComplete = true;

/////                    }
                    //

                }

                //Translating the internal analysis result to the user specific analysis result 
                //As the analysis has been looking for variants which are not always selectable, hence if the lNotAlwaysSelectedVariantList
                //contains any record then the analysis will be true, other wise it will be false
                if (lNotAlwaysSelectedVariantList.Count > 0)
                    lAnalysisResult = true;
                else
                    lAnalysisResult = false;

                
                if (lNotAlwaysSelectedVariantList.Count != 0)
                    Console.WriteLine("Variats which are NOT always selected are: " + ReturnVariantNamesFromList(lNotAlwaysSelectedVariantList));
                else
                    Console.WriteLine("All valid configurations DO include ALL of the variants!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AlwaysSelectedVariantAnalysis");
                Console.WriteLine(ex.Message);
            }
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis checks if there are any operations which are always selected
        /// </summary>
        public bool AlwaysSelectedOperationAnalysis(bool pVariationPointsSet
                                                    , bool pReportTypeSet)
        {
            //This is the result of the analysis we give the user
            bool lAnalysisResult = false;

            //This is the result of the internal analysis we get
            Status lInternalAnalysisResult = Status.UNKNOWN;
            try
            {
                //TODO: Only should be done when a flag is set
                Console.WriteLine("Always selected operation Analysis:");

                bool lAnalysisComplete = false;
////                List<variant> lVariantList = lFrameworkWrapper.VariantList;
////                List<operation> lOperationList = lFrameworkWrapper.OperationList;
                List<string> lOperationInstanceVariableNames = cFrameworkWrapper.OperationInstanceList;
                List<string> lNotAlwaysSelectedOperationNameList = new List<string>();

                ////TODO: Added as new version
                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                int lMaxNoOfTransitions = CalculateAnalysisNoOfCycles();

                ////TODO: Added as new version
                cZ3Solver.PrepareDebugDirectory();

                //2.Is there any operation that will always be selected
                //C and P => O_i

                //To start empty the constraint set to initialize the analysiss
                //In order to analyze this goal we have first add the C (configuration rules) and P (Operation precedence rules) to the constraints
                //In order to add the C and the P we set the coresponding variation points
                if (!pVariationPointsSet)
                    setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                        , Enumerations.AnalysisType.AlwaysSelectedOperationAnalysis);

                if (!pReportTypeSet)
                    setReportType(true, false, true, true, false, false, false, true);

                ////TODO: Added as new version
                ////In this analysis we only want to check the first transition, and check if each operation is in the initial status in the first transition or not
                ////Hence there is no need to loop over all transitions like previous types of analysis
                int lTransitionNo = 0;

                ////TODO: Added as new version
                MakeStaticPartOfProductPlatformModel();
                ////TODO: Added as new version
                MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, lTransitionNo);

                List<operation> lOperationList = cFrameworkWrapper.OperationList;

                foreach (operation lCurrentOperation in lOperationList)
                {
                    BoolExpr l = OperationForAllVariantsInAllTransitions(lCurrentOperation, "U", -1, 0);

                    cZ3Solver.SolverPushFunction();

                    BoolExpr lStandAloneConstraint = null;

                    /////                            //TODO: Here we have to build an expression which shows C and P => !O_u and assign it to the variable just defined!!!!!!!!!!!!!!!!!!!!!!!!!
                    //Here we have to build an expression which shows C and O_u and assign it to the variable just defined!!!!!!!!!!!!!!!!!!!!!!!!!
                    BoolExpr lRightHandSide = null;
                    BoolExpr lLeftHandSide = null;

                    /////                            ////TODO: only do this if the two list are not empty!
                    /////                            BoolExpr lP = lZ3Solver.AndOperator(lPConstraints);

                    /////lLeftHandSide = lZ3Solver.NotOperator(lZ3Solver.FindBoolExpressionUsingName(lOperationInstanceVariableName));
                    lLeftHandSide = l;

                    if (cConfigurationConstraints.Count > 0)
                    {
                        BoolExpr lC = cZ3Solver.AndOperator(cConfigurationConstraints);
                        /////                                lRightHandSide = lZ3Solver.AndOperator(new List<BoolExpr>() { lC, lP });
                        lRightHandSide = lC;
                        /////lStandAloneConstraint = lZ3Solver.ImpliesOperator(lRightHandSide, lLeftHandSide);
                        lStandAloneConstraint = cZ3Solver.AndOperator(new List<BoolExpr>() { lRightHandSide, lLeftHandSide });
                    }
                    else
                    {
                        /////                                lRightHandSide = lP;
                        lStandAloneConstraint = lLeftHandSide;
                    }

                    addStandAloneConstraint2Z3Solver(lStandAloneConstraint);

                    //For each variant check if this statement holds - carry out the analysis
                    lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                            , 0
                                                            , lAnalysisComplete);
                    /////                            if (!lAnalysisResult && !lAnalysisComplete)

                    string lOperationName = lCurrentOperation.names;

                    if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    {
                        //This line is moved to the reporting procedure in this class
                        //if it does hold, then there exists a valid configuration in which the current operation is UNUSED!
                        //Console.WriteLine("There DOES exist a configuration in which " + lOperationName + " is in an UNUSED state!");

                        if (!lNotAlwaysSelectedOperationNameList.Contains(lOperationName))
                            lNotAlwaysSelectedOperationNameList.Add(lOperationName);
                    }
                    else if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                    {
                        //Console.WriteLine("All valid configurations DO include " + lOperationName + ".");
                    }

                    cZ3Solver.SolverPopFunction();
                    //if it does hold we go to the next variant
                            
                }


                ///Previous version

                /*foreach (string lOperationInstanceVariableName in lOperationInstanceVariableNames)
                {
                    //Here we have to check if the operation instance variable is the Unused state of an operation, because in this analysis it is only needed to check if 
                    //for each operation the instance variable of the unused state can be true or not.
                    if (cFrameworkWrapper.isOperationInstanceUnusedState(lOperationInstanceVariableName))
                    {
                        if (cFrameworkWrapper.isOperationInstanceActive(lOperationInstanceVariableName))
                        {
                            //We only want operation instances which are in the inital state and in the initial transition, i.e. transition 0
                            if (cFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(lOperationInstanceVariableName).Equals(0))
                            {
                                cZ3Solver.SolverPushFunction();

                                BoolExpr lStandAloneConstraint = null;

                                /////                            //TODO: Here we have to build an expression which shows C and P => !O_u and assign it to the variable just defined!!!!!!!!!!!!!!!!!!!!!!!!!
                                //Here we have to build an expression which shows C and O_u and assign it to the variable just defined!!!!!!!!!!!!!!!!!!!!!!!!!
                                BoolExpr lRightHandSide = null;
                                BoolExpr lLeftHandSide = null;

                                /////                            ////TODO: only do this if the two list are not empty!
                                /////                            BoolExpr lP = lZ3Solver.AndOperator(lPConstraints);

                                /////lLeftHandSide = lZ3Solver.NotOperator(lZ3Solver.FindBoolExpressionUsingName(lOperationInstanceVariableName));
                                lLeftHandSide = cZ3Solver.FindBoolExpressionUsingName(lOperationInstanceVariableName);

                                if (cConfigurationConstraints.Count > 0)
                                {
                                    BoolExpr lC = cZ3Solver.AndOperator(cConfigurationConstraints);
                                    /////                                lRightHandSide = lZ3Solver.AndOperator(new List<BoolExpr>() { lC, lP });
                                    lRightHandSide = lC;
                                    /////lStandAloneConstraint = lZ3Solver.ImpliesOperator(lRightHandSide, lLeftHandSide);
                                    lStandAloneConstraint = cZ3Solver.AndOperator(new List<BoolExpr>() { lRightHandSide, lLeftHandSide });
                                }
                                else
                                {
                                    /////                                lRightHandSide = lP;
                                    lStandAloneConstraint = lLeftHandSide;
                                }

                                addStandAloneConstraint2Z3Solver(lStandAloneConstraint);

                                //For each variant check if this statement holds - carry out the analysis
                                lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                                        , 0
                                                                        , lAnalysisComplete);
                                /////                            if (!lAnalysisResult && !lAnalysisComplete)

                                string lOperationName = cFrameworkWrapper.ReturnOperationNameFromOperationInstance(lOperationInstanceVariableName);

                                if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                                {
                                    //This line is moved to the reporting procedure in this class
                                    //if it does hold, then there exists a valid configuration in which the current operation is UNUSED!
                                    //Console.WriteLine("There DOES exist a configuration in which " + lOperationName + " is in an UNUSED state!");

                                    if (!lNotAlwaysSelectedOperationNameList.Contains(lOperationName))
                                        lNotAlwaysSelectedOperationNameList.Add(lOperationName);
                                }
                                else if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                                {
                                    //Console.WriteLine("All valid configurations DO include " + lOperationName + ".");
                                }

                                cZ3Solver.SolverPopFunction();
                                //if it does hold we go to the next variant
                            }
                        }
                    }

                    ////TODO: when is the analysis complete??????????????
                    //if (??????lTransitionNo == lMaxNoOfTransitions - 1)
                        //lAnalysisComplete = true;
                }*/
                //Translating the internal analysis result to the user specific analysis result 
                if (lNotAlwaysSelectedOperationNameList.Count > 0)
                    lAnalysisResult = true;
                else
                    lAnalysisResult = false;
                
                if (lNotAlwaysSelectedOperationNameList.Count != 0)
                    Console.WriteLine("Operations which are NOT always selected are: "
                        + ReturnOperationNamesStringFromOperationNameList(lNotAlwaysSelectedOperationNameList));
                else
                    Console.WriteLine("All valid configurations DO include ALL the operations!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in AlwaysSelectedOperationAnalysis");
                Console.WriteLine(ex.Message);
            }
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis checks if there are any operations which are not able to be selected
        /// </summary>
        public bool OperationSelectabilityAnalysis(bool pVariationPointsSet
                                                    , bool pReportTypeSet)
        {
            //This is the result of the analysis we give the user
            bool lAnalysisResult = false;

            //This is the result of the internal analysis we get
            Status lInternalAnalysisResult = Status.UNKNOWN;

            try
            {
                //TODO: Only should be done when a flag is set
                Console.WriteLine("Operation Selectability Analysis:");

                bool lAnalysisComplete = false;
                List<string> lUnselectableOperationNameList = new List<string>();
                List<string> lInActiveOperationNameList = new List<string>();
                List<string> lOperationInstanceVariableNames = cFrameworkWrapper.OperationInstanceList;

                //To start all operations are unselectable unless otherwise proven
                lUnselectableOperationNameList = cFrameworkWrapper.getListOfOperationNames();

                //3.Is there any operation that will not be possible to select? (This s a ststic type of analysis hence we will not include the P set
                //O_i and C

                //To start empty the constraint set to initialize the analysiss
                //In order to analyze this goal we have first add the C (configuration rules) and P (Operation precedence rules) to the constraints
                //In order to add the C and the P we set the coresponding variation points
                if (!pVariationPointsSet)
                    setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                        , Enumerations.AnalysisType.OperationSelectabilityAnalysis);

                if (!pReportTypeSet)
                    setReportType(true, false, true, true, false, false, false, true);

                ////TODO: Added as new version
                ////In this analysis we only want to check the first transition, and check if each operation is in the initial status in the first transition or not
                ////Hence there is no need to loop over all transitions like previous types of analysis
                int lTransitionNo = 0;

                ////TODO: Added as new version
                MakeStaticPartOfProductPlatformModel();
                ////TODO: Added as new version
                MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, lTransitionNo);

                List<operation> lOperationList = cFrameworkWrapper.OperationList;

                foreach (operation lCurrentOperation in lOperationList)
                {
                    //First we check if the current operation has a relation to a specific variant, hence is it active?
                    if (cFrameworkWrapper.isOperationActive(lCurrentOperation))
                    {
                        BoolExpr l = OperationForAnyVariantsInAllTransitions(lCurrentOperation, "I", -1, lTransitionNo);

                        cZ3Solver.SolverPushFunction();

                        addStandAloneConstraint2Z3Solver(l);
                        //For each operation check if this statement holds - carry out the analysis
                        ////lAnalysisResult = AnalyzeProductPlatform(out lAnalysisComplete, "not " + lCurrentOperation.names + "_U_0_1");
                        lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                                , 0
                                                                , lAnalysisComplete);

                        if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                        //if it does hold, then it is possible to select that operation and it should be removed from the list
                        {
                            //This line is moved to the reporting procedure in this class
                            //Console.WriteLine(lFrameworkWrapper.getOperationFromOperationName(lOperationInstanceVariableName).names + " is selectable.");
                            ReportSolverResult(lTransitionNo, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, lCurrentOperation.names);

                            if (lUnselectableOperationNameList.Contains(lCurrentOperation.names))
                                lUnselectableOperationNameList.Remove(lCurrentOperation.names);
                        }

                        cZ3Solver.SolverPopFunction();
                        //if it does hold we go to the next variant

                    }
                    else
                    {
                        //If the operation is inactive?
                        //This means the operation instance is inactive hence it should be mentioned

                        //This is when we want to report only the operation name
                        //Console.WriteLine("Operation " + lFrameworkWrapper.returnOperationNameFromOperationInstance(lOperationInstanceVariableName) + " is inactive!");
                        ReportSolverResult(lTransitionNo, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, lCurrentOperation.names);

                        //This is when we want to report the operation instance
                        if (!lInActiveOperationNameList.Contains(lCurrentOperation.names))
                        {
                            lInActiveOperationNameList.Add(lCurrentOperation.names);
                            if (lUnselectableOperationNameList.Contains(lCurrentOperation.names))
                                lUnselectableOperationNameList.Remove(lCurrentOperation.names);
                        }

                    }
                }

/*                foreach (string lOperationInstanceVariableName in lOperationInstanceVariableNames)
                {
                    //Here we have to check if the operation instance variable is the Initial state of an operation, because in this analysis it is only needed to check if 
                    //for each operation the instance variable of the initial state can be true or not.
                    if (cFrameworkWrapper.isOperationInstanceInitialState(lOperationInstanceVariableName))
                    {
                        //Here we check if this operation is an active operation or not, meaning does a variant use this operation for its assembly or not
                        if (cFrameworkWrapper.isOperationInstanceActive(lOperationInstanceVariableName))
                        {
                            //We only want operation instances which are in the inital state and in the initial transition, i.e. transition 0
                            if (cFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(lOperationInstanceVariableName).Equals(0))
                            {
                                //These two lines are moved to the reporting procedure in this class
                                //Console.WriteLine("----------------------------------------------------------------");
                                //Console.WriteLine("Analysing operation instance named: " + lOperationInstanceVariableName);

                                cZ3Solver.SolverPushFunction();

                                addStandAloneConstraint2Z3Solver(cZ3Solver.FindBoolExpressionUsingName(lOperationInstanceVariableName));
                                //For each operation check if this statement holds - carry out the analysis
                                ////lAnalysisResult = AnalyzeProductPlatform(out lAnalysisComplete, "not " + lCurrentOperation.names + "_U_0_1");
                                lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                                        , 0
                                                                        , lAnalysisComplete);

                                if (lAnalysisResult.Equals(Status.SATISFIABLE))
                                //if it does hold, then it is possible to select that operation and it should be removed from the list
                                {
                                    //This line is moved to the reporting procedure in this class
                                    //Console.WriteLine(lFrameworkWrapper.getOperationFromOperationName(lOperationInstanceVariableName).names + " is selectable.");
                                    ReportSolverResult(lTransitionNo, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, lOperationInstanceVariableName);

                                    string lOperationName = cFrameworkWrapper.ReturnOperationNameFromOperationInstance(lOperationInstanceVariableName);
                                    if (lUnselectableOperationNameList.Contains(lOperationName))
                                        lUnselectableOperationNameList.Remove(lOperationName);
                                }

                                cZ3Solver.SolverPopFunction();
                                //if it does hold we go to the next variant

                            }
                        }
                        else
                        {
                            //This means the operation instance is inactive hence it should be mentioned

                            //This is when we want to report only the operation name
                            //Console.WriteLine("Operation " + lFrameworkWrapper.returnOperationNameFromOperationInstance(lOperationInstanceVariableName) + " is inactive!");
                            ReportSolverResult(lTransitionNo, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, lOperationInstanceVariableName);

                            //This is when we want to report the operation instance
                            string lOperationName = cFrameworkWrapper.ReturnOperationNameFromOperationInstance(lOperationInstanceVariableName);
                            if (!lInActiveOperationNameList.Contains(lOperationName))
                            {
                                lInActiveOperationNameList.Add(lOperationName);
                                if (lUnselectableOperationNameList.Contains(lOperationName))
                                    lUnselectableOperationNameList.Remove(lOperationName);
                            }
                        }
                    }
                }*/
                //Translating the internal analysis result to the user specific analysis result 
                if (lUnselectableOperationNameList.Count.Equals(0))
                    lAnalysisResult = true;
                else
                    lAnalysisResult = false;

                
                if (cReportAnalysisResult)
                { 
                    if (lInActiveOperationNameList.Count != 0)
                        Console.WriteLine("Operation " + ReturnOperationNamesStringFromOperationNameList(lInActiveOperationNameList) + " is inactive!");

                    if (lUnselectableOperationNameList.Count != 0)
                        Console.WriteLine("Operations which are not selectable are: " + ReturnOperationNamesStringFromOperationNameList(lUnselectableOperationNameList));
                    else
                        Console.WriteLine("All operations are selectable!");
                }

                ////TODO: when is the analysis complete??????????????
                //if (??????lTransitionNo == lMaxNoOfTransitions - 1)
                //lAnalysisComplete = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in OperationSelectabilityAnalysis");
                Console.WriteLine(ex.Message);
            }
            return lAnalysisResult;
        }

        /// <summary>
        /// This function will give us a boolean expression of all the operation instances of a specific operation on a specific variant and a specific operation state
        /// in all different transitions (or in one specific transition). All the operation instances are joined by AND
        /// </summary>
        /// <param name="pOperation"></param>
        /// <param name="pVariant"></param>
        /// <param name="pOperationState"></param>
        /// <param name="pSpecificTransitionNo">In we need the expression for one transition</param>
        /// <returns></returns>
        private BoolExpr OperationInAllTransitions(operation pOperation, variant pVariant, string pOperationState, int pSpecificTransitionNo = -1)
        {
            BoolExpr lResultExpr = null;
            try
            {
                //Here we have an operation and the related variant, but as the transition number is not given we are looking for
                //for operation instances related to this variant in all of the transitions

                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                int lMaxNoOfTransitions = CalculateAnalysisNoOfCycles();
                for (int i = 0; i < lMaxNoOfTransitions; i++)
                {
                    BoolExpr lCurrentOperationInstance;
                    if (pSpecificTransitionNo != -1 && pSpecificTransitionNo.Equals(i))
                    {
                        lCurrentOperationInstance = ReturnOperationInstanceVariable(pOperation.names
                                                                                            , pOperationState
                                                                                            , pVariant.index
                                                                                            , i);
                    }
                    else
                    {
                        lCurrentOperationInstance = ReturnOperationInstanceVariable(pOperation.names
                                                                                            , pOperationState
                                                                                            , pVariant.index
                                                                                            , i);

                    }
                    if (lCurrentOperationInstance != null)
                        if (lResultExpr != null)
                            lResultExpr = cZ3Solver.AndOperator(new List<BoolExpr> { lResultExpr, lCurrentOperationInstance });
                        else
                            lResultExpr = lCurrentOperationInstance;
                    else
                        break;

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in OperationInAllTransitions");
                Console.WriteLine(ex.Message);
            }
            return lResultExpr;
        }

        /// <summary>
        /// This function will give us a boolean expression of all the operation instances of a specific operation on a specific variant and a specific operation state
        /// in any different transitions (or in one specific transition). All the operation instances are joined by OR
        /// </summary>
        /// <param name="pOperation"></param>
        /// <param name="pVariant"></param>
        /// <param name="pOperationState"></param>
        /// <param name="pSpecificTransitionNo">In we need the expression for one transition</param>
        /// <returns></returns>
        private BoolExpr OperationInAnyTransitions(operation pOperation, variant pVariant, string pOperationState, int pSpecificTransitionNo = -1)
        {
            BoolExpr lResultExpr = null;
            try
            {
                //Here we have an operation and the related variant, but as the transition number is not given we are looking for
                //for operation instances related to this variant in all of the transitions

                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                int lMaxNoOfTransitions = CalculateAnalysisNoOfCycles();
                for (int i = 0; i < lMaxNoOfTransitions; i++)
                {
                    BoolExpr lCurrentOperationInstance;
                    if (pSpecificTransitionNo != -1 && pSpecificTransitionNo.Equals(i))
                    {
                        lCurrentOperationInstance = ReturnOperationInstanceVariable(pOperation.names
                                                                                            , pOperationState
                                                                                            , pVariant.index
                                                                                            , i);
                    }
                    else
                    {
                        lCurrentOperationInstance = ReturnOperationInstanceVariable(pOperation.names
                                                                                            , pOperationState
                                                                                            , pVariant.index
                                                                                            , i);
                    }
                    if (lCurrentOperationInstance != null)
                    {
                        if (lResultExpr != null)
                            lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr> { lResultExpr, lCurrentOperationInstance });
                        else
                            lResultExpr = lCurrentOperationInstance;
                    }
                    else
                        break;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in OperationInAllTransitions");
                Console.WriteLine(ex.Message);
            }
            return lResultExpr;
        }

        /// <summary>
        /// This function takes an operation and returns am expression of operation instnces for that operation and ALL of the variants in ALL transitions
        /// These any variants and in any transitions can be fixed to one specific variant and one specific transition if their respective parameters are set
        /// Also the operation state needs to be given
        /// </summary>
        /// <param name="pOperation"></param>
        /// <param name="pOperationState"></param>
        /// <param name="pSpecificVariantIndex">If the expression needs to be made for just one variant</param>
        /// <param name="pSpecificTransition">If the expression needs to be made for just one transition</param>
        /// <returns></returns>
        private BoolExpr OperationForAllVariantsInAllTransitions(operation pOperation
                                                                , string pOperationState
                                                                , int pSpecificVariantIndex = -1
                                                                , int pSpecificTransition = -1)
        {
            BoolExpr lResultExpr = null;
            try
            {
                List<variant> localVariantList = cFrameworkWrapper.VariantList;

                foreach (variant lCurrentVariant in localVariantList)
                    if (pSpecificVariantIndex.Equals(-1))
                        if (lResultExpr == null)
                            lResultExpr = OperationInAllTransitions(pOperation, lCurrentVariant, pOperationState, pSpecificTransition);
                        else
                            lResultExpr = cZ3Solver.AndOperator(new List<BoolExpr> { lResultExpr, OperationInAllTransitions(pOperation
                                                                                                                            , lCurrentVariant
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                    else if (pSpecificVariantIndex != -1 && pSpecificVariantIndex.Equals(lCurrentVariant.index))
                        if (lResultExpr == null)

                            lResultExpr = OperationInAllTransitions(pOperation, lCurrentVariant, pOperationState, pSpecificTransition);
                        else
                            lResultExpr = cZ3Solver.AndOperator(new List<BoolExpr> { lResultExpr, OperationInAllTransitions(pOperation
                                                                                                                            , lCurrentVariant
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in OperationForAllVariantsInAllTransitions");
                Console.WriteLine(ex.Message);
            }
            return lResultExpr;
        }

        /// <summary>
        /// This function takes an operation and returns am expression of operation instnces for that operation and ANY of the variants in ALL transitions
        /// These any variants and in any transitions can be fixed to one specific variant and one specific transition if their respective parameters are set
        /// Also the operation state needs to be given
        /// </summary>
        /// <param name="pOperation"></param>
        /// <param name="pOperationState"></param>
        /// <param name="pSpecificVariantIndex">If the expression needs to be made for just one variant</param>
        /// <param name="pSpecificTransition">If the expression needs to be made for just one transition</param>
        /// <returns></returns>
        private BoolExpr OperationForAnyVariantsInAllTransitions(operation pOperation
                                                                , string pOperationState
                                                                , int pSpecificVariantIndex = -1
                                                                , int pSpecificTransition = -1)
        {
            BoolExpr lResultExpr = null;
            try
            {
                List<variant> localVariantList = cFrameworkWrapper.VariantList;

                foreach (variant lCurrentVariant in localVariantList)
                    if (pSpecificVariantIndex.Equals(-1))
                        if (lResultExpr == null)
                            lResultExpr = OperationInAllTransitions(pOperation, lCurrentVariant, pOperationState, pSpecificTransition);
                        else
                            lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr> { lResultExpr, OperationInAllTransitions(pOperation
                                                                                                                            , lCurrentVariant
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                    else if (pSpecificVariantIndex != -1 && pSpecificVariantIndex.Equals(lCurrentVariant.index))
                        if (lResultExpr == null)

                            lResultExpr = OperationInAllTransitions(pOperation, lCurrentVariant, pOperationState, pSpecificTransition);
                        else
                            lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr> { lResultExpr, OperationInAllTransitions(pOperation
                                                                                                                            , lCurrentVariant
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in OperationForAnyVariantsInAllTransitions");
                Console.WriteLine(ex.Message);
            }
            return lResultExpr;
        }

        /// <summary>
        /// This function takes an operation and returns am expression of operation instnces for that operation and ANY of the variants in ANY transitions
        /// These any variants and in any transitions can be fixed to one specific variant and one specific transition if their respective parameters are set
        /// Also the operation state needs to be given
        /// </summary>
        /// <param name="pOperation"></param>
        /// <param name="pOperationState"></param>
        /// <param name="pSpecificVariantIndex">If the expression needs to be made for just one variant</param>
        /// <param name="pSpecificTransition">If the expression needs to be made for just one transition</param>
        /// <returns></returns>
        private BoolExpr OperationForAnyVariantsInAnyTransitions(operation pOperation
                                                                , string pOperationState
                                                                , int pSpecificVariantIndex = -1
                                                                , int pSpecificTransition = -1)
        {
            BoolExpr lResultExpr = null;
            try
            {
                List<variant> localVariantList = cFrameworkWrapper.VariantList;

                foreach (variant lCurrentVariant in localVariantList)
                    if (pSpecificVariantIndex.Equals(-1))
                        if (lResultExpr == null)
                            lResultExpr = OperationInAnyTransitions(pOperation, lCurrentVariant, pOperationState, pSpecificTransition);
                        else
                            lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr> { lResultExpr, OperationInAnyTransitions(pOperation
                                                                                                                            , lCurrentVariant
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                    else if (pSpecificVariantIndex != -1 && pSpecificVariantIndex.Equals(lCurrentVariant.index))
                        if (lResultExpr == null)

                            lResultExpr = OperationInAnyTransitions(pOperation, lCurrentVariant, pOperationState, pSpecificTransition);
                        else
                            lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr> { lResultExpr, OperationInAnyTransitions(pOperation
                                                                                                                            , lCurrentVariant
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in OperationForAnyVariantsInAnyTransitions");
                Console.WriteLine(ex.Message);
            }
            return lResultExpr;
        }

        /// <summary>
        /// This function takes the different parts of an operation instance and makes the actual operation instance name using these parts
        /// </summary>
        /// <param name="pOperationName"></param>
        /// <param name="pOperationState"></param>
        /// <param name="pVairantIndex"></param>
        /// <param name="pTransitionNo"></param>
        /// <returns>Operation instance name</returns>
        private string ReturnOperationInstanceName(string pOperationName, string pOperationState, int pVairantIndex, int pTransitionNo)
        {
            string lOperationInstance = "";
            try
            {
                if (pOperationName != "" && pOperationState != "" && pVairantIndex > -1 && pTransitionNo > -1)
                    lOperationInstance = pOperationName + "_" + pOperationState + "_" + pVairantIndex + "_" + pTransitionNo;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationInstanceName");
                Console.WriteLine(ex.Message);
            }
            return lOperationInstance;
        }

        /// <summary>
        /// This function takes the different parts of an operation instance and makes the actual operation instance using these parts
        /// </summary>
        /// <param name="pOperationName"></param>
        /// <param name="pOperationState"></param>
        /// <param name="pVairantIndex"></param>
        /// <param name="pTransitionNo"></param>
        /// <returns>Operation instance</returns>
        private BoolExpr ReturnOperationInstanceVariable(string pOperationName, string pOperationState, int pVairantIndex, int pTransitionNo)
        {
            BoolExpr lOperationInstanceVariable = null;
            try
            {
                if (pOperationName != "" && pOperationState != "" && pVairantIndex > -1 && pTransitionNo > -1)
                {
                    string lOperationInstanceName = pOperationName + "_" + pOperationState + "_" + pVairantIndex + "_" + pTransitionNo;
                    lOperationInstanceVariable = cZ3Solver.FindBoolExpressionUsingName(lOperationInstanceName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationInstanceVariable");
                Console.WriteLine(ex.Message);
            }
            return lOperationInstanceVariable;
        }

        /// <summary>
        /// This function takes a list of variants and returns a string which includes the name of all the variants in the list
        /// </summary>
        /// <param name="pVariantList">The inputed list of variants</param>
        /// <returns>The string containing the name of all variants</returns>
        private string ReturnVariantNamesFromList(List<variant> pVariantList)
        {
            //TODO: This function and the following function has to be made into one which works on generic lists
            //and for that the tostring() of the items in the list has to return the names field of the items in the list

            //Initialize the string which is going to contain the names
            string lVariantNames = "";
            try
            {
                //Loop over all the variants in the list
                foreach (variant lVariant in pVariantList)
                {
                    //if the variant list is previously filled with something then add a comma before adding the next item
                    if (lVariantNames != "")
                        lVariantNames += " ,";

                    //Add the name of the variant to the list
                    lVariantNames += lVariant.names;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnVariantNamesFromList");
                Console.WriteLine(ex.Message);
            }
            //Return the string with the name of the variants
            return lVariantNames;
        }

        /// <summary>
        /// This function takes a list of operations and returns a string which includes the name of all the operations in the list
        /// </summary>
        /// <param name="pOperationList">The inputed list of operations</param>
        /// <returns>The string containing the name of all operations</returns>
        private string ReturnOperationNamesFromList(List<operation> pOperationList)
        {
            //TODO: This function and the previous function has to be made into one which works on generic lists
            //and for that the tostring() of the items in the list has to return the names field of the items in the list

            //Initialize the string which is going to contain all the operation names
            string lOperationNames = "";
            try
            {
                //Loop over all the operations in the list
                foreach (operation lOperation in pOperationList)
                {
                    //If the string with the operation names already has an item in it first add a comma to the end of it
                    if (lOperationNames != "")
                        lOperationNames += " ,";

                    //Add the name of the operation to the string
                    lOperationNames += lOperation.names;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationNamesFromList");
                Console.WriteLine(ex.Message);
            }
            //Return the string with the name of the operations
            return lOperationNames;
        }

        /// <summary>
        /// This function takes a list of operation instances and returns a string which includes the name of all the operations in the list
        /// </summary>
        /// <param name="pOperationInstanceList">The inputed list of operation instances</param>
        /// <returns>The string containing the name of all operations</returns>
        private string ReturnOperationNamesFromOpertionInstanceList(List<string> pOperationInstanceList)
        {
            //TODO: This function and the previous function has to be made into one which works on generic lists
            //and for that the tostring() of the items in the list has to return the names field of the items in the list

            //Initialize the string which is going to contain all the operation names
            string lOperationNames = "";
            try
            {
                //Loop over all the operations in the list
                foreach (string lOperationInstance in pOperationInstanceList)
                {
                    //If the string with the operation names already has an item in it first add a comma to the end of it
                    if (lOperationNames != "")
                        lOperationNames += " ,";

                    //Add the name of the operation to the string
                    string[] lOperationInstanceParts = lOperationInstance.Split('_');
                    lOperationNames += lOperationInstanceParts[0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationNamesFromOpertionInstanceList");
                Console.WriteLine(ex.Message);
            }
            //Return the string with the name of the operations
            return lOperationNames;
        }

        /// <summary>
        /// This function simply takes a list of operation names and returns the items in one string
        /// </summary>
        /// <param name="pOperationNameList">List of operation names</param>
        /// <returns>string of all operation names</returns>
        private string ReturnOperationNamesStringFromOperationNameList(List<string> pOperationNameList)
        {
            string lOperationNamesString = "";
            try
            {
                foreach (string lOperationName in pOperationNameList)
                {
                    if (lOperationNamesString != "")
                        lOperationNamesString += ", ";

                    lOperationNamesString += lOperationName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationNamesStringFromOperationNameList");
                Console.WriteLine(ex.Message);
            }
            return lOperationNamesString;
        }

        /// <summary>
        /// This is the first and most complete goal which looks at if all the configurations are manufaturable
        /// </summary>
        /// <param name="pState"></param>
        public void convertFGoals2Z3GoalsVersion2(int pState)
        {
            try
            {
                BoolExpr lOverallGoal = null;

                //This boolean expression is used to refer to this overall goal
                cZ3Solver.AddBooleanExpression("P" + pState);
                //What we add to the solver is: P_i => (formula 7) AND (formula 8)

                List<variant> localVariantList = cFrameworkWrapper.VariantList;

                //(Big And) ((! Pre_k_j AND O_I_k_j) OR (! Post_k_j AND O_E_k_j) OR O_F_k_j OR O_U_k_j)
                BoolExpr lFormula7 = createFormula7(localVariantList, pState);
                //(Big OR) (O_I_k_j OR O_E_k_j)
                BoolExpr lFormula8 = createFormula8(localVariantList, pState);


                //lOverallGoal = cZ3Solver.AndOperator(new List<BoolExpr>() { lFormula7, lFormula8 });
                if (lFormula7 != null && lFormula8 != null)
                {
                    lOverallGoal = cZ3Solver.AndOperator(new List<BoolExpr>() { cZ3Solver.FindBoolExpressionUsingName("F7_" + pState)
                                                                                , cZ3Solver.FindBoolExpressionUsingName("F8_" + pState) });

                    //lZ3Solver.AddConstraintToSolver(lOverallGoal, "overallGoal");
                    cZ3Solver.AddImpliesOperator2Constraints(cZ3Solver.FindBoolExpressionUsingName("P" + pState)
                                                                , lOverallGoal
                                                                , "overallGoal");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in convertFGoals2Z3GoalsVersion2");
                Console.WriteLine(ex.Message);
            }
        }

        public void convertFGoals2Z3GoalsVersion1(int pState)
        {
            try
            {
                BoolExpr lOverallGoal = null;


                //formula 7

                BoolExpr formula7 = null;
                List<variant> localVariantList = cFrameworkWrapper.VariantList;

                foreach (variant lCurrentVariant in localVariantList)
                {
                    List<operation> lOperationList = cFrameworkWrapper.getVariantExprOperations(lCurrentVariant.names);
                    if (lOperationList != null)
                    {
                        foreach (operation lOperation in lOperationList)
                        {
                            resetCurrentStateAndNewStateOperationVariables(lOperation, lCurrentVariant, pState, "formula7");

                            BoolExpr lOperand = cZ3Solver.OrOperator(new List<BoolExpr>() { cOp_F_CurrentState, cOp_U_CurrentState });
                            if (formula7 == null)
                                formula7 = lOperand;
                            else
                                formula7 = cZ3Solver.AndOperator(new List<BoolExpr>() { formula7, lOperand });
                        }
                    }
                }
                //            if (formula7 != null)
                //                lZ3Solver.AddConstraintToSolver(formula7);

                //formula 8
                BoolExpr formula8 = null;

                foreach (variant lCurrentVariant in localVariantList)
                {
                    List<operation> lOperationList = cFrameworkWrapper.getVariantExprOperations(lCurrentVariant.names);
                    if (lOperationList != null)
                    {
                        foreach (operation lOperation in lOperationList)
                        {
                            resetCurrentStateAndNewStateOperationVariables(lOperation, lCurrentVariant, pState, "formula8");

                            //BoolExpr lOpPrecondition = lZ3Solver.MakeBoolVariable("lOpPrecondition");
                            BoolExpr lOpPrecondition = ReturnOperationInstanceVariable(lOperation.names,"PreCondition",lCurrentVariant.index,pState);

                            if (lOperation.precondition != null)
                            {
                                if (cFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(lOperation.precondition[0]) > pState)
                                    //This means the precondition is on a transition state which has not been reached yet!
                                    lOpPrecondition = cZ3Solver.NotOperator(lOpPrecondition);
                                else
                                {
                                    if (lOperation.precondition[0].Contains('_'))
                                        //This means the precondition includes an operation status
                                        lOpPrecondition = cZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0]);
                                    else
                                        //This means the precondition only includes an operation
                                        lOpPrecondition = ReturnOperationInstanceVariable(lOperation.precondition[0],"F",lCurrentVariant.index,pState);
                                    //Before it was this
                                    //lOperationPrecondition = lZ3Solver.FindBoolExpressionUsingName(lPrecondition + "_I_" + currentVariant.index + "_" + pState.ToString());
                                }
                            }
                            else
                                lOpPrecondition = lOpPrecondition;

                            BoolExpr lFirstOperand = null;

                            lFirstOperand = cZ3Solver.AndOperator(new List<BoolExpr>() { cOp_I_CurrentState, lOpPrecondition });


                            BoolExpr lOperand = cZ3Solver.OrOperator(new List<BoolExpr>() { lFirstOperand, cOp_E_CurrentState });

                            if (formula8 == null)
                                formula8 = lOperand;
                            else
                                formula8 = cZ3Solver.OrOperator(new List<BoolExpr>() { formula8, lOperand });
                        }
                    }
                }
                //            if (formula8 != null)
                //                lZ3Solver.AddConstraintToSolver(formula8);


                lOverallGoal = cZ3Solver.NotOperator(cZ3Solver.ImpliesOperator(new List<BoolExpr>() { cZ3Solver.NotOperator(formula7), formula8 }));
                if (lOverallGoal != null)
                    cZ3Solver.AddConstraintToSolver(lOverallGoal, "overallGoal");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in convertFGoals2Z3GoalsVersion1");
                Console.WriteLine(ex.Message);
            }
        }

        private void addVirtualVariantOperationInstances(variant pVirtualVariant, List<operation> pVariantOperations)
        {
            try
            {

                ////TODO: Before this was inside the loop and it did not look OK!
                //5. Add the VirtualVariant and its operations or variantOperationsList
                cFrameworkWrapper.CreateVariantOperationMappingInstance(pVirtualVariant.names, pVariantOperations);

                foreach (operation lCurrentOperation in pVariantOperations)
                {
                    //6. For all the operations create their respective variables
                    addCurrentVariantOperationInstanceVariables(lCurrentOperation.names, pVirtualVariant.index, 0);
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
                    lResultVariant = cFrameworkWrapper.createVirtualVariant(pVariantExpr);
                }
                else
                {
                    //Then this should be a single variant
                    lResultVariant = cFrameworkWrapper.findVariantWithName(pVariantExpr);
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
                variant lCurrentVariant;
                //First we have to see if our pVariantExpr is a single variant or an Expression of variants
                if (pVariantExpr.Contains(' '))
                {
                    //Meaning it is an expression

                    //incase the variantExpr is an expression on a couple of variants we do the following
                    //1. Enter a new VirtualVariant to the variants list
                    //2. Add the entered VirtualVariant to the VirtualVariantGroup
                    //3. Add a constraint relating the newly created VirtualVariant to the VariantExpr
                    //4. Add the VirtualVariant and the VariantExpr to the local virtualVariant2VariantExprList
                    lCurrentVariant = ParseVariantExpr(pVariantExpr);

                    //5. Add the VirtualVariant and its operations or variantOperationsList
                    //6. For all the operations create their respective variables
                    addVirtualVariantOperationInstances(lCurrentVariant, pOperations);
                }
                else
                {
                    //Meaning it is a simple variant
                    lCurrentVariant = cFrameworkWrapper.findVariantWithName(pVariantExpr);

                    //5. Add the VirtualVariant and its operations or variantOperationsList
                    //6. For all the operations create their respective variables
                    addVirtualVariantOperationInstances(lCurrentVariant, pOperations);
                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ParsingVariantExpr2OperationsMapping");
                Console.WriteLine(ex.Message);
            }
        }

        public bool createVariantOperationInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                //First we extract the current set of variant operations from the input file
                List<variantOperations> lTemporaryVariantOperationsList = new List<variantOperations>();
                lTemporaryVariantOperationsList = cFrameworkWrapper.createVariantOperationTemporaryInstances(pXDoc);

                if (lTemporaryVariantOperationsList.Count.Equals(0))
                    lDataLoaded = false;
                else
                {
                    foreach (variantOperations lVariantOperation in lTemporaryVariantOperationsList)
                    {
                        string lCurrentVariantExpr = lVariantOperation.getVariantExpr();
                        List<operation> lCurrentOperation = lVariantOperation.getOperations();

                        //Now we parse each instace of variant operation so for those who have a variant expression instead of variant we can create Virtual variants
                        ParsingVariantExpr2OperationsMapping(lCurrentVariantExpr, lCurrentOperation);
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariantOperationInstances");
                Console.WriteLine(ex.Message);
            }
            return lDataLoaded;
        }

        public bool LoadInitialDataFromXMLFile(string pFilePath)
        {
            bool lDataLoaded = false;
            try
            {
                //new instance of xdoc
                XmlDocument xDoc = new XmlDocument();

                //First load the XML file from the file path
                xDoc.Load(pFilePath);

                bool lOperationsLoaded = false;
                lOperationsLoaded = cFrameworkWrapper.createOperationInstances(xDoc);

                bool lVariantsLoaded = false;
                lVariantsLoaded = cFrameworkWrapper.createVariantInstances(xDoc);

                bool lVariantGroupsLoaded = false;
                lVariantGroupsLoaded = cFrameworkWrapper.createVariantGroupInstances(xDoc);

                bool lConstraintsLoaded = false;
                lConstraintsLoaded = cFrameworkWrapper.createConstraintInstances(xDoc);

                bool lVariantOperationLoaded = false;
                lVariantOperationLoaded = createVariantOperationInstances(xDoc);

                //createStationInstances(xDoc);
                bool lTraitsLoaded = false;
                lTraitsLoaded = cFrameworkWrapper.createTraitInstances(xDoc);

                bool lResourceLoaded = false;
                lResourceLoaded = cFrameworkWrapper.createResourceInstances(xDoc);

                lDataLoaded = ((lVariantsLoaded && lVariantGroupsLoaded) && lOperationsLoaded);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in LoadInitialDataFromXMLFile, FilePath: " + pFilePath);
                Console.WriteLine(ex.Message);
            }
            return lDataLoaded;
        }

        /// <summary>
        /// This function returns the number of analysis cycles which are needed 
        /// This will be the number of operations times 2 
        /// as each operation will have two transitions between its states
        /// </summary>
        /// <returns>The number of cycles needed for the analysis</returns>
        private int CalculateAnalysisNoOfCycles()
        {
            int lResultNoOfCycles = 0;
            try
            {
                //NoOfCycles = NumOfOperations * NumOfTransitions
                ////TODO: It should be the second one because that would be more accurate but at the time of this calculation the number of active operations is not known! Maybe can be fixed!
                int lNoOfOperations = cFrameworkWrapper.getNumberOfOperations();
                //int lNoOfOperations = lFrameworkWrapper.getNumberOfActiveOperations();


                //Each operation will have two transitions
                lResultNoOfCycles = lNoOfOperations * 2;

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CalculateAnalysisNoOfCycles");
                Console.WriteLine(ex.Message);
            }
            return lResultNoOfCycles;
        }

        /// <summary>
        /// This function loads the initial data (product platform data) from the external file
        /// Then according to the type of analysis which is stated in the argument it will call the respective analyzer function
        /// </summary>
        /// <param name="pAnalysisType">Type of analysis which we want</param>
        /// <param name="pExtraConfigurationRule">Any configuration rule whch needs to be added to the product platform configuration rule set</param>
        /// <param name="pInternalFileData">In case the initial data file is internal it has to be provided here</param>
        /// <returns>The result of the analysis</returns>
        public bool ProductPlatformAnalysis(string pInternalFileData = "", string pExtraConfigurationRule = "")
        {
            bool lLoadInitialData = false;
            bool lTestResult = false;
            bool lAnalysisDone = false;

            String file = null;
            if (pInternalFileData != "")
                file = pInternalFileData;

            try
            {
                cZ3Solver.setDebugMode(true);

                lLoadInitialData = loadInitialData(Enumerations.InitializerSource.InternalFile, file);

                if (lLoadInitialData)
                {
                    switch (cAnalysisType)
                    {
                        case Enumerations.AnalysisType.ModelEnumerationAnalysis:
                            {
                                lTestResult = ModelEnumerationAnalysis(true, true);
                                break;
                            }
                        case Enumerations.AnalysisType.VariantSelectabilityAnalysis:
                            {
                                lTestResult = VariantSelectabilityAnalysis(true, true);
                                break;
                            }
                        case Enumerations.AnalysisType.AlwaysSelectedVariantAnalysis:
                            {
                                lTestResult = AlwaysSelectedVariantAnalysis(true, true);
                                break;
                            }
                        case Enumerations.AnalysisType.OperationSelectabilityAnalysis:
                            {
                                lTestResult = OperationSelectabilityAnalysis(true, true);
                                break;
                            }
                        case Enumerations.AnalysisType.AlwaysSelectedOperationAnalysis:
                            {
                                lTestResult = AlwaysSelectedOperationAnalysis(true, true);
                                break;
                            }
                        case Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis:
                            {
                                lTestResult = ExistanceOfDeadlockAnalysis(true, true);
                                break;
                            }
                        case Enumerations.AnalysisType.CompleteAnalysis:
                            {
                                ////TODO: this analysis was removed as there needs to be a loop over the different transition numbers before it!!!
                                ////lTestResult = AnalyzeProductPlatform(out lAnalysisDone, AnalysisType.CompleteAnalysis);
                                break;
                            }
                        default:
                            {
                                ////TODO: this analysis was removed as there needs to be a loop over the different transition numbers before it!!!
                                ////lTestResult = AnalyzeProductPlatform(out lAnalysisDone, AnalysisType.CompleteAnalysis);
                                break;
                            }
                    }

                    if (cStopAEndOfAnalysis)
                        Console.ReadKey();

                }
                else
                {
                    Console.WriteLine("Initial data incomplete! Analysis can't be done!");
                    if (cStopAEndOfAnalysis)
                        Console.ReadKey();
                }

            }
            catch (Z3Exception ex)
            {
                Console.WriteLine("Z3 Managed Exception: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);
            }
            return lTestResult;
        }

        /// <summary>
        /// This is where the program starts
        /// </summary>
        /// <param name="args">This list can be used to input any wanted parameters to the program</param>
        static void Main(string[] args)
        {
            try
            {
                Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

                //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                //             , Build P Constraints, Number Of Models Required
                lZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                                    , Enumerations.AnalysisType.VariantSelectabilityAnalysis
                                                    , 4);

                //Parameters: Analysis Result, Analysis Detail Result, Variants Result, Transitions Result, Analysis Timing, Unsat Core, Stop between each transition, Stop at end of analysis
                lZ3SolverEngineer.setReportType(true, true, true, false, false, true, false, true);

                //lZ3SolverEngineer.ProductPlatformAnalysis("0.0V0VG0O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("0.0V0VG1O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("0.1V0VG1O0C0P.xml");

                lZ3SolverEngineer.ProductPlatformAnalysis("1.0.1V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("1.1.2V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("1.2.3V1VG2O0C0P.xml");

                //lZ3SolverEngineer.ProductPlatformAnalysis("2.0.3V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("2.1.3V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("2.2.3V1VG2O0C0P.xml");

                //lZ3SolverEngineer.ProductPlatformAnalysis("4.0.1V1VG2O0C1P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("4.1.3V2VG2O0C0P.xml");

                //lZ3SolverEngineer.ProductPlatformAnalysis("5.0.4V2VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("5.1.4V2VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("5.2.4V2VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("5.0.4V2VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("5.0.4V2VG2O0C0P.xml");
                
                //TODO: for a variant which is selectable what would be a good term for completing the analysis?
                //lZ3SolverEngineer.ProductPlatformAnalysis("2.2V1VG2O1C1PNoTransitions-1UnselectV.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("3.2V1VG2O1C1PNoTransitions.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("4.2V1VG3O1C1PNoTransition.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("5.2V1VG2O1C1PNoTransition.xml");

                //lZ3SolverEngineer.ProductPlatformAnalysis("6.2V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("7.2V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("8.2V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("9.0.2V1VG2O0C1P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("10.2V1VG2O0C1P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("11.2V1VG2O0C1P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("12.2V1VG2O0C2P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("13.2V1VG2O0C2P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis("14.2V1VG2O0C2P.xml");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }


    }
}
