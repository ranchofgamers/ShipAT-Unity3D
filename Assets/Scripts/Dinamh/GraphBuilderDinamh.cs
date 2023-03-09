using UnityEngine;


public class GraphBuilderDinamh : MonoBehaviour
{
    Material material;

    Vector3 LeftButtonAngleWP;
    Vector3 LeftUpperAngleWP;
    Vector3 RightButtonAngleWP;
    Vector3 RightUpperAngleWP;

    float width;
    float height;

    Vector2 center;

    public float gridStep = 8f;

    public float test = 0f;

    void Update()
    {
        LeftButtonAngleWP = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 1f));
        LeftUpperAngleWP = Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelHeight, 1f));
        RightButtonAngleWP = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0f, 1f));
        RightUpperAngleWP = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 1f));

        width = RightButtonAngleWP.x - LeftButtonAngleWP.x;
        height = LeftUpperAngleWP.y - LeftButtonAngleWP.y;

        center = new Vector2(LeftButtonAngleWP.x, LeftButtonAngleWP.y + height / 2);
    }
    private void Awake()
    {
        material = Resources.Load("Lines") as Material;

    }
    void OnRenderObject()
    {
        DrawGreyBG();
        DrawGrid();
        DrawXLine();
        DrawGraph();
    }

    void DrawGreyBG()
    {
        material.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        material.SetPass(0);

        GL.Begin(GL.QUADS);

        GL.Vertex3(LeftButtonAngleWP.x, LeftButtonAngleWP.y, LeftButtonAngleWP.z + 1);
        GL.Vertex3(LeftUpperAngleWP.x, LeftUpperAngleWP.y, LeftUpperAngleWP.z + 1);
        GL.Vertex3(RightUpperAngleWP.x, RightUpperAngleWP.y, RightUpperAngleWP.z + 1);
        GL.Vertex3(RightButtonAngleWP.x, RightButtonAngleWP.y, RightButtonAngleWP.z + 1);

        GL.End();
    } //Отрисовка серого фона
    void DrawGrid()
    {
        material.color = new Color(255, 255, 255, 0.3f);
        material.SetPass(0);

        float stepW = width / gridStep;
        float stepH = height / gridStep;


        GL.Begin(GL.LINES);

        for (int i = 0; i < gridStep; i++)
        {
            Plot(LeftButtonAngleWP.x + stepW * i, LeftButtonAngleWP.y, LeftButtonAngleWP.x + stepW * i, LeftUpperAngleWP.y);
            Plot(LeftButtonAngleWP.x, LeftButtonAngleWP.y + stepH * i, RightButtonAngleWP.x, LeftButtonAngleWP.y + stepH * i);
        }

        GL.End();
    } //Отрисовка сетки
    void DrawXLine()
    {
        material.color = Color.white;
        material.SetPass(0);

        GL.Begin(GL.LINES);

        Plot(LeftButtonAngleWP.x, LeftButtonAngleWP.y + height / 2, RightButtonAngleWP.x, LeftButtonAngleWP.y + height / 2);

        GL.End();
    } //Отрисовка нуля (абсцисса)
    void DrawGraph()
    {
        float density = 3000f;
        float step = width / density;

        material.color = Color.red;
        material.SetPass(0);

        GL.Begin(GL.LINES);

        for (int i = 1; i < density; ++i)
        {
            Plot(center.x + step * (i - 1), 0, center.x + step * i, 0);
        }

        GL.End();
    }

    void smoothing()
    {

    }

    void Plot(float x0, float y0, float x1, float y1)
    {
        GL.Vertex3(x0, y0, LeftButtonAngleWP.z);
        GL.Vertex3(x1, y1, LeftButtonAngleWP.z);
    } //Построение Vertex3
}
