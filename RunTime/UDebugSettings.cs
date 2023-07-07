using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DGames.DDebug
{
    public class UDebugSettings : ScriptableObject
    {
        public static UDebugSettings Default => Resources.Load<UDebugSettings>(nameof(UDebugSettings));

        [SerializeField] private List<PlatformDebuggers> _debuggers = new();


        public IDebugger GetDebugger(RuntimePlatform platform) =>
            new CombinedDebugger(_debuggers.FirstOrDefault(d => d.platform == platform).ActiveDebuggers.ToArray());

        [Serializable]
        public struct PlatformDebuggers
        {
            public RuntimePlatform platform;
            public List<DebuggerAndActive> debuggers;

            public IEnumerable<IDebugger> ActiveDebuggers =>
                debuggers?.Where(d => d.active).Select(d => d.debugger) ?? new List<Debugger>();
        }


        [Serializable]
        public struct DebuggerAndActive
        {
            public Debugger debugger;
            public bool active;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("MyGames/Debug/Settings")]
        public static void Open()
        {
            ScriptableEditorUtils.OpenOrCreateDefault<UDebugSettings>();
        }
#endif
    }
}