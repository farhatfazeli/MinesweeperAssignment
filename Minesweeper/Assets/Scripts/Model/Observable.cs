using System;

namespace Minesweeper.Model
{
    // This class handles making properties with callback handling abilities
    public class Observable<T>
    {
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

        public void Subscribe(Action<T> callback)
        {
            _action += callback;
        }

        public void Unsubscribe(Action<T> callback)
        {
            _action -= callback;
        }

        public Observable(T value)
        {
            _value = value;
        }

        private T _value;
        private Action<T> _action;
    }

}