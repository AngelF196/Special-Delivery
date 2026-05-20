using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilExampleScenePortal : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    [SerializeField] private bool disableOnEnter = true;
    [SerializeField] private NeilAnimationTransitionExample SceneTransitionAnimationPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            var playerController = collision.GetComponent<PlayerMove>();
            if(playerController == null)
            {
                Debug.LogError("Fuck you, you forgot to add a player controller to this object you chud");
                return;
            }

            var animation = Instantiate(SceneTransitionAnimationPrefab, Camera.main.transform.position, Quaternion.identity);
            animation.Init(playerController);
            if (disableOnEnter) gameObject.SetActive(false);
        }
    }
}
