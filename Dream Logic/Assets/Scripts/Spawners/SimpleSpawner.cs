using UnityEngine;

public class SimpleSpawner : MonoBehaviour
{
    private PlayerController player;

    [SerializeField]
    private Transform objectParent;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float minDistance;
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private float height;

    [SerializeField]
    private float frequency;
    private float counter;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        counter += Time.deltaTime;

        if (counter >= frequency)
        {
            counter = 0f;

            Vector2 circleOffset = Random.insideUnitCircle.normalized * Random.Range(minDistance, maxDistance);
            Vector3 fallOffset = player.tr.forward * Mathf.Sqrt(2f * height * player.speed / Physics.gravity.magnitude);
            Vector3 position = player.tr.position + fallOffset + new Vector3(circleOffset.x, height, circleOffset.y);

            Instantiate(prefab, position, Quaternion.identity, objectParent);
        }
    }
}
