using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DialogueSystem
{
    using TMPro;
    
    public class DialogueLine : Dialogue
    {
        private TextMeshProUGUI textHolder;
        [Header ("Text")]
        [SerializeField] private string input;
        [SerializeField] private float delay;
        [Header ("Image")]
        [SerializeField] private Sprite sprite;
        [SerializeField] private Image imageHolder;
        private void Awake()
        {
            textHolder = GetComponent<TextMeshProUGUI>();
            textHolder.text = "";
            
            gameObject.SetActive(false);
        }
        
        private void OnEnable()
        {
            imageHolder.sprite = sprite;
            imageHolder.preserveAspect = true;
            StartCoroutine(WriteText(input, textHolder, delay));
        }
    }

}