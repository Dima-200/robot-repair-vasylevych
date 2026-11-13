
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    EnemyController[] enemies;
    public UIHandler uiHandler;

    private bool gameEnded = false;
    private bool gameStarted = false;

    void Start()
    {
        enemies = FindObjectsByType<EnemyController>(
            FindObjectsSortMode.None);
        uiHandler.ShowStartScreen(BeginGame, ReloadScene);
    }

    void Update()
    {
        if (gameEnded || !gameStarted) return;

        if (player.health <= 0)
        {
            gameEnded = true;
            uiHandler.DisplayLoseScreen();
            return;
        }

        if (enemies.Length > 0 && AllEnemiesFixed())
        {
            gameEnded = true;
            uiHandler.DisplayWinScreen();
        }
    }

    bool AllEnemiesFixed()
    {
        foreach (EnemyController enemy in enemies)
        {
            if (enemy.isBroken)
                return false;
        }

        return true;
    }

    void BeginGame()
    {
        gameStarted = true;
        Time.timeScale = 1f;
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
    }
}
