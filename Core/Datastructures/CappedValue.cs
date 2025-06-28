using System.Numerics;

namespace DnDSharp.Core
{
    public class CappedValue<T> where T : struct, ISignedNumber<T>, IComparisonOperators<T, T, bool>
    {

        public CappedValue(T startValue, T capacity)
        {
            Capacity = new(capacity);
            if (startValue > capacity) startValue = capacity;
            if (startValue < T.AdditiveIdentity) startValue = T.AdditiveIdentity;
            m_Current = startValue;
        }

        public readonly ModifyableValue<T> Capacity;


        private T m_Current;
        public T Current
        {
            get => m_Current;
            set
            {
                if (m_Current == value) return;
                value = value > T.AdditiveIdentity ? value : T.AdditiveIdentity;
                value = value < Capacity.Current ? value : Capacity.Current;
                m_Current = value;
                this.m_OnChanged?.Invoke(m_Current);
            }
        }


        private event Action<T>? m_OnChanged;
        public event Action<T> OnChanged
        {
            add
            {
                m_OnChanged += value;
                value?.Invoke(m_Current);
            }
            remove => m_OnChanged -= value;
        }

        public bool Has(T amount) => m_Current >= amount;
        public bool Remove(T amount) => Remove(amount, out var _);
        public bool Remove(T amount, out T underflow)
        {
            var available = m_Current;
            if (available > amount) available = amount;
            underflow = amount - available;
            m_Current -= available;
            return amount == available;
        }
        public bool Add(T amount) => Add(amount, out var _);
        public bool Add(T amount, out T overflow)
        {
            var fits = Capacity.Current - m_Current;
            if (amount < fits) fits = amount;
            overflow = amount - fits;
            m_Current += fits;
            return amount == fits;
        }
        public void Refill() => m_Current = Capacity.Current;
    }
}