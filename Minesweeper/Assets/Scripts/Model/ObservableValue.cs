namespace Minesweeper.Model
{
    // This class simplifies making properties with callback handling abilities
    public class ObservableValue<T> : Observable<T>
    {
        private T _value;

        public ObservableValue(T value)
        {
            _value = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value)) return;
                _value = value;
                _action?.Invoke(value);
            }
        }

        public void Invoke()
        {
            _action?.Invoke(_value);
        }
    }
}