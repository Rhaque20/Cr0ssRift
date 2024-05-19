using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField]protected Transform _statusIcons;

    protected void Start()
    {
        if (_statusIcons != null)
        {
            for(int i = 0; i < _statusIcons.childCount; i++)
            {
                _statusIcons.GetChild(i).gameObject.SetActive(false);
            }
        }
        
    }

    // Set up display for when being afflicted with statusbuild up
    public virtual void SetStatusBuildUpDisplay(EnumLib.Status status, float ratio)
    {
        GameObject statusObject = _statusIcons.GetChild((int)status).gameObject;

        if(!statusObject.activeSelf)
            statusObject.SetActive(true);
        
        statusObject.transform.GetChild(0).GetComponent<Image>().fillAmount = ratio;
    }

    //Set up display for when status is fully applied.
    public void SetStatusDisplayTick(EnumLib.Status status, float ratio)
    {
        GameObject statusObject = _statusIcons.GetChild((int)status).gameObject;
        statusObject.transform.GetChild(2).GetComponent<Image>().fillAmount = ratio;

        if (ratio <= 0.0f)
        {
            statusObject.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
            statusObject.SetActive(false);
        }
    }
}