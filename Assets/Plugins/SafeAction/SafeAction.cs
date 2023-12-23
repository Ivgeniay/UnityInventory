using System.Linq;
using System;

namespace System
{
    public class SafeAction
    {
        private event Action _action;

        public event Action Event
        {
            add
            {
                Subscribe(value);
            }
            remove
            {
                Unsubscribe(value);
            }
        }

        public bool Subscribe(Action action)
        {
            if (_action == null) _action += action;
            else if (!action.GetInvocationList().Contains(action))
            {
                _action += action;
                return true;
            }
            return false;
        }

        public void Unsubscribe(Action action)
        {
            _action -= action;
        }

        public void Invoke()
        {
            _action?.Invoke();
        }

        public static implicit operator SafeAction(Action action)
        {
            SafeAction sa = new SafeAction();
            sa._action = action;
            return sa;
        }

        public static SafeAction operator +(SafeAction actionS, Action action)
        {
            if (actionS._action == null) actionS._action += action;
            else
            if (!actionS._action.GetInvocationList().Contains(action))
            {
                actionS._action += action;
            }
            return actionS;
        }

        public static SafeAction operator -(SafeAction actionS, Action action)
        {
            actionS._action -= action;
            return actionS;
        }
    }
    public class SafeAction<T> 
    {
        private event Action<T> _action;

        public event Action<T> Event
        {
            add
            {
                Subscribe(value);
            }
            remove
            {
                Unsubscribe(value);
            }
        }

        public bool Subscribe(Action<T> action)
        {
            if (_action == null) _action += action;
            else if (!action.GetInvocationList().Contains(action))
            {
                _action += action;
                return true;
            }
            return false;
        }

        public void Unsubscribe(Action<T> action)
        {
            _action -= action;
        }

        public void Invoke(T value)
        {
            _action?.Invoke(value);
        }

        public static implicit operator SafeAction<T>(Action<T> action)
        {
            SafeAction<T> sa = new SafeAction<T>();
            sa._action = action;
            return sa;
        }

        public static SafeAction<T> operator +(SafeAction<T> actionS, Action<T> action)
        {
            if (actionS._action == null) actionS._action += action;
            else 
            if (!actionS._action.GetInvocationList().Contains(action))
            {
                actionS._action += action;
            }
            return actionS;
        }

        public static SafeAction<T> operator -(SafeAction<T> actionS, Action<T> action)
        {
            actionS._action -= action;
            return actionS;
        }
    }
    public class SafeAction<T, T2> 
    {
        private event Action<T, T2> _action;

        public event Action<T, T2> Event
        {
            add
            {
                Subscribe(value);
            }
            remove
            {
                Unsubscribe(value);
            }
        }

        public bool Subscribe(Action<T, T2> action)
        {
            if (_action == null) _action += action;
            else if (!action.GetInvocationList().Contains(action))
            {
                _action += action;
                return true;
            }
            return false;
        }

        public void Unsubscribe(Action<T, T2> action)
        {
            _action -= action;
        }

        public void Invoke(T value, T2 value2)
        {
            _action?.Invoke(value, value2);
        }

        public static implicit operator SafeAction<T, T2>(Action<T, T2> action)
        {
            SafeAction<T, T2> sa = new SafeAction<T, T2>();
            sa._action = action;
            return sa;
        }

        public static SafeAction<T, T2> operator +(SafeAction<T, T2> actionS, Action<T, T2> action)
        {
            if (actionS._action == null) actionS._action += action;
            else
            if (!actionS._action.GetInvocationList().Contains(action))
            {
                actionS._action += action;
            }
            return actionS;
        }

        public static SafeAction<T, T2> operator -(SafeAction<T, T2> actionS, Action<T, T2> action)
        {
            actionS._action -= action;
            return actionS;
        }
    }
    public class SafeAction<T, T2, T3> 
    {
        private event Action<T, T2, T3> _action;

        public event Action<T, T2, T3> Event
        {
            add
            {
                Subscribe(value);
            }
            remove
            {
                Unsubscribe(value);
            }
        }

        public bool Subscribe(Action<T, T2, T3> action)
        {
            if (_action == null) _action += action;
            else if (!action.GetInvocationList().Contains(action))
            {
                _action += action;
                return true;
            }
            return false;
        }

        public void Unsubscribe(Action<T, T2, T3> action)
        {
            _action -= action;
        }

        public void Invoke(T value, T2 value2, T3 value3)
        {
            _action?.Invoke(value, value2, value3);
        }

        public static implicit operator SafeAction<T, T2, T3>(Action<T, T2, T3> action)
        {
            SafeAction<T, T2, T3> sa = new SafeAction<T, T2, T3>();
            sa._action = action;
            return sa;
        }

        public static SafeAction<T, T2, T3> operator +(SafeAction<T, T2, T3> actionS, Action<T, T2, T3> action)
        {
            if (actionS._action == null) actionS._action += action;
            else
            if (!actionS._action.GetInvocationList().Contains(action))
            {
                actionS._action += action;
            }
            return actionS;
        }

        public static SafeAction<T, T2, T3> operator -(SafeAction<T, T2, T3> actionS, Action<T, T2, T3> action)
        {
            actionS._action -= action;
            return actionS;
        }
    }
    public class SafeAction<T, T2, T3, T4> 
    {
        private event Action<T, T2, T3, T4> _action;

        public event Action<T, T2, T3, T4> Event
        {
            add
            {
                Subscribe(value);
            }
            remove
            {
                Unsubscribe(value);
            }
        }

        public bool Subscribe(Action<T, T2, T3, T4> action)
        {
            if (_action == null) _action += action;
            else if (!action.GetInvocationList().Contains(action))
            {
                _action += action;
                return true;
            }
            return false;
        }

        public void Unsubscribe(Action<T, T2, T3, T4> action)
        {
            _action -= action;
        }

        public void Invoke(T value, T2 value2, T3 value3, T4 value4)
        {
            _action?.Invoke(value, value2, value3, value4);
        }

        public static implicit operator SafeAction<T, T2, T3, T4>(Action<T, T2, T3, T4> action)
        {
            SafeAction<T, T2, T3, T4> sa = new SafeAction<T, T2, T3, T4>();
            sa._action = action;
            return sa;
        }

        public static SafeAction<T, T2, T3, T4> operator +(SafeAction<T, T2, T3, T4> actionS, Action<T, T2, T3, T4> action)
        {
            if (actionS._action == null) actionS._action += action;
            else
            if (!actionS._action.GetInvocationList().Contains(action))
            {
                actionS._action += action;
            }
            return actionS;
        }

        public static SafeAction<T, T2, T3, T4> operator -(SafeAction<T, T2, T3, T4> actionS, Action<T, T2, T3, T4> action)
        {
            actionS._action -= action;
            return actionS;
        }
    }
}
