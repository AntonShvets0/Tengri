using System.Diagnostics;

namespace TengriLang.Language.System.Library
{
    public class TENGRI_clock
    {
        private Stopwatch _stopWatch;
        public long TENGRI_elapsedMilliseconds => _stopWatch.ElapsedMilliseconds;
        public bool TENGRI_isRunning => _stopWatch.IsRunning;

        public TENGRI_clock(dynamic[] args)
        {
            _stopWatch = new Stopwatch();
        }

        public void TENGRI_start(dynamic[] args)
        {
            _stopWatch.Start();
        }

        public void TENGRI_stop(dynamic[] args)
        {
            _stopWatch.Stop();
        }

        public void TENGRI_restart(dynamic[] args)
        {
            _stopWatch.Restart();
        }

        public void TENGRI_reset(dynamic[] args)
        {
            _stopWatch.Restart();
        }
    }
}