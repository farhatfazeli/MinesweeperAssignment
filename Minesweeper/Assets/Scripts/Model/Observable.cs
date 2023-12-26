using System;

namespace Minesweeper.Model
{
    public class Observable<T>
    {
        public void Subscribe(Action<T> callback)
        {
            _action += callback;
        }

        public void Unsubscribe(Action<T> callback)
        {
            _action -= callback;
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

        private T _value;
        private Action<T> _action;

        public Observable(T value)
        {
            _value = value;
        }
    }

}