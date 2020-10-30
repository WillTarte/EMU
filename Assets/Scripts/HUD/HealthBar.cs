using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{

    public GameObject fill;
    public int currentHealth = 10;
    public Color red_color, white_color;

    private bool performingAction = false;

    // Start is called before the first frame update
    void Start()
    {
        if (currentHealth < 10)
        {
            for (int hp = 9; hp >= currentHealth; --hp)
            {
                fill.transform.GetChild(hp).GetComponent<Image>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool XDown = Input.GetKeyDown(KeyCode.X);
        bool CDown = Input.GetKeyDown(KeyCode.C);
        bool VDown = Input.GetKeyDown(KeyCode.V);

        if (XDown && !performingAction) StartCoroutine(LoseHealthPoint());
        if (CDown && !performingAction) StartCoroutine(RecoverAllHealthPoints());
        if (VDown && !performingAction) AddHealthPoint();

    }

    IEnumerator LoseHealthPoint()
    {
        performingAction = true;
        if (currentHealth > 0)
        {
            --currentHealth;
            Image fill_image = fill.transform.GetChild(currentHealth).GetComponent<Image>();
            for (int blinking = 6; blinking > 0; --blinking)
            {
                if (fill_image.enabled)
                {
                    fill_image.enabled = false;
                }
                else fill_image.enabled = true;

                yield return new WaitForSeconds(0.25f);

            }

            fill_image.enabled = false;
        }
        performingAction = false;
    }
    
    void AddHealthPoint()
    {
        performingAction = true;
        if (currentHealth < 10)
        {
            Image fill_image = fill.transform.GetChild(currentHealth).GetComponent<Image>();
            fill_image.enabled = true;
            ++currentHealth;
        }
        performingAction = false;
    }
    
    IEnumerator RecoverAllHealthPoints()
    {
        performingAction = true;
        if (currentHealth < 10)
        {
            while(currentHealth < 10)
            {
                fill.transform.GetChild(currentHealth).GetComponent<Image>().enabled = true;
                ++currentHealth;
                yield return new WaitForSeconds(0.25f);
            }
        }
        performingAction = false;
    }
}
