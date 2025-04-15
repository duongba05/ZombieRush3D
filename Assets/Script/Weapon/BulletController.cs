using System.Runtime.CompilerServices;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
