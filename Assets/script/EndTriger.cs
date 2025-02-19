using UnityEngine;

public class EndTriger : MonoBehaviour
{
    public NewMonoBehaviourScript gameManager;
    void OnTriggerEnter()  
    {
        gameManager.Complete();
    }
}
