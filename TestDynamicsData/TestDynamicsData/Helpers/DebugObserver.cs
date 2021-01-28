using System;

namespace TestDynamicsData.Helpers
{
    public class DebugObserver<T> : IObserver<T>
    {
        public string ObservableName { get; }
        public DebugObserver(string name)
        {
            ObservableName = name;
        }

        public virtual void OnCompleted()
        {
            LogHelper.Log($"[{ObservableName}] OnCompleted");
        }

        public virtual void OnError(Exception error)
        {
            LogHelper.Log($"[{ObservableName}] OnError");
            LogHelper.LogException(error);
        }

        public virtual void OnNext(T value)
        {
            LogHelper.Log("[{0}] OnNext(): {1}", ObservableName, value.ToString());
        }
    }
}
