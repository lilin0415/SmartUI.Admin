using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Model.StatsModels
{
    public class BaseStatsModel
    {
        public int Total { get; set; }
        public int IncreasedToday { get; set; }
        public int IncreasedYesterday { get; set; }
        public int IncreasedThisWeek { get; set; }

    }
}
