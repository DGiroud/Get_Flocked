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
    // the type of input this player is using, e.g. keyboard
    public PlayerInput playerInput;


    /// <summary>
    /// handle player input
    /// </summary>
    override public void Update ()
    {
        switch (playerInput)
        {
            case PlayerInput.Keyboard:
                KeyboardInput();
                break;

            case PlayerInput.Controller:
                GamePadInput();
                break;
        }

        // call update on BaseActor
        base.Update();
    }

    private void GamePadInput()
    {
        GamePadState input = GamePad.GetState((PlayerIndex)actorID);

        GamePadMovement(input);
        GamePadKick(input);
        GamePadRelease(input);
    }

    /// <summary>
    /// placeholder movement function for player. Handles the translation and
    /// rotation of the player in the x and z axis
    /// </summary>
    private void GamePadMovement(GamePadState gamePad)
    {
        // x & z translation mapped to horizontal & vertical respectively
        if (gamePad.ThumbSticks.Left.X == 0.0f && gamePad.ThumbSticks.Left.Y == 0.0f)
            return;

        Move(gamePad.ThumbSticks.Left.X, gamePad.ThumbSticks.Left.Y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gamePad"></param>
    private void GamePadKick(GamePadState gamePad)
    {
        // A to kick sheep
        if (gamePad.Buttons.A == ButtonState.Pressed)
        {
            GameObject sheep = ReleaseSheep();
            LaunchSheep(sheep);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gamePad"></param>
    private void GamePadRelease(GamePadState gamePad)
    {
        // B to release sheep
        if (gamePad.Buttons.B == ButtonState.Pressed)
        {
            ReleaseSheep();
        }
    }

    private void KeyboardInput()
    {
        KeyboardMovement();
        KeyboardKick();
        KeyboardRelease();
    }

    private void KeyboardMovement()
    {
        // x & z translation mapped to horizontal & vertical respectively
        if (Input.GetAxisRaw("Horizontal") == 0.0f && Input.GetAxisRaw("Vertical") == 0.0f)
            return;

        Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void KeyboardKick()
    {
        // space to kick sheep
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject sheep = ReleaseSheep();
            LaunchSheep(sheep);
        }
    }

    private void KeyboardRelease()
    {

    }
}
