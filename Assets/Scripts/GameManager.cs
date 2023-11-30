using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NPC currentNPC;
    [SerializeField] [TextArea(minLines: 5, maxLines: 10)] private string dialogueRules;

    private OpenAIApi aiApi = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();

    [Header("Components")]
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private TMP_Text npcDialogueText;
    [SerializeField] private TMP_InputField playerDialogueInput;
    [SerializeField] private Image npcIcon;

    public bool IsTalking;

    public void StartDialogue (NPC npc)
    {
        IsTalking = true;
        currentNPC = npc;
        dialogueCanvas.SetActive(true);
        npcIcon.sprite = npc.Icon;
        Time.timeScale = 0.0f;
        messages.Clear();

        DialogueRequest("");
    }

    public void EndDialogue ()
    {
        IsTalking = false;
        currentNPC = null;
        dialogueCanvas.SetActive(false);
        Time.timeScale = 1.0f;
        messages.Clear();
    }

    async void DialogueRequest (string playerMessage)
    {
        // Update the UI.
        npcDialogueText.text = "Hmm...";
        playerDialogueInput.text = string.Empty;
        playerUI.SetActive(false);

        // This is our message to the API.
        var message = new ChatMessage()
        {
            Role = "user",
            Content = playerMessage
        };

        if(messages.Count == 0)
        {
            message.Content = $"Act as a {currentNPC.PhysicalDescription} in a fantasy RPG." +
                $"As a character, you are {currentNPC.Personality}." +
                $"Your location is {currentNPC.LocationDescription}." +
                $"You also have secret knowledge that you will not speak about unless asked by me: " +
                $"{currentNPC.SecretKnowledge}.\n{dialogueRules}";
        }

        // Add it to the list.
        messages.Add(message);

        // Create the API request and the content to send over.
        var request = new CreateChatCompletionRequest
        {
            //Model = "gpt-3.5-turbo-0301",
            Model = "gpt-3.5-turbo-0613",       // Updated 7/2023
            Messages = messages
        };

        // Send off the request and wait for a response.
        var response = await aiApi.CreateChatCompletion(request);

        // Did we get a valid response?
        if(response.Choices != null)
        {
            ChatMessage responseMessage = response.Choices[0].Message;
            messages.Add(responseMessage);

            npcDialogueText.text = responseMessage.Content;
        }

        playerUI.SetActive(true);
    }

    public void OnTalkButton ()
    {
        DialogueRequest(playerDialogueInput.text);
    }
}