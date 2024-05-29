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

        if(ratio > 0f)
        {
            Debug.Log("status object not active for "+status.ToString());
            statusObject.SetActive(true);
        }
        else
        {
            Debug.Log("empty ratio on status display"+status.ToString());
            statusObject.SetActive(false);
        }
        
        statusObject.transform.GetChild(0).GetComponent<Image>().fillAmount = ratio;
    }

    //Set up display for when status is fully applied.
    public void SetStatusDisplayTick(EnumLib.Status status, float ratio)
    {
        GameObject statusObject = _statusIcons.GetChild((int)status).gameObject;
        statusObject.transform.GetChild(2).GetComponent<Image>().fillAmount = ratio;

        if (ratio <= 0.0f)
        {
            Debug.Log("Disabling display for"+status.ToString());
            statusObject.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
            statusObject.SetActive(false);
        }
        else
        {
            Debug.Log("Somehow inactive for "+status.ToString());
            statusObject.SetActive(true);
        }
    }

    public void SetStatusDisplayTick(EnumLib.Status status, float ratio, float buildupRatio)
    {
        GameObject statusObject = _statusIcons.GetChild((int)status).gameObject;
        statusObject.transform.GetChild(2).GetComponent<Image>().fillAmount = ratio;

        if (ratio <= 0.0f && buildupRatio <= 0.0f)
        {
            Debug.Log("Disabling display for"+status.ToString());
            statusObject.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
            statusObject.SetActive(false);
        }
        else
        {
            Debug.Log("Somehow inactive for "+status.ToString());
            statusObject.SetActive(true);
        }
    }
}