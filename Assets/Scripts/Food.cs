using System;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int Size = 1;

    public static event Action<Vector3> OnFoodEaten;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        SlimeController slime = other.GetComponent<SlimeController>();

        if (slime == null) return;

        if (slime.Size >= Size)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            OnFoodEaten?.Invoke(spawnPosition);
            slime.Eat(Size);
            gameObject.SetActive(false);
        }
    }
}