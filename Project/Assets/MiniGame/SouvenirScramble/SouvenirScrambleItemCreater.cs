using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouvenirScrambleItemCreater : MonoBehaviour
{
    private SouvenirScrambleManager _manager;

    private GameObject _earth;

    [SerializeField]
    private List<GameObject> _itemPrefabs;

    [SerializeField]
    private List<GameObject> _itemCreatePositionObjects;
    private List<Vector3> _itemTargetPositions = new List<Vector3>();

    [SerializeField]
    private float _createInterval;
    private float _createIntervalTimer;
    

    void Start()
    {
        _manager = FindObjectOfType<SouvenirScrambleManager>();
        _earth = FindObjectOfType<SouvenirScrambleEarth>().gameObject;

        foreach(var posObj in _itemCreatePositionObjects)
        {
            _itemTargetPositions.Add(posObj.transform.position);
        }

        transform.RotateAround(transform.position, new Vector3(1, 0, 0), 90.0f);
    }

    void Update()
    {
        if (_manager.State != SouvenirScrambleManager.SouvenirScrambleState.PLAY)
            return;

        if(_createIntervalTimer > _createInterval)
        {
            int createNum = (int)_manager.PlayTimeCounter / 10 + 2;
            List<int> usablePosIndex = new List<int>();
            for(int i = 0; i < _itemCreatePositionObjects.Count;i++)
            {
                usablePosIndex.Add(i);
            }

            for(int i = 0; i < createNum; i++)
            {
                int prefabIndex = Random.Range(0, _itemPrefabs.Count);
                int posIndex = usablePosIndex[Random.Range(0, usablePosIndex.Count)];
                usablePosIndex.Remove(posIndex);

                var item = Instantiate(_itemPrefabs[prefabIndex], _itemCreatePositionObjects[posIndex].transform.position, Quaternion.identity);
                item.GetComponent<SouvenirScrambleItem>().Init(_itemTargetPositions[posIndex],_manager);
                item.transform.SetParent(_earth.transform);
            }            

            _createIntervalTimer = 0;
        }

        _createIntervalTimer += Time.deltaTime;
    }
}
