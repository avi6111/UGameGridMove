using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 利用 MVVM的 ICommand， 增加是否可点击的条件。。。。
/// </summary>
public class BindableAnimatedButton : AnimatedButton
{
    private ICommand _bindedCommand;
    /// <summary>
    /// 有空研究下这个方法，哪里绑定，和绑定的事件是？？
    /// </summary>
    /// <param name="command"></param>
    public void Bind(ICommand command){
        _bindedCommand = command;
        OnClick.AddListener(_bindedCommand.Execute);
        //command.CanExecuteChanged += () => Interactable = command.CanExecute();
        command.CanExecuteChanged += OnCanExecuteChanged;
        Interactable = command.CanExecute();
        Debug.LogError("OnBindComman bind()");
    }

    public void Unbind(){
        _bindedCommand = null;
        OnClick.RemoveListener(_bindedCommand.Execute);
        _bindedCommand.CanExecuteChanged -= OnCanExecuteChanged;
    }

    private void OnCanExecuteChanged(){
        Debug.LogError("OnBindComman Execute Changed in AnimButton??");
        Interactable = _bindedCommand.CanExecute();
    }
}