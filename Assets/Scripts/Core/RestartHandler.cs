using System;

namespace Core
{
    public static class RestartHandler
    {
        public static event Action OnRestart;
        public static void Restart() => OnRestart?.Invoke();
    }
}
