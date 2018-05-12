using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapProperties : MonoBehaviour
{
    public static string TAG = "MapProperties";

    [Tooltip("Set the horizontal scale for imported data. By default the horizotnal scale is 1:1, if you need for example scale 1:1000, set this parameter to 1000.")]
    public float HorizontalScale;
    [Tooltip("Set the vertical scale for imported data. By default the scale is 1:1, if you need for example scale 1:10, set this parameter to 10.")]
    public float VerticalScale;
    [Tooltip("Origin location for the camera in world coordinates (Web Mercator). This point is used to reduce the coordinates of all map objects.")]
    public Vector3 Origin;
}
