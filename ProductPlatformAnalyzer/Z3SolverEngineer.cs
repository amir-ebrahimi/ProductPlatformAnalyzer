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
        private FrameworkWrapper _frameworkWrapper;
        private Z3Solver _z3Solver;
        private OutputHandler _outputHandler;
        private RandomTestCreator _randomTestCreator;

        //private bool cDebugMode;
        private bool _opSeqAnalysis;
        private bool _needPreAnalysis;
        private bool _preAnalysisResult;

        //private OperationInstance cOperationInstance_CurrentState;
        //private OperationInstance cOperationInstance_NextState;

        private List<BoolExpr> _configurationConstraints;
        private HashSet<BoolExpr> _pConstraints;


        private int _currentTransitionNumber;
        private int _maxNumberOfTransitions;

        //boolean variation point variables for model building
        private Enumerations.GeneralAnalysisType _generalAnalysisType;
        private Enumerations.AnalysisType _analysisType;

        //private bool cConvertVariants;
        //private bool cConvertConfigurationRules;
        //private bool cConvertOperations;
        //private bool cConvertOperationPrecedenceRules;
        ////private bool cConvertVariantOperationRelations;
        //private bool cConvertResources;
        //private bool cConvertGoal;
        //private bool cBuildPConstraints;
        //private bool cOperationMutualExecution;

        //boolean variation point variables for reporting in output
        //private bool cReportAnalysisResult;
        //private bool cReportAnalysisDetailResult;
        //private bool cReportVariantsResult;
        //private bool cReportTransitionsResult;
        //private bool cReportAnalysisTiming;
        //private bool cReportUnsatCore;
        //private bool cStopBetweenEachTransition;
        //private bool cStopAEndOfAnalysis;
        //private bool cCreateHTMLOutput;
        //private bool cReportTimings;
        //private int cNoOfModelsRequired;
        //private bool cOperationWaiting;

        //boolean variation points for random file creation
        //private int cRandomMaxVariantGroupumber;
        //private int cRandomMaxVariantNumber;
        //private int cRandomMaxPartNumber;
        //private int cRandomMaxOperationNumber;
        //private int cRandomMaxNoOfConfigurationRules;
        //private int cRandomTrueProbability;
        //private int cRandomFalseProbability;
        //private int cRandomExpressionProbability;
        //private int cRandomMaxTraitNumber;
        //private int cRandomMaxNoOfTraitAttributes;
        //private int cRandomMaxResourceNumber;
        //private int cRandomMaxExpressionOperandNumber;
        //private bool cRandomInputFile;

        //Timing variables
        private Stopwatch _stopWatchDetailed = new Stopwatch();
        private Stopwatch _stopWatchTotal = new Stopwatch();

        private double _modelCreationTime;
        private double _modelAnalysisTime;
        private double _modelAnalysisReportingTime;

        Dictionary<string, List<Operation>> _resourcesOperations = new Dictionary<string, List<Operation>>();


        #region Getter-Setters
        //public List<DependentActions> DependentActionsList { get; set; }

        //public void setDebugMode(bool pDebugMode)
        //{
        //    cZ3Solver.SetDebugMode(pDebugMode);
        //}

        //public bool getDebugMode()
        //{
        //    return cZ3Solver.GetDebugMode();
        //}

        //public int RandomMaxNoOfConfigurationRules {
        //    get { return cRandomMaxNoOfConfigurationRules; }
        //    set { cRandomMaxNoOfConfigurationRules = value; }
        //}
        //public int RandomMaxExpressionOperandNumber
        //{
        //    get { return cRandomMaxExpressionOperandNumber; }
        //    set { cRandomMaxExpressionOperandNumber = value; }
        //}
        //public int RandomMaxResourceNumber
        //{
        //    get { return cRandomMaxResourceNumber; }
        //    set { cRandomMaxResourceNumber = value; }
        //}
        //public int RandomMaxNoOfTraitAttributes
        //{
        //    get { return cRandomMaxNoOfTraitAttributes; }
        //    set { cRandomMaxNoOfTraitAttributes = value; }
        //}
        //public int RandomMaxTraitNumber
        //{
        //    get { return cRandomMaxTraitNumber; }
        //    set { cRandomMaxTraitNumber = value; }
        //}
        //public int RandomExpressionProbability
        //{
        //    get { return cRandomExpressionProbability; }
        //    set { cRandomExpressionProbability = value; }
        //}
        //public int RandomFalseProbability
        //{
        //    get { return cRandomFalseProbability; }
        //    set { cRandomFalseProbability = value; }
        //}
        //public int RandomTrueProbability
        //{
        //    get { return cRandomTrueProbability; }
        //    set { cRandomTrueProbability = value; }
        //}
        //public int RandomMaxOperationNumber
        //{
        //    get { return cRandomMaxOperationNumber; }
        //    set { cRandomMaxOperationNumber = value; }
        //}
        //public int RandomMaxPartNumber
        //{
        //    get { return cRandomMaxPartNumber; }
        //    set { cRandomMaxPartNumber = value; }
        //}
        //public int RandomMaxVariantNumber
        //{
        //    get { return cRandomMaxVariantNumber; }
        //    set { cRandomMaxVariantNumber = value; }
        //}
        //public int RandomMaxVariantGroupumber
        //{
        //    get { return cRandomMaxVariantGroupumber; }
        //    set { cRandomMaxVariantGroupumber = value; }
        //}
        //public bool RandomInputFile
        //{
        //    get { return cRandomInputFile; }
        //    set { cRandomInputFile = value; }
        //}
        //public bool ConvertVariants
        //{
        //    get { return cConvertVariants; }
        //    set { cConvertVariants = value; }
        //}
        //public bool ConvertConfigurationRules
        //{
        //    get { return cConvertConfigurationRules; }
        //    set { cConvertConfigurationRules = value; }
        //}
        //public bool ConvertOperations
        //{
        //    get { return cConvertOperations; }
        //    set { cConvertOperations = value; }
        //}
        //public bool ConvertOperationPrecedenceRules
        //{
        //    get { return cConvertOperationPrecedenceRules; }
        //    set { cConvertOperationPrecedenceRules = value; }
        //}
        //public bool ConvertResources
        //{
        //    get { return cConvertResources; }
        //    set { cConvertResources = value; }
        //}
        //public bool ConvertGoal
        //{
        //    get { return cConvertGoal; }
        //    set { cConvertGoal = value; }
        //}
        //public bool BuildPConstraints
        //{
        //    get { return cBuildPConstraints; }
        //    set { cBuildPConstraints = value; }
        //}
        //public bool OperationMutualExecution
        //{
        //    get { return cOperationMutualExecution; }
        //    set { cOperationMutualExecution = value; }
        //}
        //public bool ReportAnalysisResult
        //{
        //    get { return cReportAnalysisResult; }
        //    set { cReportAnalysisResult = value; }
        //}
        //public bool ReportAnalysisDetailResult
        //{
        //    get { return cReportAnalysisDetailResult; }
        //    set { cReportAnalysisDetailResult = value; }
        //}
        //public bool ReportVariantsResult
        //{
        //    get { return cReportVariantsResult; }
        //    set { cReportVariantsResult = value; }
        //}
        //public bool ReportTransitionsResult
        //{
        //    get { return cReportTransitionsResult; }
        //    set { cReportTransitionsResult = value; }
        //}
        //public bool ReportAnalysisTiming
        //{
        //    get { return cReportAnalysisTiming; }
        //    set { cReportAnalysisTiming = value; }
        //}
        //public bool ReportUnsatCore
        //{
        //    get { return cReportUnsatCore; }
        //    set { cReportUnsatCore = value; }
        //}
        //public bool StopBetweenEachTransition {
        //    get { return cStopBetweenEachTransition; }
        //    set { cStopBetweenEachTransition = value;}
        //}
        //public bool StopAEndOfAnalysis {
        //    get { return cStopAEndOfAnalysis; }
        //    set { cStopAEndOfAnalysis = value; }
        //}
        //public bool CreateHTMLOutput
        //{
        //    get { return cCreateHTMLOutput; }
        //    set { cCreateHTMLOutput = value; }
        //}
        //public bool ReportTimings {
        //    get { return cReportTimings; }
        //    set { cReportTimings = value;}
        //}
        //public int NoOfModelsRequired
        //{
        //    get { return cNoOfModelsRequired; }
        //    set { cNoOfModelsRequired = value; }
        //}
        //public bool OperationWaiting
        //{
        //    get { return cOperationWaiting; }
        //    set { cOperationWaiting = value; }
        //}
        #endregion

        #region Props
        public List<DependentActions> DependentActionsList { get; set; }
        public bool DebugMode { get; set; }
        public int RandomMaxNoOfConfigurationRules { get; set; }
        public int RandomMaxExpressionOperandNumber { get; set; }
        public int RandomMaxResourceNumber { get; set; }
        public int RandomMaxNoOfTraitAttributes { get; set; }
        public int RandomMaxTraitNumber { get; set; }
        public int RandomExpressionProbability { get; set; }
        public int RandomFalseProbability { get; set; }
        public int RandomTrueProbability { get; set; }
        public int RandomMaxOperationNumber { get; set; }
        public int RandomMaxPartNumber { get; set; }
        public int RandomMaxVariantNumber { get; set; }
        public int RandomMaxVariantGroupumber { get; set; }
        public bool RandomInputFile { get; set; }
        public bool ConvertVariants { get; set; }
        public bool ConvertConfigurationRules { get; set; }
        public bool ConvertOperations { get; set; }
        public bool ConvertOperationPrecedenceRules { get; set; }
        public bool ConvertResources { get; set; }
        public bool ConvertGoal { get; set; }
        public bool BuildPConstraints { get; set; }
        public bool OperationMutualExecution { get; set; }
        public bool ReportAnalysisResult { get; set; }
        public bool ReportAnalysisDetailResult { get; set; }
        public bool ReportVariantsResult { get; set; }
        public bool ReportTransitionsResult { get; set; }
        public bool ReportAnalysisTiming { get; set; } 
        public bool ReportUnsatCore { get; set; }
        public bool StopBetweenEachTransition { get; set; }
        public bool StopAEndOfAnalysis { get; set; }
        public bool CreateHTMLOutput { get; set; }
        public bool ReportTimings { get; set; }
        public int NoOfModelsRequired { get; set; }
        public bool OperationWaiting { get; set; }
        #endregion

        /// <summary>
        /// This is the creator for the class which initializes the class variables
        /// </summary>
        public Z3SolverEngineer(FrameworkWrapper pFrameworkWrapper = null)
        {
            DependentActionsList = new List<ProductPlatformAnalyzer.DependentActions>();

            _currentTransitionNumber = 0;
            _maxNumberOfTransitions = 0;

            //DefaultAnalyzerSetting();

            DebugMode = false;
            _opSeqAnalysis = true;
            _needPreAnalysis = true;
            _preAnalysisResult = true;
            _configurationConstraints = new List<BoolExpr>();
            _pConstraints = new HashSet<BoolExpr>();

            if (pFrameworkWrapper != null)
                _frameworkWrapper = pFrameworkWrapper;
            //boolean variation point variables
            //setVariationPoints(Enumerations.GeneralAnalysisType.Static, Enumerations.AnalysisType.CompleteAnalysis);

            //Parameters: Analysis Result, Analysis Detail Result, Variants Result
            //          , Transitions Result, Analysis Timing, Unsat Core
            //          , Stop between each transition, Stop at end of analysis, Create HTML Output
            //          , Report timings, Debug Mode (Make model file), User Messages
            /*setReportType(true, true, true
                        , true, true, true
                        , true, true, false
                        , true, true, true);*/
        }

        public void DefaultAnalyzerSetting(OutputHandler pOutputHandler)
        {
            try
            {
                _outputHandler = pOutputHandler;
                _frameworkWrapper = new FrameworkWrapper(_outputHandler);
                _z3Solver = new Z3Solver(_outputHandler);
                _randomTestCreator = new RandomTestCreator(_outputHandler, this, _frameworkWrapper, _z3Solver);

                //LoadVariationPointsFromXMLFile();
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefaultAnalyzerSetting");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// This function sets how the outputs should be reported, it does this by setting the output variation points
        /// </summary>
        public void SetReportType(bool pAnalysisResult
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
                ReportAnalysisResult = pAnalysisResult;
                ReportAnalysisDetailResult = pAnalysisDetailResult;
                ReportVariantsResult = pVariantsResult;
                ReportTransitionsResult = pTransitionsResult;
                ReportAnalysisTiming = pAnalysisTiming;
                ReportUnsatCore = pUnsatCore;
                StopBetweenEachTransition = pStopBetweenEachTransition;
                StopAEndOfAnalysis = pStopAtEndOfAnalysis;
                CreateHTMLOutput = pCreateHTMLOutput;
                ReportTimings = pReportTimings;
                _outputHandler.SetEnableUserMessages(pUserMessages);
                _z3Solver.DebugMode = pDebugMode;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in setReportType");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void DefaultProductModelEnumerationAnalysisVariationPointSetting(bool pOverrideConvertVariants = false
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
                //if (pOverrideNoOfModelsRequired == null)
                //    cNoOfModelsRequired = 1;
                //else
                //    cNoOfModelsRequired = pOverrideNoOfModelsRequired;
                ConvertVariants = (!pOverrideConvertVariants) ? true : false;
                ConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                ConvertOperations = (!pOverrideConvertOperations) ? true : false;
                ConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                ConvertResources = (!pOverrideConvertResources) ? false : true;
                ConvertGoal = (!pOverrideConvertGoal) ? true : false;
                BuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                OperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefaultProductModelEnumerationAnalysisVariationPointSetting");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void DefaultVariantSelectabilityAnalysisVariationPointSetting(bool pOverrideConvertVariants = false
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
                ConvertVariants = (!pOverrideConvertVariants) ? true : false;
                ConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                ConvertOperations = (!pOverrideConvertOperations) ? true : false;
                ConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                ConvertResources = (!pOverrideConvertResources) ? false : true;
                ConvertGoal = (!pOverrideConvertGoal) ? false : true;
                BuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                OperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefaultVariantSelectabilityAnalysisVariationPointSetting");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void DefaultPartSelectabilityAnalysisVariationPointSetting(bool pOverrideConvertVariants = false
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
                ConvertVariants = (!pOverrideConvertVariants) ? true : false;
                ConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                ConvertOperations = (!pOverrideConvertOperations) ? true : false;
                ConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                ConvertResources = (!pOverrideConvertResources) ? false : true;
                ConvertGoal = (!pOverrideConvertGoal) ? false : true;
                BuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                OperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefaultPartSelectabilityAnalysisVariationPointSetting");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void DefaultAlwaysSelectedVariantAnalysisVariationPointSetting(bool pOverrideConvertVariants = false
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
                ConvertVariants = (!pOverrideConvertVariants) ? true : false;
                ConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                ConvertOperations = (!pOverrideConvertOperations) ? true : false;
                ConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                ConvertResources = (!pOverrideConvertResources) ? false : true;
                ConvertGoal = (!pOverrideConvertGoal) ? false : true;
                BuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                OperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefaultAlwaysSelectedVariantAnalysisVariationPointSetting");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void DefaultNeverSelectedVariantAnalysisVariationPointSetting(bool pOverrideConvertVariants = false
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
                ConvertVariants = (!pOverrideConvertVariants) ? true : false;
                ConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                ConvertOperations = (!pOverrideConvertOperations) ? true : false;
                ConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                ConvertResources = (!pOverrideConvertResources) ? false : true;
                ConvertGoal = (!pOverrideConvertGoal) ? false : true;
                BuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                OperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefaultNeverSelectedVariantAnalysisVariationPointSetting");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void DefaultAlwaysSelectedPartAnalysisVariationPointSetting(bool pOverrideConvertVariants = false
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
                ConvertVariants = (!pOverrideConvertVariants) ? true : false;
                ConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                ConvertOperations = (!pOverrideConvertOperations) ? true : false;
                ConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                ConvertResources = (!pOverrideConvertResources) ? false : true;
                ConvertGoal = (!pOverrideConvertGoal) ? false : true;
                BuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                OperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefaultAlwaysSelectedPartAnalysisVariationPointSetting");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void DefaultNeverSelectedPartAnalysisVariationPointSetting(bool pOverrideConvertVariants = false
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
                ConvertVariants = (!pOverrideConvertVariants) ? true : false;
                ConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                ConvertOperations = (!pOverrideConvertOperations) ? true : false;
                ConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                ConvertResources = (!pOverrideConvertResources) ? false : true;
                ConvertGoal = (!pOverrideConvertGoal) ? false : true;
                BuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                OperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefaultNeverSelectedPartAnalysisVariationPointSetting");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void DefaultOperationSelectabilityAnalysisVariationPointSetting(bool pOverrideConvertVariants = false
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
                ConvertVariants = (!pOverrideConvertVariants) ? true : false;
                ConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                ConvertOperations = (!pOverrideConvertOperations) ? true : false;
                ConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                ConvertResources = (!pOverrideConvertResources) ? false : true;
                ConvertGoal = (!pOverrideConvertGoal) ? false : true;
                BuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                OperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefaultOperationSelectabilityAnalysisVariationPointSetting");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void DefaultAlwaysSelectedOperationAnalysisVariationPointSetting(bool pOverrideConvertVariants = false
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
                ConvertVariants = (!pOverrideConvertVariants) ? true : false;
                ConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                ConvertOperations = (!pOverrideConvertOperations) ? true : false;
                ConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                ConvertResources = (!pOverrideConvertResources) ? false : true;
                ConvertGoal = (!pOverrideConvertGoal) ? false : true;
                BuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                OperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefaultAlwaysSelectedOperationAnalysisVariationPointSetting");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void DefaultNeverSelectedOperationAnalysisVariationPointSetting(bool pOverrideConvertVariants = false
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
                ConvertVariants = (!pOverrideConvertVariants) ? true : false;
                ConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                ConvertOperations = (!pOverrideConvertOperations) ? true : false;
                ConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? false : true;
                ConvertResources = (!pOverrideConvertResources) ? false : true;
                ConvertGoal = (!pOverrideConvertGoal) ? false : true;
                BuildPConstraints = (!pOverrideBuildPConstaints) ? false : true;
                OperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefaultNeverSelectedOperationAnalysisVariationPointSetting");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void DefaultExistanceOfDeadlockAnalysisVariationPointSetting(bool pOverrideConvertVariants = false
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
                ConvertVariants = (!pOverrideConvertVariants) ? true : false;
                ConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                ConvertOperations = (!pOverrideConvertOperations) ? true : false;
                ConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? true : false;
                ConvertResources = (!pOverrideConvertResources) ? false : true;
                ConvertGoal = (!pOverrideConvertGoal) ? true : false;
                BuildPConstraints = (!pOverrideBuildPConstaints) ? true : false;
                OperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefaultExistanceOfDeadlockAnalysisVariationPointSetting");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void DefaultProductManufacturingEnumerationAnalysisVariationPointSetting(bool pOverrideConvertVariants = false
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
                ConvertVariants = (!pOverrideConvertVariants) ? true : false;
                ConvertConfigurationRules = (!pOverrideConvertConfigurationRules) ? true : false;
                ConvertOperations = (!pOverrideConvertOperations) ? true : false;
                ConvertOperationPrecedenceRules = (!pOverrideConvertOperationPrecedenceRules) ? true : false;
                ConvertResources = (!pOverrideConvertResources) ? false : true;
                ConvertGoal = (!pOverrideConvertGoal) ? true : false;
                BuildPConstraints = (!pOverrideBuildPConstaints) ? true : false;
                OperationMutualExecution = (!pOverrideOperationMutualExecution) ? false : true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefaultProductManufacturingEnumerationAnalysisVariationPointSetting");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
        public void SetVariationPoints(Enumerations.GeneralAnalysisType pGeneralAnalysisType
                                        , Enumerations.AnalysisType pAnalysisType
                                        , bool pOverrideOperationMutualExecution = false
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
                _generalAnalysisType = pGeneralAnalysisType;
                _analysisType = pAnalysisType;
                switch (_analysisType)
                {
                    case Enumerations.AnalysisType.ProductModelEnumerationAnalysis:
                        {
                            DefaultProductModelEnumerationAnalysisVariationPointSetting(pOverrideConvertVariants
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
                            DefaultProductManufacturingEnumerationAnalysisVariationPointSetting(pOverrideConvertVariants
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
                            DefaultVariantSelectabilityAnalysisVariationPointSetting(pOverrideConvertVariants
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
                            DefaultAlwaysSelectedVariantAnalysisVariationPointSetting(pOverrideConvertVariants
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
                    case Enumerations.AnalysisType.NeverSelectedVariantAnalysis:
                        {
                            DefaultNeverSelectedVariantAnalysisVariationPointSetting(pOverrideConvertVariants
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
                    case Enumerations.AnalysisType.PartSelectabilityAnalysis:
                        {
                            DefaultPartSelectabilityAnalysisVariationPointSetting(pOverrideConvertVariants
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
                    case Enumerations.AnalysisType.AlwaysSelectedPartAnalysis:
                        {
                            DefaultAlwaysSelectedPartAnalysisVariationPointSetting(pOverrideConvertVariants
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
                    case Enumerations.AnalysisType.NeverSelectedPartAnalysis:
                        {
                            DefaultNeverSelectedPartAnalysisVariationPointSetting(pOverrideConvertVariants
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
                            DefaultOperationSelectabilityAnalysisVariationPointSetting(pOverrideConvertVariants
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
                            DefaultAlwaysSelectedOperationAnalysisVariationPointSetting(pOverrideConvertVariants
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
                    case Enumerations.AnalysisType.NeverSelectedOperationAnalysis:
                        {
                            DefaultNeverSelectedOperationAnalysisVariationPointSetting(pOverrideConvertVariants
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
                            DefaultExistanceOfDeadlockAnalysisVariationPointSetting(pOverrideConvertVariants
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
                _outputHandler.PrintMessageToConsole("error in setVariationPoints");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
        public void MakeExpressionListFromVariantGroupList()
        {
            try
            {
                //List<variantGroup> localVariantGroupList = cFrameworkWrapper.VariantGroupList;

                foreach (variantGroup localVariantGroup in _frameworkWrapper.VariantGroupSet)
                    _z3Solver.AddBooleanExpression(localVariantGroup.names);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in makeExpressionListFromVariantGroupList");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// NOT USED ANYWHERE IN THE PROJECT
        /// For each part in the local list a boolean variable is created
        /// </summary>
        public void MakeExpressionListFromPartList()
        {
            try
            {
                foreach (part localPart in _frameworkWrapper.PartSet)
                    _z3Solver.AddBooleanExpression(localPart.names);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in makeExpressionListFromPartList");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// NOT USED ANYWHERE IN THE PROJECT
        /// For each variant in the local list a boolean variable is created
        /// </summary>
        public void MakeExpressionListFromVariantList()
        {
            try
            {
                foreach (variant localVariant in _frameworkWrapper.VariantSet)
                    _z3Solver.AddBooleanExpression(localVariant.names);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in makeExpressionListFromVariantList");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// Load AML File data
        /// </summary>
        /// <param name="pFile"></param>
        /// <returns>If the data is loaded correctly or not</returns>
        public bool LoadAMLInitialData(string pFile)
        {
            bool lDataLoaded = true;
            try
            {
                var document = CAEXDocument.LoadFromFile(pFile);
                
                var converter = new AMLConverter(document, _z3Solver);

                //This function populate should return a boolean result which indicates if the population is done right or not
                converter.Populate();
                converter.PopulateFrameworkWrapper(_frameworkWrapper);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in loadAMLInitialData");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        /// <summary>
        /// This function loads the initial data from the external fle to internal lists
        /// </summary>
        /// <param name="pInitialData"></param>
        /// <param name="pFile"></param>
        /// <returns>If the data is loaded correctly or not</returns>
        public bool LoadInitialData(Enumerations.InitializerSource pInitialData
                                    , String pInitialDataFileName = "")
        {
            bool lDataLoaded = false;
            try
            {
                Enumerations.InputFileType lInitialDataFileExtension = ReturnInputFileType(pInitialDataFileName);
                if (lInitialDataFileExtension.Equals(Enumerations.InputFileType.AML))
                    lDataLoaded = LoadAMLInitialData(pInitialDataFileName);
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
                                lDataLoaded = _randomTestCreator.createRandomData(RandomMaxVariantGroupumber
                                                                                , RandomMaxVariantNumber
                                                                                , RandomMaxPartNumber
                                                                                , RandomMaxOperationNumber
                                                                                , RandomTrueProbability
                                                                                , RandomFalseProbability
                                                                                , RandomExpressionProbability
                                                                                , RandomMaxTraitNumber
                                                                                , RandomMaxNoOfTraitAttributes
                                                                                , RandomMaxResourceNumber
                                                                                , RandomMaxExpressionOperandNumber
                                                                                , RandomMaxNoOfConfigurationRules);

                                if (RandomMaxPartNumber > 0)
                                    _frameworkWrapper.UsePartInfo = true;
                                else
                                    _frameworkWrapper.UsePartInfo = false;

                                break;
                            }
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in loadInitialData");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        /// <summary>
        /// This function looks at the input file name and from the extension returns the type of the file
        /// </summary>
        /// <param name="pFileName">Input file name</param>
        /// <returns>Extension of the file</returns>
        public Enumerations.InputFileType ReturnInputFileType(string pFileName)
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
                _outputHandler.PrintMessageToConsole("error in returnInputFileType");
                _outputHandler.PrintMessageToConsole(ex.Message);
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

        public Status PreAnalysis()
        {
            Status lResultStatus = Status.UNKNOWN;
            try
            {
                CompleteOperationsZ3Variables();

                //Finds which operation's actions are dependent to each other hence they have to be mutually exclusive
                FindDependentOperations();

                //Finds which operations share a resource hence they have to be EXECUTED with mutual exclusion
                ComputeOperationsToResourceMap();

                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                _maxNumberOfTransitions = CalculateAnalysisNoOfCycles();

                //This check is because if it is a random input file then the configuration rules are only created which satisfy the model!
                if (!RandomInputFile)
                    //This line checks if the constraints in the model have any conflicts with each other
                    lResultStatus = AnalyzeModelConstraints(0);
                else
                    lResultStatus = Status.SATISFIABLE;


            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in PreAnalysis");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultStatus;
        }

        /// <summary>
        /// This function creates the static parts of the product platform model, which includes variants, group cardinality, configuration rules, and resources info
        /// </summary>
        /// <param name="pExtraConfigurationRule">If an extra configuration rule needs to be added to the information which comes from the file</param>
        public void MakeStaticPartOfProductPlatformModel(string pExtraConfigurationRule = "", bool pOptimizer = false)
        {
            try
            {
                //If it is asked for the model to include the variants they are added to the created model
                //Variation Point
                if (ConvertVariants)
                    ConvertProductPlatform(pOptimizer);

                //If it is asked for the configuration rules to be added to the model then they are added
                //Variation Point
                
                if (ConvertConfigurationRules)
                    ConvertFConstraint2Z3Constraint(pExtraConfigurationRule, pOptimizer);

                ////I am not sure this part would be needed here, hence I added it to the previous part.
                ////if (lConvertConfigurationRules)
                ////    AddExtraConstraint2Z3Constraint(pExtraConfigurationRule);

                //If it is asked for the resources to be added to the model then they are added
                //Variation Point
                if (ConvertResources)
                    ConvertResourcesNOperationResourceRelations(pOptimizer);

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in MakeStaticPartOfProductPlatformModel");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void CompleteOperationsZ3Variables()
        {
            try
            {
                HashSet<Operation> lOperationSet = _frameworkWrapper.OperationSet;

                foreach (Operation lCurrentOperation in lOperationSet)
                {
                    lCurrentOperation.OperationTriggerVariable = AddZ3ModelVariable(lCurrentOperation.OperationTriggerVariableName);

                    lCurrentOperation.OperationRequirementVariable = AddZ3ModelVariable(lCurrentOperation.OperationRequirementVariableName);
                }

            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        private void FindDependentOperations()
        {
            try
            {
                //First we do an intial analysis on the set of operations, to find out which operation pairs are dependent on each other
                //For the dependent operation pairs formula 6 is applied, but for independent operations formula 6 is not applied
                if (ConvertOperations)
                {
                    Stopwatch lStopWatchDetailed = new Stopwatch();
                    Stopwatch lStopWatchTotal = new Stopwatch();
                    double lActionDependencyModelCreationTime = 0;
                    if (ReportTimings)
                    {
                        lStopWatchDetailed.Start();
                        lStopWatchTotal.Start();

                    }

                    OperationDependencyAnalysis();

                    if (ReportTimings)
                    {
                        lStopWatchDetailed.Stop();
                        lActionDependencyModelCreationTime = lStopWatchDetailed.ElapsedMilliseconds;

                        _outputHandler.PrintMessageToConsole("Action Dependency Analysis Time:" + lActionDependencyModelCreationTime + "ms.");

                        lStopWatchDetailed.Restart();
                    }

                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in FindDependentOperations");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// This function creates the dynamic part of the product platform model which includes the opertions, the precedence rules among the operations
        /// </summary>
        /// <param name="pAnalysisComplete">If the analysis is concluded, finished</param>
        /// <param name="pTransitionNo"></param>
        public void MakeDynamicPartOfProductPlatformModel(bool pAnalysisComplete, int pTransitionNo)
        {
            try
            {
                //////First we do an intial analysis on the set of operations, to find out which operation pairs are dependent on each other
                //////For the dependent operation pairs formula 6 is applied, but for independent operations formula 6 is not applied
                ////if (cConvertOperations)
                ////{
                ////    if (pTransitionNo == 0)
                ////    {
                ////        Stopwatch lStopWatchDetailed = new Stopwatch();
                ////        Stopwatch lStopWatchTotal = new Stopwatch();
                ////        double lActionDependencyModelCreationTime = 0;
                ////        if (cReportTimings)
                ////        {
                ////            lStopWatchDetailed.Start();
                ////            lStopWatchTotal.Start();

                ////        }

                ////        OperationDependencyAnalysis();

                ////        if (cReportTimings)
                ////        {
                ////            lStopWatchDetailed.Stop();
                ////            lActionDependencyModelCreationTime = lStopWatchDetailed.ElapsedMilliseconds;

                ////            cOutputHandler.printMessageToConsole("Action Dependency Analysis Time:" + lActionDependencyModelCreationTime + "ms.");

                ////            lStopWatchDetailed.Restart();
                ////        }

                ////    }
                ////}

                //TODO: write what formulas each of these lines include

                //If it is asked for the model to include the operations then they are added to the model
                //Variation Point
                if (ConvertOperations)
                {
                    //int lTempTransitionNo = int.Parse(pTransitionNo) + 1;
                    //for (int i = 0; i <= lTempTransitionNo; i++)
                    //{
                    ConvertFOperations2Z3Operations(pTransitionNo);
                    //}

                }


                BoolExpr lProgressionRule = null;
                BoolExpr lDeadlockRule = null;
                BoolExpr lFinishedRule = null;
                //If it is asked for the operation precedence rues to be added to the model then they are added
                //Variation Point
                if (ConvertOperationPrecedenceRules || BuildPConstraints)
                {
                    ConvertOperationsPrecedenceRules(pTransitionNo);
                    if (pTransitionNo <= _maxNumberOfTransitions)
                        lProgressionRule = InitializeOperationProgressionRule(pTransitionNo);
                }

                if (_preAnalysisResult)
                    if (_opSeqAnalysis)
                        lDeadlockRule = InitializeDeadlockRule(pTransitionNo);

                lFinishedRule = InitializeFinishedRule(pTransitionNo);

                //Initialize progression rule
                //(not Deadlock) and (not Finished) => Progression
                SetProgressionRule(lDeadlockRule, lFinishedRule, lProgressionRule);


            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in MakeDynamicPartOfProductPlatformModel");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void SetProgressionRule(BoolExpr pDeadlockRule
                                        , BoolExpr pFinishedRule
                                        , BoolExpr pProgressionRule)
        {
            try
            {
                BoolExpr lNotDeadlock = null;
                BoolExpr lNotFinished = null;
                BoolExpr lRightHandSide = null;

                //(not Deadlock) and (not Finished) => Progression
                lNotDeadlock = _z3Solver.NotOperator(pDeadlockRule);

                if (pFinishedRule != null)
                    lNotFinished = _z3Solver.NotOperator(pFinishedRule);

                if (lNotFinished != null)
                    lRightHandSide = _z3Solver.AndOperator(new List<BoolExpr>() { lNotDeadlock, lNotFinished });
                else
                    lRightHandSide = lNotDeadlock;

                _z3Solver.AddImpliesOperator2Constraints(lRightHandSide, pProgressionRule, "SettingProgressionRule");
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in SetProgressionRule");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private BoolExpr InitializeFinishedRule(int pTransitionNo)
        {
            BoolExpr lFinishedRule = null;
            try
            {
                BoolExpr lFinishedVariable = _z3Solver.AddBooleanExpression("Finished_" + pTransitionNo);

                HashSet<OperationInstance> lOperationInstanceSet = _frameworkWrapper.GetOperationInstancesInOneTransition(pTransitionNo);

                foreach (OperationInstance lCurrentOperationInstance in lOperationInstanceSet)
                {
                    BoolExpr lTempExpr = null;
                    lTempExpr = _z3Solver.OrOperator(new List<BoolExpr>() { lCurrentOperationInstance.FinishedVariable
                                                                            , lCurrentOperationInstance.UnusedVariable});
                    if (lFinishedRule == null)
                        lFinishedRule = lTempExpr;
                    else
                        lFinishedRule = _z3Solver.AndOperator(new List<BoolExpr>() { lFinishedRule, lTempExpr});
                }

                _z3Solver.AddTwoWayImpliesOperator2Constraints(lFinishedVariable, lFinishedRule, "Finished Rule Initialization");

                lFinishedRule = lFinishedVariable;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in InitializeFinishedRule");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lFinishedRule;
        }

        public string ReturnPreconditionsAsString(List<string> pPreconditions, bool pInfixFormat = true, bool pPrefixFormat = false)
        {
            string lResultString = "";
            try
            {
                foreach (string lPrecondition in pPreconditions)
                {
                    if (lResultString.Equals(""))
                        lResultString += lPrecondition;
                    else
                        if (pInfixFormat)
                            lResultString += " and " + lPrecondition;
                        else if (pPrefixFormat)
                            if (lResultString.StartsWith("and"))
                                lResultString += lPrecondition;
                            else
                                lResultString = "and " + lResultString + " " + lPrecondition;
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ReturnPreconditionsAsString");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultString;
        }

        public bool OperationDependencyAnalysis()
        {
            bool lOperationDependencyAnalysisResult = false;
            try
            {
                string lActionDependencyTracking = "";
                //Here we pick each pair of operations and carry out the analysis on them, 
                //SAT => means that the operation pair are dependent
                //UNSAT => means the operation pair are independent
                Z3Solver lTempZ3Solver = _z3Solver;

                var OperationList = _frameworkWrapper.OperationSet;

                Z3Solver dependencySolver = new Z3Solver(_outputHandler);

                for (int i = 0; i < OperationList.Count-1; i++)
                {
                    for (int j = i+1; j < OperationList.Count; j++)
                    {
                            bool AnalysisResult = false;
                            Action lAction1 = null;
                            Action lAction2 = null;
                            Action lAction1_New = null;
                            Action lAction2_New = null;

                            Status lTempActionsDependency = Status.UNSATISFIABLE;

                            Operation Operation1 = OperationList.ElementAt(i);
                            Operation Operation2 = OperationList.ElementAt(j);

                            ///Before that we created all the operation instances right when loading the files we could read the needed operation instance
                            //HashSet<OperationInstance> lCurrentOperation1Instances = cFrameworkWrapper.getOperationInstancesForOneOperationInOneTrasition(Operation1, 0);
                            //OperationInstance lCurrentOperation1Instance = lCurrentOperation1Instances.First();

                            //HashSet<OperationInstance> lCurrentOperation2Instances = cFrameworkWrapper.getOperationInstancesForOneOperationInOneTrasition(Operation2, 0);
                            //OperationInstance lCurrentOperation2Instance = lCurrentOperation2Instances.First();

                            ///But now we have to create the needed operation instance right when it is neededo
                            OperationInstance lCurrentOperation1Instance = new OperationInstance(Operation1, "K", false, true);
                            OperationInstance lCurrentOperation2Instance = new OperationInstance(Operation2, "K", false, true);

                            OperationInstance lNextOperation1Instance = lCurrentOperation1Instance.CreateNextOperationInstance(false, true);
                            OperationInstance lNextOperation2Instance = lCurrentOperation2Instance.CreateNextOperationInstance(false, true);
                            //OperationInstance lNextOperation1Instance = cFrameworkWrapper.addOperationInstance(Operation1, "K+1");
                            //OperationInstance lNextOperation2Instance = cFrameworkWrapper.addOperationInstance(Operation2, "K+1");

                            //Case 1: Operation1.ActionI2E vs Operation2.ActionI2E
                            lAction1 = lCurrentOperation1Instance.Action_I2E;
                            lAction2 = lCurrentOperation2Instance.Action_I2E;
                            lAction1_New = lNextOperation1Instance.Action_I2E;
                            lAction2_New = lNextOperation2Instance.Action_I2E;

                            lActionDependencyTracking += "Action1: " + lAction1.Name + ", ";
                            lActionDependencyTracking += "Action2: " + lAction2.Name + Environment.NewLine;
                            lActionDependencyTracking += "Action1 Precondition: " + ReturnPreconditionsAsString(lAction1.Precondition) + Environment.NewLine;
                            lActionDependencyTracking += "Action2 Precondition: " + ReturnPreconditionsAsString(lAction2.Precondition) + Environment.NewLine;
                            lActionDependencyTracking += "Action2 Effect: " + lAction2.Effect + " -> ";

                            lTempActionsDependency = Check2ActionsDependency(dependencySolver, lCurrentOperation1Instance
                                                                            , lCurrentOperation2Instance
                                                                            , lNextOperation1Instance
                                                                            , lNextOperation2Instance
                                                                            , lAction1
                                                                            , lAction2
                                                                            , lAction1_New
                                                                            , lAction2_New);

                            if (lTempActionsDependency.Equals(Status.SATISFIABLE))
                            {
                                AnalysisResult = true;
                                lActionDependencyTracking += "Actions Dependent!" + Environment.NewLine;
                            }
                            else
                            {
                                AnalysisResult = false;
                                lActionDependencyTracking += Environment.NewLine;
                            }

                            if (AnalysisResult)
                                //This means the two analyzed actions are dependent hence they should not happen at the same time
                                DependentActionsList.Add(new DependentActions(lAction1, lAction2));

                            _z3Solver.WriteDebugFile("0", 0, lAction1.Name + "-" + lAction2.Name + "-Dependency");

                            //Case 2: Operation1.ActionI2E vs Operation2.ActionE2F
                            lAction1 = lCurrentOperation1Instance.Action_I2E;
                            lAction2 = lCurrentOperation2Instance.Action_E2F;
                            lAction1_New = lNextOperation1Instance.Action_I2E;
                            lAction2_New = lNextOperation2Instance.Action_I2E;

                            lActionDependencyTracking += "Action1: " + lAction1.Name + ", ";
                            lActionDependencyTracking += "Action2: " + lAction2.Name + " -> ";
                            lActionDependencyTracking += "Action1 Precondition: " + ReturnPreconditionsAsString(lAction1.Precondition) + Environment.NewLine;
                            lActionDependencyTracking += "Action2 Precondition: " + ReturnPreconditionsAsString(lAction2.Precondition) + Environment.NewLine;
                            lActionDependencyTracking += "Action2 Effect: " + lAction2.Effect + " -> ";

                            lTempActionsDependency = Check2ActionsDependency(dependencySolver, lCurrentOperation1Instance
                                                                            , lCurrentOperation2Instance
                                                                            , lNextOperation1Instance
                                                                            , lNextOperation2Instance
                                                                            , lAction1
                                                                            , lAction2
                                                                            , lAction1_New
                                                                            , lAction2_New);

                            if (lTempActionsDependency.Equals(Status.SATISFIABLE))
                            {
                                AnalysisResult = true;
                                lActionDependencyTracking += "Actions Dependent!" + Environment.NewLine;
                            }
                            else
                            {
                                AnalysisResult = false;
                                lActionDependencyTracking += Environment.NewLine;
                            }

                            if (AnalysisResult)
                                //This means the two analyzed actions are dependent hence they should not happen at the same time
                                DependentActionsList.Add(new DependentActions(lAction1, lAction2));

                            _z3Solver.WriteDebugFile("0", 0, lAction1.Name + "-" + lAction2.Name + "-Dependency");

                            //Case 3: Operation1.ActionE2F vs Operation2.ActionI2E
                            lAction1 = lCurrentOperation1Instance.Action_E2F;
                            lAction2 = lCurrentOperation2Instance.Action_I2E;
                            lAction1_New = lNextOperation1Instance.Action_I2E;
                            lAction2_New = lNextOperation2Instance.Action_I2E;

                            lActionDependencyTracking += "Action1: " + lAction1.Name + ", ";
                            lActionDependencyTracking += "Action2: " + lAction2.Name + " -> ";
                            lActionDependencyTracking += "Action1 Precondition: " + ReturnPreconditionsAsString(lAction1.Precondition) + Environment.NewLine;
                            lActionDependencyTracking += "Action2 Precondition: " + ReturnPreconditionsAsString(lAction2.Precondition) + Environment.NewLine;
                            lActionDependencyTracking += "Action2 Effect: " + lAction2.Effect + " -> ";

                            lTempActionsDependency = Check2ActionsDependency(dependencySolver, lCurrentOperation1Instance
                                                                            , lCurrentOperation2Instance
                                                                            , lNextOperation1Instance
                                                                            , lNextOperation2Instance
                                                                            , lAction1
                                                                            , lAction2
                                                                            , lAction1_New
                                                                            , lAction2_New);

                            if (lTempActionsDependency.Equals(Status.SATISFIABLE))
                            {
                                AnalysisResult = true;
                                lActionDependencyTracking += "Actions Dependent!" + Environment.NewLine;
                            }
                            else
                            {
                                AnalysisResult = false;
                                lActionDependencyTracking += Environment.NewLine;
                            }

                            if (AnalysisResult)
                                //This means the two analyzed actions are dependent hence they should not happen at the same time
                                DependentActionsList.Add(new DependentActions(lAction1, lAction2));

                            _z3Solver.WriteDebugFile("0", 0, lAction1.Name + "-" + lAction2.Name + "-Dependency");

                            //Case 4: Operation1.ActionE2F vs Operation2.ActionE2F
                            lAction1 = lCurrentOperation1Instance.Action_E2F;
                            lAction2 = lCurrentOperation2Instance.Action_E2F;
                            lAction1_New = lNextOperation1Instance.Action_I2E;
                            lAction2_New = lNextOperation2Instance.Action_I2E;

                            lActionDependencyTracking += "Action1: " + lAction1.Name + ", ";
                            lActionDependencyTracking += "Action2: " + lAction2.Name + " -> ";
                            lActionDependencyTracking += "Action1 Precondition: " + ReturnPreconditionsAsString(lAction1.Precondition) + Environment.NewLine;
                            lActionDependencyTracking += "Action2 Precondition: " + ReturnPreconditionsAsString(lAction2.Precondition) + Environment.NewLine;
                            lActionDependencyTracking += "Action2 Effect: " + lAction2.Effect + " -> ";

                            lTempActionsDependency = Check2ActionsDependency(dependencySolver, lCurrentOperation1Instance
                                                                            , lCurrentOperation2Instance
                                                                            , lNextOperation1Instance
                                                                            , lNextOperation2Instance
                                                                            , lAction1
                                                                            , lAction2
                                                                            , lAction1_New
                                                                            , lAction2_New);

                            if (lTempActionsDependency.Equals(Status.SATISFIABLE))
                            {
                                AnalysisResult = true;
                                lActionDependencyTracking += "Actions Dependent!" + Environment.NewLine;
                            }
                            else
                            {
                                AnalysisResult = false;
                                lActionDependencyTracking += Environment.NewLine;
                            }

                            if (AnalysisResult)
                                //This means the two analyzed actions are dependent hence they should not happen at the same time
                                DependentActionsList.Add(new DependentActions(lAction1, lAction2));

                            _z3Solver.WriteDebugFile("0", 0, lAction1.Name + "-" + lAction2.Name + "-Dependency");
                    }
                }

                _z3Solver.WriteDebugFile("0", 0, "ActionDependencyResults", lActionDependencyTracking);

                if (DependentActionsList.Count > 0)
                    lOperationDependencyAnalysisResult = true;

                _outputHandler.PrintMessageToConsole("No of Actions Dependent: " + DependentActionsList.Count);

                _z3Solver = lTempZ3Solver;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in OperationDependencyAnalysis");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lOperationDependencyAnalysisResult;
        }

        public void CarryOutNeededActionsOnPrecondition(BoolExpr pAction_Pre
                                                        , Action pAction
                                                        , BoolExpr pAction_New_Pre
                                                        , Action pAction_New
                                                        , Dictionary<string, string> pOldNewVariables)
        {
            
            try
            {
                ///BoolExpr lAction_Pre = cZ3Solver.AddBooleanExpression("ActionPre_K"); //Formula first part name
                ///BoolExpr lAction_New_Pre = cZ3Solver.AddBooleanExpression("Action1Pre_K+1");
                
                ///pOldNewVariables.Add("Action1Pre_K", "Action1Pre_K+1");

                if (pAction.Precondition.Count>0)
                {
                    BoolExpr lTempAction_Pre = CreatePreconditionNeededVariablesNReturnBoolExpr(pAction.Precondition, "K", false);

                    BoolExpr lTempAction_New_Pre = CreatePreconditionNeededVariablesNReturnBoolExpr(pAction_New.Precondition, "K+1", false);

                    string lTempAction_PreStr = GeneralUtilities.RemoveSpecialCharsFromString(lTempAction_Pre.ToString(), new char[] { '(', ')' });

                    string lTempAction_New_PreStr = GeneralUtilities.RemoveSpecialCharsFromString(lTempAction_New_Pre.ToString(), new char[] { '(', ')' });

                    string[] lParts_Action_PreStr = lTempAction_PreStr.Split(' ');

                    foreach (string lPart in lParts_Action_PreStr)
                    {
                        if (lPart.EndsWith("K"))
                            if (!pOldNewVariables.ContainsKey(lPart))
                                pOldNewVariables.Add(lPart, lPart + "+1");
                    }

                    _z3Solver.AddConstraintToSolver(_z3Solver.TwoWayImpliesOperator(pAction_Pre, lTempAction_Pre), "ActionPrecondition");

                    ///string lAction_PreNew_Name = ReplaceStringVariables(lTempAction_PreStr, pOldNewVariables);
                    ///BoolExpr lTempBoolExpr = convertComplexString2BoolExpr(lAction1PreNew_Name, "K+1");

                    _z3Solver.AddConstraintToSolver(_z3Solver.TwoWayImpliesOperator(pAction_New_Pre, lTempAction_New_Pre), "ActionNewVariables");
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CarryOutNeededActionsOnPrecondition");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public Status Check2ActionsDependency(Z3Solver lZ3Solver
                                            , OperationInstance pOperationInstance1
                                            , OperationInstance pOperationInstance2
                                            , OperationInstance pNextOperationInstance1
                                            , OperationInstance pNextOperationInstance2
                                            , Action pAction1
                                            , Action pAction2
                                            , Action pAction1_New
                                            , Action pAction2_New)
        {
            Status lResult = Status.UNSATISFIABLE;
            try
            {
                //A new Z3Solver is used, with a new Solver
               var lZ3Solver2 = new Z3Solver(_outputHandler);

                //lZ3Solver.cISolver.Reset();
                _z3Solver = lZ3Solver2;

                //This line will make copy of the created model in an external file
                _z3Solver.DebugMode = true;

                Dictionary<string, string> OldNewVariables = new Dictionary<string, string>();

                foreach (resource lCurrentResource in _frameworkWrapper.ResourceSet)
                {
                    BoolExpr lResource_k = _z3Solver.AddBooleanExpression(lCurrentResource.names + "_K");
                    BoolExpr lResource_Newk = _z3Solver.AddBooleanExpression(lCurrentResource.names + "_K+1");
                    Add2OldNewVariableList(lCurrentResource.names + "_K", lCurrentResource.names + "_K+1", OldNewVariables);
                }

                BoolExpr lTarget = _z3Solver.AddBooleanExpression("Target");
                BoolExpr lAction1Pre = _z3Solver.AddBooleanExpression("Action1Pre_K"); //Formula first part name
                BoolExpr lAction1PreNew = _z3Solver.AddBooleanExpression("Action1Pre_K+1");
                Add2OldNewVariableList("Action1Pre_K", "Action1Pre_K+1", OldNewVariables);

                BoolExpr lAction2Pre = _z3Solver.AddBooleanExpression("Action2Pre_K"); //Formula first part name
                BoolExpr lAction2PreNew = _z3Solver.AddBooleanExpression("Action2Pre_K+1");
                Add2OldNewVariableList("Action2Pre_K", "Action2Pre_K+1", OldNewVariables);

                //First we create the needed variables mentioned in the precondition expression
                ///BoolExpr lTempAction1Pre = CreatePreconditionNeededVariablesNReturnBoolExpr(pAction1.Precondition, "K");

                ///string lTempAction1PreStr = GeneralUtilities.RemoveSpecialCharsFromString(lTempAction1Pre.ToString(), new char[] { '(', ')' });

                ///cZ3Solver.AddConstraintToSolver(cZ3Solver.TwoWayImpliesOperator( lAction1Pre, lTempAction1Pre ), "Action1Precondition");

                ///BoolExpr lTempAction2Pre = CreatePreconditionNeededVariablesNReturnBoolExpr(pAction2.Precondition, "K");

                ///string lTempAction2PreStr = GeneralUtilities.RemoveSpecialCharsFromString(lTempAction2Pre.ToString(), new char[] { '(', ')' });

                ///cZ3Solver.AddConstraintToSolver(cZ3Solver.TwoWayImpliesOperator( lAction2Pre, lTempAction2Pre ), "Action2Precondition");
                CarryOutNeededActionsOnPrecondition(lAction1Pre, pAction1, lAction1PreNew, pAction1_New, OldNewVariables);

                CarryOutNeededActionsOnPrecondition(lAction2Pre, pAction2, lAction2PreNew, pAction2_New, OldNewVariables);

                //We need to build the model as a string according to Operation1 and Operation2
                //This string has to be added to the model in the form of a BoolExpr, String => BoolExpr
                //1. Define the needed variables, and add the variables to the model
                //Variables needed: 4 variables of Operation1 in transition 0 (intial, executing, finished, unused, Pre, Eff)
                //                  4 new variables of Operation1 (Operation1-PostOp2Pre_i, Operation1-PostOp2Pre_e, Operation1-PostOp2Pre_f, Operation1-PostOp2Pre_u)
                //                  4 variables of Operation2 in transition 0 (intial, executing, finished, unused, Pre, Eff)
                //                  3 NEW variables for Op1PostOp2Eff_i_0, Op1PostOp2Eff_e_0, Op1PostOp2Eff_Pre
                //                  One variable for each type of resource, as the resource tatus can change as part of any operation precondition or effect

                BoolExpr lAction1EffNew = _z3Solver.AddBooleanExpression("Action1Eff_K+1");
                BoolExpr lAction2EffNew = _z3Solver.AddBooleanExpression("Action2Eff_K+1");
                BoolExpr lOtherVarSame1 = _z3Solver.AddBooleanExpression("OtherVarSame1");
                BoolExpr lOtherVarSame2 = _z3Solver.AddBooleanExpression("OtherVarSame2");

                //Now for creating operation instance variables for the current operation 1 and in transition 0
                //HashSet<OperationInstance> lOperationInstances4Operation1 = cFrameworkWrapper.getOperationInstancesForOneOperationInOneTrasition(pOperation1, 0);


                //foreach (var lCurrentOperationInstance in lOperationInstances4Operation1)
                //{
                AddZ3ModelVariable(pOperationInstance1.OperationPreconditionVariableName);

                AddZ3ModelVariable(pOperationInstance1.InitialVariableName);

                //OperationInstance lNextOperation1Instance = getNextTransitionOperationInstance(pOperationInstance1);

                AddZ3ModelVariable(pNextOperationInstance1.InitialVariableName);
                Add2OldNewVariableList(pOperationInstance1.InitialVariableName
                                    , pNextOperationInstance1.InitialVariableName
                                    , OldNewVariables);

                AddZ3ModelVariable(pOperationInstance1.ExecutingVariableName);
                AddZ3ModelVariable(pNextOperationInstance1.ExecutingVariableName);
                Add2OldNewVariableList(pOperationInstance1.ExecutingVariableName
                                    , pNextOperationInstance1.ExecutingVariableName
                                    , OldNewVariables);

                AddZ3ModelVariable(pOperationInstance1.FinishedVariableName);
                AddZ3ModelVariable(pNextOperationInstance1.FinishedVariableName);

                Add2OldNewVariableList(pOperationInstance1.FinishedVariableName
                                    , pNextOperationInstance1.FinishedVariableName
                                    , OldNewVariables);

                AddZ3ModelVariable(pOperationInstance1.UnusedVariableName);

                BoolExpr lPickOne1 = _z3Solver.PickOneOperator(new List<string> { pOperationInstance1.InitialVariableName
                                                                                , pOperationInstance1.ExecutingVariableName
                                                                                , pOperationInstance1.FinishedVariableName
                                                                                , pOperationInstance1.UnusedVariableName });
                BoolExpr lPickOne1New = _z3Solver.PickOneOperator(new List<string> { pNextOperationInstance1.InitialVariableName
                                                                                , pNextOperationInstance1.ExecutingVariableName
                                                                                , pNextOperationInstance1.FinishedVariableName
                                                                                , pNextOperationInstance1.UnusedVariableName });
                _z3Solver.AddConstraintToSolver(lPickOne1, "PickOne1_K");
                _z3Solver.AddConstraintToSolver(lPickOne1New, "PickOne1_K+1");

                //}

                //Now for creating operation instance variables for the current operation 2 and in transition 0
                //HashSet<OperationInstance> lOperationInstances4Operation2 = cFrameworkWrapper.getOperationInstancesForOneOperationInOneTrasition(pOperation2, 0);


                //foreach (var lCurrentOperationInstance in lOperationInstances4Operation2)
                //{
                AddZ3ModelVariable(pOperationInstance2.OperationPreconditionVariableName);

                AddZ3ModelVariable(pOperationInstance2.InitialVariableName);

                //OperationInstance lNextOperation2Instance = getNextTransitionOperationInstance(pOperationInstance2);

                AddZ3ModelVariable(pNextOperationInstance2.InitialVariableName);

                Add2OldNewVariableList(pOperationInstance2.InitialVariableName
                                    , pNextOperationInstance2.InitialVariableName
                                    , OldNewVariables);

                AddZ3ModelVariable(pOperationInstance2.ExecutingVariableName);
                AddZ3ModelVariable(pNextOperationInstance2.ExecutingVariableName);

                Add2OldNewVariableList(pOperationInstance2.ExecutingVariableName
                                    , pNextOperationInstance2.ExecutingVariableName
                                    , OldNewVariables);

                AddZ3ModelVariable(pOperationInstance2.FinishedVariableName);
                AddZ3ModelVariable(pNextOperationInstance2.FinishedVariableName);

                Add2OldNewVariableList(pOperationInstance2.FinishedVariableName
                                    , pNextOperationInstance2.FinishedVariableName
                                    , OldNewVariables);

                AddZ3ModelVariable(pOperationInstance2.UnusedVariableName);

                BoolExpr lPickOne2 = _z3Solver.PickOneOperator(new List<string> { pOperationInstance2.InitialVariableName
                                                                                , pOperationInstance2.ExecutingVariableName
                                                                                , pOperationInstance2.FinishedVariableName
                                                                                , pOperationInstance2.UnusedVariableName });
                BoolExpr lPickOne2New = _z3Solver.PickOneOperator(new List<string> { pNextOperationInstance2.InitialVariableName
                                                                                , pNextOperationInstance2.ExecutingVariableName
                                                                                , pNextOperationInstance2.FinishedVariableName
                                                                                , pNextOperationInstance2.UnusedVariableName });
                _z3Solver.AddConstraintToSolver(lPickOne2, "PickOne2_K");
                _z3Solver.AddConstraintToSolver(lPickOne2New, "PickOne2_K+1");
                //}

                //2. Use the defined variables to build the formula
                //Formula: (Action1Pre) AND !(Action1PreNew) AND (Action2EffNew) AND (Operation variables which are not mentioned in Action1Pre or Action2Eff)

                ///OldNewVariables.Add("Action1Pre_K", "Action1Pre_K+1");

                ///string lAction1PreNew_Name = ReplaceStringVariables(lTempAction1PreStr, OldNewVariables);
                ///BoolExpr lTempBoolExpr = convertComplexString2BoolExpr(lAction1PreNew_Name,"K+1");
                ///cZ3Solver.AddConstraintToSolver(cZ3Solver.TwoWayImpliesOperator(lAction1PreNew, lTempBoolExpr), "Action1NewVariables");

                string lAction1EffNew_Name = ReplaceStringVariables(pAction1.Effect, OldNewVariables);
                lAction1EffNew = ConvertComplexString2BoolExpr(lAction1EffNew_Name);

                string lAction2EffNew_Name = ReplaceStringVariables(pAction2.Effect, OldNewVariables);
                lAction2EffNew = ConvertComplexString2BoolExpr(lAction2EffNew_Name);

                BoolExpr lTempBoolExpr1 = MakingOtherVariablesSameExpression(pAction1.Effect, OldNewVariables);
                _z3Solver.AddConstraintToSolver(_z3Solver.TwoWayImpliesOperator(lTempBoolExpr1, lOtherVarSame1), "OtherVarsStaySame1");

                BoolExpr lTempBoolExpr2 = MakingOtherVariablesSameExpression(pAction2.Effect, OldNewVariables);
                _z3Solver.AddConstraintToSolver(_z3Solver.TwoWayImpliesOperator(lTempBoolExpr2, lOtherVarSame2), "OtherVarsStaySame2");

                BoolExpr lRightHandSide1 = _z3Solver.AndOperator(new List<BoolExpr>(){ lAction1Pre
                                                                                        , _z3Solver.NotOperator(lAction1PreNew)
                                                                                        , lAction2Pre
                                                                                        , lAction2EffNew
                                                                                        , lOtherVarSame1});

                BoolExpr lRightHandSide2 = _z3Solver.AndOperator(new List<BoolExpr>(){ lAction2Pre
                                                                                        , _z3Solver.NotOperator(lAction2PreNew)
                                                                                        , lAction1Pre
                                                                                        , lAction1EffNew
                                                                                        , lOtherVarSame2});

                BoolExpr lTempExpr = _z3Solver.OrOperator(new List<BoolExpr>() { lRightHandSide1, lRightHandSide2 });

                //BoolExpr lFormula = cZ3Solver.TwoWayImpliesOperator(lTarget, lRightHandSide);
                BoolExpr lFormula = _z3Solver.TwoWayImpliesOperator(lTarget, lTempExpr);
                _z3Solver.AddConstraintToSolver(lFormula, "ActionsDependencyFormula");

                //The satisfiability of the model is checked
                lResult = _z3Solver.CheckModelSatisfiability("Target");
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in Check2ActionsDependency");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResult;
        }

        public void Add2OldNewVariableList(string pKey, string pValue, Dictionary<string,string> pDictionary)
        {
            try
            {
                if (!pDictionary.ContainsKey(pKey))
                    pDictionary.Add(pKey, pValue);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in Add2OldNewVariableList");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private string BuildNewVariableName(string pOldVariableName)
        {
            string lResultVariableName = "";
            try
            {
                int lIndex = pOldVariableName.IndexOf('_');
                StringBuilder sb = new StringBuilder(pOldVariableName);
                sb.Replace(sb[lIndex].ToString(), "NEW_",lIndex,1);
                lResultVariableName = sb.ToString();
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in BuildNewVariableName");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultVariableName;
        }

        private BoolExpr MakingOtherVariablesSameExpression(string pEffect, Dictionary<string, string> pOldNewVariables)
        {
            BoolExpr lResultExpression = null;
            try
            {
                //Here we have to remove any variable which is mentioned from Action1Pre from the list OldNewVariables and for the rest of the remaining variables keep them same
                List<string> lEffectVariables = ReturnListOfVariablesInAString(pEffect);
                        
                //Now we have to remove the items from the list of PreconditionVariables from the OldNewVariables
                foreach (string lEffectVariable in lEffectVariables)
                {
                    pOldNewVariables.Remove(lEffectVariable);
                }

                //Now the rest of the variables which are left in the OldNewVariablesSet make the constraint set
                foreach (var lKeyValuePair in pOldNewVariables)
                {
                    if (lKeyValuePair.Key.StartsWith("O"))
                    {
                        BoolExpr lTempExpression = null;
                        lTempExpression = _z3Solver.TwoWayImpliesOperator(lKeyValuePair.Key, lKeyValuePair.Value);

                        if (lResultExpression == null)
                            lResultExpression = lTempExpression;
                        else
                            lResultExpression = _z3Solver.AndOperator(new List<BoolExpr>() { lResultExpression, lTempExpression });
                    }
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in MakingOtherVariablesSameExpression");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultExpression;
        }

        /// <summary>
        /// Takes a series of preconditions and extracts a list of their variables
        /// </summary>
        /// <param name="pPreconditions">List of the preconditions</param>
        /// <returns>List of the precondition variables</returns>
        private List<string> ReturnListOfVariablesInAString(string pEffect)
        {
            List<string> lResultList = new List<string>();
            try
            {
                //foreach (string lPrecondition in pPreconditions)
                //{
                    string[] lParts = pEffect.Split(' ');
                    foreach (string lPart in lParts)
                    {
                        if (!lPart.Equals("and") || !lPart.Equals("or") || !lPart.Equals("not") || !lPart.Equals("=>"))
                            lResultList.Add(lPart);
                    }
                //}
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ReturnListOfVariablesInAString");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultList;
        }

        public string ReplaceStringVariables(string pOldString, Dictionary<string, string> pDictionary)
        {
            string lResult = "";
            try
            {
                string[] lParts = pOldString.Split(' ');

                foreach (string lPart in lParts)
                {
                    if (pDictionary.ContainsKey(lPart))
                        lParts[Array.IndexOf(lParts, lPart)] = pDictionary[lPart];
                }

                lResult = string.Join(" ", lParts);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ReplaceStringVariables");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResult;
        }

        public BoolExpr CreatePreconditionNeededVariablesNReturnBoolExpr(List<string> pPreconditions, string pTransitionNo = "-1", bool pAddToList = true)
        {
            BoolExpr lResultBoolExpr = null;
            try
            {
                //Here we parse every precondition in the list and add the needed variables if they are not already added
                foreach (var Precondition in pPreconditions)
                {
                    BoolExpr lTempBoolExpr = null;
                    //Each precondition can be an expression over needed variables
                    lTempBoolExpr = ConvertComplexString2BoolExpr(Precondition, pTransitionNo, false, false);

                    if (lResultBoolExpr == null)
                        lResultBoolExpr = lTempBoolExpr;
                    else
                        lResultBoolExpr = _z3Solver.AndOperator(new List<BoolExpr> { lResultBoolExpr, lTempBoolExpr });
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CreatePreconditionNeededVariables");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultBoolExpr;
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
                                            , bool pCheckingModelConstraints = false
                                            , string pExtraConfigurationRule = ""
                                            , bool pQuickAnalysis = true)
        {
            //This is the variable which holds the result of the analysis
            Status lTestResult = Status.UNKNOWN;

            ////TODO: Removed as new version
            //Each analysis is initially going to be incomplete
            ////pAnalysisComplete = false;
            try
            {

                _opSeqAnalysis = true;

                string lStrExprToBeAnalyzed = "";

                //If it is only checking the satisfiability of model constraints then there is no specific goal the check
                if (!pCheckingModelConstraints)
                {
                    if (_analysisType == Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis)
                        lStrExprToBeAnalyzed = "Deadlock_" + pTransitionNo;
                    else if (_analysisType == Enumerations.AnalysisType.OperationSelectabilityAnalysis)
                        lStrExprToBeAnalyzed = pExtraConfigurationRule;
                }

                ////Previous version: lTestResult = anlyzeModel(pTransitionNo, pAnalysisComplete, lStrExprToBeAnalyzed);
                if (_preAnalysisResult)
                {
                    if (!pAnalysisComplete)
                    {
                        if (_generalAnalysisType.Equals(Enumerations.GeneralAnalysisType.Dynamic))
                            if (!pQuickAnalysis)
                                _outputHandler.PrintMessageToConsole("Analysis No: " + pTransitionNo);
                        ////Removing unwanted code, this function was pointing to a one line function
                        ////lTestResult = analyseZ3Model(pState, pDone, lFrameworkWrapper, pStrExprToCheck);
                        lTestResult = _z3Solver.CheckSatisfiability(pTransitionNo.ToString()
                                                                    , pAnalysisComplete
                                                                    , _frameworkWrapper
                                                                    , ConvertGoal
                                                                    , ReportAnalysisDetailResult
                                                                    , ReportVariantsResult
                                                                    , ReportTransitionsResult
                                                                    , ReportAnalysisTiming
                                                                    , ReportUnsatCore
                                                                    , lStrExprToBeAnalyzed);

                        if (_analysisType.Equals(Enumerations.AnalysisType.ProductModelEnumerationAnalysis))
                            _z3Solver.WriteDebugFile("-1", pModelIndex);
                        else
                            _z3Solver.WriteDebugFile(pTransitionNo.ToString(), -1);

                            
                        if (lTestResult.Equals(Status.SATISFIABLE) && !pQuickAnalysis)
                            _z3Solver.AddModelItem2SolverAssertion(_frameworkWrapper);
                    }
                }

                //variation point
                if (StopBetweenEachTransition)
                    Console.ReadKey();

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AnalyzeProductPlatform");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _outputHandler.SetFrameworkWrapper(pFrameworkWrapper);

                if (pSatResult == Status.SATISFIABLE)
                {
                    _outputHandler = _z3Solver.PopulateOutputHandler(pState.ToString(), _outputHandler);
                }

                    switch (_analysisType)
                {
                    case Enumerations.AnalysisType.ProductModelEnumerationAnalysis:
                    case Enumerations.AnalysisType.ProductManufacturingModelEnumerationAnalysis:
                        {
                            if (pSatResult.Equals(Status.SATISFIABLE))
                            {
                                //What does the satisfiable result in this analysis mean?
                                if (ReportAnalysisResult)
                                    _outputHandler.PrintMessageToConsole("The ProductPlatform has a model to satisfy it.");

                                //Means that there has been a model which has been found
                                _outputHandler.PrintMessageToConsole("Model No " + pExtraField.ToString() + ":");
                                if (ReportVariantsResult)
                                    _outputHandler.PrintChosenVariants();
                                if (ReportTransitionsResult)
                                    _outputHandler.PrintOperationsTransitions();

                                if (CreateHTMLOutput)
                                {
                                    _outputHandler.WriteModel();
                                    _outputHandler.WriteModelNoPost();
                                }

                            }
                            else if (pSatResult.Equals(Status.UNSATISFIABLE))
                            {
                                //What does the unsatisfiable result in this analysis mean?
                                if (ReportAnalysisResult)
                                    _outputHandler.PrintMessageToConsole("The ProductPlatform has no more valid models.");

                                if (ReportAnalysisDetailResult)
                                    _outputHandler.PrintCounterExample();

                                if (CreateHTMLOutput)
                                {
                                    _outputHandler.WriteCounterExample();
                                    _outputHandler.WriteCounterExampleNoPost();
                                }

                                //cOutputHandler.printMessageToConsole("proof: {0}", iSolver.Proof);
                                //cOutputHandler.printMessageToConsole("core: ");
                                if (ReportUnsatCore)
                                    _z3Solver.ConsoleWriteUnsatCore();
                            }
                            break;
                        }
                    case Enumerations.AnalysisType.VariantSelectabilityAnalysis:
                        {
                            if (pSatResult.Equals(Status.SATISFIABLE))
                            {
                                if (pExtraField != null)
                                {
                                    variant lAnalyzedVariant = null;
                                    part lAnalyzedPart = null;

                                    if (pExtraField.GetType().FullName.Equals("variant"))
                                    {
                                        lAnalyzedVariant = (variant)pExtraField;
                                        if (ReportAnalysisResult)
                                        {
                                            _outputHandler.PrintMessageToConsole("---------------------------------------------------------------------");
                                            _outputHandler.PrintMessageToConsole("Selected Variant: " + lAnalyzedVariant.names);
                                            _outputHandler.PrintMessageToConsole(lAnalyzedVariant.names + " is Selectable.");
                                        }

                                    }
                                    else if (pExtraField.GetType().FullName.Equals("part"))
                                    {
                                        lAnalyzedPart = (part)pExtraField;
                                        if (ReportAnalysisResult)
                                        {
                                            _outputHandler.PrintMessageToConsole("---------------------------------------------------------------------");
                                            _outputHandler.PrintMessageToConsole("Selected Part: " + lAnalyzedPart.names);
                                            _outputHandler.PrintMessageToConsole(lAnalyzedPart.names + " is Selectable.");
                                        }

                                    }
                                }
                            }
                            else if (pSatResult.Equals(Status.UNSATISFIABLE))
                            {

                            }
                            break;
                        }
                    case Enumerations.AnalysisType.PartSelectabilityAnalysis:
                        {
                            if (pSatResult.Equals(Status.SATISFIABLE))
                            {
                                if (pExtraField != null)
                                {
                                    variant lAnalyzedVariant = null;
                                    part lAnalyzedPart = null;

                                    if (pExtraField.GetType().FullName.Equals("variant"))
                                    {
                                        lAnalyzedVariant = (variant)pExtraField;
                                        if (ReportAnalysisResult)
                                        {
                                            _outputHandler.PrintMessageToConsole("---------------------------------------------------------------------");
                                            _outputHandler.PrintMessageToConsole("Selected Variant: " + lAnalyzedVariant.names);
                                            _outputHandler.PrintMessageToConsole(lAnalyzedVariant.names + " is Selectable.");
                                        }

                                    }
                                    else if (pExtraField.GetType().FullName.Equals("part"))
                                    {
                                        lAnalyzedPart = (part)pExtraField;
                                        if (ReportAnalysisResult)
                                        {
                                            _outputHandler.PrintMessageToConsole("---------------------------------------------------------------------");
                                            _outputHandler.PrintMessageToConsole("Selected Part: " + lAnalyzedPart.names);
                                            _outputHandler.PrintMessageToConsole(lAnalyzedPart.names + " is Selectable.");
                                        }

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
                                    if (ReportAnalysisResult)
                                    {
                                        //Initial info
                                        _outputHandler.PrintMessageToConsole("---------------------------------------------------------------------");
                                        _outputHandler.PrintMessageToConsole("Selected Variant: " + lAnalyzedVariant.names);

                                        //Analysis Result
                                        //if it does hold, then there is a configuration which is valid and this current variant is not present in it
                                        _outputHandler.PrintMessageToConsole("There DOES exist a valid configuration which does not include " + lAnalyzedVariant.names + ".");
                                    }
                                }
                            }
                            else if (pSatResult.Equals(Status.UNSATISFIABLE))
                            {
                                if (pExtraField != null)
                                {
                                    part lAnalyzedVariant = _frameworkWrapper.PartLookupByName(pExtraField.ToString());
                                    _outputHandler.PrintMessageToConsole("All valid configurations DO include " + lAnalyzedVariant.names + ".");
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
                                    if (ReportAnalysisResult)
                                    {
                                        //Initial info
                                        _outputHandler.PrintMessageToConsole("----------------------------------------------------------------");
                                        _outputHandler.PrintMessageToConsole("Analysing operation named: " + lAnalyzedOperationName);

                                        //Analysis Result
                                        _outputHandler.PrintMessageToConsole(lAnalyzedOperationName + " is selectable.");
                                    }
                                }
                            }
                            else if (pSatResult.Equals(Status.UNSATISFIABLE))
                            {
                                //Here we check if this operation is in active operation or not, meaning does a variant use this operation for its assembly or not
                                Operation lTempOperation = _frameworkWrapper.OperationLookupByName(pExtraField.ToString());
                                //Initial info
                                _outputHandler.PrintMessageToConsole("----------------------------------------------------------------");
                                _outputHandler.PrintMessageToConsole("Analysing operation named: " + lTempOperation.Name);

                                //This is when we want to report only the operation name
                                _outputHandler.PrintMessageToConsole("Operation " + lTempOperation.Name + " is NOT selectable!");
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
                                    _outputHandler.PrintMessageToConsole("There DOES exist a configuration in which " + lOperationName + " is in an UNUSED state!");
                                }
                            }
                            else if (pSatResult.Equals(Status.UNSATISFIABLE))
                            {
                                if (pExtraField != null)
                                {
                                    Operation lTempOperation = _frameworkWrapper.OperationLookupByName(pExtraField.ToString());
                                    string lOperationName = lTempOperation.Name;

                                    _outputHandler.PrintMessageToConsole("All valid configurations DO include " + lOperationName + ".");
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
                                    if (ReportAnalysisDetailResult)
                                        _outputHandler.PrintCounterExample();

                                    if (CreateHTMLOutput)
                                    {
                                        _outputHandler.WriteCounterExample();
                                        _outputHandler.WriteCounterExampleNoPost();
                                    }

                                    //cOutputHandler.printMessageToConsole("proof: {0}", iSolver.Proof);
                                    //cOutputHandler.printMessageToConsole("core: ");
                                    if (ReportUnsatCore)
                                        _z3Solver.ConsoleWriteUnsatCore();
                                }
                                else if (pSatResult.Equals(Status.UNSATISFIABLE))
                                {
                                    //If the result of the analysis is UNSAT meaning that there was NO deadlock found

                                    //Print and writes an output file showing the result of a finished test
                                    if (ReportAnalysisDetailResult)
                                    {
                                        _outputHandler.PrintMessageToConsole("Model No " + pState + ":");
                                        if (ReportVariantsResult)
                                        {
                                            _outputHandler.PrintChosenVariants();
                                        }
                                        if (ReportTransitionsResult)
                                            _outputHandler.PrintOperationsTransitions();
                                    }

                                    if (CreateHTMLOutput)
                                    {
                                        _outputHandler.WriteFinished();
                                        _outputHandler.WriteFinishedNoPost();
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
                _outputHandler.PrintMessageToConsole("error in ReportSolverResults");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }
        
        /// <summary>
        /// This function converts the static part of the product platform, i.e. variants, followed by the group cardinalities
        /// </summary>
        private void ConvertProductPlatform(bool pOptimizer = false)
        {
            try
            {
                ConvertFVariants2Z3Variants();

                ConvertFParts2Z3Parts();

                ConvertItemUsageRulesConstraints(pOptimizer);

                //formula 2
                ProduceVariantGroupGCardinalityConstraints(pOptimizer);

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertProductPlatform");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void ConvertItemUsageRulesConstraints(bool pOptmizer = false)
        {
            try
            {

                foreach (itemUsageRule lItemUsageRule in _frameworkWrapper.ItemUsageRuleSet)
                {
                    ////Previous version: In this version it was variant -> parts
                    ////first we have to make a string of the parts anded to gether
                    //List<string> lPartNames = new List<string>();
                    //foreach (var lPart in lItemUsageRule.getParts())
                    //{
                    //    lPartNames.Add(lPart.names);
                    //}

                    //BoolExpr lRightHandSide = cZ3Solver.AndOperator(lPartNames);

                    //BoolExpr lLeftHandSide = (BoolExpr) cZ3Solver.FindExprInExprSet(lItemUsageRule.getVariant().names);

                    //BoolExpr lConstraint = cZ3Solver.TwoWayImpliesOperator(lLeftHandSide, lRightHandSide);
                    //cZ3Solver.AddConstraintToSolver(lConstraint, "ItemUsageRule");

                    BoolExpr lRightHandSide = (BoolExpr)_z3Solver.FindExprInExprSet(lItemUsageRule.getPart().names);

                    BoolExpr lLeftHandSide = ConvertComplexString2BoolExpr(lItemUsageRule.getVariantExp());

                    BoolExpr lConstraint = _z3Solver.TwoWayImpliesOperator(lLeftHandSide, lRightHandSide);
                    _z3Solver.AddConstraintToSolver(lConstraint, "ItemUsageRule");

                    if (pOptmizer)
                        _z3Solver.AddConstraintToOptimizer(lConstraint, "ItemUsageRule");
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertItemUsageRulesConstraints");
                _outputHandler.PrintMessageToConsole(ex.Message);
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


        private BoolExpr InitializeOperationProgressionRule(int pTransitionNo)
        {
            BoolExpr lResultExpr = null;
            try
            {
                /*
                 * NOT

                    (

                    (O1_i_t <-> O1_i_t+1) AND (O1_e_t <-> O1_e_t+1)

                    AND

                    ...

                    AND

                    (On_i_t <-> On_i_t+1) AND (On_e_t <-> On_e_t+1)

                    )
                 */

                BoolExpr lOverallOperationProgressionRule = null;
                List<BoolExpr> lSingleOperationProressionList = new List<BoolExpr>();
                BoolExpr lAllOperationProgression = null;

                BoolExpr lProgressionRule = _z3Solver.AddBooleanExpression("ProgressionRule_" + pTransitionNo);

                HashSet<Operation> lAbstractOperations = _frameworkWrapper.OperationSet;
                //var lOperationInstanceSet = cFrameworkWrapper.getOperationInstancesInOneTransition(pTransitionNo.ToString());
                List<OperationInstance> lOperationInstanceSet = new List<OperationInstance>();
                foreach (Operation lCurrentAbstractOperation in lAbstractOperations)
                {
                    lOperationInstanceSet.Add(lCurrentAbstractOperation.GetOperationInstanceForTransition(pTransitionNo));
                }

                foreach (var lCurrentOperationInstance in lOperationInstanceSet)
                {
                    //(O1_i_t <-> O1_i_t+1) AND (O1_e_t <-> O1_e_t+1)
                    ///OperationInstance lNextTransitionOperationInstance = getNextTransitionOperationInstance(lCurrentOperationInstance);
                    OperationInstance lNextTransitionOperationInstance = lCurrentOperationInstance.NextOperationInstance();
                    BoolExpr lFirstPart = _z3Solver.TwoWayImpliesOperator(lCurrentOperationInstance.InitialVariable
                                                                , lNextTransitionOperationInstance.InitialVariable);
                    BoolExpr lSecondPart = _z3Solver.TwoWayImpliesOperator(lCurrentOperationInstance.ExecutingVariable
                                                                , lNextTransitionOperationInstance.ExecutingVariable);
                    BoolExpr lCurrentOperationExpression = _z3Solver.AndOperator(new List<BoolExpr>() { lFirstPart, lSecondPart });

                    lSingleOperationProressionList.Add(lCurrentOperationExpression);
                }

                lAllOperationProgression = _z3Solver.AndOperator(lSingleOperationProressionList);

                lOverallOperationProgressionRule = _z3Solver.NotOperator(lAllOperationProgression);
                _z3Solver.AddTwoWayImpliesOperator2Constraints(lProgressionRule, lOverallOperationProgressionRule, "OperationProgression");


                lResultExpr = lProgressionRule;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertOperationProgressionRule");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }

        /// <summary>
        /// This function converts operation relations (precedence rules)
        /// and the relationship between operations and variants
        /// </summary>
        /// <param name="pState"></param>
        private void ConvertOperationsPrecedenceRules(int pTransitionNo)
        {
            try
            {
                //TODO: Here we have to check if the operations have really been converted.

                if (_needPreAnalysis && _currentTransitionNumber.Equals(0))
                    _preAnalysisResult = _frameworkWrapper.CheckPreAnalysis();

                if (_preAnalysisResult)
                {
                    IntializeOperationTriggerRules();
                    //formula 5 and New Formula
                    //5.1: (O_I_k_j and Pre_k_j) => O_E_k_j+1 or O_I_k_j+1
                    //5.2: O_I_k_j and (not Pre_k_j) => (O_I_k_j <=> O_I_k_j+1)
                    //5.3: XOR O_I_k_j O_E_k_j O_F_k_j O_U_k_j
                    //5.4: (O_E_k_j AND Post_k_j) => O_F_k_j+1 or O_E_k_j+1
                    //5.5: O_E_k_j and (not Post_k_j) <=> (O_E_k_j <=> O_E_k_j+1)
                    //5.6: O_U_k_j => O_U_k_j+1
                    //5.7: O_F_k_j => O_F_k_j+1
                    //5.8: Ai2e <=> (O_I_k_j AND O_E_k_j+1)
                    //5.9: Ae2f <=> (O_E_k_j AND O_F_k_j+1)
                    //TODO: Bring the comment for formula 6 from the follwing function
                    ConvertFOperationsPrecedenceRulesNStatusControl2Z3ConstraintNewVersion(pTransitionNo);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertOperationsPrecedenceRules");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void IntializeOperationTriggerRules()
        {
            try
            {
                HashSet<Operation> lOperationSet = _frameworkWrapper.OperationSet;

                foreach (Operation lCurrentOperation in lOperationSet)
                {
                    if (_currentTransitionNumber == 0)
                        DefiningNInitializingOperationTriggerAttributes(lCurrentOperation);
                }


            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertOperationsPrecedenceRules");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }


        /// <summary>
        /// This function converts the resources and the operation resource relationships
        /// </summary>
        private void ConvertResourcesNOperationResourceRelations(bool pOptimizer = false)
        {
            try
            {
                if (_preAnalysisResult)
                {
                    //New formulas for implementing resources
                    if (_frameworkWrapper.ResourceSet.Count != 0)
                    {
                        ConvertFResource2Z3Constraints(pOptimizer);
                        CheckFOperationExecutabilityWithCurrentResourcesUsingZ3Constraints(pOptimizer);
                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertResourcesNOperationResourceRelations");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// This function converts the goal(s)
        /// </summary>
        /// <param name="pState"></param>
        /// <param name="pDone"></param>
        ////private void convertGoal(int pState, bool pDone)
        private void ConvertExistenceOfDeadlockGoal(int pState)
        {
            //TODO: this function is not needed and the lower level function down below can be called straight away
            try
            {
                //TODO: these two conditions have to be moved to one level higher and this function should be removed.
                if (_preAnalysisResult)
                {
                    if (_opSeqAnalysis)
                    {
                        ////Removed when seperating the model building from model analysis
                        ////Might be added by runa, don't know why!
                        ////if (!pDone)
                            //formula 7 and 8
                            InitializeDeadlockRule(pState);
                    }
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertGoal");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// This function analyzes the result of the goals
        /// </summary>
        /// <param name="pState">The transition number which we are in</param>
        /// <param name="pDone">If the analysis is finished or not</param>
        /// <param name="pStrExprToCheck">If a special epression needs to be checked</param>
        /// <returns>Analysis result of the goal</returns>
        private Status AnlyzeModel(int pState, bool pDone, string pStrExprToCheck = "")
        {
            Status lTestResult = Status.UNKNOWN;
            try
            {
                if (_preAnalysisResult)
                {
                    if (!pDone)
                    {
                        if (_generalAnalysisType.Equals(Enumerations.GeneralAnalysisType.Dynamic))
                            _outputHandler.PrintMessageToConsole("Analysis No: " + pState);
                        ////Removing unwanted code, this function was pointing to a one line function
                        ////lTestResult = analyseZ3Model(pState, pDone, lFrameworkWrapper, pStrExprToCheck);
                        lTestResult = _z3Solver.CheckSatisfiability(pState.ToString()
                                                                    , pDone
                                                                    , _frameworkWrapper
                                                                    , ConvertGoal
                                                                    , ReportAnalysisDetailResult
                                                                    , ReportVariantsResult
                                                                    , ReportTransitionsResult
                                                                    , ReportAnalysisTiming
                                                                    , ReportUnsatCore
                                                                    , pStrExprToCheck);

                        _z3Solver.WriteDebugFile(pState.ToString(), -1);
                    }
                    else
                    {
                        ////TODO: If the analysis is finished why should it check the satisfiablity again???????
                        ////TODO: Have to remember the reason behind this else.

                        _outputHandler.PrintMessageToConsole("Finished: ");
                        ////Removing unwanted code, this function was pointing to a one line function
                        ////lTestResult = analyseZ3Model(pState, pDone, lFrameworkWrapper, pStrExprToCheck);
                        lTestResult = _z3Solver.CheckSatisfiability(pState.ToString()
                                                                    , pDone
                                                                    , _frameworkWrapper
                                                                    , ConvertGoal
                                                                    , ReportAnalysisDetailResult
                                                                    , ReportVariantsResult
                                                                    , ReportTransitionsResult
                                                                    , ReportAnalysisTiming
                                                                    , ReportUnsatCore
                                                                    , pStrExprToCheck);

                        _z3Solver.WriteDebugFile(pState.ToString(), -1);
                    }
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in anlyzeModel");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lTestResult;
        }

        private void ConvertFResource2Z3Constraints(bool pOptimizer = false)
        {
            try
            {
                //List<resource> lResourceList = cFrameworkWrapper.ResourceList;
                foreach (resource lResource in _frameworkWrapper.ResourceSet)
                {
                    string lResourceName = lResource.names;
                    foreach (Tuple<string,string,string> lAttribute in lResource.attributes)
                    {
                        ConvertFResourceAttribute2Z3Constraint(lResourceName, lAttribute, pOptimizer);


                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertFResource2Z3Constraints");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void ConvertFResourceAttribute2Z3Constraint(string pResourceName, Tuple<string,string,string> pAttribute, bool pOptimizer = false)
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
                        _z3Solver.AddIntegerExpression(lAttributeName);
                        //IntExpr lExprVariable = cZ3Solver.FindIntExpressionUsingName(lAttributeName);
                        Expr lExprVariable = _z3Solver.FindExprInExprSet(lAttributeName);

                        _z3Solver.AddEqualOperator2Constraints(lExprVariable
                                                                , int.Parse(lAttributeValue)
                                                                , "Attribute_Value"
                                                                , pOptimizer);

                        break;
                    default:
                        //The default case is boolean variables
                        _z3Solver.AddBooleanExpression(lAttributeName);
                        break;
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertFResourceAttribute2Z3Constraint");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void CheckFOperationExecutabilityWithCurrentResourcesUsingZ3Constraints(bool pOptimizer = false)
        {
            try
            {

                //foreach (Operation lActiveOperation in cFrameworkWrapper.ActiveOperationSet)
                foreach (Operation lActiveOperation in _frameworkWrapper.OperationSet)
                {
//                    List<string> lPossibleResourceVariablesForActiveOperation = new List<string>();
                    List<string> lUseResourceVariablesForActiveOperation = new List<string>();
                    
                    //This variable shows if the current operation can be run with at least one resource
                    string lPossibleToRunActiveOperationName = "Possible_to_run_" + lActiveOperation.Name;
                    _z3Solver.AddBooleanExpression(lPossibleToRunActiveOperationName);

                    foreach (resource lActiveResource in _frameworkWrapper.ResourceSet)
                    {
                        //This variable shows if the current operation CAN be run with the current resource
                        string lPossibleToUseResource4OperationName = "Possible_to_use_" + lActiveResource.names + "_for_" + lActiveOperation.Name;
                        _z3Solver.AddBooleanExpression(lPossibleToUseResource4OperationName);
//                        lPossibleResourceVariablesForActiveOperation.Add(lPossibleToUseResource4OperationName);

                        //This variable shows if the current operation WILL be run with the current resource
                        string lUseResource4OperationName = "Use_" + lActiveResource.names + "_for_" + lActiveOperation.Name;
                        _z3Solver.AddBooleanExpression(lUseResource4OperationName);
                        lUseResourceVariablesForActiveOperation.Add(lUseResource4OperationName);

                        //formula 6.1
                        //Possible_to_use_ActiveResource_for_ActiveOperation <-> Operation.Requirement
                        string lActiveOperationRequirements = lActiveOperation.Requirement;
                        if (lActiveOperationRequirements != "")
                        {
                            //Active operation has requirements defined

                            HashSet<resource> lOperationChosenResources = _frameworkWrapper.ReturnOperationChosenResource(lActiveOperation.Name);

                            if (lOperationChosenResources.Contains(lActiveResource))
                            {
                                //This active resource is one of the resources that can run this operation
                                BoolExpr lActiveOperationRequirementExpr = ReturnFExpression2Z3Constraint(lActiveOperationRequirements);
                                /*cZ3Solver.AddTwoWayImpliesOperator2Constraints(cZ3Solver.FindBoolExpressionUsingName(lPossibleToUseResource4OperationName)
                                                                            , lActiveOperationRequirementExpr
                                                                            , "formula 6.1");*/
                                _z3Solver.AddTwoWayImpliesOperator2Constraints((BoolExpr)_z3Solver.FindExprInExprSet(lPossibleToUseResource4OperationName)
                                                                            , lActiveOperationRequirementExpr
                                                                            , "formula 6.1"
                                                                            , pOptimizer);
                            }
                            else
                            {
                                //This active resource is not one of the resources that can run this operation
                                _z3Solver.AddNotOperator2Constraints(lPossibleToUseResource4OperationName
                                                                        , "formula 6.1"
                                                                        , pOptimizer);
                            }
                        }
                        else
                            //Active operation has no requirements defined, hence it is always possible to run
                            _z3Solver.AddConstraintToSolver((BoolExpr)_z3Solver.FindExprInExprSet(lPossibleToRunActiveOperationName)
                                                            , "formula 6.1"
                                                            , pOptimizer);


                        //formula 6.2
                        // Use_ActiveResource_ActiveOperation -> Possible_to_use_ActiveResource_for_ActiveOperation
                        _z3Solver.AddImpliesOperator2Constraints((BoolExpr)_z3Solver.FindExprInExprSet(lUseResource4OperationName)
                                                                , (BoolExpr)_z3Solver.FindExprInExprSet(lPossibleToUseResource4OperationName)
                                                                , "formula 6.2"
                                                                , pOptimizer);
                    }

                    /////////////////////////////////////////////////////////
                    //formula 6.3
                    //This formula makes sure this operation can be run by ONLY one resource

                    //Possible_to_run_ActiveOperation -> Possible_to_use_Resource1_for_ActiveOperation or Possible_to_use_Resource2_for_ActiveOperation or ...

                    //                    lPossibleToRunOperationVariableNames.Add(lPossibleToRunActiveOperationName);
                    BoolExpr lPossibleToRunActiveOperation = (BoolExpr)_z3Solver.FindExprInExprSet(lPossibleToRunActiveOperationName);
                    _z3Solver.AddTwoWayImpliesOperator2Constraints(lPossibleToRunActiveOperation
                                                                , _z3Solver.PickOneOperator(lUseResourceVariablesForActiveOperation)
                                                                , "formula6.3"
                                                                , pOptimizer);
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
                _outputHandler.PrintMessageToConsole("error in convertFResourceOperationMapping2Z3Constraints");
                _outputHandler.PrintMessageToConsole(ex.Message);
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

        public void ConvertFVariants2Z3Variants()
        {
            try
            {
                foreach (variant lVariant in _frameworkWrapper.VariantSet)
                    _z3Solver.AddBooleanExpression(lVariant.names);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertFVariants2Z3Variants");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }

        }

        public void ConvertFParts2Z3Parts()
        {
            try
            {
                foreach (part lPart in _frameworkWrapper.PartSet)
                    _z3Solver.AddBooleanExpression(lPart.names);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertFParts2Z3Parts");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
        public void DefiningNInitializingOperationTriggerAttributes(Operation pSpecificOperation, string pExtraDescription = "")
        {
            try
            {
                //Defining trigger variable
                pSpecificOperation.OperationTriggerVariable = AddZ3ModelVariable(pSpecificOperation.OperationTriggerVariableName, null, pExtraDescription);
                //Initializing trigger variable (only if the transition number is 0)

                string lConstraintSource = "";
                if (!pExtraDescription.Equals(""))
                {
                    lConstraintSource = pExtraDescription + " - Operation_Trigger_Initializing";
                    //O_Trigger <=> Trigger expression
                    _z3Solver.AddTwoWayImpliesOperator2Constraints(pSpecificOperation.OperationTriggerVariable
                                                                    , ConvertComplexString2BoolExpr(pSpecificOperation.Trigger)
                                                                    , lConstraintSource
                                                                    , true);

                }
                else
                {
                    lConstraintSource = "Operation_Trigger_Initializing";
                    //O_Trigger <=> Trigger expression
                    _z3Solver.AddTwoWayImpliesOperator2Constraints(pSpecificOperation.OperationTriggerVariable
                                                                    , ConvertComplexString2BoolExpr(pSpecificOperation.Trigger)
                                                                    , lConstraintSource
                                                                    , false);

                }


            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefiningNInitializingOperationTriggerAttributes");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void DefiningNInitializingOperationTriggerAttributes()
        {
            try
            {
                var lOperations = _frameworkWrapper.OperationSet;

                foreach (var lOperation in lOperations)
                {
                    //Defining trigger variable
                    lOperation.OperationTriggerVariable = AddZ3ModelVariable(lOperation.OperationTriggerVariableName);
                    //Initializing trigger variable (only if the transition number is 0)

                    //O_Trigger <=> Trigger expression
                    _z3Solver.AddTwoWayImpliesOperator2Constraints(lOperation.OperationTriggerVariable
                                                                    , ConvertComplexString2BoolExpr(lOperation.Trigger)
                                                                    , "Operation_Trigger_Initializing");

                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in DefiningNInitializingOperationTriggerAttributes");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// In this function we go over all the defined operations and for the active operations we define current state and next state variables in Z3 model
        /// For the inactive operations we only define current state variables in the Z3 model
        /// </summary>
        /// <param name="pState"></param>
        public void ConvertFOperations2Z3Operations(int pTransitionNo)
        {
            try
            {
                // Here we have to loop over all operations and operation instances and create the needed variables

                var lOperations = _frameworkWrapper.OperationSet;

                foreach (var lOperation in lOperations)
                {
                    OperationInstance lCurrentOperationInstance = null;
                    int lCurrentTransitionNo = pTransitionNo;
                    int lNextTransitionNo = lCurrentTransitionNo + 1;
                    for (int i = lCurrentTransitionNo; i <= lNextTransitionNo; i++)
                    {
                        //Now for creating operation instance objects in the FrameworkWrapper and Z3Model variables
                        if (i == 0)
                            lCurrentOperationInstance = new OperationInstance(lOperation, pTransitionNo.ToString());
                        else if (lCurrentOperationInstance == null)
                            //I am pretty sure we can only have one operation instance for each operation in each transition!!!!!
                            lCurrentOperationInstance = _frameworkWrapper.GetOperationInstancesForOneOperationInOneTrasition(lOperation, lCurrentTransitionNo).First();
                        else
                            lCurrentOperationInstance = lCurrentOperationInstance.CreateNextOperationInstance();

                        ////These two lines are now carried out in the Static part of the model making
                        ////lOperation.OperationTriggerVariable = AddZ3ModelVariable(lOperation.OperationTriggerVariableName);
                        ////lOperation.OperationRequirementVariable = AddZ3ModelVariable(lOperation.OperationRequirementVariableName);

                        //Placeholder to add any additional dictionaries on operation instances
                        //var lKeyTuple = new Tuple<string, String>(lOperation.Name, pTransitionNo.ToString());
                        //if (!cFrameworkWrapper.OperationInstanceDictionary.ContainsKey(lKeyTuple))
                        //{
                        //    cFrameworkWrapper.OperationInstanceDictionary.Add(lKeyTuple, lCurrentOperationInstance);
                        //}

                        lCurrentOperationInstance.OperationPreconditionVariable = AddZ3ModelVariable(lCurrentOperationInstance.OperationPreconditionVariableName);
                        lCurrentOperationInstance.OperationPostconditionVariable = AddZ3ModelVariable(lCurrentOperationInstance.OperationPostconditionVariableName);

                        lCurrentOperationInstance.InitialVariable = AddZ3ModelVariable(lCurrentOperationInstance.InitialVariableName);
                        lCurrentOperationInstance.ExecutingVariable = AddZ3ModelVariable(lCurrentOperationInstance.ExecutingVariableName);
                        lCurrentOperationInstance.FinishedVariable = AddZ3ModelVariable(lCurrentOperationInstance.FinishedVariableName);
                        lCurrentOperationInstance.UnusedVariable = AddZ3ModelVariable(lCurrentOperationInstance.UnusedVariableName);

                        lCurrentOperationInstance.Action_I2E.Variable = AddZ3ModelVariable(lCurrentOperationInstance.Action_I2E_VariableName);
                        lCurrentOperationInstance.Action_E2F.Variable = AddZ3ModelVariable(lCurrentOperationInstance.Action_E2F_VariableName);
                    }

                }

                if (_currentTransitionNumber.Equals(0))
                {
                    //forula 4 - Setting operation status if they were picked or not
                    // C = (BIG AND) (O_I_j_0 <=> O_Trigger) AND (! O_e_j_0) AND (! O_f_j_0) AND (O_u_j_0 <=> (! O_Trigger))
                    InitializeFOperation2Z3Constraints();

                    ////computeOperationsToResourceMap();

                }

                //formula x - Setting constraints for operation resources
                //Op1.Req = Res1, Op2.Req = Res2, Op3.Req = Res3 
                //Res1 => Op1, Op3 ; Res2 => Op2 
                //Add constraint: ZeroOrOne(Op1,Op2)
                AddOperationResourcesConstraints();
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertFOperations2Z3Operations");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void ComputeOperationsToResourceMap()
        {
            try
            {
                HashSet<resource> lResourceList = _frameworkWrapper.ResourceSet;

                if (lResourceList.Count >0)
                {
                    // Create a list of operations for every resource
                    foreach (var lResource in lResourceList)
                    {
                        //This part creates a list of all resources with an empty list of the operations related to each resource
                        List<Operation> lOperations = new List<Operation>();

                        _resourcesOperations.Add(lResource.names, lOperations);
                    }

                    HashSet<Operation> lOperationSet = _frameworkWrapper.OperationSet;

                    // If an operations has a resource add it to the list of that resource
                    foreach (Operation lCurrentAbstractOperation in lOperationSet)
                    {
                        string lNeededResource = lCurrentAbstractOperation.Resource;
                        _resourcesOperations[lNeededResource].Add(lCurrentAbstractOperation);
                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ComputeOperationsToResourceMap");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }

        }

        private void AddOperationResourcesConstraints()
        {
            //formula x - Setting constraints for operation resources
            //Op1.Req = Res1, Op2.Req = Res2, Op3.Req = Res1
            //Res1 => Op1, Op3 ; Res2 => Op2 
            //Add constraint: ZeroOrOne(Op1_e,Op2_e)
            try
            {
  /*
                var lResourcesOperations = new Dictionary<string, List<Operation>>();

                HashSet<resource> lResourceList = cFrameworkWrapper.ResourceSet;
                
                // Create a list of operations for every resource
                foreach (var lResource in lResourceList)
                {
                    //This part creates a list of all resources with an empty list of the operations related to each resource
                    List<Operation> lOperations = new List<Operation>();

                    lResourcesOperations.Add(lResource.names, lOperations);
                }

                HashSet<Operation> lOperationSet = cFrameworkWrapper.OperationSet;

                // If an operations has a resource add it to the list of that resource
                foreach (Operation lCurrentAbstractOperation in lOperationSet)
                {
                    string lNeededResource = lCurrentAbstractOperation.Resource;
                    lResourcesOperations[lNeededResource].Add(lCurrentAbstractOperation);
                }
*/
/*
                if (lResourceList.Count>0)
                {
                    foreach (var lResource in lResourceList)
                    {
                        //This part creates a list of all resources with an empty list of the operations related to each resource
                        List<Operation> lOperations = new List<Operation>();

                        lResourcesOperations.Add(lResource.names, lOperations);
                    }

                    //HashSet<OperationInstance> lOperationInstanceList = cFrameworkWrapper.OperationInstanceSet;
                    HashSet<Operation> lOperationSet = cFrameworkWrapper.OperationSet;

                    foreach (Operation lCurrentAbstractOperation in lOperationSet)
                    {
                        /*
                        foreach (OperationInstance lCurrentOperationInstance in lCurrentAbstractOperation.MyOperationInstances)
                        {
                            if (lCurrentOperationInstance.TransitionNumber.Equals(cCurrentTransitionNumber.ToString()))
                            {
                                string lNeededResource = lCurrentOperationInstance.AbstractOperation.Resource;
                                lResourcesOperations[lNeededResource].Add(lCurrentOperationInstance.ExecutingVariable);
                            }
                            
                        }
  */
                   //
                    //Add constraint: ZeroOrOne(Op1,Op2)
                    foreach (KeyValuePair<string, List<Operation>> lCurrentKVP in _resourcesOperations)
                    {
                        
                        if (lCurrentKVP.Value.Count > 1)
                        { // More than two operations refer to this resource

                            //Add a constraint on the operations in this list so at most one of them is executed at the same time
                            BoolExpr lZeroConstraint = null;
                            BoolExpr lOneConstraint = null;
                            BoolExpr lConstraint = null;
                            if (_currentTransitionNumber == 0)
                            {
                                lZeroConstraint = _z3Solver.PickZeroResource(lCurrentKVP.Value, 0);
                                lOneConstraint = _z3Solver.PickOneResource(lCurrentKVP.Value, 0);
                                lConstraint = _z3Solver.OrOperator(new List<BoolExpr>() { lZeroConstraint, lOneConstraint });
                                _z3Solver.AddConstraintToSolver(lConstraint, "formula X");
                            }

                            if (_currentTransitionNumber < _maxNumberOfTransitions)
                            {
                                lZeroConstraint = _z3Solver.PickZeroResource(lCurrentKVP.Value, _currentTransitionNumber + 1);
                                lOneConstraint = _z3Solver.PickOneResource(lCurrentKVP.Value, _currentTransitionNumber + 1);
                                lConstraint = _z3Solver.OrOperator(new List<BoolExpr>() { lZeroConstraint, lOneConstraint });
                                _z3Solver.AddConstraintToSolver(lConstraint, "formula X");
                            }


                        }

                    }
                }


            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in addOperationResourcesConstraints");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private BoolExpr AddZ3ModelVariable(string pVariableName, Z3Solver pZ3Solver = null, string pExtraDescription = "")
        {
            BoolExpr lResultVariable = null;
            try
            {
                if (pZ3Solver != null)
                    lResultVariable = pZ3Solver.AddBooleanExpression(pVariableName, pExtraDescription);
                else
                    lResultVariable = _z3Solver.AddBooleanExpression(pVariableName, pExtraDescription);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddZ3ModelVariable");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultVariable;
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

        public void ProduceVariantGroupGCardinalityConstraints(bool pOptimizer = false)
        {
            try
            {
                //Formula 2
                //List<variantGroup> localVariantGroupsList = cFrameworkWrapper.VariantGroupList;

                foreach (variantGroup lVariantGroup in _frameworkWrapper.VariantGroupSet)
                {
                    MakeGCardinalityConstraint(lVariantGroup, pOptimizer);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in produceVariantGroupGCardinalityConstraints");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        //TODO: We need an enum for the Variant Group Cardinality to be used here and also used where the Variant Groups are created from the input files
        public void MakeGCardinalityConstraint(variantGroup pVariantGroup, bool pOptimizer = false)
        {
            try
            {
                //Formula 2
                List<variant> lVariants = pVariantGroup.variants;

                List<string> lVariantNames = new List<string>();
                foreach (variant lVariant in lVariants)
                    lVariantNames.Add(lVariant.names);

                if (lVariantNames.Count.Equals(1))
                {
                    BoolExpr lTempBoolExpr = (BoolExpr)_z3Solver.FindExprInExprSet(lVariantNames.First());
                    _z3Solver.AddConstraintToSolver(lTempBoolExpr, "GroupCardinality");

                    if (pOptimizer)
                        _z3Solver.AddConstraintToOptimizer(lTempBoolExpr, "GroupCardinality");
                }
                else
                {
                    switch (pVariantGroup.gCardinality)
                    {
                        case "choose exactly one":
                            {
                                _z3Solver.AddPickOneOperator2Constraints(lVariantNames, "GroupCardinality", pOptimizer);

                                break;
                            }
                        case "choose at least one":
                            {
                                _z3Solver.AddOrOperator2Constraints(lVariantNames, "GroupCardinality", pOptimizer);

                                break;
                            }
                        case "choose all":
                            {
                                _z3Solver.AddAndOperator2Constraints(lVariantNames, "GroupCardinality", pOptimizer);

                                break;
                            }
                        case "choose zero or more":
                            {
                                //In this case there really doesn't need to be any constraint because all options are allowed
                                break;
                            }
                        case "choose zero or one":
                            {
                                _z3Solver.AddPickZeroOrOneOperator2Constraints(lVariantNames, "GroupCardinality", pOptimizer);
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

        public void InitializeFOperation2Z3Constraints()
        {
            try
            {
                //First for each operation we have to specify the trigger condition
                //HashSet<Operation> lOperationSet = cFrameworkWrapper.OperationSet;
                //foreach (var lOperation in lOperationSet)
                //{
                //    //O_Trigger <=> Trigger expression
                //    cZ3Solver.AddTwoWayImpliesOperator2Constraints((BoolExpr)cZ3Solver.FindExprInExprSet(lOperation.OperationTriggerVariableName)
                //                                                    , convertComplexString2BoolExpr(lOperation.Trigger)
                //                                                    , "Operation_Trigger_Initializing");
                //}

                //formula 4
                // C = (BIG AND) (O_I_j_0 <=> O_Trigger) AND (! O_e_j_0) AND (! O_f_j_0) AND (O_u_j_0 <=> (! O_Trigger))
                //This formula has to be applied to all operation instances
                //Each operation has the field of when it is triggered which can be used in this formula.
                HashSet<OperationInstance> lOperationInstanceList = _frameworkWrapper.GetOperationInstancesInOneTransition(0);

                foreach (OperationInstance lCurrentOperationInstance in lOperationInstanceList)
                {
                    //if (lCurrentOperationInstance.TransitionNumber.Equals(0))
                    //{
                        //(O_I_j_0 <=> O_Trigger)
                        BoolExpr lFirstPart = _z3Solver.TwoWayImpliesOperator(lCurrentOperationInstance.InitialVariable
                                                                            , lCurrentOperationInstance.AbstractOperation.OperationTriggerVariable);

                        //(! O_e_j_0)
                        BoolExpr lSecondPart = _z3Solver.NotOperator(lCurrentOperationInstance.ExecutingVariable);
                        //(! O_f_j_0)
                        BoolExpr lThirdPart = _z3Solver.NotOperator(lCurrentOperationInstance.FinishedVariable);

                        //(O_u_j_0 <=> (! O_Trigger))
                        BoolExpr lFirstOperand = lCurrentOperationInstance.UnusedVariable;
                        BoolExpr lFourthPart = _z3Solver.TwoWayImpliesOperator(lFirstOperand
                                                                            , _z3Solver.NotOperator(lCurrentOperationInstance.AbstractOperation.OperationTriggerVariable));

                        //(O_I_j_0 <=> O_Trigger) AND (! O_e_j_0) AND (! O_f_j_0) AND (O_u_j_0 <=> (! O_Trigger))
                        BoolExpr lWholeFormula = _z3Solver.AndOperator(new List<BoolExpr>() { lFirstPart, lSecondPart, lThirdPart, lFourthPart });

                        //(BIG AND) (O_I_j_0 <=> O_Trigger) AND (! O_e_j_0) AND (! O_f_j_0) AND (O_u_j_0 <=> (! O_Trigger))
                        if (ConvertOperations)
                            _z3Solver.AddAndOperator2Constraints(new List<BoolExpr>() { lWholeFormula }, "formula4-Operation_Status_Initializing");

                        if (BuildPConstraints)
                            AddPrecedanceConstraintToLocalList(lWholeFormula);
                    //}
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in initializeFOperation2Z3Constraints");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                if (!_pConstraints.Contains(pPrecedanceConstraint))
                _pConstraints.Add(pPrecedanceConstraint);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddPrecedanceConstraintToLocalList");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                        variant lVariant = _frameworkWrapper.VariantLookupByName(lExprPart);
                        lResultVariantList.Add(lVariant);
                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in GetListOfVariantsFromAVariantExpr");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                        part lPart = _frameworkWrapper.PartLookupByName(lExprPart);
                        lResultPartList.Add(lPart);
                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in GetListOfPartsFromAPartExpr");
                _outputHandler.PrintMessageToConsole(ex.Message);
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

        public BoolExpr ConvertIncompleteOperationInstances2CompleteOperationInstanceExpr(string pIncompleteOperationInstance
                                                                                        , string pTransitionNumber = "-1"
                                                                                        , bool pKeepOriginalVariable = false
                                                                                        , bool pAdd2List = true) 
        {
            BoolExpr lResultBoolExpr = null;
            try
            {
                if (pIncompleteOperationInstance.Equals("true"))
                    lResultBoolExpr = _z3Solver.MakeTrueExpr();
                else if (pIncompleteOperationInstance.Equals("false"))
                    lResultBoolExpr = _z3Solver.MakeFalseExpr();
                else
                {
                    //By "Complete" we mean it mentions the operation state and transition
                    if (IsOperationInstanceComplete(pIncompleteOperationInstance))
                    {
                        lResultBoolExpr = (BoolExpr)_z3Solver.FindExprInExprSet(pIncompleteOperationInstance);
                    }
                    else
                    {
                        //If some part of the precondition are missing, i.e. operation name, operation state, operation variant, operation transition no
                        if (IsOperationInstanceMissingStatus(pIncompleteOperationInstance))
                        {
                            //This preconditon should consider state FINISHED or state UNUSED and any variants and any transitions
                            string lOperationName = pIncompleteOperationInstance;
                            var lOperation = _frameworkWrapper.GetOperationFromOperationName(lOperationName);
                            //Note: In this part we want to implement weak links, which means if an operation is mentioned as a precondition 
                            //it means either the operation is in finished state or in an unused state
                            //Hence we have to make an operation instance from it
                            if (!pKeepOriginalVariable)
                            {
                                BoolExpr lBoolExprPart1 = ConvertOperationInstanceWithMissingTransitionNumber(lOperation, Enumerations.OperationInstanceState.Finished, pTransitionNumber, false, false);
                                BoolExpr lBoolExprPart2 = ConvertOperationInstanceWithMissingTransitionNumber(lOperation, Enumerations.OperationInstanceState.Unused, pTransitionNumber, false, false);

                                lResultBoolExpr = _z3Solver.OrOperator(new List<BoolExpr>() { lBoolExprPart1, lBoolExprPart2 });

                            }
                            else
                            {

                            }
                        }
                        else if (IsOperationInstanceMissingTransitionNo(pIncompleteOperationInstance))
                        {
                            //This means that we want to complete the operation instance with every instance of transitions
                            if (!pKeepOriginalVariable)
                            {
                                //This precondition should consider current transitions
                                string lOperationName = _frameworkWrapper.ReturnOperationNameFromOperationInstanceName(pIncompleteOperationInstance);
                                var lOperation = _frameworkWrapper.GetOperationFromOperationName(lOperationName);
                                Enumerations.OperationInstanceState lOperationState = _frameworkWrapper.ReturnOperationStateFromOperationInstanceName(pIncompleteOperationInstance);

                                lResultBoolExpr = ConvertOperationInstanceWithMissingTransitionNumber(lOperation, lOperationState, pTransitionNumber, false, false);
                            }
                            //This means (pKeepOriginalVariable = true) that we don't want to complete the incomplete operation instance variable
                            else
                            {
                                lResultBoolExpr = (BoolExpr)_z3Solver.FindExprInExprSet(pIncompleteOperationInstance);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertIncompleteOperationInstances2CompleteOperationInstanceExpr");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                //If the operation instance contains space then it is an expression!!
                if (!pOperationInstanceName.Contains(' '))
                {
                    string[] lOperationInstanceParts = pOperationInstanceName.Split('_');
                    //(operation name, operation state, operation transition no)
                    if (lOperationInstanceParts.Length.Equals(3))
                        lResult = true;
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in IsOperationInstanceComplete");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _outputHandler.PrintMessageToConsole("error in IsOperationInstanceMissingTransitionNo");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _outputHandler.PrintMessageToConsole("error in IsOperationInstanceMissingVariant");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
            bool lResult = false;
            try
            {
                string[] lOperationInstanceParts = pOperationInstanceName.Split('_');
                //ONLY operation instance name mentioned
                if (lOperationInstanceParts.Length.Equals(1))
                    lResult = true;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in IsOperationInstanceMissingStatus");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                    lResultExpr = (BoolExpr)_z3Solver.FindExprInExprSet(pOperationInstance);
                else
                    lResultExpr = ConvertIncompleteOperationInstances2CompleteOperationInstanceExpr(pOperationInstance, pTransitionNumber.ToString());
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in FindExprInExprList");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                    lResult = _z3Solver.MakeBoolExprFromString(pNode.Data);
                }
                else if ((pNode.Data != "and") && (pNode.Data != "or") && (pNode.Data != "not"))
                {
                    //We have one operator
                    ////lResult = pNode.Data;
                    lResult = MkCondition(pNode.Data, pState);
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
                                lResult = _z3Solver.AndOperator(new List<BoolExpr>() { ParseCondition(lChildren[0], pState)
                                                                                    , ParseCondition(lChildren[1], pState) });
                                break;
                            }
                        case "or":
                            {
                                lResult = _z3Solver.OrOperator(new List<BoolExpr>() { ParseCondition(lChildren[0], pState)
                                                                                    , ParseCondition(lChildren[1], pState) });
                                break;
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                lResult = _z3Solver.NotOperator(ParseCondition(lChildren[0], pState));
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
        private BoolExpr MkCondition(string pCon, int pTransitionNo)
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
                            lResult = ((BoolExpr)_z3Solver.FindExprInExprSet(pCon + "_" + pTransitionNo.ToString()));
                        }
                        else
                            //This means the precondition only includes an operation
                            throw new System.ArgumentException("Precondition did not include a status", pCon);
                    }
                    else
                        //This means the precondition includes a state and a variant
                        if (_frameworkWrapper.GetOperationTransitionNumberFromOperationInstanceString(pCon) > pTransitionNo)
                        {
                            //This means the precondition is on a transition state which has not been reached yet!
                            //lZ3Solver.AddConstraintToSolver(lZ3Solver.NotOperator(lOpPrecondition), pPreconditionSource);
                            //unsatisfiable = true;
                            //break;
                            lResult = _z3Solver.GetFalseBoolExpr();
                        }
                        else
                        {
                            lResult = (BoolExpr)_z3Solver.FindExprInExprSet(pCon);
                        }
                }
                else
                    //This means the postcondition only includes an operation
                    throw new System.ArgumentException("Precondition did not include a status", pCon);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in mkCondition");
                _outputHandler.PrintMessageToConsole(ex.Message);
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

        public void InitializingOperationPreconditionVariable(OperationInstance pOperationInstance, int pTransitionNo)
        {
            try
            {
                string lConstraintSource = "Operation_Precondition_Initializing";

                /*if (pOperationInstance.AbstractOperation.Precondition.Count > 0)
                    //First we have to set the precondition field of each operation as the precondition is needed in these formulas
                    cZ3Solver.AddTwoWayImpliesOperator2Constraints((BoolExpr)cZ3Solver.FindExprInExprSet(pOperationInstance.OperationPreconditionVariableName)
                                                                   , cZ3Solver.AndOperator(pOperationInstance.AbstractOperation.Precondition)
                                                                   , lConstraintSource);*/

                List<BoolExpr> lPreconditionExpressions = new List<BoolExpr>();

                if (pOperationInstance.AbstractOperation.Precondition != null)
                {
                    if (pOperationInstance.AbstractOperation.Precondition.Count != 0)
                    {
                        foreach (var lPrecondition in pOperationInstance.AbstractOperation.Precondition)
                        {
                            //If the operation HAS a precondition
                            //By "Complete" we mean it mentions the operation state, and transition
                            if (IsOperationInstanceComplete(lPrecondition))
                            {
                                //This means firstly that this precondition is a single operation instance
                                //Secondly the precondition has all parts mentioned, i.e. operation name, operation state, and operation transition no
                                int lTransitionNo = _frameworkWrapper.GetOperationTransitionNumberFromOperationInstanceString(lPrecondition);

                                if (lTransitionNo.Equals("K"))
                                {
                                    //First remove the precondition with transition number K
                                    pOperationInstance.AbstractOperation.RemovePrecondition(lPrecondition);
                                    //Then add precondition with current transition number

                                }

                                else if (lTransitionNo.Equals("K+1"))
                                {
                                    //First remove precondition with transition number K+1
                                    pOperationInstance.AbstractOperation.RemovePrecondition(lPrecondition);
                                    //Then add precondition with next transition number

                                }
                                else if (lTransitionNo > int.Parse(pOperationInstance.TransitionNumber))
                                    {
                                        //This means the precondition is on a transition state which has not been reached yet!
                                        //lOpPrecondition = lZ3Solver.NotOperator(lOpPrecondition);
                                        _z3Solver.AddConstraintToSolver(_z3Solver.NotOperator(lPrecondition), lConstraintSource);
                                    }
                                else
                                {
                                    //We already know that the precondition is a complete operation instance
                                    //This means the precondition includes an operation status
                                    //lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0] + "_" + currentVariant.index  + "_" + pState.ToString());
                                    lPreconditionExpressions.Add((BoolExpr)_z3Solver.FindExprInExprSet(lPrecondition));
                                }
                            }
                            else
                            {
                                //This means that the precondition has been an expression
                                BoolExpr lTempBoolExpr = ConvertComplexString2BoolExpr(lPrecondition, pOperationInstance.TransitionNumber);
                                lPreconditionExpressions.Add(lTempBoolExpr);
                            }
                        }

                        //At the end we have to look at each operation's resource and see which other operations need the same resource
                        //This information is previously mentioned in cResourceOperationsMap and we have to make sure that the needed resource is idle
                        if (!pOperationInstance.AbstractOperation.Resource.Equals(""))
                            lPreconditionExpressions.Add(ExpressionToEvaluateIfResourceIsIdle(pOperationInstance.AbstractOperation.Resource
                                                                                         , pTransitionNo));


                        _z3Solver.AddTwoWayImpliesOperator2Constraints(pOperationInstance.OperationPreconditionVariable
                                                                       , _z3Solver.AndOperator(lPreconditionExpressions)
                                                                       , lConstraintSource);
                    }
                    else
                        //If the operation DOES NOT have a precondition hence
                        //We want to force the precondition to be true
                        if (!pOperationInstance.AbstractOperation.Resource.Equals(""))
                        {
                            lPreconditionExpressions.Add(ExpressionToEvaluateIfResourceIsIdle(pOperationInstance.AbstractOperation.Resource
                                                                                             , pTransitionNo));            
                            _z3Solver.AddTwoWayImpliesOperator2Constraints(pOperationInstance.OperationPreconditionVariable
                                                                           , _z3Solver.AndOperator(lPreconditionExpressions)
                                                                           , lConstraintSource);

                        }
                        else
                            _z3Solver.AddConstraintToSolver(pOperationInstance.OperationPreconditionVariable
                                                        , lConstraintSource);
                }

                //At the end we have to look at each operation's resource and see which other operations need the same resource
                //This information is previously mentioned in cResourceOperationsMap and we have to make sure that the needed resource is idle


            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in initializingOperationPreconditionVariable");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public BoolExpr ExpressionToEvaluateIfResourceIsIdle(string pResourceName, int pTransitionNumber)
        {
            BoolExpr lResultExpression = null;
            try
            {

                //Ri is idle if not(O1^e_CurrntTransition OR O5^e_CurrntTransition OR O6^e_CurrntTransition)
                //This will give us the list of operations which use this resource
                List<Operation> lListOfAbstractOperations = _resourcesOperations[pResourceName];

                if (lListOfAbstractOperations != null)
                {
                    foreach (Operation lCurrentOperation in lListOfAbstractOperations)
	                {
                        HashSet<OperationInstance> lOperationInstanceList = _frameworkWrapper.GetOperationInstancesForOneOperationInOneTrasition(lCurrentOperation, pTransitionNumber);
                        OperationInstance lOperationInstance = lOperationInstanceList.First();

                        if (lResultExpression == null)
                            lResultExpression = lOperationInstance.ExecutingVariable;
                        else
                            lResultExpression = _z3Solver.OrOperator(new List<BoolExpr>() { lResultExpression, lOperationInstance.ExecutingVariable });
		 
	                }
                }

                if (lResultExpression != null)
                    lResultExpression = _z3Solver.NotOperator(lResultExpression);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ExpressionToEvaluateIfResourceIsIdle");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultExpression;
        }

        public void InitializingOperationPostconditionVariable(OperationInstance pOperationInstance, int pTransitionNo)
        {
            try
            {
                string lConstraintSource = "Operation_Postcondition_Initializing";

                if (pOperationInstance.AbstractOperation.Postcondition != null)
                {
                    if (pOperationInstance.AbstractOperation.Postcondition.Count != 0)
                    {
                        List<BoolExpr> lPostconditionExpressions = new List<BoolExpr>();
                        foreach (var lPostcondition in pOperationInstance.AbstractOperation.Postcondition)
                        {
                            //If the operation HAS a precondition
                            //By "Complete" we mean it mentions the operation state, and transition
                            if (IsOperationInstanceComplete(lPostcondition))
                            {
                                //This means firstly that this precondition is a single operation instance
                                //Secondly the precondition has all parts mentioned, i.e. operation name, operation state, and operation transition no
                                int lOperationInstanceTransitionNo = _frameworkWrapper.GetOperationTransitionNumberFromOperationInstanceString(lPostcondition);
                                if (lOperationInstanceTransitionNo > int.Parse(pOperationInstance.TransitionNumber))
                                {
                                    //This means the precondition is on a transition state which has not been reached yet!
                                    //lOpPrecondition = lZ3Solver.NotOperator(lOpPrecondition);
                                    _z3Solver.AddConstraintToSolver(_z3Solver.NotOperator(lPostcondition), lConstraintSource);
                                }
                                else
                                {
                                    //We already know that the precondition is a complete operation instance
                                    //This means the precondition includes an operation status
                                    //lOpPrecondition = lZ3Solver.FindBoolExpressionUsingName(lOperation.precondition[0] + "_" + currentVariant.index  + "_" + pState.ToString());
                                    lPostconditionExpressions.Add((BoolExpr)_z3Solver.FindExprInExprSet(lPostcondition));
                                }
                            }
                            else
                            {
                                //This means that the precondition has been an expression
                                BoolExpr lTempBoolExpr = ConvertComplexString2BoolExpr(lPostcondition, pOperationInstance.TransitionNumber);
                                lPostconditionExpressions.Add(lTempBoolExpr);
                            }
                        }
                        _z3Solver.AddTwoWayImpliesOperator2Constraints(pOperationInstance.OperationPostconditionVariable
                                                                       , _z3Solver.AndOperator(lPostconditionExpressions)
                                                                       , lConstraintSource);
                    }
                    else
                        //If the operation DOES NOT have a precondition hence
                        //We want to force the precondition to be true
                        _z3Solver.AddConstraintToSolver(pOperationInstance.OperationPostconditionVariable
                                                        , lConstraintSource);
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in initializingOperationPostconditionVariable");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public void ConvertFOperationsPrecedenceRulesNStatusControl2Z3ConstraintNewVersion(int pTransitionNo)
        {
            try
            {

                //After the operation trigger field is introduced we will loop over operation instances

                //var lOperationInstanceSet = cFrameworkWrapper.OperationInstanceSet;
                var lOperationInstanceSet = _frameworkWrapper.GetOperationInstancesInOneTransition(pTransitionNo);

                foreach (var lCurrentOperationInstance in lOperationInstanceSet)
                {
                    //First we have to set the precondition field of each operation as the precondition is needed in these formulas
                    InitializingOperationPreconditionVariable(lCurrentOperationInstance, pTransitionNo);
                    InitializingOperationPostconditionVariable(lCurrentOperationInstance, pTransitionNo);

                    //With operation waiting
                    //5.1: (O_I_j and Pre_j) => O_E_j+1 or O_I_j+1
                    //Without operation waiting
                    //5.1: (O_I_j and Pre_j) => O_E_j+1
                    CreateFormula51(lCurrentOperationInstance);

                    //5.2: not (O_I_j and Pre_j) => (O_I_j <=> O_I_j+1)
                    CreateFormula52(lCurrentOperationInstance);

                    //5.3: PICKONE O_I_j O_E_j O_F_j O_U_j
                    CreateFormula53(lCurrentOperationInstance, pTransitionNo);

                    //With operation waiting
                    //5.4: (O_E_j AND Post_j) => O_F_j+1 or O_E_j+1
                    //Without operation waiting
                    //5.4: (O_E_j AND Post_j) => O_F_j+1
                    CreateFormula54(lCurrentOperationInstance);

                    //5.5: O_E_j and (not Post_j) => (O_E_j <=> O_E_j+1)
                    CreateFormula55(lCurrentOperationInstance);

                    //5.6: O_U_j => O_U_j+1
                    CreateFormula56(lCurrentOperationInstance);

                    //5.7: O_F_j => O_F_j+1
                    CreateFormula57(lCurrentOperationInstance);

                    //5.8: A_i2e_j <=> (O_I_j AND O_E_j+1)
                    CreateFormula58(lCurrentOperationInstance);

                    //5.9: A_e2f_j <=> (O_E_j AND O_F_j+1)
                    CreateFormula59(lCurrentOperationInstance);

                }

                if (!OperationMutualExecution)
                {
                    //formula 6
                    //In the list of operations, start with operations indexed for variant 0 and compare them with all operations indexed with one more
                    //For each pair (e.g. 0 and 1, 0 and 2,...) compare its current state with its new state on the operation_I
                    CreateFormula6();
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertFOperations2Z3ConstraintNewVersion");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void CreateFormula58(OperationInstance pCurrentOperationInstance)
        {
            //5.8: A_I2E_j <=> (O_I_j AND O_E_j+1)
            try
            {
                BoolExpr LeftHandSide = pCurrentOperationInstance.Action_I2E.Variable;

                //A_I2E_j <=> (O_I_j AND O_E_j+1)
                ///OperationInstance lNextTransitionOperationInstance = getNextTransitionOperationInstance(pCurrentOperationInstance);
                OperationInstance lNextTransitionOperationInstance = pCurrentOperationInstance.NextOperationInstance();

                if (lNextTransitionOperationInstance != null)
                {
                    BoolExpr tempRightHandSide = null;
                    if (!OperationWaiting)
                    {
                        //With operation waiting
                        //O_I_j AND O_E_j+1
                        tempRightHandSide = _z3Solver.AndOperator(new List<BoolExpr>() {pCurrentOperationInstance.InitialVariable
                                                                                            , lNextTransitionOperationInstance.ExecutingVariable});
                    }
                    else
                    {
                        //Without operation waiting
                        //O_E_j+1
                        tempRightHandSide = lNextTransitionOperationInstance.ExecutingVariable;
                    }
                    //In case the current transition number is the last transition then the next transition number will be NULL!
                    BoolExpr lWholeFormula = _z3Solver.TwoWayImpliesOperator(LeftHandSide, tempRightHandSide);

                    if (ConvertOperationPrecedenceRules)
                        _z3Solver.AddConstraintToSolver(lWholeFormula, "formula5.8");

                    if (BuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lWholeFormula);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CreateFormula58");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }


        private void CreateFormula59(OperationInstance pCurrentOperationInstance)
        {
            //5.9: A_E2F_j <=> (O_E_j AND O_F_j+1)
            try
            {
                BoolExpr LeftHandSide = pCurrentOperationInstance.Action_E2F.Variable;

                //A_E2F_j <=> (O_E_j AND O_F_j+1)
                ///OperationInstance lNextTransitionOperationInstance = getNextTransitionOperationInstance(pCurrentOperationInstance);
                OperationInstance lNextTransitionOperationInstance = pCurrentOperationInstance.NextOperationInstance();

                if (lNextTransitionOperationInstance != null)
                {
                    BoolExpr lRightHandSide = null;

                    if (!OperationWaiting)
                    {
                        //With operation waiting
                        //O_E_j AND O_F_j+1
                        lRightHandSide = _z3Solver.AndOperator(new List<BoolExpr>() { pCurrentOperationInstance.ExecutingVariable 
                                                                                          , lNextTransitionOperationInstance.FinishedVariable});
                    }
                    else
                    {
                        //Without operation waiting
                        //O_F_j+1
                        lRightHandSide = lNextTransitionOperationInstance.FinishedVariable;
                    }

                    BoolExpr lWholeFormula = _z3Solver.TwoWayImpliesOperator(LeftHandSide, lRightHandSide);

                    if (ConvertOperationPrecedenceRules)
                        _z3Solver.AddConstraintToSolver(lWholeFormula, "formula5.9");

                    if (BuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lWholeFormula);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CreateFormula59");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void CreateFormula6()
        {
            try
            {
                //formula 6
                //In the list of operations, start with operations indexed for part 0 and compare them with all operations indexed with one more
                //For each pair (e.g. 0 and 1, 0 and 2,...) compare its current state with its new state on the operation_I

                BoolExpr lFormulaSix = null;

                //This will get all operation instances which are in the Initial state for the given state
                //HashSet<String> lActiveOperationSet = cFrameworkWrapper.getActiveOperationNamesSet(pState, "I");
                //HashSet<OperationInstance> lOperationInstanceSet = cFrameworkWrapper.getOperationInstancesInOneTransition(cCurrentTransitionNumber);
                #region Previous Version
                //Previous version when we did not use the actions
                /*if (lOperationInstanceSet != null)
                {
                    foreach (OperationInstance lFirstOperationInstance in lOperationInstanceSet)
                    {

                        foreach (OperationInstance lSecondOperationInstance in lOperationInstanceSet)
                        {

                            if (lFirstOperationInstance.Index < lSecondOperationInstance.Index)
                            {
                                //Formula 6 = Big AND (!(O_I_k_j and !(O_I_k_(j+1)) AND (O_I_l_j AND !(O_I_l_(j+1)))))
                                
                                //lFirstOperand = O_I_k_j
                                BoolExpr lFirstOperand = (BoolExpr)cZ3Solver.FindExprInExprSet(lFirstOperationInstance.InitialVariableName);

                                //lSecondOperand = !(O_I_k_(j+1))
                                OperationInstance lNextStateFirstOperationInstance = getNextTransitionOperationInstance(lFirstOperationInstance);
                                if (lNextStateFirstOperationInstance != null)
                                {
                                    BoolExpr lSecondOperand = cZ3Solver.NotOperator(lNextStateFirstOperationInstance.InitialVariableName);

                                    //lFirstParanthesis = (lFirstOperand AND lSecondOperand)
                                    BoolExpr lFirstParantesis = cZ3Solver.AndOperator(new List<BoolExpr>() { lFirstOperand, lSecondOperand });

                                    //lThirdOperand = O_I_l_j
                                    BoolExpr lThirdOperand = (BoolExpr)cZ3Solver.FindExprInExprSet(lSecondOperationInstance.InitialVariableName);

                                    OperationInstance lNextStateSecondOperationInstance = getNextTransitionOperationInstance(lSecondOperationInstance);
                                    if (lNextStateSecondOperationInstance != null)
                                    {
                                        //lFourthOperand = !(O_I_l_(j+1))
                                        BoolExpr lFourthOperand = cZ3Solver.NotOperator(lNextStateSecondOperationInstance.InitialVariableName);

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
                        }
                    }
                }*/
                #endregion Previous Version

                foreach (DependentActions lCurrectDependencyActions in DependentActionsList)
                {
                    //Formula 6 = Big AND (!(O_I_k_j and !(O_I_k_(j+1)) AND (O_I_l_j AND !(O_I_l_(j+1)))))


                    Action lFirstDependencyAction = lCurrectDependencyActions.Action1; // Action from Dependecy Analysis, ie. Action K
                    Action lSecondDependencyAction = lCurrectDependencyActions.Action2;  // Action from Dependency Analysis, i.e Action K

                    //lFirstOperand = O_I_k_j
                    Action lFirstAction = lFirstDependencyAction.GetActionForTransition(_currentTransitionNumber);
                    //lSecondAction = O_I_l_j
                    Action lSecondAction = lSecondDependencyAction.GetActionForTransition(_currentTransitionNumber);

                    //lNextStateFirstOperand = O_I_k_(j+1)
                    Action lNextStateFirstAction = lFirstAction.GetActionForTransition(_currentTransitionNumber + 1);
                    //lNextStateSecondOperand = O_I_l_(j+1)
                    Action lNextStateSecondAction = lSecondAction.GetActionForTransition(_currentTransitionNumber + 1); 

                    //FirstOperand_NextTransition = !(O_I_k_(j+1))
                    BoolExpr lTempSecondOperand = _z3Solver.NotOperator(lNextStateFirstAction.Name);

                    //FirstOperand_NextTransition = !(O_I_l_(j+1))
                    BoolExpr lTempFourthOperand = _z3Solver.NotOperator(lNextStateSecondAction.Name);

                    //lTempFirstPart = (O_I_k_j and !(O_I_k_(j+1)))
                    BoolExpr lTempFirstPart = _z3Solver.AndOperator(new List<BoolExpr>(){lFirstAction.Variable
                                                                                            , lTempSecondOperand});

                    //lTempSecondPart = (O_I_l_j AND !(O_I_l_(j+1)))
                    BoolExpr lTempSecondPart = _z3Solver.AndOperator(new List<BoolExpr>(){lSecondAction.Variable
                                                                                            , lTempFourthOperand});

                    //lTempExpr = lTempFirstPart AND lTempSecondPart
                    BoolExpr lTempExpr = _z3Solver.AndOperator(new List<BoolExpr>(){lTempFirstPart
                                                                                    , lTempSecondPart});

                    //lTempExpr = not (lTempExpr)
                    lTempExpr = (BoolExpr)_z3Solver.NotOperator(lTempExpr);

                    if (lFormulaSix == null)
                        lFormulaSix = lTempExpr;
                    else
                        lFormulaSix = (BoolExpr)_z3Solver.AndOperator(new List<BoolExpr>() { lFormulaSix, lTempExpr });
                }

                if (lFormulaSix != null)
                {
                    if (ConvertOperationPrecedenceRules)
                        _z3Solver.AddConstraintToSolver(lFormulaSix, "formula6");

                    if (BuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lFormulaSix);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CreateFormula6");
                _outputHandler.PrintMessageToConsole(ex.Message);
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

        private void CreateFormula57(OperationInstance pOperationInstance)
        {
            try
            {
                //Formula 5.7
                //O_F_k_j => O_F_k_j+1
                ///OperationInstance lNextTransitionOperationInstance = getNextTransitionOperationInstance(pOperationInstance);
                OperationInstance lNextTransitionOperationInstance = pOperationInstance.NextOperationInstance();
                if (lNextTransitionOperationInstance != null)
                {
                    BoolExpr lWholeFormula = _z3Solver.ImpliesOperator(new List<BoolExpr>() { pOperationInstance.FinishedVariable
                                                                                    , lNextTransitionOperationInstance.FinishedVariable });

                    if (ConvertOperationPrecedenceRules)
                        _z3Solver.AddConstraintToSolver(lWholeFormula, "formula5.7");

                    if (BuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lWholeFormula);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CreateFormula57");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void CreateFormula56(OperationInstance pOperationInstance)
        {
            try
            {
                //Formula 5.6
                //O_U_k_j => O_U_k_j+1
                ///OperationInstance lNextTransitionOperationInstance = getNextTransitionOperationInstance(pOperationInstance);
                OperationInstance lNextTransitionOperationInstance = pOperationInstance.NextOperationInstance();

                if (lNextTransitionOperationInstance != null)
                {
                    BoolExpr lWholeFormula = _z3Solver.ImpliesOperator(new List<BoolExpr>{ pOperationInstance.UnusedVariable
                                                                                        , lNextTransitionOperationInstance.UnusedVariable});

                    if (ConvertOperationPrecedenceRules)
                        _z3Solver.AddConstraintToSolver(lWholeFormula, "formula5.6");

                    if (BuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lWholeFormula);

                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CreateFormula56");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void CreateFormula54(OperationInstance pOperationInstance)
        {
            try
            {
                //With operation waiting
                //5.4: (O_E_k_j AND Post_k_j) => O_F_k_j+1 or O_E_k_j+1
                //Without operation waiting
                //5.4: (O_E_k_j AND Post_k_j) => O_F_k_j+1

                // Optimized //TODO:Check this line, it might be the fact that this line is not needed as the post conditions are set outside this method in the mother method!!
                //resetOperationPostcondition(pOperation, pCurrentVariant, pState, "formula5.4");

                //Optimized:  BoolExpr lLeftHandSide = lZ3Solver.AndOperator(new List<BoolExpr>() { lOp_E_CurrentState, lOpPostcondition });
                BoolExpr lLeftHandSide = ReturnOperationExecutingStateNItsPostcondition(pOperationInstance);

                ///OperationInstance lNextTransitionOperationInstance = getNextTransitionOperationInstance(pOperationInstance);
                OperationInstance lNextTransitionOperationInstance = pOperationInstance.NextOperationInstance();

                if (lNextTransitionOperationInstance != null)
                {
                    BoolExpr lRightHandSide = null;

                    if (!OperationWaiting)
                    {
                        //With operation waiting
                        //O_F_k_j+1 or O_E_k_j+1
                        lRightHandSide = _z3Solver.OrOperator(new List<BoolExpr>() { lNextTransitionOperationInstance.FinishedVariable 
                                                                                          , lNextTransitionOperationInstance.ExecutingVariable});
                    }
                    else
                    {
                        //Without operation waiting
                        //O_F_k_j+1
                        lRightHandSide = lNextTransitionOperationInstance.FinishedVariable;
                    }

                    BoolExpr lWholeFormula = _z3Solver.ImpliesOperator(new List<BoolExpr>() { lLeftHandSide
                                                                                        , lRightHandSide });

                    if (ConvertOperationPrecedenceRules)
                        _z3Solver.AddConstraintToSolver(lWholeFormula, "formula5.4");

                    if (BuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lWholeFormula);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CreateFormula54");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void CreateFormula53(OperationInstance pOperationInstance, int pTransitionNo)
        {
            try
            {
                //Fromula 5.3
                //for this XOR O_I_k_j O_E_k_j O_F_k_j O_U_k_j

                if (pTransitionNo == 0)
                {
                    BoolExpr lWholeFormula = _z3Solver.PickOneOperator(new List<BoolExpr> { pOperationInstance.InitialVariable
                                                                                , pOperationInstance.ExecutingVariable
                                                                                , pOperationInstance.FinishedVariable
                                                                                , pOperationInstance.UnusedVariable });

                    if (ConvertOperationPrecedenceRules)
                        _z3Solver.AddConstraintToSolver(lWholeFormula, "formula5.3");

                    if (BuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lWholeFormula);
                }

                ///OperationInstance lNextTransitionOperationInstance = getNextTransitionOperationInstance(pOperationInstance);
                OperationInstance lNextTransitionOperationInstance = pOperationInstance.NextOperationInstance();

                BoolExpr lWholeFormula1 = _z3Solver.PickOneOperator(new List<BoolExpr> { lNextTransitionOperationInstance.InitialVariable
                                                                            , lNextTransitionOperationInstance.ExecutingVariable
                                                                            , lNextTransitionOperationInstance.FinishedVariable
                                                                            , lNextTransitionOperationInstance.UnusedVariable });

                if (ConvertOperationPrecedenceRules)
                    _z3Solver.AddConstraintToSolver(lWholeFormula1, "formula5.3");

                if (BuildPConstraints)
                    AddPrecedanceConstraintToLocalList(lWholeFormula1);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CreateFormula53");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }


        private void CreateFormula53GeneralVersion(OperationInstance pOperationInstance)
        {
            try
            {
                //Fromula 5.3
                //for this XOR O_I_k_j O_E_k_j O_F_k_j O_U_k_j

                _z3Solver.AddPickOneOperator2Constraints(new List<string> { pOperationInstance.InitialVariableName
                                                                      , pOperationInstance.ExecutingVariableName
                                                                      , pOperationInstance.FinishedVariableName
                                                                      , pOperationInstance.UnusedVariableName }, "formula5.3");

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CreateFormula53");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void CreateFormula52(OperationInstance pOperationInstance)
        {
            try
            {
                //Formula 5.2
                // O_I_k_j and (not Pre_k_j) => (O_I_k_j <=> O_I_k_j+1)
                BoolExpr tempLeftHandSideTwo;

                tempLeftHandSideTwo = ReturnOperationInitialStateNNotItsPrecondition(pOperationInstance);

                //tempLeftHandSideTwo = cZ3Solver.NotOperator(tempLeftHandSideTwo);

                BoolExpr tempRightHandSideTwo;

                ///OperationInstance lNextOperationInstance = getNextTransitionOperationInstance(pOperationInstance);
                OperationInstance lNextOperationInstance = pOperationInstance.NextOperationInstance();

                if (lNextOperationInstance != null)
                {
                    tempRightHandSideTwo = _z3Solver.TwoWayImpliesOperator(pOperationInstance.InitialVariable
                                                                            , lNextOperationInstance.InitialVariable);

                    BoolExpr lWholeFormula = _z3Solver.ImpliesOperator(new List<BoolExpr>() { tempLeftHandSideTwo, tempRightHandSideTwo });

                    if (ConvertOperationPrecedenceRules)
                        _z3Solver.AddConstraintToSolver(lWholeFormula, "formula5.2");

                    if (BuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lWholeFormula);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CreateFormula52");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void CreateFormula55(OperationInstance pOperationInstance)
        {
            try
            {
                //Formula 5.5
                // O_E_k_j and (not Post_k_j) => (O_E_k_j <=> O_E_k_j+1)
                BoolExpr tempLeftHandSideTwo;

                tempLeftHandSideTwo = ReturnOperationExecutingStateNNotItsPostcondition(pOperationInstance);

                //tempLeftHandSideTwo = cZ3Solver.NotOperator(tempLeftHandSideTwo);

                BoolExpr tempRightHandSideTwo;

                ///OperationInstance lNextOperationInstance = getNextTransitionOperationInstance(pOperationInstance);
                OperationInstance lNextOperationInstance = pOperationInstance.NextOperationInstance();

                if (lNextOperationInstance != null)
                {
                    tempRightHandSideTwo = _z3Solver.TwoWayImpliesOperator(pOperationInstance.ExecutingVariable
                                                                            , lNextOperationInstance.ExecutingVariable);

                    BoolExpr lWholeFormula = _z3Solver.ImpliesOperator(new List<BoolExpr>() { tempLeftHandSideTwo, tempRightHandSideTwo });

                    if (ConvertOperationPrecedenceRules)
                        _z3Solver.AddConstraintToSolver(lWholeFormula, "formula5.5");

                    if (BuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lWholeFormula);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CreateFormula5");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        ///////// <summary>
        //////// This function takes one operation instance and returns the operation instance of the next transition if there is one
        ///////// </summary>
        ///////// <param name="pOperationInstance"></param>
        ///////// <returns></returns>
        //////private OperationInstance getNextTransitionOperationInstance(OperationInstance pOperationInstance)
        //////{
            
        //////    OperationInstance lOperationInstance = null;
        //////    try
        //////    {
        //////        if (pOperationInstance.TransitionNumber != "K")
        //////        {
        //////            int lCurrentOperationInstanceTransitionNumber = int.Parse(pOperationInstance.TransitionNumber);

        //////            int lTempInt = lCurrentOperationInstanceTransitionNumber + 1;

        //////            lOperationInstance = cFrameworkWrapper.operationInstanceLookup(pOperationInstance.AbstractOperation.Name
        //////                                                                        , lTempInt.ToString());
        //////        }
        //////        else
        //////        {
        //////            string lCurrentOperationInstanceTransitionNumber = pOperationInstance.TransitionNumber;

        //////            string lNextTransitionNumber = lCurrentOperationInstanceTransitionNumber + "+1";

        //////            lOperationInstance = cFrameworkWrapper.operationInstanceLookup(pOperationInstance.AbstractOperation.Name
        //////                                                                        , lNextTransitionNumber);
        //////        }
        //////    }
        //////    catch (Exception ex)
        //////    {
        //////        cOutputHandler.printMessageToConsole("error in getNextTransitionOperationInstance");
        //////        cOutputHandler.printMessageToConsole(ex.Message);
        //////    }
        //////    return lOperationInstance;
        //////}

        private void CreateFormula51(OperationInstance pOperationInstance)
        {
            try
            {
                //Formula 5.1
                //With operation waiting
                //(O_I_k_j and Pre_k_j) => O_E_k_j+1 or O_I_k_j+1
                //Without operation waiting
                //(O_I_k_j and Pre_k_j) => O_E_k_j+1
                BoolExpr tempLeftHandSideOne;
                //if (lOperation.precondition != null)

                //(O_I_k_j and Pre_k_j)
                //Not optimized! tempLeftHandSideOne = lZ3Solver.AndOperator(new List<BoolExpr>() { lOp_I_CurrentState, lOpPrecondition });
                tempLeftHandSideOne = ReturnOperationInitialStateNItsPrecondition(pOperationInstance);
                //else
                //    tempLeftHandSideOne = lOp_I_CurrentState;

                //(O_I_k_j and Pre_k_j) => O_E_k_j+1 or O_I_k_j+1
                ////OperationInstance lNextTransitionOperationInstance = getNextTransitionOperationInstance(pOperationInstance);
                OperationInstance lNextTransitionOperationInstance = pOperationInstance.NextOperationInstance();
                
                if (lNextTransitionOperationInstance != null)
                {
                    BoolExpr tempRightHandSide = null;
                    if (!OperationWaiting)
                    {
                        //With operation waiting
                        //O_E_k_j+1 or O_I_k_j+1
                        tempRightHandSide = _z3Solver.OrOperator(new List<BoolExpr>() {lNextTransitionOperationInstance.ExecutingVariable
                                                                                            , lNextTransitionOperationInstance.InitialVariable});
                    }
                    else
                    {
                        //Without operation waiting
                        //O_E_k_j+1
                        tempRightHandSide = lNextTransitionOperationInstance.ExecutingVariable;
                    }
                    //In case the current transition number is the last transition then the next transition number will be NULL!
                    BoolExpr lWholeFormula = _z3Solver.ImpliesOperator(new List<BoolExpr>() { tempLeftHandSideOne
                                                                                            , tempRightHandSide });

                    if (ConvertOperationPrecedenceRules)
                        _z3Solver.AddConstraintToSolver(lWholeFormula, "formula5.1");

                    if (BuildPConstraints)
                        AddPrecedanceConstraintToLocalList(lWholeFormula);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CreateFormula51");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public BoolExpr ReturnOperationExecutingStateNNotItsPostcondition(OperationInstance pOperationInstance)
        {
            BoolExpr result;
            try
            {
                //If the operation has no precondition then the relevant variable is forced to be true, other wise it will be the precondition
                if (pOperationInstance.AbstractOperation.Postcondition != null)
                {
                    if (pOperationInstance.AbstractOperation.Postcondition.Count != 0)
                    {
                        BoolExpr lNotPost = _z3Solver.NotOperator(pOperationInstance.OperationPostconditionVariable);
                        result = _z3Solver.AndOperator(new List<BoolExpr> { pOperationInstance.ExecutingVariable
                                                                    ,  lNotPost});
                    }
                    else
                        result = _z3Solver.MakeFalseExpr();
                }
                else
                    result = pOperationInstance.ExecutingVariable;

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ReturnOperationExecutingStateNItsPostcondition");
                _outputHandler.PrintMessageToConsole(ex.Message);
                result = null;
            }
            return result;
        }

        public BoolExpr ReturnOperationExecutingStateNItsPostcondition(OperationInstance pOperationInstance)
        {
            BoolExpr result;
            try
            {
                //If the operation has no precondition then the relevant variable is forced to be true, other wise it will be the precondition
                if (pOperationInstance.AbstractOperation.Postcondition != null)
                {
                    if (pOperationInstance.AbstractOperation.Postcondition.Count != 0)
                    {
                        result = _z3Solver.AndOperator(new List<BoolExpr> { pOperationInstance.ExecutingVariable
                                                                    ,  pOperationInstance.OperationPostconditionVariable});
                    }
                    else
                        result = pOperationInstance.ExecutingVariable;
                }
                else
                    result = pOperationInstance.ExecutingVariable;

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ReturnOperationExecutingStateNItsPostcondition");
                _outputHandler.PrintMessageToConsole(ex.Message);
                result = null;
            }
            return result;
        }

        public BoolExpr ReturnOperationInitialStateNNotItsPrecondition(OperationInstance pOperationInstance)
        {
            BoolExpr result;
            try
            {
                //If the operation has no precondition then the relevant variable is forced to be true, other wise it will be the precondition
                if (pOperationInstance.AbstractOperation.Precondition != null)
                {
                    if (pOperationInstance.AbstractOperation.Precondition.Count != 0)
                    {
                        BoolExpr lNotPre = _z3Solver.NotOperator(pOperationInstance.OperationPreconditionVariable);
                        result = _z3Solver.AndOperator(new List<BoolExpr> { pOperationInstance.InitialVariable
                                                                    , lNotPre });
                    }
                    else
                        result = _z3Solver.MakeFalseExpr();
                }
                else
                    result = pOperationInstance.InitialVariable;

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ReturnOperationInitialStateNItsPrecondition");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                    {
                        result = _z3Solver.AndOperator(new List<BoolExpr> { pOperationInstance.InitialVariable
                                                                    , pOperationInstance.OperationPreconditionVariable });
                    }
                    else
                        result = pOperationInstance.InitialVariable;
                }
                else
                    result = pOperationInstance.InitialVariable;

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ReturnOperationInitialStateNItsPrecondition");
                _outputHandler.PrintMessageToConsole(ex.Message);
                result = null;
            }
            return result;
        }

        //public BoolExpr ReturnOperationExecutingStateNItsPostcondition(OperationInstance pOperationInstance)
        //{
        //    BoolExpr result;
        //    try
        //    {
        //        //If the operation has no precondition then the relevant variable is forced to be true, other wise it will be the precondition
        //        if (pOperationInstance.AbstractOperation.Postcondition != null)
        //        {
        //            if (pOperationInstance.AbstractOperation.Postcondition.Count != 0)
        //                result = cZ3Solver.AndOperator(new List<BoolExpr> { pOperationInstance.ExecutingVariable
        //                                                            , pOperationInstance.OperationPostconditionVariable });
        //            else
        //                result = pOperationInstance.InitialVariable;
        //        }
        //        else
        //            result = pOperationInstance.InitialVariable;

        //    }
        //    catch (Exception ex)
        //    {
        //        cOutputHandler.printMessageToConsole("error in ReturnOperationExecutingStateNItsPostcondition");
        //        cOutputHandler.printMessageToConsole(ex.Message);
        //        result = null;
        //    }
        //    return result;
        //}

        public BoolExpr FindingADatasBooleanVariable(string pData)
        {
            BoolExpr lResultVariable = null;
            try
            {

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in FindingADatasBooleanVariable");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultVariable;
        }

        public BoolExpr ReturnResourceVariable(string pResourceName, string pTransitionNo, bool pKeepOriginalVariables = true)
        {
            BoolExpr lResultExpr = null;

            try
            {
                string lResourceVariableName = pResourceName + "_" + pTransitionNo;

                lResultExpr = (BoolExpr)_z3Solver.FindExprInExprSet(lResourceVariableName);

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in MakeResourceVariable");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }

            return lResultExpr;
        }

        //TODO: We need enum for operators to be used here
        public BoolExpr ParseComplexString(Node<string> pNode, string pTransitionNumber, bool pKeepOriginalVariables = false, bool pAdd2List = true)
        {
            BoolExpr lResult = null;
            try
            {
                if ((pNode.Data != "and") 
                    && (pNode.Data != "or") 
                    && (pNode.Data != "not")
                    && (pNode.Data != "=>")
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
                    if (_frameworkWrapper.HavePartWithName(pNode.Data) || _frameworkWrapper.HaveVariantWithName(pNode.Data))
                        lResult = (BoolExpr)_z3Solver.FindExprInExprSet(pNode.Data);
                    else
                        if (pNode.Data.StartsWith("R"))
                            lResult = ReturnResourceVariable(pNode.Data, pTransitionNumber, pKeepOriginalVariables);
                        else
                            lResult = ConvertIncompleteOperationInstances2CompleteOperationInstanceExpr(pNode.Data, pTransitionNumber, pKeepOriginalVariables);

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
                                lResult = _z3Solver.AndOperator(new List<BoolExpr>() { ParseComplexString(lChildren[0], pTransitionNumber, pKeepOriginalVariables)
                                                                                    , ParseComplexString(lChildren[1], pTransitionNumber, pKeepOriginalVariables) });
                                break;
                            }
                        case "or":
                            {
                                lResult = _z3Solver.OrOperator(new List<BoolExpr>() { ParseComplexString(lChildren[0], pTransitionNumber, pKeepOriginalVariables)
                                                                                    , ParseComplexString(lChildren[1], pTransitionNumber, pKeepOriginalVariables) });
                                break;
                            }
                        case "=>":
                            {
                                lResult = _z3Solver.ImpliesOperator(new List<BoolExpr>() { ParseComplexString(lChildren[0], pTransitionNumber, pKeepOriginalVariables)
                                                                                            , ParseComplexString(lChildren[1], pTransitionNumber, pKeepOriginalVariables) });
                                break;
                            }
                        case "not":
                            {
                                lResult = _z3Solver.NotOperator(ParseComplexString(lChildren[0], pTransitionNumber, pKeepOriginalVariables));
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
                _outputHandler.PrintMessageToConsole("error in ParseExpression");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                    lResult = (BoolExpr)_z3Solver.FindExprInExprSet(pNode.Data);
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
                                lResult = _z3Solver.AndOperator(new List<BoolExpr>() { ParseExpression(lChildren[0])
                                                                                    , ParseExpression(lChildren[1]) });
                                break;
                            }
                        case "or":
                            {
                                lResult = _z3Solver.OrOperator(new List<BoolExpr>() { ParseExpression(lChildren[0])
                                                                                    , ParseExpression(lChildren[1]) });
                                break;
                            }
                        case "not":
                            {
                                lResult = _z3Solver.NotOperator(ParseExpression(lChildren[0]));
                                break;
                            }
                        case ">=":
                            {
                                lResult = _z3Solver.GreaterOrEqualOperator(lChildren[0].Data
                                                                            , int.Parse(lChildren[1].Data));
                                break;
                            }
                        case "<=":
                            {
                                lResult = _z3Solver.LessOrEqualOperator(lChildren[0].Data
                                                                        , int.Parse(lChildren[1].Data));
                                break;
                            }
                        case "<":
                            {
                                lResult = _z3Solver.LessThanOperator(lChildren[0].Data
                                                                    , int.Parse(lChildren[1].Data));
                                break;
                            }
                        case ">":
                            {
                                lResult = _z3Solver.GreaterThanOperator(lChildren[0].Data
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
                _outputHandler.PrintMessageToConsole("error in ParseExpression");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResult;
        }

        public string ReturnConstraintsString()
        {
            string lConstraintsString = "";
            try
            {
                //First we have to loop the constraint list
                //List<string> localConstraintList = cFrameworkWrapper.ConstraintList;

                foreach (string lConstraint in _frameworkWrapper.ConstraintSet)
                {
                    if (lConstraintsString != "")
                        lConstraintsString += " AND ";
                    lConstraintsString += lConstraint;

                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in returnConstraintsString");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lConstraintsString;
        }

        /// <summary>
        /// This function uses the constraint set to return all the constraints in one boolExpr
        /// </summary>
        /// <returns>The one boolExpr which represents the constraint set</returns>
        public BoolExpr ReturnBoolExprOfConstraintsSet()
        {
            BoolExpr lReturnBoolExpr = null;
            try
            {
                //First we have to loop the constraint list
                HashSet<string> localConstraintList = _frameworkWrapper.ConstraintSet;

                ////foreach (string lConstraint in localConstraintList)

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in returnBoolExprOfConstraintsSet");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
        public void AddStandAloneConstraint2Z3Solver(BoolExpr pStandAloneConstraint)
        {
            try
            {
                if (pStandAloneConstraint != null)
                    _z3Solver.AddConstraintToSolver(pStandAloneConstraint, "StandAlone Constraint");
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in addStandAloneConstraint2Z3Solver");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _z3Solver.AddConstraintToSolver(ConvertComplexString2BoolExpr(pExtraConfigurationRule)
                                                , "formula3-extra configuration rule");

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddExtraConstraint2Z3Constraint");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// Adds all the framework constrints to the Z3 model
        /// Also adds any given extra constraints to the Z3 model
        /// </summary>
        /// <param name="pExtraConfigurationRule"></param>
        public void ConvertFConstraint2Z3Constraint(string pExtraConfigurationRule = "", bool pOptimizer = false)
        {
            try
            {
                //formula 3
                //First we have to loop the constraint list
                //List<string> localConstraintList = cFrameworkWrapper.ConstraintList;

                foreach (string lConstraint in _frameworkWrapper.ConstraintSet)
                {
                    BoolExpr lBoolExprConstraint = ConvertComplexString2BoolExpr(lConstraint);

                    _z3Solver.AddConstraintToSolver(lBoolExprConstraint
                                                    , "formula3");
                    if (pOptimizer)
                        _z3Solver.AddConstraintToOptimizer(lBoolExprConstraint
                                                        , "formula3");

                    if (_analysisType == Enumerations.AnalysisType.AlwaysSelectedVariantAnalysis 
                        || _analysisType == Enumerations.AnalysisType.AlwaysSelectedOperationAnalysis)
                        AddConfigurationConstraintToLocalList(lBoolExprConstraint);

                }

                //TODO: by default this extra configuration rule can be an array
                if (pExtraConfigurationRule != "")
                {
                    BoolExpr lTempBoolExpr = ConvertComplexString2BoolExpr(pExtraConfigurationRule);
                    _z3Solver.AddConstraintToSolver(lTempBoolExpr
                                                    , "formula3-ExtraConfigRule");
                    if (pOptimizer)
                        _z3Solver.AddConstraintToOptimizer(lTempBoolExpr
                                , "formula3-ExtraConfigRule");
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertFConstraint2Z3Constraint");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                if (!_configurationConstraints.Contains(pConfigurationConstraint))
                    _configurationConstraints.Add(pConfigurationConstraint);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AddConfigurationConstraintToLocalList");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private BoolExpr ReturnFExpression2Z3Constraint(string pExpression)
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
                _outputHandler.PrintMessageToConsole("error in addFExpression2Z3Constraint");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }

        public BoolExpr ConvertComplexString2BoolExpr(string pExpression
                                                    , string pTransitionNumber = "-1"
                                                    , bool pKeepOriginalVariables = false
                                                    , bool pAdd2List = true)
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
                    lResultExpr = ParseComplexString(item, pTransitionNumber, pKeepOriginalVariables);
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertComplexString2BoolExpr");
                _outputHandler.PrintMessageToConsole(ex.Message);
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

        public BoolExpr ConvertOperationPreconditions2BoolExpr(List<string> pOperationPreconditionList)
        {
            BoolExpr lResultExpr = null;
            try
            {
                foreach (var lOperationPrecondition in pOperationPreconditionList)
                {
                    BoolExpr lTempBoolEpr = null;



                    if (lResultExpr != null)
                        lResultExpr = (BoolExpr)_z3Solver.AndOperator(new List<BoolExpr>() { lResultExpr, lTempBoolEpr });
                    else
                        lResultExpr = lTempBoolEpr;
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertOperationPreconditions2BoolExpr");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultExpr;
        }

        public BoolExpr CreateFormula7(HashSet<OperationInstance> pOperationInstanceList, int pState)
        {
            BoolExpr lResultFormula7 = null;
            try
            {
                //NEW formula 7
                //No operation can proceed
                //(Big And) ((! Pre_k_pState AND O_I_k_pState) OR (! Post_k_pState AND O_E_k_pState) OR O_F_k_pState OR O_U_k_pState)

                //This boolean expression is used to refer to this overall goal
                _z3Solver.AddBooleanExpression("F7_" + pState);

                //This formula should be carried out for each created operation instance

                foreach (OperationInstance lCurrentOperationInstance in pOperationInstanceList)
                {
                    if (lCurrentOperationInstance.TransitionNumber.Equals(pState.ToString()))
                    {
                        BoolExpr lNotPreCondition = _z3Solver.NotOperator(lCurrentOperationInstance.OperationPreconditionVariable);
                        //BoolExpr lPreCondition = lCurrentOperationInstance.OperationPreconditionVariable;

                        //Assumption: As the post condition is not used here this part of the formula will become false!
                        //BoolExpr lNotPostCondition = cZ3Solver.NotOperator(cOpPostcondition);

                        BoolExpr lFirstOperand = _z3Solver.AndOperator(new List<BoolExpr>() { lNotPreCondition
                                                                                    , lCurrentOperationInstance.InitialVariable });

                        BoolExpr lNotPostCondition = _z3Solver.NotOperator(lCurrentOperationInstance.OperationPostconditionVariable);
                        //Assumption: The previous assumption will make this part false as well
                        BoolExpr lSecondOperand = _z3Solver.AndOperator(new List<BoolExpr>() { lNotPostCondition
                                                                                                , lCurrentOperationInstance.ExecutingVariable });
                        //BoolExpr lSecondOperand = (BoolExpr)cZ3Solver.FindExprInExprSet(lCurrentOperationInstance.ExecutingVariableName);

                        //In the case where the lSecondPart is false, we should not include the lSecondPart in this section
                        //BoolExpr lOperand = cZ3Solver.OrOperator(new List<BoolExpr>() { lFirstOperand
                        //                                                    , lCurrentOperationInstance.FinishedVariable
                        //                                                    , lCurrentOperationInstance.UnusedVariable });
                        BoolExpr lOperand = _z3Solver.OrOperator(new List<BoolExpr>() { lFirstOperand
                                                                            , lSecondOperand
                                                                            , lCurrentOperationInstance.FinishedVariable
                                                                            , lCurrentOperationInstance.UnusedVariable });

                        if (lResultFormula7 == null)
                            lResultFormula7 = lOperand;
                        else
                            //lResultFormula7 = cZ3Solver.AndOperator(new List<BoolExpr>() { lResultFormula7, lOperand });
                            lResultFormula7 = _z3Solver.AndOperator(new List<BoolExpr>() { lResultFormula7, lOperand });
                    }
                }
                if (lResultFormula7 != null)
                    //lZ3Solver.AddConstraintToSolver(lOverallGoal, "overallGoal");
                    _z3Solver.AddTwoWayImpliesOperator2Constraints((BoolExpr)_z3Solver.FindExprInExprSet("F7_" + pState)
                                                                , lResultFormula7
                                                                , "Formula7");
                //            if (formula7 != null)
                //                lZ3Solver.AddConstraintToSolver(formula7);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in createFormula7");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultFormula7;
        }

        public BoolExpr CreateFormula8(HashSet<OperationInstance> pOperationInstanceList, int pState)
        {
            BoolExpr lResultFormula8 = null;
            try
            {
                //formula 8
                //At least one operation is in initial or executing state
                //(Big OR) (O_I_k_j OR O_E_k_j)

                //This boolean expression is used to refer to this formula 8
                _z3Solver.AddBooleanExpression("F8_" + pState);

                foreach (OperationInstance lCurrentOperationInstance in pOperationInstanceList)
                {
                    BoolExpr lOperand = null;
                    if (lCurrentOperationInstance.TransitionNumber.Equals(pState.ToString()))
                    {
                        lOperand = _z3Solver.OrOperator(new List<string>() { lCurrentOperationInstance.InitialVariableName
                                                                                , lCurrentOperationInstance.ExecutingVariableName });

                        if (lResultFormula8 == null)
                            lResultFormula8 = lOperand;
                        else
                            lResultFormula8 = _z3Solver.OrOperator(new List<BoolExpr>() { lResultFormula8, lOperand });
                    }
                }
                if (lResultFormula8 != null)
                    //lZ3Solver.AddConstraintToSolver(lOverallGoal, "overallGoal");
                    _z3Solver.AddTwoWayImpliesOperator2Constraints((BoolExpr)_z3Solver.FindExprInExprSet("F8_" + pState)
                                                                , lResultFormula8
                                                                , "Formula8");
                //            if (formula8 != null)
                //                lZ3Solver.AddConstraintToSolver(formula8);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in createFormula8");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultFormula8;
        }

        private void StartModelTiming()
        {
            try
            {
                if (ReportTimings)
                {
                    _stopWatchDetailed.Start();
                    _stopWatchTotal.Start();

                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in StartModelTiming");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void StopNReportModelTimingNStartAnalysisTiming(string pMessage)
        {
            try
            {
                if (ReportTimings)
                {
                    _stopWatchDetailed.Stop();
                    _modelCreationTime = _stopWatchDetailed.ElapsedMilliseconds;

                    _outputHandler.PrintMessageToConsole(pMessage + " Time: " + _modelCreationTime + "ms.");

                    _stopWatchDetailed.Restart();
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in StopNReportModelTimingNStartAnalysisTiming");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void StopNReportAnalysisTiming()
        {
            try
            {
                if (ReportTimings)
                {
                    _stopWatchDetailed.Stop();
                    _modelAnalysisTime = _stopWatchDetailed.ElapsedMilliseconds;

                    _outputHandler.PrintMessageToConsole("Model Analysis Time: " + _modelAnalysisTime + "ms.");

                    _stopWatchDetailed.Restart();
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in StopNReportAnalysisTiming");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        private void StopNReportModelAnalysisTiming()
        {
            try
            {
                if (ReportTimings)
                {
                    _stopWatchDetailed.Stop();
                    _stopWatchTotal.Stop();
                    _modelAnalysisReportingTime = _stopWatchDetailed.ElapsedMilliseconds;
                    _outputHandler.PrintMessageToConsole("Model Analysis Reporting Time: " + _modelAnalysisReportingTime + "ms.");
                    _outputHandler.PrintMessageToConsole("Model Analysis TOTAL Time: " + _stopWatchTotal.ElapsedMilliseconds + "ms.");
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in StopNReportModelAnalysisTiming");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public Status AnalyzeModelConstraints(int pTransitionNo)
        {
            Status lResult = Status.UNSATISFIABLE;
            try
            {
                lResult = AnalyzeProductPlatform(pTransitionNo
                                                , 0
                                                , false
                                                , true);

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AnalyzeModelConstraints");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResult;
        }

        /// <summary>
        /// This analysis checks if there are any Deadlock
        /// </summary>
        public bool ExistanceOfDeadlockAnalysis(bool pVariationPointsSet
                                                , bool pReportTypeSet)
        {
            bool lAnalysisResult = false;

            //This is the result of the internal analysis we get
            Status lInternalAnalysisResult = Status.UNKNOWN;

            Status lModelConstraintCheck = Status.UNSATISFIABLE;
            try
            {
                StartModelTiming();

                //TODO: Only should be done when a flag is set
                _outputHandler.PrintMessageToConsole("Existance Of Valid Production Path Analysis:");

                bool lAnalysisComplete = false;

                _z3Solver.PrepareDebugDirectory();

                if (!pVariationPointsSet)
                    SetVariationPoints(Enumerations.GeneralAnalysisType.Dynamic
                                        , Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis);

                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                //          , Transitions Result, Analysis Timing, Unsat Core
                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                //          , Report timings, Debug Mode (Make model file), User Messages
                if (!pReportTypeSet)
                    SetReportType(true, true, true
                                , true, false, false
                                , false, true, false
                                , true, true, true);

                lModelConstraintCheck = PreAnalysis();

                ////Making the static part of the model
                MakeStaticPartOfProductPlatformModel("", true);

                if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                {
                    for (int lTransitionNo = 0; lTransitionNo <= _maxNumberOfTransitions; lTransitionNo++)
                    {
                        _currentTransitionNumber = lTransitionNo;

                        _outputHandler.PrintMessageToConsole("--------------------Transition: " + lTransitionNo + " --------------------");

                        lAnalysisComplete = false;

                        ////Making the dynamic part of the model (Operations and transition between operation status)
                        MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, lTransitionNo);

                        //Now the goal is added to the model according to the type of analysis
                        //Variation Point
                        if (ConvertGoal && !lAnalysisComplete)
                            ConvertExistenceOfDeadlockGoal(lTransitionNo);

                        StopNReportModelTimingNStartAnalysisTiming("Model Creation");

                        //For each variant check if this statement holds - carry out the analysis
                        lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                                , 0
                                                                , lAnalysisComplete);

                        //if the result of the previous analysis is true then we go to the next analysis part
                        StopNReportModelTimingNStartAnalysisTiming("Model Analysis");

                        if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                        {

                            StopNReportAnalysisTiming();

                            //If the result is true it means we have found a deadlock!
                            _outputHandler.PrintMessageToConsole("A deadlock was found!");
                            int lTempInt = lTransitionNo + 1;
                            ReportSolverResult(lTempInt, lAnalysisComplete, _frameworkWrapper, lInternalAnalysisResult, null);
                            break;
                        }

                        //TODO: Is this the best way to end the loop on the transiton numbers???????
                        //If all the transition cycles are completed then the analysis is completed
                        if (lTransitionNo == _maxNumberOfTransitions - 1)
                            lAnalysisComplete = true;
                    }
                }

                //Translating the internal analysis result to the user specific analysis result 
                if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    lAnalysisResult = true;
                else
                    lAnalysisResult = false;

                _outputHandler.PrintMessageToConsole("Analysis Report: ");

                //if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                if (RandomInputFile)
                {
                    if (!lAnalysisResult)
                    {
                        //If the result is false it means no deadlock can be found! HENCE DEADLOCK FREE!!

                        //If t is deadlock free then there is nothing to report as there will be no model given!!
                        ReportSolverResult(_maxNumberOfTransitions, lAnalysisComplete, _frameworkWrapper, lInternalAnalysisResult, null);

                        _outputHandler.PrintMessageToConsole("NO deadlock was found!");
                    }

                }
                else
                {
                    if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                    {
                        if (!lAnalysisResult)
                        {
                            //If the result is false it means no deadlock can be found! HENCE DEADLOCK FREE!!

                            //If t is deadlock free then there is nothing to report as there will be no model given!!
                            ReportSolverResult(_maxNumberOfTransitions, lAnalysisComplete, _frameworkWrapper, lInternalAnalysisResult, null);

                            _outputHandler.PrintMessageToConsole("NO deadlock was found!");
                        }

                    }
                    else
                        _outputHandler.PrintMessageToConsole("There are conflics between model constraints.");
                }

                StopNReportModelAnalysisTiming();

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ExistanceOfDeadlockAnalysis");
                _outputHandler.PrintMessageToConsole(ex.Message);
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

            bool lAnalysisResult = false;

            Status lInternalAnalysisResult = Status.UNKNOWN;
            Status lModelConstraintCheck = Status.UNSATISFIABLE;
            try
            {
                StartModelTiming();

                _outputHandler.PrintMessageToConsole("Product Manufacturing Enumeration Analysis:");

                bool lAnalysisComplete = false;

                ////Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                //cMaxNumberOfTransitions = CalculateAnalysisNoOfCycles();

                _z3Solver.PrepareDebugDirectory();

                if (!pVariationPointsSet)
                    SetVariationPoints(Enumerations.GeneralAnalysisType.Dynamic
                                        , Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis);

                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                //          , Transitions Result, Analysis Timing, Unsat Core
                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                //          , Report timings, Debug Mode (Make model file), User Messages
                if (!pReportTypeSet)
                    SetReportType(true, true, true
                                , true, false, false
                                , false, true, false
                                , true, true, true);

                lModelConstraintCheck = PreAnalysis();


                ////Making the static part of the model
                MakeStaticPartOfProductPlatformModel();

                for (int i = 0; i < NoOfModelsRequired; i++)
                {

                    for (int lTransitionNo = 0; lTransitionNo < _maxNumberOfTransitions; lTransitionNo++)
                    {
                        _currentTransitionNumber = lTransitionNo;

                        _outputHandler.PrintMessageToConsole("--------------------Transition: " + lTransitionNo + " --------------------");

                        ////Making the dynamic part of the model (Operations and transition between operation status)
                        MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, lTransitionNo);

                        //Now the goal is added to the model
                        //Variation Point
                        if (ConvertGoal)
                            AddFindingModelGoal(lTransitionNo.ToString());

                        StopNReportModelTimingNStartAnalysisTiming("Model Creation");

                        //For each variant check if this statement holds - carry out the analysis
                        lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                                , 0
                                                                , lAnalysisComplete);

                        StopNReportModelTimingNStartAnalysisTiming("Model Analysis");

                        //if the result of the previous analysis is true then we go to the next analysis part
                        if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                        {
                            //If the result is true it means we have found a model.
                            int lTempInt2 = lTransitionNo + 1;

                            //cOutputHandler.printMessageToConsole("Model created in transition No: " + lTempInt2);

                            ReportSolverResult(lTempInt2, lAnalysisComplete, _frameworkWrapper, lInternalAnalysisResult, i);
                            break;
                        }

                        if (lTransitionNo == _maxNumberOfTransitions - 1)
                            lAnalysisComplete = true;


                    }
                }

                //Translating the internal analysis result to the user specific analysis result 
                if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    lAnalysisResult = true;
                else
                    lAnalysisResult = false;

                _outputHandler.PrintMessageToConsole("Analysis Report: ");

                if (ReportTimings)
                {
                    lStopWatch.Stop();
                    _modelAnalysisReportingTime = lStopWatch.ElapsedMilliseconds;
                    _outputHandler.PrintMessageToConsole("Model Analysis Reporting Time: " + _modelAnalysisReportingTime + "ms.");
                }

                //TODO: Do we need a stand alone constraint????????? Considering that this line comes with a stand alone constraint!!!
                //                        lZ3Solver.SolverPopFunction();

                //TODO: Is this the best way to end the loop on the transiton numbers???????
                //If all the transition cycles are completed then the analysis is completed
                //                }


                if (!lAnalysisResult)
                {
                    //If the result is false it means no deadlock can be found! HENCE DEADLOCK FREE!!
                    ReportSolverResult(_maxNumberOfTransitions, lAnalysisComplete, _frameworkWrapper, lInternalAnalysisResult, null);
                    _outputHandler.PrintMessageToConsole("NO deadlock was found!");
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ExistanceOfDeadlockAnalysis");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _outputHandler.PrintMessageToConsole("Model enumeration Analysis:");

                //This variable controls if the analysis has been completed or not
                bool lAnalysisComplete = false;

                _z3Solver.PrepareDebugDirectory();

                StartModelTiming();
                //This analysis is going to be carried out in two steps:
                //Step A, check the product platform structurally without the goal
                //        this check is done for each seperate variant from the variant group
                //        this checks to see if any of the rules are conflicting each other

                /*                if (!pVariationPointsSet)
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
                                , true, true, true);*/

                MakeStaticPartOfProductPlatformModel();

                StopNReportModelTimingNStartAnalysisTiming("Model Creation");

                for (int i = 0; i < NoOfModelsRequired; i++)
                {
                    _outputHandler.ResetOutputResult();
                    //For each variant check if this statement holds - carry out the analysis
                    lInternalAnalysisResult = AnalyzeProductPlatform(0
                                                            , i
                                                            , lAnalysisComplete
                                                            , false
                                                            , ""
                                                            , false);

                    StopNReportAnalysisTiming();

                    //if the result of the previous analysis is true then we go to the next analysis part
                    if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    {
                        //??addStandAloneConstraint2Z3Solver(lZ3Solver.FindBoolExpressionUsingName(lCurrentVariant.names));
                        ReportSolverResult(i
                                        , lAnalysisComplete
                                        , _frameworkWrapper
                                        , lInternalAnalysisResult
                                        , null);
                    }
                    else if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                    {
                        //If the product platform did not have any more models!
                        if (i.Equals(0))
                            _outputHandler.PrintMessageToConsole("This Product Platform DID NOT have any Models.");
                        else if (i > 0)
                            _outputHandler.PrintMessageToConsole("This Product Platform only had " + i + " Models.");
                        break;
                    }

                }

                //Translating the internal analysis result to the user specific analysis result 
                if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                    lAnalysisResult = true;
                else
                    lAnalysisResult = false;

                StopNReportModelAnalysisTiming();

                //In this analysis there was not a meaningful analysis report!
                //cOutputHandler.printMessageToConsole("Analysis Report: ");
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ProductModelEnumerationAnalysis");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
            bool lAnalysisResult = false;

            //This is the result of the internal analysis we get
            Status lInternalAnalysisResult = Status.UNKNOWN;
            Status lModelConstraintCheck = Status.UNSATISFIABLE;

            try
            {
                StartModelTiming();

                _outputHandler.PrintMessageToConsole("Variant Selectability Analysis:");

                bool lAnalysisComplete = false;


                //This is the list of all the variants/parts in the product platform
                HashSet<variant> lVariantList = _frameworkWrapper.VariantSet;

                //This is an empty list which is going to be filled by all he variants which are not able to be picked
                List<variant> lUnselectableVariantList = new List<variant>();

                _z3Solver.PrepareDebugDirectory();

                //This analysis is going to be carried out in two steps:
                //Step A, check the product platform structurally without the goal
                //        this check is done for each seperate variant from the variant group
                //        this checks to see if any of the rules are conflicting each other

                ////As this analysis type is a static analysis we only carry t out for the first transition
                int lTransitionNo = 0;
                _currentTransitionNumber = lTransitionNo;

                MakeStaticPartOfProductPlatformModel();

                MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, lTransitionNo);

                lModelConstraintCheck = AnalyzeProductPlatform(lTransitionNo
                                                        , 0
                                                        , lAnalysisComplete);

                if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                {
                    //Then we have to go over all variants one by one, for this we use the list we previously filled
                    foreach (variant lCurrentVariant in lVariantList)
                    {

                        _z3Solver.SolverPushFunction();

                        AddStandAloneConstraint2Z3Solver((BoolExpr)_z3Solver.FindExprInExprSet(lCurrentVariant.names));

                        StopNReportModelTimingNStartAnalysisTiming("Model Creation");

                        //For each variant check if this statement holds - carry out the analysis
                        lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                                , 0
                                                                , lAnalysisComplete);

                        //if the result of the previous analysis is true then we go to the next analysis part
                        if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                        {
                            ReportSolverResult(lTransitionNo
                                                , lAnalysisComplete
                                                , _frameworkWrapper
                                                , lInternalAnalysisResult
                                                , lCurrentVariant);
                        }
                        else if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                        {
                            //If the result of the first analysis was false that means the selected variant is in conflict with the rest of the product platform
                            if (!lUnselectableVariantList.Contains(lCurrentVariant))
                                lUnselectableVariantList.Add(lCurrentVariant);
                        }

                        StopNReportAnalysisTiming();

                        _z3Solver.SolverPopFunction();

                        //Here according to the number of unselectable variants which we have found we will give the coresponding report
                    }
                }

                _outputHandler.PrintMessageToConsole("Analysis Report: ");
                if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                {
                    if (lUnselectableVariantList.Count != 0)
                    {
                        _outputHandler.PrintMessageToConsole("Variats which are not selectable are: " + ReturnVariantNamesFromList(lUnselectableVariantList));
                        lAnalysisResult = false;
                    }
                    else
                    {
                        _outputHandler.PrintMessageToConsole("All Variats are selectable!");
                        lAnalysisResult = true;
                    }
                }
                else
                    _outputHandler.PrintMessageToConsole("There are conflicts between the model constraints!");

                StopNReportModelAnalysisTiming();
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in VariantSelectabilityGoal");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            //Returns the result of the analysis
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis checks if there are any parts which are not able to be selected and manufactured
        /// </summary>
        public bool PartSelectabilityAnalysis(bool pVariationPointsSet
                                                    , bool pReportTypeSet)
        {
            bool lAnalysisResult = false;

            //This is the result of the internal analysis we get
            Status lInternalAnalysisResult = Status.UNKNOWN;
            Status lModelConstraintCheck = Status.UNSATISFIABLE;

            try
            {
                StartModelTiming();

                _outputHandler.PrintMessageToConsole("Part Selectability Analysis:");

                bool lAnalysisComplete = false;


                //This is the list of all the variants/parts in the product platform
                HashSet<part> lPartList = _frameworkWrapper.PartSet;

                //This is an empty list which is going to be filled by all he variants which are not able to be picked
                List<part> lUnselectablePartList = new List<part>();

                _z3Solver.PrepareDebugDirectory();

                //This analysis is going to be carried out in two steps:
                //Step A, check the product platform structurally without the goal
                //        this check is done for each seperate variant from the variant group
                //        this checks to see if any of the rules are conflicting each other

                ////As this analysis type is a static analysis we only carry t out for the first transition
                int lTransitionNo = 0;
                _currentTransitionNumber = lTransitionNo;

                MakeStaticPartOfProductPlatformModel();

                MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, lTransitionNo);
                lModelConstraintCheck = AnalyzeProductPlatform(lTransitionNo
                                                        , 0
                                                        , lAnalysisComplete);

                if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                {
                    if (_frameworkWrapper.UsePartInfo)
                    {
                        //Then we have to go over all variants one by one, for this we use the list we previously filled
                        foreach (part lCurrentPart in lPartList)
                        {
                            _z3Solver.SolverPushFunction();

                            AddStandAloneConstraint2Z3Solver((BoolExpr)_z3Solver.FindExprInExprSet(lCurrentPart.names));

                            StopNReportModelTimingNStartAnalysisTiming("Model Creation");

                            //For each variant check if this statement holds - carry out the analysis
                            lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                                    , 0
                                                                    , lAnalysisComplete);

                            //if the result of the previous analysis is true then we go to the next analysis part
                            if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                            {
                                ReportSolverResult(lTransitionNo
                                                    , lAnalysisComplete
                                                    , _frameworkWrapper
                                                    , lInternalAnalysisResult
                                                    , lCurrentPart);

                            }
                            else if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                            {
                                //If the result of the first analysis was false that means the selected variant is in conflict with the rest of the product platform
                                if (!lUnselectablePartList.Contains(lCurrentPart))
                                    lUnselectablePartList.Add(lCurrentPart);
                            }

                            StopNReportModelAnalysisTiming();

                            _z3Solver.SolverPopFunction();

                            //Here according to the number of unselectable variants which we have found we will give the coresponding report
                        }

                    }
                    else
                    {
                        _outputHandler.PrintMessageToConsole("Parts are not available in this model!");
                    }
                }

                _outputHandler.PrintMessageToConsole("Analysis Report: ");
                if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                {
                    if (_frameworkWrapper.UsePartInfo)
                    {
                        if (lUnselectablePartList.Count != 0)
                        {
                            _outputHandler.PrintMessageToConsole("Parts which are not selectable are: " + ReturnPartNamesFromList(lUnselectablePartList));
                            lAnalysisResult = false;
                        }
                        else
                        {
                            _outputHandler.PrintMessageToConsole("All Parts are selectable!");
                            lAnalysisResult = true;
                        }
                    }
                }
                else
                    _outputHandler.PrintMessageToConsole("There are conflicts between the model constraints!");

                StopNReportModelAnalysisTiming();
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in PartSelectabilityGoal");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            //Returns the result of the analysis
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis checks if there are any variants which are never selected
        /// </summary>
        public bool NeverSelectedVariantAnalysis(bool pVariationPointsSet
                                                    , bool pReportTypeSet)
        {
            bool lAnalysisResult = false;

            Status lInternalAnalysisResult = Status.UNKNOWN;
            Status lModelConstraintCheck = Status.UNSATISFIABLE;

            try
            {
                StartModelTiming();

                _outputHandler.PrintMessageToConsole("Never selected variant Analysis:");

                HashSet<variant> lVariantList = _frameworkWrapper.VariantSet;

                if (lVariantList.Count > 0)
                {
                    bool lAnalysisComplete = false;

                    List<variant> lNeverSelectedVariantList = new List<variant>();
                    List<variant> lSelectedVariantList = new List<variant>();

                    _z3Solver.PrepareDebugDirectory();

                    //4. Is there any variant(s) that are NEVER selected for a configuration?
                    //V_i
                    //SAT:  There is a configuration where this variant is selected
                    //UNSAT: This variant is never selected

                    //This analysis only has meaning in the first transition, as it is a static analysis
                    _currentTransitionNumber = 0;

                    lAnalysisComplete = false;

                    MakeStaticPartOfProductPlatformModel();

                    MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, _currentTransitionNumber);

                    lModelConstraintCheck = AnalyzeProductPlatform(_currentTransitionNumber
                                                            , 0
                                                            , lAnalysisComplete);

                    if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                    {
                        //Then we have to go over all variants one by one
                        foreach (variant lCurrentVariant in lVariantList)
                        {

                            _z3Solver.SolverPushFunction();

                            BoolExpr lStandAloneConstraint = null;

                            //Here we have to build an expression which shows C => ! V_i and assign it to the variable just defined
                            BoolExpr lRightHandSide = null;
                            BoolExpr lLeftHandSide = null;

                            //V_i
                            lLeftHandSide = (BoolExpr)_z3Solver.FindExprInExprSet(lCurrentVariant.names);

                            lStandAloneConstraint = lLeftHandSide;

                            AddStandAloneConstraint2Z3Solver(lStandAloneConstraint);

                            StopNReportModelTimingNStartAnalysisTiming("Model Creation");

                            //For each variant check if this statement holds - carry out the analysis
                            lInternalAnalysisResult = AnalyzeProductPlatform(_currentTransitionNumber
                                                                    , 0
                                                                    , lAnalysisComplete);


                            if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                            {
                                if (!lNeverSelectedVariantList.Contains(lCurrentVariant))
                                    lNeverSelectedVariantList.Add(lCurrentVariant);
                            }

                            StopNReportAnalysisTiming();

                            //if it does hold we go to the next variant
                            _z3Solver.SolverPopFunction();

                        }
                    }


                    //Translating the internal analysis result to the user specific analysis result 
                    //As the analysis has been looking for variants which are not always selectable, hence if the lNotAlwaysSelectedVariantList
                    //contains any record then the analysis will be true, other wise it will be false
                    if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                    {
                        if (lSelectedVariantList.Count > 0)
                            lAnalysisResult = true;
                        else
                            lAnalysisResult = false;


                        if (lNeverSelectedVariantList.Count != 0)
                            _outputHandler.PrintMessageToConsole("Variats which can NEVER be selected are: " + ReturnVariantNamesFromList(lNeverSelectedVariantList));
                        else
                            _outputHandler.PrintMessageToConsole("Every Variat can be selected in at least one configuration!");


                    }
                    else
                        _outputHandler.PrintMessageToConsole("There are conflicts between the model constraints!");
                }
                else
                    _outputHandler.PrintMessageToConsole("There are no Variants to perform this analysis!");

                StopNReportModelAnalysisTiming();
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in NeverSelectedVariantAnalysis");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis checks if there are any variants which are always selected
        /// </summary>
        public bool AlwaysSelectedVariantAnalysis(bool pVariationPointsSet
                                                    , bool pReportTypeSet)
        {
            bool lAnalysisResult = false;

            Status lInternalAnalysisResult = Status.UNKNOWN;
            Status lModelConstraintCheck = Status.UNSATISFIABLE;

            try
            {
                StartModelTiming();

                _outputHandler.PrintMessageToConsole("Always selected variant Analysis:");

                HashSet<variant> lVariantList = _frameworkWrapper.VariantSet;

                if (lVariantList.Count > 0)
                {
                    bool lAnalysisComplete = false;

                    List<variant> lAlwaysSelectedVariantList = new List<variant>();
                    List<variant> lNotSelectedVariantList = new List<variant>();

                    _z3Solver.PrepareDebugDirectory();

                    //3. Is there any variant(s) that are ALWAYS selected for a configuration?
                    //!V_i
                    //SAT: There is a configuration where this variant is NOT selected
                    //UNSAT: All configurations include this variant

                    //This analysis only has meaning in the first transition, as it is a static analysis
                    _currentTransitionNumber = 0;

                    lAnalysisComplete = false;

                    MakeStaticPartOfProductPlatformModel();

                    MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, _currentTransitionNumber);

                    //This model is checked to see if there is no initial problem with the model's constraint set
                    lModelConstraintCheck = AnalyzeProductPlatform(_currentTransitionNumber
                                                            , 0
                                                            , lAnalysisComplete);

                    if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                    {
                        //Then we have to go over all variants one by one
                        foreach (variant lCurrentVariant in lVariantList)
                        {

                            _z3Solver.SolverPushFunction();

                            BoolExpr lStandAloneConstraint = null;

                            //Here we have to build an expression which shows C AND V_i and assign it to the variable just defined
                            BoolExpr lRightHandSide = null;
                            BoolExpr lLeftHandSide = null;

                            //!V_i
                            lLeftHandSide = _z3Solver.NotOperator((BoolExpr)_z3Solver.FindExprInExprSet(lCurrentVariant.names));

                            lStandAloneConstraint = lLeftHandSide;

                            AddStandAloneConstraint2Z3Solver(lStandAloneConstraint);

                            StopNReportModelTimingNStartAnalysisTiming("Model Creation");

                            //For each variant check if this statement holds - carry out the analysis
                            lInternalAnalysisResult = AnalyzeProductPlatform(_currentTransitionNumber
                                                                    , 0
                                                                    , lAnalysisComplete);


                            if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                            {
                                if (!lAlwaysSelectedVariantList.Contains(lCurrentVariant))
                                    lAlwaysSelectedVariantList.Add(lCurrentVariant);
                            }

                            StopNReportAnalysisTiming();

                            //if it does hold we go to the next variant
                            _z3Solver.SolverPopFunction();

                        }
                    }


                    //Translating the internal analysis result to the user specific analysis result 
                    //As the analysis has been looking for variants which are not always selectable, hence if the lNotAlwaysSelectedVariantList
                    //contains any record then the analysis will be true, other wise it will be false
                    if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                    {
                        if (lNotSelectedVariantList.Count > 0)
                            lAnalysisResult = true;
                        else
                            lAnalysisResult = false;


                        if (lAlwaysSelectedVariantList.Count != 0)
                            _outputHandler.PrintMessageToConsole("Variats which are ALWAYS selected are: " + ReturnVariantNamesFromList(lAlwaysSelectedVariantList));
                        else
                            _outputHandler.PrintMessageToConsole("None of the Variats are ALWAYS selected!");

                    }
                    else
                        _outputHandler.PrintMessageToConsole("There are conflicts between the model constraints!");
                }
                else
                    _outputHandler.PrintMessageToConsole("There are no Variants to perform this analysis!");


                StopNReportModelAnalysisTiming();
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AlwaysSelectedVariantAnalysis");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis checks if there are any parts which are always selected
        /// </summary>
        public bool AlwaysSelectedPartAnalysis(bool pVariationPointsSet
                                                    , bool pReportTypeSet)
        {
            bool lAnalysisResult = false;

            Status lInternalAnalysisResult = Status.UNKNOWN;
            Status lModelConstraintCheck = Status.UNSATISFIABLE;
            try
            {
                StartModelTiming();

                _outputHandler.PrintMessageToConsole("Always selected part Analysis:");

                HashSet<part> lPartList = _frameworkWrapper.PartSet;

                if (lPartList.Count > 0)
                {
                    bool lAnalysisComplete = false;

                    List<part> lAlwaysSelectedPartList = new List<part>();
                    List<part> lNotSelectedPartList = new List<part>();

                    _z3Solver.PrepareDebugDirectory();

                    //6. Is there any part(s) that are ALWAYS selected for a configuration?
                    //!P_i
                    //SAT: There is a configuration in which this part is not selected
                    //UNSAT: This part is selected in ALL configurations

                    //This analysis only has meaning in the first transition, as it is a static analysis
                    _currentTransitionNumber = 0;

                    lAnalysisComplete = false;

                    MakeStaticPartOfProductPlatformModel();

                    MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, _currentTransitionNumber);

                    lModelConstraintCheck = AnalyzeProductPlatform(_currentTransitionNumber
                                                            , 0
                                                            , lAnalysisComplete);

                    if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                    {
                        if (_frameworkWrapper.UsePartInfo)
                        {
                            foreach (part lCurrentPart in lPartList)
                            {
                                _z3Solver.SolverPushFunction();

                                BoolExpr lStandAloneConstraint = null;

                                //Here we have to build an expression which shows C => P_i and assign it to the variable just defined
                                BoolExpr lRightHandSide = null;
                                BoolExpr lLeftHandSide = null;

                                //!P_i 
                                lLeftHandSide = _z3Solver.NotOperator((BoolExpr)_z3Solver.FindExprInExprSet(lCurrentPart.names));

                                lStandAloneConstraint = lLeftHandSide;

                                AddStandAloneConstraint2Z3Solver(lStandAloneConstraint);

                                StopNReportModelTimingNStartAnalysisTiming("Model Creation");

                                //For each part check if this statement holds - carry out the analysis
                                lInternalAnalysisResult = AnalyzeProductPlatform(_currentTransitionNumber
                                                                        , 0
                                                                        , lAnalysisComplete);

                                if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                                {
                                    if (!lAlwaysSelectedPartList.Contains(lCurrentPart))
                                        lAlwaysSelectedPartList.Add(lCurrentPart);
                                }

                                StopNReportAnalysisTiming();

                                //if it does hold we go to the next part
                                _z3Solver.SolverPopFunction();

                            }

                        }
                    }
                    //Translating the internal analysis result to the user specific analysis result 
                    //As the analysis has been looking for variants which are not always selectable, hence if the lNotAlwaysSelectedVariantList
                    //contains any record then the analysis will be true, other wise it will be false
                    if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                    {
                        if (_frameworkWrapper.UsePartInfo)
                        {
                            if (lNotSelectedPartList.Count > 0)
                                lAnalysisResult = true;
                            else
                                lAnalysisResult = false;
                        }


                        if (_frameworkWrapper.UsePartInfo)
                        {
                            if (lAlwaysSelectedPartList.Count != 0)
                                _outputHandler.PrintMessageToConsole("Parts which are ALWAYS selected are: " + ReturnPartNamesFromList(lAlwaysSelectedPartList));
                            else
                                _outputHandler.PrintMessageToConsole("None of the Parts are ALWAYS selected!");

                        }
                    }
                    else
                        _outputHandler.PrintMessageToConsole("There are conflicts between the model constraints!");

                }
                else
                    _outputHandler.PrintMessageToConsole("There are no Parts to perform this analysis!");

                StopNReportModelAnalysisTiming();
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AlwaysSelectedPartAnalysis");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis checks if there are any parts which are never selected
        /// </summary>
        public bool NeverSelectedPartAnalysis(bool pVariationPointsSet
                                                    , bool pReportTypeSet)
        {
            bool lAnalysisResult = false;

            Status lInternalAnalysisResult = Status.UNKNOWN;
            Status lModelConstraintCheck = Status.UNSATISFIABLE;
            try
            {
                StartModelTiming();

                _outputHandler.PrintMessageToConsole("Never selected part Analysis:");

                HashSet<part> lPartList = _frameworkWrapper.PartSet;

                if (lPartList.Count > 0)
                {
                    bool lAnalysisComplete = false;

                    List<part> lNeverSelectedPartList = new List<part>();
                    List<part> lSelectedPartList = new List<part>();

                    _z3Solver.PrepareDebugDirectory();

                    //7. Is there any part(s) that are NEVER selected for a configuration?
                    //P_i
                    //SAT: There is a configuration where this part is selected
                    //UNSAT: There is NO configuration where this part is selected

                    //This analysis only has meaning in the first transition, as it is a static analysis
                    _currentTransitionNumber = 0;

                    lAnalysisComplete = false;

                    MakeStaticPartOfProductPlatformModel();

                    MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, _currentTransitionNumber);

                    lModelConstraintCheck = AnalyzeProductPlatform(_currentTransitionNumber
                                                            , 0
                                                            , lAnalysisComplete);

                    if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                    {
                        if (_frameworkWrapper.UsePartInfo)
                        {
                            foreach (part lCurrentPart in lPartList)
                            {
                                _z3Solver.SolverPushFunction();

                                BoolExpr lStandAloneConstraint = null;

                                //Here we have to build an expression which shows C AND ! P_i and assign it to the variable just defined
                                BoolExpr lRightHandSide = null;
                                BoolExpr lLeftHandSide = null;

                                //P_i 
                                lLeftHandSide = (BoolExpr)_z3Solver.FindExprInExprSet(lCurrentPart.names);

                                lStandAloneConstraint = lLeftHandSide;

                                AddStandAloneConstraint2Z3Solver(lStandAloneConstraint);

                                StopNReportModelTimingNStartAnalysisTiming("Model Creation");

                                //For each part check if this statement holds - carry out the analysis
                                lInternalAnalysisResult = AnalyzeProductPlatform(_currentTransitionNumber
                                                                        , 0
                                                                        , lAnalysisComplete);

                                if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                                {
                                    if (!lNeverSelectedPartList.Contains(lCurrentPart))
                                        lNeverSelectedPartList.Add(lCurrentPart);
                                }

                                StopNReportAnalysisTiming();

                                //if it does hold we go to the next part
                                _z3Solver.SolverPopFunction();

                            }

                        }
                    }
                    //Translating the internal analysis result to the user specific analysis result 
                    //As the analysis has been looking for variants which are not always selectable, hence if the lNotAlwaysSelectedVariantList
                    //contains any record then the analysis will be true, other wise it will be false
                    if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                    {
                        if (_frameworkWrapper.UsePartInfo)
                        {
                            if (lNeverSelectedPartList.Count > 0)
                                lAnalysisResult = true;
                            else
                                lAnalysisResult = false;
                        }


                        if (_frameworkWrapper.UsePartInfo)
                        {
                            if (lNeverSelectedPartList.Count != 0)
                                _outputHandler.PrintMessageToConsole("Parts which are NEVER selected are: " + ReturnPartNamesFromList(lNeverSelectedPartList));
                            else
                                _outputHandler.PrintMessageToConsole("Every Part can be selected in at least one configuration!");

                        }
                    }
                    else
                        _outputHandler.PrintMessageToConsole("There are conflicts between the model constraints!");
                }
                else
                    _outputHandler.PrintMessageToConsole("There are no Parts to perform this analysis!");


                StopNReportModelAnalysisTiming();
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in NeverSelectedPartAnalysis");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lAnalysisResult;
        }

        /// <summary>
        /// This analysis checks if there are any operations which are never selected
        /// </summary>
        public bool NeverSelectedOperationAnalysis(bool pVariationPointsSet
                                                    , bool pReportTypeSet)
        {
            bool lAnalysisResult = false;

            Status lInternalAnalysisResult = Status.UNKNOWN;
            Status lModelConstraintCheck = Status.UNSATISFIABLE;

            try
            {
                StartModelTiming();

                _outputHandler.PrintMessageToConsole("Never selected operation Analysis:");

                HashSet<Operation> lOperationSet = _frameworkWrapper.OperationSet;

                if (lOperationSet.Count > 0)
                {
                    //10. Is there any operation(s) that are NEVER selected for a configuration?
                    //O_i_initial
                    //SAT: There is one configuration where this operation is used
                    //UNSAT: This operation is NEVER used

                    bool lAnalysisComplete = false;
                    HashSet<string> lNeverSelectedOperationNameList = new HashSet<string>();
                    HashSet<string> lSelectedOperationNameList = new HashSet<string>();

                    _z3Solver.PrepareDebugDirectory();

                    ////In this analysis we only want to check the first transition, and check if each operation is in the initial status in the first transition or not
                    ////Hence there is no need to loop over all transitions like previous types of analysis
                    _currentTransitionNumber = 0;

                    MakeStaticPartOfProductPlatformModel();

                    MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, _currentTransitionNumber);

                    lModelConstraintCheck = AnalyzeProductPlatform(_currentTransitionNumber
                                                            , 0
                                                            , lAnalysisComplete);

                    if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                    {
                        foreach (Operation lCurrentOperation in lOperationSet)
                        {
                            BoolExpr l = ConvertOperationInstanceWithMissingTransitionNumber(lCurrentOperation
                                                                                            , Enumerations.OperationInstanceState.Initial
                                                                                            , "0");

                            _z3Solver.SolverPushFunction();

                            BoolExpr lStandAloneConstraint = null;

                            /////                            //TODO: Here we have to build an expression which shows C and P => !O_u and assign it to the variable just defined!!!!!!!!!!!!!!!!!!!!!!!!!
                            //Here we have to build an expression which shows C and O_u and assign it to the variable just defined!!!!!!!!!!!!!!!!!!!!!!!!!
                            BoolExpr lRightHandSide = null;
                            BoolExpr lLeftHandSide = null;

                            lLeftHandSide = l;

                            lStandAloneConstraint = lLeftHandSide;

                            AddStandAloneConstraint2Z3Solver(lStandAloneConstraint);

                            StopNReportModelTimingNStartAnalysisTiming("Model Creation");

                            //For each variant check if this statement holds - carry out the analysis
                            lInternalAnalysisResult = AnalyzeProductPlatform(_currentTransitionNumber
                                                                    , 0
                                                                    , lAnalysisComplete);
                            /////                            if (!lAnalysisResult && !lAnalysisComplete)

                            if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                            {
                                //This line is moved to the reporting procedure in this class
                                //if it does hold, then there exists a valid configuration in which the current operation is UNUSED!
                                //cOutputHandler.printMessageToConsole("There DOES exist a configuration in which " + lOperationName + " is in an UNUSED state!");

                                if (!lNeverSelectedOperationNameList.Contains(lCurrentOperation.Name))
                                    lNeverSelectedOperationNameList.Add(lCurrentOperation.Name);
                            }

                            StopNReportAnalysisTiming();

                            _z3Solver.SolverPopFunction();

                        }
                    }


                    if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                    {
                        //Translating the internal analysis result to the user specific analysis result 
                        if (lNeverSelectedOperationNameList.Count > 0)
                            lAnalysisResult = true;
                        else
                            lAnalysisResult = false;

                        if (lNeverSelectedOperationNameList.Count != 0)
                            _outputHandler.PrintMessageToConsole("Operations which are NEVER selected are: "
                                + ReturnOperationNamesStringFromOperationNameList(lNeverSelectedOperationNameList));
                        else
                            _outputHandler.PrintMessageToConsole("Every operation can be selected in at least one configuration!");

                    }
                    else
                        _outputHandler.PrintMessageToConsole("There are conflicts between the model constraints!");
                }
                else
                    _outputHandler.PrintMessageToConsole("There are no Operations to perform this analysis!");

                StopNReportModelAnalysisTiming();
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in NeverSelectedOperationAnalysis");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
            Status lModelConstraintCheck = Status.UNSATISFIABLE;

            try
            {
                StartModelTiming();

                _outputHandler.PrintMessageToConsole("Always selected operation Analysis:");

                HashSet<Operation> lOperationSet = _frameworkWrapper.OperationSet;

                if (lOperationSet.Count > 0)
                {
                    //9. Is there any operation(s) that are ALWAYS selected for a configuration?
                    //O_i_unused
                    //SAT: This operation is NEVER used
                    //UNSAT: There is one configuration where this operation is used

                    bool lAnalysisComplete = false;
                    HashSet<string> lAlwaysSelectedOperationNameList = new HashSet<string>();
                    HashSet<string> lUnusedOperationNameList = new HashSet<string>();

                    _z3Solver.PrepareDebugDirectory();

                    ////In this analysis we only want to check the first transition, and check if each operation is in the initial status in the first transition or not
                    ////Hence there is no need to loop over all transitions like previous types of analysis
                    _currentTransitionNumber = 0;

                    MakeStaticPartOfProductPlatformModel();

                    MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, _currentTransitionNumber);

                    lModelConstraintCheck = AnalyzeProductPlatform(_currentTransitionNumber
                                                            , 0
                                                            , lAnalysisComplete);

                    if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                    {

                        foreach (Operation lCurrentOperation in lOperationSet)
                        {
                            BoolExpr l = ConvertOperationInstanceWithMissingTransitionNumber(lCurrentOperation
                                                                                            , Enumerations.OperationInstanceState.Unused
                                                                                            , "0");

                            _z3Solver.SolverPushFunction();

                            BoolExpr lStandAloneConstraint = null;

                            /////                            //TODO: Here we have to build an expression which shows C and P => !O_u and assign it to the variable just defined!!!!!!!!!!!!!!!!!!!!!!!!!
                            //Here we have to build an expression which shows C and O_u and assign it to the variable just defined!!!!!!!!!!!!!!!!!!!!!!!!!
                            BoolExpr lRightHandSide = null;
                            BoolExpr lLeftHandSide = null;

                            lLeftHandSide = l;

                            lStandAloneConstraint = lLeftHandSide;

                            AddStandAloneConstraint2Z3Solver(lStandAloneConstraint);

                            StopNReportModelTimingNStartAnalysisTiming("Model Creation");

                            //For each variant check if this statement holds - carry out the analysis
                            lInternalAnalysisResult = AnalyzeProductPlatform(_currentTransitionNumber
                                                                    , 0
                                                                    , lAnalysisComplete);
                            /////                            if (!lAnalysisResult && !lAnalysisComplete)

                            if (lInternalAnalysisResult.Equals(Status.UNSATISFIABLE))
                            {
                                //if it does hold, then there exists a valid configuration in which the current operation is UNUSED!

                                if (!lAlwaysSelectedOperationNameList.Contains(lCurrentOperation.Name))
                                    lAlwaysSelectedOperationNameList.Add(lCurrentOperation.Name);
                            }

                            StopNReportAnalysisTiming();

                            _z3Solver.SolverPopFunction();

                        }
                    }


                    if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                    {
                        //Translating the internal analysis result to the user specific analysis result 
                        if (lAlwaysSelectedOperationNameList.Count > 0)
                            lAnalysisResult = true;
                        else
                            lAnalysisResult = false;

                        if (lAlwaysSelectedOperationNameList.Count != 0)
                            _outputHandler.PrintMessageToConsole("Operations which are ALWAYS USED are: "
                                + ReturnOperationNamesStringFromOperationNameList(lAlwaysSelectedOperationNameList));
                        else
                            _outputHandler.PrintMessageToConsole("None of the operations is ALWAYS selected!");

                    }
                    else
                        _outputHandler.PrintMessageToConsole("There are conflicts between the model constraints!");
                }
                else
                    _outputHandler.PrintMessageToConsole("There are no Operations to perform this analysis!");

                StopNReportModelAnalysisTiming();
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in AlwaysSelectedOperationAnalysis");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
            Status lModelConstraintCheck = Status.UNSATISFIABLE;

            try
            {
                StartModelTiming();

                _outputHandler.PrintMessageToConsole("Operation Selectability Analysis:");

                //11. Is there any operation(s) which is possible to choose for a configuration?
                //O_i

                bool lAnalysisComplete = false;
                HashSet<string> lUnselectableOperationNameSet = new HashSet<string>();
                //HashSet<OperationInstance> lOperationInstanceList = cFrameworkWrapper.OperationInstanceSet;

                //To start all operations are unselectable unless otherwise proven
                lUnselectableOperationNameSet = _frameworkWrapper.GetSetOfOperationNames();

                ////In this analysis we only want to check the first transition, and check if each operation is in the initial status in the first transition or not
                ////Hence there is no need to loop over all transitions like previous types of analysis
                int lTransitionNo = 0;

                MakeStaticPartOfProductPlatformModel();

                MakeDynamicPartOfProductPlatformModel(lAnalysisComplete, lTransitionNo);

                lModelConstraintCheck = AnalyzeProductPlatform(lTransitionNo
                                                        , 0
                                                        , lAnalysisComplete);

                if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                {
                    HashSet<Operation> lOperationSet = _frameworkWrapper.OperationSet;

                    foreach (Operation lCurrentOperation in lOperationSet)
                    {
                        BoolExpr l = ConvertOperationInstanceWithMissingTransitionNumber(lCurrentOperation
                                                                        , Enumerations.OperationInstanceState.Initial
                                                                        , lTransitionNo.ToString());

                        _z3Solver.SolverPushFunction();

                        AddStandAloneConstraint2Z3Solver(l);

                        StopNReportModelTimingNStartAnalysisTiming("Model Creation");

                        //For each operation check if this statement holds - carry out the analysis
                        lInternalAnalysisResult = AnalyzeProductPlatform(lTransitionNo
                                                                , 0
                                                                , lAnalysisComplete);

                        if (lInternalAnalysisResult.Equals(Status.SATISFIABLE))
                        //if it does hold, then it is possible to select that operation and it should be removed from the list
                        {
                            ReportSolverResult(lTransitionNo
                                                , lAnalysisComplete
                                                , _frameworkWrapper
                                                , lInternalAnalysisResult
                                                , lCurrentOperation.Name);

                            if (lUnselectableOperationNameSet.Contains(lCurrentOperation.Name))
                                lUnselectableOperationNameSet.Remove(lCurrentOperation.Name);
                        }
                        else
                        {
                            //If the operation is inactive?
                            //This means the operation instance is inactive hence it should be mentioned

                            //This is when we want to report only the operation name
                            ReportSolverResult(lTransitionNo
                                                , lAnalysisComplete
                                                , _frameworkWrapper
                                                , lInternalAnalysisResult
                                                , lCurrentOperation.Name);

                            //This is when we want to report the operation instance
                            if (lInternalAnalysisResult == Status.SATISFIABLE)
                                if (lUnselectableOperationNameSet.Contains(lCurrentOperation.Name))
                                    lUnselectableOperationNameSet.Remove(lCurrentOperation.Name);
                        }
                        StopNReportAnalysisTiming();

                        _z3Solver.SolverPopFunction();
                    }
                }

                if (lModelConstraintCheck.Equals(Status.SATISFIABLE))
                {
                    //Translating the internal analysis result to the user specific analysis result 
                    if (lUnselectableOperationNameSet.Count.Equals(0))
                        lAnalysisResult = true;
                    else
                        lAnalysisResult = false;


                    if (ReportAnalysisResult)
                    {
                        if (lUnselectableOperationNameSet.Count != 0)
                            _outputHandler.PrintMessageToConsole("Operations which are not selectable are: " + ReturnOperationNamesStringFromOperationNameList(lUnselectableOperationNameSet));
                        else
                            _outputHandler.PrintMessageToConsole("All operations are selectable!");
                    }
                }
                else
                    _outputHandler.PrintMessageToConsole("There are conflicts between the model constraints!");

                StopNReportModelAnalysisTiming();

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in OperationSelectabilityAnalysis");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                ///int lMaxNoOfTransitions = CalculateAnalysisNoOfCycles();
                for (int i = 0; i < _maxNumberOfTransitions; i++)
                {
                    BoolExpr lCurrentOperationInstance;
                    if (pSpecificTransitionNo != -1 && pSpecificTransitionNo.Equals(i))
                    {
                        lCurrentOperationInstance = (BoolExpr)_z3Solver.FindExprInExprSet(pOperationInstance.FinishedVariableName);
                    }
                    else
                    {
                        OperationInstance lTempOperationInstance = new OperationInstance(pOperationInstance.AbstractOperation
                                                                                        , i.ToString());
                        lCurrentOperationInstance = (BoolExpr)_z3Solver.FindExprInExprSet(lTempOperationInstance.FinishedVariableName);

                    }
                    if (lCurrentOperationInstance != null)
                        if (lResultExpr != null)
                            lResultExpr = _z3Solver.AndOperator(new List<BoolExpr>() { lResultExpr, lCurrentOperationInstance });
                        else
                            lResultExpr = lCurrentOperationInstance;
                    else
                        break;

                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in OperationInAllTransitions");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                                                                            , string pTransitionNo = "-1"
                                                                            , bool pKeepOriginalVariables = false
                                                                            , bool pAdd2List = true)
        {
            BoolExpr lResultExpr = null;
            try
            {
                //Here we have an operation instance but as the transition number is not given we are looking for
                //for operation instances in the current transition and or previous transition of the transitions

                BoolExpr lCurrentOperationInstance = null;
                if (!pTransitionNo.Equals(-1))
                {
                    OperationInstance lTempOperationInstance = new OperationInstance(pOperation
                                                                                    , pTransitionNo.ToString()
                                                                                    , pAdd2List);
                    lCurrentOperationInstance = ReturnOperationInstanceVariableInOneState(lTempOperationInstance, pOperationState);

                    if (lCurrentOperationInstance != null)
                    {
                        if (lResultExpr != null)
                            lResultExpr = _z3Solver.OrOperator(new List<BoolExpr>() { lResultExpr, lCurrentOperationInstance });
                        else
                            lResultExpr = lCurrentOperationInstance;
                    }
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in OperationInAllTransitions");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                        lResultOperationInstanceVariable = (BoolExpr)_z3Solver.FindExprInExprSet(pOperationInstance.InitialVariableName);
                        break;
                    case Enumerations.OperationInstanceState.Executing:
                        lResultOperationInstanceVariable = (BoolExpr)_z3Solver.FindExprInExprSet(pOperationInstance.ExecutingVariableName);
                        break;
                    case Enumerations.OperationInstanceState.Finished:
                        lResultOperationInstanceVariable = (BoolExpr)_z3Solver.FindExprInExprSet(pOperationInstance.FinishedVariableName);
                        break;
                    case Enumerations.OperationInstanceState.Unused:
                        lResultOperationInstanceVariable = (BoolExpr)_z3Solver.FindExprInExprSet(pOperationInstance.UnusedVariableName);
                        break;
                    default:
                        lResultOperationInstanceVariable = (BoolExpr)_z3Solver.FindExprInExprSet(pOperationInstance.UnusedVariableName);
                        break;
                }

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ReturnOperationInstanceVariableInOneState");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _outputHandler.PrintMessageToConsole("error in ReturnPartNamesFromList");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _outputHandler.PrintMessageToConsole("error in ReturnVariantNamesFromList");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _outputHandler.PrintMessageToConsole("error in ReturnOperationNamesFromList");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                _outputHandler.PrintMessageToConsole("error in ReturnOperationNamesFromOpertionInstanceList");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
                        lOperationNamesString += "\n, ";

                    lOperationNamesString += lOperationName;
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in ReturnOperationNamesStringFromOperationNameList");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lOperationNamesString;
        }

        public void AddFindingModelGoal(string pTransitionNo)
        {
            try
            {
                BoolExpr lResultFormula = null;
                //HashSet<OperationInstance> lOperationInstanceSet = cFrameworkWrapper.OperationInstanceSet;
                //Forall Operation Instances: (O_F_0_m OR O_U_0_m) AND (O_F_1_m OR O_U_1_m) AND ... AND (O_F_n_m OR O_U_n_m))

                //TODO: This has to be fed into a class instance variable, as it is done in another part of this class as well!!
                //Here we calculate the maximum number of loops the analysis has to have, which is according to the maximum number of operations
                ///int lMaxNoOfTransitions = CalculateAnalysisNoOfCycles();

                //Filter to only operation instances of the current transition
                ///int lTempInteger = int.Parse(lMaxNoOfTransitions) - 1;
                ///string lTempString = lTempInteger.ToString();
                //List<OperationInstance> lCurrentTrnasitionNoOperationInstanceSet = filterOperationInstancesOfOneTransition(lOperationInstanceSet, pTransitionNo);
                HashSet<OperationInstance> lCurrentTrnasitionNoOperationInstanceSet = _frameworkWrapper.GetOperationInstancesInOneTransition(int.Parse(pTransitionNo));
                
                //List of : (O_F_0_m OR O_U_0_m), (O_F_1_m OR O_U_1_m), ..., (O_F_n_m OR O_U_n_m)
                List<BoolExpr> lWantedExprForEachVariant = ReturnWantedExprForEachVariantORPart(lCurrentTrnasitionNoOperationInstanceSet);

                //BoolExpr lCurrentTransitionFormulaPart = returnPartsANDedExpression(lWantedExprForEachVariant);

                lResultFormula = _z3Solver.AndOperator(lWantedExprForEachVariant);

                _z3Solver.AddConstraintToSolver(lResultFormula, "FindingModelGoal");
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in convertFindingModelGoal");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
        }

        public List<BoolExpr> ReturnWantedExprForEachVariantORPart(HashSet<OperationInstance> pCurrentTransitionOperationInstanceList)
        {
            //List of : (O_F_0_m OR O_U_0_m), (O_F_1_m OR O_U_1_m), ..., (O_F_n_m OR O_U_n_m)
            //The transition 'm' is given from the set in the parameter

            List<BoolExpr> lResultWantedExpr = new List<BoolExpr>();
            try
            {
                foreach (var lCurrentOperationInstance in pCurrentTransitionOperationInstanceList)
                {

                    BoolExpr lCurrentOperationInstanceExpr = _z3Solver.OrOperator(new List<string>() { lCurrentOperationInstance.FinishedVariableName
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
                _outputHandler.PrintMessageToConsole("error in returnWantedExprForEachPart");
                _outputHandler.PrintMessageToConsole(ex.Message);
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

        public List<OperationInstance> FilterOperationInstancesOfOneTransition(HashSet<OperationInstance> pOperationInstanceList, string pTransitionNo)
        {
            List<OperationInstance> lTempOperationInstanceList = new List<OperationInstance>();
            try
            {
                foreach (var lOperationInstance in pOperationInstanceList)
                {
                    string lOperationTransitionNo = lOperationInstance.TransitionNumber;
                    if (lOperationTransitionNo.Equals(pTransitionNo))
                    {
                        lTempOperationInstanceList.Add(lOperationInstance);
                    }
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in filterOperationInstancesOfOneTransition");
                _outputHandler.PrintMessageToConsole(ex.Message);
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
        /// <param name="pTransitionNo"></param>
        public BoolExpr InitializeDeadlockRule(int pTransitionNo)
        {
            BoolExpr lDeadlockRule = null;
            BoolExpr lDeadlockVariable = null;
            try
            {

                //This boolean expression is used to refer to this overall goal
                lDeadlockVariable = _z3Solver.AddBooleanExpression("Deadlock_" + pTransitionNo);
                //What we add to the solver is: P_i => (formula 7) AND (formula 8)

                //HashSet<OperationInstance> lOperationInstanceSet = cFrameworkWrapper.OperationInstanceSet;
                HashSet<OperationInstance> lOperationInstanceSet = _frameworkWrapper.GetOperationInstancesInOneTransition(pTransitionNo);

                //Meaning: NO operation can preceed!
                //(Big And) ((! Pre_k_pState AND O_I_k_pState) OR (! Post_k_pState AND O_E_k_pState) OR O_F_k_pState OR O_U_k_pState)
                BoolExpr lFormula7;
                lFormula7 = CreateFormula7(lOperationInstanceSet, pTransitionNo);

                //Meaning: At least ONE operation is in the initial or executing state!
                //(Big OR) (O_I_k_pState OR O_E_k_pState)
                BoolExpr lFormula8;
                lFormula8 = CreateFormula8(lOperationInstanceSet, pTransitionNo);

                if (lFormula7 != null && lFormula8 != null)
                {
                    lDeadlockRule = _z3Solver.AndOperator(new List<BoolExpr>() { (BoolExpr)_z3Solver.FindExprInExprSet("F7_" + pTransitionNo)
                                                                                , (BoolExpr)_z3Solver.FindExprInExprSet("F8_" + pTransitionNo) });

                    //lOverallGoal = cZ3Solver.OrOperator(new List<BoolExpr>() { lTempExpr, cZ3Solver.NotOperator(lProgressionRules) });

                    _z3Solver.AddTwoWayImpliesOperator2Constraints(lDeadlockVariable
                                                                , lDeadlockRule
                                                                , "Deadlock Rule");
                }
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("Error in InitializeDeadlockRule");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lDeadlockVariable;
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

        public void InitializeDataFiles()
        {
            try
            {
                //Parts
                _frameworkWrapper.PartSet.Clear();
                _frameworkWrapper.PartNameLookup.Clear();
                _frameworkWrapper.PartIndexLookup.Clear();
                _frameworkWrapper.PartSymbolicNameLookup.Clear();

                //Variants
                _frameworkWrapper.VariantSet.Clear();
                _frameworkWrapper.VariantNameLookup.Clear();
                _frameworkWrapper.IndexVariantLookup.Clear();
                _frameworkWrapper.VariantIndexLookup.Clear();
                _frameworkWrapper.VariantSymbolicNameLookup.Clear();

                //Variant group
                _frameworkWrapper.VariantGroupSet.Clear();

                //Constraint
                _frameworkWrapper.ConstraintSet.Clear();

                //Operations
                _frameworkWrapper.OperationSet.Clear();
                _frameworkWrapper.OperationNameLookup.Clear();
                _frameworkWrapper.OperationSymbolicNameLookup.Clear();

                //cFrameworkWrapper.OperationInstanceSet.Clear();
                //cFrameworkWrapper.OperationInstanceDictionary.Clear();

                //Item usadge rules
                _frameworkWrapper.ItemUsageRuleSet.Clear();

                //Traits
                _frameworkWrapper.TraitSet.Clear();
                _frameworkWrapper.TraitNameLookup.Clear();
                _frameworkWrapper.TraitSymbolicNameLookup.Clear();

                //Resources
                _frameworkWrapper.ResourceSet.Clear();
                _frameworkWrapper.ResourceNameLookup.Clear();
                _frameworkWrapper.ResourceSymbolicNameLookup.Clear();

            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in InitializeDataFiles");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
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
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine("Initial data importing...");

                InitializeDataFiles();

                bool lPartsLoaded = false;
                lPartsLoaded = _frameworkWrapper.CreatePartInstances(xDoc);

                bool lVariantsLoaded = false;
                lVariantsLoaded = _frameworkWrapper.CreateVariantInstances(xDoc);

                bool lVariantGroupsLoaded = false;
                lVariantGroupsLoaded = _frameworkWrapper.CreateVariantGroupInstances(xDoc);

                bool lConstraintsLoaded = false;
                lConstraintsLoaded = _frameworkWrapper.CreateConstraintInstances(xDoc);

                bool lOperationsLoaded = false;
                lOperationsLoaded = _frameworkWrapper.CreateOperationInstances(xDoc);

                bool lItemUsageRulesLoaded = false;
                lItemUsageRulesLoaded = _frameworkWrapper.CreateItemUsageRulesInstances(xDoc);

                //These two parts are not needed because the trigger field in the operationinstance object will serve this purpose
                /*
                bool lPartOperationLoaded = false;
                lPartOperationLoaded = cFrameworkWrapper.createPartOperationsInstances(xDoc);

                bool lVariantOperationLoaded = false;
                lVariantOperationLoaded = cFrameworkWrapper.createVariantOperationsInstances(xDoc);
                */

                bool lTraitsLoaded = false;
                lTraitsLoaded = _frameworkWrapper.CreateTraitInstances(xDoc);

                bool lResourceLoaded = false;
                lResourceLoaded = _frameworkWrapper.CreateResourceInstances(xDoc);


                lDataLoaded = ((lVariantsLoaded && lVariantGroupsLoaded) && lOperationsLoaded);

                Console.WriteLine("-----------------------------------------------------------");
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in LoadInitialDataFromXMLFile, FilePath: " + pFilePath);
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lDataLoaded;
        }

        /// <summary>
        /// This function returns the number of analysis cycles which are needed 
        /// This will be the number of operations times 2 
        /// as each operation will have two transitions between its states
        /// </summary>
        /// <returns>The number of cycles needed for the analysis</returns>
        public int CalculateAnalysisNoOfCycles()
        {
            int lResultNoOfCycles = 0;
            try
            {
                //NoOfCycles = NumOfOperations * NumOfTransitions
                ////TODO: It should be the second one because that would be more accurate but at the time of this calculation the number of active operations is not known! Maybe can be fixed!
                int lNoOfOperations = _frameworkWrapper.OperationSet.Count();
                //int lNoOfOperations = lFrameworkWrapper.getNumberOfActiveOperations();


                //Each operation will have two transitions
                //lResultNoOfCycles = (lNoOfOperations * 2) + 1;

                //------------------------------------------------------------------------------------------------------------
                //New version: in this version we have to calculate the number of transitions more accuratly

                //First we need to have the static part of the model

                List<IntExpr> lListOfOperationUsedVariables = new List<IntExpr>();
                //We do this by defining an interger variable for each operation
                foreach (Operation lCurrentOperation in _frameworkWrapper.OperationSet)
                {
                    //(declare-const O1_Used Int)
                    string lOperationVariableName = lCurrentOperation.Name + "_Used";
                    IntExpr lOperationUsedVariable = _z3Solver.AddIntegerExpression(lOperationVariableName, "Optimizer");

                    lListOfOperationUsedVariables.Add(lOperationUsedVariable);
                    lListOfOperationUsedVariables.Add(lOperationUsedVariable);

                    //(assert (< O1_Used 2))
                    BoolExpr lAssertion = _z3Solver.LessThanOperator(lOperationUsedVariable, 2);
                    _z3Solver.AddConstraintToOptimizer(lAssertion, "OperationUsedVariableUpperBound");

                    //We also need to connect the variable of each operation to the trigger expression of the operation
                    //(declare-const O1_Trigger Bool)
                    //(assert (and (=> O1_Trigger (or V2 V1)) (=> (or V2 V1) O1_Trigger))); Constraint 2 , Source: Operation_Trigger_Initializing
                    DefiningNInitializingOperationTriggerAttributes(lCurrentOperation, "Optimizer");
                    Expr lOperationTriggerVariable = _z3Solver.FindExprInExprSet(lCurrentOperation.OperationTriggerVariableName, false, "Optimizer");
                    //(assert (=> O1_Trigger (= O1_Used 1)))
                    _z3Solver.AddConstraintToOptimizer(_z3Solver.ImpliesOperator(new List<BoolExpr>{(BoolExpr)lOperationTriggerVariable
                                                                                , _z3Solver.EqualOperator(lOperationUsedVariable, 1)}), "InitializingOperationUsedVariable");
                    //(assert (=> (not O1_Trigger) (= O1_Used 0)))
                    _z3Solver.AddConstraintToOptimizer(_z3Solver.ImpliesOperator(new List<BoolExpr>{_z3Solver.NotOperator((BoolExpr)lOperationTriggerVariable)
                                                                                , _z3Solver.EqualOperator(lOperationUsedVariable, 0)}), "InitializingOperationUsedVariable");
                
                }


                //Then we will try to maximize the function which is built from adding up these variables on the operations
                //(declare-const MaxNoOfOpsUsed Int)
                Expr lMaximizeVariable = _z3Solver.AddIntegerExpression("MaxNoOfOperationUsed", "Optimizer");
                //Expr lMaximizeVariable = cZ3Solver.FindExprInExprSet("MaxNoOfOperationUsed");
                //(assert (= MaxNoOfOpsUsed (+ O1_Used O2_Used)))
                
                IntExpr lAddedOperationUsedVariables = _z3Solver.AddOperator(lListOfOperationUsedVariables);
                //IntExpr l2Expr = cZ3Solver.Number2Expr(2);
                //IntExpr lTempExpr = cZ3Solver.MulOperator(new List<IntExpr>() { lAddedOperationUsedVariables, lAddedOperationUsedVariables });

                _z3Solver.AddConstraintToOptimizer(_z3Solver.EqualOperator(lMaximizeVariable
                                                                        , lAddedOperationUsedVariables), "InitializingTheMaxVariable");

                //We will solve this as an optimization problom. Solver.Optimize
                //(maximize MaxNoOfOpsUsed)
                int lMaxValue = _z3Solver.MaximizeExpression(lMaximizeVariable);

                //Then we will return the maximum number which is returned with Solver.printmode[variable]
                //This returned value will be an accurate maximum number of operations which will be actvated in this case
                //And from that we can calculate the maximum number of transitions
                lResultNoOfCycles = lMaxValue;
            }
            catch (Exception ex)
            {
                _outputHandler.PrintMessageToConsole("error in CalculateAnalysisNoOfCycles");
                _outputHandler.PrintMessageToConsole(ex.Message);
            }
            return lResultNoOfCycles;
        }

        private void LoadDefaultVariationPointsToAnalyzer()
        {
            //TODO: Design decision
            //Here we have to build and call a function which loads the initial values for the initializer from the external xml file
        }

        /// <summary>
        /// This function loads the initial data (product platform data) from the external file
        /// Then according to the type of analysis which is stated in the argument it will call the respective analyzer function
        /// </summary>
        /// <param name="pAnalysisType">Type of analysis which we want</param>
        /// <param name="pExtraConfigurationRule">Any configuration rule whch needs to be added to the product platform configuration rule set</param>
        /// <param name="pInternalFileData">In case the initial data file is internal it has to be provided here</param>
        /// <returns>The result of the analysis</returns>
        public bool ProductPlatformAnalysis(string pInternalFileData = ""
                                            , string pExtraConfigurationRule = "")

        {
            bool lLoadInitialData = false;
            bool lTestResult = false;

            try
            {
                if (pInternalFileData != "")
                    lLoadInitialData = LoadInitialData(Enumerations.InitializerSource.InitialDataFile, pInternalFileData);
                else
                    lLoadInitialData = LoadInitialData(Enumerations.InitializerSource.RandomData, "");



                if (lLoadInitialData)
                {
                    switch (_analysisType)
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
                        case Enumerations.AnalysisType.NeverSelectedVariantAnalysis:
                            {
                                lTestResult = NeverSelectedVariantAnalysis(true, true);
                                break;
                            }
                        case Enumerations.AnalysisType.PartSelectabilityAnalysis:
                            {
                                lTestResult = PartSelectabilityAnalysis(true, true);
                                break;
                            }
                        case Enumerations.AnalysisType.AlwaysSelectedPartAnalysis:
                            {
                                lTestResult = AlwaysSelectedPartAnalysis(true, true);
                                break;
                            }
                        case Enumerations.AnalysisType.NeverSelectedPartAnalysis:
                            {
                                lTestResult = NeverSelectedPartAnalysis(true, true);
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
                        case Enumerations.AnalysisType.NeverSelectedOperationAnalysis:
                            {
                                lTestResult = NeverSelectedOperationAnalysis(true, true);
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

                    if (StopAEndOfAnalysis)
                        Console.ReadKey();

                }
                else
                {
                    _outputHandler.PrintMessageToConsole("Initial data incomplete! Analysis can't be done!");
                    if (StopAEndOfAnalysis)
                        Console.ReadKey();
                }

            }
            catch (Z3Exception ex)
            {
                _outputHandler.PrintMessageToConsole("Z3 Managed Exception: " + ex.Message);
                _outputHandler.PrintMessageToConsole("Stack trace: " + ex.StackTrace);
            }
            return lTestResult;
        }


        #region Previous Versions
        #endregion


    }
}
