using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TapToStart : MonoBehaviour
{
    public Button tapToStartButton;
    public GameSongManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tapToStartButton.onClick.AddListener(StartGame);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartGame();
    }

    void StartGame()
    {
        Debug.Log("Button Clicked!"); // Ki?m tra xem click c� ho?t ??ng kh�ng

        // ?n button (c�ch ??n gi?n nh?t)
        tapToStartButton.gameObject.SetActive(false);

        PlayGameTap();
    }

    void PlayGameTap()
    {
        if (gameManager != null)
        {
            gameManager.PlaySong(); // G?i PlaySong t? GameManager
        }
    }
}
