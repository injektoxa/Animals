using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Animals.Extansions
{
    public static class Extension
    {
        public static Guid ToGuid(this Guid? source)
        {
            return source ?? Guid.Empty;
        }
    }
}