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
        // is the player using keyboard or controller?
        switch (playerInput)
        {
            case PlayerInput.Keyboard:
                KeyboardInput();
                break;

            case PlayerInput.Controller:
                GamePadInput();
                break;
        }

        // if the 
        if (HeldSheep)
            animator.SetBool("isPushing", true);
        else
            animator.SetBool("isPushing", false);

        // call update on BaseActor
        base.Update();
    }

    /// <summary>
    /// gets the current state of the player's controller, and depending on the current
    /// state of the game, allow input
    /// </summary>
    private void GamePadInput()
    {
        GamePadState input = GamePad.GetState((PlayerIndex)actorID);

        // different input depending on the current state of the game
        switch (LevelManager.Instance.gameState)
        {
            case GameState.Main: // main game, so...
                {
                    GamePadKick(input); // allow kick
                    GamePadMovement(input); // movement
                    GamePadDash(input); // dash
                    GamePadPause(input); //pause
                    break;
                }
            case GameState.RoundEnd: // round end, so...
                {
                    GamePadReady(input); // allow player to ready up for next round
                    break;
                }
        }
    }

    /// <summary>
    /// CONTROLLER
    /// pauses game and getting access to level manager
    /// </summary>
    /// <param name="gamePad"></param>
    private void GamePadPause(GamePadState gamePad)
    {
        //checking to see if the start button state has been pressed
        if (gamePad.Buttons.Start == ButtonState.Pressed)
        {
            //if the button has been pressed, the game state will be
            //paused which is called from LevelManager
            if (LevelManager.Instance.gameState == GameState.Pause)
                //instanciates the pause menu
                LevelManager.Instance.Pause();
        }

    }

    /// <summary>
    /// KEYBOARD
    /// pauses game and getting access to level manager
    /// </summary>
    private void KeyBoardPause()
    {
        //checking to see if the Escape key has been pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if the Escape key has been pressed, the game state will be 
            //paused which is called from LevelManager
            if (LevelManager.Instance.gameState == GameState.Pause)
                //instanciates the pause menu
                LevelManager.Instance.Pause();
            
        }
    }


    /// <summary>
    /// uses the gamepad joysticks to call BaseActor movement function
    /// </summary>
    private void GamePadMovement(GamePadState gamePad)
    {
        if (stunned)
            return;

        // if no joysticks are being moved, don't move
        if (gamePad.ThumbSticks.Left.X == 0.0f && gamePad.ThumbSticks.Left.Y == 0.0f)
        {
            animator.SetBool("isWalking", false); // not walking, idle animation
            return;
        }
        
        animator.SetBool("isWalking", true); // is walking, walking animation

        // get the normalized translation vector (direction)
        Vector3 translation = new Vector3(gamePad.ThumbSticks.Left.X, 0, gamePad.ThumbSticks.Left.Y);
        translation.Normalize();

        // x & z translation mapped to horizontal & vertical respectively
        Move(translation.x, translation.z);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gamePad"></param>
    private void GamePadDash(GamePadState gamePad)
    {
        // if A was pressed (not held down)
        if (gamePad.Buttons.A == ButtonState.Pressed && !wasAPressed)
        {
            // don't allow dash if holding a sheep or interacting with a held sheep
            if (HeldSheep || interactionSheep)
                return;

            wasAPressed = true; // A was pressed

            // get the normalized translation vector (direction)
            Vector3 translation = new Vector3(gamePad.ThumbSticks.Left.X, 0, gamePad.ThumbSticks.Left.Y);
            translation.Normalize();

            // x & z translation mapped to horizontal & vertical respectively
            Move(translation.x, translation.z, true);
        }
        else if (gamePad.Buttons.A == ButtonState.Released) 
            wasAPressed = false; // A was released
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
    /// CONTROLLER
    /// lobby menu ready
    /// </summary>
    /// <param name="gamePad"></param>
    private void GamePadReady(GamePadState gamePad)
    {
        //checking to see if the A button was pressed
        if (gamePad.Buttons.A == ButtonState.Pressed)
        {
            //toggles the players status to ready
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
        if (stunned)
            return;
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
