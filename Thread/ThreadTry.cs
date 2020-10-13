using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using SkiaSharp;

// Simple threading scenario:  Start a static method running
// on a second thread.
namespace ImageProcessing
{
    public class ThreadExample
    {
        // The ThreadProc method is called when the thread starts.
        // It loops ten times, writing to the console and yielding
        // the rest of its time slice each time, and then ends.
        public static void ThreadContraste(ImageProcessing image_process)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            image_process.AlargamentoContraste();
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine($"Elapsed time of running ContrasteThread. {ts}");
        }
        public static void ThreadEqualizador(ImageProcessing image_process)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            image_process.EqualizacaoHistograma();
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine($"Elapsed time of running EqualizadorThread. {ts}");
        }

        public static void Main()
        {
            
            string filepath = @"C:\Users\Petch\source\repos\Thread\Image\Thread.png";
            ImageProcessing image_process = new ImageProcessing(filepath);

            Thread EqualizadorThread = new Thread(() => ThreadEqualizador(image_process));
            Thread ContrasteThread = new Thread(() => ThreadContraste(image_process));

            EqualizadorThread.Start();
            ContrasteThread.Start();
            EqualizadorThread.Join();
            ContrasteThread.Join();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            image_process = new ImageProcessing(filepath);
            image_process.AlargamentoContraste();
            image_process.EqualizacaoHistograma();
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine($"Elapsed time of running both functions on the same thread. {ts}");



        }
    }
}
