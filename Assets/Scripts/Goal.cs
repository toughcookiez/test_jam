using System.Collections;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private int _targetSceneIndex;


    [SerializeField] private float _timeBeforeTransition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(RevealRoom(collision));
        }
    }

    private IEnumerator RevealRoom(Collider2D collision)
    {
        StartCoroutine(collision.GetComponent<PlayerController>().DisapplyShadows());
        yield return new WaitForSeconds(_timeBeforeTransition);
        LevelManager.Instance.NextLevel(_targetSceneIndex);
    }
}
