
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
namespace DialogueSystem
{
    public class Dialogue : MonoBehaviour
    {
        public bool finished { get; private set; }
        protected IEnumerator WriteText(string input, TextMeshProUGUI textHolder, float delay)
        {
            finished = false;
            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(1.5f);
            finished = true;
        }
    }
}