
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    EnemyController[] enemies;
    public UIHandler uiHandler;

    private bool gameEnded = false;

    void Start()
    {
        enemies = FindObjectsByType<EnemyController>(
            FindObjectsSortMode.None);
    }

    void Update()
    {
        if (gameEnded) return;

        if (player.health <= 0)
        {
            gameEnded = true;
            uiHandler.DisplayLoseScreen();
            Invoke(nameof(ReloadScene), 3f);
            return;
        }

        if (enemies.Length > 0 && AllEnemiesFixed())
        {
            gameEnded = true;
            uiHandler.DisplayWinScreen();
            Invoke(nameof(ReloadScene), 3f);
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

    void ReloadScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
    }
}
