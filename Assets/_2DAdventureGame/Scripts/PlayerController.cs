
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float speed = 3.0f;

    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;
    public InputAction LaunchAction;
    public InputAction TalkAction;
    public AudioClip hitSound;
    public AudioClip healSound;
    SpriteRenderer spriteRenderer;
    Color originalColor;
    Coroutine feedbackRoutine;
    private NonPlayerCharacter lastNonPlayerCharacter;

    void Start()
    {
        MoveAction.Enable();
        LaunchAction.Enable();
        TalkAction.Enable();

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
        currentHealth = maxHealth;
    }

    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

        if (!Mathf.Approximately(move.x, 0.0f)
            || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;

            if (damageCooldown < 0)
                isInvincible = false;
        }

        if (LaunchAction.WasPressedThisFrame())
        {
            Launch();
        }

        RaycastHit2D hit = Physics2D.Raycast(
            rigidbody2d.position + Vector2.up * 0.2f,
            moveDirection, 1.5f, LayerMask.GetMask("NPC"));

        if (hit.collider != null)
        {
            NonPlayerCharacter npc =
                hit.collider.GetComponent<NonPlayerCharacter>();

            npc.dialogueBubble.SetActive(true);
            lastNonPlayerCharacter = npc;
            FindFriend();
        }
        else
        {
            if (lastNonPlayerCharacter != null)
            {
                lastNonPlayerCharacter.dialogueBubble.SetActive(false);
                lastNonPlayerCharacter = null;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position
            + move * speed * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
            PlayFeedback(hitSound);
            ShowFeedback(new Color(1f, 0.55f, 0.55f));
        }

        currentHealth = Mathf.Clamp(
            currentHealth + amount, 0, maxHealth);

        UIHandler.instance.SetHealthValue(
            currentHealth / (float)maxHealth);
    }

    public void PlayHealFeedback()
    {
        PlayFeedback(healSound);
        ShowFeedback(new Color(0.55f, 1f, 0.65f));
    }

    void PlayFeedback(AudioClip clip)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    void ShowFeedback(Color color)
    {
        if (spriteRenderer == null)
            return;

        if (feedbackRoutine != null)
            StopCoroutine(feedbackRoutine);
        feedbackRoutine = StartCoroutine(FlashSprite(color));
    }

    IEnumerator FlashSprite(Color color)
    {
        spriteRenderer.color = color;
        yield return new WaitForSecondsRealtime(0.12f);
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
        feedbackRoutine = null;
    }

    void Launch()
    {
        if (projectilePrefab == null)
            return;

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            rigidbody2d.position + moveDirection * 0.8f,
            Quaternion.identity);

        Projectile projectile =
            projectileObject.GetComponent<Projectile>();

        if (projectile == null)
        {
            Destroy(projectileObject);
            return;
        }

        projectile.Launch(moveDirection, 300);
        animator.SetTrigger("Launch");
    }

    void FindFriend()
    {
        if (TalkAction.WasPressedThisFrame())
        {
            UIHandler.instance.DisplayDialogue();
        }
    }
}
