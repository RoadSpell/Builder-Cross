using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingZombie : Character
{
    [SerializeField] internal Transform target;
    //[SerializeField] internal Animator characterAnimatorController;
    [SerializeField] internal BoxCollider followArea;
    //[SerializeField] internal float characterMoveSpeed;

    [SerializeField] internal int x;
    [SerializeField] internal int y;
    [SerializeField] internal int z;

    private void OnEnable()
    {
        CanMove = true;
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
        GetComponent<Rigidbody>().inertiaTensorRotation = Quaternion.identity;
    }


    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Death>().Die();
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        characterAnimatorController.Play("Zombie Running");
    //        transform.LookAt(target);
    //        transform.Rotate(x, y, z);
    //        transform.position = Vector3.MoveTowards(transform.position, target.position, 0.02f);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        characterAnimatorController.Play("Zombie Idle");

    //    }
    //}


}