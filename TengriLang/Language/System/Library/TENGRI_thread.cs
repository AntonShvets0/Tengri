using System.Threading;

namespace TengriLang.Language.System.Library
{
    public class TENGRI_thread
    {
        private Thread _thread;
        public bool TENGRI_isAlive => _thread.IsAlive;

        public TENGRI_thread(dynamic[] args)
        {
            if (args.Length < 1 || !(args[0] is TengriData.TengriMethod)) return;
            
            _thread = new Thread(() =>
            {
                args[0](new dynamic[] {});
            });
        }

        public void TENGRI_start(dynamic[] args)
        {
            _thread.Start();
        }

        public void TENGRI_stop(dynamic[] args)
        {
            _thread.Abort();
        }

        public static void TENGRI_wait(dynamic[] args)
        {
            Thread.Sleep(args[0]);
        }
    }
}