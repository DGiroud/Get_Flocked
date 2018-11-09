using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

[System.Serializable]
public struct PlayerMaterials
{
    public Material shepherd;
    public Material goal;
}

public struct WasButtonPressed
{
    public bool A;
    public bool B;
    public bool Right;
    public bool Left;
}

public class UILobbyMenu : MonoBehaviour
{
    // array of colours, to be filled up by designers
    [SerializeField]
    private Color[] colours;
    [SerializeField]
    private PlayerMaterials[] playerMaterials;

    [SerializeField]
    private GameObject[] CPUPanels;
    [SerializeField]
    private GameObject[] colourSelectionPanels;
    [SerializeField]
    private Image[] colourPickers;
    [SerializeField]
    private GameObject[] readyPanels;
    
    private bool[] joined;
    private int[] colourID;
    private bool[] ready;
    private WasButtonPressed[] wasButtonPressed;

    // Use this for initialization
    void Awake()
    {
        joined = new bool[4];
        colourID = new int[colours.Length];
        ready = new bool[4];
        wasButtonPressed = new WasButtonPressed[4];

        for (int i = 0; i < 4; i++)
        {
            joined[i] = false;
            colourID[i] = 0;
            ready[i] = false;
            wasButtonPressed[i].A = false;
            wasButtonPressed[i].B = false;
            wasButtonPressed[i].Left = false;
            wasButtonPressed[i].Right = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForJoin();
        CheckForInput();
    }
    
    private void Join(int playerID)
    {
        joined[playerID] = true;
        ToggleColourSelectionPanel(playerID);
        wasButtonPressed[playerID].A = true;
    }

    private void UnJoin(int playerID)
    {
        joined[playerID] = false;
        ToggleCPUPanel(playerID);
        wasButtonPressed[playerID].B = true;
    }

    private void Ready(int playerID)
    {
        ready[playerID] = true;
        ToggleReadyPanel(playerID);
        wasButtonPressed[playerID].A = true;
    }

    private void NextColour(int playerID, bool previousColour = false)
    {
        // increment or decrement colour ID depending on left or right input
        if (previousColour)
        {
            colourID[playerID]--;
            wasButtonPressed[playerID].Left = true;
        }
        else
        {
            colourID[playerID]++;
            wasButtonPressed[playerID].Right = true;
        }

        // clamp the colourID such that it loops around (no array index overflow)
        if (colourID[playerID] >= colours.Length)
            colourID[playerID] = 0; // too far right, reset colour to first colour
        else if (colourID[playerID] < 0)
            colourID[playerID] = colours.Length - 1; // too far left, reset colour to last colour

        Color newColour = colours[colourID[playerID]];
        colourPickers[playerID].color = newColour;

        playerMaterials[playerID].shepherd.SetColor("_PlayerColour", newColour);
        playerMaterials[playerID].goal.SetColor("_PlayerColour", newColour);
    }

    /// <summary>
    /// iterates over all possible controllers (four), checks if they're connected and checks
    /// for presses on the A, B, X, Y buttons.
    /// </summary>
    private void CheckForJoin()
    {
        // iterate over four controllers
        for (int i = 0; i <= (int)PlayerIndex.Four; i++)
        {
            // get the controller
            GamePadState gamePad = GamePad.GetState((PlayerIndex)i);

            // is it connected?
            if (gamePad.IsConnected)
            {
                // press A for player 1
                if (!joined[i] && gamePad.Buttons.A == ButtonState.Pressed)
                {
                    Join(i);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckForInput()
    {
        for (int i = 0; i < joined.Length; i++)
        {
            if (joined[i])
            {
                GamePadState gamePad = GamePad.GetState((PlayerIndex)i);

                // A button functionality for choosing your colour and joining the game
                if (gamePad.Buttons.A == ButtonState.Pressed && !wasButtonPressed[i].A)
                    Ready(i);
                else if (gamePad.Buttons.A == ButtonState.Released)
                    wasButtonPressed[i].A = false; // button released

                // B button functionality for opting out of joining the game
                if (gamePad.Buttons.B == ButtonState.Pressed && !wasButtonPressed[i].B)
                    UnJoin(i);
                else if (gamePad.Buttons.B == ButtonState.Released)
                    wasButtonPressed[i].B = false; // button released

                // right button functionality for scrolling through preset colours
                if (gamePad.DPad.Right == ButtonState.Pressed && !wasButtonPressed[i].Right)
                    NextColour(i);
                else if (gamePad.DPad.Right == ButtonState.Released)
                    wasButtonPressed[i].Right = false; // button released

                // left button functionality for scrolling through preset colours
                if (gamePad.DPad.Left == ButtonState.Pressed && !wasButtonPressed[i].Left)
                    NextColour(i, true); 
                else if (gamePad.DPad.Left == ButtonState.Released)
                    wasButtonPressed[i].Left = false; // button released
            }
        }
    }

    private void ToggleCPUPanel(int playerID)
    {
        CPUPanels[playerID].SetActive(true);
        colourSelectionPanels[playerID].SetActive(false);
        readyPanels[playerID].SetActive(false);
    }

    private void ToggleColourSelectionPanel(int playerID)
    {
        CPUPanels[playerID].SetActive(false);
        colourSelectionPanels[playerID].SetActive(true);
        readyPanels[playerID].SetActive(false);
    }

    private void ToggleReadyPanel(int playerID)
    {
        CPUPanels[playerID].SetActive(false);
        colourSelectionPanels[playerID].SetActive(false);
        readyPanels[playerID].SetActive(true);
    }
}
