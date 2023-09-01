using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class StartupScript : MonoBehaviour
{
    [SerializeField]
    RectangleGenerator generator;

    void Start ()
    {
        generator.GenerateTiles ();
    }
}
