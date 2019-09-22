using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rigidbody;
    private Vector2 moveVelocity;
    private int dashingTime;
    private bool dashReady;
    private float stamina;
    private float staminaRecoveryTime;
    private float staminaRecoveryRateStandingStill;
    private float staminaRecoveryRateMoving;
    private float lastRecoveryTime;
    private float dashStaminaConsumption;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        dashingTime = 0;
        dashReady = true;
        stamina = 100;
        staminaRecoveryTime = 1;
        staminaRecoveryRateStandingStill = 25;
        lastRecoveryTime = 0;
        dashStaminaConsumption = 20;
        staminaRecoveryRateMoving = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (dashingTime == 0 && Input.GetKeyUp(KeyCode.LeftShift))
        {
            dashReady = true;
        }
        if (Input.GetKey(KeyCode.LeftShift) && dashingTime == 0 && dashReady && stamina > dashStaminaConsumption)
        {
            dashingTime = 10;
            dashReady = false;
            stamina -= dashStaminaConsumption;
        } 
        

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        moveVelocity = moveInput.normalized * speed;
        if (dashingTime > 0)
        {
            moveVelocity *= 5;
            dashingTime--;
        }
    }

    void FixedUpdate()
    {
        
        rigidbody.MovePosition(rigidbody.position + moveVelocity * Time.fixedDeltaTime);
        Debug.Log(lastRecoveryTime);
        if(Time.fixedTime  > lastRecoveryTime - staminaRecoveryTime && stamina < 100)
        {
            Debug.Log("ENTERED TIME CHECK AND STAMINA");
            lastRecoveryTime = Time.fixedTime;
            if (moveVelocity == Vector2.zero)
            {
                Debug.Log("ENTERED VELOCITY CHECK");
                if (stamina > (100 - Time.fixedDeltaTime * staminaRecoveryRateStandingStill))
                {
                    Debug.Log("RECOVERING TO 100");
                    stamina = 100;
                }
                else
                {
                    Debug.Log("INC STAMINA");
                    stamina += staminaRecoveryRateStandingStill * Time.fixedDeltaTime;
                }
            }
            else
            {
                if (stamina > (100 - Time.fixedDeltaTime * staminaRecoveryRateMoving))
                {
                    Debug.Log("RECOVERING TO 100");
                    stamina = 100;
                }
                else
                {
                    Debug.Log("INC STAMINA");
                    stamina += staminaRecoveryRateMoving * Time.fixedDeltaTime;
                }
            }
        }
    }
}
