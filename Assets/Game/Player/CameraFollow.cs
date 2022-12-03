using System.Threading.Tasks;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector2 targetPosition;

    public static CameraFollow inst;

    public async void Shake(float duration, float amplitude, bool descending)
    {
        Timer timer = new Timer(duration);
        while (!timer.ExecuteTimer())
        {
            Vector3 offset = new Vector3(Random.value * 2 - 1, Random.value * 2 - 1, 0) * amplitude;
            if (descending)
            {
                offset *= timer.GetTimeLeft() / duration;
            }
            transform.position += offset;
            await Task.Yield();
        }
    }

    private void Update()
    {
        targetPosition = Vector2.Lerp(transform.position, Player.inst.transform.position, Time.deltaTime * speed);
        transform.position = new Vector3(targetPosition.x, targetPosition.y, -10f);
    }

    private void Awake()
    {

        if (inst != null && inst != this)
        {
            Destroy(this);
        }
        else
        {
            inst = this;
        }
    }
}
