using System;
using EVIL.ExecutionEngine;
using EVIL.ExecutionEngine.Abstraction;

namespace Starlight.Engine
{
    public class Renderer
    {
        private AnimeMatrix.AnimeMatrix _matrix;
        private Table _global;
        private PerformanceCounter _performanceCounter;

        public Renderer(AnimeMatrix.AnimeMatrix matrix, Table global)
        {
            _matrix = matrix;
            _global = global;

            Reset();
        }

        public void Reset()
        {
            _performanceCounter = new();
            _matrix.Clear();
        }

        public void Draw(Callback pixelCallback)
        {
            for (var y = 0; y < _matrix.Rows; y++)
            {
                var columns = _matrix.Columns(y);

                for (var x = 0; x < columns; x++)
                {
                    try
                    {
                        var value = pixelCallback.Invoke(
                            new[]
                            {
                                new DynamicValue(x / (double)columns),
                                new DynamicValue(y / (double)_matrix.Rows)
                            }
                        ).Number;


                        _matrix.SetLedPlanar(x, y, (byte)(255 * Math.Clamp(value, 0, 1)));
                        _performanceCounter.Tick();
                        _global["_time"] = new DynamicValue(_performanceCounter.Time);
                        _global["_delta"] = new DynamicValue(_performanceCounter.Delta);
                    }
                    catch (InvalidTypeUsageException)
                    {
                        _matrix.SetLedPlanar(x, y, 0);
                    }
                }
            }

            _matrix.Present();
        }
    }
}