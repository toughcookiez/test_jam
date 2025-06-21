using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButon : MonoBehaviour
{
  
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
