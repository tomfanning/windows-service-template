using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace windows_service
{
    public partial class ServiceImpl : ServiceBase
    {
        Thread t;
        bool stopping;
        LogDelegate logger;

        public ServiceImpl(LogDelegate logger)
        {
            this.logger = logger;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            t = new Thread(new ThreadStart(DoWork)) { IsBackground = true };
            t.Start();
        }

        protected override void OnStop()
        {
        }

        /// <summary>
        /// Blocking method that loops, handles timeouts, handles exceptions.
        /// </summary>
        public void DoWork()
        {
            while (!stopping)
            {
                try
                {
                    DoSomething();
                }
                catch (Exception ex)
                {
                    // this is the general exception handler for the thread, without this the thread would just disappear.
                    // this implementation retries with a sleep to avoid a tight loop. Might need to make this fatal, depending on your needs.

                    Sleep(TimeSpan.FromSeconds(10));
                }
            }

            logger("Service stopped");
            //NLog.LogManager.Flush();
        }

        /// <summary>
        /// A cancellable sleep
        /// </summary>
        /// <param name="duration"></param>
        void Sleep(TimeSpan duration)
        {
            var sw = Stopwatch.StartNew();

            while (sw.Elapsed < duration && !stopping)
            {
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// This method can return regularly or it can block, but if it blocks then it needs to be watching the stopping flag.
        /// If it returns, it will be called again immediately, so it shouldn't return immediately to avoid a tight loop.
        /// </summary>
        /// <returns></returns>
        public void DoSomething()
        {
            Thread.Sleep(5000); // pretend to do something
        }
    }
}
