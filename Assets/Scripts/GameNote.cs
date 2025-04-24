using UnityEngine;

public class GameNote : MonoBehaviour
{
    public float assignedTime;
    public float assignedDuration;

    private SpriteRenderer spriteRenderer;
    private bool isHolding = false;
    private Color originalColor;
    private Color targetColor = Color.red;

    public bool IsHolding => isHolding;
    public float Zoom
    {
        get
        {
            GameLane lane = GetComponentInParent<GameLane>();
            return lane != null ? lane.zoomFactor : 1f;
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateNoteAppearance();
        originalColor = spriteRenderer.color;
        Debug.Log($"[NoteGamePlay] Spawned note {name} - Assigned Duration: {assignedDuration}, Beat Duration: {GameSongManager.Instance.beatDuration}");
    }

    void Update()
    {
        spriteRenderer.enabled = true;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        bool isHitThis = (hit.collider != null && hit.collider.gameObject == gameObject);

        // Nh?n chu?t trái
        if (Input.GetMouseButtonDown(0) && isHitThis)
        {
            float cameraBottomY = Camera.main.transform.position.y - Camera.main.orthographicSize;
            float noteBottomY = transform.position.y - (assignedDuration * Zoom / 2);

            if (noteBottomY < cameraBottomY)
            {
                Debug.Log($"[NoteGamePlay] Cannot hit note {name} - It has passed the camera bottom (NoteBottomY: {noteBottomY}, CameraBottomY: {cameraBottomY})");
                return;
            }

            int score = GameSongManager.Instance.CalculateNoteScore(assignedDuration);
            GameSongManager.Instance.AddScore(score);

            Debug.Log($"[NoteGamePlay] Clicked note {name} - Duration: {assignedDuration}, Beat: {GameSongManager.Instance.beatDuration}");

            if (GameSongManager.Instance.CanHitNote(this))
            {
                if (assignedDuration >= GameSongManager.Instance.beatDuration)
                {
                    Debug.Log("[NoteGamePlay] Holding note...");
                    isHolding = true;
                }
                else
                {
                    Debug.Log("[NoteGamePlay] Short note - Destroy immediately");
                    GameSongManager.Instance.HitNote(this);
                    Destroy(gameObject);
                }
            }
        }

        // Nh? chu?t trái
        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            Debug.Log($"[NoteGamePlay] Released note {name}");
            isHolding = false;
            spriteRenderer.color = originalColor;
            GameSongManager.Instance.HitNote(this);
            Destroy(gameObject);
        }

        // Trong lúc gi? chu?t
        if (isHolding)
        {
            float hitY = mousePos.y;
            float noteBottomY = transform.position.y - (assignedDuration * Zoom / 2);
            float noteTopY = transform.position.y + (assignedDuration * Zoom / 2);

            if (hitY >= noteBottomY && hitY <= noteTopY)
            {
                float progress = Mathf.InverseLerp(noteBottomY, noteTopY, hitY);
                spriteRenderer.color = Color.Lerp(originalColor, targetColor, progress);
            }
        }
    }

    private void UpdateNoteAppearance()
    {
        transform.localScale = new Vector2(1, (float)(assignedDuration * Zoom));
    }
}
