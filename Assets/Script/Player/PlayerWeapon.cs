using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject prefabBullet;
    public GameObject pointShot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            Instantiate(prefabBullet,pointShot.transform.position,transform.rotation);

        }
    }
}
