using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HormingGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _generate;

    void Start()
    {
        foreach (var tama in _generate)
        {
            tama.SetActive(false);
        }
    } 
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach(var tama in _generate)
            {
                tama.transform.position = transform.position;
                tama.SetActive(true);
                tama.GetComponent<HomingLaser>().SetInit();
            }
        }
    }
}
