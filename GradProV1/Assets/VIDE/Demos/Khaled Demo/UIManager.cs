using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


    public GameObject container_NPC;
    public GameObject container_Player;
    public Text text_NPC;
    public Text[] text_Choice;
    // Start is called before the first frame update
    void Start()
    {
        container_NPC.SetActive(false);
        container_Player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            if (!VD.isActive)
            {
                Begin();
            }
            else
            {
                VD.Next();
            }
        }
    }
    void Begin() 
    {
        VD.OnNodeChange += UpdateUI;
        VD.OnEnd += End;
        VD.BeginDialogue(GetComponent<VIDE_Assign>());
    }

    void UpdateUI(VD.NodeData data) 
    {
        container_NPC.SetActive(false);
        container_Player.SetActive(false);

        if (data.isPlayer)
        {
            container_Player.SetActive(true);
            for (int i = 0; i < text_Choice.Length; i++) 
            {
                if (i < data.comments.Length)
                {
                    text_Choice[i].transform.parent.gameObject.SetActive(true);
                    text_Choice[i].text = data.comments[i];
                }
                else 
                {
                    text_Choice[i].transform.parent.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            container_NPC.SetActive(true);
            //The commentIndex variable updates automatically for NPC nodes.
            text_NPC.text = data.comments[data.commentIndex];
        }
    }
    void End(VD.NodeData data)
    {
        container_NPC.SetActive(false);
        container_Player.SetActive(false);
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= End;
        VD.EndDialogue();
    }

    void OnDisable()
    {
        if (container_NPC != null) 
            End(null); 
    }
    public void SetPlayerChoice(int choice)
    {
        VD.nodeData.commentIndex = choice;
        if (Input.GetMouseButtonUp(0))
            VD.Next();
    }
}
