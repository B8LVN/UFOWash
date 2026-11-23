using UnityEngine;
using UnityEngine.InputSystem;

namespace UFOWASH
{
    public class UfoWashHUD : MonoBehaviour
    {
        public static int Credits;
        public static bool JobComplete;
        UfoWashSprayTool tool;
        void Start(){tool=FindFirstObjectByType<UfoWashSprayTool>();}
        void Update(){if(Keyboard.current!=null && Keyboard.current.rKey.wasPressedThisFrame) UfoWashDirtSurface.ResetJob();}
        void OnGUI()
        {
            float cleanliness=1f-UfoWashDirtSurface.TotalDirt();
            GUI.color=new Color(0.04f,0.07f,0.1f,0.88f);
            GUI.Box(new Rect(18,18,330,138),"");
            GUI.color=Color.white;
            GUI.Label(new Rect(32,30,300,30),"<b>UFO WASH // SERVICE BAY 01</b>");
            GUI.Label(new Rect(32,58,300,24),"CREDITS: "+Credits);
            GUI.Label(new Rect(32,82,300,24),"JOB CLEANLINESS: "+Mathf.RoundToInt(cleanliness*100)+"%");
            GUI.Box(new Rect(32,108,290,18),"");
            GUI.color=new Color(0.2f,0.85f,1f);
            GUI.Box(new Rect(34,110,286*cleanliness,14),"");
            GUI.color=Color.white;
            GUI.Label(new Rect(32,160,420,25),JobComplete?"JOB COMPLETE  +"+UfoWashDirtSurface.RewardCredits+" CREDITS":"WASHTOOL: HOLD LEFT MOUSE TO SPRAY");
            GUI.Label(new Rect(32,184,420,25),"WASD MOVE  |  MOUSE LOOK  |  ESC UNLOCK  |  R RESET");
            if(tool!=null && tool.spraying) GUI.Label(new Rect(Screen.width/2-35,Screen.height/2+24,100,20),"SPRAYING");
            GUI.Label(new Rect(Screen.width/2-10,Screen.height/2-10,20,20),"+");
        }
    }
}