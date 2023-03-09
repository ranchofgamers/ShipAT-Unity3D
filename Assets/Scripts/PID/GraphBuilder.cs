using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Managers;

namespace Assets.Scripts.PID
{
    public class GraphBuilder : MonoBehaviour
    {
        public static float Size { get; set; }

        public bool AddInf { get; set; }

        static int maxDataPoints = 1000;
        static float step = 8f;
        static float width = 20f;
        static float height = 8f;

        static Material material;

        static Dictionary<string, GraphData> graphs;

        static Rect rectangle;

        static float mousePosX;
        static float mousePosY;
        static float compression;
        static float zoom; 
        
        static float timer;
        static int counter;

        static float CURRENT_RANGE;

        static string firstGraph = null;

        static bool drawMouseLine = false;

        static GUIStyle style1;
        static GUIStyle style2;

        class GraphData
        {
            public string Name { get; set; }
          
            public Color Color { get; set; }
        
            public float MaxValue { get; set; }
            public float CurrentMousePoint { get; set; }

            public List<float> dataPoints;

            public GraphData()
            {
                dataPoints = new List<float>();
            }
        }

        private void Awake()
        {
            style1 = new GUIStyle
            {
                alignment = TextAnchor.LowerLeft,
                fontStyle = FontStyle.Bold
            };
            style1.normal.textColor = Color.yellow;
            style1.fontSize = 17;

            style2 = new GUIStyle
            {
                alignment = TextAnchor.LowerRight
            };
            style2.normal.textColor = new Color(1, 1, 1, 0.5f);

            zoom = 0.3f;
            Size = 0.8f;

            AddInf = true;
        }
        void Update()
        {
            timer += Time.deltaTime;
            Zoom();

            rectangle = new Rect(Manager.LeftButtonAngleWP.x + 0.1f, Manager.LeftButtonAngleWP.y + 0.1f, width * Size, height * Size);

        }
        void OnGUI()
        {
            mousePosX = Event.current.mousePosition.x;
            mousePosY = Event.current.mousePosition.y;

            if (AddInf)
                CreateSupportInformation();
        }

        static void Init()
        {
            material = Resources.Load("Lines") as Material;

            if (graphs == null)
            {
                graphs = new Dictionary<string, GraphData>();

            }

        } //Инициализация словаря

        public static void CreateGraph(string name, float argument, Color color, bool visible)
        {
            if (firstGraph == null)
                firstGraph = name;

            if (graphs == null)
                Init();

            if (firstGraph == name)
                CreateFrame();

            if (graphs.ContainsKey(name) == false)
                AddGraph(name, color);

            if (graphs[name] != null)
                DrawGraph(name, argument, visible);

        } //Создание графика    
        public static float GetMaxValue(string name)
        {
            float x = 0;

            if (graphs != null && graphs.ContainsKey(name) == true)
                x = graphs[name].MaxValue;
            return x;
        } //Получение амплитудного значения графика за 30 сек

        static void CreateFrame()
        {
            DrawGreyBG();
            DrawGrid();
            DrawRectangle();
            DrawXLine();
            DrawMouseLine();

        } //Создание рамки для графиков  
        static void CreateSupportInformation()
        {
            int dx = 0;
            float dw = 70f;

            if (graphs != null && graphs.Count != 0 && drawMouseLine)
            {
                GUI.Box(new Rect(mousePosX, mousePosY, dw * graphs.Count, 30), "");

                foreach (string name in graphs.Keys)
                {
                    if (graphs[name].dataPoints.Count == maxDataPoints)
                    {
                        style1.normal.textColor = graphs[name].Color;
                        GUI.TextField(new Rect(mousePosX + 10 + (dw * dx), mousePosY + 25, 0, 0), graphs[name].CurrentMousePoint.ToString("00.00"), style1);
                    }
                    else
                    {
                        style1.normal.textColor = graphs[name].Color;
                        GUI.TextField(new Rect(mousePosX + 10 + (dw * dx), mousePosY + 25, 0, 0), "WAIT", style1);
                    }
                    dx++;
                }
            }

            //todo: оптимизировать
            float posY = Manager.LeftUpperAngleWP.y - rectangle.height / 2;
            float posX = rectangle.x + rectangle.width - Size * 0.4f;
            float step = 4;

            float y1 = (CURRENT_RANGE / posY / zoom);

            if (y1 == Mathf.Infinity | y1 == -Mathf.Infinity)
                y1 = 0;

            float y2 = -y1;
            float y3 = y1 / 2;
            float y4 = y2 / 2;
            float y5 = 3 * y1 / 2;
            float y6 = 3 * y2 / 2;
          
            Vector2 posVector1 = Camera.main.WorldToScreenPoint(new Vector2(posX, posY - rectangle.height / step));
            Vector2 posVector3 = Camera.main.WorldToScreenPoint(new Vector2(posX, posY - rectangle.height / step / 2));
            Vector2 posVector5 = Camera.main.WorldToScreenPoint(new Vector2(posX, posY - (3 * rectangle.height / step) / 2));

            Vector2 posVector2 = Camera.main.WorldToScreenPoint(new Vector2(posX, posY + rectangle.height / step));          
            Vector2 posVector4 = Camera.main.WorldToScreenPoint(new Vector2(posX, posY + rectangle.height / step / 2));         
            Vector2 posVector6 = Camera.main.WorldToScreenPoint(new Vector2(posX, posY + (3 * rectangle.height / step) / 2));



            Vector2 size = new Vector2(0, 0);

            style2.fontSize = (int)(15 * Size);

            GUI.TextField(new Rect(posVector1, new Vector2(0, 0)), y1.ToString("00"), style2);
            GUI.TextField(new Rect(posVector2, new Vector2(0, 0)), y2.ToString("00"), style2);
            GUI.TextField(new Rect(posVector3, new Vector2(0, 0)), y3.ToString("00"), style2);
            GUI.TextField(new Rect(posVector4, new Vector2(0, 0)), y4.ToString("00"), style2);
            GUI.TextField(new Rect(posVector5, new Vector2(0, 0)), y5.ToString("00"), style2);
            GUI.TextField(new Rect(posVector6, new Vector2(0, 0)), y6.ToString("00"), style2);

        } //Создание GUI отображения значений точек и линий на графике

        static void AddGraph(string name, Color color)
        {
            GraphData inst_graphs = new GraphData();

            graphs.Add(name, inst_graphs);
            graphs[name].Name = name;
            graphs[name].Color = color;

            if (graphs[name].dataPoints.Count < maxDataPoints)
            {
                for (int i = 0; i < maxDataPoints; i++)
                {
                    graphs[name].dataPoints.Add(0);
                }
            }


        } //Добавление графика в словарь 

        static void Zoom()
        {
            if (Manager.MouseScrollWheel > 0 & zoom < 3.0f)
                zoom += 0.03f;

            if (Manager.MouseScrollWheel < 0 & zoom > 0.1f)
            {
                zoom -= 0.03f;
            }
        }

        static float GetX(int index, float offset)
        { 
            return index * rectangle.width / maxDataPoints + offset + rectangle.x;
        } //Получение координаты X для точки

        static float GetY(float value)
        {
            return compression * value + rectangle.y + rectangle.height / 2;
        } //Получение координаты Y для точки

        static void DrawGraph(string name, float value, bool visible)
        {

            if (Manager.Pause == false)
            {                
                if (counter >= graphs.Count)
                {
                    timer = 0f;
                    counter = 0;
                }                   

                if (timer >= 0.03f)
                {
                    counter++;
                    graphs[name].dataPoints.Add(value);

                    if (graphs[name].dataPoints.Count > maxDataPoints)
                        graphs[name].dataPoints.RemoveAt(0);                
                }
            }

            float max = float.MinValue;
            float min = float.MaxValue;
            float currentRange = 0f;

            float screenOffset = graphs[name].dataPoints.Count != maxDataPoints ? (rectangle.width - graphs[name].dataPoints.Count * rectangle.width / maxDataPoints) : 0.0f;

            material.color = graphs[name].Color;
            material.SetPass(0);

            if (visible)
            {
                GL.Begin(GL.LINES);

                for (int i = 1; i < graphs[name].dataPoints.Count; ++i)
                {
                    float POINT = graphs[name].dataPoints[i];

                    if (POINT > max)
                        max = POINT;

                    if (POINT < min)
                        min = POINT;

                    float y0 = GetY(graphs[name].dataPoints[i - 1]);
                    float y1 = GetY(POINT);

                    if (y0 >= rectangle.y + rectangle.height)
                        y0 = rectangle.y + rectangle.height;

                    if (y1 >= rectangle.y + rectangle.height)
                        y1 = rectangle.y + rectangle.height;

                    if (y0 <= rectangle.y)
                        y0 = rectangle.y;

                    if (y1 <= rectangle.y)
                        y1 = rectangle.y;

                    Plot(GetX(i - 1, screenOffset), y0, GetX(i, screenOffset), y1);
                }

                GL.End();

                graphs[name].MaxValue = Mathf.Max(Mathf.Abs(min), Mathf.Abs(max));

                foreach (KeyValuePair<string, GraphData> graph in graphs)
                {
                    if (graphs[graph.Key].MaxValue > currentRange)
                    {
                        currentRange = graphs[graph.Key].MaxValue;
                        CURRENT_RANGE = currentRange;
                    }
                }

                if (currentRange != 0)
                {
                    compression = zoom * rectangle.height / currentRange;
                }
            }


        } //Отрисовка графика
        static void DrawMouseLine()
        {
            material.color = new Color(255, 255, 255, 1f);
            material.SetPass(0);

            Vector3 LinePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePosX, Screen.height - mousePosY));

            Vector3 p1 = Camera.main.WorldToScreenPoint(new Vector3(rectangle.x, rectangle.y));
            Vector3 p2 = Camera.main.WorldToScreenPoint(new Vector3(rectangle.xMax, rectangle.yMax));

            p1.y = Screen.height - p1.y;
            p2.y = Screen.height - p2.y;

            if (mousePosX > p1.x & mousePosX < p2.x & mousePosY < p1.y & mousePosY > p2.y)
            {
                Cursor.visible = false;

                drawMouseLine = true;

                GL.Begin(GL.LINES);

                Plot(LinePos.x, rectangle.y, LinePos.x, rectangle.yMax); //Vertical

                GL.End();

                CurrentMousePoint(LinePos);
            }
            else
            {
                Cursor.visible = true;
                drawMouseLine = false;
            }
        } //Отрисовка линии при наведении мышки
        static void DrawRectangle()
        {
            material.color = Color.white;
            material.SetPass(0);

            GL.Begin(GL.LINES);

            Plot(rectangle.x, rectangle.y, rectangle.xMax, rectangle.y);
            Plot(rectangle.x, rectangle.y, rectangle.x, rectangle.yMax);
            Plot(rectangle.x, rectangle.yMax, rectangle.xMax, rectangle.yMax);
            Plot(rectangle.xMax, rectangle.yMax, rectangle.xMax, rectangle.y);

            GL.End();
        } //Отрисовка границы графика
        static void DrawXLine()
        {
            material.color = Color.white;
            material.SetPass(0);

            GL.Begin(GL.LINES);

            Plot(rectangle.x, rectangle.y + rectangle.height / 2, rectangle.xMax, rectangle.y + rectangle.height / 2);

            GL.End();
        } //Отрисовка нуля (абсцисса)
        static void DrawGreyBG()
        {
            material.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            material.SetPass(0);

            GL.Begin(GL.QUADS);

            GL.Vertex3(rectangle.x, rectangle.y, Manager.LeftButtonAngleWP.z + 3);
            GL.Vertex3(rectangle.xMax, rectangle.y, Manager.LeftButtonAngleWP.z + 3);
            GL.Vertex3(rectangle.xMax, rectangle.yMax, Manager.LeftButtonAngleWP.z + 3);
            GL.Vertex3(rectangle.x, rectangle.yMax, Manager.LeftButtonAngleWP.z + 3);

            GL.End();
        } //Отрисовка серого фона
        static void DrawGrid()
        {
            material.color = new Color(255, 255, 255, 0.3f);
            material.SetPass(0);

            float stepW = rectangle.width / step;
            float stepH = rectangle.height / step;


            GL.Begin(GL.LINES);

            for (int i = 0; i < step; i++)
            {
                Plot(rectangle.x + stepW * i, rectangle.y, rectangle.x + stepW * i, rectangle.yMax);
                Plot(rectangle.x, rectangle.y + stepH * i, rectangle.xMax, rectangle.y + stepH * i);
            }

            GL.End();
        } //Отрисовка сетки

        static void CurrentMousePoint(Vector3 LinePos)
        {
            if (graphs != null && graphs.Count != 0 && drawMouseLine)
            {
                foreach (string name in graphs.Keys)
                {
                    if (graphs[name].dataPoints.Count == maxDataPoints)
                    {
                        graphs[name].CurrentMousePoint = graphs[name].dataPoints[Mathf.RoundToInt((LinePos.x - rectangle.x) * maxDataPoints / rectangle.width) - 1];
                    }
                }
            }
        } //Получение информации о текущей точки на графике соответствующей позиции мышки

        static void Plot(float x0, float y0, float x1, float y1)
        {
            GL.Vertex3(x0, y0, Manager.LeftButtonAngleWP.z + 5);
            GL.Vertex3(x1, y1, Manager.LeftButtonAngleWP.z + 5);
        } //Построение Vertex3

        public void ClearGraphs()
        {
            foreach (KeyValuePair<string, GraphData> graph in graphs)
            {
                graph.Value.dataPoints.Clear();
                for (int i = 0; i < maxDataPoints; i++)
                {
                    graph.Value.dataPoints.Add(0);
                }

            }
        }
    }
}
