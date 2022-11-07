using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour, IControllable
{
   [SerializeField] private float speed = 10f;
   [SerializeField] private float gravity = -9.81f;
   [SerializeField] private float jumpHeight = 3f;
   [SerializeField] private Transform groundCheckerPivot;
   [SerializeField] private float checkGroundRadius;
   [SerializeField] private LayerMask groundMask;

   private CharacterController characterController;
   private float velocity;
   private Vector3 moveDirection;
   private bool isGrounded;

   private void Awake()
   {
      characterController = GetComponent<CharacterController>();
   }

   private void FixedUpdate()
   {
      isGrounded = IsOnGround();

      if (isGrounded && velocity < 0)
      {
         velocity = -2;
      }

      MoveInternal();
      DoGravity();
   }

   private bool IsOnGround()
   {
      bool result = Physics.CheckSphere(groundCheckerPivot.position, checkGroundRadius, groundMask);
      return result;
   }

   public void Move(Vector3 direction)
   {
      moveDirection = direction;
   }

   public void Jump()
   {
      if (isGrounded)
      {
         velocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
      }
   }

   private void MoveInternal()
   {
      characterController.Move(moveDirection * speed * Time.fixedDeltaTime);
   }

   private void DoGravity()
   {
      velocity += gravity * Time.fixedDeltaTime;

      characterController.Move(Vector3.up * velocity * Time.fixedDeltaTime);
   }
}
