using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerActions : MonoBehaviour
{
    public float speed = 5.0f;
    private bool moveToTarget = false;
    public Transform targetTransform;
    private string currentSpeaker;
    
    private float lastInteractionTime = 0f;
    public float interactionCooldown = 0.5f;

    public Sprite tvOpenSprite;
    public Sprite tvClosedSprite;
    public Sprite doorOpenSprite;
    public Sprite doorClosedSprite;
    public Sprite windowOpenSprite;
    public Sprite windowClosedSprite;
    private PetAnimationManager petAnimManager;
    public Rigidbody2D rb;
    private RaviAction currentAction = RaviAction.None;

    public enum RaviAction
    {
        None,
        OpenDoor,
        CloseDoor,
        TurnOnTV,
        TurnOffTV,
        OpenWindow,
        CloseWindow,
        TurnOnLight,
        TurnOffLight
        // Add any more actions that you need here
    }
    private void Awake()
    {
        petAnimManager = GetComponent<PetAnimationManager>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    public bool CanPlayerAct()
    {
        return Connection.statusCode == 200;
    }

    void Update()
    {
        //Debug.Log("moveToTarget" + CanPlayerAct());
        if (petAnimManager.CanPlayerAct())
        {   
            if (moveToTarget && targetTransform != null)
            {
                Debug.Log("Target Transform: " + targetTransform.position);
                Debug.Log("Transform: " + transform.position);
                Vector2 direction = (targetTransform.position - transform.position).normalized;

                direction.y = 0; // Ignora a componente 'y' na direção.
                
                // Aplicar força na direção do target.
                rb.AddForce(direction * speed * 100f * Time.deltaTime, ForceMode2D.Force); 
            }
            if (targetTransform != null && Vector2.Distance(transform.position, targetTransform.position) < 0.2f) // or whatever distance is appropriate
            { 
                
                ExecuteTargetInteraction();
                StopMovement();
            }
            else if (moveToTarget && targetTransform == null)
            {
                Debug.Log("MoveToTarget is true, but targetTransform is null.");
            }
        }
    }


    // void HandleInput()
        // {
        //     if (Input.GetKeyDown(KeyCode.A))
        //     {
        //         targetTransform = GameObject.FindGameObjectWithTag("Tv")?.transform;
        //         InitiateMovement();
        //     }
        //     else if (Input.GetKeyDown(KeyCode.B))
        //     {
        //         targetTransform = GameObject.FindGameObjectWithTag("Door")?.transform;
        //         InitiateMovement();
        //     }
        //     else if (Input.GetKeyDown(KeyCode.C))
        //     {
        //         targetTransform = GameObject.FindGameObjectWithTag("Window")?.transform;
        //         InitiateMovement();
        //     }
        //     else if (Input.GetKeyDown(KeyCode.D))
        //     {
        //         targetTransform = GameObject.FindGameObjectWithTag("LightBulb")?.transform;
        //         InitiateMovement();
        //     }
        // }
        //
        public void HandleSpeech(string speech)
        {
            Debug.Log("raviSpeaker" + Connection.raviSpeaker);
            Debug.Log("predictionSpeaker" + Connection.predictionSpeaker);

            if (speech.Contains("oi ravi") && Connection.raviSpeaker == Connection.predictionSpeaker)
            {
                if (speech.Contains("abra") && speech.Contains("porta"))
                {
                    currentAction = RaviAction.OpenDoor;
                    InitiateMovementToObjectWithTag("Door");
                }
                else if (speech.Contains("feche") && speech.Contains("porta"))
                {
                    currentAction = RaviAction.CloseDoor;
                    InitiateMovementToObjectWithTag("Door");
                }

                if (speech.Contains("abra") && speech.Contains("janela"))
                {
                    currentAction = RaviAction.OpenWindow;
                    InitiateMovementToObjectWithTag("Window");
                }
                else if (speech.Contains("feche") && speech.Contains("janela"))
                {
                    currentAction = RaviAction.CloseWindow;
                    InitiateMovementToObjectWithTag("Window");
                }

                if (speech.Contains("ligue") && speech.Contains("tv") && !speech.Contains("des"))
                {
                    currentAction = RaviAction.TurnOnTV;
                    InitiateMovementToObjectWithTag("Tv");
                }
                else if (speech.Contains("desligue") && speech.Contains("tv"))
                {
                    currentAction = RaviAction.TurnOffTV;
                    InitiateMovementToObjectWithTag("Tv");
                }
                if (speech.Contains("ligue") && speech.Contains("luz") && !speech.Contains("des"))
                {
                    currentAction = RaviAction.TurnOnLight;
                    InitiateMovementToObjectWithTag("Light");
                }
                else if (speech.Contains("desligue") && speech.Contains("luz"))
                {
                    currentAction = RaviAction.TurnOffLight;
                    InitiateMovementToObjectWithTag("Light");
                }

            }
        }

        public void StopMovement()
        {
            moveToTarget = false;
            rb.velocity = Vector2.zero; // Parar o personagem completamente
            petAnimManager.StopWalking(); // Parar a animação de caminhada
            
            float direction = targetTransform.position.x - transform.position.x;

            if (direction > 0)
            {
                petAnimManager.ExecuteOkRight();
            }
            else if (direction < 0)
            {
                petAnimManager.ExecuteOkLeft();
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
            else
            {
                Debug.Log("Target transform is null."); // And this
            }
        }
        
        void InitiateMovementToObjectWithTag(string tag)
        {
            GameObject targetObject = GameObject.FindGameObjectWithTag(tag);
            if (targetObject != null)
            {
                targetTransform = targetObject.transform;
                InitiateMovement();

                // Check if the player is already within range
                InteractiveObject interactiveObj = targetObject.GetComponent<InteractiveObject>();
                if (interactiveObj != null)
                {
                    interactiveObj.CheckPlayerInRange(transform);
                }
            }
            else
            {
                Debug.Log("Object with tag " + tag + " not found!");
            }
        }


        public void ExecuteTargetInteraction()
        {
            // Check if enough time has passed since the last interaction
            if (Time.time - lastInteractionTime < interactionCooldown)
            {
                Debug.Log("Still in cooldown. Interaction not allowed.");
                return;
            }

            Debug.Log("Executing interaction with the object");
            if (targetTransform == null) return;

            InteractiveObject interactiveObject = targetTransform.GetComponent<InteractiveObject>();
            SpriteRenderer sr = targetTransform.GetComponent<SpriteRenderer>();
            
            // Check if we can perform the action
            if (interactiveObject != null && sr != null && interactiveObject.CanPerformAction())
            {
                // Toggle the isOpen status
                interactiveObject.isOpen = !interactiveObject.isOpen;

                // Set the sprite based on the new isOpen status
                switch (currentAction)
                {
                    case RaviAction.OpenDoor:
                        sr.sprite = doorOpenSprite;
                        break;
                    case RaviAction.CloseDoor:
                        sr.sprite = doorClosedSprite;
                        break;
                    case RaviAction.TurnOnTV:
                        sr.sprite = tvOpenSprite;
                        break;
                    case RaviAction.TurnOffTV:
                        sr.sprite = tvClosedSprite;
                        break;
                    case RaviAction.OpenWindow:
                        sr.sprite = windowOpenSprite;
                        break;
                    case RaviAction.CloseWindow:
                        sr.sprite = windowClosedSprite;
                        break;
                    case RaviAction.TurnOnLight:
                        GameObject.FindGameObjectWithTag("Light").GetComponent<Light2D>().enabled = true;
                        break;
                    case RaviAction.TurnOffLight:
                        GameObject.FindGameObjectWithTag("Light").GetComponent<Light2D>().enabled = false;
                        break;
                    // ... other cases...
                    default:
                        Debug.Log("No action set or action not recognized.");
                        break;
                }

                lastInteractionTime = Time.time; // Update the last interaction time after a successful interaction
            }
            else
            {
                Debug.Log("InteractiveObject or SpriteRenderer not found on the target object, or the player is not in range.");
            }
        }
    }
