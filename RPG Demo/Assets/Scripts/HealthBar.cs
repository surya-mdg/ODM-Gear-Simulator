using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void MaxHealth(int max)
    {
        slider.maxValue = max;
        slider.value = max;

        fill.color = gradient.Evaluate(1f);
    }

    public void ReduceHealth(int damage)
    {
        if(slider.value != 0)
        {
            slider.value -= damage;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }     
    }
}
