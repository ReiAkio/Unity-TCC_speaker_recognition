using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public float speed = 5.0f;
    private bool moveToTarget = false;
    public Transform targetTransform;

    public Sprite tvNewSprite;
    public Sprite doorNewSprite;
    public Sprite windowNewSprite;

    private PetAnimationManager petAnimManager;

    private void Awake()
    {
        petAnimManager = GetComponent<PetAnimationManager>();
    }

    void Update()
    {
        if (petAnimManager.CanPlayerAct())
        {
            HandleInput();

            if (moveToTarget && targetTransform != null)
            {
                Vector2 direction = (targetTransform.position - transform.position).normalized;
                transform.position += (Vector3)direction * speed * Time.deltaTime;

                if (Vector2.Distance(transform.position, targetTransform.position) < 0.1f)
                {
                    moveToTarget = false;
                    petAnimManager.StopWalking();
                    ExecuteTargetInteraction();
                }
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
        else if (Input.GetKeyDown(KeyCode.D))
        {
            targetTransform = GameObject.FindGameObjectWithTag("LightBulb")?.transform;
            InitiateMovement();
        }
    }

    void InitiateMovement()
    {
        if (targetTransform != null)
        {
            moveToTarget = true;

            float direction = targetTransform.position.x - transform.position.x;

            if (direction > 0)
            {
                petAnimManager.WalkRight();
            }
            else if (direction < 0)
            {
                petAnimManager.WalkLeft();
            }
        }
    }

    void ExecuteTargetInteraction()
        {
            if (targetTransform == null) return;

            float direction = targetTransform.position.x - transform.position.x;

            if (direction > 0)
            {
                petAnimManager.ExecuteOkRight();
            }
            else if (direction < 0)
            {
                petAnimManager.ExecuteOkLeft();
            }

            // Logic for changing the sprite of the target (already written in your previous code)
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
