using System;

namespace Starlight.Engine
{
    public class PerformanceCounter
    {
        private DateTime _nowTime = DateTime.Now;
        private DateTime _prevTime = DateTime.Now;
        
        public double Delta { get; private set; }
        public double Time { get; private set; }

        public void Tick()
        {
            _nowTime = DateTime.Now;
            Delta = (_nowTime.Ticks - _prevTime.Ticks) / 10000000.0;
            Time += Delta;
            _prevTime = _nowTime;
        }
    }
}