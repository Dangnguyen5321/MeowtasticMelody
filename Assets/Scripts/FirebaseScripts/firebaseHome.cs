using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using TMPro;
public class firebaseHome : MonoBehaviour
{

    private DatabaseReference databaseReference;
    // Tham chiếu đến các UI elements để hiển thị username, stars, coins, level
    public TextMeshProUGUI usernameText; // Text để hiển thị username (Đăng Nhập)
    public TextMeshProUGUI levelText;    // Text để hiển thị level (LV. ?)
    public TextMeshProUGUI starsText;    // Text để hiển thị stars (⭐ ?)
    public TextMeshProUGUI coinsText;    // Text để hiển thị coins (💰 ?)
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                // Kết nối thành công
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                // FetchSongData();
                DataHome(); // Gọi hàm để lấy dữ liệu từ node "home"
            }
            else
            {
                Debug.LogError("Không thể kết nối Firebase: " + task.Exception);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DataHome()
    {
        // Lấy dữ liệu từ node "home"
        databaseReference.Child("home").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Lỗi khi lấy dữ liệu từ node 'home': " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Lấy dữ liệu từ snapshot
                string username = snapshot.Child("username").Value.ToString();
                long stars = (long)snapshot.Child("stars").Value;
                long coins = (long)snapshot.Child("coins").Value;
                long level = (long)snapshot.Child("level").Value;

                // Cập nhật UI
                if (usernameText != null)
                {
                    usernameText.text = username; // Hiển thị username (TẰNG NHẠP)
                }
                if (levelText != null)
                {
                    levelText.text = "LV." + level.ToString(); // Hiển thị level (LV.0)
                }
                if (starsText != null)
                {
                    //starsText.text = "⭐ " + stars.ToString(); // Hiển thị stars (⭐ 9)
                    starsText.text =  stars.ToString(); // Hiển thị stars (⭐ 9)
                }
                if (coinsText != null)  
                {
                    //coinsText.text = "💰 " + coins.ToString() + "+"; // Hiển thị coins (💰 430+)
                    coinsText.text = coins.ToString() + "+"; // Hiển thị coins (💰 430+)
                }

                Debug.Log($"Home Data - Username: {username}, Stars: {stars}, Coins: {coins}, Level: {level}");
            }
        });
    }
}
