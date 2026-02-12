using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerTextDisplay : MonoBehaviour
{
    public static PlayerTextDisplay Instance;

    [Header("References")]
    public GameObject panel;          // The UI panel (Image + border)
    public TextMeshProUGUI textMesh;  // TMP text
    public float delay = 0.05f;       // Typewriter speed
    public float panelHideDelay = 1f; // Delay before hiding panel after typing

    private Coroutine typingCoroutine;

    private void Awake()
    {
        Instance = this;

        // Hide panel at start
        panel.SetActive(false);

        // Clear text
        textMesh.text = "";
    }

    public void DisplayText(string message)
    {
        // Show panel first
        panel.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeTextCoroutine(message));
    }

    private IEnumerator TypeTextCoroutine(string message)
    {
        textMesh.text = "";

        foreach (char c in message)
        {
            textMesh.text += c;
            yield return new WaitForSeconds(delay);
        }

        typingCoroutine = null;

        // Wait before hiding panel
        yield return new WaitForSeconds(panelHideDelay);

        panel.SetActive(false);
    }
}
