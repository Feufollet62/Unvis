using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject Buttons;

    public void ShowMenu()
    {
        Buttons.SetActive(true);
    }
    
    public void Restart()
    {
        SceneManager.LoadScene("NewLevel");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main menu");
    }
}
