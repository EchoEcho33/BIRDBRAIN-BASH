using UnityEngine;
public class CameraBallTrack : MonoBehaviour
{
    void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(BallManager.Instance.gameObject.transform.position.x + BallManager.Instance.goingTo.x, -1f, 1f);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 1f);
    }
}
