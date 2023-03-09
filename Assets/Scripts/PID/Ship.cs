using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PID
{

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]

    public class Ship : MonoBehaviour
    {

        Rigidbody2D rb2d;
        Transform myTransform;

        public Sliders _sliderP;
        public Sliders _sliderI;
        public Sliders _sliderD;

        public Sliders _sliderWindForceGain;
        public Sliders _sliderConstantError;

        public Text _textAmplitude;
        public Text _textCourse;
        public Text _textAngleError;

        public Toggle _toggleRandomWind;

        public static float AngleShip { get; set; }
        public static float SetAngle { get; set; }
        public static float AngleError { get; set; }
        public static float SetConstantError { get; set; }
        public static float Torque { get; set; }

        public static float P { get; set; }
        public static float I { get; set; }
        public static float D { get; set; }
        public static float ControllerOutput { get; set; }
        public static float WindForce { get; set; }
        public static float WindForceGain { get; set; }

        public float _thrust = 2f;

        float windTimer = 9.38f;
        float windGust = 0f;
        float randomWind = 0f;
        int windDirection = 1;

        string ANGLE_ERROR_NAME = "Angle Error";

        float ANGLE_ERROR_VALUE;
        float COURSE;
        float AMPLITUDE;

        public RegulatorPID _angleController = new RegulatorPID(0.0f, 0f, 0.0f);

        void Awake()
        {
            Init();
        }
        void FixedUpdate()
        {
            AddTorque();
            AddWind();
        }
        void Update()
        {
            _angleController.Kp = _sliderP.Value;
            _angleController.Ki = _sliderI.Value;
            _angleController.Kd = _sliderD.Value;

            P = _angleController.P * _angleController.Kp;
            I = _angleController.I * _angleController.Ki;
            D = _angleController.D * _angleController.Kd;

            SetConstantError = -_sliderConstantError.Value;
            WindForceGain = _sliderWindForceGain.Value;

            if (AMPLITUDE != GraphBuilder.GetMaxValue(ANGLE_ERROR_NAME))
                _textAmplitude.text = GraphBuilder.GetMaxValue(ANGLE_ERROR_NAME).ToString("00.0");
            if (COURSE != 360 - AngleShip)
                _textCourse.text = (360 - AngleShip).ToString("00.0");
            if (ANGLE_ERROR_VALUE != AngleError)
                _textAngleError.text = AngleError.ToString("00.0");

            AMPLITUDE = GraphBuilder.GetMaxValue(ANGLE_ERROR_NAME);
            COURSE = 360 - AngleError;
            ANGLE_ERROR_VALUE = AngleError;
        }

        void Init()
        {
            myTransform = transform;
            WindForceGain = 0f;

            rb2d = GetComponent<Rigidbody2D>();
        } //Инициализация

        void AddTorque()
        {
            Torque = ControlRotate();
            rb2d.AddTorque(Torque);

        } //Регулирующее воздействие
        void AddWind()
        {
            randomWind = 0f;
            if (windTimer < 9.37f)
            {
                windGust = windDirection * WindForceGain * (float)((1.7569967 * (Mathf.Pow(10, -4))) * Mathf.Pow(windTimer, 8) - 0.007028 * Mathf.Pow(windTimer, 7) + 0.1143144 * Mathf.Pow(windTimer, 6) - 0.9696358 * Mathf.Pow(windTimer, 5) + 4.5674437 * Mathf.Pow(windTimer, 4) - 11.7360836 * Mathf.Pow(windTimer, 3) + 14.7625776 * Mathf.Pow(windTimer, 2) - 5.5501612 * windTimer + 0.0016725);
            }
            else windGust = 0f;

            windTimer += Time.deltaTime;

            if (_toggleRandomWind.isOn)
            {
                randomWind = 5f * WindForceGain * (Mathf.PerlinNoise(Time.time / 5f, Time.time / 5f) - 0.5f);
            }

            WindForce = SetConstantError + windGust + randomWind;

            rb2d.AddTorque(WindForce);

        } //Добавление ветра

        public void GustWest()
        {
            if (windTimer >= 9.37)
            {
                windTimer = 0f;
                windDirection = -1;
            }

        }
        public void GustOst()
        {
            if (windTimer >= 9.37)
            {
                windTimer = 0f;
                windDirection = 1;
            }
        }

        float ControlRotate()
        {
            float CO = 0f;
            float MV = 0f;
            float dt = Time.fixedDeltaTime;

            AngleShip = myTransform.eulerAngles.z;

            //Контроллер угла поворота
            AngleError = Mathf.DeltaAngle(AngleShip, SetAngle);
            float torqueCorrectionForAngle = _angleController.Update(AngleError, AngleShip, dt);

            //Выход контроллера
            CO = torqueCorrectionForAngle;

            //Округление до #.##
            CO = Mathf.Round(CO * 100.0f) / 100.0f;

            //Процесс сатурации
            MV = CO;

            ControllerOutput = CO;

            if (CO > _thrust) MV = _thrust;
            if (CO < -_thrust) MV = -_thrust;

            return MV;

        } //Расчет регулирующего воздействия

    }
}
