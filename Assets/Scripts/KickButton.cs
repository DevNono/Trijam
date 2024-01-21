using UnityEngine;

public class KickButton : MonoBehaviour
{
    private GameObject pnjToFollow;
    private GameObject day;

    void Start()
    {
        day = GameObject.Find("GameManager");
    }

    public void SetPNJToFollow(GameObject pnj)
    {
        pnjToFollow = pnj;
    }

    public void OnClick()
    {
        int index = transform.GetSiblingIndex();
        day.GetComponent<Day>().KickPNJ(index + 1);
    }

    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pnjToFollow.transform.position);
        transform.gameObject.GetComponent<RectTransform>().position = new Vector3(screenPos.x - 32.5f, screenPos.y + 125f, 0);
    }
}