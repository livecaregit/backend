using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace LC_Service
{
    public class ClassLog
    {
        public static readonly ILog _mLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}