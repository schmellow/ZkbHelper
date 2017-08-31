using System;
using System.Collections.Generic;

namespace ZkbHelper.Logging
{
    public class Logger
    {
        public static Logger Instance = new Logger();

        private Dictionary<string, ILoggingTarget> _targets = new Dictionary<string, ILoggingTarget>();

        public void Write(string message)
        {
            foreach (var target in _targets.Values)
                target.Write(message);
        }

        public void SetTarget(string name, ILoggingTarget target)
        {
            _targets[name] = target;
        }

        public void ClearTarget(string name)
        {
            if (_targets.ContainsKey(name))
                _targets.Remove(name);
        }
    }
}
