using UnityEngine;
public class RamCharge : StateMachineBehaviour {

    private Transform player;       //Player reference
    private GameObject ram;         //Ram reference
    private Vector3 temp;           //temp for newDir;
    private Vector3 newPos;         //Direction getting
    private Vector3 newDir;
    private float destRad = 1;   //The radius at which the Ram will decide it's within range of it's target
    private GameObject chargeEffect; //The same effect as when the Ram landed

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

        chargeEffect = ram.GetComponent<Ram>().crashEffect;

        //Keep looking at the player. Intimidate them
        ram.transform.LookAt(newDir);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Drawing the path
        Debug.DrawLine(ram.transform.position, newDir);
        ram.GetComponent<Rigidbody>().AddForce(newDir * 50, ForceMode.Acceleration);

        //If the ram is within a reasonable distance, controlled by float destRad above
        if (ram.transform.position.x >= newPos.x - destRad && ram.transform.position.x <= newPos.x + destRad &&
           ram.transform.position.z >= newPos.z - destRad && ram.transform.position.z <= newPos.z + destRad)
        {
            ram.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); //Halt the Ram's movement and momentum
            Instantiate(chargeEffect, new Vector3(ram.transform.position.x, 0, ram.transform.position.z),
                                                         new Quaternion(-0.7071068f, 0, 0, 0.7071068f));
            ram.GetComponent<Animator>().SetBool("isStunned", true);
        }

        //To do list:
        // Halt movement, create "crack" effect on ground for juice
        // Send ram into stunned;
        // Player collision IF player hit        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}