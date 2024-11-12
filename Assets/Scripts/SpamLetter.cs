using UnityEngine;

public class SpamLetter : MonoBehaviour
{
    [SerializeField] private GameObject BinCollector, SpamSector;
    public void ToBin()
    {
        gameObject.transform.SetParent(BinCollector.transform);
    }

    public void Restore()
    {
        gameObject.transform.SetParent(SpamSector.transform);
    }
}
