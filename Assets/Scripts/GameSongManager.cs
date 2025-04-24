using UnityEngine;
using Melanchall.DryWetMidi.Core;
using System.Linq;
using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using System.Threading.Tasks;
public class GameSongManager : MonoBehaviour
{
    public static GameSongManager Instance;
    public AudioSource audioSource;
    public GameLane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds
    public string fileLocation;
    //public string mp3FileLocation;

    public bool isPlaying = false;
    public static double pausedTime = 0;
    public static MidiFile midiFile;
    //private int trackIndex;
    public float beatDuration;
    public float noteTime;
    public float noteTapY;

    public int selectedTrackIndex; // Bien de nhap so track trong Inspector
    private List<TrackChunk> midiTracks = new List<TrackChunk>(); // Luu danh sach cac track

    public List<GameNote> orderedNotes = new List<GameNote>();
    private int nextNoteIndex = 0; // Chi so cua not tiep theo can nhan

    public TMP_Text scoreText; // Them Text UI vao GameManager de hien thi diem
    public int totalScore = 0;  // Tong diem cua nguoi choi

    public int starCount = 0; // So sao hien tai
    public TMP_Text starText; // Text UI de hien thi so sao
    private float milestoneDuration; // Thoi luong cua moi moc (1/3 tong thoi luong MIDI)
    private GameNote milestone1Note, milestone2Note, milestone3Note; // Not cuoi cung cua moc tuong ung

    // Cac bien hien co giu nguyen, them bien moi
    public bool isGameOver = false;
    public GameObject gameOverScreen; // Gan UI Game Over trong Inspector

    public GameObject winScreen; // UI Win Screen
    public TMP_Text winScoreText; // Text UI trong WinScreen de hien thi diem

    public TMP_Text songNameText;

    private Coroutine rewindCoroutine;

    void Awake()
    {
        Instance = this; // Gán Instance trong Awake ?? ??m b?o nó ???c kh?i t?o s?m
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Tu dong them AudioSource neu chua co
        }
    }

    void Start()
    {
        // L?y d? li?u t? SongManager
        //if (SongManager.SelectedMidiFile != null)
        //{
        //    midiFile = SongManager.SelectedMidiFile;
        //    trackIndex = SongManager.SelectedTrackIndex;
        //    beatDuration = SongManager.SelectedBeatDuration;

        //    // Gan MP3 vao AudioSource
        //    if (SongManager.SelectedMp3Clip != null)
        //    {
        //        audioSource.clip = SongManager.SelectedMp3Clip;
        //        Debug.Log("MP3 assigned to AudioSource: " + audioSource.clip.name);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("No MP3 file loaded.");
        //    }

        //    Debug.Log("=== Debugging MIDI Data in GameTestScene ===");
        //    Debug.Log($"Loaded MIDI File: {(midiFile != null ? "Yes" : "No")}");
        //    Debug.Log($"Track Index: {trackIndex}");
        //    Debug.Log($"Total Tracks: {midiFile.GetTrackChunks().Count()}");
        //    Debug.Log($"MIDI Duration: {midiFile.GetDuration<MetricTimeSpan>().TotalSeconds} seconds");

        //    LoadMidiData();

        //    // Tat WinScreen
        //    if (winScreen != null)
        //    {
        //        winScreen.SetActive(false);
        //    }
        //}
        //else
        //{
        //    Debug.LogError("No MIDI data found in SongManager.");
        //}

        fileLocation = PlayerPrefs.GetString("SelectedMidiFile", "");
        if (string.IsNullOrEmpty(fileLocation))
        {
            Debug.LogError("File location is empty. Please set a valid MIDI file path in StreamingAssets.");
            return;
        }
        //if (!string.IsNullOrEmpty(mp3FileLocation))
        //{
        //    StartCoroutine(LoadMP3File(mp3FileLocation));
        //}
        else
        {
            ReadFromFile();
            if (winScreen != null)
            {
                winScreen.SetActive(false);
            }
        }

        //if (winScreen != null)
        //{
        //    winScreen.SetActive(false);
        //}

        //if (!string.IsNullOrEmpty(mp3FileLocation))
        //{
        //    StartCoroutine(LoadMP3File(mp3FileLocation));
        //}

        // Tinh toan moc thoi gian
        if (midiFile != null)
        {
            double midiDuration = midiFile.GetDuration<MetricTimeSpan>().TotalMicroseconds / 1_000_000.0;
            milestoneDuration = (float)(midiDuration / 3.0); // Moi moc la 1/3 tong thoi luong
            Debug.Log($"Milestone Duration: {milestoneDuration}");
            // Tim not cuoi cung cua tung moc
            FindMilestoneNotes(midiDuration);
        }

        // Khoi tao UI sao
        UpdateStarUI();
        //StartCoroutine(LoadSongDataFromFirebase());
    }

    //private IEnumerator LoadSongDataFromFirebase()
    //{
    //    // Wait until Firebase is initialized
    //    yield return new WaitUntil(() => FireBaseManager.Instance.IsFirebaseInitialized());

    //    // Now safe to call GetAllSongsAsync
    //    Task<Dictionary<string, Song>> task = FireBaseManager.Instance.GetAllSongsAsync();
    //    yield return new WaitUntil(() => task.IsCompleted);

    //    if (task.Result != null && task.Result.Count > 0)
    //    {
    //        // Gi? s? b?n có m?t bi?n l?u key c?a bài hát ???c ch?n
    //        string selectedSongKey = "song1"; // Thay b?ng key bài hát ???c ch?n t? UI
    //        if (task.Result.ContainsKey(selectedSongKey))
    //        {
    //            Song selectedSong = task.Result[selectedSongKey];

    //            fileLocation = selectedSong.midi;
    //            mp3FileLocation = selectedSong.mp3;
    //            selectedTrackIndex = selectedSong.track;

    //            if (songNameText != null)
    //            {
    //                songNameText.text = selectedSong.name;
    //            }

    //            if (!string.IsNullOrEmpty(fileLocation))
    //            {
    //                ReadFromFile();
    //            }

    //            if (!string.IsNullOrEmpty(mp3FileLocation))
    //            {
    //                StartCoroutine(LoadMP3File(mp3FileLocation));
    //            }

    //            if (midiFile != null)
    //            {
    //                double midiDuration = midiFile.GetDuration<MetricTimeSpan>().TotalSeconds;
    //                milestoneDuration = (float)(midiDuration / 3.0);
    //                FindMilestoneNotes(midiDuration);
    //            }

    //            UpdateStarUI();
    //        }
    //        else
    //        {
    //            Debug.LogError($"Song with key {selectedSongKey} not found.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("No songs found in Firebase.");
    //    }
    //}

    // Ham moi de nhan du lieu tu FireBaseManager
    //public void SetSongData(string midiFilePath, string mp3FilePath, int trackIndex)
    //{
    //    fileLocation = midiFilePath;
    //    mp3FileLocation = mp3FilePath;
    //    selectedTrackIndex = trackIndex;

    //    Debug.Log($"Received song data: MIDI={fileLocation}, MP3={mp3FileLocation}, Track={selectedTrackIndex}");

    //    // Sau khi nh?n d? li?u, t?i MIDI và MP3
    //    if (!string.IsNullOrEmpty(fileLocation))
    //    {
    //        ReadFromFile();
    //    }
    //    if (!string.IsNullOrEmpty(mp3FileLocation))
    //    {
    //        StartCoroutine(LoadMP3File(mp3FileLocation));
    //    }

    //    if (midiFile != null)
    //    {
    //        double midiDuration = midiFile.GetDuration<MetricTimeSpan>().TotalMicroseconds / 1_000_000.0;
    //        milestoneDuration = (float)(midiDuration / 3.0);
    //        Debug.Log($"Milestone Duration: {milestoneDuration}");
    //        FindMilestoneNotes(midiDuration);
    //    }
    //}

    private void ReadFromFile()
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, fileLocation);
        Debug.Log("Attempting to read MIDI file from: " + fullPath);

        if (File.Exists(fullPath))
        {
            midiFile = MidiFile.Read(fullPath);
            if (midiFile != null)
            {
                PopulateMidiTracks();
                if (midiTracks.Count > 0)
                {
                    ProcessSelectedTrack();
                }
                else
                {
                    Debug.LogError("No valid tracks found in the MIDI file.");
                }
            }
            else
            {
                Debug.LogError("Failed to read MIDI file: " + fullPath);
            }
        }
        else
        {
            Debug.LogError("MIDI file not found at: " + fullPath);
        }
    }

    private void PopulateMidiTracks()
    {
        if (midiFile == null)
        {
            Debug.LogError("Cannot populate MIDI tracks: midiFile is null.");
            return;
        }

        midiTracks.Clear();
        int trackIndex = 0;
        foreach (var track in midiFile.GetTrackChunks())
        {
            if (trackIndex == 0)
            {
                trackIndex++;
                continue; // B? qua track ??u tiên (th??ng là metadata)
            }
            midiTracks.Add(track);
            trackIndex++;
        }
        Debug.Log($"Loaded {midiTracks.Count} tracks from MIDI file.");
    }

    private void ProcessSelectedTrack()
    {
        // Ki?m tra n?u selectedTrackIndex h?p l?
        if (selectedTrackIndex < 0 || selectedTrackIndex >= midiTracks.Count)
        {
            Debug.LogError($"Invalid track index: {selectedTrackIndex}. Valid range is 0 to {midiTracks.Count - 1}. Defaulting to track 0.");
            selectedTrackIndex = 0; // M?c ??nh ch?n track 0 n?u index không h?p l?
        }

        Debug.Log($"Processing track {selectedTrackIndex}");
        GetDataFromMidi(selectedTrackIndex);
    }

    public void GetDataFromMidi(int selectedTrackIndex)
    {
        if (midiFile == null)
        {
            Debug.LogError("Cannot process MIDI data: midiFile is null.");
            return;
        }

        var tempoMap = midiFile.GetTempoMap();
        TrackChunk selectedTrack = midiTracks[selectedTrackIndex];
        var notes = selectedTrack.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var tempoChange in tempoMap.GetTempoChanges())
        {
            var tempo = tempoChange.Value.BeatsPerMinute;
            var time = tempoChange.Time;
            beatDuration = (float)(60f / tempo);
            Debug.Log($"Tempo: {tempo} BPM, Time: {time}, beatDuration: {beatDuration}s");
        }

        if (selectedTrackIndex < 0 || selectedTrackIndex >= midiTracks.Count)
        {
            Debug.LogError("Invalid track index!");
            return;
        }

        DistributeNotesToLanes(array);

        orderedNotes.Clear(); // Sap xep tat ca cac not theo thoi gian xuat hien
        foreach (var lane in lanes)
        {
            lane.ClearNotes();
            lane.SetTimeStamps(array);
            lane.SpawnAllNotes();
            orderedNotes.AddRange(lane.notes);
        }

        orderedNotes.Sort((a, b) => a.assignedTime.CompareTo(b.assignedTime));
        nextNoteIndex = 0; // Reset index
        GameMidiTimeLine.Instance.ResetTimeline();
    }

    //void LoadMidiData()
    //{
    //    var trackChunks = midiFile.GetTrackChunks();
    //    Debug.Log($"Track count: {trackChunks.Count()}");
    //    if (trackIndex >= 0 && trackIndex < trackChunks.Count())
    //    {
    //        var selectedTrack = trackChunks.ElementAt(trackIndex);
    //        var notes = selectedTrack.GetNotes();
    //        var noteArray = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
    //        notes.CopyTo(noteArray, 0);
    //        Debug.Log($"Notes in track {trackIndex}: {noteArray.Length}");

    //        DistributeNotesToLanes(noteArray);
    //        Debug.Log($"Lanes count: {lanes.Length}");

    //        orderedNotes.Clear(); // Sap xep tat ca cac not theo thoi gian xuat hien
    //        foreach (var lane in lanes)
    //        {
    //            lane.ClearNotes();
    //            lane.SetTimeStamps(noteArray);
    //            lane.SpawnAllNotes();
    //            orderedNotes.AddRange(lane.notes);
    //            Debug.Log($"Lane {lane.name}: Notes spawned");
    //        }

    //        orderedNotes.Sort((a, b) => a.assignedTime.CompareTo(b.assignedTime));
    //        nextNoteIndex = 0; // Reset index
    //        GameMidiTimeLine.Instance.ResetTimeline();

    //    }
    //    else
    //    {
    //        Debug.LogError($"Invalid track index: {trackIndex}. Total tracks: {trackChunks.Count()}");
    //    }
    //}

    void DistributeNotesToLanes(Melanchall.DryWetMidi.Interaction.Note[] allNotes)
    {
        var noteValues = new HashSet<int>();
        foreach (var note in allNotes)
        {
            noteValues.Add((int)note.NoteName);
        }

        var sortedNotes = new List<int>(noteValues);
        sortedNotes.Sort();

        int laneCount = lanes.Length;
        for (int i = 0; i < sortedNotes.Count; i++)
        {
            int laneIndex = Mathf.Clamp(i * laneCount / sortedNotes.Count, 0, laneCount - 1);
            lanes[laneIndex].noteRestriction = (Melanchall.DryWetMidi.MusicTheory.NoteName)sortedNotes[i];
        }
    }

    public bool CanHitNote(GameNote note)
    {
        if (nextNoteIndex >= orderedNotes.Count)
            return false;

        bool canHit = orderedNotes[nextNoteIndex] == note;
        Debug.Log($"CanHitNote: {canHit} for note {note.name}");
        return canHit;
    }

    public void HitNote(GameNote note)
    {
        if (CanHitNote(note))
        {
            nextNoteIndex++;
            Debug.Log($"?ã nh?n ?úng n?t {note.name}");

            // Kiem tra xem not co phai la not cuoi cua moc hay khong
            if (note == milestone1Note && starCount < 1)
            {
                starCount = 1;
                UpdateStarUI();
                Debug.Log($"Reached 1 star at note time {note.assignedTime}");
            }
            else if (note == milestone2Note && starCount < 2)
            {
                starCount = 2;
                UpdateStarUI();
                Debug.Log($"Reached 2 stars at note time {note.assignedTime}");
            }
            else if (note == milestone3Note && starCount < 3)
            {
                starCount = 3;
                UpdateStarUI();
                Debug.Log($"Reached 3 stars at note time {note.assignedTime}");
            }
        }
    }

    //private IEnumerator LoadMP3File(string mp3Path)
    //{
    //    string fullPath = Path.Combine(Application.streamingAssetsPath, mp3Path);
    //    Debug.Log("Attempting to load MP3 file from: " + fullPath);

    //    using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + fullPath, AudioType.MPEG))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result == UnityWebRequest.Result.Success)
    //        {
    //            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
    //            if (clip != null)
    //            {
    //                audioSource.clip = clip;
    //                Debug.Log("MP3 file loaded successfully: " + clip.name);
    //            }
    //            else
    //            {
    //                Debug.LogError("Failed to load AudioClip from MP3 file.");
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogError("Error loading MP3 file: " + www.error);
    //        }
    //    }
    //}
    public static double GetAudioSourceTime()
    {
        //return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
        if (!Instance.isPlaying) return pausedTime;
        return Time.time - startTime; // Tính th?i gian ?ã trôi qua k? t? khi bài hát b?t ??u
    }

    public static double startTime;

    public void PlaySong()
    {
        if (!isPlaying)
        {
            startTime = Time.time;
            isPlaying = true;
            if (audioSource.clip != null)
            {
                audioSource.Play(); // Phát MP3
                Debug.Log("Playing MP3 from AudioSource.");
            }
            else
            {
                Debug.LogWarning("No MP3 clip assigned to AudioSource.");
            }
        }
    }

    public int CalculateNoteScore(float assignedDuration)
    {
        // Xác ??nh s? l?n 1/2 beatDuration xu?t hi?n trong assignedDuration
        int scoreFactor = Mathf.RoundToInt(assignedDuration / (beatDuration / 2));

        return Mathf.Max(1, scoreFactor); // ??m b?o ít nh?t là 1 ?i?m
    }

    // Cap nhat diem
    public void AddScore(int score)
    {
        totalScore += score;
        UpdateScoreUI();  // Cap nhat Text UI voi diem moi
    }

    // Cap nhat hien thi diem len UI
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = totalScore.ToString();  // Hien thi diem len UI
        }
    }

    //void UpdateStarCount()
    //{
    //    double currentTime = GetAudioSourceTime();
    //    int newStarCount = 0;

    //    if (currentTime >= milestoneDuration)
    //        newStarCount = 1;
    //    if (currentTime >= 2 * milestoneDuration)
    //        newStarCount = 2;
    //    if (currentTime >= 3 * milestoneDuration)
    //        newStarCount = 3;

    //    if (newStarCount != starCount)
    //    {
    //        starCount = newStarCount;
    //        UpdateStarUI();
    //        Debug.Log($"Reached {starCount} star(s) at time {currentTime}");
    //    }
    //}

    void UpdateStarUI()
    {
        if (starText != null)
        {
            starText.text = $"Stars: {starCount}";
        }
    }

    void FindMilestoneNotes(double midiDuration)
    {
        milestone1Note = null;
        milestone2Note = null;
        milestone3Note = null;

        foreach (var note in orderedNotes)
        {
            double noteTime = note.assignedTime;

            // Moc 1: Not cuoi cung truoc hoac tai milestoneDuration
            if (noteTime <= milestoneDuration)
            {
                if (milestone1Note == null || noteTime > milestone1Note.assignedTime)
                {
                    milestone1Note = note;
                }
            }

            // Moc 2: Not cuoi cung truoc hoac tai 2 * milestoneDuration
            if (noteTime <= 2 * milestoneDuration)
            {
                if (milestone2Note == null || noteTime > milestone2Note.assignedTime)
                {
                    milestone2Note = note;
                }
            }

            // Moc 3: Not cuoi cung cua bai
            if (milestone3Note == null || noteTime > milestone3Note.assignedTime)
            {
                milestone3Note = note;
            }
        }

        Debug.Log($"Milestone 1 Note: {(milestone1Note != null ? milestone1Note.assignedTime : -1)}");
        Debug.Log($"Milestone 2 Note: {(milestone2Note != null ? milestone2Note.assignedTime : -1)}");
        Debug.Log($"Milestone 3 Note: {(milestone3Note != null ? milestone3Note.assignedTime : -1)}");
    }

    void Update()
    {
        if (isPlaying && !isGameOver)
        {
            CheckMissedNotes();
            //UpdateStarCount();

            // Kiem tra neu bai MIDI da ket thuc
            if (midiFile != null)
            {
                double midiDuration = midiFile.GetDuration<MetricTimeSpan>().TotalMicroseconds / 1_000_000.0;
                double currentTime = GetAudioSourceTime();
                if (currentTime >= midiDuration - 0.1f && !winScreen.activeSelf) // Buffer nho de tranh loi
                {
                    TriggerWinGame();
                }
            }
        }
    }

    void CheckMissedNotes()
    {
        if (nextNoteIndex >= orderedNotes.Count) return;

        GameNote nextNote = orderedNotes[nextNoteIndex];
        if (nextNote == null) return;

        // Kiem tra xem not co phai la not dai va dang duoc giu hay khong
        GameNote noteGamePlay = nextNote as GameNote;
        if (noteGamePlay != null && noteGamePlay.IsHolding)
        {
            // Neu not dang duoc giu, bo qua kiem tra troi qua khung hinh
            Debug.Log($"Skipping Game Over check for holding note: {nextNote.name}");
            return;
        }

        // Lay vi tri camera và tinh mep duoi
        float cameraY = Camera.main.transform.position.y;
        float cameraHeight = Camera.main.orthographicSize * 2; // Chieu cao khung hinh camera
        float cameraBottomY = cameraY - Camera.main.orthographicSize;

        // Them mot khoang dem (buffer) de not co the troi qua mep duoi mot chut
        float buffer = 3f; // Dieu chinh gia tri nay neu can (don vi la world units)
        cameraBottomY -= buffer;

        // Tinh mep duoi cua not
        float noteBottomY = nextNote.transform.position.y - (nextNote.assignedDuration * nextNote.Zoom / 2);

        // Kiem tra neu not da troi qua khoi camera bottom (co tinh buffer)
        if (noteBottomY < cameraBottomY)
        {
            Debug.Log($"Note missed! NoteBottomY: {noteBottomY}, CameraBottomY: {cameraBottomY}");
            nextNoteIndex++; // Tang nextNoteIndex de chuyen sang not tiep theo
            StartRewindTimeline(nextNote);
            TriggerGameOver(nextNote);
        }
    }

    //public void RewindBeat(Note missedNote)
    //{
    //    if (midiFile == null || missedNote == null) return; // Dam bao MIDI va not hop le

    //    // Lay thoi gian assignedTime cua not bi miss
    //    double noteTime = missedNote.assignedTime;

    //    // Tinh thoi gian beat gan nhat truoc thoi gian cua not
    //    //double previousBeatTime = Math.Floor(noteTime / beatDuration) * beatDuration;
    //    double previousBeatTime = noteTime - beatDuration;

    //    // Dam bao khong xuong duoi 0 giay
    //    previousBeatTime = Mathf.Max(0, (float)previousBeatTime);

    //    // Neu dang dung, cap nhat pausedTime
    //    if (!isPlaying)
    //    {
    //        pausedTime = previousBeatTime;
    //    }
    //    else
    //    {
    //        startTime = Time.time - previousBeatTime; // Cap nhat thoi gian bat dau
    //    }

    //    // Goi cap nhat Timeline
    //    GameMidiTimeLine.Instance.UpdateTimeLinePosition(previousBeatTime);

    //    // Neu audioSource dang chay, tua lai audio theo thoi gian moi
    //    if (audioSource.isPlaying)
    //    {
    //        audioSource.time = (float)previousBeatTime;
    //    }

    //    // Goi ham di chuyen camera den thoi gian moi
    //    GameCamera camera = UnityEngine.Object.FindFirstObjectByType<GameCamera>();
    //    if (camera != null)
    //    {
    //        camera.MoveCameraToMidiTime();
    //    }

    //    Debug.Log($"Rewound to beat at time: {previousBeatTime}");
    //}

    void TriggerGameOver(GameNote missedNote)
    {
        isGameOver = true;
        isPlaying = false;
        audioSource.Pause();

        //RewindBeat(missedNote);

        // Bat dau coroutine de cho vai giay truoc khi hien thi Game Over
        StartCoroutine(ShowGameOverWithDelay(2f)); // Doi 2 giay, ban co the thay doi thoi gian nay
    }

    IEnumerator ShowGameOverWithDelay(float delay)
    {
        Debug.Log($"Missed note detected. Waiting {delay} seconds before Game Over...");
        yield return new WaitForSeconds(delay); // Cho so giay duoc chi dinh

        // Sau khi cho, hien thi man hnh Game Over
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    void TriggerWinGame()
    {
        isGameOver = true;
        isPlaying = false;
        UpdateStarUI();

        if (winScreen != null)
        {
            winScreen.SetActive(true);
            if (winScoreText != null)
            {
                winScoreText.text = "Score: " + totalScore.ToString() + "\nStars: " + starCount.ToString();
            }
            Debug.Log("Win Screen displayed!");
        }
        else
        {
            Debug.LogWarning("Win Screen is not assigned!");
        }
    }

    public void StartRewindTimeline(GameNote missedNote)
    {
        // Neu dang chay thi dung truoc
        if (rewindCoroutine != null)
        {
            StopCoroutine(rewindCoroutine);
        }

        if (isPlaying)
        {
            pausedTime = GetAudioSourceTime();
            isPlaying = false;
            //audioSource.Pause(); // Tam dung am thanh
        }

        // Tinh thoi gian beat gan nhat truoc thoi gian cua not bi miss
        //float targetTime = Mathf.Floor((float)missedNote.assignedTime / beatDuration) * beatDuration;
        float targetTime = missedNote.assignedTime - beatDuration / 2;

        rewindCoroutine = StartCoroutine(RewindTimelineCoroutine(targetTime, 2f));
    }

    private IEnumerator RewindTimelineCoroutine(float targetTime, float speed)
    {
        isPlaying = false; // Dung playback de tranh xung dot

        while (true)
        {
            double currentTime = GetAudioSourceTime();
            double newTime = currentTime - speed * Time.deltaTime;

            // Kiem tra neu da dat den hoac vuot qua targetTime
            if (newTime <= targetTime)
            {
                newTime = targetTime; // Dat chinh xac tai targetTime
                //pausedTime = newTime;
                //GameMidiTimeLine.Instance.UpdateTimeLinePosition(newTime);

                //// Di chuyen camera den thoi gian moi
                //GameCamera cameraa = UnityEngine.Object.FindFirstObjectByType<GameCamera>();
                //if (cameraa != null)
                //{
                //    cameraa.MoveCameraToMidiTime();
                //}

                // Cap nhat lai nextNoteIndex de da bao bat dau tu not hop le
                UpdateNextNoteIndex(targetTime);

            }

            // Cap nhat thoi gian va vi tri timeline
            pausedTime = newTime;
            GameMidiTimeLine.Instance.UpdateTimeLinePosition(newTime);

            // Di chuyen camera den thoi gian moi
            GameCamera camera = UnityEngine.Object.FindFirstObjectByType<GameCamera>();
            if (camera != null)
            {
                camera.MoveCameraToMidiTime();
            }

            yield return null;
        }
    }

    // Ham moi de cap nhat nextNoteIndex dua tren targetTime
    private void UpdateNextNoteIndex(double targetTime)
    {
        nextNoteIndex = 0;
        for (int i = 0; i < orderedNotes.Count; i++)
        {
            if (orderedNotes[i].assignedTime > targetTime)
            {
                break;
            }
            nextNoteIndex = i + 1;
        }
        Debug.Log($"Updated nextNoteIndex to {nextNoteIndex} for targetTime {targetTime}");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
