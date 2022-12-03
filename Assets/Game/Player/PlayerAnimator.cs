using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpriteDir
{
    Front,
    Back,
    Right,
    Left,
    Idle
}

public struct SpriteDirData
{
    public SpriteDir dir;
    public int index;
    public SpriteDirData(SpriteDir dir, int index)
    {
        this.dir = dir;
        this.index = index;
    }
}

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private int fps;

    [SerializeField] private List<Sprite> front = new List<Sprite>();
    [SerializeField] private List<Sprite> back = new List<Sprite>();
    [SerializeField] private List<Sprite> left = new List<Sprite>();
    [SerializeField] private List<Sprite> right = new List<Sprite>();
    [SerializeField] private List<Sprite> idle = new List<Sprite>();

    private SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();
    private int frame = 0, maxValue = 0;
    private SpriteDirData data;

    public void AnimateWalk(Vector2 movementDirection) {
        int dx = movementDirection.x > 0 ? 1 : -1;
        if (Mathf.Abs(movementDirection.x) < 0.1f) dx = 0;
        int dy = movementDirection.y > 0 ? 1 : -1;
        if (Mathf.Abs(movementDirection.y) < 0.1f) dy = 0;

        data = GenerateSpriteDirData(dx, dy);
        maxValue = data.dir == SpriteDir.Back ? back.Count : data.dir == SpriteDir.Right ? right.Count : data.dir == SpriteDir.Left ? left.Count :  data.dir == SpriteDir.Idle ? idle.Count : front.Count;
        if (dx == 0 && dy == 0)
        {
            //Go idle
            data.index = 0;
            data.dir = SpriteDir.Idle;
            maxValue = idle.Count;
        }
        data.index = frame % maxValue;
        SetSprite(data);
    }

    private void SetSprite(SpriteDirData data)
    {
        
        switch (data.dir)
        {
            case SpriteDir.Front:
                spriteRenderer.sprite = front[data.index];
                break;
            case SpriteDir.Back:
                spriteRenderer.sprite = back[data.index];
                break;
            case SpriteDir.Right:
                spriteRenderer.sprite = right[data.index];
                break;
            case SpriteDir.Left:
                spriteRenderer.sprite = left[data.index];
                break;
            case SpriteDir.Idle:
                spriteRenderer.sprite = idle[data.index];
                break;
        }
    }
    private SpriteDirData GenerateSpriteDirData(int dx, int dy)
    {
        if (dy == 1)
        {
            return new SpriteDirData(SpriteDir.Back, 0);
        }
        if (dx == 1)
        {
            return new SpriteDirData(SpriteDir.Right, 0);
        }
        if (dx == -1) {
            return new SpriteDirData(SpriteDir.Left, 0);
        }
        if (dy == -1)
        {
            return new SpriteDirData(SpriteDir.Front, 0);
        }
        return data;
    }

    private IEnumerator FramerateRoutine()
    {
        Timer timer = new Timer();
        timer.SetTimerFrequency(fps);
        while (!timer.ExecuteTimer())
        {
            yield return null;
        }
        frame = (frame + 1) % maxValue;
        StartCoroutine(FramerateRoutine());
    }

    private void Start()
    {
        StartCoroutine(FramerateRoutine());
    }

}
