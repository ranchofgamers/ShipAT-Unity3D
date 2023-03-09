using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PID
{
    public class StopWatch : MonoBehaviour
    {
        public bool TimerActive { get; set; }

        float timer;
        Text text;

        void Start()
        {
            text = gameObject.GetComponentInChildren<Text>();
            TimerActive = false;
        }

        void Update()
        {
            if (TimerActive)
            {
                timer += Time.deltaTime;
                text.text = timer.ToString("00.00");
            }
            if (!TimerActive)
            {
                timer = 0f;
            }
        }

        public void ButtonActive()
        {
            TimerActive = !TimerActive;
        }
    }
}

