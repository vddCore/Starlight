using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EVIL.ExecutionEngine;
using EVIL.ExecutionEngine.Abstraction;
using EVIL.RT;

namespace Starlight.Engine
{
    internal static class Program
    {
        private static string _filePath = "anime.vil";
        private static Table _global;
        private static EVM _evm;
        private static EvilRuntime _runtime;

        private static FileSystemWatcher _fsw;
        private static AnimeMatrix.AnimeMatrix _matrix;

        private static Renderer _renderer;
        private static CodeRunner _runner;

        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                _filePath = args[0];
            }
            

            _global = new Table();
            _evm = new EVM(_global);
            _runtime = new EvilRuntime(_global);

            InitializeMatrix();
            InitializeExecutionEngine();
            InitializeWatcher();

            _runner.Run();

            while (true)
                Thread.Sleep(1);
        }

        private static void InitializeExecutionEngine()
        {
            try
            {
                _runner = new CodeRunner(_filePath, _evm, _renderer.Draw);
            }
            catch
            {
                Environment.Exit(-1);
            }

            _runtime.LoadCoreRuntime();
        }

        private static void InitializeMatrix()
        {
            try
            {
                _matrix = new AnimeMatrix.AnimeMatrix();
                _renderer = new Renderer(_matrix, _global);
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.Message);
                Environment.Exit(-1);
            }
        }

        private static void InitializeWatcher()
        {
            _fsw = new FileSystemWatcher(
                Path.GetDirectoryName(Path.GetFullPath(_filePath))!,
                Path.GetFileName(_filePath)!
            );
            
            _fsw.NotifyFilter = NotifyFilters.LastWrite;
            _fsw.Changed += ResetSimulation;
            
            _fsw.EnableRaisingEvents = true;
        }

        private static async void ResetSimulation(object sender, FileSystemEventArgs e)
        {
            _fsw.EnableRaisingEvents = false;

            await _runner.Stop();
            _renderer.Reset();

            try
            {
                _runner = new CodeRunner(_filePath, _evm, _renderer.Draw);
                Task.Run(_runner.Run);

                Console.WriteLine($"Reloaded display script '{_filePath}'.");
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.Message);
            }

            _fsw.EnableRaisingEvents = true;
        }
    }
}