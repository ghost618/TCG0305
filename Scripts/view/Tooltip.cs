using UnityEngine;

using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text toolTipText;
    public Text toolTipChildText;

    public void UpdateToolTip(string content)
    {
        toolTipText.text = content;
        toolTipChildText.text = content;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void SetLocationPosition(Vector2 position)
    {
        this.transform.localPosition = position;
    }

}
