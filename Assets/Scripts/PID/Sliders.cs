using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{

    Slider slider;

    public Text _textValue;

    public float Value { get; set; }

    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    private void Update()
    {
        if (slider.value != Value)
            _textValue.text = slider.value.ToString("00.00");

        Value = slider.value;
    }
}
