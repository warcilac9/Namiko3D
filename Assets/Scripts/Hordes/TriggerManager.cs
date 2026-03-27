using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    GameObject Vlimit1, Vlimit2;

    public void SetStage(GameObject triggerObj,GameObject limit1,GameObject limit2)
    {
        Vlimit1 = limit1;
        Vlimit2 = limit2;
        triggerObj.SetActive(false);
        Vlimit1.SetActive(true);
        Vlimit2.SetActive(true);
    }

    public void CloseStage()
    {
        Vlimit1.SetActive(false);
        Vlimit2.SetActive(false);
    }
}