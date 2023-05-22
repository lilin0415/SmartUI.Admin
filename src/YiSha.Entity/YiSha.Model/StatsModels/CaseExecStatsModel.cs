using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Model.StatsModels
{
    public class CaseExecStatsModel: BaseStatsModel
    {
        public RunningStatusStatsModel RunningStatus { get; set; }
        public ExecuteResultStatsModel ExecuteResult { get; set; }
    }
}
