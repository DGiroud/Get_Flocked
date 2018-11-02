using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

#region Structs
//storing state of button for gamepad(s)
public struct XButton
{
    public ButtonState prev_state;
    public ButtonState state;
}

//storing state of gamepad trigger
public struct TrigState
{
    public float prev_val;
    public float current_val;
}
#endregion
public class xBox360_Gamepad : MonoBehaviour
{
    #region Variables
    private GamePadState prev_state; //previous state of gamepad
    private GamePadState state;      //current state of gamepad

    private int gamepadVal;          //index for gamepad(s)
    private PlayerIndex playerIndex; //XInput for the player

   // private Dictionary inputMap;

    private XButton A, B, X, Y;                                //Player Buttons
    private XButton DPad_Up, DPad_Down, DPad_Left, DPad_Right; //Players DPads

    private XButton Guide;            //XBox button
    private XButton Back, Starting;   //XBox Start and Back
    private XButton L3, R3;           //JoySticks/Thumbsticks (whatever u wanna call them)
    private XButton LB, RB;           //Bumpers for controler
    private TrigState LT, RT;         //triggers
    public int Index {get             //returning value for gamepad value
                     { return gamepadVal; } } 
    public bool IsConnected {get     //returning connection state for gamepad
                            { return state.IsConnected; } }
    #endregion
	
	// Update is called once per frame
	void Update ()
    {
        //getting the current state of the gamepad
        state = GamePad.GetState(playerIndex);
        
        
        if (state.IsConnected)
        {
            //update input
  //          UIM();
        }
    }

    public void Refreash()
    {
        //refreashing the previous gamepad state
        prev_state = state;

        if (state.IsConnected)
        {
            //update input
 //           UIM();
        }
    }
    //returning state of button
 //   public bool GetButton(string button)
 //   {
 //       return inputMap(button).state == ButtonState.Pressed ? true : false;
 //   }
 //
 //   //returning state of button but on frame
 //   public bool GetButtonDown(string button)
 //   {
 //       return (inputMap[button].prev_state == ButtonState.Released &&
 //               inputMap[button].state == ButtonState.Pressed);
 //   }
 //
 //   //updainging the input map(UIM)
 //   void UIM()
 //   {
 //       //buttons
 //       inputMap["A"] = A;
 //       inputMap["X"] = X;
 //       inputMap["Y"] = Y;
 //       inputMap["B"] = B;
 //
 //       //dpads
 //       inputMap["DPad_Down"]  = DPad_Down;
 //       inputMap["DPad_Up"]    = DPad_Up;
 //       inputMap["DPad_Left"]  = DPad_Left;
 //       inputMap["DPad_Right"] = DPad_Right;
 //
 //       //misc buttons (Start, Back, Guide)
 //       inputMap["Guide"]      = Guide;
 //       inputMap["Back"]       = Back;
 //       inputMap["Start"]      = Starting;
 //
 //       //thumbsticks
 //       inputMap["L3"] = L3;
 //       inputMap["R3"] = R3;
 //
 //       //bumpers
 //       inputMap["LB"] = LB;
 //       inputMap["RB"] = RB;
 //
 //   }

    public xBox360_Gamepad(int index)
    {
        //setting pad index
        gamepadVal = index - 1;
        playerIndex = (PlayerIndex)gamepadVal;
    }

    private void states()
    {
        //Getting current State
        state = GamePad.GetState(playerIndex);

        //checking gamepad connection
        if (state.IsConnected)
        {
            //buttons
            A.state = state.Buttons.A;
            X.state = state.Buttons.X;
            Y.state = state.Buttons.Y;
            B.state = state.Buttons.B;

            //dpads
            DPad_Down.state = state.DPad.Down;
            DPad_Up.state = state.DPad.Up;
            DPad_Left.state = state.DPad.Left;
            DPad_Right.state = state.DPad.Right;

            //misc
            Guide.state = state.Buttons.Guide;
            Back.state = state.Buttons.Back;
            Starting.state = state.Buttons.Start;

            //JoyStick
            L3.state = state.Buttons.LeftStick;
            R3.state = state.Buttons.RightStick;

            //Triggers
            LB.state = state.Buttons.LeftShoulder;
            RB.state = state.Buttons.RightShoulder;

        }
    }

}
