using System.Collections.Generic;

namespace DeviceManagement
{
    public class FirstAvailableStrategy<T> : ISelectionStrategy<T>
    {
        public T Select(IReadOnlyList<T> candidates)
        {
            if (candidates == null || candidates.Count == 0)
            {
                throw new NoSuitableItemException("Danh sách ứng viên rỗng, không thể chọn thiết bị phù hợp.");
            }

            return candidates[0];
        }
    }
}