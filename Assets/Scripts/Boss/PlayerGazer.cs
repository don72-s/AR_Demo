using UnityEngine;

public class PlayerGazer : MonoBehaviour {

    Transform player;

    private void Awake() {

        player = GameObject.FindWithTag("Player").transform;

    }

    private void Update() {

        transform.LookAt(player);

    }

}
