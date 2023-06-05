using UnityEngine;

namespace DGames.DDebug
{
    public static class UDebug
    {
        private static readonly UDebugSettings _uDebugSettings;
        private static IDebugger _debugger;

        static UDebug()
        {
            _uDebugSettings = UDebugSettings.Default;
        }



        public static void Debug(string message, Object context = null, params string[] tags)
        {
            CurrentDebugger?.Debug(message, context, tags);
        }

        public static void Debug(string message, params string[] tags)
        {
            CurrentDebugger?.Debug(message, null, tags);
        }

        private static IDebugger CurrentDebugger => _debugger ??= _uDebugSettings.GetDebugger(Application.platform);
    }
}