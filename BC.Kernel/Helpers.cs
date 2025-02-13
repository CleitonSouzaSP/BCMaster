using MediatR;

namespace BC.Kernel
{
    public static class Helpers
    {
        public static readonly NoneType None;

        private static readonly Unit unit;

        public static Unit Unit()
        {
            return unit;
        }

        public static Func<T, Unit> ToFunc<T>(Action<T> action)
        {
            return delegate (T o)
            {
                action(o);
                return Unit();
            };
        }

        public static Func<Unit> ToFunc(Action action)
        {
            return delegate
            {
                action();
                return Unit();
            };
        }
        
    }
}
