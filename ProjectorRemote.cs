using System;

namespace DeviceManagement
{
    public class ProjectorRemote : TeachingDevice
    {
        public int BatteryLevel { get; private set; }

        public ProjectorRemote(string id, string name, string storageLocation, int batteryLevel, DeviceStatus status = DeviceStatus.Available)
            : base(id, name, storageLocation, status)
        {
            if (batteryLevel < 0 || batteryLevel > 100)
            {
                throw new ArgumentOutOfRangeException("batteryLevel", "Mức pin của điều khiển máy chiếu phải từ 0% đến 100%.");
            }
            BatteryLevel = batteryLevel;
        }

        public override bool IsReadyForUse()
        {
            return Status == DeviceStatus.Available && BatteryLevel >= 20;
        }

        public override double GetDefaultPriorityScore()
        {
            return BatteryLevel;
        }

        public override string ToString()
        {
            return base.ToString() + " | Pin: " + BatteryLevel + "% | Điểm ưu tiên: " + GetDefaultPriorityScore() + " | Sẵn sàng: " + IsReadyForUse();
        }
    }
}
