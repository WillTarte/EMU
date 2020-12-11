using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DialogueSystem
{
    using TMPro;
    
    public class DialogueLine : MonoBehaviour
    {
        private TextMeshProUGUI textHolder;
        private Image imageHolder;
        [Header ("Text")]
        [SerializeField] private string input;
        [SerializeField] private float delay;
        [Header ("Image")]
        [SerializeField] private Sprite sprite;
        
        private void Awake()
        {
            imageHolder = gameObject.transform.parent.parent.GetChild(0).GetComponent<Image>();
            textHolder = GetComponent<TextMeshProUGUI>();
            textHolder.text = "";
        }
        
        public IEnumerator WriteText()
        {
            imageHolder.sprite = sprite;
            imageHolder.preserveAspect = true;
            foreach (var c in input)
            {
                textHolder.text += c;
                
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

}