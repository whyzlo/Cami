using System;
using System.Collections.Generic;
using Cami.Core.Interfaces;

namespace Cami.Core
{
    public class TrackableObject
    {
        public TrackableObjectType Type { get; set; }
        public Guid Id { get; set; }

        public List<ITrackableObjectAttribute> Attributes { get; set; } = new List<ITrackableObjectAttribute>();
    }
}