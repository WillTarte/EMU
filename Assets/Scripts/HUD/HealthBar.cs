using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class HealthBar : MonoBehaviour
    {
        //Fill contains the game objects that we use in health bar to display the hit points of the player.
        public GameObject fill;
        public int currentHitPoints = 10;

        //These are used to maintain a smooth animation in the health bar in case of quick hit points modifications.
        private int _newHitPoints = 10;
        private bool _performingAnimation = false;
        
        //Making sure that health bar displays the right amount of hit points when initialized.
        void Start()
        {
            if (currentHitPoints < 10)
            {
                for (int healthBarIndex = 9; healthBarIndex >= currentHitPoints; --healthBarIndex)
                {
                    fill.transform.GetChild(healthBarIndex).GetComponent<Image>().enabled = false;
                }
            }
        }

        //Remove or add fill objects in the health bar according to the player's hit points using animation.
        public void UpdateHealthBar(int newHitPoints)
        {
            _newHitPoints = newHitPoints;
            if(!_performingAnimation) StartCoroutine(AnimationUpdateHealthPoints());
        }

        private IEnumerator AnimationUpdateHealthPoints()
        {
            _performingAnimation = true;
                while (_newHitPoints != currentHitPoints)
                {
                    if (currentHitPoints > _newHitPoints)
                    {
                        fill.transform.GetChild(currentHitPoints - 1).GetComponent<Image>().enabled = false;
                        --currentHitPoints;
                        yield return new WaitForSeconds(0.15f);
                    }
                    else if (currentHitPoints < _newHitPoints)
                    {
                        fill.transform.GetChild(currentHitPoints).GetComponent<Image>().enabled = true;
                        ++currentHitPoints;
                        yield return new WaitForSeconds(0.15f);
                    }
                }
            _performingAnimation = false;
        }

        //Remove or add fill objects in the health bar according to the player's hit points directly.
        public void ResetHealthBar(int value)
        {
            int hitPointsDiff = value - currentHitPoints;

            if (hitPointsDiff > 0)
            {
                while (currentHitPoints < value)
                {
                    fill.transform.GetChild(currentHitPoints).GetComponent<Image>().enabled = true;
                    ++currentHitPoints;
                }
            }
            else if (hitPointsDiff < 0)
            {
                while (currentHitPoints > value)
                {
                    --currentHitPoints;
                    fill.transform.GetChild(currentHitPoints).GetComponent<Image>().enabled = false;
                }
            }
        }
    }
}