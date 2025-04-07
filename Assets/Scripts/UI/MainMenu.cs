using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public int mainMenuIndex = 0;

    public void StartGame()
    {
        LevelLoader._Instance.loadedIndex = mainMenuIndex;
        LevelLoader._Instance.LoadLobby();
    }
}
