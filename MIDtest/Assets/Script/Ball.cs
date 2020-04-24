using UnityEngine;

public class Ball : MonoBehaviour
{/// <summary>
/// 足球是否進入球門
/// </summary>
    public static bool conplate;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Wall" )
        {
            conplate = true;
        }
    }
}