using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace GameSystem
{
    public class Checkpoint : MonoBehaviour
    {
        private bool enabled;
        private bool textEnabled;
        private float timer = 0.0f;
        [SerializeField] private Sprite enabledSprite;
        [SerializeField] public TextMeshProUGUI CheckpointText;

        void Start()
        {
            CheckpointText.enabled = false;
        }

        private void Update()
        {
            if (enabled && textEnabled)
            {
                timer += Time.deltaTime;
                if (timer > 2.0f)
                {
                    CheckpointText.enabled = false;
                    textEnabled = false;
                }
            }
        }
    
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !enabled)
            {
                enabled = true;
                textEnabled = true;
                CheckpointText.enabled = true;
                Indestructibles.respawnPos = transform.position;
                GetComponent<SpriteRenderer>().sprite = enabledSprite;
                GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
    }
}