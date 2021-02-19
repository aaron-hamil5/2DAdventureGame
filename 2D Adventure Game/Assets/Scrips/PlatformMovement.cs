using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    #region Variables
    private Transform transformX;
    private Vector3 OriginalPossision = Vector3.zero;
    public Vector3 MoveOnAxis = Vector2.zero;
    public float speed = 3f;
    #endregion

    void Awake()
    {
        #region Save Location
        //Saving the location of the platform
        transformX = GetComponent<Transform>();
        OriginalPossision = transformX.position;
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        //Moving the platform endlessly
        transformX.position = OriginalPossision + MoveOnAxis * Mathf.PingPong(Time.time, speed);
        #endregion
    }
}
