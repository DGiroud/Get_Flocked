using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerManager : MonoBehaviour
{
    // singleton instance
    #region singleton

    private static PlayerManager instance;

    /// <summary>
    /// getter for singleton instance of PlayerManager
    /// </summary>
    public static PlayerManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    // prefabs
    [Header("Prefabs")]
    public GameObject playerPrefab; // reference to player
    public GameObject CPUPrefab; // reference to CPU
    
    // spawn settings
    [Header("Start Positions")]
    public Transform[] actorStartPositions; // array of player spawn positions

    // lists of all players and their respective controllers
    private List<GameObject> players; // the players
    private List<GamePadState> gamePads; // their gamepads

    /// <summary>
    /// detects the number of game pads plugged in and assigns them accordingly
    /// </summary>
	void Awake ()
    {
        // assign singleton instance
        instance = this;

        // initialise player array
        players = new List<GameObject>();

        // get a list of all connected controllers
        gamePads = GetGamePads();

        if (gamePads.Count == 0)
        {
            // using keyboard
        }
        else
        {
            AssignGamePads(); // using controller/s
        }
    }

    /// <summary>
    /// Helper function which retrieves all connected Xbox controllers
    /// </summary>
    /// <returns>list of currently connected Xbox controllers</returns>
    public List<GamePadState> GetGamePads()
    {
        List<GamePadState> output = new List<GamePadState>();

        for (int i = 0; i < (int)PlayerIndex.Four; i++)
        {
            // get gamepad 0, 1, 2, etc.
            GamePadState gamePad = GamePad.GetState((PlayerIndex)i);

            // if said gamepad is connected...
            if (gamePad.IsConnected)
            {
                output.Add(gamePad); //... add it to gamepad list
            }
        }

        return output;
    }

    /// <summary>
    /// Uses list of game controllers to determine how many players and
    /// CPUs are required, and assigns them accordingly
    /// </summary>
    public void AssignGamePads()
    {
        // for every connected gamepad...
        for (int i = 0; i < gamePads.Count; i++)
        {
            // ...create a player
            players.Add(InstantiatePlayer(i));
        }

        // for every non-connected gamepad...
        for (int i = gamePads.Count; i <= (int)PlayerIndex.Four; i++)
        {
            // ...create a CPU
            players.Add(InstantiateCPU(i));
        }
    }

    public void AssignKeyboard()
    {
        players.Add(InstantiatePlayer(0, PlayerInput.Keyboard));

        for (int i = 1; i <= (int)PlayerIndex.Four; i++)
        {
            players.Add(InstantiateCPU(i));
        }
    }

    /// <summary>
    /// function which instantiates the player prefab based on their pre-
    /// determined spawn location
    /// </summary>
    /// <param name="playerIndex">the controller index of the player, e.g. 0</param>
    /// <returns>the instantiated player for convenience</returns>
    public GameObject InstantiatePlayer(int playerIndex, PlayerInput playerInput = PlayerInput.Controller)
    {
        // get player spawn position
        Transform startTransform = actorStartPositions[playerIndex];

        // add player to scene, then set position
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = startTransform.position;
        

        // assign the player access to a controller
        player.GetComponent<Player>().SetController((PlayerIndex)playerIndex);

        return player;
    }

    /// <summary>
    /// function which instantiates the CPU prefab based on their pre-
    /// determined spawn location
    /// </summary>
    /// <param name="CPUIndex">the index of the CPU, e.g. 1, 2, 3</param>
    /// <returns>the instantiated CPU for convenience</returns>
    public GameObject InstantiateCPU(int CPUIndex)
    {
        // get CPU spawn position
        Transform startTransform = actorStartPositions[CPUIndex];

        // add CPU to scene, then set position
        GameObject CPU = Instantiate(CPUPrefab);
        CPU.transform.position = startTransform.position;

        return CPU;
    }
}
