using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu
{
    public class Menu : MonoBehaviour
    {
        public float dx = 400;
        public float dy = 100;
        public float xPos = 2;
        public float yPos = 400;
        public float compression = 25;

        public Texture icon;
        public GUISkin iconSkin;

        public List<string> title;

        public bool iconActive = false;

        private GUIStyle style;

        //todo: ИЗМЕНТЬ НАЧАЛЬНОЕ МЕНЮ
        void Awake()
        {
            style = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            style.normal.textColor = Color.white;
            style.fontSize = 15;
        }
        void Start()
        {
            Application.targetFrameRate = 50;

            title = new List<string>
            {
                "Выбор лабораторной работы",
                "Законы автоматического управления",
                "Типовые динамические звенья",
                "Автоматизация прокладки",
                "Навигационные фильтры",
                "Выход"
            };

        }
        void OnGUI()
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            Rect rectangleIcon = new Rect(screenWidth / 25.0f, screenHeight / 25.0f, 200, 200);
            Rect rectangleBox = new Rect(screenWidth / 2 - xPos, screenHeight / 2 - yPos, dx, 60);

            BuildBoxes(rectangleBox, style, title);

            if (GUI.Button(rectangleIcon, icon, iconSkin.button))
            {
                if (!iconActive)
                {
                    iconActive = true;
                }
                else
                {
                    iconActive = false;
                }
            }

        }

        void BuildBoxes(Rect rectangle, GUIStyle style, List<string> name)
        {
            rectangle.x -= dx / 2;

            for (int i = 0; i < name.Count; i++)
            {
                if (name[i] == "Выбор лабораторной работы")
                {
                    style.fontSize = 20;
                    style.normal.textColor = Color.yellow;
                }
                else
                {
                    style.fontSize = 15;
                    style.normal.textColor = Color.white;
                }
                rectangle.width -= compression;
                rectangle.x += compression / 2;
                rectangle.y += dy;

                if (name[i] == "Выбор лабораторной работы") GUI.Box(rectangle, "");
                if (name[i] == "Законы автоматического управления")
                {
                    if (GUI.Button(rectangle, ""))
                        SceneManager.LoadScene("PID");                
                }
                if (name[i] == "Типовые динамические звенья")
                {
                    GUI.Button(rectangle, "");
                }
                if (name[i] == "Автоматизация прокладки")
                {
                    GUI.Button(rectangle, "");
                }
                if (name[i] == "Навигационные фильтры")
                {
                    GUI.Button(rectangle, "");                        
                }
                if (name[i] == "Выход")
                {
                    if (GUI.Button(rectangle, ""))
                        Application.Quit();
                }
                GUI.Label(rectangle, name[i], style);
            }
        } //Создание кнопок
    }
}
