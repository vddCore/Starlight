using System.Reflection;
using NLua;
using Starlight.AnimeMatrix;

namespace Starlight.Engine
{
    public class AnimeMatrixRenderer : IDisposable
    {
        private LuaFunction? _pixelCallback;
        private LuaFunction? _initCallback;

        private DateTime? _lastFrameTime;
        
        protected readonly AnimeMatrixDevice _device;

        protected Lua Lua { get; private set; }

        public double Time { get; private set; }
        public ulong TotalFrames { get; private set; }

        public bool Running { get; private set; }

        public AnimeMatrixRenderer(AnimeMatrixDevice device)
        {
            _device = device;
            Reset();            
        }

        public void LoadScript(string fileName)
        {
            Reset();

            Lua.DoFile(fileName);
            _initCallback = Lua["init"] as LuaFunction;
            _pixelCallback = Lua["pixel"] as LuaFunction;

            if (_pixelCallback == null)
            {
                throw new InvalidOperationException("`function pixel(x,y)' is missing from your script.");
            }
            
            _initCallback?.Call();
        }

        public async Task Run(double framerate)
        {
            if (Running)
                return;
            
            Running = true;
            
            while (Running)
            {
                Frame();
                await Task.Delay((int)(1 / framerate * 1000));
            }
        }

        public void Stop()
        {
            Running = false;
        }

        private void Frame()
        {
            if (_lastFrameTime != null)
            {
                var time = DateTime.Now - _lastFrameTime;
                Time += time.Value.TotalMilliseconds;
                Lua["dt"] = time.Value.TotalMilliseconds / 1000f;
            }
            
            Lua["frames"] = TotalFrames++;
            Lua["t"] = Time;

            for (var y = 0; y < _device.Rows; y++)
            {
                var cols = _device.Columns(y);
                
                for (var x = 0; x < cols; x++)
                {
                    try
                    {
                        var value = Convert.ToDouble(_pixelCallback!.Call(x, y)[0]);
                        _device.SetLedPlanar(x, y, (byte)(255 * value));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            
            _device.Present();
            _lastFrameTime = DateTime.Now;
        }

        public void Reset()
        {
            Stop();
            
            Lua?.Dispose();
            _device.Clear(true);
            
            TotalFrames = 0;
            Time = 0;
            
            Lua = new(true);
            Lua["frames"] = 0;
            Lua["t"] = 0;
            Lua["dt"] = 0;
            Lua["rows"] = _device.Rows;
            Lua["columns"] = Lua.RegisterFunction(
                "columns", 
                this,
                GetType().GetMethod(
                    nameof(AnimeColumns),
                    BindingFlags.NonPublic 
                    | BindingFlags.Instance
                )
            );
        }
        
        private int AnimeColumns(int row)
            => _device.Columns(row);

        public void Dispose()
        {
            Lua.Dispose();
        }
    }
}