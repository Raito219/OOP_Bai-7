using System;

namespace DeviceManagement
{
    public class DeviceNotReadyException : Exception
    {
        public string DeviceId { get; }
        public DeviceStatus CurrentStatus { get; }

        public DeviceNotReadyException(string deviceId, DeviceStatus currentStatus)
            : base("Thiết bị [" + deviceId + "] hiện không sẵn sàng để cấp phát (Trạng thái: " + currentStatus + ").")
        {
            DeviceId = deviceId;
            CurrentStatus = currentStatus;
        }

        public DeviceNotReadyException(string deviceId, DeviceStatus currentStatus, string message)
            : base(message)
        {
            DeviceId = deviceId;
            CurrentStatus = currentStatus;
        }
    }
}