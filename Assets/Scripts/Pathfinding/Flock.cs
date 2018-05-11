using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    private float _flockSeparationSpeed = 0.01f;
    private GameObject _flockController;
    private FlockController _flockControllerComponent;
    private float _flockSpacing = 2f;
    private float _destSpacing = 1f;
    private int _updateDirection = 0;
    private const float _LOOK_AT_DEVIATION = 0.1f;
    private bool stunned = false;
    private float timeStunned = 0;
    private const float STUN_TIME = 1f;
    private Color origColor;

    private float damage = 0.5f;

    private GraphController _graphController;

	// Use this for initialization
	void Start () {
        _flockController = transform.parent.gameObject;
        _flockControllerComponent = _flockController.GetComponent<FlockController>();
        _graphController = GameObject.FindGameObjectWithTag("GraphController").GetComponent<GraphController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!stunned)
        {
            oldFlock();
        }
        else if (Time.time - timeStunned > STUN_TIME)
        {
            stunned = false;
            gameObject.transform.Find("Sphere").GetComponent<Renderer>().material.color = origColor;
        }
	}

    private void oldFlock()
    {
        Vector3 centerOfFlock = Vector3.zero;

        _flockSeparationSpeed = Random.Range(0.0085f, 0.01f);
        //_flockSpacing = GameObject.FindGameObjectWithTag("FlockSpacingSlider").GetComponent<UI_FlockingSpaceSlider_Controller>().getFlockSpacing();

        if (_updateDirection == 2)
        {
            transform.LookAt(_flockControllerComponent.GetDestinationPosition()
                + new Vector3(Random.Range(-_LOOK_AT_DEVIATION, _LOOK_AT_DEVIATION),
                Random.Range(-_LOOK_AT_DEVIATION, _LOOK_AT_DEVIATION),
                Random.Range(-_LOOK_AT_DEVIATION, _LOOK_AT_DEVIATION)));
            _updateDirection = 0;
        }
        _updateDirection++;

        foreach (GameObject go in _flockControllerComponent.GetFlockArray())
        {
            centerOfFlock += go.transform.position;

            float dist = Vector3.Distance(go.transform.position, transform.position);

            if (dist <= _flockSpacing && _flockControllerComponent.GetFlockArray().Length > 1)
            {
                transform.position += (transform.position - go.transform.position) * _flockSeparationSpeed;

                if (transform.position.y <= 1.0f)
                {
                    transform.position += transform.up * _flockSeparationSpeed * 2f;
                }

            }
        }
            
        if (Vector3.Distance(transform.position, _flockControllerComponent.GetDestinationPosition()) <= _destSpacing)
        {
            Vector3 clampedMovement = Vector3.ClampMagnitude((transform.position - _flockControllerComponent.GetDestinationPosition()), 0.003f);
            transform.position += clampedMovement;
            //GetComponent<Rigidbody>().AddForce(clampedMovement / 4f);
        }
        else
        {
            transform.position += transform.forward * 0.085f;

            if (Vector3.Distance(transform.position, _flockControllerComponent.GetDestinationPosition()) >=
                Vector3.Distance(centerOfFlock, _flockControllerComponent.GetDestinationPosition()))
            {
                //Debug.Log("Speed up");
                transform.position += transform.forward * 0.03f;
                        
            }
            else
            {
                //Debug.Log("Slow down");
                transform.position -= transform.forward * 0.005f;
            }
        }
            
        

        if (Vector3.Distance((centerOfFlock / _flockControllerComponent.GetFlockArray().Length),
            _flockControllerComponent.GetDestinationPosition()) <= _destSpacing + 15f)
        {
            _flockControllerComponent.SetLazyFlightDestinationReached(true);
        }
    }

    public void setStunned(bool set)
    {
        if (!stunned)
        {
            origColor = gameObject.transform.Find("Sphere").GetComponent<Renderer>().material.color;
            gameObject.transform.Find("Sphere").GetComponent<Renderer>().material.color = Color.red;
        }

        stunned = set;
        timeStunned = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            setStunned(true);
            pushObjectBack(collision);
        }
    }

    private void pushObjectBack(Collision collision)
    {
        Vector3 dir = collision.contacts[0].point - transform.position;
        float force = 250f;

        dir = -dir.normalized;
        gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
    }
}
