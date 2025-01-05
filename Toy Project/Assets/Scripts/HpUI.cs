using UnityEngine;

public class HpUI : MonoBehaviour
{
    public Transform[] hpIcons;

    public void ChangeHp(int hp)
    {
        for (int i = 0; i < hpIcons.Length; i++)
        {
            if(i < hp)
                hpIcons[i].gameObject.SetActive(true);
            else
                hpIcons[i].gameObject.SetActive(false);
        }
    }
}