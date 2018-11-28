using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public struct PlayerPreferences
{
    public bool isReady;
    public Color playerColour;
}

/// <summary>
/// struct used to encapsulate variables related to the different
/// materials of the player
/// </summary>
[System.Serializable]
public struct PlayerMaterials
{
    public Material shepherd;
    public Material goal;
}

/// <summary>
/// struct filled with booleans for each relevant button. Said bools
/// are true if the button is currently being pressed, false otherwise.
/// This is used to prevent situations in which pressed buttons are 
/// considered "held down"
/// </summary>
public struct WasButtonPressed
{
    public bool A;
    public bool B;
    public bool Y;
    public bool Right;
    public bool Left;
}

public class UILobbyMenu : MonoBehaviour
{
    // scene indexes used for scene transitions
    [Header("Scene Transitions")]
    [SerializeField]
    [Tooltip("the scene index (in build settings) of the main menu")]
    private int mainMenuID;
    [SerializeField]
    [Tooltip("the scene index (in build settings) of the main game")]
    private int mainGameID;

    // array of colours, to be filled up by designers
    [Header("Player Customization")]
    [SerializeField]
    private Color[] selectableColours;
    [SerializeField]
    private PlayerMaterials[] playerMaterials;
    
    [Header("Button References")]
    [SerializeField]
    private Button[] joinButtons;
    [SerializeField]
    private Button[] unJoinButtons;
    [SerializeField]
    private Button[] readyButtons;
    [SerializeField]
    private Button[] unReadyButtons;
    [SerializeField]
    private Image[] colourPickers;
    [SerializeField]
    private Button startButton;
    
    // all arrays of length 4, one index for each player
    private bool[] joined; // has the player pressed A but not readied up?
    private bool[] ready; // has the player readied up?
    private int[] colourID; // which colour from the colour array did each player pick?
    private WasButtonPressed[] wasButtonPressed; // are they pressing buttons? 
    private bool wasSpacePressed = false;
    static private PlayerPreferences[] playerPrefs;
    static public PlayerPreferences[] GetPlayerPrefs { get { return playerPrefs; } }

    /// <summary>
    /// initialises all of the relevant arrays
    /// </summary>
    void Awake()
    {
        joined = new bool[4];
        colourID = new int[4];
        ready = new bool[4];
        wasButtonPressed = new WasButtonPressed[4];

        for (int i = 0; i < 4; i++)
        {
            // joined and ready arrays initialised to false
            joined[i] = false;
            colourID[i] = i;
            ready[i] = false;

            // all button pressed states initalised to false
            wasButtonPressed[i].A = false;
            wasButtonPressed[i].B = false;
            wasButtonPressed[i].Left = false;
            wasButtonPressed[i].Right = false;

            playerMaterials[i].shepherd.SetColor("_PlayerColour", selectableColours[i]);
            playerMaterials[i].goal.SetColor("_PlayerColour", selectableColours[i]);
            colourPickers[i].color = playerMaterials[i].shepherd.GetColor("_PlayerColour");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        CheckForMenu();

        // check each unjoined player for A button input (join)
        CheckForJoin();

        // check each joined player for B button input (opt out)
        CheckForUnJoin();

        // check each joined player for second A button input (ready up)
        CheckForReady();

        // check each ready player for B button input (back to colour select)
        CheckForUnReady();

        // check each joined player for left/right D-Pad input (select colour)
        CheckForColourSelect();

        // check each unjoined/ready player for start input (game start)
        // (doesn't work if anybody is still selecting colour)
        CheckForStart();

        wasSpacePressed = false;
    }
    
    /// <summary>
    /// allows the player to join the game
    /// </summary>
    /// <param name="playerID">the ID of the player that's joining</param>
    public void Join(int playerID)
    {
        joined[playerID] = true; // joined
        wasButtonPressed[playerID].A = true;
        wasSpacePressed = true;
    }

    /// <summary>
    /// allows the player to opt out of the game
    /// </summary>
    /// <param name="playerID">the ID of the player that's opting out</param>
    public void UnJoin(int playerID)
    {
        joined[playerID] = false; // not joined
        wasButtonPressed[playerID].B = true;
    }

    /// <summary>
    /// allows the player to go back to colour select
    /// </summary>
    /// <param name="playerID">the ID of the player that's going back</param>
    public void UnReady(int playerID)
    {
        joined[playerID] = true; // joined
        ready[playerID] = false; // not ready
        wasButtonPressed[playerID].B = true;
    }


    /// <summary>
    /// allows the player to ready up once they've selected a colour
    /// </summary>
    /// <param name="playerID">the ID of the player that's readying up</param>
    public void Ready(int playerID)
    {
        joined[playerID] = false; // not joined
        ready[playerID] = true; // but ready!
        wasButtonPressed[playerID].A = true;
        wasSpacePressed = true;
    }

    /// <summary>
    /// iterates over all possible controllers (four), checks if they're connected and checks
    /// for presses on the A, B, X, Y buttons.
    /// </summary>
    private void CheckForMenu()
    {
        // iterate over four controllers
        for (int i = 0; i <= (int)PlayerIndex.Four; i++)
        {
            // get the controller
            GamePadState gamePad = GamePad.GetState((PlayerIndex)i);

            // is it connected?
            if (gamePad.IsConnected)
            {
                // B input to go back to the main menu
                if (gamePad.Buttons.Y == ButtonState.Pressed)
                    SceneManager.LoadScene(mainMenuID);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                // otherwise, escape to go back to main menu
                SceneManager.LoadScene(mainMenuID);
            }
        }
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
                // if the player hasn't joined
                if (IsPlayerCPU(i))
                {
                    // allow A input to join
                    if (gamePad.Buttons.A == ButtonState.Pressed)
                        joinButtons[i].onClick.Invoke();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !wasSpacePressed)
                if (IsPlayerCPU(0))
                    joinButtons[0].onClick.Invoke();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckForReady()
    {
        for (int i = 0; i < joined.Length; i++)
        {
            if (joined[i])
            {
                GamePadState gamePad = GamePad.GetState((PlayerIndex)i);

                // A button functionality for choosing your colour and joining the game
                if (gamePad.Buttons.A == ButtonState.Pressed && !wasButtonPressed[i].A && !IsColourTaken(i))
                    readyButtons[i].onClick.Invoke();
                else if (gamePad.Buttons.A == ButtonState.Released)
                    wasButtonPressed[i].A = false; // button released

                if (Input.GetKeyDown(KeyCode.Space) && !wasSpacePressed)
                    readyButtons[i].onClick.Invoke();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckForUnJoin()
    {
        for (int i = 0; i < joined.Length; i++)
        {
            if (joined[i])
            {
                GamePadState gamePad = GamePad.GetState((PlayerIndex)i);

                // B button functionality for opting out of joining the game
                if (gamePad.Buttons.B == ButtonState.Pressed && !wasButtonPressed[i].B)
                    unJoinButtons[i].onClick.Invoke();
                else if (gamePad.Buttons.B == ButtonState.Released)
                    wasButtonPressed[i].B = false; // button released

                if (Input.GetKeyDown(KeyCode.Backspace))
                    unJoinButtons[i].onClick.Invoke();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckForUnReady()
    {
        for (int i = 0; i < ready.Length; i++)
        {
            if (ready[i])
            {
                GamePadState gamePad = GamePad.GetState((PlayerIndex)i);

                // B button functionality for opting out of joining the game
                if (gamePad.Buttons.B == ButtonState.Pressed && !wasButtonPressed[i].B)
                    unReadyButtons[i].onClick.Invoke();
                else if (gamePad.Buttons.B == ButtonState.Released)
                    wasButtonPressed[i].B = false; // button released

                if (Input.GetKeyDown(KeyCode.Backspace))
                    unReadyButtons[i].onClick.Invoke();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckForColourSelect()
    {
        for (int i = 0; i < joined.Length; i++)
        {
            if (joined[i])
            {
                GamePadState gamePad = GamePad.GetState((PlayerIndex)i);

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

                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                    NextColour(i);
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                    NextColour(i, true);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckForStart()
    {
        // check to see if there is anybody who has joined (e.g. not CPU and not ready)
        for (int i = 0; i < joined.Length; i++)
        {
            // if someone is still choosing a colour, don't allow start
            if (joined[i])
            {
                startButton.interactable = false; // grey out button
                return;
            }
        }

        // everyone is ready, ungrey out button
        startButton.interactable = true;

        // iterate over all the CPUs
        for (int i = 0; i <= (int)PlayerIndex.Four; i++)
            if (IsPlayerCPU(i))
                while (IsElsewhereInArray(colourID, i)) // if the CPU's colour is already taken
                    NextColour(i); // assign the CPU a new colour

        // check for input from all ready players
        for (int i = 0; i <= (int)PlayerIndex.Four; i++)
        {
            GamePadState gamePad = GamePad.GetState((PlayerIndex)i);

            if (gamePad.IsConnected)
                if (gamePad.Buttons.Start == ButtonState.Pressed) // on start pressed
                {
                    // initialise home made player preferences array
                    playerPrefs = new PlayerPreferences[4];

                    for (int j = 0; j < 4; j++)
                    {
                        playerPrefs[j].isReady = ready[j]; // boolean used to decide which players to assign controllers to in-game
                        playerPrefs[j].playerColour = selectableColours[colourID[j]]; // each player's colour
                    }

                    // load the game
                    SceneManager.LoadScene(mainGameID);
                }
            if (Input.GetKeyDown(KeyCode.Return))
                {
                    // initialise home made player preferences array
                    playerPrefs = new PlayerPreferences[4];

                    for (int j = 0; j < 4; j++)
                    {
                        playerPrefs[j].isReady = ready[j]; // boolean used to decide which players to assign controllers to in-game
                        playerPrefs[j].playerColour = selectableColours[colourID[j]]; // each player's colour
                    }

                    // load the game
                    SceneManager.LoadScene(mainGameID);
                }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="previousColour"></param>
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
        if (colourID[playerID] >= selectableColours.Length)
            colourID[playerID] = 0; // too far right, reset colour to first colour
        else if (colourID[playerID] < 0)
            colourID[playerID] = selectableColours.Length - 1; // too far left, reset colour to last colour
        
        if (IsColourTaken(playerID))
            readyButtons[playerID].interactable = false;
        else
            readyButtons[playerID].interactable = true;

        Color newColour = selectableColours[colourID[playerID]];
        colourPickers[playerID].color = newColour;

        playerMaterials[playerID].shepherd.SetColor("_PlayerColour", newColour);
        playerMaterials[playerID].goal.SetColor("_PlayerColour", newColour);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="actorID"></param>
    /// <returns></returns>
    private bool IsColourTaken(int actorID)
    {
        int colour = colourID[actorID];

        for (int i = 0; i < ready.Length; i++)
        {
            if (ready[i])
            {
                if (colourID[i] == colour)
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="actorID"></param>
    /// <returns></returns>
    private bool IsColourHovered(int actorID)
    {
        int colour = colourID[actorID];

        for (int i = 0; i < joined.Length; i++)
        {
            if (joined[i])
            {
                if (colourID[i] == colour)
                    return true;
            }
        }

        return false;
    }

    private bool IsPlayerCPU(int actorID)
    {
        return (!joined[actorID] && !ready[actorID]);
    }

    private bool IsElsewhereInArray(int[] array, int index)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (i == index)
                continue;

            if (array[index] == array[i])
                return true;
        }

        return false;
    }
}
