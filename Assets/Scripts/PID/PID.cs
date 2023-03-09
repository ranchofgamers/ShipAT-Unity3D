using System;

namespace Assets.Scripts.PID
{
    [Serializable]

    public class RegulatorPID
    {
        float lastPV, lastD;

        public float Kp, Ki, Kd;
        public float P, I, D;
        public float CO;

        public RegulatorPID()
        {
            Kp = 1f;
            Ki = 0;
            Kd = 0f;
        }

        public RegulatorPID(float p, float i, float d)
        {
            Kp = p;
            Ki = i;
            Kd = d;
        }

        public float Update(float error, float PV, float dt)
        {
            P = error;
            I += error * dt;
            D = -(PV - lastPV) / dt;

            if (PV - lastPV < -10 || PV - lastPV > 10) D = lastD;

            lastD = D;
            lastPV = PV;

            if (I > 30) I = 30;
            if (I < -30) I = -30;

            CO = P * Kp + I * Ki + D * Kd;

            return CO;
        }
    }
}