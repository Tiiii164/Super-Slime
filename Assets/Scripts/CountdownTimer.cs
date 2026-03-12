using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    private float countdownTime = 30f;

    void Start()
    {
        StartCoroutine(Countdown());
    }

    System.Collections.IEnumerator Countdown()
    {
        float timeLeft = countdownTime;
        while (timeLeft > 0)
        {
            Debug.Log("Th?i gian còn l?i: " + timeLeft);
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        EndGame();
    }

    void EndGame()
    {
        Time.timeScale = 0f;

    }
}
