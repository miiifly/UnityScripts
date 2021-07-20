using UnityEngine;

public class GameManager : MonoBehaviour
{   private bool chek = true;
    [Header("Unity Time speed (1 = realtime)")]
    [SerializeField]
    private float _timeScaleMod = 3.0f;
    [SerializeField]
    private float _timeScaleInfo;

    [Header("Scaling options")]
    [SerializeField]
    [Tooltip("1 unit = SpaceScale kilometers")]
    private float _spaceScale = 6000;

    [SerializeField]
    [Tooltip("1 unit = TimeScale seconds")]
    private float _timeScale = 100; //1 unity unit = timeScale seconds
    [SerializeField]
    [Tooltip("Fixed update loop time (default = 0.02)")]
    private float _modifiedFixedDeltaTime = 0.02f;//Default

    public float SpaceScaleMeters
    {
        get { return _spaceScale * 1000; }
    }
    public float SpaceScaleKm
    {
        get { return _spaceScale; }
    }

    public float TimeScale
    {
        get { return _timeScale; }
    }
    public float ModifiedDeltaTime
    {
        get { return _modifiedFixedDeltaTime; }
    }

    private void Awake()
    {
        Time.fixedDeltaTime = ModifiedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeScale();
    }

    private void UpdateTimeScale()
    {

        if (Input.GetKeyDown(KeyCode.W) && chek== true)
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale + _timeScaleMod, 1.0f, 99);
            _timeScaleInfo = Time.timeScale;
            chek = false;
        }

        if (Input.GetKeyDown(KeyCode.S) )
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale - _timeScaleMod, 1.0f, 99);
            _timeScaleInfo = Time.timeScale;
            chek = true;
        }
    }
}