using System;

namespace DeviceManagement
{
    public class AllocationService<T> where T : TeachingDevice
    {
        private readonly IRepository<T> repository;
        private ISelectionStrategy<T> strategy;

        public AllocationService(IRepository<T> repository, ISelectionStrategy<T> strategy)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository), "Kho lưu trữ không được null.");
            }
            if (strategy == null)
            {
                throw new ArgumentNullException(nameof(strategy), "Chiến lược lựa chọn không được null.");
            }

            this.repository = repository;
            this.strategy = strategy;
        }

        public void ChangeStrategy(ISelectionStrategy<T> newStrategy)
        {
            if (newStrategy == null)
            {
                throw new ArgumentNullException(nameof(newStrategy), "Chiến lược mới không được null.");
            }
            this.strategy = newStrategy;
        }

        public T Allocate(Predicate<T>? additionalCondition = null)
        {
            var candidates = repository.FindAll(delegate(T item)
            {
                if (!item.IsReadyForUse())
                {
                    return false;
                }
                if (additionalCondition != null && !additionalCondition(item))
                {
                    return false;
                }
                return true;
            });

            T selected = strategy.Select(candidates);

            selected.Borrow();

            return selected;
        }
    }
}
