using UnityEngine;

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
                
                Vector2 direction = (targetTransform.position - transform.position).normalized;

                direction.y = 0; // Ignora a componente 'y' na direção.
                
                // Aplicar força na direção do target.
                rb.AddForce(direction * speed * 100f * Time.deltaTime, ForceMode2D.Force); 
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
            
            if (speech.Contains("oi ravi") || speech.Contains("oi davi") || speech.Contains("liga") && speech.Contains("abra") && speech.Contains("porta") && Connection.raviSpeaker == Connection.predictionSpeaker)
            {
                GameObject tvObject = GameObject.FindGameObjectWithTag("Door");
                if (tvObject != null)
                {
                    targetTransform = tvObject.transform;
                    InitiateMovement();
                }
                else
                {
                    Debug.Log("Objeto Porta não encontrado!");
                }
            }
            if (speech.Contains("oi ravi") || speech.Contains("oi davi") || speech.Contains("liga") && speech.Contains("ligue") && speech.Contains("tv") && Connection.raviSpeaker == Connection.predictionSpeaker)
            {
                GameObject tvObject = GameObject.FindGameObjectWithTag("Tv");
                if (tvObject != null)
                {
                    Debug.Log("oi");
                    targetTransform = tvObject.transform;
                    InitiateMovement();
                }
                else
                {
                    Debug.Log("Objeto tv não encontrado!");
                }
            }
            if (speech.Contains("oi ravi") || speech.Contains("oi davi") || speech.Contains("abre") && speech.Contains("abra") && speech.Contains("janela")&& Connection.raviSpeaker == Connection.predictionSpeaker)
            {
                GameObject tvObject = GameObject.FindGameObjectWithTag("Window");
                if (tvObject != null)
                {
                    targetTransform = tvObject.transform;
                    InitiateMovement();
                }
                else
                {
                    Debug.Log("Objeto Window não encontrado!");
                }
            }
            
        }
        
        public void StopMovement()
        {
            moveToTarget = false;
            rb.velocity = Vector2.zero; // Parar o personagem completamente
            petAnimManager.StopWalking(); // Parar a animação de caminhada
            
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
                switch (targetTransform.tag)
                {
                    case "Tv":
                        sr.sprite = interactiveObject.isOpen ? tvOpenSprite : tvClosedSprite;
                        break;
                    case "Door":
                        sr.sprite = interactiveObject.isOpen ? doorOpenSprite : doorClosedSprite;
                        break;
                    case "Window":
                        sr.sprite = interactiveObject.isOpen ? windowOpenSprite : windowClosedSprite;
                        break;
                    // Add more cases as needed
                }

                lastInteractionTime = Time.time; // Update the last interaction time after a successful interaction
            }
            else
            {
                Debug.Log("InteractiveObject or SpriteRenderer not found on the target object, or the player is not in range.");
            }
        }
    }
