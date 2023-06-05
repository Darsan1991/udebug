using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DGames.Utils;
using UnityEngine;

public interface IDebugger
{
    void Debug(string message ,Object context,params string[] tags);
    
}

public class CombinedDebugger : IDebugger, IEnumerable<IDebugger>
{
    private readonly List<IDebugger> _debuggers;

    public CombinedDebugger(params IDebugger[] debuggers)
    {
        _debuggers = debuggers.ToList();
    }
    
    public void Debug(string message,  Object context,params string[] tags)
    {
        _debuggers.ForEach(d=>d.Debug(message,context,tags));
    }

    public IEnumerator<IDebugger> GetEnumerator()
    {
        return _debuggers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}