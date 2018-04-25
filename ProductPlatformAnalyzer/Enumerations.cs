using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class Enumerations
    {
        public enum LogicOperator
        {
            and,
            or,
            xor,
            not
        }

        public enum InputFileType
        {
            XML,
            AML
        }

        public enum OperationInstanceState
        {
            Initial,
            Executing,
            Finished,
            Unused
        }

        public enum OperationStatus
        {
            Active,
            Inactive
        }

        public enum GeneralAnalysisType
        {
            Static,
            Dynamic
        }

        public enum AnalysisType
        {
            ProductModelEnumerationAnalysis,
            ProductManufacturingModelEnumerationAnalysis,
            VariantSelectabilityAnalysis,
            AlwaysSelectedVariantAnalysis,
            OperationSelectabilityAnalysis,
            AlwaysSelectedOperationAnalysis,
            ExistanceOfDeadlockAnalysis,
            AlwaysDeadlockAnalysis,
            CompleteAnalysis
        }

        public enum InitializerSource
        {
            InitialDataFile,
            RandomData
        }

        public enum VariantGroupCardinality
        {
            Choose_Exactly_One,
            Choose_At_Least_One,
            Choose_Zero_Or_More,
            Choose_All
        }

    }
}
