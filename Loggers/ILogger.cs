using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loggers
{
    interface ILogger
    {
        void Info(string email,string ev, string source);
        void Error(string email, string ev, string source);
        void Warning(string email, string ev, string source);
    }
}
