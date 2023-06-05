using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DGames.DDebug
{
    [CreateAssetMenu(menuName = "Debugger/Editor Debugger")]

    public class EditorDebugger : Debugger
    {
        protected override void WriteDebug(string message, Object context, params string[] tags)
        {
            UnityEngine.Debug.Log(BuildString(message, context, tags), context);
        }
    }

    public abstract class Debugger : ScriptableObject, IDebugger
    {
        [SerializeField] protected DebuggerSettings settings;

        public void Debug(string message, Object context, params string[] tags)
        {
            if (tags.Any() && settings.IsTagsAllowed(tags))
                return;

            WriteDebug(message, context, settings.DebugTags ? tags : Array.Empty<string>());
        }

        protected abstract void WriteDebug(string message, Object context, params string[] tags);

        protected virtual string BuildString(string message, Object context, params string[] tags)
        {
            var tagsString = $"[{string.Join(',', tags)}]";
            return $"{message} " + (context ? "- {context.name}" : "") + tagsString;
        }

    }

    [Serializable]
    public class DebuggerSettings
    {
        [SerializeField] private List<TagAndActive> _whiteList;
        [SerializeField] private List<TagAndActive> _blackList;
        [SerializeField] private bool _debugTags;

        public bool DebugTags => _debugTags;

        public IEnumerable<string> WhiteList =>
            _whiteList?.Where(t => t.active).Select(t => t.tag) ?? new List<string>();

        public IEnumerable<string> BlackList =>
            _blackList?.Where(t => t.active).Select(t => t.tag) ?? new List<string>();

        public bool IsTagsAllowed(params string[] tags) => !BlackList.Any(tags.Contains) &&
                                                           (!WhiteList.Any() || tags.Any(t => WhiteList.Contains(t)));

        [Serializable]
        public struct TagAndActive
        {
            public string tag;
            public bool active;
        }
    }
}