using UnityEngine;

public class CoinComponent : MonoBehaviour
{
    private float _timeout = 0;
    private MeshRenderer _meshRenderer;

    public void Start()
    {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
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
	    }
	    else
	    {
            transform.RotateAround(transform.position, Vector3.up, 180 * Time.deltaTime);
        }
	}

    public void OnTriggerEnter(Collider target)
    {
        if (_timeout > 0) return;

        if (target.gameObject.CompareTag("Player"))
        {
            _meshRenderer.enabled = false;
            _timeout = 6f;
        }
    }
}