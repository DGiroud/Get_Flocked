using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public enum PlayerInput
{
    Controller,
    Keyboard
}

public class Player : BaseActor
{
    [SerializeField]
    private PlayerInput playerInput;

    // game pad index
    private PlayerIndex controllerNumber;

    private Vector3 translation;
	
	/// <summary>
    /// handle player input
    /// </summary>
	override public void Update ()
    {
        GamePadState input = GamePad.GetState(controllerNumber);
        
        // movement
        Movement(input);

        // action
        Kick(input);

        // call update on BaseActor
        base.Update();
    }

    /// <summary>
    /// set controller function, which assigns the player a controller index
    /// </summary>
    /// <param name="playerIndex">example input: PlayerIndex.One</param>
    public void SetController(PlayerIndex playerIndex)
    {
        controllerNumber = playerIndex;
    }

    /// <summary>
    /// placeholder movement function for player. Handles the translation and
    /// rotation of the player in the x and z axis
    /// </summary>
    public void Movement(GamePadState gamePad)
    {
        // x & z translation mapped to horizontal & vertical respectively
        if (gamePad.ThumbSticks.Left.X == 0.0f && gamePad.ThumbSticks.Left.Y == 0.0f)
            return;

        translation.x = gamePad.ThumbSticks.Left.X;
        translation.z = gamePad.ThumbSticks.Left.Y;

        // rotation handling
        transform.rotation = Quaternion.LookRotation(translation);

        // multiply by speed and delta time
        translation *= speed * Time.deltaTime;

        // perform movement
        transform.Translate(translation, Space.World);
    }

    public void Kick(GamePadState gamePad)
    {
        // space or A to kick sheep
        if (Input.GetKeyDown(KeyCode.Space) || gamePad.Buttons.A == ButtonState.Pressed)
        {
            GameObject sheep = ReleaseSheep();
            LaunchSheep(sheep);
        }
    }

    public void Release(GamePadState gamePad)
    {
        // B to release sheep
        if (gamePad.Buttons.B == ButtonState.Pressed)
        {
            ReleaseSheep();
        }
    }
}
