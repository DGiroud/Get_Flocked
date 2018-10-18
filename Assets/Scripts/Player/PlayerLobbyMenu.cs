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
public class PlayerLobbyMenu : MonoBehaviour {
    [SerializeField]
    public List<GameObject> players
    {
        get;
        private set;
    }
    private List<GamePadState> gamePads;

    


	// Use this for initialization
	void Awake () {
        gamePads = AllControllers();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<GamePadState> AllControllers()
    {
        List<GamePadState> controllerNumber = new List<GamePadState>();

        for (int i = 0; i <= (int)PlayerIndex.Four; i++ )
        {
          //  GamePadState GamePadXInput = GamePad.GetState();
        }


        return controllerNumber;
    }


   public void ConnectedController()
   {
        PlayerIndex player = PlayerIndex.One;
        GamePadState showState = GamePad.GetState(player);

        if (showState.IsConnected)
        {
            
        }

    }

    public void GamePadXInput()
    {
        PlayerIndex controllerNumber = PlayerIndex.One;
        GamePadState state = GamePad.GetState(controllerNumber);

        if (state.Buttons.A == ButtonState.Pressed)
        {
            
        }
        if (state.Buttons.A == ButtonState.Released)
        {

        }
    }


}
