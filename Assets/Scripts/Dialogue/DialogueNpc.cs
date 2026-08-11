using UnityEngine;
using Interfaces;

namespace Dialogue
{
    public class DialogueNpc : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueData dialogue;

        [SerializeField] private string interactionHint = "Нажмите E, чтобы поговорить";
        
        public string InteractionHint => interactionHint;

        public void Interact(Player.Player player)
        {
            DialogueUI dialogueUI = DialogueUI.Instance;
            if (!dialogueUI)
            {
                return;
            }
            
            dialogueUI.StartDialogue(dialogue);
        }
    }
}

