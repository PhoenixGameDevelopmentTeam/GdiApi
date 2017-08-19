using System;

namespace GdiApi.Tests
{
    public class FrameRateManager
    {
        public int frameCntThisSpan = 0;
        public int FrameRate = 0;
        public int MarksPerSecond = 3;
        public TimeSpan span = new TimeSpan();

        public void Frame(TimeSpan delta)
        {
            span += delta;
            frameCntThisSpan++;
            if (span.TotalMilliseconds >= 1000 / MarksPerSecond)
            {
                FrameRate = frameCntThisSpan * MarksPerSecond;
                frameCntThisSpan = 0;
                span = TimeSpan.Zero;
            }
        }

        public override string ToString() => FrameRate.ToString();
    }
}
