using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PID
{

    [RequireComponent(typeof(Ship))]
    [RequireComponent(typeof(LineRenderer))]


    public class DebugInfo : MonoBehaviour
    {
        LineRenderer courseVector;
        LineRenderer setValueVector;
        LineRenderer torqueVector;

        public Toggle _toggleAngleError;
        public Toggle _toggleWind;
        public Toggle _toggleP;
        public Toggle _toggleI;
        public Toggle _toggleD;
        public Toggle _toggleCO;

        public Slider _graphSize;

        public float _vectorShortening = 1.0f;

        float vectorWidth = 0.05f;

        void Start()
        {

            courseVector = transform.Find("CourseVector").GetComponent<LineRenderer>();
            setValueVector = transform.Find("SetValueVector").GetComponent<LineRenderer>();
            torqueVector = transform.Find("TorqueVector").GetComponent<LineRenderer>();

        }
        void Update()
        {
            DrawDebugVectors();
        }

        void OnRenderObject()
        {
            GraphBuilder.CreateGraph("Angle Error", -Ship.AngleError, Color.yellow, _toggleAngleError.isOn);
            GraphBuilder.CreateGraph("Wind", Ship.WindForce, Color.cyan, _toggleWind.isOn);
            GraphBuilder.CreateGraph("P", Ship.P, Color.red, _toggleP.isOn);
            GraphBuilder.CreateGraph("I", Ship.I, new Color(0.98f, 0.46f, 0.07f, 1), _toggleI.isOn);
            GraphBuilder.CreateGraph("D", Ship.D, Color.green, _toggleD.isOn);
            GraphBuilder.CreateGraph("CO", Ship.ControllerOutput, Color.magenta, _toggleCO.isOn);
        }
        public void ChangeGraphSize()
        {
            GraphBuilder.Size = _graphSize.value;
        }

        void DrawDebugVectors()
        {
            //Заданный вектор поворота
            Vector3 vectorToTarget = transform.position + 30.0f * new Vector3(-Mathf.Sin(Ship.SetAngle * Mathf.Deg2Rad), Mathf.Cos(Ship.SetAngle * Mathf.Deg2Rad), 0f);

            //Текущий вектор ДП
            Vector3 heading = transform.position + 3.5f * transform.up;

            //Вектор угловое ускорения
            Vector3 torque = heading - transform.right * Ship.Torque / _vectorShortening;

            courseVector.SetPosition(0, transform.position);
            courseVector.SetPosition(1, heading);
            courseVector.startWidth = vectorWidth;
            courseVector.endWidth = vectorWidth;

            setValueVector.SetPosition(0, transform.position);
            setValueVector.SetPosition(1, vectorToTarget);
            setValueVector.startWidth = vectorWidth;
            setValueVector.endWidth = vectorWidth;

            torqueVector.SetPosition(0, heading);
            torqueVector.SetPosition(1, torque);
            torqueVector.startWidth = vectorWidth;
            torqueVector.endWidth = vectorWidth;
        } //Отрисовка вспомогательных векторов
    }
}