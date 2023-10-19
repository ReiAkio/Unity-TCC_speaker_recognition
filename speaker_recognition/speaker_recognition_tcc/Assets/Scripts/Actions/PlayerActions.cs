using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public float speed = 5.0f;
    private bool moveToTarget = false;
    public Transform targetTransform;
    private string currentSpeaker;
    

    public Sprite tvNewSprite;
    public Sprite doorNewSprite;
    public Sprite windowNewSprite;

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
            
            if (speech.Contains("Oi Ravi") && speech.Contains("abra") && speech.Contains("porta") && Connection.raviSpeaker == Connection.predictionSpeaker)
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
            if (speech.Contains("Oi Ravi") && speech.Contains("Ligue") && speech.Contains("TV"))
            {
                GameObject tvObject = GameObject.FindGameObjectWithTag("Tv");
                if (tvObject != null)
                {
                    targetTransform = tvObject.transform;
                    InitiateMovement();
                }
                else
                {
                    Debug.Log("Objeto tv não encontrado!");
                }
            }
            if (speech.Contains("Oi Ravi") && speech.Contains("abra") && speech.Contains("janela"))
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
            // Adicione mais condições aqui para outros comandos de voz
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
            Debug.Log("Executando interação com o objeto");
            if (targetTransform == null) return;
            InteractiveObject interactiveObject = targetTransform.GetComponent<InteractiveObject>();
            

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
