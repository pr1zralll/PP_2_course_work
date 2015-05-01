using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PP_2_course_work
{
    class Program
    {
        public static int N = 4;
        public static int P = 4; 
        public static int H = N / P;

        public static int[][] MA, MB, MK, MC, MO, MR;
        public static int a, b;

        private static Object cs;//cricital section
        private static EventWaitHandle ev1, ev2;
        private static EventWaitHandle[] ev;
        static void Main(string[] args)
        {
            HiPerfTimer timer = new HiPerfTimer();
            timer.Start();
            //init
            if (P == 1){ P = 2; P = 1; H = N / P;}//2 threads for 1 processor
            cs = new Object();

            ev = new EventWaitHandle[P - 1];
            for (int i = 0; i < P - 1; i++)
                ev[i] = new EventWaitHandle(false, EventResetMode.ManualReset);

            ev1 = new EventWaitHandle(false, EventResetMode.ManualReset);
            ev2 = new EventWaitHandle(false, EventResetMode.ManualReset);

            //threads
            Thread[] t = new Thread[P];

            t[0] = new Thread(new ThreadStart(task1));
            if (P > 1) t[P - 1] = new Thread(new ThreadStart(taskP));

            for (int k = 1; k < P - 1; k++)
            {
                int id = k;
                t[k] = new Thread(() => task(id));
            }
            for (int i = 0; i < P; i++)
                t[i].Start();

            t[0].Join();
            timer.Stop();
            Console.WriteLine("main time :" + timer.Duration);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("res.txt", true))
            {
                file.WriteLine(N + " " + P + " - " + timer.Duration);
            }
            Console.Read();
        }

        static void task1()
        {
            
            Console.WriteLine("T1 started");
            HiPerfTimer t = new HiPerfTimer();    
            t.Start();  
            //input
            MA = Tools.generateMatrix(0, N);
            MB = Tools.generateMatrix(1, N);
            MK = Tools.generateMatrix(1, N);
            b = 1;
            //signal about input
            ev1.Set();
            //wait for others input
            ev2.WaitOne();
            //copy
            int[][] MO1, MR1;
            int a1, b1;
            lock (cs)//sync
            {
                MO1 = MO;
                MR1 = MR;
                a1 = a;
                b1 = b;
            }
            //calc
            for (int i = 0; i < H; i++)
                MA[i] = Tools.SumVectors(Tools.MultiNumVec(a1,
                    Tools.SumVectors(MB[i],
                    Tools.MultiVecToMatrix(MC[i],
                    MO1))), Tools.MultiNumVec(b1,
                    Tools.MultiVecToMatrix(MK[i], MR1)));
            //wait all about end calc
            for (int i = 0; i < P-1; i++)
                ev[i].WaitOne();
          
            //print
            if (N < 15) Tools.print(MA);
            else Console.WriteLine("N: "+N+" value = "+MA[0][0]);
            
            t.Stop();
            Console.WriteLine("T" + 1 + " end. " + t.Duration);

        }
        static void taskP()
        {
            Console.WriteLine("T"+P+" started");
            HiPerfTimer t = new HiPerfTimer();
            t.Start();  
            //input
            MC = Tools.generateMatrix(1, N);
            MO = Tools.generateMatrix(1, N);
            MR = Tools.generateMatrix(1, N);
            a = 1;
            //signal about input
            ev2.Set();
            //wait for others input
            ev1.WaitOne();
            //copy
            int[][] MO1, MR1;
            int a1, b1;
            lock (cs)//sync
            {
                MO1 = MO;
                MR1 = MR;
                a1 = a;
                b1 = b;
            }
            //calc
            for (int i = H*(P-1); i < N; i++)
                MA[i] = Tools.SumVectors(Tools.MultiNumVec(a1,
                    Tools.SumVectors(MB[i],
                    Tools.MultiVecToMatrix(MC[i],
                    MO1))), Tools.MultiNumVec(b1,
                    Tools.MultiVecToMatrix(MK[i], MR1)));
            //signal about end calc
            ev[0].Set();
            t.Stop();
            Console.WriteLine("T" + P + " end. " + t.Duration );
        }
        static void task(int c)
        {
            Console.WriteLine("T" + (c+1) + " started");
            HiPerfTimer t = new HiPerfTimer();
            t.Start();  
            //wait for others input
            ev1.WaitOne();
            ev2.WaitOne();
            //copy
            int[][] MO1, MR1;
            int a1, b1;
            lock (cs)//sync
            {
                MO1 = MO;
                MR1 = MR;
                a1 = a;
                b1 = b;
            }
            //calc
            for (int i = c*H; i < (c+1)*H; i++)
                MA[i] = Tools.SumVectors(Tools.MultiNumVec(a1,
                    Tools.SumVectors(MB[i],
                    Tools.MultiVecToMatrix(MC[i],
                    MO1))), Tools.MultiNumVec(b1,
                    Tools.MultiVecToMatrix(MK[i], MR1)));
            //signal about end calc
            ev[c].Set();
            t.Stop();
            Console.WriteLine("T" + (c + 1) + " end. " + t.Duration);
        }
    }
}
