using UnityEngine;
using UnityEngine.UI;

public class ChangeLuminosity : MonoBehaviour
{
    public Light luce;
    [SerializeField] public Slider slider;
    void Start()
    {
        if(luce != null && slider != null)
        {
            slider.value = luce.intensity;
            slider.onValueChanged.AddListener(y);
        }
    }

    void y(float value)
    {
        if (luce != null) luce.intensity = value;
    }

}
