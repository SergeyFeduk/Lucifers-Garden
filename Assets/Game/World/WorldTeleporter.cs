using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldTeleporter : MonoBehaviour, IInteractable
{
    [SerializeField] private string actionStringValue;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private UnityEvent eventOnInteraction;
    [SerializeField] private float animationDuration;

    private bool delayRejection = false;
    public string actionString { get; set; }
    public void Interact() {
        if (delayRejection) return;
        eventOnInteraction.Invoke();
        StartCoroutine(TeleportationRoutine());
    }

    private IEnumerator TeleportationRoutine() {
        delayRejection = true;
        Timer timer = new Timer(animationDuration);
        while (!timer.ExecuteTimer()) {
            //Animation goes here
            yield return null;
        }
        Player.inst.transform.position = targetTransform.position;
        Camera.main.transform.position = targetTransform.position;
        delayRejection = false;
    }

    private void Start()
    {
        actionString = actionStringValue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.inst.interactor.AddInteraction(this);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.inst.interactor.RemoveInteraction(this);
        }
    }
}
