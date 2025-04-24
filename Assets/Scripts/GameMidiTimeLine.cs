using UnityEngine;

public class GameMidiTimeLine : MonoBehaviour
{
    public GameLane laneReference; // Tham chi?u ??n Lane ?? l?y zoomFactor và n?t
    //public float width = 5f; // Chi?u r?ng c?a timeline (?? kh?p v?i giao di?n)
    private Renderer renderer; // ?? thay ??i màu s?c (hi?u ?ng tr?c quan)
    public GameSongManager GameSongManager;
    //public Text playPauseButtonText;
    public static GameMidiTimeLine Instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        if (laneReference == null && GameSongManager.Instance.lanes.Length > 0)
        {
            laneReference = GameSongManager.Instance.lanes[0];
        }

        // ?i?u ch?nh kích th??c c?a timeline
        //transform.localScale = new Vector3(width, 0.1f, 1f); // Chi?u cao m?ng, chi?u r?ng tùy ch?nh
        transform.localPosition = new Vector3(0, GameSongManager.Instance.noteTapY, 0); // B?t ??u t? noteTapY

        renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Cube.mesh");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameSongManager.Instance.isPlaying || laneReference == null || GameSongManager.midiFile == null) return;

        // L?y th?i gian hi?n t?i
        double currentTime = GameSongManager.GetAudioSourceTime();

        // Tính v? trí Y c?a timeline
        float yPosition = GameSongManager.Instance.noteTapY + ((float)currentTime * GameSongManager.Instance.noteTime * laneReference.zoomFactor);
        transform.localPosition = new Vector3(0, yPosition, 0);
    }

    public void UpdateTimeLinePosition(double newTime)
    {
        float yPosition = GameSongManager.Instance.noteTapY + ((float)newTime * GameSongManager.Instance.noteTime * laneReference.zoomFactor);
        transform.localPosition = new Vector3(0, yPosition, 0);
    }

    //public void PlayButtonClicked()
    //{
    //    if (GameSongManager != null)
    //    {
    //        GameSongManager.PlaySong(); // G?i PlaySong t? GameSongManager
    //    }
    //    else
    //    {
    //        Debug.LogError("GameSongManager ch?a ???c gán!");
    //    }
    //}

    //public void PlayPauseButtonClicked()
    //{
    //    if (GameSongManager != null)
    //    {
    //        GameSongManager.PlayPauseSong();

    //        if (songManager.isPlaying)
    //        {
    //            playPauseButtonText.text = "Pause";
    //        }
    //        else
    //        {
    //            playPauseButtonText.text = "Play";
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("SongManager ch?a ???c gán!");
    //    }
    //}

    //public void StopButtonClicked()
    //{
    //    if (songManager != null)
    //    {
    //        songManager.StopSong();
    //        playPauseButtonText.text = "Play";
    //    }
    //    else
    //    {
    //        Debug.LogError("SongManager ch?a ???c gán!");
    //    }
    //}

    public void ResetTimeline()
    {
        transform.localPosition = new Vector3(0, GameSongManager.Instance.noteTapY, 0);
    }

    //// Reset lai thanh nut play khi open file moi cho SongManager.cs
    //public void ResetAfterOpenFile()
    //{
    //    if (playPauseButtonText != null)
    //    {
    //        playPauseButtonText.text = "Play";
    //    }
    //}

    //public void FastForwardButtonClicked()
    //{
    //    if (songManager != null)
    //    {
    //        songManager.FastForwardMeasure();
    //    }
    //}

    //public void RewindButtonClicked()
    //{
    //    if (songManager != null)
    //    {
    //        songManager.RewindMeasure();
    //    }
    //}

    //void SeekToNearestBeat()
    //{
    //    // L?y v? trí chu?t trong không gian màn hình
    //    Vector3 mousePosition = Input.mousePosition;

    //    // Chuy?n ??i v? trí chu?t sang t?a ?? th? gi?i
    //    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

    //    // L?y v? trí Y
    //    float clickedY = worldPosition.y;

    //    // Chuy?n v? trí Y thành th?i gian
    //    float clickedTime = (clickedY - SongManager.Instance.noteTapY) / (SongManager.Instance.noteTime * laneReference.zoomFactor);

    //    // Tìm th?i gian g?n nh?t trong danh sách các beat
    //    double closestBeatTime = Mathf.Round((float)clickedTime / SongManager.Instance.beatDuration) * SongManager.Instance.beatDuration;

    //    // C?p nh?t th?i gian cho timeline
    //    SongManager.Instance.SetTime(closestBeatTime);
    //}

    //void OnMouseDown()
    //{
    //    Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    SeekToNearestBeat(clickPosition);
    //}
}
