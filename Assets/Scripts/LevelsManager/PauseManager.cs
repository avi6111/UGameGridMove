using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IPauseHandler
{
    void SetPaused(bool isPaused);
}

public class PauseManager
{
    static PauseManager _instance;
    public static PauseManager Inst{get{
        if(_instance==null)
        {
            _instance = new PauseManager();
        }
        return _instance;
    }}
    

    private List<IPauseHandler> _pauseHandlers = new List<IPauseHandler>();
    public void Subscribe(IPauseHandler handler)
    {
        Debug.LogError("<color=cyan>PauseManager->Subscribe()</color>");
        _pauseHandlers.Add(handler);
    }
    public void SetPaused(bool isPaused){
        foreach(var handler in _pauseHandlers)
            handler.SetPaused(isPaused);
    }
}


