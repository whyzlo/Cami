using System;
using System.Collections.Generic;

namespace Cami.Core
{
    public class TrackableObjectDescription
    {
        public TrackableObjectType Type { get; set; }
        public Dictionary<string, Type> Properties { get; set; } = new Dictionary<string, Type>();
    }
}