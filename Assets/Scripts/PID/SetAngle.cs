using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.PID;

public class SetAngle : MonoBehaviour
{
    public Text setAngletText;

    public float SetAngleOnPanel { get; set; }

    float setAngleDelta = 40f;
    float setAngle;
    float minSetAngle = 0;
    float maxSetAngle = 360f;

    bool down = false;
    bool up = true;

    bool negative = false;
    bool positive = false;

    public void Positive()
    {
        positive = true;
    }
    public void Negative()
    {
        negative = true;
    }
    public void AllUp()
    {
        negative = false;
        positive = false;
    }
    public void SetButtonClick()
    {
        Ship.SetAngle = -setAngle;
    }
    void Update()
    {
        if (negative == true & up == false & down == true)
        {
            if (setAngle < minSetAngle)
                setAngle = maxSetAngle;
            else setAngle += - setAngleDelta * Time.deltaTime;
        }
        if (positive == true & up == false & down == true)
        {
            if (setAngle > maxSetAngle)
                setAngle = minSetAngle;
            else setAngle += setAngleDelta * Time.deltaTime;
        }

        if(setAngle != SetAngleOnPanel)
            setAngletText.text = setAngle.ToString("000.0");

        SetAngleOnPanel = setAngle;
    }

    public void SetDown()
    {
        up = !up;
        down = !down;
    }

    public void SetUp()
    {
        up = !up;
        down = !down;
    }
}
