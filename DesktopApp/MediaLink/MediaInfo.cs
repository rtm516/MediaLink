using System;

namespace MediaLink
{
    internal class MediaInfo
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string PlaybackStatus { get; set; }
        public string Thumbnail { get; set; }

        public override bool Equals(object obj)
        {
            return obj is MediaInfo info &&
                   Title == info.Title &&
                   Artist == info.Artist &&
                   PlaybackStatus == info.PlaybackStatus &&
                   Thumbnail == info.Thumbnail;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Artist, PlaybackStatus, Thumbnail);
        }
    }
}
