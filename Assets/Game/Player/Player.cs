using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player inst;
    public PlayerController controller;
    public PlayerAnimator animator;
    public PlayerInventory inventory;
    public PlayerHealth health;
    public PlayerInteractor interactor;

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
}
