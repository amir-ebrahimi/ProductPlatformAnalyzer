using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    public class DependentActions
    {
        public Action Action1 { get; set; }
        public Action Action2 { get; set; }

        public DependentActions(Action pAction1, Action pAction2)
        {
            Action1 = pAction1;
            Action2 = pAction2;
        }
    }
}
