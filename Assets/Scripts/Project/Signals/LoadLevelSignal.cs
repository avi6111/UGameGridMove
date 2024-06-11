/// <summary>
/// 这里 single 类，是和游戏强关联的，为了GAME 而写的 
/// </summary>
public class LoadLevelSignal
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="levelNumber"></param>
    public LoadLevelSignal(int levelNumber) => LevelNumber = levelNumber;

    private int _levelNumber;
    public int LevelNumber { 
        get => _levelNumber;
        set {
            if(value < 1)
                throw new System.Exception("Level number can't be less than one");

            _levelNumber = value;
        }
    }
}
