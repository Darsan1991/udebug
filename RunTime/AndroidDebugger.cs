using UnityEngine;

namespace DGames.DDebug
{
    
        [CreateAssetMenu(menuName = "Debugger/Android Debugger")]

    public class AndroidDebugger : Debugger
    {
        protected override void WriteDebug(string message, Object context, params string[] tags)
        {
            ToastMessage.ShowToastOnUiThread(BuildString(message, context, tags));
        }
    }
}