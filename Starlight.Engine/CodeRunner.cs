using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EVIL.ExecutionEngine;
using EVIL.Grammar;
using EVIL.Grammar.Parsing;
using EVIL.Intermediate.CodeGeneration;
using EVIL.Lexical;

namespace Starlight.Engine
{
    public class CodeRunner
    {
        private readonly EVM _evm;
        private readonly Executable _executable;
        private readonly CancellationTokenSource _cts;

        private Callback _pixelCallback;
        private Callback _setupCallback;

        private Action<Callback> _render;

        public bool Running { get; private set; }

        public CodeRunner(string filePath, EVM evm, Action<Callback> render)
        {
            _evm = evm;

            if (CompileScript(filePath, out _executable))
            {
                RegisterCallbacks();
            }
            else
            {
                throw new IOException($"Failed to compile '{filePath}'.");
            }

            _cts = new CancellationTokenSource();
            _render = render;
        }

        public async Task Run()
        {
            _evm.Stop();

            Running = true;

            try
            {
                _setupCallback?.Invoke();
                _setupCallback?.Delete();
            }
            catch (VirtualMachineException vme)
            {
                Vomit(vme);
            }

            try
            {
                while (true)
                {
                    _cts.Token.ThrowIfCancellationRequested();

                    try
                    {
                        _render?.Invoke(_pixelCallback);
                    }
                    catch (VirtualMachineException vme)
                    {
                        Vomit(vme);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _pixelCallback.Delete();
                _cts.Dispose();

                Running = false;
            }
        }

        public async Task Stop()
        {
            _cts.Cancel();

            while (Running)
                await Task.Delay(1);
        }

        public void RegisterCallbacks()
        {
            if (_executable == null)
            {
                throw new InvalidOperationException("Attempted to look for chunks in a null executable.");
            }

            try
            {
                _setupCallback = new Callback("setup", 0, _evm, _executable);
            }
            catch
            {
                // Ignore. Setup is optional.
            }

            try
            {
                _pixelCallback = new Callback("pixel", 2, _evm, _executable);
            }
            catch
            {
                throw new InvalidOperationException(
                    "Script requires 'fn pixel(x, y)' returning a number between 0.0 and 1.0.");
            }
        }

        private bool CompileScript(string filePath, out Executable executable)
        {
            executable = null;

            try
            {
                using (var sr = new StreamReader(filePath))
                {
                    var lexer = new Lexer();
                    lexer.LoadSource(sr.ReadToEnd());

                    var parser = new Parser(lexer, false);
                    var program = parser.Parse();
                    var compiler = new Compiler();
                    executable = compiler.Compile(program);
                }

                return true;
            }
            catch (LexerException le)
            {
                Console.WriteLine($"[{le.Line}:{le.Column}] [ PARSER ] - Syntax error: {le.Message}");
                return false;
            }
            catch (ParserException pe)
            {
                Console.WriteLine($"[{pe.Line}:{pe.Column}] [ PARSER ] - Syntax error: {pe.Message}");
                return false;
            }
            catch (CompilerException ce)
            {
                Console.WriteLine($"[{ce.Line}:{ce.Column}] [COMPILER] - Syntax error: {ce.Message}");
                return false;
            }
            catch (FileNotFoundException fnfe)
            {
                Console.WriteLine($"{fnfe.FileName} not found.");
                return false;
            }
        }

        private void Vomit(VirtualMachineException vme)
        {
            Console.WriteLine($"Runtime error: {vme.Message}\n{vme.StackTrace}");
            Console.WriteLine(vme.ExecutionContext.DumpCallStack());
            Console.WriteLine("---");

            if (vme.InnerException != null)
            {
                Console.WriteLine(vme.InnerException);

                if (vme.InnerException is VirtualMachineException vme2)
                {
                    Console.WriteLine(vme2.ExecutionContext.DumpCallStack());
                }
            }
        }
    }
}