using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {

    #region Sheep Properties
    GameObject fieldObject;     //Reference to the field, so that we can find a new position relative to it's dimensions
    private float fieldPosX = 0;
    private float fieldPosZ = 0;

    [SerializeField]
    private bool isGoldSheep = false;
    public bool IsGoldSheep { get { return isGoldSheep; } }

    [Tooltip("How fast the sheep will move towards it's destination")]
    public float speed;
    public float radius;
    public int score;
    [Range(0, 1.0f)]
    public float speedModifier;
    private bool isPaused;
    private Vector3 pauseVelocity;

    public float distToGround;                  //Variable used for IsGrounded();
    [Tooltip("The range of force that the sheep will be moved at when spawned")]
    public Vector2 spawnRangeForce;

    private GameObject[] fieldBox;
    private int previousNum;
    private int rand;

    public string currentBehaviour;

    public Animator animAnim;
    private Animator sheepAnim;

    [SerializeField]
    private AudioClip[] sheepSFX;
    private AudioClip prevSFX;

    [HideInInspector]
    public GameObject lastPlayer;
    #endregion

    void Start()
    {
        AudioClip randomSFX = sheepSFX[Random.Range(0, sheepSFX.Length - 1)];
        int diceRoll = Random.Range(0, 6);

        if (diceRoll < 2)
        {
            while (randomSFX == prevSFX)
                randomSFX = sheepSFX[Random.Range(0, sheepSFX.Length - 1)];

            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = randomSFX;
            audioSource.Play();
            prevSFX = randomSFX;
        }

        //Creating the FieldBox array;
        fieldBox = new GameObject[4];

        //Initialising each element of our field array to the 4 field objects in our scene
        fieldBox[0] = GameObject.Find("FieldTop");
        fieldBox[1] = GameObject.Find("FieldRight");
        fieldBox[2] = GameObject.Find("FieldBottom");
        fieldBox[3] = GameObject.Find("FieldLeft");

        sheepAnim = GetComponent<Animator>();
    }

    void Update()
    {
        //if the sheep's y axis reaches too far into the negatives (meaning that it's fallen off the edge or through teh floor), 
        // put it back into the object pool

        if (transform.position.y < -1)
            SheepManager.Instance.DestroySheep(gameObject);

        if (Time.timeScale == 0 && isPaused == false)
        {
            isPaused = true;
            pauseVelocity = GetComponent<Rigidbody>().velocity;
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }

        if (isPaused == true && Time.timeScale != 0)
        {
            GetComponent<Rigidbody>().velocity = pauseVelocity;
            isPaused = false;
        }
    }

    #region Set Sheep Behaviours Functions
    public void SetWanderTrue()
    {
        sheepAnim.SetBool("isWandering", true);
        sheepAnim.SetBool("isKicked", false);
        sheepAnim.SetBool("isPushed", false);
    }

    public void SetKickedTrue()
    {
        sheepAnim.SetBool("isKicked", true);
        sheepAnim.SetBool("isWandering", false);
        sheepAnim.SetBool("isPushed", false);
    }

    public void SetPushedTrue()
    {
        sheepAnim.SetBool("isPushed", true);
        sheepAnim.SetBool("isWandering", false);
        sheepAnim.SetBool("isKicked", false);
    }

    public void SetIdleTrue()
    {
        sheepAnim.SetBool("isPushed", false);
        sheepAnim.SetBool("isWandering", false);
        sheepAnim.SetBool("isKicked", false);
    }
    #endregion

    public bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, distToGround))
            return true;

        else return false;
    }

    //Function to pull a new random position within the confines of the play field
    public Vector3 GetNewDestination()
    {
        Vector3 newPos = new Vector3();

        //Randomly select one of the 4 fieldBoxes in the scene
        rand = Random.Range(0, 4);

        //We don't want to seek into the same box
        if (rand == previousNum)
            rand = Random.Range(0, 4);

        previousNum = rand;

        //Randomly get an x and z position within that field for our Object to seek to
        newPos.x = Random.Range(fieldBox[rand].transform.position.x - fieldBox[rand].transform.localScale.x / 2f,
                                fieldBox[rand].transform.position.x + fieldBox[rand].transform.localScale.x / 2f);

        newPos.z = Random.Range(fieldBox[rand].transform.position.z - fieldBox[rand].transform.localScale.z / 2f,
                                fieldBox[rand].transform.position.z + fieldBox[rand].transform.localScale.z / 2f);

        return newPos;
    }

    //Function checks if a sheep has moved outside of the given play field area
    public bool CheckOutsideField(GameObject sheep)
    {
        if (sheep.transform.position.x >= fieldPosX + fieldObject.transform.localScale.x / 2.5f ||
            sheep.transform.position.x <= fieldPosX - fieldObject.transform.localScale.x / 2.5f ||
            sheep.transform.position.z >= fieldPosZ + fieldObject.transform.localScale.z / 2.5f ||
            sheep.transform.position.z <= fieldPosZ - fieldObject.transform.localScale.z / 2.5f)
        {
            return true;
        }

        else return false;
    }
}