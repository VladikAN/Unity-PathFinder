using UnityEngine;

public class CoinComponent : MonoBehaviour
{
    private float _timeout = 0;
    private MeshRenderer _meshRenderer;

    public void Start()
    {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        transform.Rotate(Vector3.up, Random.Range(0, 180));
    }

	public void Update()
	{
	    if (_timeout > 0)
	    {
	        _timeout -= Time.deltaTime;
	        if (_timeout <= 0)
	        {
	            _meshRenderer.enabled = true;
	        }

	        return;
	    }

        transform.RotateAround(transform.position, Vector3.up, 360 * Time.deltaTime);
	}

    public void OnTriggerEnter(Collider target)
    {
        if (_timeout > 0) return;

        if (target.gameObject.CompareTag("Player"))
        {
            _meshRenderer.enabled = false;
            _timeout = 5f;
        }
    }
}