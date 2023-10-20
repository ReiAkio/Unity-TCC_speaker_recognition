using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    private bool playerInRange = false;
    public PlayerActions playerActions;
    public bool isOpen = false;

    public float interactionCooldown = 1.0f; // Cooldown period in seconds
    private float lastInteractionTime;
    
    private void Start()
    {
        // Se playerActions não for definido no inspector, tente encontrar um automaticamente
        if (playerActions == null)
        {
            playerActions = FindObjectOfType<PlayerActions>();
            // Verifique se o objeto de destino atual de Ravi é este objeto
            if (playerActions.targetTransform == transform)
            {
                Debug.Log("Ravi entrou na área de trigger do objeto correto: " + gameObject.name);
                playerActions.ExecuteTargetInteraction(); // Chame o método para interagir com o objeto
            }
        }
        if (playerActions != null)
        {
            playerActions.ExecuteTargetInteraction(); // This will set the initial sprite based on the 'isOpen' state.
        }
    }

    /*void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ravi"))
        {
            // Verifica se o Ravi está interagindo com o objeto de destino correto
            PlayerActions playerActions = other.GetComponent<PlayerActions>();
            if (playerActions != null && playerActions.targetTransform == transform) // Adicionado comparação com targetTransform
            {
                {
                    playerInRange = true;
                    Debug.Log("Ravi entrou na área de trigger do objeto correto: " + gameObject.name);

                    // Parar o Ravi ao entrar na área de trigger
                    playerActions.StopMovement();

                    // Interagir com o objeto
                    if (CanPerformAction())
                    {
                        playerActions.ExecuteTargetInteraction();
                    }
                    
                }
            }
            else if (playerActions.targetTransform != transform)
            {
                Debug.Log("Ravi entrou em uma área de trigger, mas não é o objeto de destino: " + gameObject.name);
            }
            
        }
    }*/
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ravi"))
        {
            PlayerActions playerActions = other.GetComponent<PlayerActions>();
            if (playerActions != null && playerActions.targetTransform == transform) // Adicionado comparação com targetTransform
            {
                {
                    playerInRange = true;
                    Debug.Log("Ravi entrou na área de trigger do objeto correto: " + gameObject.name);
                    
                    playerActions.StopMovement();


                    // Interagir com o objeto
                    if (CanPerformAction())
                    {
                        playerActions.ExecuteTargetInteraction();
                    }
                    
                }
            }
            else if (playerActions.targetTransform != transform)
            {
                Debug.Log("Ravi entrou em uma área de trigger, mas não é o objeto de destino: " + gameObject.name);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ravi"))
        {
            playerInRange = false;
            Debug.Log("Ravi saiu da área de trigger");
        }
    }

    public bool CanPerformAction()
    {
        return playerInRange;
    }
    
    
}
