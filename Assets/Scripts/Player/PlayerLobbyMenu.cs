using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;


/*#region XInput: !IMPORTANT! 
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
          GamePadState GamePadXInput = GamePad.GetState();
        }
        return controllerNumber;
    }
  
  
   public void ConnectedController()
   {
        PlayerIndex player = PlayerIndex.One;
        GamePadState showState = GamePad.GetState(player);
  
        if (showState.IsConnected)
        {
            return true;
        }
        else
        {
            return false;
        }
  
    }
  
    public void GamePadXInput()
    {
        PlayerIndex controllerNumber = PlayerIndex.One;
        GamePadState state = GamePad.GetState(controllerNumber);

        if (controllerNumber.IsConnected && controllerNumber.Buttons.A == ButtonState.Pressed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region RandomCode

    //  [SerializeField]
    //  private PlayerIndex gamePadID;
    //  private GamePadState current;
    //  private GamePadState previous;
    //
    //  
    //
    //  
    //  public void Update()
    //  {
    //
    //  }
    //
    //  public void Awake()
    //  {
    //
    //  }
    //
    //  public XInputController (int padID)
    //  {
    //      if (padID == 1 || padID == 2 || padID == 3 || padID == 4 )
    //      {
    //          padID = (PlayerIndex)padID;     //same value for PlayerIndex
    //          current = GamePad.GetState(padID);
    //      }
    //      else
    //      {
    //          Debug.Log(padID.ToString() + "Not Vaild");
    //      }
    //
    //  }
    //
    //  public bool GetConnection()
    //  {
    //      return current.IsConnected;
    //  }
    //
    //  public int GetPad()
    //  {
    //      return (int)gamePadID;
    //  }
    //
    //  public GamePadState GetCurrentState()
    //  {
    //      return current;
    //  }
    //
    //  public GamePadState GetPreviousState()
    //  {
    //      return previous;
    //  }
    //
    //  public void UpdatePad()
    //  {
    //      previous = current;
    //      current = GamePad.GetState(gamePadID);
    //  }
    //
    //
    //  /// <summary>
    //  /// Button Taps
    //  /// Tapping the A button
    //  /// </summary>
    //  /// <returns></returns>
    //  public bool PressedA()
    //  {
    //      if (current.IsConnected && current.Buttons.A == 
    //          ButtonState.Pressed && previous.Buttons.A == ButtonState.Released)
    //      {
    //          return true;
    //      }
    //      else
    //      {
    //          return false;
    //      }
    //  }
    //
    //  public bool PressA()
    //  {
    //      if (current.IsConnected && current.Buttons.A
    //          == ButtonState.Pressed)
    //      {
    //          return true;
    //      }
    //      else
    //      {
    //          return false;
    //      }
    //  }

    #endregion RandomCode
}*/