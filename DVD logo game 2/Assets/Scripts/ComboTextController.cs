using System.Collections;
using UnityEngine;
using TMPro;

public class ComboTextController : MonoBehaviour
{
    public TMP_Text comboText; // Reference to the TMP_Text component
    public RectTransform comboRectTransform; // Reference to the RectTransform component
    public float moveUpDistance = 100f; // The total distance the text will move up
    public float moveUpDuration = 1f; // Duration for moving up before shrinking
    public float shrinkDuration = 0.5f; // Duration for shrinking the text
    public float finalMoveUpDistance = 50f; // Additional move distance after shrinking

    private void Start()
    {
        StartCoroutine(MoveAndShrinkText());
    }

    IEnumerator MoveAndShrinkText()
    {
        Vector2 initialPosition = comboRectTransform.anchoredPosition;
        float elapsedTime = 0f;

        // Step 1: Move up the text without shrinking
        while (elapsedTime < moveUpDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / moveUpDuration;

            // Linearly interpolate the position upwards
            float moveY = Mathf.Lerp(0f, moveUpDistance, progress);
            comboRectTransform.anchoredPosition = initialPosition + new Vector2(0f, moveY);

            yield return null;
        }

        // Step 2: Shrink the text, fade it out, and move it up a bit more
        elapsedTime = 0f;
        float initialFontSize = comboText.fontSize;
        Color initialColor = comboText.color;
        Vector2 midPosition = comboRectTransform.anchoredPosition;

        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / shrinkDuration;

            // Linearly interpolate the font size to 0
            comboText.fontSize = Mathf.Lerp(initialFontSize, 0f, progress);

            // Linearly interpolate the position upwards again
            float additionalMoveY = Mathf.Lerp(0f, finalMoveUpDistance, progress);
            comboRectTransform.anchoredPosition = midPosition + new Vector2(0f, additionalMoveY);

            // Linearly interpolate the color alpha to 0
            comboText.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(initialColor.a, 0f, progress));

            yield return null;
        }

        // Optionally destroy or deactivate the text after the effect
        Destroy(gameObject);
    }
}
