namespace Headway.Core.Flow
{
    public static class StateExtensions
    {
        public static State<T> Next<T>(State<T> state, string nextStateCode)
        {
            return state;
        }

        public static State<T> Revert<T>(State<T> state, string revertStateCode)
        {
            return state;
        }

        public static State<T> Reset<T>(State<T> state)
        {
            return state;
        }
    }
}
