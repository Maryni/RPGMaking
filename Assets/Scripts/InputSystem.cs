using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    private Inventory inventory;

    private void Start()
    {
        if (inventory == null)
        {
            inventory = GetComponent<Inventory>();
        }
    }

    private void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            inventory.ChangeActiveState();
        }
    }
}
