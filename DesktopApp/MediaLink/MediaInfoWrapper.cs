

using System;
using System.Collections.Generic;

namespace MediaLink
{
    internal class MediaInfoWrapper
    {
        public bool HasData { get; set; } = false;
        public MediaInfo Data { get; set; }

        public override bool Equals(object obj)
        {
            return obj is MediaInfoWrapper wrapper &&
                   EqualityComparer<MediaInfo>.Default.Equals(Data, wrapper.Data);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Data);
        }
    }
}
