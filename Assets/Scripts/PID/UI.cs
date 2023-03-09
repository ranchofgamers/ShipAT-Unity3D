using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Managers;


namespace Assets.Scripts.PID
{
    public class UI : MonoBehaviour
    {
        public Image _pauseIcon;

        float screenWidth;
        float screenHeight;

        void Update()
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
        }
        void OnGUI()
        {
            if (GUI.Button(new Rect(screenWidth - 110, 20, 100, 50), "Выход"))
            {
                Application.Quit();
            }


            if (GUI.Button(new Rect(screenWidth - 110, 90, 100, 50), "Пауза"))
            {
                if (Manager.Pause == false)
                {
                    _pauseIcon.enabled = true;
                    Manager.Pause = true;
                    Time.timeScale = 0;                 
                }
                else
                {
                    _pauseIcon.enabled = false;
                    Manager.Pause = false;
                    Time.timeScale = 1;
                }
            }
        }
    }
}


