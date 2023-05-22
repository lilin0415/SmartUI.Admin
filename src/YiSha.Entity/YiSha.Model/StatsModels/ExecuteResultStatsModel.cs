using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Model.StatsModels
{
    /// <summary>
    /// finish->finished
    /// </summary>
    public class ExecuteResultStatsModel
    {
        /// <summary>
        /// success
        /// </summary>
        public int Succeeded { get; set; }
        /// <summary>
        /// failure
        /// </summary>
        public int Failed { get; set; }
        /// <summary>
        /// cancel
        /// </summary>
        public int Cancelled { get; set; }

        /// <summary>
        /// skip
        /// </summary>
        public int Skipped { get; set; }

        public double SucceededPercentage {
            get {
                var total = this.Succeeded + this.Failed + this.Cancelled + this.Skipped;
                if (total == 0)
                {
                    return 0;
                }
                return this.Succeeded*1.0 / total;
            }
        }
    }
}
