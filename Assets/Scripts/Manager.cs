using UnityEngine;
using Assets.Scripts.PID;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class Manager : MonoBehaviour
    {
        Camera Camera;

        public static Vector3 LeftButtonAngleWP { get; set; }
        public static Vector3 LeftUpperAngleWP { get; set; }
        public static Vector3 PositionForSupportWindow { get; set; }

        public static float MouseScrollWheel { get; set; }
        public static bool Pause { get; set; }

        public RectTransform _supportWindow;

        private Vector3 cameraOldPosition;
      
        private void Awake()
        {
            _supportWindow.localPosition = PositionForSupportWindow;
        }
        void Start()
        {
            Application.targetFrameRate = 50;

            Camera = Camera.main;

            cameraOldPosition = Camera.transform.position;
            LeftButtonAngleWP = Camera.ScreenToWorldPoint(new Vector3(0f, 0f, 1f));
            LeftUpperAngleWP = Camera.ScreenToWorldPoint(new Vector3(0f, Camera.pixelHeight, 1f));

            Pause = false;
        }
        void Update()

        {
            MouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");

            if (Camera.transform.position != cameraOldPosition)
            {
                LeftButtonAngleWP = Camera.ScreenToWorldPoint(new Vector3(0f, 0f, 1f));
                LeftUpperAngleWP = Camera.ScreenToWorldPoint(new Vector3(0f, Camera.pixelHeight, 1f));

                cameraOldPosition = Camera.transform.position;
            }
        }
        public void ReloadScenePID()
        {
            PositionForSupportWindow = _supportWindow.localPosition;        
            SceneManager.LoadScene(0);         
            Ship.SetAngle = 0f;

            gameObject.AddComponent<GraphBuilder>().ClearGraphs();
        }
    }
}



