using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public Bluetooth PanelCentral;
    public static void SetupAndroidTheme(string label = null)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
            label = label??Application.productName;
            AndroidJavaObject activity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject> ("currentActivity");
            activity.Call ("runOnUiThread", new AndroidJavaRunnable (() => {
                AndroidJavaClass layoutParamsClass = new AndroidJavaClass ("android.view.WindowManager$LayoutParams");
                int flagFullscreen = layoutParamsClass.GetStatic<int> ("FLAG_FULLSCREEN");
                int flagNotFullscreen = layoutParamsClass.GetStatic<int> ("FLAG_FORCE_NOT_FULLSCREEN");
                int flagDrawsSystemBarBackgrounds = layoutParamsClass.GetStatic<int> ("FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS");
                AndroidJavaObject windowObject = activity.Call<AndroidJavaObject> ("getWindow");
                windowObject.Call ("clearFlags", flagFullscreen);
                windowObject.Call ("addFlags", flagNotFullscreen);
                windowObject.Call ("addFlags", flagDrawsSystemBarBackgrounds);
                int sdkInt = new AndroidJavaClass ("android.os.Build$VERSION").GetStatic<int> ("SDK_INT");
                int lollipop = 21;
                if (sdkInt > lollipop) {
                   // windowObject.Call ("setStatusBarColor", darkARGB);
                    string myName = activity.Call<string> ("getPackageName");
                    AndroidJavaObject packageManager = activity.Call<AndroidJavaObject> ("getPackageManager");
                    AndroidJavaObject drawable = packageManager.Call<AndroidJavaObject> ("getApplicationIcon", myName);
                  //  AndroidJavaObject taskDescription = new AndroidJavaObject ("android.app.ActivityManager$TaskDescription", label, drawable.Call<AndroidJavaObject> ("getBitmap"), primaryARGB);
                  //  activity.Call ("setTaskDescription", taskDescription);
                }
            }));
#endif
    }
    public static int ToARGB(Color color){
            Color32 c = (Color32)color;
            byte[] b = new byte[]{ c.b, c.g, c.r,c.a };
            return System.BitConverter.ToInt32 (b, 0);      
        }
        
    void Start () {
        PanelCentral.Initialize();

        SetupAndroidTheme("RED");
    }
	
	void Update () {
		
	}
}
