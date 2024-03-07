using UnityEngine;
using TMPro;

public class TokenUIScript : MonoBehaviour
{
    public PlayerMovement player; // Reference to the PlayerMovement script
    public TextMeshProUGUI tokenText; // Reference to the TextMeshPro UI text for displaying the token count

    void Update()
    {
        if (player != null && tokenText != null)
        {
            tokenText.text = "Tokens: " + player.GetTokenCount().ToString(); // Update the UI text to display the token count
        }
    }
}
