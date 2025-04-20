using System;
using UnityEngine;

namespace Script.Player
{
    public class Player : MonoBehaviour
    {
        private Rigidbody2D rb;
    
        public  float   speed;
        private float   inputX;
        private float   inputY;
        private Vector2 movementInput;
    
        private Animator[] animators;
        private bool       isMoving;
        private bool       inputDisable ;
        private void Awake()
        {
            rb        = GetComponent<Rigidbody2D>();
            animators = GetComponentsInChildren<Animator>();
        }

        private void OnEnable()
        {
            EventHandler.BeforeTransitionEvent += OnBeforTransitionEvent;
            EventHandler.AfterTransitionEvent  += OnAfterTransitionEvent;
            EventHandler.MoveToPositionEvent   += OnMoveToPositionEvent;
        }
        
        private void OnDisable()
        {
            EventHandler.BeforeTransitionEvent -= OnBeforTransitionEvent;
            EventHandler.AfterTransitionEvent  -= OnAfterTransitionEvent;
            EventHandler.MoveToPositionEvent   -= OnMoveToPositionEvent;
        }

        private void Update()
        {
            if (!inputDisable) 
            PlayerInput();
            else
            isMoving = false;
            SwitchAnimation();
        }
        private void FixedUpdate()
        {
            if (!inputDisable) 
            Movement();
        }
        private void PlayerInput()
        {
        
        
            inputX = Input.GetAxis("Horizontal");
            inputY = Input.GetAxis("Vertical"); 
            if (inputX != 0 && inputY != 0)
            {
                inputX = inputX * 0.6f;
                inputY = inputY * 0.6f;
            }
            //走路中速度
            if (Input.GetKey(KeyCode.LeftShift))
            {
                inputX = inputX * 0.5f;
                inputY = inputY * 0.5f;
            }

            movementInput = new Vector2(inputX, inputY);
            isMoving      = movementInput != Vector2.zero;
        }
        private void Movement() 
        {
            rb.MovePosition(rb.position+movementInput*speed*Time.deltaTime);
        }

        private void OnMoveToPositionEvent(Vector3 targetPos)
        {
            transform.position = targetPos;
        }

        private void OnAfterTransitionEvent()
        {
            inputDisable = false;
        }

        private void OnBeforTransitionEvent()
        {
            inputDisable = true;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void SwitchAnimation()
        {
            foreach (var anim in animators)
            {
                anim.SetBool("isMoving" , isMoving);
                if (isMoving)
                {
                    anim.SetFloat("InputX" , inputX);
                    anim.SetFloat("InputY" , inputY);
                }

            }
        }
    }
}
