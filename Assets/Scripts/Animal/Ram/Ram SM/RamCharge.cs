using UnityEngine;
public class RamCharge : StateMachineBehaviour {

    private Transform player;       //Player reference
    private GameObject ram;         //Ram reference
    private Vector3 temp;           //temp for newDir;
    private Vector3 newPos;         //Direction getting
    private Vector3 newDir;
    private GameObject chargeEffect; //The same effect as when the Ram landed
    private GameObject sceneChargeEffect;    //Where we store the charge effect that we've created
    private GameObject sceneStunnedEffect;   //Where we store the stunned effect that we've created
    private bool charged = false;            //Check that we don't
    private float forceOut = 0f;             //If the ram gets stuck in this state, this will force it out

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Initiliasing our player for easier access throughout this script
        player = animator.GetComponent<Ram>().player.transform;
        ram = animator.gameObject;
        //Whilst the Ram is charging, we don't want it turning, we want it to continuously face the direction it's charging.
        ram.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        //(Destination - Current)
        newPos = player.position;
        temp = (player.position - ram.transform.position);
        newDir = temp.normalized;
        ram.GetComponent<Rigidbody>().isKinematic = false;

        chargeEffect = ram.GetComponent<Ram>().chargeEffect;

        if(sceneChargeEffect != null)
        {
            Destroy(sceneChargeEffect);     //Everytime we enter this state, we want to remove the leftover charge effect
            //Stops us from clogging up the scene with inactive particle systems
        }

        if(sceneStunnedEffect != null)
        {
            Destroy(sceneStunnedEffect);    //Everytime we enter this state, we want to remove the leftover stunned effect
            //Stops us from clogging up the scene with inactive particle systems
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        forceOut += Time.deltaTime;
        
        ram.GetComponent<Rigidbody>().AddForce(newDir * 50, ForceMode.Acceleration);      //Ram charging        

        //*****************************************************************************************************
        //                              IF THE RAM HITS THE PLAYER
        //*****************************************************************************************************
        if (ram.GetComponent<Ram>().playerHit == true)
        {
            PlayerHit();
        }


        //*****************************************************************************************************
        //                              IF THE RAM MISSES THE PLAYER
        //*****************************************************************************************************
        else if (ram.GetComponent<Ram>().boundaryHit == true)
        {
            PlayerMissed();
        }

        //If for some reason, the ram gets stuck in this state, we have a timer that will force it back into the loop.
        if (forceOut >= 4)
        {
            forceOut = 0;

            //Before the Ram leaves this state, we want to reset it's velocity, bringing it to a halt. 
            // this should fix the error where the Ram constantly builds up momentum and starts "sliding" around the scene
            ram.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            ram.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            ram.GetComponent<Animator>().SetBool("isWandering", true);
        }
    }

    //WHEN WE EXIT THIS STATE TO ENTER EITHER STUNNED OR WANDER (Depends on whether we've missed the player or not)
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charged = false;    //Simple boolean to ensure we don't instantiate the same crash effect more than once during update

        ram.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        //Now that our charge is complete, regardless of whether we hit our player or not, we want to turn off our hitCollider
        // so that we can't interact with the player anymore, and we want to reenable our charge sphere so that we can start
        // looking for more daring players who wander too close
        ram.GetComponent<Ram>().hitCollider.enabled = false;
        ram.GetComponent<Ram>().chargeSphere.enabled = true;

        //Reset our boolean value so that we don't immediately charge the next time we enter the StepBack state
        ram.GetComponent<Animator>().SetBool("isCharging", false);
    }


    //If the Ram hits the player during it's charge, run this function
    private void PlayerHit()
    {
        //We stun the player through baseActor's functionality, then we activate it again based on a timer in the base Ram class
        //player.GetComponent<Player>().isStunned = true;  //Set this to true so that the Ram knows whether or not to ignore it
        player.GetComponent<BaseActor>().stunned = true;

        //Now that we've hit and stunned a player, throw up the stunned particle effectss to convey this to the player
        sceneStunnedEffect = Instantiate(ram.GetComponent<Ram>().stunnedEffect, player.transform);

        if (ram.GetComponent<Ram>().playerHit == true)
        ram.GetComponent<Ram>().playerHit = false;    //Reset so that we don't keep stunning the player
        //ram.GetComponent<Ram>().player = null;      //Reset so that we don't keep charging the player

        //Before the Ram leaves this state, we want to reset it's velocity, bringing it to a halt. 
        // this should fix the error where the Ram constantly builds up momentum and starts "sliding" around the scene
        ram.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        //When the Ram hit's a player, we want it to immediately start wandering again
        ram.GetComponent<Animator>().SetBool("isWandering", true);
    }

    //Else if the Ram misses the player during it's charge, run this function
    private void PlayerMissed()
    {
        ram.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); //Halt the Ram's movement and momentum
        ram.GetComponent<Ram>().player = player.gameObject;

        //bool check as we only want to create the cracks on the ground once - when the ram hits. 
        if (!charged)
        {
            //Creating the "crack" effect on the ground when we miss, we raise this by 0.1 on the y-axis so that it doesn't share
            // the same plane as the ground and give a glitchy, flashing effect.
            sceneChargeEffect = Instantiate(chargeEffect, new Vector3(ram.transform.position.x, 0.1f, ram.transform.position.z),
                                                          new Quaternion(-0.7071068f, 0, 0, 0.7071068f));
            charged = true;
        }

        ram.GetComponent<Ram>().boundaryHit = false;    //We reset this to false when we leave

        //Before the Ram leaves this state, we want to reset it's velocity, bringing it to a halt. 
        // this should fix the error where the Ram constantly builds up momentum and starts "sliding" around the scene
        ram.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        //Change state to continue the behavioural lööp, bröther
        ram.GetComponent<Animator>().SetBool("isStunned", true);
    }

    //On the more than likely chance that a sheep happens to get in the way of the Ram
    private void SheepHit()
    {

    }
}