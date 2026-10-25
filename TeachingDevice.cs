using System;

namespace DeviceManagement
{
    public abstract class TeachingDevice : IIdentifiable
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string StorageLocation { get; private set; }
        public DeviceStatus Status { get; protected set; }

        public TeachingDevice(string id, string name, string storageLocation, DeviceStatus status = DeviceStatus.Available)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Mã thiết bị không được để trống.");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Tên thiết bị không được để trống.");
            }
            if (string.IsNullOrEmpty(storageLocation))
            {
                throw new ArgumentException("Vị trí lưu trữ không được để trống.");
            }

            Id = id;
            Name = name;
            StorageLocation = storageLocation;
            Status = status;
        }

        public abstract bool IsReadyForUse();

        public abstract double GetDefaultPriorityScore();

        public void Borrow()
        {
            if (!IsReadyForUse())
            {
                throw new DeviceNotReadyException(Id, Status, "Thiết bị " + Id +  " không sẵn sàng để cấp phát (" + Status + ").");
            }
            Status = DeviceStatus.Borrowed;
        }

        public void Return()
        {
            if (Status != DeviceStatus.Borrowed)
            {
                throw new InvalidOperationException("Không thể trả thiết bị " + Id + " vì không ở trạng thái Borrowed (" + Status + ").");
            }
            Status = DeviceStatus.Available;
        }

        public override string ToString()
        {
            return "[" + Id + "] " + Name + " (Vị trí: " + StorageLocation + ", Trạng thái: " + Status + ")";
        }
    }
}
