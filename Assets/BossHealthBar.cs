using UnityEngine;
using UnityEngine.UI;
public class BossHealthBar : MonoBehaviour
{
    private Slider _slider;
    private float _currentHealthPoints;
    private float _maxHealthPoints;
    void Start()
    {
        _slider = this.GetComponent<Slider>();
    }

    public void UpdateBossHealthBar(int hitPoints)
    {
        _currentHealthPoints = (float)hitPoints;
        if (gameObject.activeSelf)
        {
            UpdateSliderValue();
        }
        else
        {
            InitializeBossHealthBar();
        }
    }

    private void UpdateSliderValue()
    {
        _slider.value = (_currentHealthPoints / _maxHealthPoints);
    }

    private void InitializeBossHealthBar()
    {
        _maxHealthPoints = _currentHealthPoints;
        gameObject.SetActive(true);
    }
}
