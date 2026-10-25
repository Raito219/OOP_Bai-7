using System;
using System.Collections.Generic;

namespace DeviceManagement
{

    public class InMemoryRepository<T> : IRepository<T> where T : IIdentifiable
    {
        private readonly Dictionary<string, T> _items = new Dictionary<string, T>();

        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Phần tử thêm vào kho không được null.");
            }

            if (string.IsNullOrWhiteSpace(item.Id))
            {
                throw new ArgumentException("Mã định danh không hợp lệ.", nameof(item));
            }

            if (_items.ContainsKey(item.Id))
            {
                throw new DuplicateIdException(item.Id);
            }

            _items.Add(item.Id, item);
        }

        public bool Remove(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            return _items.Remove(id);
        }

        public T? FindById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return default;
            }

            if (_items.TryGetValue(id, out T? item))
            {
                return item;
            }
            return default;
        }

        public IReadOnlyList<T> GetAll()
        {
            List<T> list = new List<T>(_items.Values);
            return list.AsReadOnly();
        }

        public IReadOnlyList<T> FindAll(Predicate<T> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "Điều kiện lọc không được null.");
            }

            List<T> result = new List<T>();
            foreach (T item in _items.Values)
            {
                if (condition(item))
                {
                    result.Add(item);
                }
            }
            return result.AsReadOnly();
        }
    }
}