using UnityEngine;
public class RamCharge : StateMachineBehaviour {

    private Transform player;       //Player reference
    private GameObject ram;         //Ram reference
    private Vector3 temp;           //temp for newDir;
    private Vector3 newPos;         //Direction getting
    private Vector3 newDir;
    private float destRad = 1;   //The radius at which the Ram will decide it's within range of it's target
    private GameObject chargeEffect; //The same effect as when the Ram landed
    private bool charged = false;            //Check that we don't

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Initiliasing our player for easier access throughout this script
        player = animator.GetComponent<Ram>().player.transform;
        ram = animator.gameObject;

        //We don't want the Ram moving with the NavMesh during this charge, as we don't want him avoiding other objects. 
        // He'll simply be charging in a straight line and a navMeshAgent will get in the way of that
        //animator.GetComponent<NavMeshAgent>().enabled = false;
        animator.transform.LookAt(player);

        //(Destination - Current)
        newPos = player.position;
        temp = (player.position - ram.transform.position);
        newDir = temp.normalized;

        chargeEffect = ram.GetComponent<Ram>().chargeEffect;

        //Keep looking at the player. Intimidate them
        ram.transform.LookAt(newDir);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Drawing the path
        Debug.DrawLine(ram.transform.position, newDir);

        ram.GetComponent<Rigidbody>().AddForce(newDir * 50, ForceMode.Acceleration);        //Ram charging

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
        //If the ram is within a reasonable distance, controlled by float destRad above
        /*else*/
        if (ram.transform.position.x >= newPos.x - destRad && ram.transform.position.x <= newPos.x + destRad &&
           ram.transform.position.z >= newPos.z - destRad && ram.transform.position.z <= newPos.z + destRad)
        {
            PlayerMissed();
        }
    }

    //WHEN WE EXIT THIS STATE TO ENTER EITHER STUNNED OR WANDER (Depends on whether we've missed the player or not)
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charged = false;    //Simple boolean to ensure we don't instantiate the same crash effect more than once during update

        //Now that our charge is complete, regardless of whether we hit our player or not, we want to turn off our hitCollider
        // so that we can't interact with the player anymore, and we want to reenable our charge sphere so that we can start
        // looking for more daring players who wander too close
        ram.GetComponent<Ram>().hitCollider.enabled = false;
        ram.GetComponent<Ram>().chargeSphere.enabled = true;
    }


    //If the Ram hits the player during it's charge, run this function
    private void PlayerHit()
    {
        //We stun the player through baseActor's functionality, then we activate it again based on a timer in the base Ram class
        player.GetComponent<Player>().isStunned = true;  //Set this to true so that the Ram knows whether or not to ignore it
        player.GetComponent<BaseActor>().stunned = true;

        ram.GetComponent<Ram>().playerHit = false;  //Reset so that we don't keep stunning the player
        ram.GetComponent<Ram>().player = null;      //Reset so that we don't keep charging the player

        //When the Ram hit's a player, we want it to immediately start wandering again
        ram.GetComponent<Animator>().SetBool("isWandering", true);
    }

    //Else if the Ram misses the player during it's charge, run this function
    private void PlayerMissed()
    {
        ram.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); //Halt the Ram's movement and momentum

        //bool check as we only want to create the cracks on the ground once - when the ram hits. 
        if (!charged)
        {
            Instantiate(chargeEffect, new Vector3(ram.transform.position.x, 0, ram.transform.position.z),
/*THESE VALUES NEED TO CHANGE TO FIX ROTATION -->*/  new Quaternion(-0.7035975f, 0.09950372f, 0, 0.7035975f));
            charged = true;
        }

        //Change state to continue the behavioural lööp, bröther
        ram.GetComponent<Animator>().SetBool("isStunned", true);
    }

    //On the more than likely chance that a sheep happens to get in the way of the Ram
    private void SheepHit()
    {

    }
}