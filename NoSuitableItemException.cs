using System;

namespace DeviceManagement
{
    public class NoSuitableItemException : Exception
    {
        public NoSuitableItemException()
            : base("Không tìm thấy thiết bị nào phù hợp")
        {
        }

        public NoSuitableItemException(string message)
            : base(message)
        {
        }
    }
}
