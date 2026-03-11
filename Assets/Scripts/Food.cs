using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Food : MonoBehaviour
{
    public int Size = 1;
    private float moveSpeed = 15f;
    private float shrinkSpeed = 3f;

    public static event Action<Vector3> OnFoodEaten;
    private SphereCollider col;
    private bool isBeingEaten = false;

    void Reset()
    {
        col = GetComponent<SphereCollider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isBeingEaten) return;
        if (!other.CompareTag("Player")) return;

        SlimeController slime = other.GetComponent<SlimeController>();
        if (slime == null) return;

        if (slime.Size >= Size)
        {
            isBeingEaten = true;
            slime.Eat(Size);
            StartCoroutine(HoverToSlime(slime));
            OnFoodEaten?.Invoke(transform.position);
        }
    }

    private System.Collections.IEnumerator HoverToSlime(SlimeController slime)
    {
        Transform target = slime.transform;
        Vector3 startScale = transform.localScale;

        while (Vector3.Distance(transform.position, target.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime
            );

            transform.localScale = Vector3.Lerp(
                transform.localScale,
                Vector3.zero,
                shrinkSpeed * Time.deltaTime
            );

            yield return null;
        }


        Destroy(gameObject);
    }
}
