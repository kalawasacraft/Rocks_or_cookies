using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateRocks : MonoBehaviour
{
    public static PopulateRocks Instance;
    
    public List<GameObject> prefabs;
    public int quadrantSide;
    public Vector2 RangeSides;
    public Vector2 RangeScales;
    public Vector2Int RangePoints;

    private HashSet<Vector2Int> _points;
    private HashSet<Vector2Int> _centers;
    private Vector2Int _mainCenter;

    void Awake()
    {
        Instance = this;

        _points = new HashSet<Vector2Int>();
        _centers = new HashSet<Vector2Int>();
    }

    void Start()
    {
        _mainCenter = new Vector2Int(0, 0);

        for (int i = _mainCenter.y - 1; i <= _mainCenter.y + 1; i++) {
            PopulationPoint(new Vector2Int(_mainCenter.x - 1, i));
            PopulationPoint(new Vector2Int(_mainCenter.x + 1, i));
        }

        PopulationPoint(new Vector2Int(_mainCenter.x, _mainCenter.y + 1));
        PopulationPoint(new Vector2Int(_mainCenter.x, _mainCenter.y - 1));

        MapInCenter(_mainCenter);
    }

    void Update()
    {
        Vector2 centerPositionCam = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/2, Screen.height/2));
        Vector2Int currentCenter = new Vector2Int(ValuePositionToPoint(centerPositionCam.x), ValuePositionToPoint(centerPositionCam.y));

        if (!_mainCenter.Equals(currentCenter)) {
            MapInCenter(currentCenter);
        }
    }
    
    private int ValuePositionToPoint(float value)
    {
        float p = value + quadrantSide / 2;
        return ((int) (p / quadrantSide)) + (p >= 0 ? 0 : -1);
    }

    private void MapInCenter(Vector2Int center)
    {
        _mainCenter = center;
        if (!_centers.Contains(_mainCenter)) {
            _centers.Add(_mainCenter);

            Population();
        }
    }

    private void Population()
    {
        for (int i = _mainCenter.x - 2; i <= _mainCenter.x + 2; i++) {
            PopulationPoint(new Vector2Int(i, _mainCenter.y + 2));
            PopulationPoint(new Vector2Int(i, _mainCenter.y - 2));
        }

        for (int i = _mainCenter.y - 1; i <= _mainCenter.y + 1; i++) {
            PopulationPoint(new Vector2Int(_mainCenter.x - 2, i));
            PopulationPoint(new Vector2Int(_mainCenter.x + 2, i));
        }
    }

    private void PopulationPoint(Vector2Int point)
    {
        if (!_points.Contains(point)) {

            int type = Random.Range(0, prefabs.Count);
            float scale, side;

            if (type == 0) {
                scale = ((int) (Random.Range(RangeScales.x, RangeScales.y) * 10)) / 10f;
                side = RangeSides.y - (((RangeSides.y - RangeSides.x) * (RangeScales.y - scale)) / (RangeScales.y - RangeScales.x));
            } else {
                scale = RangeScales.x;
                side = RangeSides.x;
            }

            Vector2 rPoint = new Vector2(point.x * quadrantSide, point.y * quadrantSide);
            Vector2 position = new Vector2(Random.Range(rPoint.x - quadrantSide / 2 + side, rPoint.x + quadrantSide / 2 - side), 
                                            Random.Range(rPoint.y - quadrantSide / 2 + side, rPoint.y + quadrantSide / 2 - side));

            if (type == 0) {
                GameObject rock = Instantiate(prefabs[type], position, Quaternion.identity, this.transform);
                rock.GetComponent<RockController>().Init(scale, 2 * Random.Range(RangePoints.x, RangePoints.y + 1));
            } else {
                Instantiate(prefabs[type], position, Quaternion.identity, this.transform);
            }

            _points.Add(point);
        }
    }
}
