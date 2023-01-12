using System;
using System.Linq;
using EVIL.ExecutionEngine;
using EVIL.ExecutionEngine.Abstraction;
using EVIL.ExecutionEngine.Diagnostics;
using EVIL.Intermediate.CodeGeneration;

namespace Starlight.Engine
{
    public class Callback
    {
        private readonly string _functionName;
        private readonly int _parameterCount;

        private readonly EVM _evm;
        private readonly Executable _executable;

        private ExecutionContext _executionContext;
        private Chunk _chunk;

        public Callback(string functionName, int parameterCount, EVM evm, Executable executable)
        {
            _functionName = functionName;
            _parameterCount = parameterCount;

            _evm = evm;
            _executable = executable;
            
            SetUp();
        }

        public DynamicValue Invoke(params DynamicValue[] args)
        {
            _evm.InvokeCallback(
                _chunk,
                _executionContext,
                args
            );
            _evm.Start();

            return _executionContext.EvaluationStackTop;
        }

        public void Delete()
        {
            _executionContext.Pause();
            _evm.DeleteExecutionContext(_executionContext);
        }

        private void SetUp()
        {
            _evm.GlobalTable.Unset(new DynamicValue(_functionName));
            _executionContext = _evm.CreateNewExecutionContext();
                
            _chunk = _executable.Chunks.FirstOrDefault(x => x.Name == _functionName);

            if (_chunk == null || _chunk.Parameters.Count != _parameterCount)
            {
                throw new InvalidOperationException(
                    $"Script is missing a required function '{_functionName}' taking {_parameterCount} parameter(s)."
                );
            }
        }
    }
}