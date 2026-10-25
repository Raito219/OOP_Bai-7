using System;

namespace DeviceManagement
{
    public class WirelessMicrophone : TeachingDevice
    {
        public int BatteryLevel { get; private set; }
        public int SignalQuality { get; private set; }

        public WirelessMicrophone(string id, string name, string storageLocation, int batteryLevel, int signalQuality, DeviceStatus status = DeviceStatus.Available)
            : base(id, name, storageLocation, status)
        {
            if (batteryLevel < 0 || batteryLevel > 100)
            {
                throw new ArgumentException("Mức pin micro từ 0 đến 100.");
            }
            if (signalQuality < 0 || signalQuality > 100)
            {
                throw new ArgumentException("Chất lượng tín hiệu micro từ 0 đến 100.");
            }

            BatteryLevel = batteryLevel;
            SignalQuality = signalQuality;
        }

        public override bool IsReadyForUse()
        {
            return Status == DeviceStatus.Available && BatteryLevel >= 30 && SignalQuality >= 40;
        }

        public override double GetDefaultPriorityScore()
        {
            return (BatteryLevel + SignalQuality) / 2.0;
        }

        public override string ToString()
        {
            return base.ToString() + " | Pin: " + BatteryLevel + "% | Tín hiệu: " + SignalQuality + "% | Điểm ưu tiên: " + GetDefaultPriorityScore().ToString("F1") + " | Sẵn sàng: " + IsReadyForUse();
        }
    }
}
