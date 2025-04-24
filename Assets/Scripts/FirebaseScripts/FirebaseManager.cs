// using UnityEngine;
// using Firebase;
// using Firebase.Database;
// using Firebase.Extensions;
// using UnityEngine.UI; // Để hiển thị trên giao diện
// using TMPro; // Thêm namespace này

// public class FirebaseManager : MonoBehaviour
// {
//     private DatabaseReference databaseReference;
//     // public Text songNameText; // Kéo thả Text UI từ Inspector vào đây
//     public TextMeshProUGUI songNameText; // Thay đổi từ Text thành TextMeshProUGUI

//     void Start()
//     {
//         // Khởi tạo Firebase
//         FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
//         {
//             if (task.IsCompleted)
//             {
//                 // Kết nối thành công
//                 databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
//                 FetchSongData();
//             }
//             else
//             {
//                 Debug.LogError("Không thể kết nối Firebase: " + task.Exception);
//             }
//         });
//     }

//     void FetchSongData()
//     {
//         // Lấy dữ liệu từ node "songs"
//         databaseReference.Child("songs").GetValueAsync().ContinueWithOnMainThread(task =>
//         {
//             if (task.IsFaulted)
//             {
//                 Debug.LogError("Lỗi khi lấy dữ liệu: " + task.Exception);
//             }
//             else if (task.IsCompleted)
//             {
//                 DataSnapshot snapshot = task.Result;

//                 // Duyệt qua từng bài hát trong node "songs"
//                 foreach (DataSnapshot song in snapshot.Children)
//                 {
//                     string songName = song.Child("name").Value.ToString();
//                     Debug.Log("Tên bài hát: " + songName);

//                     // Hiển thị tên bài hát lên giao diện (Text UI)
//                     if (songNameText != null)
//                     {
//                         songNameText.text += songName + "\n"; // Hiển thị từng tên bài hát
//                     }
//                 }
//             }
//         });
//     }
// }


// using UnityEngine;
// using Firebase;
// using Firebase.Database;
// using Firebase.Extensions;
// using UnityEngine.UI; // Để hiển thị trên giao diện
// using TMPro; // Thêm namespace này

// public class FirebaseManager : MonoBehaviour
// {
//     private DatabaseReference databaseReference;

//     // Tham chiếu đến prefab của nút
//     public GameObject buttonPrefab; // Kéo thả prefab của nút từ Inspector vào đây
//     // Tham chiếu đến đối tượng cha (ví dụ: nội dung của ScrollView hoặc một panel)
//     public Transform buttonParent; // Kéo thả đối tượng cha (như nội dung ScrollView) từ Inspector vào đây

//     void Start()
//     {
//         // Khởi tạo Firebase
//         FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
//         {
//             if (task.IsCompleted)
//             {
//                 // Kết nối thành công
//                 databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
//                 FetchSongData();
//             }
//             else
//             {
//                 Debug.LogError("Không thể kết nối Firebase: " + task.Exception);
//             }
//         });
//     }

//     void FetchSongData()
//     {
//         // Lấy dữ liệu từ node "songs"
//         databaseReference.Child("songs").GetValueAsync().ContinueWithOnMainThread(task =>
//         {
//             if (task.IsFaulted)
//             {
//                 Debug.LogError("Lỗi khi lấy dữ liệu: " + task.Exception);
//             }
//             else if (task.IsCompleted)
//             {
//                 DataSnapshot snapshot = task.Result;

//                 // Xóa các nút hiện có (tùy chọn, nếu bạn muốn làm mới danh sách)
//                 foreach (Transform child in buttonParent)
//                 {
//                     Destroy(child.gameObject);
//                 }

//                 // Duyệt qua từng bài hát trong node "songs"
//                 foreach (DataSnapshot song in snapshot.Children)
//                 {
//                     string songName = song.Child("name").Value.ToString();
//                     Debug.Log("Tên bài hát: " + songName);

//                     // Tạo một nút mới từ prefab
//                     GameObject newButton = Instantiate(buttonPrefab, buttonParent);

//                     // Lấy thành phần TextMeshProUGUI từ nút (đảm bảo prefab của nút có TextMeshProUGUI con)
//                     TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
//                     if (buttonText != null)
//                     {
//                         buttonText.text = songName; // Gán tên bài hát cho nút
//                     }
//                     else
//                     {
//                         Debug.LogWarning("Prefab của nút không có TextMeshProUGUI con!");
//                     }

//                     // (Tùy chọn) Gán sự kiện cho nút nếu cần
//                     Button buttonComponent = newButton.GetComponent<Button>();
//                     if (buttonComponent != null)
//                     {
//                         // Ví dụ: Thêm sự kiện click để ghi log tên bài hát
//                         buttonComponent.onClick.AddListener(() => Debug.Log("Đã click: " + songName));
//                     }
//                 }
//             }
//         });
//     }

//     void datahome()
//     {

//     }
// }


//using UnityEngine;
//using Firebase;
//using Firebase.Database;
//using Firebase.Extensions;
//using UnityEngine.UI;
//using TMPro;
//using UnityEngine.SceneManagement;
//using System.Collections.Generic; // Thêm để dùng Dictionary

//public class FirebaseManager : MonoBehaviour
//{
//    private DatabaseReference databaseReference;
//    public GameObject buttonPrefab;
//    public Transform buttonParent;

//    void Start()
//    {
//        // Khởi tạo Firebase
//        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
//        {
//            if (task.IsCompleted)
//            {
//                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
//                FetchSongData();
//            }
//            else
//            {
//                Debug.LogError("Không thể kết nối Firebase: " + task.Exception);
//            }
//        });
//    }

//    void FetchSongData()
//    {
//        // Lấy dữ liệu từ node "songs"
//        databaseReference.Child("songs").GetValueAsync().ContinueWithOnMainThread(task =>
//        {
//            if (task.IsFaulted)
//            {
//                Debug.LogError("Lỗi khi lấy dữ liệu: " + task.Exception);
//                ShowErrorUI("Không thể tải danh sách bài hát!");
//            }
//            else if (task.IsCompleted)
//            {
//                DataSnapshot snapshot = task.Result;

//                // Kiểm tra nếu không có bài hát
//                if (snapshot.ChildrenCount == 0)
//                {
//                    Debug.LogWarning("Không tìm thấy bài hát nào trong Firebase!");
//                    ShowErrorUI("Danh sách bài hát trống!");
//                    return;
//                }

//                // Xóa các nút hiện có
//                foreach (Transform child in buttonParent)
//                {
//                    Destroy(child.gameObject);
//                }

//                // Duyệt qua từng bài hát
//                foreach (DataSnapshot song in snapshot.Children)
//                {
//                    string songName = song.Child("name").Value?.ToString();
//                    string midiFile = song.Child("midi").Value?.ToString();

//                    // Kiểm tra dữ liệu hợp lệ
//                    if (string.IsNullOrEmpty(songName) || string.IsNullOrEmpty(midiFile))
//                    {
//                        Debug.LogWarning($"Dữ liệu bài hát không hợp lệ: {song.Key}");
//                        continue;
//                    }

//                    Debug.Log($"Tên bài hát: {songName}, MIDI: {midiFile}");

//                    // Tạo nút mới từ prefab
//                    GameObject newButton = Instantiate(buttonPrefab, buttonParent);
//                    TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
//                    if (buttonText != null)
//                    {
//                        buttonText.text = songName;
//                    }
//                    else
//                    {
//                        Debug.LogWarning("Prefab của nút không có TextMeshProUGUI con!");
//                    }

//                    // Gán sự kiện click
//                    Button buttonComponent = newButton.GetComponent<Button>();
//                    if (buttonComponent != null)
//                    {
//                        buttonComponent.onClick.AddListener(() => OnSongButtonClicked(songName, midiFile));
//                    }
//                }
//            }
//        });
//    }

//    void OnSongButtonClicked(string songName, string midiFile)
//    {
//        // Lưu thông tin bài hát để sử dụng trong scene chơi nhạc
//        PlayerPrefs.SetString("SelectedSongName", songName);
//        PlayerPrefs.SetString("SelectedMidiFile", midiFile);
//        PlayerPrefs.Save(); // Đảm bảo dữ liệu được lưu

//        Debug.Log($"Chọn bài hát: {songName}, MIDI: {midiFile}. Chuyển sang GameScene.");

//        // Chuyển sang scene chơi nhạc duy nhất
//        SceneManager.LoadScene("GameScene");
//    }

//    // Hiển thị thông báo lỗi trên UI
//    private void ShowErrorUI(string message)
//    {
//        Debug.LogWarning($"Thông báo lỗi cho người chơi: {message}");
//        // TODO: Tạo một TextMeshProUGUI hoặc panel để hiển thị lỗi
//    }
//}

using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    private DatabaseReference databaseReference;
    public GameObject buttonPrefab; // Prefab của nút bài hát
    public Transform buttonParent; // Parent chứa các nút trong tab "Chung" (ScrollViewChung/Content)
    public Transform buttonParentYeuthich; // Parent chứa các nút trong tab "Yêu thích" (ScrollViewYeuthich/Content)
    public GameObject scrollViewChung; // ScrollView cho tab "Chung"
    public GameObject scrollViewYeuthich; // ScrollView cho tab "Yêu thích"
    public Button tabChungButton; // Nút để chuyển sang tab "Chung"
    public Button tabYeuthichButton; // Nút để chuyển sang tab "Yêu thích"

    // Quản lý sprite của trái tim
    public Sprite chuatimSprite; // Sprite trái tim trắng
    public Sprite timSprite;     // Sprite trái tim đỏ

    void Start()
    {
        // Kiểm tra và khởi tạo Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                FetchSongData(); // Lấy danh sách bài hát từ Firebase cho tab "Chung"

                // Gán sự kiện cho các nút tab
                tabChungButton.onClick.AddListener(() => ShowTab("Chung"));
                tabYeuthichButton.onClick.AddListener(() => ShowTab("Yeuthich"));

                // Hiển thị tab "Chung" mặc định
                ShowTab("Chung");
            }
            else
            {
                Debug.LogError("Không thể kết nối Firebase: " + task.Exception);
                ShowErrorUI("Không thể kết nối với Firebase!");
            }
        });
    }

    void ShowTab(string tabName)
    {
        if (tabName == "Chung")
        {
            scrollViewChung.SetActive(true);
            scrollViewYeuthich.SetActive(false);
            FetchSongData(); // Tải lại danh sách bài hát cho tab "Chung"
        }
        else if (tabName == "Yeuthich")
        {
            scrollViewChung.SetActive(false);
            scrollViewYeuthich.SetActive(true);
            FetchFavoriteSongs(); // Tải danh sách bài hát yêu thích cho tab "Yêu thích"
        }
    }

    void FetchSongData()
    {
        // Lấy dữ liệu từ node "songs" trên Firebase
        databaseReference.Child("songs").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Lỗi khi lấy dữ liệu từ node 'songs': " + task.Exception);
                ShowErrorUI("Không thể tải danh sách bài hát!");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.ChildrenCount == 0)
                {
                    Debug.LogWarning("Không tìm thấy bài hát nào trong Firebase!");
                    ShowErrorUI("Danh sách bài hát trống!");
                    return;
                }

                // Xóa các nút cũ trong tab "Chung"
                foreach (Transform child in buttonParent)
                {
                    Destroy(child.gameObject);
                }

                // Duyệt qua từng bài hát trong node "songs"
                foreach (DataSnapshot song in snapshot.Children)
                {
                    string songKey = song.Key;
                    string songName = song.Child("name").Value?.ToString();
                    string midiFile = song.Child("midi").Value?.ToString();
                    string mp3File = song.Child("mp3").Value?.ToString();
                    long track = (long)song.Child("track").Value;
                    long id = (long)song.Child("id").Value;
                    long score = (long)song.Child("score").Value;
                    bool yeuthich = (bool)song.Child("yeuthich").Value;

                    if (string.IsNullOrEmpty(songName) || string.IsNullOrEmpty(midiFile) || string.IsNullOrEmpty(mp3File))
                    {
                        Debug.LogWarning($"Dữ liệu bài hát không hợp lệ: {songKey}");
                        continue;
                    }

                    Debug.Log($"Tạo nút cho bài hát - Tên: {songName}, MIDI: {midiFile}, MP3: {mp3File}, Yêu thích: {yeuthich}");

                    // Tạo nút mới từ prefab
                    GameObject newButton = Instantiate(buttonPrefab, buttonParent);

                    // Gán tên bài hát vào TextMeshProUGUI
                    TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText != null)
                    {
                        buttonText.text = songName;
                    }
                    else
                    {
                        Debug.LogWarning($"Prefab của nút không có TextMeshProUGUI con cho bài hát {songName}!");
                    }

                    // Tìm ButtonTim trong nút vừa tạo
                    Image heartImage = newButton.transform.Find("ButtonTim")?.GetComponent<Image>();
                    if (heartImage != null)
                    {
                        heartImage.sprite = yeuthich ? timSprite : chuatimSprite;

                        HeartButtonState heartState = heartImage.gameObject.GetComponent<HeartButtonState>();
                        if (heartState == null)
                        {
                            heartState = heartImage.gameObject.AddComponent<HeartButtonState>();
                        }
                        heartState.Yeuthich = yeuthich;
                        heartState.SongKey = songKey;

                        Button heartButton = heartImage.GetComponent<Button>();
                        if (heartButton != null)
                        {
                            UpdateHeartButtonListener(heartButton, heartImage, heartState, songName, midiFile, mp3File, id, track, score);
                        }
                        else
                        {
                            Debug.LogWarning($"ButtonTim trong {songName} không có thành phần Button!");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Không tìm thấy ButtonTim trong {songName}! Đảm bảo tên là 'ButtonTim'.");
                    }

                    // Gán sự kiện nhấn cho nút play bài hát
                    Button buttonComponent = newButton.GetComponent<Button>();
                    if (buttonComponent != null)
                    {
                        buttonComponent.onClick.AddListener(() => OnSongButtonClicked(songName, midiFile));
                    }
                    else
                    {
                        Debug.LogWarning($"Nút {songName} không có thành phần Button để play bài hát!");
                    }
                }
            }
        });
    }

    void FetchFavoriteSongs()
    {
        // Lấy dữ liệu từ node "danhsachyeuthich" trên Firebase
        databaseReference.Child("danhsachyeuthich").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Lỗi khi lấy dữ liệu từ node 'danhsachyeuthich': " + task.Exception);
                ShowErrorUI("Không thể tải danh sách yêu thích!");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.ChildrenCount == 0)
                {
                    Debug.LogWarning("Không có bài hát nào trong danh sách yêu thích!");
                    ShowErrorUI("Danh sách yêu thích trống!");
                    return;
                }

                // Xóa các nút cũ trong tab "Yêu thích"
                foreach (Transform child in buttonParentYeuthich)
                {
                    Destroy(child.gameObject);
                }

                // Duyệt qua từng bài hát trong node "danhsachyeuthich"
                foreach (DataSnapshot song in snapshot.Children)
                {
                    string songKey = song.Key;
                    string songName = song.Child("name").Value?.ToString();
                    string midiFile = song.Child("midi").Value?.ToString();
                    string mp3File = song.Child("mp3").Value?.ToString();
                    long track = (long)song.Child("track").Value;
                    long id = (long)song.Child("id").Value;
                    long score = (long)song.Child("score").Value;
                    bool yeuthich = (bool)song.Child("yeuthich").Value;

                    if (string.IsNullOrEmpty(songName) || string.IsNullOrEmpty(midiFile) || string.IsNullOrEmpty(mp3File))
                    {
                        Debug.LogWarning($"Dữ liệu bài hát yêu thích không hợp lệ: {songKey}");
                        continue;
                    }

                    Debug.Log($"Tạo nút cho bài hát yêu thích - Tên: {songName}, MIDI: {midiFile}, MP3: {mp3File}, Yêu thích: {yeuthich}");

                    // Tạo nút mới từ prefab
                    GameObject newButton = Instantiate(buttonPrefab, buttonParentYeuthich);

                    // Gán tên bài hát vào TextMeshProUGUI
                    TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText != null)
                    {
                        buttonText.text = songName;
                    }
                    else
                    {
                        Debug.LogWarning($"Prefab của nút không có TextMeshProUGUI con cho bài hát {songName}!");
                    }

                    // Tìm ButtonTim trong nút vừa tạo
                    Image heartImage = newButton.transform.Find("ButtonTim")?.GetComponent<Image>();
                    if (heartImage != null)
                    {
                        heartImage.sprite = yeuthich ? timSprite : chuatimSprite;

                        HeartButtonState heartState = heartImage.gameObject.GetComponent<HeartButtonState>();
                        if (heartState == null)
                        {
                            heartState = heartImage.gameObject.AddComponent<HeartButtonState>();
                        }
                        heartState.Yeuthich = yeuthich;
                        heartState.SongKey = songKey;

                        Button heartButton = heartImage.GetComponent<Button>();
                        if (heartButton != null)
                        {
                            UpdateHeartButtonListener(heartButton, heartImage, heartState, songName, midiFile, mp3File, id, track, score);
                        }
                        else
                        {
                            Debug.LogWarning($"ButtonTim trong {songName} không có thành phần Button!");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Không tìm thấy ButtonTim trong {songName}! Đảm bảo tên là 'ButtonTim'.");
                    }

                    // Gán sự kiện nhấn cho nút play bài hát
                    Button buttonComponent = newButton.GetComponent<Button>();
                    if (buttonComponent != null)
                    {
                        buttonComponent.onClick.AddListener(() => OnSongButtonClicked(songName, midiFile));
                    }
                    else
                    {
                        Debug.LogWarning($"Nút {songName} không có thành phần Button để play bài hát!");
                    }
                }
            }
        });
    }

    void OnSongButtonClicked(string songName, string midiFile)
    {
        PlayerPrefs.SetString("SelectedSongName", songName);
        PlayerPrefs.SetString("SelectedMidiFile", midiFile);
        PlayerPrefs.Save();

        Debug.Log($"Chọn bài hát: {songName}, MIDI: {midiFile}. Chuyển sang scene GameScene.");
        SceneManager.LoadScene("GameScene");
    }

    void UpdateHeartButtonListener(Button heartButton, Image heartImage, HeartButtonState heartState, string songName, string midiFile, string mp3File, long id, long track, long score)
    {
        heartButton.onClick.RemoveAllListeners();

        heartButton.onClick.AddListener(() =>
        {
            heartState.Yeuthich = !heartState.Yeuthich;
            heartImage.sprite = heartState.Yeuthich ? timSprite : chuatimSprite;

            databaseReference.Child("songs").Child(heartState.SongKey).Child("yeuthich").SetValueAsync(heartState.Yeuthich).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"Lỗi khi cập nhật trạng thái yêu thích trong node 'songs' cho {heartState.SongKey}: {task.Exception}");
                }
                else
                {
                    Debug.Log($"Đã cập nhật trạng thái yêu thích trong node 'songs' cho {heartState.SongKey}: {heartState.Yeuthich}");
                }
            });

            if (heartState.Yeuthich)
            {
                var songData = new Dictionary<string, object>
                {
                    { "id", id },
                    { "key", heartState.SongKey },
                    { "midi", midiFile },
                    { "mp3", mp3File },
                    { "name", songName },
                    { "score", score },
                    { "track", track },
                    { "yeuthich", true }
                };

                databaseReference.Child("danhsachyeuthich").Child(heartState.SongKey).SetValueAsync(songData).ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError($"Lỗi khi thêm {songName} vào danh sách yêu thích: {task.Exception}");
                    }
                    else
                    {
                        Debug.Log($"Đã thêm {songName} vào danh sách yêu thích trên Firebase!");
                        // Cập nhật lại tab "Yêu thích" nếu đang hiển thị
                        if (scrollViewYeuthich.activeSelf)
                        {
                            FetchFavoriteSongs();
                        }
                    }
                });
            }
            else
            {
                databaseReference.Child("danhsachyeuthich").Child(heartState.SongKey).RemoveValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError($"Lỗi khi xóa {songName} khỏi danh sách yêu thích: {task.Exception}");
                    }
                    else
                    {
                        Debug.Log($"Đã xóa {songName} khỏi danh sách yêu thích trên Firebase!");
                        // Cập nhật lại tab "Yêu thích" nếu đang hiển thị
                        if (scrollViewYeuthich.activeSelf)
                        {
                            FetchFavoriteSongs();
                        }
                    }
                });
            }

            // Cập nhật lại tab "Chung" nếu đang hiển thị
            if (scrollViewChung.activeSelf)
            {
                FetchSongData();
            }

            UpdateHeartButtonListener(heartButton, heartImage, heartState, songName, midiFile, mp3File, id, track, score);
        });
    }

    private void ShowErrorUI(string message)
    {
        Debug.LogWarning($"Thông báo lỗi cho người chơi: {message}");
    }
}