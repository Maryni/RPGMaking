using System;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    private IControllable controllable;

    private void Awake()
    {
        controllable = GetComponent<IControllable>();
        if (controllable == null)
        {
            throw new Exception($"No IControllable component on object {gameObject.name}");
        }
    }

    private void Update()
    {
        ReadMove();
        ReadJump();
    }

    private void ReadMove()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var directional = new Vector3(horizontal, 0f, vertical);
        
        controllable.Move(directional);
    }

    private void ReadJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            controllable.Jump();
        }
    }
}