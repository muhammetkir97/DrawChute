using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DrawZoneController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Draw Settings")]
    [SerializeField] private float minPointDistance;
    [Header("Draw Points")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform pointPool;
    [SerializeField] private RectTransform pointObject;
    [SerializeField] private int initialPoolSize;
    private int currentPointCount = 0;
    private List<Vector2> pointPositionsList;
    private Vector2 lastPosition = Vector2.positiveInfinity;
    public UnityAction<Vector2[]> onLineDrawingEnd;

    public void OnPointerDown(PointerEventData _eventData)
    {
        AddPointToDrawZone(_eventData.position);
    }

    public void OnDrag(PointerEventData _eventData)
    {
        AddPointToDrawZone(_eventData.position);
    }

    public void OnPointerUp(PointerEventData _eventData)
    {
        if(onLineDrawingEnd != null) onLineDrawingEnd.Invoke(pointPositionsList.ToArray());
        
        DeleteLine();
    }

    
    private void Awake() 
    {
        pointPositionsList = new List<Vector2>();
        InitializePointPool(initialPoolSize);
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddPointToDrawZone(Vector2 _pos)
    {
        float _distance = Vector2.Distance(lastPosition, _pos);
        if(_distance < minPointDistance * canvas.scaleFactor ) return;

        GameObject _pointObj = pointPool.GetChild(currentPointCount).gameObject;
        _pointObj.SetActive(true);
        _pointObj.GetComponent<RectTransform>().position = _pos;

        if(currentPointCount > 0)
        {
            RectTransform _line = _pointObj.transform.GetChild(0).GetComponent<RectTransform>();
            _line.sizeDelta = new Vector2(_distance / canvas.scaleFactor, _line.sizeDelta.y);
            _line.right = lastPosition - _pos;
            //_line.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(lastPosition, _pos) * Mathf.Rad2Deg);
        }
        
        pointPositionsList.Add(_pos);
        lastPosition = _pos;
        currentPointCount++;
    }

    public void DeleteLine()
    {
        for(int i=0; i<currentPointCount; i++)
        {
            GameObject _pointObj = pointPool.GetChild(i).gameObject;
            _pointObj.SetActive(false);
            RectTransform _line = _pointObj.transform.GetChild(0).GetComponent<RectTransform>();
            _line.sizeDelta = new Vector2(0, _line.sizeDelta.y);
        }

        pointPositionsList.Clear();
        Vector2 lastPosition = Vector2.positiveInfinity;
        currentPointCount = 0;
    }

    public void InitializePointPool(int _size)
    {
        pointObject.gameObject.SetActive(false);

        for(int i=0; i<_size; i++)
        {
            Instantiate(pointObject, pointPool.transform);
        }
    }
}
