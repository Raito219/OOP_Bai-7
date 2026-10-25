using System;
using System.Collections.Generic;

namespace DeviceManagement
{
    public class AirConditionerRemote : TeachingDevice
    {
        public IReadOnlySet<string> CompatibleRooms { get; private set; }

        public AirConditionerRemote(
            string id,
            string name,
            string storageLocation,
            IEnumerable<string> compatibleRooms,
            DeviceStatus status = DeviceStatus.Available)
            : base(id, name, storageLocation, status)
        {
            if (compatibleRooms == null)
            {
                throw new ArgumentNullException(nameof(compatibleRooms), "Tập danh sách phòng tương thích không được null.");
            }

           
            HashSet<string> rooms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string room in compatibleRooms)
            {
                if (!string.IsNullOrWhiteSpace(room))
                {
                    rooms.Add(room.Trim());
                }
            }
            CompatibleRooms = rooms;
        }

        public bool IsCompatibleWith(string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId))
            {
                return false;
            }
            return CompatibleRooms.Contains(roomId.Trim());
        }

       
        public override bool IsReadyForUse()
        {
            return Status == DeviceStatus.Available;
        }

        public override double GetDefaultPriorityScore()
        {
            return CompatibleRooms.Count > 0 ? 100.0 : 0.0;
        }

        public override string ToString()
        {
            string rooms = string.Join(", ", CompatibleRooms);
            return base.ToString() + " | Phòng tương thích: [" + rooms + "] | Điểm ưu tiên: " + GetDefaultPriorityScore() + " | Sẵn sàng: " + IsReadyForUse();
        }
    }
}