using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public enum CursorEnum {
        Plant,
        Water,
        Collect
    }

    private Dictionary<CursorEnum, Sprite> cursorSpriteLookup;

    public static CursorController instance = null;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Sprite cursorMainSprite;
    [SerializeField] private Sprite cursorTooFarSprite;

    [SerializeField] private Sprite cursorPlantSprite;
    [SerializeField] private Sprite cursorWaterSprite;
    [SerializeField] private Sprite cursorCollectSprite;


    [SerializeField] private GameObject cursorObject;
    [SerializeField] private float interactionDistance;
    private GameObject cursor;
    private SpriteRenderer cursorSpriteRenderer;
    private Sprite actualCursorSprite;

    public void SetCursor(CursorEnum cursorSprite) {
        actualCursorSprite = cursorSpriteLookup[cursorSprite];
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        actualCursorSprite = cursorMainSprite;
        Cursor.visible = false;
        if (GameObject.Find("Cursor") != null) { cursor = GameObject.Find("Cursor"); return; }
        cursor = Instantiate(cursorObject, (Vector3)offset +
            new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), Quaternion.identity);
        cursorSpriteRenderer = cursor.GetComponent<SpriteRenderer>();
        cursorSpriteRenderer.sprite = actualCursorSprite;
        cursor.name = "Cursor";

        cursorSpriteLookup = new Dictionary<CursorEnum, Sprite>() {
            {CursorEnum.Plant,  cursorPlantSprite},
            {CursorEnum.Water,  cursorWaterSprite},
            {CursorEnum.Collect,  cursorCollectSprite}
        };
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10; ;
        cursor.transform.position = (Vector3)offset + new Vector3(Camera.main.ScreenToWorldPoint(mousePos).x, Camera.main.ScreenToWorldPoint(mousePos).y, 0);
    }
}
