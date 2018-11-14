using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

#region XInput: !IMPORTANT! 
//In any C# script you need to access controllers, add the follow line with your using declarations:
//                  "using XInputDotNetPure;" 

//                      !IMPORTANT NOTE! 
//XInput.NET is not transferred correctly when Unity builds a project.After
//building the project, you must copy XInputInterface.dll into
//your directory containing the built EXE for your project. You
//can see an example in the XInput.NET binary zip, inside the "Output" directory.
#endregion

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

    private PlayerPreferences[] playerPrefs;

    // prefabs
    [Header("Prefabs")]
    [SerializeField]
    private GameObject playerPrefab; // reference to player
    [SerializeField]
    private GameObject CPUPrefab; // reference to CPU

    // spawn settings
    [Header("Start Positions")]
    [SerializeField] 
    private Transform[] actorStartPositions; // array of player spawn positions

    [SerializeField]
    private Material[] playerMaterials;

    // lists of all players and their respective controllers
    public List<GameObject> players { get; private set; } // the players
    private List<GamePadState> gamePads; // their gamepads

    /// <summary>
    /// detects the number of game pads plugged in and assigns them accordingly
    /// </summary>
	void Awake ()
    {
        instance = this; // assign singleton instance

        // retrieve player preferences
        playerPrefs = UILobbyMenu.GetPlayerPrefs;

        // initialise player array
        players = new List<GameObject>();

        // get a list of all connected controllers
        gamePads = GetGamePads();

        if (gamePads.Count == 0)
        {
            // use keyboard if no gamepads connected
            AssignKeyboard();
        }
        else
        {
            // use gamepad/s
            AssignGamePads(); 
        }
    }

    /// <summary>
    /// Helper function which retrieves all connected Xbox controllers
    /// </summary>
    /// <returns>list of currently connected Xbox controllers</returns>
    public List<GamePadState> GetGamePads()
    {
        List<GamePadState> output = new List<GamePadState>();

        for (int i = 0; i <= (int)PlayerIndex.Four; i++)
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
        if (playerPrefs != null)
        {
            for (int i = 0; i < playerPrefs.Length; i++)
            {
                bool isPlayerReady = playerPrefs[i].isReady;

                if (isPlayerReady)
                    players.Add(InstantiatePlayer(i));
                else
                    players.Add(InstantiateCPU(i));
            }
        }
        else
        {
            // for every connected gamepad...
            for (int i = 0; i < gamePads.Count; i++)
            {
                // ...create a player
                players.Add(InstantiatePlayer(i));
            }

            // for every non-connected gamepad...
            for (int i = gamePads.Count; i < actorStartPositions.Length; i++)
            {
                // ...create a CPU
                players.Add(InstantiateCPU(i));
            }
        }
    }

    public void AssignKeyboard()
    {
        // player one uses keyboard
        players.Add(InstantiatePlayer(0, PlayerInput.Keyboard));

        // the rest are a CPU
        for (int i = 1; i < actorStartPositions.Length; i++)
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
    public GameObject InstantiatePlayer(int playerID, PlayerInput playerInput = PlayerInput.Controller)
    {
        // get player spawn position
        Transform startTransform = actorStartPositions[playerID];

        // add player to scene, then set position
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = startTransform.position;
        player.transform.rotation = startTransform.rotation;
        player.GetComponentInChildren<SkinnedMeshRenderer>().material = playerMaterials[playerID];

        Player script = player.GetComponent<Player>();
        script.actorID = playerID;
        script.SetPlayerInput(playerInput);

        return player;
    }

    /// <summary>
    /// function which instantiates the CPU prefab based on their pre-
    /// determined spawn location
    /// </summary>
    /// <param name="cpuID">the index of the CPU, e.g. 1, 2, 3</param>
    /// <returns>the instantiated CPU for convenience</returns>
    public GameObject InstantiateCPU(int cpuID)
    {
        // get CPU spawn position
        Transform startTransform = actorStartPositions[cpuID];

        // add CPU to scene, then set position
        GameObject cpu = Instantiate(CPUPrefab);
        cpu.transform.position = startTransform.position;
        cpu.transform.rotation = startTransform.rotation;
        //cpu.GetComponentInChildren<MeshRenderer>().material = actorColours[CPUIndex];

        CPU script = cpu.GetComponent<CPU>();
        script.actorID = cpuID;

        return cpu;
    }
}
