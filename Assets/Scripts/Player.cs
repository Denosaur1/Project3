using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{

    [SerializeField] float speed = 4.0f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float slashForce = 7f;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip slice;
    [SerializeField] AudioClip jump;
    [SerializeField] Camera pCamera;
    private Animator animator;
    private Rigidbody2D body2d;
    private Sensor groundSensor;
    private Sensor orbSensorBR;
    private Sensor orbSensorTR;
    private Sensor orbSensorBL;
    private Sensor orbSensorTL;
    private bool grounded = false;
   
    private int facingDirection = 1;
    
    private float delayToIdle = 0.0f;
    

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor>();
        orbSensorBR = transform.Find("orbSensor_BR").GetComponent<Sensor>();
        orbSensorTR = transform.Find("orbSensor_TR").GetComponent<Sensor>();
        orbSensorBL = transform.Find("orbSensor_BL").GetComponent<Sensor>();
        orbSensorTL = transform.Find("orbSensor_TL").GetComponent<Sensor>();
        
    }

    
    void Update()
    {
        pCamera.transform.position = new Vector3(0, this.transform.position.y, -10);
        

        //Check if character just landed on the ground
        if (!grounded && groundSensor.State() )
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }

        //Check if character just started falling
        if (grounded && !groundSensor.State())
        {
            grounded = false;
            animator.SetBool("Grounded", grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            facingDirection = 1;
        }

        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            facingDirection = -1;
        }


        // Move
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);
        }
        
        //Set AirSpeed in animator
        animator.SetFloat("AirSpeedY", body2d.velocity.y);

        // -- Handle Animations --

        //Attack
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack1");
            source.clip = slice;
            source.Play();
            if (!grounded)
            {
                
                Vector2 slashV;
                if (orbSensorBR.State())
                {
                    slashV = new Vector2(slashForce, -slashForce);
                    body2d.velocity = slashV;
                    //body2d.AddForce(100 * slashV);

                    print("BR");
                }
                else if (orbSensorTR.State())
                {
                    slashV = new Vector2(slashForce, slashForce);
                    body2d.velocity = slashV;
                    //body2d.AddForce(100 * slashV);

                    print("TR");
                }
                else if (orbSensorBL.State())
                {
                    slashV = new Vector2(-slashForce, -slashForce);
                    body2d.velocity = slashV;
                    //body2d.AddForce(100*slashV);

                    print("BL");
                }
                else if (orbSensorTL.State())
                {
                    slashV = new Vector2(-slashForce, slashForce);
                    body2d.velocity = slashV;
                    //body2d.AddForce(100 * slashV);

                    print("TL");
                }



                else
                {
                    print("no object");
                }
            }
        }
            

        

        //Jump
        else if (Input.GetKeyDown("space") && grounded)
        {
            animator.SetTrigger("Jump");
            source.clip = jump;
            source.Play();
            grounded = false;
            animator.SetBool("Grounded", grounded);
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            delayToIdle = 0.05f;
            animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            delayToIdle -= Time.deltaTime;
            if (delayToIdle < 0)
                animator.SetInteger("AnimState", 0);
        }
     
        //visuals for slash
        if (orbSensorBR.State())
        {

        }
        else if (orbSensorTR.State())
        {

        }
        else if (orbSensorBL.State())
        {

        }
        else if (orbSensorTL.State())
        {

        }

        
        


    }
   
}

