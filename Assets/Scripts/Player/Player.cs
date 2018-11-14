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
    private Animator animator;

    // player input variables
    private PlayerInput playerInput; // the type of input this player is using, e.g. keyboard
    private bool wasAPressed; // used to prevent weird behaviour when holding the A button

    public void SetPlayerInput(PlayerInput inputType)
    {
        playerInput = inputType;
    }

    /// <summary>
    /// initialise actor type (this is a player)
    /// </summary>
    public void Start()
    {
        actorType = ActorType.Player;

        // cache animator
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// handle player input
    /// </summary>
    public override void Update ()
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

        if (HeldSheep)
            animator.SetBool("isPushing", true);
        else
            animator.SetBool("isPushing", false);

        // call update on BaseActor
        base.Update();
    }

    private void GamePadInput()
    {
        GamePadState input = GamePad.GetState((PlayerIndex)actorID);

        switch (LevelManager.Instance.gameState)
        {
            case GameState.Main:
                {
                    GamePadKick(input);
                    GamePadMovement(input);
                    GamePadDash(input);
                    break;
                }
            case GameState.RoundEnd:
                {
                    GamePadReady(input);
                    break;
                }
        }
    }

    /// <summary>
    /// uses the gamepad joysticks to call BaseActor movement function
    /// </summary>
    private void GamePadMovement(GamePadState gamePad)
    {
        // if no joysticks are being moved, don't move
        if (gamePad.ThumbSticks.Left.X == 0.0f && gamePad.ThumbSticks.Left.Y == 0.0f)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        Vector3 translation = new Vector3(gamePad.ThumbSticks.Left.X, 0, gamePad.ThumbSticks.Left.Y);
        translation.Normalize();

        // x & z translation mapped to horizontal & vertical respectively
        Move(translation.x, translation.z);

        animator.SetBool("isWalking", true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gamePad"></param>
    private void GamePadDash(GamePadState gamePad)
    {
        if (gamePad.Buttons.A == ButtonState.Pressed && !wasAPressed)
        {
            if (HeldSheep || interactionSheep)
                return;

            wasAPressed = true;

            Vector3 translation = new Vector3(gamePad.ThumbSticks.Left.X, 0, gamePad.ThumbSticks.Left.Y);
            translation.Normalize();

            // x & z translation mapped to horizontal & vertical respectively
            Move(translation.x, translation.z, true);
        }
        else if (gamePad.Buttons.A == ButtonState.Released)
            wasAPressed = false;
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
            if (HeldSheep)
            {
                GameObject sheep = ReleaseSheep();
                LaunchSheep(sheep);
                ScoreManager.Instance.IncrementKickCount(actorID);

                animator.SetTrigger("Kick");
                wasAPressed = true;
            }
            else if (interactionSheep)
            {
                LaunchOpponentsSheep(interactionSheep);
                ScoreManager.Instance.IncrementInterceptCount(actorID);
                interactionSheep = null;

                animator.SetTrigger("Kick");
                wasAPressed = true;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gamePad"></param>
    private void GamePadReady(GamePadState gamePad)
    {
        if (gamePad.Buttons.A == ButtonState.Pressed)
        {
            UIMainGame.Instance.ToggleReady(actorID);
        }
    }


    /// <summary>
    /// delete this
    /// </summary>
    private void KeyboardInput()
    {
        GamePadState input = GamePad.GetState((PlayerIndex)actorID);

        switch (LevelManager.Instance.gameState)
        {
            case GameState.Main:
                {
                    KeyboardKick();
                    KeyboardMovement();
                    break;
                }
            case GameState.RoundEnd:
                {
                    KeyboardReady();
                    break;
                }
        }
    }

    /// <summary>
    /// delete this
    /// </summary>
    private void KeyboardMovement()
    {
        // x & z translation mapped to horizontal & vertical respectively
        if (Input.GetAxisRaw("Horizontal") == 0.0f && Input.GetAxisRaw("Vertical") == 0.0f)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        Vector3 translation = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        translation.Normalize();

        Move(translation.x, translation.z);

        animator.SetBool("isWalking", true);
    }

    /// <summary>
    /// delete this
    /// </summary>
    private void KeyboardKick()
    {
        // space to kick sheep
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (HeldSheep)
            {
                GameObject sheep = ReleaseSheep();
                LaunchSheep(sheep);
                ScoreManager.Instance.IncrementKickCount(actorID);

                animator.SetTrigger("Kick");
            }
            else if (interactionSheep)
            {
                LaunchOpponentsSheep(interactionSheep);
                ScoreManager.Instance.IncrementInterceptCount(actorID);
                interactionSheep = null;

                animator.SetTrigger("Kick");
            }
        }
    }

    private void KeyboardReady()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIMainGame.Instance.ToggleReady(actorID);
        }
    }
}
