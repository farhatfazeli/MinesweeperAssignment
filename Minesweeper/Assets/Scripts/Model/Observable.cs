using System;

namespace Minesweeper.Model
{
    // This class simplifies callback handling abilities
    public class Observable<T>
    {
        protected Action<T> _action;

        public void Subscribe(Action<T> callback)
        {
            _action += callback;
        }

        public void Unsubscribe(Action<T> callback)
        {
            _action -= callback;
        }

        public void Invoke(T value)
        {
            _action?.Invoke(value);
        }
    }
}
