using NLua;
using Starlight.AnimeMatrix;

namespace Starlight.Engine
{
    public class AnimeMatrixLuaContext : IDisposable
    {
        protected readonly Lua _lua;

        public AnimeMatrixLuaContext(AnimeMatrixDevice device)
        {
            _lua = new Lua(false);
            
            new LuaFunctions(device, _lua)
                .RegisterAll();
            
            _lua.DoString(
                @"anime.init()
                  anime.ledmode('full')
                  anime.setxy(10, 10, 255)
                  anime.setxy(10, 9, 255)
                  anime.setxy(10, 8, 255)
                  anime.setxy(10, 7, 255)
                  anime.present()
                "
            );
        }

        public void RunScript(string filePath)
        {
            try
            {
                _lua.DoFile(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Dispose()
        {
            _lua.Dispose();
        }
    }
}