using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Store.Models
{
    public enum ProgressStatus
    {
        None = 0,
        Started = 1,
        Continued = 2,
        Ended = 3
    }
    public class Progress
    {
        public int Id { get; set; }
        public DateTime StaretdTime { get; set; }
        public DateTime StopedTime { get; set; }
        public DateTime EndTime { get; set; }
        public ProgressStatus ProgressStatus { get; set; }
    }
}