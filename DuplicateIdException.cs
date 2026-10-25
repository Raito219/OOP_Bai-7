using System;

namespace DeviceManagement
{
    public class DuplicateIdException : Exception
    {
        public string DuplicateId { get; }

        public DuplicateIdException(string duplicateId)
            : base("Trùng mã định danh thiết bị: [" + duplicateId + "].")
        {
            DuplicateId = duplicateId;
        }

        public DuplicateIdException(string duplicateId, string message)
            : base(message)
        {
            DuplicateId = duplicateId;
        }
    }
}