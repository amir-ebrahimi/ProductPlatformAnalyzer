using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class Enumerations
    {
        public enum GeneralAnalysisType
        {
            Static,
            Dynamic
        }

        public enum AnalysisType
        {
            ModelEnumerationAnalysis,
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
