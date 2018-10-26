using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
public class PlayerLobbyMenu : MonoBehaviour {

    [SerializeField]
    public List<GameObject> players
    {
        get;
        private set;
    }
    private List<GamePadState> gamePads;
    private int playerAdded;

    // Use this for initialization
    void Awake () {

    }

    // Update is called once per frame
    void Update () {
        gamePads = AllControllers();
    }

    public void ControllerAdded()
    {
        
    }

    //gets all of the states off all controller inputs
    public List<GamePadState> AllControllers()
    {
        List<GamePadState> controllerNumber = new List<GamePadState>();
        
        //iterates through all four players to check the gamepad state
        for (int i = 0; i <= (int)PlayerIndex.Four; i++ )
        {
            GamePadState GamePadXInput = GamePad.GetState((PlayerIndex)i);
            ConnectedController(); 
        }
        return controllerNumber;
    }
  
    //checking to see if the controller is connected
   public void ConnectedController()
   {
        //iterate through all four players
        for (int i = 0; i <= (int)PlayerIndex.Four; i++)
        {
        GamePadState showState = GamePad.GetState((PlayerIndex)i);
            //checking to see if the controller is connected 
            //(should always return true)
            if (showState.IsConnected)
            {
               Debug.Log(showState.ToString() + "Player is connected");
            }
            GamePadXInput();
        }
    }
   
    //reading the users input
    public void GamePadXInput()
    {
        PlayerIndex controllerNumber = PlayerIndex.One;
        GamePadState state = GamePad.GetState(controllerNumber);

       if (state.Buttons.A == ButtonState.Pressed)
       {
            //button being pressed down
            Debug.Log(state.ToString() + "Button has been pressed!");
       }
       if (state.Buttons.A == ButtonState.Released)
       {
            //button being released
            Debug.Log(state.ToString() + "Button has been released!");
       }

    } 
}