using System.Numerics;
namespace DnDSharp.Core
{
    [Serializable]
    public class ModifyableValue<T> where T : ISignedNumber<T>, IComparisonOperators<T, T, bool>
    {
        public T Current { get; private set; }

        public T m_Base;
        public T Base
        {
            get => m_Base; set
            {
                if (m_Base == value) return;
                m_Base = value;
                UpdateCurrent();
            }
        }

        private readonly Dictionary<Guid, T> additiveModifiers = [];
        private readonly Dictionary<Guid, T> multiplicativeModifiers = [];
        private readonly Dictionary<Guid, T> dividingModifiers = [];
        private readonly Dictionary<Guid, T> minModifiers = [];
        private readonly Dictionary<Guid, T> maxModifiers = [];

        private event Action<T>? m_OnChanged;
        public event Action<T> OnChanged
        {
            add
            {
                m_OnChanged += value;
                value?.Invoke(Current);
            }
            remove => m_OnChanged -= value;
        }

        public ModifyableValue(T baseValue)
        {
            m_Base = baseValue;
            Current = baseValue;
        }

        private T CalculateFormula()
        {
            var result = Base;

            foreach (var add in additiveModifiers.Values)
                result += add;

            foreach (var factor in multiplicativeModifiers.Values)
                result *= factor;

            foreach (var factor in dividingModifiers.Values)
                result /= factor;

            foreach (var min in minModifiers.Values)
                result = result > min ? result : min;

            foreach (var max in maxModifiers.Values)
                result = result < max ? result : max;

            return result;
        }

        private void UpdateCurrent()
        {
            var next = CalculateFormula();
            if (next == Current) return;
            Current = next;
            m_OnChanged?.Invoke(Current);
        }

        public Guid RegisterAdd(T value)
        {
            var guid = Guid.NewGuid();
            additiveModifiers[guid] = value;
            UpdateCurrent();
            return guid;
        }

        public Guid RegisterMultiply(T value)
        {
            var guid = Guid.NewGuid();
            multiplicativeModifiers[guid] = value;
            UpdateCurrent();
            return guid;
        }

        public Guid RegisterDivide(T value)
        {
            var guid = Guid.NewGuid();
            dividingModifiers[guid] = value;
            UpdateCurrent();
            return guid;
        }

        public Guid RegisterLowerBound(T value)
        {
            var guid = Guid.NewGuid();
            minModifiers[guid] = value;
            UpdateCurrent();
            return guid;
        }

        public Guid RegisterUpperBound(T value)
        {
            var guid = Guid.NewGuid();
            maxModifiers[guid] = value;
            UpdateCurrent();
            return guid;
        }

        public void FreeAdd(Guid guid) { additiveModifiers.Remove(guid); UpdateCurrent(); }
        public void FreeMultiply(Guid guid) { multiplicativeModifiers.Remove(guid); UpdateCurrent(); }
        public void FreeDivide(Guid guid) { dividingModifiers.Remove(guid); UpdateCurrent(); }
        public void FreeFloor(Guid guid) { minModifiers.Remove(guid); UpdateCurrent(); }
        public void FreeCeil(Guid guid) { maxModifiers.Remove(guid); UpdateCurrent(); }

        public ModifyableValue<T> GetDeriving() => GetDeriving(v => v);
        public ModifyableValue<K> GetDeriving<K>(Func<T, K> Cast) where K : ISignedNumber<K>, IComparisonOperators<K, K, bool>
        {
            var deriving = new ModifyableValue<K>(Cast.Invoke(this.Current));
            this.OnChanged += value => deriving.Base = Cast.Invoke(value);
            return deriving;
        }

        public static ModifyableValue<T> Merge(ModifyableValue<T> a, ModifyableValue<T> b, Func<T, T, T> MergeOperation)
        {
            var merged = new ModifyableValue<T>(MergeOperation.Invoke(a.Current, b.Current));
            a.OnChanged += value => merged.Base = MergeOperation.Invoke(a.Current, b.Current);
            b.OnChanged += value => merged.Base = MergeOperation.Invoke(a.Current, b.Current);
            return merged;
        }
        public static ModifyableValue<T> operator +(ModifyableValue<T> a, ModifyableValue<T> b) => Merge(a, b, (a, b) => a + b);
        public static ModifyableValue<T> operator -(ModifyableValue<T> a, ModifyableValue<T> b) => Merge(a, b, (a, b) => a - b);
        public static ModifyableValue<T> operator *(ModifyableValue<T> a, ModifyableValue<T> b) => Merge(a, b, (a, b) => a * b);
        public static ModifyableValue<T> operator /(ModifyableValue<T> a, ModifyableValue<T> b) => Merge(a, b, (a, b) => a / b);
    }
}