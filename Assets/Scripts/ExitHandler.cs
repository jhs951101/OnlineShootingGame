using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitHandler : MonoBehaviour
{
    public enum BackType
    {
        Back,
        Quit
    }

    public BackType backType;
    public string previousScene;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            BackButtonPressed();
    }

    public void BackButtonPressed()
    {
        if (backType == BackType.Back)
        {
            SceneManager.LoadScene(previousScene);
        }
        else if (backType == BackType.Quit)
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }
    }
}
