using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalkTrigger : MonoBehaviour
{
    [SerializeField] private GameObject promptText;
    [SerializeField] private GameManager gameManager;

    private NPC currentSelectedNPC;
    
    void Update ()
    {
        // If we press E key, have an NPC selected and are not currently talking...
        if(Input.GetKeyDown(KeyCode.E) && currentSelectedNPC != null && gameManager.IsTalking == false)
        {
            gameManager.StartDialogue(currentSelectedNPC);
        }
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.TryGetComponent<NPC>(out NPC npc))
        {
            currentSelectedNPC = npc;
            promptText.SetActive(true);
        }
    }

    void OnTriggerExit2D (Collider2D collision)
    {
        if(collision.gameObject == currentSelectedNPC.gameObject)
        {
            currentSelectedNPC = null;
            promptText.SetActive(false);
        }
    }
}