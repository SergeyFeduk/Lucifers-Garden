using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float resistanceDuration;
    [SerializeField] List<Image> heartRenderers = new List<Image>();
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite deadHeartSprite;

    private int hp = 2;
    private Timer resistanceTimer = new Timer();
    private bool resistanceOn = false;

    public void Damage() {
        if (resistanceOn) return;
        hp--;
        heartRenderers[hp - 1].sprite = deadHeartSprite;
        if (hp <= 0) {
            //Death screen
            return;
        }
        StartCoroutine(ResistanceResetRoutine());
    }

    public void Heal() {
        hp++;
        heartRenderers[hp - 1].sprite = fullHeartSprite;
    }

    private IEnumerator ResistanceResetRoutine() {
        resistanceOn = true;
        resistanceTimer = new Timer(resistanceDuration);
        while (!resistanceTimer.ExecuteTimer()) {
            yield return null;
        }
        resistanceOn = false;
    }
}
