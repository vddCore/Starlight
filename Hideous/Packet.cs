namespace Hideous
{
    public abstract class Packet
    {
        public byte[] Data { get; protected set; } = new byte[0];
    }

    public abstract class Packet<T> : Packet
        where T : Packet
    {
        private int _currentDataIndex;

        internal Packet(int initialDataIndex, int packetLength, params byte[] data)
        {
            if (packetLength < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(packetLength),
                    "Packet length must be at least 1."
                );
            }

            _currentDataIndex = initialDataIndex;

            Data = new byte[packetLength];

            if (data.Length > 0)
            {
                if (_currentDataIndex >= Data.Length)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(data),
                        "Your packet length does not allow for initial data to be appended."
                    );
                }

                AppendData(data);
            }
        }

        public T AppendData(params byte[] data)
            => AppendData(out _, data);

        public T AppendData(out int bytesWritten, params byte[] data)
        {
            bytesWritten = 0;

            for (var i = 0;
                 i < data.Length && _currentDataIndex < Data.Length - 1;
                 i++, bytesWritten++, _currentDataIndex++)
            {
                if (_currentDataIndex > Data.Length - 1)
                    break;

                Data[_currentDataIndex] = data[i];
            }

            return (this as T)!;
        }
    }
}