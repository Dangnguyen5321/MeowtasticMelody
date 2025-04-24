using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public float cameraYOffset = 3f;
    void Update()
    {
        if (GameSongManager.Instance != null && GameSongManager.Instance.isPlaying)
        {
            MoveCameraToMidiTime();
        }
    }

    public void MoveCameraToMidiTime()
    {
        if (GameSongManager.Instance == null) return;
        if (GameSongManager.Instance.lanes == null || GameSongManager.Instance.lanes.Length == 0) return;

        double currentTime = GameSongManager.GetAudioSourceTime();

        //Tính toán v? trí Y d?a trên th?i gian MIDI
        float newY = (float)(currentTime * GameSongManager.Instance.noteTime * GameSongManager.Instance.lanes[0].zoomFactor) + GameSongManager.Instance.noteTapY + cameraYOffset;

        //C?p nh?t v? trí c?a camera
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        //Debug.Log($"[GameCamera] Current Time: {currentTime}, Calculated Y: {newY}, noteTime: {GameSongManager.Instance.noteTime}, zoomFactor: {GameSongManager.Instance.lanes[0].zoomFactor}, noteTapY: {GameSongManager.Instance.noteTapY}");
    }
}
