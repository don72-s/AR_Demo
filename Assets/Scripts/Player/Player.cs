using UnityEngine;

public class Player : MonoBehaviour {

    private void OnCollisionEnter(Collision collision) {

        Debug.Log("충돌");

    }


    private void OnTriggerEnter(Collider other) {

        Debug.Log("충돌2");

    }

}
