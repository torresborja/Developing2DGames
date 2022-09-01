using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TravelArc(Vector3 destination, float duration)
    {
        // 3
        var startPosition = transform.position;
        // 4
        var percentComplete = 0.0f;
        // 5
        //while (percentComplete < 1.0f)
        //{
        //    // 6
        //    percentComplete += Time.deltaTime / duration;
        //    // 7
        //    transform.position = Vector3.Lerp(startPosition, destination, percentComplete);
        //    // 8
        //    yield return null;
        //}
        while (percentComplete < 1.0f)
        {
            // Leave this existing line alone.
            percentComplete += Time.deltaTime / duration;
            // 1
            var currentHeight = Mathf.Sin(Mathf.PI * percentComplete);
            // 2
            transform.position = Vector3.Lerp(startPosition, destination, percentComplete) + Vector3.up *  currentHeight;
            // Leave these existing lines alone.
            percentComplete += Time.deltaTime / duration;
            yield return null;
        }


        // 9
        gameObject.SetActive(false);
    }
}
