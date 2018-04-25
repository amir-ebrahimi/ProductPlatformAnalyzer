using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Z3;
using System.Collections;
using System.IO;
using System.Xml;
using CAEX_ClassModel;
using AMLEngineExtensions;
using CAEX_ClassModel.Validation;
using System.Diagnostics;

namespace ProductPlatformAnalyzer
{
    public class Z3SolverEngineer
    {
        private FrameworkWrapper cFrameworkWrapper;
        private Z3Solver cZ3Solver;
        private OutputHandler cOutputHandler;
        private RandomTestCreator cRandomTestCreator;

        private bool cDebugMode;
        private bool cOpSeqAnalysis;
        private bool cNeedPreAnalysis;
        private bool cPreAnalysisResult;

        //private OperationInstance cOperationInstance_CurrentState;
        //private OperationInstance cOperationInstance_NextState;

        private List<BoolExpr> cConfigurationConstraints;
        private HashSet<BoolExpr> cPConstraints;

        private int cCurrentTransitionNumber;
        private int cMaxNumberOfTransitions;

        //boolean variation point variables for model building
        private Enumerations.GeneralAnalysisType cGeneralAnalysisType;
        private Enumerations.AnalysisType cAnalysisType;

        private bool cConvertVariants;
        private bool cConvertConfigurationRules;
        private bool cConvertOperations;
        private bool cConvertOperationPrecedenceRules;
        //private bool cConvertVariantOperationRelations;
        private bool cConvertResources;
        private bool cConvertGoal;
        private bool cBuildPConstraints;
        private bool cOperationMutualExecution;

        //boolean variation point variables for reporting in output
        private bool cReportAnalysisResult;
        private bool cReportAnalysisDetailResult;
        private bool cReportVariantsResult;
        private bool cReportTransitionsResult;
        private bool cReportAnalysisTiming;
        private bool cReportUnsatCore;
        private bool cStopBetweenEachTransition;
        private bool cStopAEndOfAnalysis;
        private bool cCreateHTMLOutput;
        private bool cReportTimings;
        private int cNoOfModelsRequired;

        private double cModelCreationTime;
        private double cModelAnalysisTime;
        private double cModelAnalysisReportingTime;

        #region Getter-Setters
        public bool ConvertVariants
        {
            get { return this.cConvertVariants; }
            set { this.cConvertVariants = value; }
        }

        public bool ConvertConfigurationRules
        {
            get { return this.cConvertConfigurationRules; }
            set { this.cConvertConfigurationRules = value; }
        }

        public bool ConvertOperations
        {
            get { return this.cConvertOperations; }
            set { this.cConvertOperations = value; }
        }

        public bool ConvertOperationPrecedenceRules
        {
            get { return this.cConvertOperationPrecedenceRules; }
            set { this.cConvertOperationPrecedenceRules = value; }
        }

        public bool ConvertResources
        {
            get { return this.cConvertResources; }
            set { this.cConvertResources = value; }
        }

        public bool ConvertGoal
        {
            get { return this.cConvertGoal; }
            set { this.cConvertGoal = value; }
        }

        public bool BuildPConstraints
        {
            get { return this.cBuildPConstraints; }
            set { this.cBuildPConstraints = value; }
        }

        public bool OperationMutualExecution
        {
            get { return this.cOperationMutualExecution; }
            set { this.cOperationMutualExecution = value; }
        }


        public bool ReportAnalysisResult
        {
            get { return this.cReportAnalysisResult; }
            set { this.cReportAnalysisResult = value; }
        }

        public bool ReportAnalysisDetailResult
        {
            get { return this.cReportAnalysisDetailResult; }
            set { this.cReportAnalysisDetailResult = value; }
        }

        public bool ReportVariantsResult
        {
            get { return this.cReportVariantsResult; }
            set { this.cReportVariantsResult = value; }
        }

        public bool ReportTransitionsResult
        {
            get { return this.cReportTransitionsResult; }
            set { this.cReportTransitionsResult = value; }
        }

        public bool ReportAnalysisTiming
        {
            get { return this.cReportAnalysisTiming; }
            set { this.cReportAnalysisTiming = value; }
        }

        public bool ReportUnsatCore
        {
            get { return this.cReportUnsatCore; }
            set { this.cReportUnsatCore = value; }
        }

        public bool ReportStopBetweenEachTransition
        {
            get { return this.cStopBetweenEachTransition; }
            set { this.cStopBetweenEachTransition = value; }
        }

        public bool StopAEndOfAnalysis
        {
            get { return this.cStopAEndOfAnalysis; }
            set { this.cStopAEndOfAnalysis = value; }
        }

        public bool CreateHTMLOutput
        {
            get { return this.cCreateHTMLOutput; }
            set { this.cCreateHTMLOutput = value; }
        }

        public bool ReportTimings
        {
            get { return this.cReportTimings; }
            set { this.cReportTimings = value; }
        }

        public int NoOfModelsRequired
        {
            get { return this.cNoOfModelsRequired; }
            set { this.cNoOfModelsRequired = value; }
        }

        #endregion

        /// <summary>
        /// This is the creator for the class which initializes the class variables
        /// </summary>
        public Z3SolverEngineer()
        {
            cCurrentTransitionNumber = 0;
            cMaxNumberOfTransitions = 0;

            DefaultAnalyzerSetting(this);

            cDebugMode = false;
            cOpSeqAnalysis = true;
            cNeedPreAnalysis = true;
            cPreAnalysisResult = true;
            cConfigurationConstraints = new List<BoolExpr>();
            cPConstraints = new HashSet<BoolExpr>();

            //boolean variation point variables
            setVariationPoints(Enumerations.GeneralAnalysisType.Static, Enumerations.AnalysisType.CompleteAnalysis);

            //Parameters: Analysis Result, Analysis Detail Result, Variants Result
            //          , Transitions Result, Analysis Timing, Unsat Core
            //          , Stop between each transition, Stop at end of analysis, Create HTML Output
            //          , Report timings, Debug Mode (Make model file), User Messages
            setReportType(true, true, true
                        , true, true, true
                        , true, true, false
                        , true, true, true);
        }

        public void DefaultAnalyzerSetting(Z3SolverEngineer pZ3SolverEngineer)
        {
            try
            {
                cOutputHandler = new OutputHandler();
                cFrameworkWrapper = new FrameworkWrapper(cOutputHandler);
                cRandomTestCreator = new RandomTestCreator(cOutputHandler);
                cZ3Solver = new Z3Solver(cOutputHandler);

                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                //          , Transitions Result, Analysis Timing, Unsat Core
                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                //          , Report timings, Debug Mode (Make model file), User Messages
                pZ3SolverEngineer.setReportType(true, true, true
                                                , false, false, true
                                                , false, true, true
                                                , true, true, true);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in DefaultAnalyzerSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
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
                                    , bool pStopAtEndOfAnalysis
                                    , bool pCreateHTMLOutput
                                    , bool pReportTimings
                                    , bool pDebugMode
                                    , bool pUserMessages)
        {
            try
            {
                cReportAnalysisResult = pAnalysisResult;
                cReportAnalysisDetailResult = pAnalysisDetailResult;
                cReportVariantsResult = pVariantsResult;
                cReportTransitionsResult = pTransitionsResult;
                cReportAnalysisTiming = pAnalysisTiming;
                cReportUnsatCore = pUnsatCore;
                cStopBetweenEachTransition = pStopBetweenEachTransition;
                cStopAEndOfAnalysis = pStopAtEndOfAnalysis;
                cCreateHTMLOutput = pCreateHTMLOutput;
                cReportTimings = pReportTimings;
                cOutputHandler.setEnableUserMessages(pUserMessages);
                cZ3Solver.setDebugMode(pDebugMode);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in setReportType");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void DefaultProductModelEnumerationAnalysisVariationPointSetting(int pOverrideNoOfModelsRequired = 1
                                        , bool pOverrideConvertVariants = false
                                        , bool pOverrideConvertConfigurationRules = false
                                        , bool pOverrideConvertOperations = false
                                        , bool pOverrideConvertOperationPrecedenceRules = false
                                        , bool pOverrideConvertVariantOperationRelations = false
                                        , bool pOverrideConvertResources = false
                                        , bool pOverrideConvertGoal = false
                                        , bool pOverrideBuildPConstaints = false
                                        , bool pOverrideOperationMutualExecution = false)
        {
            try
            {
                if (pOverrideNoOfModelsRequired == null)
                    cNoOfModelsRequired = 1;
                else
                    cNoOfModelsRequired = pOverrideNoOfModelsRequired;
                cConvertVariants = (!pOverrideConvertVariants) ? true : false;
                cConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                cConvertOperations = (!pOverrideConvertOperations) ? true : false;
                cConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                cConvertResources = (!pOverrideConvertResources) ? false : true;
                cConvertGoal = (!pOverrideConvertGoal) ? true : false;
                cBuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                cOperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in DefaultProductModelEnumerationAnalysisVariationPointSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                                        , bool pOverrideBuildPConstaints = false
                                        , bool pOverrideOperationMutualExecution = false)
        {
            try
            {
                cConvertVariants = (!pOverrideConvertVariants) ? true : false;
                cConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                cConvertOperations = (!pOverrideConvertOperations) ? true : false;
                cConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                cConvertResources = (!pOverrideConvertResources) ? false : true;
                cConvertGoal = (!pOverrideConvertGoal) ? false : true;
                cBuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                cOperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in DefaultVariantSelectabilityAnalysisVariationPointSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                                        , bool pOverrideBuildPConstaints = false
                                        , bool pOverrideOperationMutualExecution = false)
        {
            try
            {
                cConvertVariants = (!pOverrideConvertVariants) ? true : false;
                cConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                cConvertOperations = (!pOverrideConvertOperations) ? true : false;
                cConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                cConvertResources = (!pOverrideConvertResources) ? false : true;
                cConvertGoal = (!pOverrideConvertGoal) ? false : true;
                cBuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                cOperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in DefaultAlwaysSelectedVariantAnalysisVariationPointSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                                        , bool pOverrideBuildPConstaints = false
                                        , bool pOverrideOperationMutualExecution = false)
        {
            try
            {
                cConvertVariants = (!pOverrideConvertVariants) ? true : false;
                cConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                cConvertOperations = (!pOverrideConvertOperations) ? true : false;
                cConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                cConvertResources = (!pOverrideConvertResources) ? false : true;
                cConvertGoal = (!pOverrideConvertGoal) ? false : true;
                cBuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                cOperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in DefaultOperationSelectabilityAnalysisVariationPointSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                                        , bool pOverrideBuildPConstaints = false
                                        , bool pOverrideOperationMutualExecution = false)
        {
            try
            {
                cConvertVariants = (!pOverrideConvertVariants) ? true : false;
                cConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                cConvertOperations = (!pOverrideConvertOperations) ? true : false;
                cConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                cConvertResources = (!pOverrideConvertResources) ? false : true;
                cConvertGoal = (!pOverrideConvertGoal) ? false : true;
                cBuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                cOperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in DefaultAlwaysSelectedOperationAnalysisVariationPointSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                                        , bool pOverrideBuildPConstaints = false
                                        , bool pOverrideOperationMutualExecution = false)
        {
            try
            {
                cConvertVariants = (!pOverrideConvertVariants) ? true : false;
                cConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                cConvertOperations = (!pOverrideConvertOperations) ? true : false;
                cConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? true : false;
                cConvertResources = (!pOverrideConvertResources) ? false : true;
                cConvertGoal = (!pOverrideConvertGoal) ? true : false;
                cBuildPConstraints = (!pOverrideBuildPConstaints) ? true : false;
                cOperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in DefaultExistanceOfDeadlockAnalysisVariationPointSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void DefaultProductManufacturingEnumerationAnalysisVariationPointSetting(int pOverrideNoOfModelsRequired = 1
                                        , bool pOverrideConvertVariants = false
                                        , bool pOverrideConvertConfigurationRules = false
                                        , bool pOverrideConvertOperations = false
                                        , bool pOverrideConvertOperationPrecedenceRules = false
                                        , bool pOverrideConvertVariantOperationRelations = false
                                        , bool pOverrideConvertResources = false
                                        , bool pOverrideConvertGoal = false
                                        , bool pOverrideBuildPConstaints = false
                                        , bool pOverrideOperationMutualExecution = false)
        {
            try
            {
                cConvertVariants = (!pOverrideConvertVariants) ? true : false;
                cConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                cConvertOperations = (!pOverrideConvertOperations) ? true : false;
                cConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? true : false;
                cConvertResources = (!pOverrideConvertResources) ? false : true;
                cConvertGoal = (!pOverrideConvertGoal) ? true : false;
                cBuildPConstraints = (!pOverrideBuildPConstaints) ? true : false;
                cOperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in DefaultProductManufacturingEnumerationAnalysisVariationPointSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                                        , bool pOverrideBuildPConstaints = false
                                        , bool pOverrideOperationMutualExecution = false)
        {
            try
            {
                cGeneralAnalysisType = pGeneralAnalysisType;
                cAnalysisType = pAnalysisType;
                switch (cAnalysisType)
                {
                    case Enumerations.AnalysisType.ProductModelEnumerationAnalysis:
                        {
                            DefaultProductModelEnumerationAnalysisVariationPointSetting(pOverrideNoOfModelsRequired
                                                                                , pOverrideConvertVariants
                                                                                , pOverrideConvertConfigurationRules
                                                                                , pOverrideConvertOperations
                                                                                , pOverrideConvertOperationPrecedenceRules
                                                                                , pOverrideConvertVariantOperationRelations
                                                                                , pOverrideConvertResources
                                                                                , pOverrideConvertGoal
                                                                                , pOverrideBuildPConstaints
                                                                                , pOverrideOperationMutualExecution);
                            break;
                        }
                    case Enumerations.AnalysisType.ProductManufacturingModelEnumerationAnalysis:
                        {
                            DefaultProductManufacturingEnumerationAnalysisVariationPointSetting(pOverrideNoOfModelsRequired
                                                                                , pOverrideConvertVariants
                                                                                , pOverrideConvertConfigurationRules
                                                                                , pOverrideConvertOperations
                                                                                , pOverrideConvertOperationPrecedenceRules
                                                                                , pOverrideConvertVariantOperationRelations
                                                                                , pOverrideConvertResources
                                                                                , pOverrideConvertGoal
                                                                                , pOverrideBuildPConstaints
                                                                                , pOverrideOperationMutualExecution);
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
                                                                                , pOverrideBuildPConstaints
                                                                                , pOverrideOperationMutualExecution);
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
                                                                                , pOverrideBuildPConstaints
                                                                                , pOverrideOperationMutualExecution);
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
                                                                                , pOverrideBuildPConstaints
                                                                                , pOverrideOperationMutualExecution);
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
                                                                                , pOverrideBuildPConstaints
                                                                                , pOverrideOperationMutualExecution);
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
                                                                                , pOverrideBuildPConstaints
                                                                                , pOverrideOperationMutualExecution);
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
                cOutputHandler.printMessageToConsole("error in setVariationPoints");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                //List<variantGroup> localVariantGroupList = cFrameworkWrapper.VariantGroupList;

                foreach (variantGroup localVariantGroup in cFrameworkWrapper.VariantGroupSet)
                    cZ3Solver.AddBooleanExpression(localVariantGroup.names);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in makeExpressionListFromVariantGroupList");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// NOT USED ANYWHERE IN THE PROJECT
        /// For each part in the local list a boolean variable is created
        /// </summary>
        public void makeExpressionListFromPartList()
        {
            try
            {
                foreach (part localPart in cFrameworkWrapper.PartSet)
                    cZ3Solver.AddBooleanExpression(localPart.names);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in makeExpressionListFromPartList");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                foreach (variant localVariant in cFrameworkWrapper.VariantSet)
                    cZ3Solver.AddBooleanExpression(localVariant.names);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in makeExpressionListFromVariantList");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// Load AML File data
        /// </summary>
        /// <param name="pFile"></param>
        /// <returns>If the data is loaded correctly or not</returns>
        public bool loadAMLInitialData(string pFile)
        {
            bool lDataLoaded = true;
            try
            {
                var document = CAEXDocument.LoadFromFile(pFile);
                
                var converter = new AMLConverter(document, cZ3Solver);

                //This function populate should return a boolean result which indicates if the population is done right or not
                converter.Populate();
                converter.PopulateFrameworkWrapper(cFrameworkWrapper);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in loadAMLInitialData");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        /// <summary>
        /// This function loads the initial data from the external fle to internal lists
        /// </summary>
        /// <param name="pInitialData"></param>
        /// <param name="pFile"></param>
        /// <returns>If the data is loaded correctly or not</returns>
        public bool loadInitialData(Enumerations.InitializerSource pInitialData, String pInitialDataFileName = ""
                                    , int pMaxVariantGroupumber = 0, int pMaxVariantNumber = 0, int pMaxPartNumber = 0
                                    , int pMaxOperationNumber = 0, int pTrueProbability = 0, int pFalseProbability = 0
                                    , int pExpressionProbability = 0, int pMaxTraitNumber = 0, int pMaxNoOfTraitAttributes = 0
                                    , int pMaxResourceNumber = 0, int pMaxExpressionOperandNumber = 0)
        {
            bool lDataLoaded = false;
            try
            {
                Enumerations.InputFileType lInitialDataFileExtension = returnInputFileType(pInitialDataFileName);
                if (lInitialDataFileExtension.Equals(Enumerations.InputFileType.AML))
                    lDataLoaded = loadAMLInitialData(pInitialDataFileName);
                else if (lInitialDataFileExtension.Equals(Enumerations.InputFileType.XML))
                {
                    switch (pInitialData)
                    {
                        case Enumerations.InitializerSource.InitialDataFile:
                            {
                                //                            LoadInitialDataFromXMLFile(exePath + "../../../" + endPath);
                                //                            lFrameworkWrapper.LoadInitialDataFromXMLFile(endPath);
                                //                            lDataLoaded = LoadInitialDataFromXMLFile("../../Test/" + endPath);
                                lDataLoaded = LoadInitialDataFromXMLFile(pInitialDataFileName);
                                break;
                            }
                        case Enumerations.InitializerSource.RandomData:
                            {
                                //Creating random data
                                //Parameters: pMaxVariantGroupNumber, pMaxVariantNumber, pMaxPartNumber, pMaxOperationNumber
                                //           ,pTrueProbability, pFalseProbability, pExpressionProbability
                                //           ,pMaxTraitNumber, pMaxNoOfTraitAttributes, pMaxResourceNumber
                                lDataLoaded = cRandomTestCreator.createRandomData(pMaxVariantGroupumber, pMaxVariantNumber, pMaxPartNumber
                                                                                , pMaxOperationNumber
                                                                                , pTrueProbability, pFalseProbability, pExpressionProbability
                                                                                , pMaxTraitNumber, pMaxNoOfTraitAttributes, pMaxResourceNumber
                                                                                , pMaxExpressionOperandNumber
                                                                                , cFrameworkWrapper);

                                if (pMaxPartNumber > 0)
                                    cFrameworkWrapper.UsePartInfo = true;
                                else
                                    cFrameworkWrapper.UsePartInfo = false;

                                break;
                            }
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in loadInitialData");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        /// <summary>
        /// This function looks at the input file name and from the extension returns the type of the file
        /// </summary>
        /// <param name="pFileName">Input file name</param>
        /// <returns>Extension of the file</returns>
        public Enumerations.InputFileType returnInputFileType(string pFileName)
        {
            Enumerations.InputFileType lDataFileType = Enumerations.InputFileType.XML;
            try
            {
                if (pFileName.Contains(".xml"))
                    lDataFileType = Enumerations.InputFileType.XML;
                else if (pFileName.Contains(".aml"))
                    lDataFileType = Enumerations.InputFileType.AML;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in returnInputFileType");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataFileType;
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
                cOutputHandler.printMessageToConsole("error in MakeProdutPlatformModel");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in MakeStaticPartOfProductPlatformModel");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// This function creates the dynamic part of the product platform model which includes the opertions, the precedence rules among the operations
        /// </summary>
        /// <param name="pAnalysisComplete">If the analysis is concluded, finished</param>
        /// <param name="pTransitionNo"></param>
        public void MakeDynamicPartOfProductPlatformModel(bool pAnalysisComplete)
        {
            try
            {
                //TODO: write what formulas each of these lines include

                //If it is asked for the model to include the operations then they are added to the model
                //Variation Point
                if (cConvertOperations)
                    convertFOperations2Z3Operations();

                //If it is asked for the operation precedence rues to be added to the model then they are added
                //Variation Point
                if (cConvertOperationPrecedenceRules || cBuildPConstraints)
                    convertOperationsPrecedenceRules();

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in MakeDynamicPartOfProductPlatformModel");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                                cOutputHandler.printMessageToConsole("Analysis No: " + pTransitionNo);
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

                            if (cAnalysisType.Equals(Enumerations.AnalysisType.ProductModelEnumerationAnalysis))
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

                            cOutputHandler.printMessageToConsole("Finished: ");
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
                    if (cStopBetweenEachTransition)
                        Console.ReadKey();

                    ////TODO: Removed as new version
                    //If all the transition cycles are completed then the analysis is completed
                    ////if (pTransitionNo == lNoOfCycles - 1)
                    ////    pAnalysisComplete = true;

                ////}
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AnalyzeProductPlatform");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.setFrameworkWrapper(pFrameworkWrapper);

                cOutputHandler = cZ3Solver.PopulateOutputHandler(pState, cOutputHandler);

                switch (cAnalysisType)
                {
                    case Enumerations.AnalysisType.ProductModelEnumerationAnalysis:
                    case Enumerations.AnalysisType.ProductManufacturingModelEnumerationAnalysis:
                        {
                            if (pSatResult.Equals(Status.SATISFIABLE))
                            {
                                //What does the satisfiable result in this analysis mean?
                                if (cReportAnalysisResult)
                                    cOutputHandler.printMessageToConsole("The ProductPlatform has a model to satisfy it.");

                                //Means that there has been a model which has been found
                                cOutputHandler.printMessageToConsole("Model No " + pState + ":");
                                if (cReportVariantsResult)
                                    cOutputHandler.printChosenVariants();
                                if (cReportTransitionsResult)
                                    cOutputHandler.printOperationsTransitions();

                                if (cCreateHTMLOutput)
                                {
                                    cOutputHandler.writeModel();
                                    cOutputHandler.writeModelNoPost();
                                }

                            }
                            else if (pSatResult.Equals(Status.UNSATISFIABLE))
                            {
                                //What does the unsatisfiable result in this analysis mean?
                                if (cReportAnalysisResult)
                                    cOutputHandler.printMessageToConsole("The ProductPlatform has no more valid models.");

                                if (cReportAnalysisDetailResult)
                                    cOutputHandler.printCounterExample();

                                if (cCreateHTMLOutput)
                                {
                                    cOutputHandler.writeCounterExample();
                                    cOutputHandler.writeCounterExampleNoPost();
                                }

                                //cOutputHandler.printMessageToConsole("proof: {0}", iSolver.Proof);
                                //cOutputHandler.printMessageToConsole("core: ");
                                if (cReportUnsatCore)
                                    cZ3Solver.ConsoleWriteUnsatCore();
                            }
                            break;
                        }
                    case Enumerations.AnalysisType.VariantSelectabilityAnalysis:
                        {
                            if (pSatResult.Equals(Status.SATISFIABLE))
                            {
                                if (pExtraField != null)
                                {
                                    part lAnalyzedVariant = (part)pExtraField;
                                    //What does the satisfiable result in this analysis mean?
                                    if (cReportAnalysisResult)
                                    {
                                        cOutputHandler.printMessageToConsole("---------------------------------------------------------------------");
                                        cOutputHandler.printMessageToConsole("Selected Variant: " + lAnalyzedVariant.names);
                                        cOutputHandler.printMessageToConsole(lAnalyzedVariant.names + " is Selectable.");

                                    }
                                }
                            }
                            else if (pSatResult.Equals(Status.UNSATISFIABLE))
                            {

                            }
                            break;
                        }
                    case Enumerations.AnalysisType.AlwaysSelectedVariantAnalysis:
                        {
                            if (pSatResult.Equals(Status.SATISFIABLE))
                            {
                                if (pExtraField != null)
                                {
                                    part lAnalyzedVariant = (part)pExtraField;
                                    if (cReportAnalysisResult)
                                    {
                                        //Initial info
                                        cOutputHandler.printMessageToConsole("---------------------------------------------------------------------");
                                        cOutputHandler.printMessageToConsole("Selected Variant: " + lAnalyzedVariant.names);

                                        //Analysis Result
                                        //if it does hold, then there is a configuration which is valid and this current variant is not present in it
                                        cOutputHandler.printMessageToConsole("There DOES exist a valid configuration which does not include " + lAnalyzedVariant.names + ".");
                                    }
                                }
                            }
                            else if (pSatResult.Equals(Status.UNSATISFIABLE))
                            {
                                if (pExtraField != null)
                                {
                                    part lAnalyzedVariant = (part)pExtraField;
                                    cOutputHandler.printMessageToConsole("All valid configurations DO include " + lAnalyzedVariant.names + ".");
                                }
                            }
                            break;
                        }
                    case Enumerations.AnalysisType.OperationSelectabilityAnalysis:
                        {
                            if (pSatResult.Equals(Status.SATISFIABLE))
                            {
                                if (pExtraField != null)
                                {
                                    string lAnalyzedOperationName = (string)pExtraField;
                                    //What does the satisfiable result in this analysis mean?
                                    if (cReportAnalysisResult)
                                    {
                                        //Initial info
                                        cOutputHandler.printMessageToConsole("----------------------------------------------------------------");
                                        cOutputHandler.printMessageToConsole("Analysing operation named: " + lAnalyzedOperationName);

                                        //Analysis Result
                                        cOutputHandler.printMessageToConsole(lAnalyzedOperationName + " is selectable.");
                                    }
                                }
                            }
                            else if (pSatResult.Equals(Status.UNSATISFIABLE))
                            {
                                //Here we check if this operation is in active operation or not, meaning does a variant use this operation for its assembly or not
                                OperationInstance lTempOperationInstace = (OperationInstance)pExtraField;
                                if (lTempOperationInstace.Status.Equals(Enumerations.OperationStatus.Active))
                                {

                                    //This means the operation instance is inactive hence it should be mentioned

                                    //This is when we want to report only the operation name
                                    cOutputHandler.printMessageToConsole("Operation " + lTempOperationInstace.AbstractOperation.Name + " is inactive!");
                                }
                            }
                            break;
                        }
                    case Enumerations.AnalysisType.AlwaysSelectedOperationAnalysis:
                        {
                            if (pSatResult.Equals(Status.SATISFIABLE))
                            {
                                if (pExtraField != null)
                                {
                                    OperationInstance lTempOperationInstace = (OperationInstance)pExtraField;
                                    string lOperationName = lTempOperationInstace.AbstractOperation.Name;

                                    //if it does hold, then there exists a valid configuration in which the current operation is UNUSED!
                                    cOutputHandler.printMessageToConsole("There DOES exist a configuration in which " + lOperationName + " is in an UNUSED state!");
                                }
                            }
                            else if (pSatResult.Equals(Status.UNSATISFIABLE))
                            {
                                if (pExtraField != null)
                                {
                                    OperationInstance lTempOperationInstace = (OperationInstance)pExtraField;
                                    string lOperationName = lTempOperationInstace.AbstractOperation.Name;

                                    cOutputHandler.printMessageToConsole("All valid configurations DO include " + lOperationName + ".");
                                }
                            }
                            break;
                        }
                    case Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis:
                        {

                            //if (pDone)
                            //{
                                //If the analysis is complete, all transitions have been done
                                if (pSatResult.Equals(Status.SATISFIABLE))
                                {
                                    //If the result of the analysis is SAT meaning that there has been a deadlock found

                                    //Print and writes an output file showing the result of a deadlocked test
                                    if (cReportAnalysisDetailResult)
                                        cOutputHandler.printCounterExample();

                                    if (cCreateHTMLOutput)
                                    {
                                        cOutputHandler.writeCounterExample();
                                        cOutputHandler.writeCounterExampleNoPost();
                                    }

                                    //cOutputHandler.printMessageToConsole("proof: {0}", iSolver.Proof);
                                    //cOutputHandler.printMessageToConsole("core: ");
                                    if (cReportUnsatCore)
                                        cZ3Solver.ConsoleWriteUnsatCore();
                                }
                                else if (pSatResult.Equals(Status.UNSATISFIABLE))
                                {
                                    //If the result of the analysis is UNSAT meaning that there was NO deadlock found

                                    //Print and writes an output file showing the result of a finished test
                                    if (cReportAnalysisDetailResult)
                                    {
                                        cOutputHandler.printMessageToConsole("Model No " + pState + ":");
                                        if (cReportVariantsResult)
                                        {
                                            cOutputHandler.printChosenVariants();
                                        }
                                        if (cReportTransitionsResult)
                                            cOutputHandler.printOperationsTransitions();
                                    }

                                    if (cCreateHTMLOutput)
                                    {
                                        cOutputHandler.writeFinished();
                                        cOutputHandler.writeFinishedNoPost();
                                    }
                                }
                            //}
                            /*else
                            {
                                //If it is in the middle of the analysis, not all transitions have been done
                                if (cReportVariantsResult)
                                    output.printChosenVariants();
                                if (cReportTransitionsResult)
                                    output.printOperationsTransitions();
                            }*/

                            break;
                        }
                    case Enumerations.AnalysisType.AlwaysDeadlockAnalysis:
                        {
                            if (pSatResult.Equals(Status.SATISFIABLE))
                            {
                            }
                            else if (pSatResult.Equals(Status.UNSATISFIABLE))
                            {

                            }
                            break;
                        }
                    case Enumerations.AnalysisType.CompleteAnalysis:
                        {
                            if (pSatResult.Equals(Status.SATISFIABLE))
                            {
                            }
                            else if (pSatResult.Equals(Status.UNSATISFIABLE))
                            {

                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReportSolverResults");
                cOutputHandler.printMessageToConsole(ex.Message);
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

                convertFParts2Z3Parts();

                //formula 2
                produceVariantGroupGCardinalityConstraints();

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertProductPlatform");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in convertConfigurationRules");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in convertOperations");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/


        /// <summary>
        /// This function converts operation relations (precedence rules)
        /// and the relationship between operations and variants
        /// </summary>
        /// <param name="pState"></param>
        private void convertOperationsPrecedenceRules()
        {
            try
            {
                //TODO: Here we have to check if the operations have really been converted.

                if (cNeedPreAnalysis && cCurrentTransitionNumber.Equals(0))
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
                    convertFOperationsPrecedenceRulesNStatusControl2Z3ConstraintNewVersion();
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertOperationsPrecedenceRules");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                    if (cFrameworkWrapper.ResourceSet.Count != 0)
                    {
                        convertFResource2Z3Constraints();
                        checkFOperationExecutabilityWithCurrentResourcesUsingZ3Constraints();
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertResourcesNOperationResourceRelations");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// This function converts the goal(s)
        /// </summary>
        /// <param name="pState"></param>
        /// <param name="pDone"></param>
        ////private void convertGoal(int pState, bool pDone)
        private void convertExistenceOfDeadlockGoal(int pState)
        {
            //TODO: this function is not needed and the lower level function down below can be called straight away
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
                            convertExistenceOfDeadlockGoalV2(pState);
                    }
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertGoal");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                            cOutputHandler.printMessageToConsole("Analysis No: " + pState);
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

                        cOutputHandler.printMessageToConsole("Finished: ");
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
                cOutputHandler.printMessageToConsole("error in anlyzeModel");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lTestResult;
        }

        private void convertFResource2Z3Constraints()
        {
            try
            {
                //List<resource> lResourceList = cFrameworkWrapper.ResourceList;
                foreach (resource lResource in cFrameworkWrapper.ResourceSet)
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
                cOutputHandler.printMessageToConsole("error in convertFResource2Z3Constraints");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                        //IntExpr lExprVariable = cZ3Solver.FindIntExpressionUsingName(lAttributeName);
                        Expr lExprVariable = cZ3Solver.FindExprInExprSet(lAttributeName);
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
                cOutputHandler.printMessageToConsole("error in convertFResourceAttribute2Z3Constraint");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private void checkFOperationExecutabilityWithCurrentResourcesUsingZ3Constraints()
        {
            try
            {

                //foreach (Operation lActiveOperation in cFrameworkWrapper.ActiveOperationSet)
                foreach (Operation lActiveOperation in cFrameworkWrapper.OperationSet)
                {
//                    List<string> lPossibleResourceVariablesForActiveOperation = new List<string>();
                    List<string> lUseResourceVariablesForActiveOperation = new List<string>();
                    
                    //This variable shows if the current operation can be run with at least one resource
                    string lPossibleToRunActiveOperationName = "Possible_to_run_" + lActiveOperation.Name;
                    cZ3Solver.AddBooleanExpression(lPossibleToRunActiveOperationName);

                    foreach (resource lActiveResource in cFrameworkWrapper.ResourceSet)
                    {
                        //This variable shows if the current operation CAN be run with the current resource
                        string lPossibleToUseResource4OperationName = "Possible_to_use_" + lActiveResource.names + "_for_" + lActiveOperation.Name;
                        cZ3Solver.AddBooleanExpression(lPossibleToUseResource4OperationName);
//                        lPossibleResourceVariablesForActiveOperation.Add(lPossibleToUseResource4OperationName);

                        //This variable shows if the current operation WILL be run with the current resource
                        string lUseResource4OperationName = "Use_" + lActiveResource.names + "_for_" + lActiveOperation.Name;
                        cZ3Solver.AddBooleanExpression(lUseResource4OperationName);
                        lUseResourceVariablesForActiveOperation.Add(lUseResource4OperationName);

                        //formula 6.1
                        //Possible_to_use_ActiveResource_for_ActiveOperation <-> Operation.Requirement
                        string lActiveOperationRequirements = lActiveOperation.Requirement;
                        if (lActiveOperationRequirements != "")
                        {
                            //Active operation has requirements defined

                            HashSet<resource> lOperationChosenResources = cFrameworkWrapper.ReturnOperationChosenResource(lActiveOperation.Name);

                            if (lOperationChosenResources.Contains(lActiveResource))
                            {
                                //This active resource is one of the resources that can run this operation
                                BoolExpr lActiveOperationRequirementExpr = returnFExpression2Z3Constraint(lActiveOperationRequirements);
                                /*cZ3Solver.AddTwoWayImpliesOperator2Constraints(cZ3Solver.FindBoolExpressionUsingName(lPossibleToUseResource4OperationName)
                                                                            , lActiveOperationRequirementExpr
                                                                            , "formula 6.1");*/
                                cZ3Solver.AddTwoWayImpliesOperator2Constraints((BoolExpr)cZ3Solver.FindExprInExprSet(lPossibleToUseResource4OperationName)
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
                            cZ3Solver.AddConstraintToSolver((BoolExpr)cZ3Solver.FindExprInExprSet(lPossibleToRunActiveOperationName), "formula 6.1");


                        //formula 6.2
                        // Use_ActiveResource_ActiveOperation -> Possible_to_use_ActiveResource_for_ActiveOperation
                        cZ3Solver.AddImpliesOperator2Constraints((BoolExpr)cZ3Solver.FindExprInExprSet(lUseResource4OperationName)
                                                                , (BoolExpr)cZ3Solver.FindExprInExprSet(lPossibleToUseResource4OperationName)
                                                                , "formula 6.2");
                    }

                    /////////////////////////////////////////////////////////
                    //formula 6.3
                    //This formula makes sure this operation can be run by ONLY one resource

                    //Possible_to_run_ActiveOperation -> Possible_to_use_Resource1_for_ActiveOperation or Possible_to_use_Resource2_for_ActiveOperation or ...

                    //                    lPossibleToRunOperationVariableNames.Add(lPossibleToRunActiveOperationName);
                    BoolExpr lPossibleToRunActiveOperation = (BoolExpr)cZ3Solver.FindExprInExprSet(lPossibleToRunActiveOperationName);
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
                    if (!lActiveOperation.Precondition.Contains(lPossibleToRunActiveOperationName))
                        lActiveOperation.Precondition.Add(lPossibleToRunActiveOperationName);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertFResourceOperationMapping2Z3Constraints");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                foreach (variant lVariant in cFrameworkWrapper.VariantSet)
                    cZ3Solver.AddBooleanExpression(lVariant.names);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertFVariants2Z3Variants");
                cOutputHandler.printMessageToConsole(ex.Message);
            }

        }

        public void convertFParts2Z3Parts()
        {
            try
            {
                foreach (part lPart in cFrameworkWrapper.PartSet)
                    cZ3Solver.AddBooleanExpression(lPart.names);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertFParts2Z3Parts");
                cOutputHandler.printMessageToConsole(ex.Message);
            }

        }

        //TODO: is it needed?
        /*private variant ReturnCurrentVariant(string pVariantExpression)
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
                    lResultVariant = cFrameworkWrapper.variantLookupByName(pVariantExpression);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnCurrentVariant");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultVariant;
        }*/

        /// <summary>
        /// In this function we go over all the defined operations and for the active operations we define current state and next state variables in Z3 model
        /// For the inactive operations we only define current state variables in the Z3 model
        /// </summary>
        /// <param name="pState"></param>
        public void convertFOperations2Z3Operations()
        {
            try
            {
                // Here we have to loop over all operations and operation instances and create the needed variables


                var lOperations = cFrameworkWrapper.OperationSet;

                foreach (var lOperation in lOperations)
                {
                    AddZ3ModelVariable(lOperation.OperationTriggerVariableName);
                    AddZ3ModelVariable(lOperation.OperationRequirementVariableName);

                }

                //Now for creating operation instance variables
                foreach (var lCurrentOperationInstance in cFrameworkWrapper.OperationInstanceSet)
                {
                    AddZ3ModelVariable(lCurrentOperationInstance.OperationPreconditionVariableName);

                    AddZ3ModelVariable(lCurrentOperationInstance.InitialVariableName);
                    AddZ3ModelVariable(lCurrentOperationInstance.ExecutingVariableName);
                    AddZ3ModelVariable(lCurrentOperationInstance.FinishedVariableName);
                    AddZ3ModelVariable(lCurrentOperationInstance.UnusedVariableName);
                }

                if (cCurrentTransitionNumber.Equals(0))
                {
                    //forula 4 - Setting operation status if they were picked or not
                    // C = (BIG AND) (O_I_j_0 <=> O_Trigger) AND (! O_e_j_0) AND (! O_f_j_0) AND (O_u_j_0 <=> (! O_Trigger))
                    initializeFOperation2Z3Constraints();
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertFOperations2Z3Operations");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private void AddZ3ModelVariable(string pVariableName)
        {
            try
            {
                cZ3Solver.AddBooleanExpression(pVariableName);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddZ3ModelVariable");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        //This function is removed in the new version when operation instances have become a seperate object
        /*/// <summary>
        /// In this function the instance variables for each operation are created, in these variables the operation will be assigned to a state, part, and transitionNo
        /// </summary>
        /// <param name="pOperationName">The operation for which the instance variables will be defined</param>
        /// <param name="pPartIndex">The variant which will be assigned to this operation instance variables</param>
        /// <param name="pState">The transitionNo which will be assigned to this operation instance variable</param>
        private void addCurrentPartOperationInstanceVariables(string pOperationName, int pPartIndex, int pState)
        {
            try
            {
                string lOperationInitialVariableName = ReturnOperationInstanceName(pOperationName,"I",pPartIndex,pState);
                cZ3Solver.AddBooleanExpression(lOperationInitialVariableName);
                cFrameworkWrapper.addOperationInstance(lOperationInitialVariableName);

                string lOperationExecutingVariableName = ReturnOperationInstanceName(pOperationName,"E",pPartIndex,pState);
                cZ3Solver.AddBooleanExpression(lOperationExecutingVariableName);
                cFrameworkWrapper.addOperationInstance(lOperationExecutingVariableName);

                string lOperationFinishedVariableName = ReturnOperationInstanceName(pOperationName,"F",pPartIndex,pState);
                cZ3Solver.AddBooleanExpression(lOperationFinishedVariableName);
                cFrameworkWrapper.addOperationInstance(lOperationFinishedVariableName);

                string lOperationUnusedVariableName = ReturnOperationInstanceName(pOperationName,"U",pPartIndex,pState);
                cZ3Solver.AddBooleanExpression(lOperationUnusedVariableName);
                cFrameworkWrapper.addOperationInstance(lOperationUnusedVariableName);

                string lOperationPreConditionName = ReturnOperationInstanceName(pOperationName,"PreCondition",pPartIndex,pState);
                cZ3Solver.AddBooleanExpression(lOperationPreConditionName);
//                lFrameworkWrapper.addOperationInstance(lOperationPreConditionName);

                //string lOperationPostConditionName = ReturnOperationInstanceName(pOperationName,"PostCondition",pPartIndex,pState);
                //cZ3Solver.AddBooleanExpression(lOperationPostConditionName);
//                lFrameworkWrapper.addOperationInstance(lOperationPostConditionName);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in addCurrentPartOperationInstances");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        //This function is removed in the new version when operation instances have become a seperate object
        /*/// <summary>
        /// In this function the instance variables for each operation are created, in these variables the operation will be assigned to a state, part, and transitionNo
        /// </summary>
        /// <param name="pOperationName">The operation for which the instance variables will be defined</param>
        /// <param name="pVariantIndex">The variant which will be assigned to this operation instance variables</param>
        /// <param name="pState">The transitionNo which will be assigned to this operation instance variable</param>
        private void addCurrentVariantOperationInstanceVariables(string pOperationName, int pVariantIndex, int pState)
        {
            try
            {
                string lOperationInitialVariableName = ReturnOperationInstanceName(pOperationName, "I", pVariantIndex, pState);
                cZ3Solver.AddBooleanExpression(lOperationInitialVariableName);
                cFrameworkWrapper.addOperationInstance(lOperationInitialVariableName);

                string lOperationExecutingVariableName = ReturnOperationInstanceName(pOperationName, "E", pVariantIndex, pState);
                cZ3Solver.AddBooleanExpression(lOperationExecutingVariableName);
                cFrameworkWrapper.addOperationInstance(lOperationExecutingVariableName);

                string lOperationFinishedVariableName = ReturnOperationInstanceName(pOperationName, "F", pVariantIndex, pState);
                cZ3Solver.AddBooleanExpression(lOperationFinishedVariableName);
                cFrameworkWrapper.addOperationInstance(lOperationFinishedVariableName);

                string lOperationUnusedVariableName = ReturnOperationInstanceName(pOperationName, "U", pVariantIndex, pState);
                cZ3Solver.AddBooleanExpression(lOperationUnusedVariableName);
                cFrameworkWrapper.addOperationInstance(lOperationUnusedVariableName);

                string lOperationPreConditionName = ReturnOperationInstanceName(pOperationName, "PreCondition", pVariantIndex, pState);
                cZ3Solver.AddBooleanExpression(lOperationPreConditionName);
                //                lFrameworkWrapper.addOperationInstance(lOperationPreConditionName);

                //string lOperationPostConditionName = ReturnOperationInstanceName(pOperationName, "PostCondition", pVariantIndex, pState);
                //cZ3Solver.AddBooleanExpression(lOperationPostConditionName);
                //                lFrameworkWrapper.addOperationInstance(lOperationPostConditionName);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in addCurrentVariantOperationInstances");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        //This function is removed in the new version when operation instances have become a seperate object
        /*private void addCurrentActiveVariantToActiveVariantList(string pOperationName, int pVariantIndex, int pState)
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

                //string lOperationPostConditionName = ReturnOperationInstanceName(pOperationName,"PostCondition",pVariantIndex,pState);
                //cFrameworkWrapper.addActiveOperationInstanceName(lOperationPostConditionName);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in addCurrentActiveVariantToActiveVariantList");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        //This function is removed in the new version when operation instances have become a seperate object
        /*public void resetCurrentStateAndNewStateOperationVariables(operation pOperation, variant pVariant, int pState, string pConstraintSource)
        {
            try
            {
                int lVariantIndex = cFrameworkWrapper.indexLookupByVariant(pVariant);
                cOperationInstance_CurrentState = new OperationInstance(pOperation.names+"_"+lVariantIndex,pState);

                int lNewState = pState + 1;

                cOperationInstance_NextState = new OperationInstance(pOperation.names + "_" + lVariantIndex, lNewState);

                resetOperationPrecondition(pOperation, pVariant, pState, pConstraintSource);
                //resetOperationPostcondition(pOperation, pVariant, pState, pConstraintSource);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in resetCurrentStateAndNewStateOperationVariables");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        //This function is removed in the new version when operation instances have become a seperate object
        /*public void resetCurrentStateAndNewStateOperationVariables(operation pOperation, part pPart, int pState, string pConstraintSource)
        {
            try
            {
                int lPartIndex = cFrameworkWrapper.indexLookupByPart(pPart);
                cOperationInstance_CurrentState = new OperationInstance(pOperation.names + "_" + lPartIndex, pState);

                int lNewState = pState + 1;

                cOperationInstance_NextState = new OperationInstance(pOperation.names + "_" + lPartIndex, lNewState);

                resetOperationPrecondition(pOperation, pPart, pState, pConstraintSource);
                //resetOperationPostcondition(pOperation, pPart, pState, pConstraintSource);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in resetCurrentStateAndNewStateOperationVariables");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        //This function is removed in the new version when operation instances have become a seperate object
        /*public void resetCurrentStateOperationVariables(operation pOperation, part pPart, int pState)
        {
            try
            {
                int lPartIndex = cFrameworkWrapper.indexLookupByPart(pPart);

                cOperationInstance_NextState = new OperationInstance(pOperation.names + "_" + lPartIndex, pState);

                resetOperationPrecondition(pOperation, pPart, pState, "Don't know");
                //resetOperationPostcondition(pOperation, pPart, pState, "Don't know");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in resetCurrentStateOperationVariables");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        //This function is removed in the new version when operation instances have become a seperate object
        /*public void resetCurrentStateOperationVariables(operation pOperation, variant pVariant, int pState)
        {
            try
            {
                int lVariantIndex = cFrameworkWrapper.indexLookupByVariant(pVariant);

                cOperationInstance_CurrentState = new OperationInstance(pOperation.names + "_" + lVariantIndex, pState);

                resetOperationPrecondition(pOperation, pVariant, pState, "Don't know");
                //resetOperationPostcondition(pOperation, pVariant, pState, "Don't know");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in resetCurrentStateOperationVariables");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        public void produceVariantGroupGCardinalityConstraints()
        {
            try
            {
                //Formula 2
                //List<variantGroup> localVariantGroupsList = cFrameworkWrapper.VariantGroupList;

                foreach (variantGroup lVariantGroup in cFrameworkWrapper.VariantGroupSet)
                {
                    makeGCardinalityConstraint(lVariantGroup);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in produceVariantGroupGCardinalityConstraints");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        //TODO: We need an enum for the Variant Group Cardinality to be used here and also used where the Variant Groups are created from the input files
        public void makeGCardinalityConstraint(variantGroup pVariantGroup)
        {
            try
            {
                //Formula 2
                List<variant> lVariants = pVariantGroup.variants;

                List<string> lVariantNames = new List<string>();
                foreach (variant lVariant in lVariants)
                    lVariantNames.Add(lVariant.names);

                if (lVariantNames.Count.Equals(1))
                    cZ3Solver.AddConstraintToSolver((BoolExpr)cZ3Solver.FindExprInExprSet(lVariantNames.First()), "GroupCardinality");
                else
                {
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

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void initializeFOperation2Z3Constraints()
        {
            try
            {
                //First for each operation we have to specify the trigger condition
                HashSet<Operation> lOperationSet = cFrameworkWrapper.OperationSet;
                foreach (var lOperation in lOperationSet)
                {
                    //O_Trigger <=> Trigger expression
                    cZ3Solver.AddTwoWayImpliesOperator2Constraints((BoolExpr)cZ3Solver.FindExprInExprSet(lOperation.OperationTriggerVariableName)
                                                                    , convertComplexString2BoolExpr(lOperation.Trigger)
                                                                    , "Operation_Trigger_Initializing");
                }

                //formula 4
                // C = (BIG AND) (O_I_j_0 <=> O_Trigger) AND (! O_e_j_0) AND (! O_f_j_0) AND (O_u_j_0 <=> (! O_Trigger))
                //This formula has to be applied to all operation instances
                //Each operation has the field of when it is triggered which can be used in this formula.
                HashSet<OperationInstance> lOperationInstanceList = cFrameworkWrapper.OperationInstanceSet;

                foreach (OperationInstance lCurrentOperationInstance in lOperationInstanceList)
                {
                    if (lCurrentOperationInstance.TransitionNumber.Equals(0))
                    {
                        //(O_I_j_0 <=> O_Trigger)
                        BoolExpr lFirstPart = cZ3Solver.TwoWayImpliesOperator(lCurrentOperationInstance.InitialVariableName
                                                                            , lCurrentOperationInstance.AbstractOperation.OperationTriggerVariableName);

                        //(! O_e_j_0)
                        BoolExpr lSecondPart = cZ3Solver.NotOperator(lCurrentOperationInstance.ExecutingVariableName);
                        //(! O_f_j_0)
                        BoolExpr lThirdPart = cZ3Solver.NotOperator(lCurrentOperationInstance.FinishedVariableName);

                        //(O_u_j_0 <=> (! O_Trigger))
                        BoolExpr lFirstOperand = (BoolExpr)cZ3Solver.FindExprInExprSet(lCurrentOperationInstance.UnusedVariableName);
                        BoolExpr lFourthPart = cZ3Solver.TwoWayImpliesOperator(lFirstOperand
                                                                            , cZ3Solver.NotOperator(lCurrentOperationInstance.AbstractOperation.OperationTriggerVariableName));

                        //(O_I_j_0 <=> O_Trigger) AND (! O_e_j_0) AND (! O_f_j_0) AND (O_u_j_0 <=> (! O_Trigger))
                        BoolExpr lWholeFormula = cZ3Solver.AndOperator(new List<BoolExpr>() { lFirstPart, lSecondPart, lThirdPart, lFourthPart });

                        //(BIG AND) (O_I_j_0 <=> O_Trigger) AND (! O_e_j_0) AND (! O_f_j_0) AND (O_u_j_0 <=> (! O_Trigger))
                        if (cConvertOperations)
                            cZ3Solver.AddAndOperator2Constraints(new List<BoolExpr>() { lWholeFormula }, "formula4-Operation_Status_Initializing");

                        if (cBuildPConstraints)
                            AddPrecedanceConstraintToLocalList(lWholeFormula);
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in initializeFVariantOperation2Z3Constraints");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in AddPrecedanceConstraintToLocalList");
                cOutputHandler.printMessageToConsole(ex.Message);
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

        /// <summary>
        /// This function checks the precondition, to see if the operations in the precondition are from the active operations or inactive ones
        /// incase they are inactive they make that part of the precondition to false
        /// </summary>
        /// <param name="pPrecondition"></param>
        /*public bool preconditionSanityCheck(string pPrecondition)
        {
            bool lSanityCheckResult = false;
            try
            {
                string[] lParts = pPrecondition.Split('_');
                string lOperationName = lParts[0];
                OperationInstance lOperationInstance = cFrameworkWrapper.operationInstanceLookupByName(lOperationName);
                if (lOperationInstance.Status.Equals(Enumerations.OperationStatus.Active))
                {
                    lSanityCheckResult = true;
                }
                else
                {
                    //If the operation mentioned in the precondition is inactive
                    //That means the precondition is going to be false!
                    lSanityCheckResult = false;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in preconditionSanityCheck");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lSanityCheckResult;
        }*/

        /*public void resetOperationPrecondition(operation pOperation, variant pVariant, int pState, string pConstraintSource)
        {
            try
            {
                int lCurrentVariantIndex = cFrameworkWrapper.indexLookupByVariant(pVariant);
                //BoolExpr lOpPrecondition = lZ3Solver.MakeBoolVariable("lOpPrecondition");
                cOpPrecondition = ReturnOperationInstanceVariable(pOperation.names, "PreCondition", lCurrentVariantIndex, pState);

                if (pOperation.precondition != null)
                {
                    if (pOperation.precondition.Count != 0)
                    {
                        HashSet<BoolExpr> lPreconditionExpressions = new HashSet<BoolExpr>();
                        foreach (var lPrecondition in pOperation.precondition)
                        {
                            //Checks the precondition if the operation is an active operation or an inactive operation
                            if (preconditionSanityCheck(lPrecondition))
                            {
                                //If the operation HAS a precondition
                                //By "Complete" we mean it mentions the operation state, variant, and transition
                                if (IsOperationInstanceComplete(lPrecondition))
                                {
                                    //If the precondition has all parts mentioned, i.e. operation name, operation state, operation variant, operation transition no
                                    if (cFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(lPrecondition) > pState)
                                    {
                                        //This means the precondition is on a transition state which has not been reached yet!
                                        //lOpPrecondition = lZ3Solver.NotOperator(lOpPrecondition);
                                        cZ3Solver.AddConstraintToSolver(cZ3Solver.NotOperator(lPrecondition), pConstraintSource);
                                    }
                                    else
                                    {
                                        //We already know that the precondition is a complete operation instance
                                        //This means the precondition includes an operation status
                                        //lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0] + "_" + currentVariant.index  + "_" + pState.ToString());
                                        lPreconditionExpressions.Add((BoolExpr)cZ3Solver.FindExprInExprSet(lPrecondition));
                                    }
                                }
                                else
                                {
                                    //BoolExpr lTempBoolExpr = convertIncompleteOperationInstances2CompleteOperationInstanceExpr(lPrecondition, pVariant.index, pState);
                                    BoolExpr lTempBoolExpr = convertComplexString2BoolExpr(lPrecondition);
                                    lPreconditionExpressions.Add(lTempBoolExpr);
                                }
                            }
                            else
                            {
                                //if the sanity check turned out false, meaning the precondition contains references to inactive operations
                                //which means the precondition should be false
                                cZ3Solver.AddConstraintToSolver(cZ3Solver.NotOperator(cOpPrecondition), pConstraintSource);
                                lPreconditionExpressions.Add(cOpPrecondition);
                            }
                        }
                        cOpPrecondition = cZ3Solver.AndOperator(lPreconditionExpressions);
                    }
                    else
                        //If the operation DOES NOT have a precondition hence
                        //We want to force the precondition to be true
                        cZ3Solver.AddConstraintToSolver(cOpPrecondition, pConstraintSource);
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in resetOperationPrecondition");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        /// <summary>
        /// Gets an expression defined over variants and returns a list of containing variants
        /// </summary>
        /// <param name="pVariantExpr">Expression defined over variants</param>
        /// <returns>List of variants</returns>
        public List<variant> GetListOfVariantsFromAVariantExpr(string pVariantExpr)
        {
            List<variant> lResultVariantList = new List<variant>();
            try
            {
                string[] lExprParts = pVariantExpr.Split(' ');
                foreach (string lExprPart in lExprParts)
                {
                    if (lExprPart != "and" || lExprPart != "or" || lExprPart != "not")
                    {
                        variant lVariant = cFrameworkWrapper.variantLookupByName(lExprPart);
                        lResultVariantList.Add(lVariant);
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in GetListOfVariantsFromAVariantExpr");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultVariantList;
        }

        /// <summary>
        /// Gets an expression defined over parts and returns a list of containing parts
        /// </summary>
        /// <param name="pVariantExpr">Expression defined over parts</param>
        /// <returns>List of parts</returns>
        public HashSet<part> GetListOfPartsFromAPartExpr(string pPartExpr)
        {
            HashSet<part> lResultPartList = new HashSet<part>();
            try
            {
                string[] lExprParts = pPartExpr.Split(' ');
                foreach (string lExprPart in lExprParts)
                {
                    if (lExprPart != "and" || lExprPart != "or" || lExprPart != "not")
                    {
                        part lPart = cFrameworkWrapper.partLookupByName(lExprPart);
                        lResultPartList.Add(lPart);
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in GetListOfPartsFromAPartExpr");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultPartList;
        }

        //In the new version, after we created the trigger field of the operation hence removing part operation mapping, there is no more need for this function
        /*/// <summary>
        /// In this function we look at the part - operation mapping and return a list of parts which are related to an operation
        /// </summary>
        /// <param name="pOperation"></param>
        public HashSet<part> FindRelatedParts(string pOperation)
        {
            HashSet<part> lResultPartList = new HashSet<part>();
            try
            {
                //List<partOperations> lPartOperationsList = cFrameworkWrapper.PartsOperationsList;
                foreach (var lPartOperationMapping in cFrameworkWrapper.PartsOperationsSet)
                {
                    HashSet<operation> lOperationsList = lPartOperationMapping.getOperations();
                    if (lOperationsList.Contains(cFrameworkWrapper.getOperationFromOperationName(pOperation)))
                    {
                        string lPartExpr = lPartOperationMapping.getPartExpr();
                        HashSet<part> lPartList = GetListOfPartsFromAPartExpr(lPartExpr); 
                        lResultPartList = lPartList;

                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in FindRelatedParts");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultPartList;
        }*/

        //In the new version, after we created the trigger field of the operation hence removing variant operation mapping, there is no more need for this function
        /*/// <summary>
        /// In this function we look at the variant - operation mapping and return a list of variants which are related to an operation
        /// </summary>
        /// <param name="pOperation"></param>
        public List<variant> FindRelatedVariants(string pOperation)
        {
            List<variant> lResultVariantList = new List<variant>();
            try
            {
                foreach (var lVariantOperationMapping in cFrameworkWrapper.VariantsOperationsSet)
                {
                    HashSet<operation> lOperationsList = lVariantOperationMapping.getOperations();
                    if (lOperationsList.Contains(cFrameworkWrapper.getOperationFromOperationName(pOperation)))
                    {
                        string lVariantExpr = lVariantOperationMapping.getVariantExpr();
                        List<variant> lVariantList = GetListOfVariantsFromAVariantExpr(lVariantExpr);
                        lResultVariantList.AddRange(lVariantList);

                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in FindRelatedVariants");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultVariantList;
        }*/

        //This function was removed as it was redundant and the same as the next function
        /*//This function is ONLY used for the operation instances which are named in the precondition field of operations
        public BoolExpr convertPartialOperationInstance2CompleteForm(string pOperationInstanceName, int pCurrentTransition)
        {
            BoolExpr lOperationInstance = null;
            try
            {
                //First we check the inputed operation instance to see if it is complete or not
                //By "Complete" we mean it mentions the operation state and transition
                if (!IsOperationInstanceComplete(pOperationInstanceName))
                {
                    //Either operation instnce is missing operation status, or it s missing the transition no
                    if (IsOperationInstanceMissingStatus(pOperationInstanceName))
                    {
                        //In this part operation instance is missing operation status
                        //This operation instance should consider state FINISHED or UNUSED and current transition

                        //We KNOW that if the status is missing then the only part in the operation instance should be the operation name
                        //Here we assume that the operation is mentioned correctly
                        string lOperationInstanceOperationName = pOperationInstanceName;

                        //Note: In this part we want to implement weak links, which means if an operation is mentioned as a precondition 
                        //it means either the operation is in finished state or in an unused state

                        OperationInstance lTempOperationInstance = new OperationInstance(lOperationInstanceOperationName
                                                                                         , pCurrentTransition);

                        BoolExpr cOperationInstancePart1 = (BoolExpr)cZ3Solver.FindExprInExprSet(lTempOperationInstance.FinishedVariableName);


                        BoolExpr cOperationInstancePart2 = (BoolExpr)cZ3Solver.FindExprInExprSet(lTempOperationInstance.UnusedVariableName);

                        lOperationInstance = cZ3Solver.OrOperator(new List<BoolExpr>() { cOperationInstancePart1, cOperationInstancePart2 });
                    }
                    else if (IsOperationInstanceMissingTransitionNo(pOperationInstanceName))
                    {
                        //This precondition should consider current transitions
                        Operation lPreconditionOperation = cFrameworkWrapper.ReturnOperationFromOperationInstanceName(pOperationInstanceName);
                        string lOperationState = cFrameworkWrapper.ReturnOperationStateFromOperationInstanceName(pOperationInstanceName);

                        lOperationInstance = OperationInAnyTransitions(lPreconditionOperation
                                                                    , lOperationState);
                    }
                }
                else
                    lOperationInstance = (BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstanceName);

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertPartialOperationInstance2CompleteForm");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lOperationInstance;
        }*/

        public BoolExpr convertIncompleteOperationInstances2CompleteOperationInstanceExpr(string pIncompleteOperationInstance, int pTransitionNumber) 
        {
            BoolExpr lResultBoolExpr = null;
            try
            {
                //By "Complete" we mean it mentions the operation state and transition
                if (IsOperationInstanceComplete(pIncompleteOperationInstance))
                {
                    lResultBoolExpr = (BoolExpr)cZ3Solver.FindExprInExprSet(pIncompleteOperationInstance);
                }
                else
                {
                    //If some part of the precondition are missing, i.e. operation name, operation state, operation variant, operation transition no
                    if (IsOperationInstanceMissingStatus(pIncompleteOperationInstance))
                    {
                        //This preconditon should consider state FINISHED or state UNUSED and any variants and any transitions
                        string lOperationName = pIncompleteOperationInstance;
                        var lOperation = cFrameworkWrapper.getOperationFromOperationName(lOperationName);
                        //Note: In this part we want to implement weak links, which means if an operation is mentioned as a precondition 
                        //it means either the operation is in finished state or in an unused state
                        //Hence we have to make an operation instance from it
                        BoolExpr lBoolExprPart1 = ConvertOperationInstanceWithMissingTransitionNumber(lOperation, Enumerations.OperationInstanceState.Finished, pTransitionNumber);
                        BoolExpr lBoolExprPart2 = ConvertOperationInstanceWithMissingTransitionNumber(lOperation, Enumerations.OperationInstanceState.Unused, pTransitionNumber);

                        lResultBoolExpr = cZ3Solver.OrOperator(new List<BoolExpr>() { lBoolExprPart1, lBoolExprPart2 });
                    }
                    else if (IsOperationInstanceMissingTransitionNo(pIncompleteOperationInstance))
                    {
                        //This precondition should consider current transitions
                        string lOperationName = cFrameworkWrapper.ReturnOperationNameFromOperationInstanceName(pIncompleteOperationInstance);
                        var lOperation = cFrameworkWrapper.getOperationFromOperationName(lOperationName);
                        Enumerations.OperationInstanceState lOperationState = cFrameworkWrapper.ReturnOperationStateFromOperationInstanceName(pIncompleteOperationInstance);

                        lResultBoolExpr = ConvertOperationInstanceWithMissingTransitionNumber(lOperation, lOperationState, pTransitionNumber);
                    }

                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertIncompleteOperationInstances2CompleteOperationInstanceExpr");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultBoolExpr;
        }

        /// <summary>
        /// This function takes an operation instance name and looks at the parts (operation name, operation state, operation transition no)
        /// and makes sure all parts are mentioned in an operation instance or not
        /// </summary>
        /// <param name="pOperationInstanceName"></param>
        private bool IsOperationInstanceComplete(string pOperationInstanceName)
        {
            bool lResult = false;
            try
            {
                string[] lOperationInstanceParts = pOperationInstanceName.Split('_');
                //(operation name, operation state, operation transition no)
                if (lOperationInstanceParts.Length.Equals(3))
                    lResult = true;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in IsOperationInstanceComplete");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in IsOperationInstanceMissingTransitionNo");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in IsOperationInstanceMissingVariant");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResult;
        }

        /// <summary>
        /// This function checks if an operation instance has the state part missing
        /// </summary>
        /// <param name="pOperationInstanceName"></param>
        /// <returns></returns>
        private bool IsOperationInstanceMissingStatus(string pOperationInstanceName)
        {
            bool lResult = true;
            try
            {
                string[] lOperationInstanceParts = pOperationInstanceName.Split('_');
                //ONLY operation instance name mentioned
                if (lOperationInstanceParts.Length.Equals(1))
                    lResult = false;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in IsOperationInstanceMissingStatus");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResult;
        }

        /*public void resetOperationPostcondition(operation pOperation, part pPart, int pState, String pPostconditionSource)
        {
            try
            {
                int lPartIndex = cFrameworkWrapper.indexLookupByPart(pPart);
                cOpPostcondition = ReturnOperationInstanceVariable(pOperation.names, "PostCondition", lPartIndex, pState);

                if (pOperation.postcondition != null)
                {
                    if (pOperation.postcondition.Count != 0)
                    {
                        HashSet<BoolExpr> lPostconditionExpressions = new HashSet<BoolExpr>();
                        foreach (var lPostcondition in pOperation.postcondition)
                        {
                            if (IsOperationInstanceComplete(lPostcondition))
                            {
                                if (cFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(lPostcondition) > pState)
                                {
                                    //This means the postcondition is on a transition state which has not been reached yet!
                                    //cOpPostcondition = cZ3Solver.NotOperator(lPostcondition);
                                    cZ3Solver.AddConstraintToSolver(cZ3Solver.NotOperator(lPostcondition), pPostconditionSource);
                                }
                                else
                                {
                                    //This means the postcondition includes an operation status
                                    //lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.postcondition[0] + "_" + currentVariant.index  + "_" + pState.ToString());
                                    cOpPostcondition = ReturnOperationInstanceBoolExpr(lPostcondition);
                                }
                            }
                            else
                            {
                                BoolExpr lTempBoolExpr = convertIncompleteOperationInstances2CompleteOperationInstanceExpr(lPostcondition);
                                lPostconditionExpressions.Add(lTempBoolExpr);
                            }
                        }
                        cOpPostcondition = cZ3Solver.AndOperator(lPostconditionExpressions);
                    }
                    else
                        //We want to force the postcondition to be true
                        //lOpPostcondition = lOpPostcondition;
                        cZ3Solver.AddConstraintToSolver(cOpPostcondition, pPostconditionSource);

                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in resetOperationPostcondition");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        /*public void resetOperationPostcondition(operation pOperation, variant pVariant, int pState, String pPostconditionSource)
        {
            try
            {
                int lVariantIndex = cFrameworkWrapper.indexLookupByVariant(pVariant);
                cOpPostcondition = ReturnOperationInstanceVariable(pOperation.names, "PostCondition", lVariantIndex, pState);

                if (pOperation.postcondition != null)
                {
                    if (pOperation.postcondition.Count != 0)
                    {
                        HashSet<BoolExpr> lPostconditionExpressions = new HashSet<BoolExpr>();
                        foreach (var lPostcondition in pOperation.postcondition)
                        {
                            if (IsOperationInstanceComplete(lPostcondition))
                            {
                                if (cFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(lPostcondition) > pState)
                                {
                                    //This means the postcondition is on a transition state which has not been reached yet!
                                    //cOpPostcondition = cZ3Solver.NotOperator(lPostcondition);
                                    cZ3Solver.AddConstraintToSolver(cZ3Solver.NotOperator(lPostcondition), pPostconditionSource);
                                }
                                else
                                {
                                    //This means the postcondition includes an operation status
                                    //lOpPostcondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.postcondition[0] + "_" + currentVariant.index  + "_" + pState.ToString());
                                    cOpPostcondition = ReturnOperationInstanceBoolExpr(lPostcondition);
                                }
                            }
                            else
                            {
                                BoolExpr lTempBoolExpr = convertIncompleteOperationInstances2CompleteOperationInstanceExpr(lPostcondition);
                                lPostconditionExpressions.Add(lTempBoolExpr);
                            }
                        }
                        cOpPostcondition = cZ3Solver.AndOperator(lPostconditionExpressions);
                    }
                    else
                        //We want to force the postcondition to be true
                        //lOpPostcondition = lOpPostcondition;
                        cZ3Solver.AddConstraintToSolver(cOpPostcondition, pPostconditionSource);

                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in resetOperationPostcondition");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        public BoolExpr ReturnOperationInstanceBoolExpr(string pOperationInstance, int pTransitionNumber)
        {
            BoolExpr lResultExpr = null;
            try
            {
                if (IsOperationInstanceComplete(pOperationInstance))
                    lResultExpr = (BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance);
                else
                    lResultExpr = convertIncompleteOperationInstances2CompleteOperationInstanceExpr(pOperationInstance, pTransitionNumber);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in FindExprInExprList");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
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
                                lResult = cZ3Solver.AndOperator(new List<BoolExpr>() { ParseCondition(lChildren[0], pState)
                                                                                    , ParseCondition(lChildren[1], pState) });
                                break;
                            }
                        case "or":
                            {
                                lResult = cZ3Solver.OrOperator(new List<BoolExpr>() { ParseCondition(lChildren[0], pState)
                                                                                    , ParseCondition(lChildren[1], pState) });
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
                    if (lOperationNameParts.Length != 3)
                    {
                        //This means the precondition does not include a state
                        /*if (lOperationNameParts.Length == 2)
                        {
                            //This means the precondition does not include a variant nor a state
                            HashSet<string> vInstances = cFrameworkWrapper.getvariantInstancesForOperation(lOperationNameParts[0]);
                            HashSet<BoolExpr> opExpr = new HashSet<BoolExpr>();
                            foreach (string variant in vInstances)
                            {
                                opExpr.Add((BoolExpr)cZ3Solver.FindExprInExprSet(pCon + "_" + variant + "_" + pState.ToString()));

                            }
                            lResult = (cZ3Solver.OrOperator(opExpr));
                        }
                        else*/ if (lOperationNameParts.Length == 3)
                        {
                            //This means the precondition does includes a variant but not a state
                            lResult = ((BoolExpr)cZ3Solver.FindExprInExprSet(pCon + "_" + pState.ToString()));
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
                            lResult = (BoolExpr)cZ3Solver.FindExprInExprSet(pCon);
                        }
                }
                else
                    //This means the postcondition only includes an operation
                    throw new System.ArgumentException("Precondition did not include a status", pCon);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in mkCondition");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResult;
        }

        //TODO: I think this can be removed!!!
        /*private part returnCurrentVariant(partOperations pVariantOperations)
        {
            part lResultVariant = new part();
            try
            {
                part lCurrentVariant = cFrameworkWrapper.ReturnCurrentPart(pVariantOperations);

                //TODO: this should be done here for ALL types of variants
                if (lCurrentVariant.names.Contains("Virtual"))
                {
                    //The result variant is going to be a virtual variant hence for this variant we have to add the needed operations
                    addVirtualPartOperationInstances(lResultVariant, pVariantOperations.getOperations());
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in returnCurrentVariant");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultVariant;
        }*/

        //This function is not used anywhere in the new version after the operation instance has become a seperate object hence it is removed
        /*public void resetOperationPrecondition(OperationInstance pOperationInstance, string pConstraintSource)
        {
            try
            {

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in resetOperationPrecondition");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        public void initializingOperationPreconditionVariable(OperationInstance pOperationInstance)
        {
            try
            {
                string lConstraintSource = "Operation_Precondition_Initializing";

                /*if (pOperationInstance.AbstractOperation.Precondition.Count > 0)
                    //First we have to set the precondition field of each operation as the precondition is needed in these formulas
                    cZ3Solver.AddTwoWayImpliesOperator2Constraints((BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance.OperationPreconditionVariableName)
                                                                   , cZ3Solver.AndOperator(pOperationInstance.AbstractOperation.Precondition)
                                                                   , lConstraintSource);*/

                if (pOperationInstance.AbstractOperation.Precondition != null)
                {
                    if (pOperationInstance.AbstractOperation.Precondition.Count != 0)
                    {
                        List<BoolExpr> lPreconditionExpressions = new List<BoolExpr>();
                        foreach (var lPrecondition in pOperationInstance.AbstractOperation.Precondition)
                        {
                            //If the operation HAS a precondition
                            //By "Complete" we mean it mentions the operation state, and transition
                            if (IsOperationInstanceComplete(lPrecondition))
                            {
                                //This means firstly that this precondition is a single operation instance
                                //Secondly the precondition has all parts mentioned, i.e. operation name, operation state, and operation transition no
                                if (cFrameworkWrapper.getOperationTransitionNumberFromActiveOperation(lPrecondition) > pOperationInstance.TransitionNumber)
                                {
                                    //This means the precondition is on a transition state which has not been reached yet!
                                    //lOpPrecondition = lZ3Solver.NotOperator(lOpPrecondition);
                                    cZ3Solver.AddConstraintToSolver(cZ3Solver.NotOperator(lPrecondition), lConstraintSource);
                                }
                                else
                                {
                                    //We already know that the precondition is a complete operation instance
                                    //This means the precondition includes an operation status
                                    //lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0] + "_" + currentVariant.index  + "_" + pState.ToString());
                                    lPreconditionExpressions.Add((BoolExpr)cZ3Solver.FindExprInExprSet(lPrecondition));
                                }
                            }
                            else
                            {
                                //This means that the precondition has been an expression
                                BoolExpr lTempBoolExpr = convertComplexString2BoolExpr(lPrecondition);
                                lPreconditionExpressions.Add(lTempBoolExpr);
                            }
                        }
                        cZ3Solver.AddTwoWayImpliesOperator2Constraints((BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance.OperationPreconditionVariableName)
                                                                       , cZ3Solver.AndOperator(lPreconditionExpressions)
                                                                       , lConstraintSource);
                    }
                    else
                        //If the operation DOES NOT have a precondition hence
                        //We want to force the precondition to be true
                        cZ3Solver.AddConstraintToSolver((BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance.OperationPreconditionVariableName)
                                                        , lConstraintSource);
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in initializingOperationPreconditionVariable");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }
        public void convertFOperationsPrecedenceRulesNStatusControl2Z3ConstraintNewVersion()
        {
            try
            {

                //After the operation trigger field is introduced we will loop over operation instances

                var lOperationInstanceSet = cFrameworkWrapper.OperationInstanceSet;

                foreach (var lCurrentOperationInstance in lOperationInstanceSet)
                {
                    //First we have to set the precondition field of each operation as the precondition is needed in these formulas
                    initializingOperationPreconditionVariable(lCurrentOperationInstance);

                    //5.1: (O_I_k_j and Pre_k_j) => O_E_k_j+1
                    createFormula51(lCurrentOperationInstance);

                    //5.2: not (O_I_k_j and Pre_k_j) => (O_I_k_j <=> O_I_k_j+1)
                    createFormula52(lCurrentOperationInstance);

                    //5.3: XOR O_I_k_j O_E_k_j O_F_k_j O_U_k_j
                    createFormula53(lCurrentOperationInstance);

                    //5.4: (O_E_k_j AND Post_k_j) => O_F_k_j+1
                    createFormula54(lCurrentOperationInstance);

                    //5.6: O_U_k_j => O_U_k_j+1
                    createFormula56(lCurrentOperationInstance);

                    //5.7: O_F_k_j => O_F_k_j+1
                    createFormula57(lCurrentOperationInstance);
                }

                if (!cOperationMutualExecution)
                {
                    //formula 6
                    //In the list of operations, start with operations indexed for variant 0 and compare them with all operations indexed with one more
                    //For each pair (e.g. 0 and 1, 0 and 2,...) compare its current state with its new state on the operation_I
                    createFormula6();
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertFOperations2Z3ConstraintNewVersion");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }


        private void createFormula6()
        {
            try
            {
                //formula 6
                //In the list of operations, start with operations indexed for part 0 and compare them with all operations indexed with one more
                //For each pair (e.g. 0 and 1, 0 and 2,...) compare its current state with its new state on the operation_I

                BoolExpr lFormulaSix = null;

                //This will get all operation instances which are in the Initial state for the given state
                //HashSet<String> lActiveOperationSet = cFrameworkWrapper.getActiveOperationNamesSet(pState, "I");
                HashSet<OperationInstance> lOperationInstanceSet = cFrameworkWrapper.getOperationInstancesInOneTransition(cCurrentTransitionNumber);

                if (lOperationInstanceSet != null)
                {
/*                    foreach (OperationInstance lFirstOperationInstance in lOperationInstanceSet)
                    {
                        int lFirstIndex;
                        if (cFrameworkWrapper.UsePartInfo)
                            lFirstIndex = cFrameworkWrapper.getPartIndexFromActiveOperation(lFirstActiveOperation);
                        else
                            lFirstIndex = cFrameworkWrapper.getVariantIndexFromActiveOperation(lFirstActiveOperation);

                        foreach (String lSecondActiveOperation in lActiveOperationSet)
                        {
                            int lSecondIndex;
                            if (cFrameworkWrapper.UsePartInfo)
                                lSecondIndex = cFrameworkWrapper.getPartIndexFromActiveOperation(lSecondActiveOperation);
                            else
                                lSecondIndex = cFrameworkWrapper.getVariantIndexFromActiveOperation(lSecondActiveOperation);

                            if (lFirstIndex < lSecondIndex)
                            {
                                //Formula 6 = Big AND (!(O_I_k_j and !(O_I_k_(j+1)) AND (O_I_l_j AND !(O_I_l_(j+1)))))
                                
                                //lFirstOperand = O_I_k_j
                                BoolExpr lFirstOperand = (BoolExpr)cZ3Solver.FindExprInExprSet(lFirstActiveOperation);

                                //lSecondOperand = !(O_I_k_(j+1))
                                BoolExpr lSecondOperand = cZ3Solver.NotOperator(cFrameworkWrapper.giveNextStateActiveOperationName(lFirstActiveOperation));

                                //lFirstParanthesis = (lFirstOperand AND lSecondOperand)
                                BoolExpr lFirstParantesis = cZ3Solver.AndOperator(new List<BoolExpr>() { lFirstOperand, lSecondOperand });

                                //lThirdOperand = O_I_l_j
                                BoolExpr lThirdOperand = (BoolExpr)cZ3Solver.FindExprInExprSet(lSecondActiveOperation);

                                String lNextStateActiveOperationName = cFrameworkWrapper.giveNextStateActiveOperationName(lSecondActiveOperation);
                                if ((BoolExpr)cZ3Solver.FindExprInExprSet(lNextStateActiveOperationName) != null)
                                {
                                    //lFourthOperand = !(O_I_l_(j+1))
                                    BoolExpr lFourthOperand = cZ3Solver.NotOperator(lNextStateActiveOperationName);

                                    //lSecondParanthesis = (lThirdOperand AND lFourthOperand)
                                    BoolExpr lSecondParantesis = cZ3Solver.AndOperator(new List<BoolExpr>() { lThirdOperand, lFourthOperand });

                                    if (lFormulaSix == null)
                                        //lFormulaSix = !(lFirstParanthesis AND lSecondParanthesis)
                                        lFormulaSix = cZ3Solver.NotOperator(cZ3Solver.AndOperator(new List<BoolExpr>() { lFirstParantesis, lSecondParantesis }));
                                    else
                                        //lFormulaSix = lFormulaSix AND (!(lFirstParanthesis AND lSecondParanthesis))
                                        lFormulaSix = cZ3Solver.AndOperator(new List<BoolExpr>() { lFormulaSix, cZ3Solver.NotOperator(cZ3Solver.AndOperator(new List<BoolExpr>() { lFirstParantesis, lSecondParantesis })) });
                                }
                            }
                        }
                    }*/
                }
                if (lFormulaSix != null)
                {
                    if (cConvertOperationPrecedenceRules)
                        cZ3Solver.AddConstraintToSolver(lFormulaSix, "formula6");

                    if (cBuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lFormulaSix);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateFormula6");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in CreateFormula58");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        private void createFormula57(OperationInstance pOperationInstance)
        {
            try
            {
                //Formula 5.7
                //O_F_k_j => O_F_k_j+1
                OperationInstance lNextTransitionOperationInstance = getNextTransitionOperationInstance(pOperationInstance);
                if (lNextTransitionOperationInstance != null)
                {
                    BoolExpr lWholeFormula = cZ3Solver.ImpliesOperator(new List<string>() { pOperationInstance.FinishedVariableName
                                                                                    , lNextTransitionOperationInstance.FinishedVariableName });

                    if (cConvertOperationPrecedenceRules)
                        cZ3Solver.AddConstraintToSolver(lWholeFormula, "formula5.7");

                    if (cBuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lWholeFormula);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateFormula57");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private void createFormula56(OperationInstance pOperationInstance)
        {
            try
            {
                //Formula 5.6
                //O_U_k_j => O_U_k_j+1
                OperationInstance lNextTransitionOperationInstance = getNextTransitionOperationInstance(pOperationInstance);

                if (lNextTransitionOperationInstance != null)
                {
                    BoolExpr lWholeFormula = cZ3Solver.TwoWayImpliesOperator(pOperationInstance.UnusedVariableName
                                                                            , lNextTransitionOperationInstance.UnusedVariableName);

                    if (cConvertOperationPrecedenceRules)
                        cZ3Solver.AddConstraintToSolver(lWholeFormula, "formula5.6");

                    if (cBuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lWholeFormula);

                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateFormula56");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private void createFormula54(OperationInstance pOperationInstance)
        {
            try
            {
                //Formula 5.4
                //(O_E_k_j AND Post_k_j) => O_F_k_j+1

                // Optimized //TODO:Check this line, it might be the fact that this line is not needed as the post conditions are set outside this method in the mother method!!
                //resetOperationPostcondition(pOperation, pCurrentVariant, pState, "formula5.4");

                //Optimized:  BoolExpr lLeftHandSide = lZ3Solver.AndOperator(new List<BoolExpr>() { lOp_E_CurrentState, lOpPostcondition });
                BoolExpr lLeftHandSide = ReturnOperationExecutingStateNItsPostcondition(pOperationInstance);

                OperationInstance lNextTransitionOperationInstance = getNextTransitionOperationInstance(pOperationInstance);

                if (lNextTransitionOperationInstance != null)
                {
                    BoolExpr lWholeFormula = cZ3Solver.ImpliesOperator(new List<BoolExpr>() { lLeftHandSide
                                                                                        , (BoolExpr)cZ3Solver.FindExprInExprSet(lNextTransitionOperationInstance.FinishedVariableName) });

                    if (cConvertOperationPrecedenceRules)
                        cZ3Solver.AddConstraintToSolver(lWholeFormula, "formula5.4");

                    if (cBuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lWholeFormula);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateFormula54");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private void createFormula53(OperationInstance pOperationInstance)
        {
            try
            {
                //Fromula 5.3
                //for this XOR O_I_k_j O_E_k_j O_F_k_j O_U_k_j

                BoolExpr lWholeFormula = cZ3Solver.PickOneOperator(new string[] { pOperationInstance.InitialVariableName
                                                                                , pOperationInstance.ExecutingVariableName
                                                                                , pOperationInstance.FinishedVariableName
                                                                                , pOperationInstance.UnusedVariableName });

                if (cConvertOperationPrecedenceRules)
                    cZ3Solver.AddConstraintToSolver(lWholeFormula, "formula5.3");

                if (cBuildPConstraints)
                    AddPrecedanceConstraintToLocalList(lWholeFormula);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateFormula53");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }


        private void createFormula53GeneralVersion(OperationInstance pOperationInstance)
        {
            try
            {
                //Fromula 5.3
                //for this XOR O_I_k_j O_E_k_j O_F_k_j O_U_k_j

                cZ3Solver.AddPickOneOperator2Constraints(new List<string> { pOperationInstance.InitialVariableName
                                                                      , pOperationInstance.ExecutingVariableName
                                                                      , pOperationInstance.FinishedVariableName
                                                                      , pOperationInstance.UnusedVariableName }, "formula5.3");

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateFormula53");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private void createFormula52(OperationInstance pOperationInstance)
        {
            try
            {
                //Formula 5.2
                // not (O_I_k_j and Pre_k_j) => (O_I_k_j <=> O_I_k_j+1)
                BoolExpr tempLeftHandSideTwo;

                tempLeftHandSideTwo = ReturnOperationInitialStateNItsPrecondition(pOperationInstance);

                tempLeftHandSideTwo = cZ3Solver.NotOperator(tempLeftHandSideTwo);

                BoolExpr tempRightHandSideTwo;

                tempRightHandSideTwo = cZ3Solver.TwoWayImpliesOperator(pOperationInstance.InitialVariableName
                                                                        , pOperationInstance.InitialVariableName);

                BoolExpr lWholeFormula = cZ3Solver.ImpliesOperator(new List<BoolExpr>() { tempLeftHandSideTwo, tempRightHandSideTwo });

                if (cConvertOperationPrecedenceRules)
                    cZ3Solver.AddConstraintToSolver(lWholeFormula, "formula5.2");

                if (cBuildPConstraints)
                    AddPrecedanceConstraintToLocalList(lWholeFormula);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateFormula52");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        // This function takes one operation instance and returns the operation instance of the next transition if there is one
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        private OperationInstance getNextTransitionOperationInstance(OperationInstance pOperationInstance)
        {
            OperationInstance lOperationInstance = null;
            try
            {
                int lCurrentOperationInstanceTransitionNumber = pOperationInstance.TransitionNumber;

                lOperationInstance = cFrameworkWrapper.operationInstanceLookup(pOperationInstance.AbstractOperation.Name
                                                                            , lCurrentOperationInstanceTransitionNumber + 1);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in getNextTransitionOperationInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lOperationInstance;
        }

        private void createFormula51(OperationInstance pOperationInstance)
        {
            try
            {
                //Formula 5.1
                //(O_I_k_j and Pre_k_j) => O_E_k_j+1
                BoolExpr tempLeftHandSideOne;
                //if (lOperation.precondition != null)

                //(O_I_k_j and Pre_k_j)
                //Not optimized! tempLeftHandSideOne = lZ3Solver.AndOperator(new List<BoolExpr>() { lOp_I_CurrentState, lOpPrecondition });
                tempLeftHandSideOne = ReturnOperationInitialStateNItsPrecondition(pOperationInstance);
                //else
                //    tempLeftHandSideOne = lOp_I_CurrentState;

                //(O_I_k_j and Pre_k_j) => O_E_k_j+1
                OperationInstance lNextTransitionOperationInstance = getNextTransitionOperationInstance(pOperationInstance);
                
                if (lNextTransitionOperationInstance != null)
                {
                    //In case the current transition number is the last transition then the next transition number will be NULL!
                    BoolExpr lWholeFormula = cZ3Solver.ImpliesOperator(new List<BoolExpr>() { tempLeftHandSideOne
                                                                                            , (BoolExpr)cZ3Solver.FindExprInExprSet(lNextTransitionOperationInstance.ExecutingVariableName) });

                    if (cConvertOperationPrecedenceRules)
                        cZ3Solver.AddConstraintToSolver(lWholeFormula, "formula5.1");

                    if (cBuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lWholeFormula);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CreateFormula51");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public BoolExpr ReturnOperationExecutingStateNItsPostcondition(OperationInstance pOperationInstance)
        {
            BoolExpr result;
            try
            {
                //If the operation has no precondition then the relevant variable is forced to be true, other wise it will be the precondition
                /*if (pOperationInstance.AbstractOperation.Postcondition != null)
                {
                    if (pOperation.postcondition.Count != 0)
                        result = cZ3Solver.AndOperator(new HashSet<BoolExpr>() { cOp_E_CurrentState, cOpPostcondition });
                    else
                        result = cOp_E_CurrentState;
                }
                else*/
                result = (BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance.ExecutingVariableName);

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnOperationExecutingStateNItsPostcondition");
                cOutputHandler.printMessageToConsole(ex.Message);
                result = null;
            }
            return result;
        }

        public BoolExpr ReturnOperationInitialStateNItsPrecondition(OperationInstance pOperationInstance)
        {
            BoolExpr result;
            try
            {
                //If the operation has no precondition then the relevant variable is forced to be true, other wise it will be the precondition
                if (pOperationInstance.AbstractOperation.Precondition != null)
                {
                    if (pOperationInstance.AbstractOperation.Precondition.Count != 0)
                        result = cZ3Solver.AndOperator(new List<string> { pOperationInstance.InitialVariableName
                                                                    , pOperationInstance.OperationPreconditionVariableName });
                    else
                        result = (BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance.InitialVariableName);
                }
                else
                    result = (BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance.InitialVariableName);

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnOperationInitialStateNItsPrecondition");
                cOutputHandler.printMessageToConsole(ex.Message);
                result = null;
            }
            return result;
        }

        public BoolExpr FindingADatasBooleanVariable(string pData)
        {
            BoolExpr lResultVariable = null;
            try
            {

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in FindingADatasBooleanVariable");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultVariable;
        }

        //TODO: We need enum for operators to be used here
        public BoolExpr ParseComplexString(Node<string> pNode, int pTransitionNumber)
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
                    //This operand can be an operation or it can be a variant or it can be a part, if it is an operation then it has to be checked if it is in a complete operation instance format or not
                    if (cFrameworkWrapper.havePartWithName(pNode.Data) || cFrameworkWrapper.haveVariantWithName(pNode.Data))
                        lResult = (BoolExpr)cZ3Solver.FindExprInExprSet(pNode.Data);
                    else
                        lResult = convertIncompleteOperationInstances2CompleteOperationInstanceExpr(pNode.Data, pTransitionNumber);
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
                                lResult = cZ3Solver.AndOperator(new List<BoolExpr>() { ParseComplexString(lChildren[0], pTransitionNumber)
                                                                                    , ParseComplexString(lChildren[1], pTransitionNumber) });
                                break;
                            }
                        case "or":
                            {
                                lResult = cZ3Solver.OrOperator(new List<BoolExpr>() { ParseComplexString(lChildren[0], pTransitionNumber)
                                                                                    , ParseComplexString(lChildren[1], pTransitionNumber) });
                                break;
                            }
                        case "->":
                            {
                                lResult = cZ3Solver.ImpliesOperator(new List<BoolExpr>() { ParseComplexString(lChildren[0], pTransitionNumber)
                                                                                            , ParseComplexString(lChildren[1], pTransitionNumber) });
                                break;
                            }
                        case "not":
                            {
                                lResult = cZ3Solver.NotOperator(ParseComplexString(lChildren[0], pTransitionNumber));
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
                cOutputHandler.printMessageToConsole("error in ParseExpression");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                    lResult = (BoolExpr)cZ3Solver.FindExprInExprSet(pNode.Data);
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
                                lResult = cZ3Solver.AndOperator(new List<BoolExpr>() { ParseExpression(lChildren[0])
                                                                                    , ParseExpression(lChildren[1]) });
                                break;
                            }
                        case "or":
                            {
                                lResult = cZ3Solver.OrOperator(new List<BoolExpr>() { ParseExpression(lChildren[0])
                                                                                    , ParseExpression(lChildren[1]) });
                                break;
                            }
                        case "not":
                            {
                                lResult = cZ3Solver.NotOperator(ParseExpression(lChildren[0]));
                                break;
                            }
                        case ">=":
                            {
                                lResult = cZ3Solver.GreaterOrEqualOperator(lChildren[0].Data
                                                                            , int.Parse(lChildren[1].Data));
                                break;
                            }
                        case "<=":
                            {
                                lResult = cZ3Solver.LessOrEqualOperator(lChildren[0].Data
                                                                        , int.Parse(lChildren[1].Data));
                                break;
                            }
                        case "<":
                            {
                                lResult = cZ3Solver.LessThanOperator(lChildren[0].Data
                                                                    , int.Parse(lChildren[1].Data));
                                break;
                            }
                        case ">":
                            {
                                lResult = cZ3Solver.GreaterThanOperator(lChildren[0].Data
                                                                        , int.Parse(lChildren[1].Data));
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ParseExpression");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResult;
        }

        public string returnConstraintsString()
        {
            string lConstraintsString = "";
            try
            {
                //First we have to loop the constraint list
                //List<string> localConstraintList = cFrameworkWrapper.ConstraintList;

                foreach (string lConstraint in cFrameworkWrapper.ConstraintSet)
                {
                    if (lConstraintsString != "")
                        lConstraintsString += " AND ";
                    lConstraintsString += lConstraint;

                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in returnConstraintsString");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                HashSet<string> localConstraintList = cFrameworkWrapper.ConstraintSet;

                ////foreach (string lConstraint in localConstraintList)

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in returnBoolExprOfConstraintsSet");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in returnBoolExprOfConstraint");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in addStandAloneConstraint2Z3Solver");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cZ3Solver.AddConstraintToSolver(convertComplexString2BoolExpr(pExtraConfigurationRule)
                                                , "formula3-extra configuration rule");

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AddExtraConstraint2Z3Constraint");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                //List<string> localConstraintList = cFrameworkWrapper.ConstraintList;

                foreach (string lConstraint in cFrameworkWrapper.ConstraintSet)
                {
                    BoolExpr lBoolExprConstraint = convertComplexString2BoolExpr(lConstraint);
                    cZ3Solver.AddConstraintToSolver(lBoolExprConstraint
                                                    , "formula3");
                    if (cAnalysisType == Enumerations.AnalysisType.AlwaysSelectedVariantAnalysis 
                        || cAnalysisType == Enumerations.AnalysisType.AlwaysSelectedOperationAnalysis)
                        AddConfigurationConstraintToLocalList(lBoolExprConstraint);

                }

                //TODO: by default this extra configuration rule can be an array
                if (pExtraConfigurationRule != "")
                    cZ3Solver.AddConstraintToSolver(convertComplexString2BoolExpr(pExtraConfigurationRule)
                                                    , "formula3-ExtraConfigRule");

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertFConstraint2Z3Constraint");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in AddConfigurationConstraintToLocalList");
                cOutputHandler.printMessageToConsole(ex.Message);
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

                //lExpressionParser.AddChild(lExprTree, pExpression);
                Tuple<Node<string>, string> lReturnedResult = lExpressionParser.MakeChildNode(lExprTree, pExpression);
                Node<string> lChildNode = lReturnedResult.Item1;
                lExprTree.AddChildNode(lChildNode);

                foreach (Node<string> item in lExprTree)
                {
                    //Then we have to traverse the tree and call the appropriate Z3Solver functionalities
                    lResultExpr = ParseExpression(item);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in addFExpression2Z3Constraint");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }

        private BoolExpr convertComplexString2BoolExpr(string pExpression, int pTransitionNumber = -1)
        {
            BoolExpr lResultExpr = null;
            try
            {
                //For each expression first we have to build its coresponding tree
                Parser lExpressionParser = new Parser();
                Node<string> lExprTree = new Node<string>("root");

                //lExpressionParser.AddChild(lExprTree, pExpression);
                Tuple<Node<string>, string> lReturnedResult = lExpressionParser.MakeChildNode(lExprTree, pExpression);
                Node<string> lChildNode = lReturnedResult.Item1;
                lExprTree.AddChildNode(lChildNode);

                foreach (Node<string> item in lExprTree)
                {
                    //Then we have to traverse the tree and call the appropriate Z3Solver functionalities
                    lResultExpr = ParseComplexString(item, pTransitionNumber);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertComplexString2BoolExpr");
                cOutputHandler.printMessageToConsole(ex.Message);
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

        //This function is no longer needed as the input to this function should be operation instances
/*        public BoolExpr createFormula7(HashSet<part> pPartList, int pState)
        {
            BoolExpr lResultFormula7 = null;
            try
            {
                //NEW formula 7
                //No operation can proceed
                //(Big And) ((! Pre_k_j AND O_I_k_j) OR (! Post_k_j AND O_E_k_j) OR O_F_k_j OR O_U_k_j)

                //This boolean expression is used to refer to this overall goal
                cZ3Solver.AddBooleanExpression("F7_" + pState);

                foreach (part lCurrentPart in pPartList)
                {
                    HashSet<operation> lOperationList = cFrameworkWrapper.getPartExprOperations(lCurrentPart.names);
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
                             
                            resetCurrentStateOperationVariables(lCurrentOperation, lCurrentPart, pState);

                            BoolExpr lNotPreCondition = cZ3Solver.NotOperator(cOperationInstance_CurrentState.AbstractOperation.Precondition);
                            BoolExpr lNotPostCondition = cZ3Solver.NotOperator(cOperationInstance_CurrentState.AbstractOperation.Precondition);

                            BoolExpr lFirstOperand = cZ3Solver.AndOperator(new List<BoolExpr> { lNotPreCondition
                                                                                            , (BoolExpr)cZ3Solver.FindExprInExprSet(cOperationInstance_CurrentState.InitialVariableName) });
                            BoolExpr lSecondOperand = cZ3Solver.AndOperator(new List<BoolExpr> { lNotPostCondition
                                                                                            , (BoolExpr)cZ3Solver.FindExprInExprSet(cOperationInstance_CurrentState.ExecutingVariableName) });

                            BoolExpr lOperand = cZ3Solver.OrOperator(new List<BoolExpr> { lFirstOperand
                                                                                    , lSecondOperand
                                                                                    , (BoolExpr)cZ3Solver.FindExprInExprSet(cOperationInstance_CurrentState.FinishedVariableName)
                                                                                    , (BoolExpr)cZ3Solver.FindExprInExprSet(cOperationInstance_CurrentState.UnusedVariableName) });

                            if (lResultFormula7 == null)
                                lResultFormula7 = lOperand;
                            else
                                lResultFormula7 = cZ3Solver.AndOperator(new List<BoolExpr> { lResultFormula7, lOperand });
                        }
                    }
                }
                if (lResultFormula7 != null)
                    //lZ3Solver.AddConstraintToSolver(lOverallGoal, "overallGoal");
                    cZ3Solver.AddImpliesOperator2Constraints((BoolExpr)cZ3Solver.FindExprInExprSet("F7_" + pState)
                                                                , lResultFormula7
                                                                , "Formula7");
                //            if (formula7 != null)
                //                lZ3Solver.AddConstraintToSolver(formula7);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createFormula7");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultFormula7;
        }*/

        public BoolExpr convertOperationPreconditions2BoolExpr(List<string> pOperationPreconditionList)
        {
            BoolExpr lResultExpr = null;
            try
            {
                foreach (var lOperationPrecondition in pOperationPreconditionList)
                {
                    BoolExpr lTempBoolEpr = null;



                    if (lResultExpr != null)
                        lResultExpr = (BoolExpr)cZ3Solver.AndOperator(new List<BoolExpr>() { lResultExpr, lTempBoolEpr });
                    else
                        lResultExpr = lTempBoolEpr;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertOperationPreconditions2BoolExpr");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }

        public BoolExpr createFormula7(HashSet<OperationInstance> pOperationInstanceList, int pState)
        {
            BoolExpr lResultFormula7 = null;
            try
            {
                //NEW formula 7
                //No operation can proceed
                //(Big And) ((! Pre_k_pState AND O_I_k_pState) OR (! Post_k_pState AND O_E_k_pState) OR O_F_k_pState OR O_U_k_pState)

                //This boolean expression is used to refer to this overall goal
                cZ3Solver.AddBooleanExpression("F7_" + pState);

                //This formula should be carried out for each created operation instance

                foreach (OperationInstance lCurrentOperationInstance in pOperationInstanceList)
                {
                    if (lCurrentOperationInstance.TransitionNumber.Equals(pState))
                    {
                        BoolExpr lNotPreCondition = cZ3Solver.NotOperator(lCurrentOperationInstance.OperationPreconditionVariableName);
                        //BoolExpr lNotPostCondition = cZ3Solver.NotOperator(cOpPostcondition);

                        BoolExpr lFirstOperand = cZ3Solver.AndOperator(new List<BoolExpr>() { lNotPreCondition
                                                                                    , (BoolExpr)cZ3Solver.FindExprInExprSet(lCurrentOperationInstance.InitialVariableName) });
                        //BoolExpr lSecondOperand = cZ3Solver.AndOperator(new HashSet<BoolExpr>() { lNotPostCondition, cOp_E_CurrentState });
                        BoolExpr lSecondOperand = (BoolExpr)cZ3Solver.FindExprInExprSet(lCurrentOperationInstance.ExecutingVariableName);

                        BoolExpr lOperand = cZ3Solver.OrOperator(new List<BoolExpr>() { lFirstOperand
                                                                            , lSecondOperand
                                                                            , (BoolExpr)cZ3Solver.FindExprInExprSet(lCurrentOperationInstance.FinishedVariableName)
                                                                            , (BoolExpr)cZ3Solver.FindExprInExprSet(lCurrentOperationInstance.UnusedVariableName) });

                        if (lResultFormula7 == null)
                            lResultFormula7 = lOperand;
                        else
                            lResultFormula7 = cZ3Solver.AndOperator(new List<BoolExpr>() { lResultFormula7, lOperand });
                    }
                }
                if (lResultFormula7 != null)
                    //lZ3Solver.AddConstraintToSolver(lOverallGoal, "overallGoal");
                    cZ3Solver.AddImpliesOperator2Constraints((BoolExpr)cZ3Solver.FindExprInExprSet("F7_" + pState)
                                                                , lResultFormula7
                                                                , "Formula7");
                //            if (formula7 != null)
                //                lZ3Solver.AddConstraintToSolver(formula7);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createFormula7");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultFormula7;
        }

        public BoolExpr createFormula8(HashSet<OperationInstance> pOperationInstanceList, int pState)
        {
            BoolExpr lResultFormula8 = null;
            try
            {
                //formula 8
                //At least one operation is in initial or executing state
                //(Big OR) (O_I_k_j OR O_E_k_j)

                //This boolean expression is used to refer to this formula 8
                cZ3Solver.AddBooleanExpression("F8_" + pState);

                foreach (OperationInstance lCurrentOperationInstance in pOperationInstanceList)
                {
                    BoolExpr lOperand = cZ3Solver.OrOperator(new List<string>() { lCurrentOperationInstance.InitialVariableName
                                                                            , lCurrentOperationInstance.ExecutingVariableName });

                    if (lResultFormula8 == null)
                        lResultFormula8 = lOperand;
                    else
                        lResultFormula8 = cZ3Solver.OrOperator(new List<BoolExpr>() { lResultFormula8, lOperand });
                }
                if (lResultFormula8 != null)
                    //lZ3Solver.AddConstraintToSolver(lOverallGoal, "overallGoal");
                    cZ3Solver.AddImpliesOperator2Constraints((BoolExpr)cZ3Solver.FindExprInExprSet("F8_" + pState)
                                                                , lResultFormula8
                                                                , "Formula8");
                //            if (formula8 != null)
                //                lZ3Solver.AddConstraintToSolver(formula8);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createFormula8");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultFormula8;
        }

        /// <summary>
        /// This analysis checks if there are any Deadlock
        /// </summary>
        public bool ExistanceOfDeadlockAnalysis(bool pVariationPointsSet
                                                , bool pReportTypeSet)
        {
            var lStopWatch = new Stopwatch();

            bool lAnalysisResult = false;

            //This is the result of the internal analysis we get
            Status lInternalAnalysisResult = Status.UNKNOWN;
            try
            {
                if (cReportTimings)
                    lStopWatch.Start();

                //TODO: Only should be done when a flag is set
                cOutputHandler.printMessageToConsole("Existance Of Valid Production Path Analysis:");

                bool lAnalysisComplete = false;

                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                cMaxNumberOfTransitions = CalculateAnalysisNoOfCycles();

                cZ3Solver.PrepareDebugDirectory();

                if (!pVariationPointsSet)
                    setVariationPoints(Enumerations.GeneralAnalysisType.Dynamic
                                        , Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis);

                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                //          , Transitions Result, Analysis Timing, Unsat Core
                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                //          , Report timings, Debug Mode (Make model file), User Messages
                if (!pReportTypeSet)
                    setReportType(true, true, true
                                , true, false, false
                                , false, true, false
                                , true, true, true);

                ////Making the static part of the model
                MakeStaticPartOfProductPlatformModel();

                for (int lTransitionNo = 0; lTransitionNo < cMaxNumberOfTransitions; lTransitionNo++)
                {
                    cCurrentTransitionNumber = lTransitionNo;

                    cOutputHandler.printMessageToConsole("--------------------Transition: " + lTransitionNo + " --------------------");

                    lAnalysisComplete = false;

                    ////Making the dynamic part of the model (Operations and transition between operation status)
                    MakeDynamicPartOfProductPlatformModel(lAnalysisComplete);

                    //Now the goal is added to the model according to the type of analysis
                    //Variation Point
                    if (cConvertGoal && !lAnalysisComplete)
                        convertExistenceOfDeadlockGoal(lTransitionNo);

                    if (cReportTimings)
                    {
                        lStopWatch.Stop();
                        cModelCreationTime = lStopWatch.ElapsedMilliseconds;

                        cOutputHandler.printMessageToConsole("Model Creation Time: " + cModelCreationTime + "ms.");

                        lStopWatch.Restart();
                    }

                    //For each variant check if this statement holds - carry out the analysis
                    lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                            , 0
                                                            , lAnalysisComplete);

                    if (cReportTimings)
                    {
                        lStopWatch.Stop();
                        cModelAnalysisTime = lStopWatch.ElapsedMilliseconds;

                        cOutputHandler.printMessageToConsole("Model Analysis Time: " + cModelAnalysisTime + "ms.");

                        lStopWatch.Restart();
                    }

                    //if the result of the previous analysis is true then we go to the next analysis part

                    if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    {
                        //If the result is true it means we have found a deadlock!
                        cOutputHandler.printMessageToConsole("A deadlock was found!");
                        ReportSolverResult(lTransitionNo + 1, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, null);
                        break;
                    }

                    //TODO: Is this the best way to end the loop on the transiton numbers???????
                    //If all the transition cycles are completed then the analysis is completed
                    if (lTransitionNo == cMaxNumberOfTransitions - 1)
                        lAnalysisComplete = true;
                }

                //Translating the internal analysis result to the user specific analysis result 
                if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    lAnalysisResult = true;
                else
                    lAnalysisResult = false;

                cOutputHandler.printMessageToConsole("Analysis Report: ");

                if (!lAnalysisResult)
                {
                    //If the result is false it means no deadlock can be found! HENCE DEADLOCK FREE!!
                    
                    //If t is deadlock free then there is nothing to report as there will be no model given!!
                    //ReportSolverResult(lMaxNoOfTransitions, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, null);

                    cOutputHandler.printMessageToConsole("NO deadlock was found!");
                }

                if (cReportTimings)
                {
                    lStopWatch.Stop();
                    cModelAnalysisReportingTime = lStopWatch.ElapsedMilliseconds;
                    cOutputHandler.printMessageToConsole("Model Analysis Reporting Time: " + cModelAnalysisReportingTime + "ms.");
                }


            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ExistanceOfDeadlockAnalysis");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis checks for possible models of product manufacturing
        /// </summary>
        public bool ProductManufacturingEnumerationAnalysis(bool pVariationPointsSet
                                                            , bool pReportTypeSet)
        {
            var lStopWatch = new Stopwatch();
            //This is the result of the analysis we give the user
            bool lAnalysisResult = false;

            //This is the result of the internal analysis we get
            Status lInternalAnalysisResult = Status.UNKNOWN;
            try
            {
                if (cReportTimings)
                    lStopWatch.Start();

                //TODO: Only should be done when a flag is set
                cOutputHandler.printMessageToConsole("Product Manufacturing Enumeration Analysis:");

                //This variable controls if the analysis has been completed or not
                bool lAnalysisComplete = false;

                //This is the list of all the variants in the product platform
                //HashSet<part> lVariantList = cFrameworkWrapper.PartSet;

                ////TODO: Added as new version
                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                cMaxNumberOfTransitions = CalculateAnalysisNoOfCycles();

                ////TODO: Added as new version
                cZ3Solver.PrepareDebugDirectory();

                if (!pVariationPointsSet)
                    setVariationPoints(Enumerations.GeneralAnalysisType.Dynamic
                                        , Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis);

                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                //          , Transitions Result, Analysis Timing, Unsat Core
                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                //          , Report timings, Debug Mode (Make model file), User Messages
                if (!pReportTypeSet)
                    setReportType(true, true, true
                                , true, false, false
                                , false, true, false
                                , true, true, true);

                ////Making the static part of the model
                MakeStaticPartOfProductPlatformModel();

                ////int lTransitionNo = lMaxNoOfTransitions - 1;
                for (int lTransitionNo = 0; lTransitionNo < cMaxNumberOfTransitions; lTransitionNo++)
                {
                    cCurrentTransitionNumber = lTransitionNo;
                    ////Making the dynamic part of the model (Operations and transition between operation status)
                    MakeDynamicPartOfProductPlatformModel(lAnalysisComplete);

                }
                //Now the goal is added to the model
                //Variation Point
                if (cConvertGoal)
                    addFindingModelGoal();

                if (cReportTimings)
                {
                    lStopWatch.Stop();
                    cModelCreationTime = lStopWatch.ElapsedMilliseconds;

                    cOutputHandler.printMessageToConsole("Model Creation Time: " + cModelCreationTime + "ms.");

                    lStopWatch.Restart();
                }

                //For each variant check if this statement holds - carry out the analysis
                lInternalAnalysisResult = AnalyzeProductPlatform(cMaxNumberOfTransitions - 1
                                                        , 0
                                                        , lAnalysisComplete);

                lAnalysisComplete = true;

                if (cReportTimings)
                {
                    lStopWatch.Stop();
                    cModelAnalysisTime = lStopWatch.ElapsedMilliseconds;

                    cOutputHandler.printMessageToConsole("Model Analysis Time: " + cModelAnalysisTime + "ms.");

                    lStopWatch.Restart();
                }

                //if the result of the previous analysis is true then we go to the next analysis part
                if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    //If the result is true it means we have found a model.
                    ReportSolverResult(cMaxNumberOfTransitions - 1, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, null);

                if (cReportTimings)
                {
                    lStopWatch.Stop();
                    cModelAnalysisReportingTime = lStopWatch.ElapsedMilliseconds;
                    cOutputHandler.printMessageToConsole("Model Analysis Reporting Time: " + cModelAnalysisReportingTime + "ms.");
                }

                //TODO: Do we need a stand alone constraint????????? Considering that this line comes with a stand alone constraint!!!
                //                        lZ3Solver.SolverPopFunction();

                //TODO: Is this the best way to end the loop on the transiton numbers???????
                //If all the transition cycles are completed then the analysis is completed
                //                }

                //Translating the internal analysis result to the user specific analysis result 
                if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    lAnalysisResult = true;
                else
                    lAnalysisResult = false;

                cOutputHandler.printMessageToConsole("Analysis Report: ");

                if (!lAnalysisResult)
                {
                    //If the result is false it means no deadlock can be found! HENCE DEADLOCK FREE!!
                    ReportSolverResult(cMaxNumberOfTransitions, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, null);
                    cOutputHandler.printMessageToConsole("NO deadlock was found!");
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ExistanceOfDeadlockAnalysis");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            //Returns the result of the analysis
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis finds models that satisfy the set of formulas
        /// </summary>
        public bool ProductModelEnumerationAnalysis(bool pVariationPointsSet
                                                    , bool pReportTypeSet)
        {
            bool lAnalysisResult = false;

            Status lInternalAnalysisResult = Status.UNKNOWN;
            try
            {
                cOutputHandler.printMessageToConsole("Model enumeration Analysis:");

                //This variable controls if the analysis has been completed or not
                bool lAnalysisComplete = false;

                cZ3Solver.PrepareDebugDirectory();

                //This analysis is going to be carried out in two steps:
                //Step A, check the product platform structurally without the goal
                //        this check is done for each seperate variant from the variant group
                //        this checks to see if any of the rules are conflicting each other
                if (!pVariationPointsSet)
                    setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                        , Enumerations.AnalysisType.ProductModelEnumerationAnalysis);

                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                //          , Transitions Result, Analysis Timing, Unsat Core
                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                //          , Report timings, Debug Mode (Make model file), User Messages
                if (!pReportTypeSet)
                    setReportType(false, false, true
                                , true, false, false
                                , false, true, false
                                , true, true, true);

                MakeStaticPartOfProductPlatformModel();

                for (int i = 0; i < cNoOfModelsRequired; i++)
                {
                    cOutputHandler.ResetOutputResult();
                    //For each variant check if this statement holds - carry out the analysis
                    lInternalAnalysisResult = AnalyzeProductPlatform(0
                                                            , i
                                                            , lAnalysisComplete);

                    //if the result of the previous analysis is true then we go to the next analysis part
                    if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    {
                        //??addStandAloneConstraint2Z3Solver(lZ3Solver.FindBoolExpressionUsingName(lCurrentVariant.names));
                        ReportSolverResult(i
                                        , lAnalysisComplete
                                        , cFrameworkWrapper
                                        , lInternalAnalysisResult
                                        , null);
                    }
                    else if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                    {
                        //If the product platform did not have any more models!
                        if (i > 0)
                            cOutputHandler.printMessageToConsole("This Product Platform only had " + i + " Models.");
                        break;
                    }

                }

                //Translating the internal analysis result to the user specific analysis result 
                if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    lAnalysisResult = true;
                else
                    lAnalysisResult = false;


                //In this analysis there was not a meaningful analysis report!
                //cOutputHandler.printMessageToConsole("Analysis Report: ");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ProductModelEnumerationAnalysis");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("Variant Selectability Analysis:");

                //This variable controls if the analysis has been completed or not
                bool lAnalysisComplete = false;


                //This is the list of all the variants/parts in the product platform

                HashSet<part> lPartList = cFrameworkWrapper.PartSet;
                //This is an empty list which is going to be filled by all he variants which are not able to be picked
                List<part> lUnselectablePartList = new List<part>();
                //------------------OR------------------------------
                HashSet<variant> lVariantList = cFrameworkWrapper.VariantSet;
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

                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                //          , Transitions Result, Analysis Timing, Unsat Core
                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                //          , Report timings, Debug Mode (Make model file), User Messages
                if (!pReportTypeSet)
                    setReportType(false, false, true
                                , true, false, false
                                , false, true, false
                                , true, true, true);

                ////As this analysis type is a static analysis we only carry t out for the first transition
                //TODO: lTransitionNo should be removed!!
                int lTransitionNo = 0;
                cCurrentTransitionNumber = lTransitionNo;

                if (cFrameworkWrapper.UsePartInfo)
                {
                    //Then we have to go over all variants one by one, for this we use the list we previously filled
                    foreach (part lCurrentPart in lPartList)
                    {
                        //This line is moved to the reporting procedure in this class.
                        //cOutputHandler.printMessageToConsole("---------------------------------------------------------------------");
                        //cOutputHandler.printMessageToConsole("Selected Variant: " + lCurrentVariant.names);


                        ////TODO: Added as new version
                        MakeStaticPartOfProductPlatformModel();

                        /////TODO: Removed in the new version
                        /////for (int lTransitionNo = 0; lTransitionNo < lMaxNoOfTransitions; lTransitionNo++)
                        /////{

                        /////                        cOutputHandler.printMessageToConsole("--------------------Transition: " + lTransitionNo + " --------------------");
                        //For this new variant the analysis has just started, hence it is not complete
                        /////                        lAnalysisComplete = false;

                        ////TODO: Added as new version
                        MakeDynamicPartOfProductPlatformModel(lAnalysisComplete);

                        cZ3Solver.SolverPushFunction();

                        addStandAloneConstraint2Z3Solver((BoolExpr)cZ3Solver.FindExprInExprSet(lCurrentPart.names));

                        //For each variant check if this statement holds - carry out the analysis
                        lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                                , 0
                                                                , lAnalysisComplete);

                        //if the result of the previous analysis is true then we go to the next analysis part
                        if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                        {
                            ReportSolverResult(lTransitionNo, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, lCurrentPart);

                            //This line is moved to the reporting procedure in this class.
                            //cOutputHandler.printMessageToConsole(lCurrentVariant.names + " is Selectable.");
                        }
                        else if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                        {
                            //If the result of the first analysis was false that means the selected variant is in conflict with the rest of the product platform
                            if (!lUnselectablePartList.Contains(lCurrentPart))
                                lUnselectablePartList.Add(lCurrentPart);
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

                }
                else
                {
                    //Then we have to go over all variants one by one, for this we use the list we previously filled
                    foreach (variant lCurrentVariant in lVariantList)
                    {
                        //This line is moved to the reporting procedure in this class.
                        //cOutputHandler.printMessageToConsole("---------------------------------------------------------------------");
                        //cOutputHandler.printMessageToConsole("Selected Variant: " + lCurrentVariant.names);


                        ////TODO: Added as new version
                        MakeStaticPartOfProductPlatformModel();

                        /////TODO: Removed in the new version
                        /////for (int lTransitionNo = 0; lTransitionNo < lMaxNoOfTransitions; lTransitionNo++)
                        /////{

                        /////                        cOutputHandler.printMessageToConsole("--------------------Transition: " + lTransitionNo + " --------------------");
                        //For this new variant the analysis has just started, hence it is not complete
                        /////                        lAnalysisComplete = false;

                        ////TODO: Added as new version
                        MakeDynamicPartOfProductPlatformModel(lAnalysisComplete);

                        cZ3Solver.SolverPushFunction();

                        addStandAloneConstraint2Z3Solver((BoolExpr)cZ3Solver.FindExprInExprSet(lCurrentVariant.names));

                        //For each variant check if this statement holds - carry out the analysis
                        lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                                , 0
                                                                , lAnalysisComplete);

                        //if the result of the previous analysis is true then we go to the next analysis part
                        if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                        {
                            ReportSolverResult(lTransitionNo, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, lCurrentVariant);

                            //This line is moved to the reporting procedure in this class.
                            //cOutputHandler.printMessageToConsole(lCurrentVariant.names + " is Selectable.");
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

                }

                cOutputHandler.printMessageToConsole("Analysis Report: ");
                if (cFrameworkWrapper.UsePartInfo)
                {
                    if (lUnselectablePartList.Count != 0)
                    {
                        cOutputHandler.printMessageToConsole("Parts which are not selectable are: " + ReturnPartNamesFromList(lUnselectablePartList));
                        lAnalysisResult = false;
                    }
                    else
                    {
                        cOutputHandler.printMessageToConsole("All Parts are selectable!");
                        lAnalysisResult = true;
                    }
                }
                else
                {
                    if (lUnselectableVariantList.Count != 0)
                    {
                        cOutputHandler.printMessageToConsole("Variats which are not selectable are: " + ReturnVariantNamesFromList(lUnselectableVariantList));
                        lAnalysisResult = false;
                    }
                    else
                    {
                        cOutputHandler.printMessageToConsole("All Variats are selectable!");
                        lAnalysisResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in VariantSelectabilityGoal");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("Always selected variant Analysis:");

                bool lAnalysisComplete = false;

                HashSet<part> lPartList = cFrameworkWrapper.PartSet;
                List<part> lNotAlwaysSelectedPartList = new List<part>();
                //-------------------------OR-------------------------------
                HashSet<variant> lVariantList = cFrameworkWrapper.VariantSet;
                List<variant> lNotAlwaysSelectedVariantList = new List<variant>();

                ////TODO: Added as new version
                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                cMaxNumberOfTransitions = CalculateAnalysisNoOfCycles();

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

                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                //          , Transitions Result, Analysis Timing, Unsat Core
                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                //          , Report timings, Debug Mode (Make model file), User Messages
                if (!pReportTypeSet)
                    setReportType(true, true, true
                                , true, false, false
                                , false, true, false
                                , true, true, true);

                //This analysis only has meaning in the first transition, as it is a static analysis
                //TODO: lTransitionNo should be removed !!
                cCurrentTransitionNumber = 0;

                //This line is moved to the reporting procedure in this class
                //cOutputHandler.printMessageToConsole("---------------------------------------------------------------------");
                //cOutputHandler.printMessageToConsole("Selected Variant: " + lCurrentVariant.names);

                ////TODO: Added as new version
                lAnalysisComplete = false;

                ////TODO: Added as new version
                MakeStaticPartOfProductPlatformModel();

                /////                    for (int lTransitionNo = 0; lTransitionNo < lMaxNoOfTransitions; lTransitionNo++)
                /////                    {
                /////                        cOutputHandler.printMessageToConsole("--------------------Transition: " + lTransitionNo + " --------------------");

                ////TODO: Added as new version
                MakeDynamicPartOfProductPlatformModel(lAnalysisComplete);

                if (cFrameworkWrapper.UsePartInfo)
                {
                    //Then we have to go over all variants one by one
                    foreach (part lCurrentPart in lPartList)
                    {

                        cZ3Solver.SolverPushFunction();

                        BoolExpr lStandAloneConstraint = null;
                        /////                        //Here we have to build an expression which shows C and P => V_i and assign it to the variable just defined
                        //Here we have to build an expression which shows C => ! V_i and assign it to the variable just defined
                        BoolExpr lRightHandSide = null;
                        BoolExpr lLeftHandSide = null;

                        /////                        ////TODO: only do this if the two list are not empty!
                        /////                        BoolExpr lP = lZ3Solver.AndOperator(lPConstraints);

                        lLeftHandSide = cZ3Solver.NotOperator((BoolExpr)cZ3Solver.FindExprInExprSet(lCurrentPart.names));

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
                        lInternalAnalysisResult = AnalyzeProductPlatform(cCurrentTransitionNumber
                                                                , 0
                                                                , lAnalysisComplete);

                        /////                        if (!lAnalysisResult && !lAnalysisComplete)
                        if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                        {
                            //This line is moved to the reporting procedure in this class
                            //if it does hold, then there is a configuration which is valid and this current variant is not present in it
                            //cOutputHandler.printMessageToConsole("There DOES exist a valid configuration which does not include " + lCurrentVariant.names + ".");

                            if (!lNotAlwaysSelectedPartList.Contains(lCurrentPart))
                                lNotAlwaysSelectedPartList.Add(lCurrentPart);
                            /////                            break;
                        }
                        else if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                        {
                            //This line is moved to the reporting procedure in this class
                            //cOutputHandler.printMessageToConsole("All valid configurations DO include " + lCurrentVariant.names + ".");
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

                }
                else
                {
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

                        lLeftHandSide = cZ3Solver.NotOperator((BoolExpr)cZ3Solver.FindExprInExprSet(lCurrentVariant.names));

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
                        lInternalAnalysisResult = AnalyzeProductPlatform(cCurrentTransitionNumber
                                                                , 0
                                                                , lAnalysisComplete);

                        /////                        if (!lAnalysisResult && !lAnalysisComplete)
                        if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                        {
                            //This line is moved to the reporting procedure in this class
                            //if it does hold, then there is a configuration which is valid and this current variant is not present in it
                            //cOutputHandler.printMessageToConsole("There DOES exist a valid configuration which does not include " + lCurrentVariant.names + ".");

                            if (!lNotAlwaysSelectedVariantList.Contains(lCurrentVariant))
                                lNotAlwaysSelectedVariantList.Add(lCurrentVariant);
                            /////                            break;
                        }
                        else if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                        {
                            //This line is moved to the reporting procedure in this class
                            //cOutputHandler.printMessageToConsole("All valid configurations DO include " + lCurrentVariant.names + ".");
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

                }

                //Translating the internal analysis result to the user specific analysis result 
                //As the analysis has been looking for variants which are not always selectable, hence if the lNotAlwaysSelectedVariantList
                //contains any record then the analysis will be true, other wise it will be false
                if (cFrameworkWrapper.UsePartInfo)
                {
                    if (lNotAlwaysSelectedPartList.Count > 0)
                        lAnalysisResult = true;
                    else
                        lAnalysisResult = false;
                }
                else
                {
                    if (lNotAlwaysSelectedVariantList.Count > 0)
                        lAnalysisResult = true;
                    else
                        lAnalysisResult = false;
                }

                
                if (cFrameworkWrapper.UsePartInfo)
                {
                    if (lNotAlwaysSelectedPartList.Count != 0)
                        cOutputHandler.printMessageToConsole("Parts which are NOT always selected are: " + ReturnPartNamesFromList(lNotAlwaysSelectedPartList));
                    else
                        cOutputHandler.printMessageToConsole("All valid configurations DO include ALL of the parts!");
                }
                else
                {
                    if (lNotAlwaysSelectedVariantList.Count != 0)
                        cOutputHandler.printMessageToConsole("Variats which are NOT always selected are: " + ReturnVariantNamesFromList(lNotAlwaysSelectedVariantList));
                    else
                        cOutputHandler.printMessageToConsole("All valid configurations DO include ALL of the variants!");
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AlwaysSelectedVariantAnalysis");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis checks if there are any operations which are always selected
        /// </summary>
        public bool AlwaysSelectedOperationAnalysis(bool pVariationPointsSet
                                                    , bool pReportTypeSet)
        {
            bool lAnalysisResult = false;

            Status lInternalAnalysisResult = Status.UNKNOWN;
            try
            {
                //TODO: Only should be done when a flag is set
                cOutputHandler.printMessageToConsole("Always selected operation Analysis:");

                //2.Is there any operation that will always be selected
                //C and P => O_i

                bool lAnalysisComplete = false;
                HashSet<string> lNotAlwaysSelectedOperationNameList = new HashSet<string>();

                cZ3Solver.PrepareDebugDirectory();

                //To start empty the constraint set to initialize the analysiss
                //In order to analyze this goal we have first add the C (configuration rules) and P (Operation precedence rules) to the constraints
                //In order to add the C and the P we set the coresponding variation points
                if (!pVariationPointsSet)
                    setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                        , Enumerations.AnalysisType.AlwaysSelectedOperationAnalysis);

                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                //          , Transitions Result, Analysis Timing, Unsat Core
                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                //          , Report timings, Debug Mode (Make model file), User Messages
                if (!pReportTypeSet)
                    setReportType(true, false, true
                                , true, false, false
                                , false, true, false
                                , true, true, true);

                ////In this analysis we only want to check the first transition, and check if each operation is in the initial status in the first transition or not
                ////Hence there is no need to loop over all transitions like previous types of analysis
                cCurrentTransitionNumber = 0;

                MakeStaticPartOfProductPlatformModel();

                MakeDynamicPartOfProductPlatformModel(lAnalysisComplete);

                HashSet<Operation> lOperationSet = cFrameworkWrapper.OperationSet;

                foreach (Operation lCurrentOperation in lOperationSet)
                {
                    BoolExpr l = ConvertOperationInstanceWithMissingTransitionNumber(lCurrentOperation
                                                                                    , Enumerations.OperationInstanceState.Unused
                                                                                    , 0);

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
                    lInternalAnalysisResult = AnalyzeProductPlatform(cCurrentTransitionNumber
                                                            , 0
                                                            , lAnalysisComplete);
                    /////                            if (!lAnalysisResult && !lAnalysisComplete)

                    if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    {
                        //This line is moved to the reporting procedure in this class
                        //if it does hold, then there exists a valid configuration in which the current operation is UNUSED!
                        //cOutputHandler.printMessageToConsole("There DOES exist a configuration in which " + lOperationName + " is in an UNUSED state!");

                        if (!lNotAlwaysSelectedOperationNameList.Contains(lCurrentOperation.Name))
                            lNotAlwaysSelectedOperationNameList.Add(lCurrentOperation.Name);
                    }
                    else if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                    {
                        //cOutputHandler.printMessageToConsole("All valid configurations DO include " + lOperationName + ".");
                    }

                    cZ3Solver.SolverPopFunction();
                            
                }


                //Translating the internal analysis result to the user specific analysis result 
                if (lNotAlwaysSelectedOperationNameList.Count > 0)
                    lAnalysisResult = true;
                else
                    lAnalysisResult = false;
                
                if (lNotAlwaysSelectedOperationNameList.Count != 0)
                    cOutputHandler.printMessageToConsole("Operations which are NOT always selected are: "
                        + ReturnOperationNamesStringFromOperationNameList(lNotAlwaysSelectedOperationNameList));
                else
                    cOutputHandler.printMessageToConsole("All valid configurations DO include ALL the operations!");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in AlwaysSelectedOperationAnalysis");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis checks if there are any operations which are not able to be selected
        /// </summary>
        public bool OperationSelectabilityAnalysis(bool pVariationPointsSet
                                                    , bool pReportTypeSet)
        {
            bool lAnalysisResult = false;

            Status lInternalAnalysisResult = Status.UNKNOWN;

            try
            {
                //TODO: Only should be done when a flag is set
                cOutputHandler.printMessageToConsole("Operation Selectability Analysis:");

                //3.Is there any operation that will not be possible to select? (This s a ststic type of analysis hence we will not include the P set
                //O_i and C

                bool lAnalysisComplete = false;
                HashSet<string> lUnselectableOperationNameSet = new HashSet<string>();
                HashSet<string> lInActiveOperationNameSet = new HashSet<string>();
                //HashSet<OperationInstance> lOperationInstanceList = cFrameworkWrapper.OperationInstanceSet;

                //To start all operations are unselectable unless otherwise proven
                lUnselectableOperationNameSet = cFrameworkWrapper.getSetOfOperationNames();

                //To start empty the constraint set to initialize the analysiss
                //In order to analyze this goal we have first add the C (configuration rules) and P (Operation precedence rules) to the constraints
                //In order to add the C and the P we set the coresponding variation points
                if (!pVariationPointsSet)
                    setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                        , Enumerations.AnalysisType.OperationSelectabilityAnalysis);

                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                //          , Transitions Result, Analysis Timing, Unsat Core
                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                //          , Report timings, Debug Mode (Make model file), User Messages
                if (!pReportTypeSet)
                    setReportType(true, false, true
                                , true, false, false
                                , false, true, false
                                , true, true, true);

                ////In this analysis we only want to check the first transition, and check if each operation is in the initial status in the first transition or not
                ////Hence there is no need to loop over all transitions like previous types of analysis
                int lTransitionNo = 0;

                MakeStaticPartOfProductPlatformModel();

                MakeDynamicPartOfProductPlatformModel(lAnalysisComplete);

                HashSet<Operation> lOperationSet = cFrameworkWrapper.OperationSet;

                foreach (Operation lCurrentOperation in lOperationSet)
                {
                    //First we check if the current operation has a relation to a specific variant, hence is it active?
                    //if (cFrameworkWrapper.isOperationActive(lCurrentOperation))
                    //{
                        BoolExpr l = ConvertOperationInstanceWithMissingTransitionNumber(lCurrentOperation
                                                                        , Enumerations.OperationInstanceState.Initial
                                                                        , lTransitionNo);

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
                            //cOutputHandler.printMessageToConsole(lFrameworkWrapper.getOperationFromOperationName(lOperationInstanceVariableName).names + " is selectable.");
                            ReportSolverResult(lTransitionNo, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, lCurrentOperation.Name);

                            if (lUnselectableOperationNameSet.Contains(lCurrentOperation.Name))
                                lUnselectableOperationNameSet.Remove(lCurrentOperation.Name);
                        }
                        else
                        {
                            //If the operation is inactive?
                            //This means the operation instance is inactive hence it should be mentioned

                            //This is when we want to report only the operation name
                            //cOutputHandler.printMessageToConsole("Operation " + lFrameworkWrapper.returnOperationNameFromOperationInstance(lOperationInstanceVariableName) + " is inactive!");
                            ReportSolverResult(lTransitionNo, lAnalysisComplete, cFrameworkWrapper, lInternalAnalysisResult, lCurrentOperation.Name);

                            //This is when we want to report the operation instance
                            if (!lInActiveOperationNameSet.Contains(lCurrentOperation.Name))
                            {
                                lInActiveOperationNameSet.Add(lCurrentOperation.Name);
                                if (lUnselectableOperationNameSet.Contains(lCurrentOperation.Name))
                                    lUnselectableOperationNameSet.Remove(lCurrentOperation.Name);
                            }
                        }

                        cZ3Solver.SolverPopFunction();

                    //}
                }

                //Translating the internal analysis result to the user specific analysis result 
                if (lUnselectableOperationNameSet.Count.Equals(0))
                    lAnalysisResult = true;
                else
                    lAnalysisResult = false;

                
                if (cReportAnalysisResult)
                { 
                    if (lInActiveOperationNameSet.Count != 0)
                        cOutputHandler.printMessageToConsole("Operation " + ReturnOperationNamesStringFromOperationNameList(lInActiveOperationNameSet) + " is inactive!");

                    if (lUnselectableOperationNameSet.Count != 0)
                        cOutputHandler.printMessageToConsole("Operations which are not selectable are: " + ReturnOperationNamesStringFromOperationNameList(lUnselectableOperationNameSet));
                    else
                        cOutputHandler.printMessageToConsole("All operations are selectable!");
                }

                ////TODO: when is the analysis complete??????????????
                //if (??????lTransitionNo == lMaxNoOfTransitions - 1)
                //lAnalysisComplete = true;
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in OperationSelectabilityAnalysis");
                cOutputHandler.printMessageToConsole(ex.Message);
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
        private BoolExpr OperationInstanceFinishedStateInAllTransitions(OperationInstance pOperationInstance, int pSpecificTransitionNo = -1)
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
                        lCurrentOperationInstance = (BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance.FinishedVariableName);
                    }
                    else
                    {
                        OperationInstance lTempOperationInstance = new OperationInstance(pOperationInstance.AbstractOperation, i);
                        lCurrentOperationInstance = (BoolExpr)cZ3Solver.FindExprInExprSet(lTempOperationInstance.FinishedVariableName);

                    }
                    if (lCurrentOperationInstance != null)
                        if (lResultExpr != null)
                            lResultExpr = cZ3Solver.AndOperator(new List<BoolExpr>() { lResultExpr, lCurrentOperationInstance });
                        else
                            lResultExpr = lCurrentOperationInstance;
                    else
                        break;

                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in OperationInAllTransitions");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }

        //This function was removed in the latest version in which operation instance became an independent object
        /*/// <summary>
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

                int lVariantIndex = cFrameworkWrapper.indexLookupByVariant(pVariant);

                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                int lMaxNoOfTransitions = CalculateAnalysisNoOfCycles();
                for (int i = 0; i < lMaxNoOfTransitions; i++)
                {
                    BoolExpr lCurrentOperationInstance;
                    if (pSpecificTransitionNo != -1 && pSpecificTransitionNo.Equals(i))
                    {
                        lCurrentOperationInstance = ReturnOperationInstanceVariable(pOperation.names
                                                                                            , pOperationState
                                                                                            , lVariantIndex
                                                                                            , i);
                    }
                    else
                    {
                        lCurrentOperationInstance = ReturnOperationInstanceVariable(pOperation.names
                                                                                            , pOperationState
                                                                                            , lVariantIndex
                                                                                            , i);

                    }
                    if (lCurrentOperationInstance != null)
                        if (lResultExpr != null)
                            lResultExpr = cZ3Solver.AndOperator(new List<BoolExpr>() { lResultExpr, lCurrentOperationInstance });
                        else
                            lResultExpr = lCurrentOperationInstance;
                    else
                        break;

                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in OperationInAllTransitions");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }*/

        //This function was removed in the latest version in which operation instance became an independent object
        /*/// <summary>
        /// This function will give us a boolean expression of all the operation instances of a specific operation on a specific operation state
        /// in any different transitions (or in one specific transition). All the operation instances are joined by OR
        /// </summary>
        /// <param name="pOperation"></param>
        /// <param name="pOperationState"></param>
        /// <param name="pSpecificTransitionNo">In we need the expression for one transition</param>
        /// <returns></returns>
        private BoolExpr OperationInAnyTransitions(Operation pOperation, string pOperationState, int pSpecificTransitionNo = -1)
        {
            BoolExpr lResultExpr = null;
            try
            {
                //Here we have an operation but as the transition number is not given we are looking for
                //for operation instances in all of the transitions

                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                OperationInstance lCurrentOperationInstance;
                if (pSpecificTransitionNo != -1 && pSpecificTransitionNo.Equals(i))
                {
                    lCurrentOperationInstance = ReturnOperationInstanceVariable(pOperation.Name
                                                                                        , pOperationState
                                                                                        , i);
                }
                else
                {
                    lCurrentOperationInstance = ReturnOperationInstanceVariable(pOperation.Name
                                                                                        , pOperationState
                                                                                        , i);
                }
                if (lCurrentOperationInstance != null)
                {
                    if (lResultExpr != null)
                        lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr>() { lResultExpr, lCurrentOperationInstance });
                    else
                        lResultExpr = lCurrentOperationInstance;
                }
                else
                    break;

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in OperationInAllTransitions");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }*/

        /// <summary>
        /// This function will give us a boolean expression of all the operation instances of a specific operation on a specific operation state
        /// in any different transitions (or in one specific transition). All the operation instances are joined by OR
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <param name="pOperationState"></param>
        /// <param name="pSpecificTransitionNo">In we need the expression for one transition</param>
        /// <returns></returns>
        private BoolExpr ConvertOperationInstanceWithMissingTransitionNumber(Operation pOperation
                                                                            , Enumerations.OperationInstanceState pOperationState
                                                                            , int pTransitionNo)
        {
            BoolExpr lResultExpr = null;
            try
            {
                //Here we have an operation instance but as the transition number is not given we are looking for
                //for operation instances in the current transition and or previous transition of the transitions

                BoolExpr lCurrentOperationInstance = null;
                if (!pTransitionNo.Equals(-1))
                {
                    OperationInstance lTempOperationInstance = new OperationInstance(pOperation, pTransitionNo);
                    lCurrentOperationInstance = ReturnOperationInstanceVariableInOneState(lTempOperationInstance, pOperationState);

                    if (lCurrentOperationInstance != null)
                    {
                        if (lResultExpr != null)
                            lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr>() { lResultExpr, lCurrentOperationInstance });
                        else
                            lResultExpr = lCurrentOperationInstance;
                    }
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in OperationInAllTransitions");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }

        public BoolExpr ReturnOperationInstanceVariableInOneState(OperationInstance pOperationInstance, Enumerations.OperationInstanceState pOperationState)
        {
            BoolExpr lResultOperationInstanceVariable = null;
            try
            {
                switch (pOperationState)
                {
                    case Enumerations.OperationInstanceState.Initial:
                        lResultOperationInstanceVariable = (BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance.InitialVariableName);
                        break;
                    case Enumerations.OperationInstanceState.Executing:
                        lResultOperationInstanceVariable = (BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance.ExecutingVariableName);
                        break;
                    case Enumerations.OperationInstanceState.Finished:
                        lResultOperationInstanceVariable = (BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance.FinishedVariableName);
                        break;
                    case Enumerations.OperationInstanceState.Unused:
                        lResultOperationInstanceVariable = (BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance.UnusedVariableName);
                        break;
                    default:
                        lResultOperationInstanceVariable = (BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance.UnusedVariableName);
                        break;
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnOperationInstanceVariableInOneState");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultOperationInstanceVariable;
        }

        //This function was removed in the new version where operation instance became a independent object
        /*/// <summary>
        /// This function takes an operation and returns am expression of operation instnces for that operation and ALL of the variants in ALL transitions
        /// These any variants and in any transitions can be fixed to one specific variant and one specific transition if their respective parameters are set
        /// Also the operation state needs to be given
        /// </summary>
        /// <param name="pOperation"></param>
        /// <param name="pOperationState"></param>
        /// <param name="pSpecificIndex">If the expression needs to be made for just one variant</param>
        /// <param name="pSpecificTransition">If the expression needs to be made for just one transition</param>
        /// <returns></returns>
        private BoolExpr OperationForAllVariantsORPartsInAllTransitions(operation pOperation
                                                                , string pOperationState
                                                                , int pSpecificIndex = -1
                                                                , int pSpecificTransition = -1)
        {
            BoolExpr lResultExpr = null;
            try
            {
                if (cFrameworkWrapper.UsePartInfo)
                {
                    foreach (part lCurrentPart in cFrameworkWrapper.PartSet)
                    {
                        int lCurrentPartIndex = cFrameworkWrapper.indexLookupByPart(lCurrentPart);
                        if (pSpecificIndex.Equals(-1))
                            if (lResultExpr == null)
                                lResultExpr = OperationInAllTransitions(pOperation, lCurrentPart, pOperationState, pSpecificTransition);
                            else
                                lResultExpr = cZ3Solver.AndOperator(new List<BoolExpr>() { lResultExpr, OperationInAllTransitions(pOperation
                                                                                                                            , lCurrentPart
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                        else if (pSpecificIndex != -1 && pSpecificIndex.Equals(lCurrentPartIndex))
                            if (lResultExpr == null)

                                lResultExpr = OperationInAllTransitions(pOperation, lCurrentPart, pOperationState, pSpecificTransition);
                            else
                                lResultExpr = cZ3Solver.AndOperator(new List<BoolExpr>() { lResultExpr, OperationInAllTransitions(pOperation
                                                                                                                            , lCurrentPart
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                    }
                }
                else
                {
                    foreach (variant lCurrentVariant in cFrameworkWrapper.VariantSet)
                    {
                        int lCurrentVariantIndex = cFrameworkWrapper.indexLookupByVariant(lCurrentVariant);
                        if (pSpecificIndex.Equals(-1))
                            if (lResultExpr == null)
                                lResultExpr = OperationInAllTransitions(pOperation, lCurrentVariant, pOperationState, pSpecificTransition);
                            else
                                lResultExpr = cZ3Solver.AndOperator(new List<BoolExpr>() { lResultExpr, OperationInAllTransitions(pOperation
                                                                                                                            , lCurrentVariant
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                        else if (pSpecificIndex != -1 && pSpecificIndex.Equals(lCurrentVariantIndex))
                            if (lResultExpr == null)

                                lResultExpr = OperationInAllTransitions(pOperation, lCurrentVariant, pOperationState, pSpecificTransition);
                            else
                                lResultExpr = cZ3Solver.AndOperator(new List<BoolExpr>() { lResultExpr, OperationInAllTransitions(pOperation
                                                                                                                            , lCurrentVariant
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                    }
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in OperationForAllVariantsORPartsInAllTransitions");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }*/

        //This function was removed in the new version where operation instance became a independent object
        /*/// <summary>
        /// This function takes an operation and returns am expression of operation instnces for that operation and ANY of the variants in ALL transitions
        /// These any variants and in any transitions can be fixed to one specific variant and one specific transition if their respective parameters are set
        /// Also the operation state needs to be given
        /// </summary>
        /// <param name="pOperation"></param>
        /// <param name="pOperationState"></param>
        /// <param name="pSpecificIndex">If the expression needs to be made for just one variant</param>
        /// <param name="pSpecificTransition">If the expression needs to be made for just one transition</param>
        /// <returns></returns>
        private BoolExpr OperationForAnyVariantsORPartsInAllTransitions(operation pOperation
                                                                , string pOperationState
                                                                , int pSpecificIndex = -1
                                                                , int pSpecificTransition = -1)
        {
            BoolExpr lResultExpr = null;
            try
            {
                if (cFrameworkWrapper.UsePartInfo)
                {
                    foreach (part lCurrentPart in cFrameworkWrapper.PartSet)
                    {
                        int lCurrentPartIndex = cFrameworkWrapper.indexLookupByPart(lCurrentPart);
                        if (pSpecificIndex.Equals(-1))
                            if (lResultExpr == null)
                                lResultExpr = OperationInAllTransitions(pOperation, lCurrentPart, pOperationState, pSpecificTransition);
                            else
                                lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr>() { lResultExpr, OperationInAllTransitions(pOperation
                                                                                                                            , lCurrentPart
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                        else if (pSpecificIndex != -1 && pSpecificIndex.Equals(lCurrentPartIndex))
                            if (lResultExpr == null)

                                lResultExpr = OperationInAllTransitions(pOperation, lCurrentPart, pOperationState, pSpecificTransition);
                            else
                                lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr>() { lResultExpr, OperationInAllTransitions(pOperation
                                                                                                                            , lCurrentPart
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                    }
                }
                else
                {
                    foreach (variant lCurrentVariant in cFrameworkWrapper.VariantSet)
                    {
                        int lCurrentVariantIndex = cFrameworkWrapper.indexLookupByVariant(lCurrentVariant);
                        if (pSpecificIndex.Equals(-1))
                            if (lResultExpr == null)
                                lResultExpr = OperationInAllTransitions(pOperation, lCurrentVariant, pOperationState, pSpecificTransition);
                            else
                                lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr>() { lResultExpr, OperationInAllTransitions(pOperation
                                                                                                                            , lCurrentVariant
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                        else if (pSpecificIndex != -1 && pSpecificIndex.Equals(lCurrentVariantIndex))
                            if (lResultExpr == null)

                                lResultExpr = OperationInAllTransitions(pOperation, lCurrentVariant, pOperationState, pSpecificTransition);
                            else
                                lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr>() { lResultExpr, OperationInAllTransitions(pOperation
                                                                                                                            , lCurrentVariant
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                    }
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in OperationForAnyVariantsORPartsInAllTransitions");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }*/

        //This function was needed before the latest version in which operation instance object was created
        /*/// <summary>
        /// This function takes an operation and returns am expression of operation instnces for that operation and ANY of the variants in ANY transitions
        /// These any variants and in any transitions can be fixed to one specific variant and one specific transition if their respective parameters are set
        /// Also the operation state needs to be given
        /// </summary>
        /// <param name="pOperation"></param>
        /// <param name="pOperationState"></param>
        /// <param name="pSpecificIndex">If the expression needs to be made for just one variant</param>
        /// <param name="pSpecificTransition">If the expression needs to be made for just one transition</param>
        /// <returns></returns>
        private BoolExpr OperationForAnyVariantsInAnyTransitions(operation pOperation
                                                                , string pOperationState
                                                                , int pSpecificIndex = -1
                                                                , int pSpecificTransition = -1)
        {
            BoolExpr lResultExpr = null;
            try
            {
                if (cFrameworkWrapper.UsePartInfo)
                {
                    foreach (part lCurrentPart in cFrameworkWrapper.PartSet)
                    {
                        int lCurrentPartIndex = cFrameworkWrapper.indexLookupByPart(lCurrentPart);
                        if (pSpecificIndex.Equals(-1))
                            if (lResultExpr == null)
                                lResultExpr = OperationInAnyTransitions(pOperation, lCurrentPart, pOperationState, pSpecificTransition);
                            else
                                lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr>() { lResultExpr, OperationInAnyTransitions(pOperation
                                                                                                                            , lCurrentPart
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                        else if (pSpecificIndex != -1 && pSpecificIndex.Equals(lCurrentPartIndex))
                            if (lResultExpr == null)

                                lResultExpr = OperationInAnyTransitions(pOperation, lCurrentPart, pOperationState, pSpecificTransition);
                            else
                                lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr>() { lResultExpr, OperationInAnyTransitions(pOperation
                                                                                                                            , lCurrentPart
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                    }
                }
                else
                {
                    foreach (variant lCurrentVariant in cFrameworkWrapper.VariantSet)
                    {
                        int lCurrentVariantIndex = cFrameworkWrapper.indexLookupByVariant(lCurrentVariant);
                        if (pSpecificIndex.Equals(-1))
                            if (lResultExpr == null)
                                lResultExpr = OperationInAnyTransitions(pOperation, lCurrentVariant, pOperationState, pSpecificTransition);
                            else
                                lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr>() { lResultExpr, OperationInAnyTransitions(pOperation
                                                                                                                            , lCurrentVariant
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                        else if (pSpecificIndex != -1 && pSpecificIndex.Equals(lCurrentVariantIndex))
                            if (lResultExpr == null)

                                lResultExpr = OperationInAnyTransitions(pOperation, lCurrentVariant, pOperationState, pSpecificTransition);
                            else
                                lResultExpr = cZ3Solver.OrOperator(new List<BoolExpr>() { lResultExpr, OperationInAnyTransitions(pOperation
                                                                                                                            , lCurrentVariant
                                                                                                                            , pOperationState
                                                                                                                            , pSpecificTransition) });
                    }
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in OperationForAnyVariantsORPartsInAnyTransitions");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }*/

        //This function was needed before the latest version in which operation instance object was created
        /*/// <summary>
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
                cOutputHandler.printMessageToConsole("error in ReturnOperationInstanceName");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lOperationInstance;
        }*/

        //This function was needed before the latest version in which operation instance object was created
        /*/// <summary>
        /// This function takes the different parts of an operation instance and makes the actual operation instance using these parts
        /// </summary>
        /// <param name="pOperationName"></param>
        /// <param name="pOperationState"></param>
        /// <param name="pVairantIndex"></param>
        /// <param name="pTransitionNo"></param>
        /// <returns>Operation instance</returns>
        private BoolExpr ReturnOperationInstanceVariable(string pOperationInstanceName)
        {
            BoolExpr lOperationInstanceVariable = null;
            try
            {
                if (pOperationInstanceName != null)
                {
                    lOperationInstanceVariable = (BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstanceName);
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnOperationInstanceVariable");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lOperationInstanceVariable;
        }*/

        /// <summary>
        /// This function takes a list of parts and returns a string which includes the name of all the parts in the list
        /// </summary>
        /// <param name="pPartList">The inputed list of parts</param>
        /// <returns>The string containing the name of all parts</returns>
        private string ReturnPartNamesFromList(List<part> pPartList)
        {
            //TODO: This function and the following function has to be made into one which works on generic lists
            //and for that the tostring() of the items in the list has to return the names field of the items in the list

            //Initialize the string which is going to contain the names
            string lPartNames = "";
            try
            {
                //Loop over all the parts in the list
                foreach (part lPart in pPartList)
                {
                    //if the part list is previously filled with something then add a comma before adding the next item
                    if (lPartNames != "")
                        lPartNames += " ,";

                    //Add the name of the variant to the list
                    lPartNames += lPart.names;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnPartNamesFromList");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            //Return the string with the name of the parts
            return lPartNames;
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
                cOutputHandler.printMessageToConsole("error in ReturnVariantNamesFromList");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in ReturnOperationNamesFromList");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in ReturnOperationNamesFromOpertionInstanceList");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            //Return the string with the name of the operations
            return lOperationNames;
        }

        /// <summary>
        /// This function simply takes a list of operation names and returns the items in one string
        /// </summary>
        /// <param name="pOperationNameSet">List of operation names</param>
        /// <returns>string of all operation names</returns>
        private string ReturnOperationNamesStringFromOperationNameList(HashSet<string> pOperationNameSet)
        {
            string lOperationNamesString = "";
            try
            {
                foreach (string lOperationName in pOperationNameSet)
                {
                    if (lOperationNamesString != "")
                        lOperationNamesString += ", ";

                    lOperationNamesString += lOperationName;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ReturnOperationNamesStringFromOperationNameList");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lOperationNamesString;
        }

        public void addFindingModelGoal()
        {
            try
            {
                BoolExpr lResultFormula = null;
                HashSet<OperationInstance> lOperationInstanceSet = cFrameworkWrapper.OperationInstanceSet;
                //Forall Operation Instances: (O_F_0_m OR O_U_0_m) AND (O_F_1_m OR O_U_1_m) AND ... AND (O_F_n_m OR O_U_n_m))

                //TODO: This has to be fed into a class instance variable, as it is done in another part of this class as well!!
                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                int lMaxNoOfTransitions = CalculateAnalysisNoOfCycles();

                //Filter to only operation instances of the last transition
                List<OperationInstance> lCurrentTrnasitionNoOperationInstanceSet = filterOperationInstancesOfOneTransition(lOperationInstanceSet, lMaxNoOfTransitions - 1);
                
                //List of : (O_F_0_m OR O_U_0_m), (O_F_1_m OR O_U_1_m), ..., (O_F_n_m OR O_U_n_m)
                List<BoolExpr> lWantedExprForEachVariant = returnWantedExprForEachVariantORPart(lCurrentTrnasitionNoOperationInstanceSet);

                //BoolExpr lCurrentTransitionFormulaPart = returnPartsANDedExpression(lWantedExprForEachVariant);

                lResultFormula = cZ3Solver.AndOperator(lWantedExprForEachVariant);

                cZ3Solver.AddConstraintToSolver(lResultFormula, "FindingModelGoal");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in convertFindingModelGoal");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public List<BoolExpr> returnWantedExprForEachVariantORPart(List<OperationInstance> pCurrentTransitionOperationInstanceList)
        {
            //List of : (O_F_0_m OR O_U_0_m), (O_F_1_m OR O_U_1_m), ..., (O_F_n_m OR O_U_n_m)
            //The transition 'm' is given from the set in the parameter

            List<BoolExpr> lResultWantedExpr = new List<BoolExpr>();
            try
            {
                foreach (var lCurrentOperationInstance in pCurrentTransitionOperationInstanceList)
                {

                    BoolExpr lCurrentOperationInstanceExpr = cZ3Solver.OrOperator(new List<string>() { lCurrentOperationInstance.FinishedVariableName
                                                                                                        , lCurrentOperationInstance.UnusedVariableName});
                    lResultWantedExpr.Add(lCurrentOperationInstanceExpr);
                }

                //TODO: After the code has been tested to work then remove the comments bellow

/*                if (cFrameworkWrapper.UsePartInfo)
                {
                    foreach (var lPart in cFrameworkWrapper.PartSet)
                    {
                        int lPartIndex = cFrameworkWrapper.indexLookupByPart(lPart);
                        List<operation> lOperationSet = cFrameworkWrapper.ReturnOnePartsOperations(lPart);
                        foreach (var lCurrentOperation in lOperationSet)
                        {
                            HashSet<string> lCurrentPartOperationInstances = new HashSet<string>();
                            foreach (var lCurrentTransitionOperationInstance in pCurrentTransitionOperationInstanceList)
                            {
                                part lTempPart = cFrameworkWrapper.ReturnOperationPartFromOperationInstance(lCurrentTransitionOperationInstance);
                                int lCurrentOperationInstancePartIndex = cFrameworkWrapper.indexLookupByPart(lTempPart);
                                string lCurrentOperationName = cFrameworkWrapper.ReturnOperationNameFromOperationInstance(lCurrentTransitionOperationInstance);

                                if (lCurrentOperationInstancePartIndex.Equals(lPartIndex)
                                    && lCurrentOperationName.Equals(lCurrentOperation.names))
                                    if (lCurrentTransitionOperationInstance.Contains("_F_") || lCurrentTransitionOperationInstance.Contains("_U_"))
                                        lCurrentPartOperationInstances.Add(lCurrentTransitionOperationInstance);
                            }
                            BoolExpr lCurrentPartExpr = cZ3Solver.OrOperator(lCurrentPartOperationInstances);
                            lResultWantedExpr.Add(lCurrentPartExpr);
                        }
                    }
                }
                else
                {
                    foreach (var lVariant in cFrameworkWrapper.VariantSet)
                    {
                        int lVariantIndex = cFrameworkWrapper.indexLookupByVariant(lVariant);
                        HashSet<operation> lOperationSet = cFrameworkWrapper.ReturnOneVariantsOperations(lVariant);
                        foreach (var lCurrentOperation in lOperationSet)
                        {
                            HashSet<string> lCurrentVariantOperationInstances = new HashSet<string>();
                            foreach (var lCurrentTransitionOperationInstance in pCurrentTransitionOperationInstanceList)
                            {
                                variant lTempVariant = cFrameworkWrapper.ReturnOperationVariantFromOperationInstance(lCurrentTransitionOperationInstance);
                                int lCurrentOperationInstanceVariantIndex = cFrameworkWrapper.indexLookupByVariant(lTempVariant);
                                string lCurrentOperationName = cFrameworkWrapper.ReturnOperationNameFromOperationInstance(lCurrentTransitionOperationInstance);

                                if (lCurrentOperationInstanceVariantIndex.Equals(lVariantIndex)
                                    && lCurrentOperationName.Equals(lCurrentOperation.names))
                                    if (lCurrentTransitionOperationInstance.Contains("_F_") || lCurrentTransitionOperationInstance.Contains("_U_"))
                                        lCurrentVariantOperationInstances.Add(lCurrentTransitionOperationInstance);
                            }
                            BoolExpr lCurrentVariantExpr = cZ3Solver.OrOperator(lCurrentVariantOperationInstances);
                            lResultWantedExpr.Add(lCurrentVariantExpr);
                        }
                    }
                }*/
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in returnWantedExprForEachPart");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultWantedExpr;
        }

        //TODO: If analysis tests pass this is to be removed!
        /*public BoolExpr returnPartsANDedExpression(HashSet<BoolExpr> pOperationInstanceNames)
        {
            BoolExpr lTempExpression = null;
            try
            {
                foreach (var lOperationInstanceName in pOperationInstanceNames)
                    lTempExpression = cZ3Solver.AndOperator(pOperationInstanceNames);


            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in returnPartsANDedExpression");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lTempExpression;
        }*/

        public List<OperationInstance> filterOperationInstancesOfOneTransition(HashSet<OperationInstance> pOperationInstanceList, int pTransitionNo)
        {
            List<OperationInstance> lTempOperationInstanceList = new List<OperationInstance>();
            try
            {
                foreach (var lOperationInstance in pOperationInstanceList)
                {
                    int lOperationTransitionNo = lOperationInstance.TransitionNumber;
                    if (lOperationTransitionNo.Equals(pTransitionNo))
                    {
                        lTempOperationInstanceList.Add(lOperationInstance);
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in filterOperationInstancesOfOneTransition");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lTempOperationInstanceList;
        }

        //This function is not needed after the operation instances are created.
        /*public int returnTransitionNoFromOperationInstance(string pOperationInstance)
        {
            int lTransitionNo = -1;
            try
            {
                string[] lOperationInstanceParts = pOperationInstance.Split('_');
                if (lOperationInstanceParts[3] != null)
                    lTransitionNo = int.Parse(lOperationInstanceParts[3]);
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in returnTransitionNoFromOperationInstance");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lTransitionNo;
        }*/

        /// <summary>
        /// This is the first and most complete goal which looks at if all the configurations are manufaturable
        /// </summary>
        /// <param name="pState"></param>
        public void convertExistenceOfDeadlockGoalV2(int pState)
        {
            try
            {
                BoolExpr lOverallGoal = null;

                //This boolean expression is used to refer to this overall goal
                cZ3Solver.AddBooleanExpression("P" + pState);
                //What we add to the solver is: P_i => (formula 7) AND (formula 8)

                HashSet<OperationInstance> lOperationInstanceSet = cFrameworkWrapper.OperationInstanceSet;

                //Meaning: NO operation can preceed!
                //(Big And) ((! Pre_k_pState AND O_I_k_pState) OR (! Post_k_pState AND O_E_k_pState) OR O_F_k_pState OR O_U_k_pState)
                BoolExpr lFormula7;
                lFormula7 = createFormula7(lOperationInstanceSet, pState);

                //Meaning: At least ONE operation is in the initial or executing state!
                //(Big OR) (O_I_k_pState OR O_E_k_pState)
                BoolExpr lFormula8;
                lFormula8 = createFormula8(lOperationInstanceSet, pState);

                if (lFormula7 != null && lFormula8 != null)
                {
                    lOverallGoal = cZ3Solver.AndOperator(new List<BoolExpr>() { (BoolExpr)cZ3Solver.FindExprInExprSet("F7_" + pState)
                                                                                , (BoolExpr)cZ3Solver.FindExprInExprSet("F8_" + pState) });

                    cZ3Solver.AddImpliesOperator2Constraints((BoolExpr)cZ3Solver.FindExprInExprSet("P" + pState)
                                                                , lOverallGoal
                                                                , "overallGoal");
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("Error in convertExistenceOfDeadlockGoalV2");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        //These functions are from before the change in the operation trigger field.
        //At that time part to operation mapping and variant to operation mapping existed
        //TODO: If the analysis passed all tests these parts should be removed
        /*public variant ParseVariantExpr(string pVariantExpr)
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
                    lResultVariant = cFrameworkWrapper.variantLookupByName(pVariantExpr);
                }


            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ParseVariantExpr");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultVariant;
        }*/

        //These functions are from before the change in the operation trigger field.
        //At that time part to operation mapping and variant to operation mapping existed
        //TODO: If the analysis passed all tests these parts should be removed
        /*public part ParsePartExpr(string pPartExpr)
        {
            part lResultPart = null;
            try
            {

                if (pPartExpr.Contains(' '))
                {
                    //Then this should be an expression on parts
                    lResultPart = cFrameworkWrapper.createVirtualPart(pPartExpr);
                }
                else
                {
                    //Then this should be a single part
                    lResultPart = cFrameworkWrapper.partLookupByName(pPartExpr);
                }


            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ParsePartExpr");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultPart;
        }*/

        //These functions are from before the change in the operation trigger field.
        //At that time part to operation mapping and variant to operation mapping existed
        //TODO: If the analysis passed all tests these parts should be removed
        /*public void ParsingPartExpr2OperationsMapping(string pPartExpr, HashSet<operation> pOperations)
        {
            try
            {
                part lCurrentPart;
                //First we have to see if our pPartExpr is a single part or an Expression of parts
                if (pPartExpr.Contains(' '))
                {
                    //Meaning it is an expression

                    //incase the partExpr is an expression on a couple of parts we do the following
                    //1. Enter a new VirtualPart to the parts list
                    //2. Add the entered VirtualPart to the VirtualPartGroup ??
                    //3. Add a constraint relating the newly created VirtualPart to the PartExpr
                    //4. Add the VirtualPart and the PartExpr to the local virtualPart2PartExprList
                    lCurrentPart = ParsePartExpr(pPartExpr);

                    //5. Add the VirtualPart and its operations or variantOperationsList
                    //6. For all the operations create their respective variables
                    addVirtualPartOperationInstances(lCurrentPart, pOperations);
                }
                else
                {
                    //Meaning it is a simple part
                    lCurrentPart = cFrameworkWrapper.partLookupByName(pPartExpr);

                    //5. Add the VirtualPart and its operations or variantOperationsList
                    //6. For all the operations create their respective variables
                    addVirtualPartOperationInstances(lCurrentPart, pOperations);
                    
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ParsingPartExpr2OperationsMapping");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }*/

        //These functions are from before the change in the operation trigger field.
        //At that time part to operation mapping and variant to operation mapping existed
        //TODO: If the analysis passed all tests these parts should be removed
        /*public bool createPartOperationInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                //First we extract the current set of part operations from the input file
                HashSet<partOperations> lTemporaryPartOperationsList = new HashSet<partOperations>();
                lTemporaryPartOperationsList = cFrameworkWrapper.createPartOperationTemporaryInstances(pXDoc);

                if (lTemporaryPartOperationsList.Count.Equals(0))
                    lDataLoaded = false;
                else
                {
                    foreach (partOperations lPartOperation in lTemporaryPartOperationsList)
                    {
                        string lCurrentPartExpr = lPartOperation.getPartExpr();
                        HashSet<operation> lCurrentOperation = lPartOperation.getOperations();

                        //Now we parse each instace of part operation so for those who have a part expression instead of part we can create Virtual parts
                        ParsingPartExpr2OperationsMapping(lCurrentPartExpr, lCurrentOperation);
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in createPartOperationInstances");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }*/

        public bool LoadInitialDataFromXMLFile(string pFilePath)
        {
            bool lDataLoaded = false;
            try
            {
                //new instance of xdoc
                XmlDocument xDoc = new XmlDocument();

                //First load the XML file from the file path
                xDoc.Load(pFilePath);
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine("Initial data importing...");

                bool lPartsLoaded = false;
                lPartsLoaded = cFrameworkWrapper.createPartInstances(xDoc);

                bool lVariantsLoaded = false;
                lVariantsLoaded = cFrameworkWrapper.createVariantInstances(xDoc);

                bool lVariantGroupsLoaded = false;
                lVariantGroupsLoaded = cFrameworkWrapper.createVariantGroupInstances(xDoc);

                bool lConstraintsLoaded = false;
                lConstraintsLoaded = cFrameworkWrapper.createConstraintInstances(xDoc);

                bool lOperationsLoaded = false;
                lOperationsLoaded = cFrameworkWrapper.createOperationInstances(xDoc);

                bool lItemUsageRulesLoaded = false;
                lItemUsageRulesLoaded = cFrameworkWrapper.createItemUsageRulesInstances(xDoc);

                //These two parts are not needed because the trigger field in the operationinstance object will serve this purpose
                /*
                bool lPartOperationLoaded = false;
                lPartOperationLoaded = cFrameworkWrapper.createPartOperationsInstances(xDoc);

                bool lVariantOperationLoaded = false;
                lVariantOperationLoaded = cFrameworkWrapper.createVariantOperationsInstances(xDoc);
                */

                bool lTraitsLoaded = false;
                lTraitsLoaded = cFrameworkWrapper.createTraitInstances(xDoc);

                bool lResourceLoaded = false;
                lResourceLoaded = cFrameworkWrapper.createResourceInstances(xDoc);


                lDataLoaded = ((lVariantsLoaded && lVariantGroupsLoaded) && lOperationsLoaded);

                Console.WriteLine("-----------------------------------------------------------");
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in LoadInitialDataFromXMLFile, FilePath: " + pFilePath);
                cOutputHandler.printMessageToConsole(ex.Message);
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
                int lNoOfOperations = cFrameworkWrapper.OperationSet.Count();
                //int lNoOfOperations = lFrameworkWrapper.getNumberOfActiveOperations();


                //Each operation will have two transitions
                lResultNoOfCycles = lNoOfOperations * 2;

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in CalculateAnalysisNoOfCycles");
                cOutputHandler.printMessageToConsole(ex.Message);
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
        public bool ProductPlatformAnalysis(string pInternalFileData = "", string pExtraConfigurationRule = ""
                                            , int pMaxVariantGroupumber = 0, int pMaxVariantNumber = 0, int pMaxPartNumber = 0
                                            , int pMaxOperationNumber = 0, int pTrueProbability = 0, int pFalseProbability = 0
                                            , int pExpressionProbability = 0, int pMaxTraitNumber = 0, int pMaxNoOfTraitAttributes = 0
                                            , int pMaxResourceNumber = 0, int pMaxExpressionOperandNumber = 0)

        {
            bool lLoadInitialData = false;
            bool lTestResult = false;

            try
            {
                if (pInternalFileData != "")
                    lLoadInitialData = loadInitialData(Enumerations.InitializerSource.InitialDataFile, pInternalFileData);
                else
                    lLoadInitialData = loadInitialData(Enumerations.InitializerSource.RandomData, ""
                                                        , pMaxVariantGroupumber, pMaxVariantNumber, pMaxPartNumber
                                                        , pMaxOperationNumber, pTrueProbability, pFalseProbability
                                                        , pExpressionProbability, pMaxTraitNumber, pMaxNoOfTraitAttributes
                                                        , pMaxResourceNumber, pMaxExpressionOperandNumber);



                if (lLoadInitialData)
                {
                    switch (cAnalysisType)
                    {
                        case Enumerations.AnalysisType.ProductModelEnumerationAnalysis:
                            {
                                lTestResult = ProductModelEnumerationAnalysis(true, true);
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
                        case Enumerations.AnalysisType.ProductManufacturingModelEnumerationAnalysis:
                            {
                                lTestResult = ProductManufacturingEnumerationAnalysis(true, true);
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
                    cOutputHandler.printMessageToConsole("Initial data incomplete! Analysis can't be done!");
                    if (cStopAEndOfAnalysis)
                        Console.ReadKey();
                }

            }
            catch (Z3Exception ex)
            {
                cOutputHandler.printMessageToConsole("Z3 Managed Exception: " + ex.Message);
                cOutputHandler.printMessageToConsole("Stack trace: " + ex.StackTrace);
            }
            return lTestResult;
        }

        public void ReportingSetting()
        {
            try
            {
                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                //          , Transitions Result, Analysis Timing, Unsat Core
                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                //          , Report timings, Debug Mode (Make model file), User Messages
                bool lReturnToMainMenu = false;
                while (!lReturnToMainMenu)
                {
                    Console.Clear();
                    //1.Analysis Result
                    Console.WriteLine("1.Analysis Result: " + cReportAnalysisResult.ToString());
                    //2.Analysis Detail Result
                    Console.WriteLine("2.Analysis Detail Result: " + cReportAnalysisDetailResult.ToString());
                    //3.Variants Result
                    Console.WriteLine("3.Variants Result: " + cReportVariantsResult.ToString());
                    //4.Transitions Result
                    Console.WriteLine("4.Transitions Result: " + cReportTransitionsResult.ToString());
                    //5.Analysis Timing
                    Console.WriteLine("5.Analysis Timing: " + cReportAnalysisTiming.ToString());
                    //6.Unsat Core
                    Console.WriteLine("6.Unsat Core: " + cReportUnsatCore.ToString());
                    //7.Stop between each transition
                    Console.WriteLine("7.Stop between each transition: " + cStopBetweenEachTransition.ToString());
                    //8.Stop at end of analysis
                    Console.WriteLine("8.Stop at end of analysis: " + cStopAEndOfAnalysis.ToString());
                    //9.Create HTML Output
                    Console.WriteLine("9.Create HTML Output: " + cCreateHTMLOutput.ToString());
                    //10.Report timings
                    Console.WriteLine("10.Report timings: " + cReportTimings.ToString());
                    //11.Debug Mode (Make model file)
                    Console.WriteLine("11.Debug Mode (Make model file): " + cDebugMode.ToString());
                    //12.User Messages
                    Console.WriteLine("12.User Messages: " + cOutputHandler.getEnableUserMessages().ToString());

                    Console.WriteLine("13.Return to main menu.");

                    Console.WriteLine("Which setting do you want to change? ");
                    var lSettingToChange = Console.ReadLine();

                    switch (lSettingToChange)
                    {
                        case "1":
                            cReportAnalysisResult = ChangeSetting(cReportAnalysisResult);
                            break;
                        case "2":
                            cReportAnalysisDetailResult = ChangeSetting(cReportAnalysisDetailResult);
                            break;
                        case "3":
                            cReportVariantsResult = ChangeSetting(cReportVariantsResult);
                            break;
                        case "4":
                            cReportTransitionsResult = ChangeSetting(cReportTransitionsResult);
                            break;
                        case "5":
                            cReportAnalysisTiming = ChangeSetting(cReportAnalysisTiming);
                            break;
                        case "6":
                            cReportUnsatCore = ChangeSetting(cReportUnsatCore);
                            break;
                        case "7":
                            cStopBetweenEachTransition = ChangeSetting(cStopBetweenEachTransition);
                            break;
                        case "8":
                            cStopAEndOfAnalysis = ChangeSetting(cStopAEndOfAnalysis);
                            break;
                        case "9":
                            cCreateHTMLOutput = ChangeSetting(cCreateHTMLOutput);
                            break;
                        case "10":
                            cReportTimings = ChangeSetting(cReportTimings);
                            break;
                        case "11":
                            cDebugMode = ChangeSetting(cDebugMode);
                            break;
                        case "12":
                            {
                                var lResultValue = ChangeSetting(cOutputHandler.getEnableUserMessages());
                                cOutputHandler.setEnableUserMessages(lResultValue);
                                break;
                            }
                        case "13":
                            lReturnToMainMenu = true;
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("ReportingSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void AnalysisSetting(Z3SolverEngineer pZ3SolverEngineer)
        {
            try
            {
                bool lReturnToMainMenu = false;
                while (!lReturnToMainMenu)
                {
                    Console.Clear();
                    //1.Convert variants
                    Console.WriteLine("1.Convert variants: " + cConvertVariants.ToString());
                    //2.Convert configuration rules
                    Console.WriteLine("2.Convert configuration rules: " + cConvertConfigurationRules.ToString());
                    //3.Convert operations
                    Console.WriteLine("3.Convert operations: " + cConvertOperations.ToString());
                    //4.Convert operation precedence rules
                    Console.WriteLine("4.Convert operation precedence rules: " + cConvertOperationPrecedenceRules.ToString());
                    //5.Convert resources
                    Console.WriteLine("5.Convert resources: " + cConvertResources.ToString());
                    //6.Convert goals
                    Console.WriteLine("6.Convert goals: " + cConvertGoal.ToString());
                    //7.Build P Constraints
                    Console.WriteLine("7.Build P Constraints: " + cBuildPConstraints.ToString());
                    //8.Number Of Models Required
                    Console.WriteLine("8.Number Of Models Required: " + cNoOfModelsRequired.ToString());

                    Console.WriteLine("9.Return to main menu.");

                    Console.WriteLine("Which setting do you want to change? ");
                    var lSettingToChange = Console.ReadLine();

                    switch (lSettingToChange)
                    {
                        case "1":
                            cConvertVariants = ChangeSetting(cConvertVariants);
                            break;
                        case "2":
                            cConvertConfigurationRules = ChangeSetting(cConvertConfigurationRules);
                            break;
                        case "3":
                            cConvertOperations = ChangeSetting(cConvertOperations);
                            break;
                        case "4":
                            cConvertOperationPrecedenceRules = ChangeSetting(cConvertOperationPrecedenceRules);
                            break;
                        case "5":
                            cConvertResources = ChangeSetting(cConvertResources);
                            break;
                        case "6":
                            cConvertGoal = ChangeSetting(cConvertGoal);
                            break;
                        case "7":
                            cBuildPConstraints = ChangeSetting(cBuildPConstraints);
                            break;
                        case "8":
                            {
                                Console.WriteLine("How many models do you need to be created?");
                                cNoOfModelsRequired = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "9":
                            lReturnToMainMenu = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("AnalysisSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private bool ChangeSetting(bool pSetting)
        {
            bool lResultSettingValue = false;
            try
            {
                if (pSetting)
                    lResultSettingValue = false;
                else
                    lResultSettingValue = true;

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("ChangeSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultSettingValue;
        }

        public void SetAnalysisType(Z3SolverEngineer pZ3SolverEngineer)
        {
            try
            {
                Console.WriteLine("1. Product enumeration analysis");
                Console.WriteLine("2. Product manufacturing enumeration analysis");
                Console.WriteLine("3. Existence of deadlock analysis");
                Console.WriteLine("Which analysis do you want to perform: ");
                var lAnalysisType = Console.ReadLine();
                switch (lAnalysisType)
                {
                    case "1":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        pZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                                            , Enumerations.AnalysisType.ProductModelEnumerationAnalysis
                                                            , 4);
                        break;
                    case "2":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        pZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Dynamic
                                                            , Enumerations.AnalysisType.ProductManufacturingModelEnumerationAnalysis
                                                            , cNoOfModelsRequired);
                        break;
                    case "3":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        pZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Dynamic
                                                            , Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis
                                                            , cNoOfModelsRequired);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("SetAnalysisType");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void FileAnalysis(Z3SolverEngineer pZ3SolverEngineer)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Please enter in the file name to be analyzed: ");
                var lFileName = Console.ReadLine();

                string lPathPrefix = "../../Test/";

                SetAnalysisType(pZ3SolverEngineer);

                pZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + lFileName + ".xml");

                //Insuficiant data -> No Analysis
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "0.0.0V0VG0O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "0.1.0V0VG1O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "0.2.1V0VG1O0C0P.xml");

                //Different no of variant, different operation trigger
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "1.0.1V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "1.1.2V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "1.2.3V1VG2O0C0P.xml");

                //Different variant group cardinality
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "2.0.3V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "2.1.3V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "2.2.3V1VG2O0C0P.xml");

                //Configuration Constraint added
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.0.2V1VG2O1C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.1.2V1VG2O2C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.2.2V1VG2O1C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.3.2V1VG2O1C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.4.2V1VG2O1C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.5.2V1VG2O1C0P.xml");

                //Operation precondition added
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.0.1V1VG2O0C1P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.1.1V1VG2O0C1P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.2.1V1VG2O0C1P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.3.1V1VG2O0C1P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.4.1V1VG3O0C1P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.6.1V1VG3O0C1P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.7.1V1VG5O0C4P.xml");

                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "5.0.4V2VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "5.1.4V2VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "5.2.4V2VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "5.0.4V2VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "5.0.4V2VG2O0C0P.xml");

                //TODO: for a variant which is selectable what would be a good term for completing the analysis?
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "2.2V1VG2O1C1PNoTransitions-1UnselectV.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.2V1VG2O1C1PNoTransitions.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.2V1VG3O1C1PNoTransition.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "5.2V1VG2O1C1PNoTransition.xml");

                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "6.2V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "7.2V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "8.2V1VG2O0C0P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "9.0.2V1VG2O0C1P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "10.2V1VG2O0C1P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "11.2V1VG2O0C1P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "12.2V1VG2O0C2P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "13.2V1VG2O0C2P.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "14.2V1VG2O0C2P.xml");

                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "Case3Demo.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "Case3DemoV2.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "demo_variant.aml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "Volvo_CAB.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "Volvo_CAB_V2.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "Volvo_CAB_V3.xml");
                //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "Volvo_CAB_V4.xml");

                //Random data creation!
                /*int lMaxVariantGroupumber = 2;
                int lMaxVariantNumber = 4;
                int lMaxPartNumber = 0;
                int lMaxOperationNumber = 3;
                int lTrueProbability = 100;
                int lFalseProbability = 0;
                int lExpressionProbability = 0;
                int lMaxTraitNumber = 4;
                int lMaxNoOfTraitAttributes = 3;
                int lMaxResourceNumber = 2;
                int lMaxExpressionOperandNumber = 3;
                lZ3SolverEngineer.ProductPlatformAnalysis("", ""
                                                            , lMaxVariantGroupumber, lMaxVariantNumber, lMaxPartNumber
                                                            , lMaxOperationNumber, lTrueProbability, lFalseProbability
                                                            , lExpressionProbability, lMaxTraitNumber, lMaxNoOfTraitAttributes
                                                            , lMaxResourceNumber, lMaxExpressionOperandNumber);*/
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("FileAnalysis");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void AnalyzerSetting(Z3SolverEngineer pZ3SolverEngineer)
        {
            try
            {
                var lReturnToMainMenu = false;
                while (!lReturnToMainMenu)
                {
                    Console.Clear();
                    Console.WriteLine("1.Analysis Settings");
                    Console.WriteLine("2.Reporting Settings");
                    Console.WriteLine("3.Main Menu");
                    Console.WriteLine("Choose the category of the settings you want to change: ");
                    var lUserResponce = Console.ReadLine();

                    switch (lUserResponce)
                    {
                        case "1":
                            AnalysisSetting(pZ3SolverEngineer);
                            break;
                        case "2":
                            ReportingSetting();
                            break;
                        case "3":
                            lReturnToMainMenu = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("AnalyzerSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
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
                bool lExitProgram = false;

                lZ3SolverEngineer.DefaultAnalyzerSetting(lZ3SolverEngineer);

                while (!lExitProgram)
                {
                    Console.Clear();
                    Console.WriteLine("Please pick one of the two options: ");
                    Console.WriteLine("1. Anlyze a file");
                    Console.WriteLine("2. Analyzer Settings");
                    Console.WriteLine("3. Exit");

                    var lUserPick = Console.ReadLine();
                    switch (lUserPick)
                    {
                        case "1":
                            {
                                lZ3SolverEngineer.FileAnalysis(lZ3SolverEngineer);
                                break;
                            }
                        case "2":
                            lZ3SolverEngineer.AnalyzerSetting(lZ3SolverEngineer);
                            break;
                        case "3":
                            lExitProgram = true;
                            break;
                        default:
                            Console.WriteLine("Wrong Option! Please pick agian!");
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }


    }
}
