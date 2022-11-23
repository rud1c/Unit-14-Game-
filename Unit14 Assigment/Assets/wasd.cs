using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wasd : MonoBehaviour
{
    public float speed = 4f;
    public float move = 10f;

    // Start is called before the first frame update
    void Start()
    {
    }

    public float jumpAmount = 10;
    public bool isGrounded;


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("space") && isGrounded)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
            isGrounded = false;
        }


        float x = Input.GetAxis("Horizontal");

        float z = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z) * speed * Time.deltaTime;


        if (Input.GetKey(KeyCode.W))
        {
            transform.position += move;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += move;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += move;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += move;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 6f;
        }

        else
        {
            speed = 4f;
        }



    }


    void OnCollisionStay()
    {

        isGrounded = true;

    }

}
