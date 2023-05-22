using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Model.StatsModels
{
    public class RunningStatusStatsModel
    {
        public int Pending { get; set; }
        public int Running { get; set; }
        public int Finished { get; set; }
    }
}
