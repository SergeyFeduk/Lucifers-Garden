using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private Image interactionImage;
    private List<IInteractable> interactables = new List<IInteractable>();

    public void AddInteraction(IInteractable interactable) {
        interactables.Add(interactable);
        UpdateInteractionText();
    }

    public void RemoveInteraction(IInteractable interactable) {
        if (!interactables.Contains(interactable)) return;
        interactables.Remove(interactable);
        UpdateInteractionText();
    }

    public void UpdateInteractionText() {
        if (interactables.Count == 0 || interactables[0].actionString == "") { 
            interactionText.SetText("");
            SetInteractionImage(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(interactionText.transform.parent as RectTransform);
            return; }
        interactionText.SetText(string.Format("{0}",interactables[0].actionString));
        SetInteractionImage(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(interactionText.transform.parent as RectTransform);
    }

    public void SetInteractionImage(bool transparent) {
        interactionImage.color = transparent ? Color.clear : Color.white;
    }

    void Update()
    {
        if (interactables.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.E)) {
            interactables[0].Interact();
        }
    }
}
