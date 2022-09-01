using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    // 1
    bool isFiring;
    // 2
    [HideInInspector]
    public Animator animator;
    // 3
    Camera localCamera;
    // 4
    float positiveSlope;
    float negativeSlope;
    // 5
    enum Quadrant
    {
        East,
        South,
        West,
        North
    }

    // 3
    public GameObject ammoPrefab;
    // 4
    static List<GameObject> ammoPool;
    // 5
    public int poolSize;
    public float weaponVelocity;
    // 6
    void Awake()
    {
        // 7
        if (ammoPool == null)
        {
            ammoPool = new List<GameObject>();
        }
        // 8
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // 1
        animator = GetComponent<Animator>();
        // 2
        isFiring = false;
        // 3
        localCamera = Camera.main;

        // 1
        Vector2 lowerLeft = localCamera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 upperRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 upperLeft = localCamera.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 lowerRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        // 2
        positiveSlope = GetSlope(lowerLeft, upperRight);
        negativeSlope = GetSlope(upperLeft, lowerRight);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 1
            isFiring = true;
            FireAmmo();
        }
        // 2
        UpdateState();
    }
    float GetSlope(Vector2 pointOne, Vector2 pointTwo)
    {
        return (pointTwo.y - pointOne.y) / (pointTwo.x - pointOne.x);
    }

    bool HigherThanPositiveSlopeLine(Vector2 inputPosition)
    {
        // 1
        Vector2 playerPosition = gameObject.transform.position;
        // 2
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);
        // 3
        float yIntercept = playerPosition.y - (positiveSlope * playerPosition.x);
        // 4
        float inputIntercept = mousePosition.y - (positiveSlope * mousePosition.x);
        // 5
        return inputIntercept > yIntercept;
    }

    bool HigherThanNegativeSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);
        float yIntercept = playerPosition.y - (negativeSlope * playerPosition.x);
        float inputIntercept = mousePosition.y - (negativeSlope * mousePosition.x);
        return inputIntercept > yIntercept;
    }

    Quadrant GetQuadrant()
    {
        // 2
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerPosition = transform.position;
        // 3
        bool higherThanPositiveSlopeLine = HigherThanPositiveSlopeLine(Input.mousePosition);
        bool higherThanNegativeSlopeLine = HigherThanNegativeSlopeLine(Input.mousePosition);
        // 4
        if (!higherThanPositiveSlopeLine && higherThanNegativeSlopeLine)
        {
            // 5
            return Quadrant.East;
        }
        else if (!higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.South;
        }
        else if (higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.West;
        }
        else
        {
            return Quadrant.North;
        }
    }

    void UpdateState()
    {
        // 1
        if (isFiring)
        {
            // 2
            Vector2 quadrantVector;
            // 3
            Quadrant quadEnum = GetQuadrant();
            // 4
            switch (quadEnum)
            {
                // 5
                case Quadrant.East:
                    quadrantVector = new Vector2(1.0f, 0.0f);
                    break;
                case Quadrant.South:
                    quadrantVector = new Vector2(0.0f, -1.0f);
                    break;
                case Quadrant.West:
                    quadrantVector = new Vector2(-1.0f, 1.0f);
                    break;
                case Quadrant.North:
                    quadrantVector = new Vector2(0.0f, 1.0f);
                    break;
                default:
                    quadrantVector = new Vector2(0.0f, 0.0f);
                    break;
            }
            // 6
            animator.SetBool("isFiring", true);
            // 7
            animator.SetFloat("fireXDir", quadrantVector.x);
            animator.SetFloat("fireYDir", quadrantVector.y);
            // 8
            isFiring = false;
        }
        else
        {
            // 9
            animator.SetBool("isFiring", false);
        }
    }


    // 4
    public GameObject SpawnAmmo(Vector3 location)
    {
        // 1
        foreach (GameObject ammo in ammoPool)
        {
            // 2
            if (ammo.activeSelf == false)
            {
                // 3
                ammo.SetActive(true);
                // 4
                ammo.transform.position = location;
                // 5
                return ammo;
            }
        }
        // 6
        return null;
    }

    // 5
    void FireAmmo()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 2
        GameObject ammo = SpawnAmmo(transform.position);
        // 3
        if (ammo != null)
        {
            // 4
            Arc arcScript = ammo.GetComponent<Arc>();
            // 5
            float travelDuration = 1.0f / weaponVelocity;
            // 6
            StartCoroutine(arcScript.TravelArc(mousePosition, travelDuration));
        }
    }

    void OnDestroy()
    {
        ammoPool = null;
    }
}
