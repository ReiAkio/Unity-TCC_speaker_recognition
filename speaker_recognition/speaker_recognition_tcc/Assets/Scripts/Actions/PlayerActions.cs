using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public float speed = 5.0f;
    private bool moveToTarget = false;
    public Transform targetTransform;
    private Animator playerAnimator;
    private Animator targetAnimator;

    // Novos sprites para cada objeto
    public Sprite tvNewSprite; 
    public Sprite doorNewSprite; 
    public Sprite windowNewSprite;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();

        if (moveToTarget && targetTransform != null)
        {
            Vector2 direction = (targetTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            if (Vector2.Distance(transform.position, targetTransform.position) < 0.1f)
            {
                moveToTarget = false;
                SetMoveDirection(false, false);  // Paramos o movimento
                ExecuteTargetInteraction();
            }
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            targetTransform = GameObject.FindGameObjectWithTag("Tv")?.transform;
            InitiateMovement();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            targetTransform = GameObject.FindGameObjectWithTag("Door")?.transform;
            InitiateMovement();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            targetTransform = GameObject.FindGameObjectWithTag("Window")?.transform;
            InitiateMovement();
        }
    }

    void InitiateMovement()
    {
        if (targetTransform != null)
        {
            moveToTarget = true;

            // Determine a direção baseada na posição X do jogador e do objeto alvo
            float direction = targetTransform.position.x - transform.position.x;

            if (direction > 0)
            {
                SetMoveDirection(true, false);  // Movendo para direita
            }
            else if (direction < 0)
            {
                SetMoveDirection(false, true);  // Movendo para esquerda
            }
        }
    }

    void SetMoveDirection(bool moveRight, bool moveLeft)
    {
        Debug.Log($"Setting MoveRight to {moveRight} and MoveLeft to {moveLeft}");
        playerAnimator.SetBool("MoveRight", moveRight);
        playerAnimator.SetBool("MoveLeft", moveLeft);
    }

    void ExecuteTargetInteraction()
    {
        if (targetTransform == null) return;

        targetAnimator = targetTransform.GetComponent<Animator>();
        if (targetAnimator != null)
        {
            targetAnimator.SetTrigger("Activate");
        }
        
        SpriteRenderer sr = targetTransform.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            switch (targetTransform.tag)
            {
                case "Tv":
                    sr.sprite = tvNewSprite;
                    break;
                case "Door":
                    sr.sprite = doorNewSprite;
                    break;
                case "Window":
                    sr.sprite = windowNewSprite;
                    break;
            }
        }
    }
}
