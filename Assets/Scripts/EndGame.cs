using UnityEngine;

public class EndGame : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.GameEnding();
        }
    }
}
