using UnityEngine;

public class Player : MonoBehaviour {

    private void OnCollisionEnter(Collision collision) {

        Debug.Log("�浹");

    }


    private void OnTriggerEnter(Collider other) {

        Debug.Log("�浹2");

    }

}
