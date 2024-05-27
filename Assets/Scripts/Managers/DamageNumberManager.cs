using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    [SerializeField]private GameObject _damageNumberPrefab;
    [SerializeField]private int _amount = 15;
    public static DamageNumberManager instance;
    Queue<GameObject> _damageNumberQ = new Queue<GameObject>();
    [SerializeField] Color[] _damageColors = new Color[3];

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameObject temp;
        Vector3 defaultSize = new Vector3(1,1,1);
        for(int i = 0; i < _amount;i++)
        {
            temp = Instantiate(_damageNumberPrefab,transform);
            temp.transform.localScale = defaultSize;
            _damageNumberQ.Enqueue(temp);
            temp.SetActive(false);
        }
    }

    private IEnumerator NumberDisplayTimer(GameObject damageNumber)
    {
        yield return new WaitForSeconds(1f);
        _damageNumberQ.Enqueue(damageNumber);
        damageNumber.SetActive(false);
    }

    public void GenerateDMGNum(EnumLib.Element type, int damageValue, Vector3 position, int efficacy)
    {
        GameObject temp = _damageNumberQ.Dequeue();
        temp.SetActive(true);
        temp.transform.GetChild(0).GetComponent<TMP_Text>().SetText("<sprite="+(int)type+">"+damageValue);
        temp.transform.GetChild(0).GetComponent<TMP_Text>().color = _damageColors[efficacy];
        temp.transform.position = position;
        StartCoroutine(NumberDisplayTimer(temp));
    }
}