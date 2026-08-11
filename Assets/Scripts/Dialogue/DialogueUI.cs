using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        public static DialogueUI Instance {get; private set;}

        [Header("Objects")]
        [SerializeField] private GameObject dialoguePanel;
        
        [Header("Content")]
        [SerializeField] private TMP_Text speakerNameText;

        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private TMP_Text continueHintText;
        [SerializeField] private Image portraitImage;

        private DialogueData _currentDialogue;
        private int _currentLineIndex;
        
        public bool IsOpen =>
            dialoguePanel != null &&
            dialoguePanel.activeSelf;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            dialoguePanel.SetActive(false);
        }

        public void StartDialogue(DialogueData dialogue)
        {
            if (!dialogue ||
                dialogue.Lines == null ||
                dialogue.Lines.Length == 0)
            {
                return;
            }
            
            _currentDialogue = dialogue;
            _currentLineIndex = 0;
            
            dialoguePanel.SetActive(true);
            ShowCurrentLine();
        }

        public void ShowNextLine()
        {
            if (!IsOpen)
            {
                return;
            }
            
            _currentLineIndex++;

            if (_currentLineIndex >= _currentDialogue.Lines.Length)
            {
                CloseDialogue();
                return;
            }

            ShowCurrentLine();
        }

        public void CloseDialogue()
        {
            dialoguePanel.SetActive(false);
            
            _currentDialogue = null;
            _currentLineIndex = 0;
        }

        private void ShowCurrentLine()
        {
            DialogueLine line = _currentDialogue.Lines[_currentLineIndex];
            
            speakerNameText.text = line.SpeakerName;
            dialogueText.text = line.Text;
            
            bool hasPortrait = line.Portrait != null;
            
            portraitImage.gameObject.SetActive(hasPortrait);
            portraitImage.sprite = line.Portrait;
            
            bool isLastLine = _currentLineIndex == _currentDialogue.Lines.Length - 1;
            
            continueHintText.text = isLastLine ? "E - завершить" : "E - продолжить";
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}

