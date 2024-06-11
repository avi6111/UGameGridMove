using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{
    public event System.Action<int> GameLevelLoaded;
    public event System.Action<int> GameLevelLoading;

    public int LastLoadedLevelNumber { get; private set; }
    //已经“写死”在 ProjectContext.prefab 这个预制体；如需改名，则需要改预制体
    [SerializeField] private string tutorialSceneName = "Tutorial";
    [SerializeField] private string menuSceneName = "Menu";
    //已经“写死”在 ProjectContext.prefab 这个预制体；如需改名，则需要改预制体
    [SerializeField] private string baseSceneName = "Key Level-Base";//"Base";
    //已经“写死”在 ProjectContext.prefab 这个预制体；如需改名，则需要改预制体
    [SerializeField] private string levelSceneName = "Level";
    //写死。。。。
    /// <summary>
    /// UI动画 :D ，还没研究
    /// </summary>
    private ScenesTransitions _scenesTransitions;
    /// <summary>
    /// 第一个注入，用的通用的 ScenesTransitions
    /// </summary>
    /// <param name="scenesTransitions"></param>
    [Zenject.Inject]
    private void Init(ScenesTransitions scenesTransitions){
        _scenesTransitions = scenesTransitions;
    }

    public void LoadLevel(int number){
        if(number == 0) number = 1;
        GameLevelLoading?.Invoke(number);

        AddTransition(() => {
            //load "Level 2"
            var async = SceneManager.LoadSceneAsync($"{levelSceneName} {number}");
            if(async!=null)
                async.completed += (asyncOperation) => LevelLoaded(number);
            //load "Base"场景 必须load 一个base 场景。。。。
            SceneManager.LoadSceneAsync(baseSceneName, LoadSceneMode.Additive);
        });
    }

    public void LoadMenu(){
        LoadScene(menuSceneName);
    } 
    
    public void LoadTutorial() => LoadScene(tutorialSceneName);

    public void LoadNextLevel(){
        LoadLevel(LastLoadedLevelNumber + 1);
    }

    private void LoadScene(string name){
        AddTransition(() => {
            SceneManager.LoadSceneAsync(name).completed += (asyncOperation) => CloseTransition();
        });
    }

    public void RestartLevel(){
        LoadLevel(LastLoadedLevelNumber);
    }

    private void LevelLoaded(int levelNumber){
        LastLoadedLevelNumber = levelNumber;
        GameLevelLoaded?.Invoke(levelNumber);
        CloseTransition();
    }

    private void AddTransition(System.Action onBeginLoad) => _scenesTransitions.CreateNewTransition(onBeginLoad);
    private void CloseTransition() => _scenesTransitions.CloseCurrentTransition();
}