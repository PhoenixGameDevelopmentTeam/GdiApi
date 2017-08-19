using System;

namespace GdiApi
{
    public class FrameRateManager
    {
        public int frameCntThisSpan = 0;
        public int FrameRate = 0;
        public int UpdatesPerSecond = 3;
        public TimeSpan span = new TimeSpan();

        public void Frame(TimeSpan delta)
        {
            span += delta;
            frameCntThisSpan++;
            if (span.TotalMilliseconds >= 1000 / UpdatesPerSecond)
            {
                FrameRate = frameCntThisSpan * UpdatesPerSecond;
                frameCntThisSpan = 0;
                span = TimeSpan.Zero;
            }
        }

        public override string ToString() => FrameRate.ToString();
    }
}
