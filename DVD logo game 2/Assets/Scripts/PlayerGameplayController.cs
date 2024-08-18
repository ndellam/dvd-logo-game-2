using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayController : MonoBehaviour
{
    public bool isReflect;

    private Animator animator;
    private int health = 3;

    public Canvas deathScreen;
    public UIManager uiManager;
    public AbdomenController abdomenController;
    public RotateToFaceMouse legsRotateToFaceMouse;

    // Time pause duration and flashing effect settings
    public float timePauseDuration = 0.5f;
    public float flashDuration = 1.5f;
    public float flashSpeed = 10f;

    // Array of sprite renderers to flash
    public SpriteRenderer[] spriteRenderers;

    public AudioSource sfxSource;

    public AudioClip footstepClip, attackClip, hitClip, deathClip;

    // Boolean to track if the player is currently in the hit state
    private bool isHit, isDying;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("doAttack");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Logo"))
        {
            if (!isHit && !isDying)
            {
                StartCoroutine(PauseAndTakeDamage());
            }
        }
    }

    IEnumerator PauseAndTakeDamage()
    {
        // Pause time
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(timePauseDuration);

        // Resume time
        Time.timeScale = 1f;

        if (health > 1)
        {
            TakeDamage();
        }
        else
        {
            EndGame();
        }
    }

    void TakeDamage()
    {
        sfxSource.PlayOneShot(hitClip);

        health -= 1;

        abdomenController.ChangeSprite(health - 1);

        Debug.Log("Took damage");

        // Start the flashing coroutine
        StartCoroutine(FlashSprites());
    }

    IEnumerator FlashSprites()
    {
        isHit = true;

        float elapsedTime = 0f;
        bool toggle = false;

        while (elapsedTime < flashDuration)
        {
            // Toggle between 1 and 0.3
            float alpha = toggle ? 1f : 0.3f;

            SetSpritesAlpha(alpha);

            // Toggle the value for the next iteration
            toggle = !toggle;

            elapsedTime += Time.deltaTime;

            // Wait for a brief moment before toggling again
            yield return new WaitForSeconds(1f / flashSpeed);
        }

        // Reset all alphas to 1 and end the hit state
        SetSpritesAlpha(1f);
        isHit = false;
    }


    void SetSpritesAlpha(float alpha)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }

    void EndGame()
    {
        isDying = true;
        uiManager.canAddScore = false;
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerMovement.canMove = false;
        playerMovement.canRotate = false;

        legsRotateToFaceMouse.canRotate = false;

        sfxSource.PlayOneShot(deathClip);

        playerMovement.legsAnimator.SetTrigger("doDeath");
        animator.SetTrigger("doDeath");
    }

    public void ShowDeathScreen()
    {
        deathScreen.enabled = true;
    }


    public void PlayAttackSFX()
    {
        sfxSource.PlayOneShot(attackClip);
    }
}
