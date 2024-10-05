using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollLimitRect : ScrollRect
{
    private Vector2 stopScrollPosition;

    private bool isForceScroll = true;

    private bool isScroll = true;

    private float maxPosition;

    private float minPosition;

    public enum Direction
    {
        Horizontal,

        Vertical
    }

    private Direction targetDirection;

    public void Init(float max, float min, Direction direction)
    {
        maxPosition = max;
        minPosition = min;
        targetDirection = direction;
    }

    protected override void SetContentAnchoredPosition(Vector2 position)
    {
        Debug.Log(position);

        if (IsNextPosition(position))
        {
            base.SetContentAnchoredPosition(position);
        }
    }

    private bool IsNextPosition(Vector2 nextPosition)
    {
        if (isForceScroll)
        {
            return true;
        }

        if (targetDirection == Direction.Horizontal)
        {
            if (content.anchoredPosition.x >= maxPosition)
            {
                return maxPosition > nextPosition.x;
            }
            else if (content.anchoredPosition.x <= minPosition)
            {
                return minPosition < nextPosition.x;
            }
        }
        else
        {
            if (content.anchoredPosition.y >= maxPosition)
            {
                return maxPosition > nextPosition.y;
            }
            else if (content.anchoredPosition.y <= minPosition)
            {
                return minPosition < nextPosition.y;
            }
        }

        return true;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        isForceScroll = false;
        base.OnBeginDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        isForceScroll = true;
        base.OnEndDrag(eventData);
    }
}
