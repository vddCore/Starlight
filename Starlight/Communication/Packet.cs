using System.ComponentModel;
using HidSharp;

namespace Starlight.Communication
{
    public abstract class Packet
    {
        private int _currentDataIndex = 1;
        private byte[] _data;

        internal Packet(byte reportId, int packetLength, params byte[] data)
        {
            if (packetLength < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(packetLength),
                    "Packet length must be at least 1."
                );
            }
            
            _data = new byte[packetLength];
            _data[0] = reportId;

            if (data.Length > 0)
            {
                if (_currentDataIndex >= _data.Length)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(data),
                        "Your packet length does not allow for initial data to be appended."
                    );
                }
                
                AppendData(data);
            }
        }

        public Packet AppendData(params byte[] data)
            => AppendData(out _, data);

        public Packet AppendData(out int bytesWritten, params byte[] data)
        {
            bytesWritten = 0;

            for (var i = 0;
                 i < data.Length && _currentDataIndex < _data.Length - 1;
                 i++, bytesWritten++, _currentDataIndex++)
            {
                if (_currentDataIndex > _data.Length - 1)
                    break;

                _data[_currentDataIndex] = data[i];
            }

            return this;
        }

        public void Set(HidStream stream)
        {
            WrapException(() =>
            {
                stream.SetFeature(_data);
                stream.Flush();
            });
        }

        public byte[] Get(HidStream stream)
        {
            WrapException(() =>
            {
                stream.GetFeature(_data);
                stream.Flush();
            });

            return _data;
        }

        private void WrapException(Action action)
        {
            try
            {
                action();
            }
            catch (IOException e)
            {
                if (e.InnerException is Win32Exception w32e)
                {
                    if (w32e.NativeErrorCode != 0)
                    {
                        throw;
                    }
                }
            }
        }
    }
}