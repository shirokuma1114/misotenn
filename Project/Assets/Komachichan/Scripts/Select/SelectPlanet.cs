using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class SelectPlanet : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _cakePrefabs;

    [SerializeField]
    List<GameObject> _currentCakes;

    [SerializeField]
    float _rotateDuration = 1.0f;

    int _currentIndex;

    [SerializeField]
    Ease _inEase;

    [SerializeField]
    Ease _outEase;

    Queue<int> _selectQueue = new Queue<int>();

    bool _isRotate;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void SetIndex(int index)
    {
        _selectQueue.Enqueue(index);
        if (_isRotate) return;
        UpdateRotate();
    }

    private void UpdateRotate()
    {
        _isRotate = false;
        if (_selectQueue.Count == 0) return;
        _currentIndex = _selectQueue.Dequeue();

        transform.DORotate(new Vector3(-180, 0, 0), _rotateDuration * 0.5f, RotateMode.LocalAxisAdd).SetEase(_inEase).OnComplete(ReCreateSelectCake);
        _isRotate = true;
    }

    private void ReCreateSelectCake()
    {
        foreach (var x in _currentCakes) Destroy(x);
        _currentCakes.Clear();

        
        if(_selectQueue.Count > 0)
        {
            _currentIndex = _selectQueue.Last();
            _selectQueue.Clear();
        }

        if(_currentIndex == 0)
        {
            var obj = Instantiate(_cakePrefabs[0], transform);
            obj.transform.localPosition = new Vector3(0.0f, 0.409f, -0.345f);
            obj.transform.Rotate(0.0f, -90.0f, 42.561f);
            _currentCakes.Add(obj);
        }
        if(_currentIndex == 1)
        {
            var obj = Instantiate(_cakePrefabs[1], transform);
            obj.transform.localPosition = new Vector3(0.07f, 0.409f, -0.345f);
            obj.transform.Rotate(-4.592f, -94.23f, 42.73f);
            _currentCakes.Add(obj);
            obj = Instantiate(_cakePrefabs[0], transform);
            obj.transform.localPosition = new Vector3(-0.07f, 0.409f, -0.345f);
            obj.transform.Rotate(4.636f, -85.729f, 42.734f);
            _currentCakes.Add(obj);

        }
        if(_currentIndex == 2)
        {
            var obj = Instantiate(_cakePrefabs[1], transform);
            obj.transform.localPosition = new Vector3(0.0f, 0.409f, -0.345f);
            obj.transform.Rotate(0.0f, -90.0f, 42.561f);
            _currentCakes.Add(obj);

            obj = Instantiate(_cakePrefabs[2], transform);
            obj.transform.localPosition = new Vector3(0.127f, 0.398f, -0.335f);
            obj.transform.Rotate(-9.43f, -98.773f, 43.286f);
            _currentCakes.Add(obj);

            obj = Instantiate(_cakePrefabs[0], transform);
            obj.transform.localPosition = new Vector3(-0.1279f, 0.3988f, -0.3357f);
            obj.transform.Rotate(8.331f, -82.272f, 43.124f);
            _currentCakes.Add(obj);
            
        }
        if(_currentIndex == 3)
        {
            var obj = Instantiate(_cakePrefabs[2], transform);
            obj.transform.localPosition = new Vector3(0.0597f, 0.4071f, -0.3432f);
            obj.transform.Rotate(-5.071f, -94.674f, 42.768f);
            _currentCakes.Add(obj);

            obj = Instantiate(_cakePrefabs[1], transform);
            obj.transform.localPosition = new Vector3(-0.0598f, 0.4082f, -0.3444f);
            obj.transform.Rotate(4.567f, -85.809f, 43.551f);
            _currentCakes.Add(obj);

            obj = Instantiate(_cakePrefabs[3], transform);
            obj.transform.localPosition = new Vector3(0.1744f, 0.3888f, -0.3263f);
            obj.transform.Rotate(-12.484f, -101.73f, 43.848f);
            _currentCakes.Add(obj);

            obj = Instantiate(_cakePrefabs[0], transform);
            obj.transform.localPosition = new Vector3(-0.1765f, 0.3876f, -0.3247f);
            obj.transform.Rotate(13.711f, -77.054f, 44.124f);
            _currentCakes.Add(obj);
            
        }

        transform.DORotate(new Vector3(-180, 0, 0), _rotateDuration * 0.5f, RotateMode.LocalAxisAdd).SetEase(_outEase).OnComplete(UpdateRotate);
    }
}