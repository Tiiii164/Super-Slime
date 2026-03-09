using UnityEngine;

public class Food : MonoBehaviour
{
    public int Size = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        SlimeController slime = other.GetComponent<SlimeController>();

        if (slime == null) return;

        if (slime.Size >= Size)
        {
            slime.Grow(Size);
            gameObject.SetActive(false);
        }
    }
}