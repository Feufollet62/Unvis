using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float timeToStart = 3f;
    
    private float randomTimer;
    private float startTimer;
    private float nextAnim;

    private bool gameStarting = false;

    private void Start()
    {
        nextAnim = Random.Range(7f, 20f);
    }

    private void Update()
    {
        if (!gameStarting)
        {
            randomTimer += Time.deltaTime;

            if (randomTimer > nextAnim)
            {
                animator.SetTrigger("Pose");
            
                randomTimer = 0;
                nextAnim = Random.Range(7f, 20f);
            }
        }
        else
        {
            startTimer += Time.deltaTime;

            if (startTimer > timeToStart)
            {
                SceneManager.LoadScene("NewLevel");
            }
        }
    }

    public void StartButton()
    {
        gameStarting = true;
        animator.SetTrigger("Aim");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
