using System.Reflection;
using NLua;
using Starlight.Asus;
using Starlight.Asus.AnimeMatrix;

namespace Starlight.Engine
{
    internal class LuaFunctions
    {
        private readonly AnimeMatrixDevice _device;
        private readonly Lua _lua;

        public LuaFunctions(AnimeMatrixDevice device, Lua lua)
        {
            _device = device;

            _lua = lua;
            _lua.NewTable("anime");
        }

        public void RegisterAll()
        {
            RegisterFunction("anime.init", nameof(AnimeInitialize));
            RegisterFunction("anime.seti", nameof(AnimeSetI), typeof(int), typeof(byte));
            RegisterFunction("anime.setxy", nameof(AnimeSetXY), typeof(int), typeof(int), typeof(byte));
            RegisterFunction("anime.ledmode", nameof(AnimeSetBrightness), typeof(string));
            RegisterFunction("anime.present", nameof(AnimePresent));
            RegisterFunction("anime.ledcount", nameof(AnimeLedCount));
            RegisterFunction("anime.columns", nameof(AnimeColumns), typeof(int));
            RegisterFunction("anime.rows", nameof(AnimeRows));
            RegisterFunction("print", nameof(Print), typeof(object[]));
        }

        private LuaFunction RegisterFunction(string luaPath, string clrMethodName, params Type[] types)
        {
            var segments = luaPath.Split('.');

            var tblName = segments[0];
            var rest = string.Join('.', segments.Skip(1));

            if (_lua[tblName] is not LuaTable)
                _lua.NewTable(tblName);

            var tbl = _lua[tblName] as LuaTable;

            tbl![rest] = _lua.RegisterFunction(
                luaPath,
                this,
                GetType().GetMethod(clrMethodName, BindingFlags.NonPublic | BindingFlags.Instance, types)
            );

            return (tbl[rest] as LuaFunction)!;
        }

        private void Print(params object[] args)
        {
            foreach (var arg in args)
            {
                Console.Write(arg.ToString());
                Console.Write("    ");
            }

            Console.WriteLine();
        }

        private void AnimeInitialize()
        {
            _device.WakeUp();
            _device.SetDisplayState(true);
        }

        private void AnimeSetBrightness(string mode)
        {
            switch (mode)
            {
                case "full":
                    _device.SetBrightness(BrightnessLevel.Full);
                    break;

                case "medium":
                    _device.SetBrightness(BrightnessLevel.Medium);
                    break;

                case "dim":
                    _device.SetBrightness(BrightnessLevel.Dim);
                    break;

                case "off":
                    _device.SetBrightness(BrightnessLevel.Off);
                    break;
            }
        }

        private void AnimeSetXY(int x, int y, byte val)
            => _device.SetLedPlanar(x, y, val);

        private void AnimePresent()
            => _device.Present();

        private int AnimeColumns(int row)
            => _device.Columns(row);

        private int AnimeRows()
            => _device.Rows;

        private void AnimeSetI(int i, byte val)
            => _device.SetLedLinear(i, val);

        private int AnimeLedCount()
            => _device.LedCount;
    }
}