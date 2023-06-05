using UnityEngine;

namespace DGames.DDebug
{
    

    public class ToastMessage : Singleton<ToastMessage>
    {
        private string _toastString;
        private string _input;
        private AndroidJavaObject _currentActivity;
        private AndroidJavaClass _unityPlayer;
        private AndroidJavaObject _context;

        private void Start()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                _unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                _currentActivity = _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                _context = _currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            }
        }


        public static void ShowToastOnUiThread(string toastString)
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                return;
            }

            Instance._toastString = toastString;
            Instance._currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(Instance.ShowToast));
        }

        private void ShowToast()
        {
            Debug.Log(this + ": Running on UI thread");

            var t = new AndroidJavaClass("android.widget.Toast");
            var javaString = new AndroidJavaObject("java.lang.String", _toastString);
            var toast = t.CallStatic<AndroidJavaObject>("makeText", _context, javaString,
                t.GetStatic<int>("LENGTH_SHORT"));
            toast.Call("show");
        }
    }
    
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
                DontDestroyOnLoad(gameObject);
                OnAwake();
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnAwake()
        {

        }
    }
}