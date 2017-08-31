using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkbHelper.Logging
{
    public interface ILoggingTarget
    {
        void Write(string message);
    }
}
