namespace Starlight.AnimeMatrix
{
    public class BuiltInAnimation
    {
        public enum StartupAnimation
        {
            GlitchConstruction,
            StaticEmergence
        }

        public enum ShutdownAnimation
        {
            GlitchOut,
            SeeYa
        }

        public enum SleepAnimation
        {
            BannerSwipe,
            Starfield
        }

        public enum RunningAnimation
        {
            BinaryBannerScroll,
            RogLogoGlitch
        }

        private AnimeMatrix _animeMatrix;

        internal BuiltInAnimation(AnimeMatrix animeMatrix)
        {
            _animeMatrix = animeMatrix;
        }
        
        // public void ToggleBuiltInAnimation(bool enable)
        // {
        //     var enabled = enable ? (byte)0x80 : (byte)0x00;
        //     Set(Packet<AnimeMatrixPacket>(0xC4, 0x01, enabled));
        // }
        //
        // public void ConfigureBuiltInAnimation(BuiltInAnimation animation)
        // {
        //     Set(Packet<AnimeMatrixPacket>(0xC5, (byte)animation));
        // }
    }
}