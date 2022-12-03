using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BridgeController : MonoBehaviour
{

    public static BridgeController inst;
    public int bridgeStage = 0;
    [SerializeField] private GameObject endScreen;

    public void AddStage() {
        bridgeStage++;
        if (bridgeStage >= 2) {
            endScreen.SetActive(true);
        }
    }

    private void Awake()
    {

        if (inst != null && inst != this)
        {
            Destroy(this);
        }
        else
        {
            inst = this;
        }
    }

    public void GoMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
