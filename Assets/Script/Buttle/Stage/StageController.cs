using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//複数の水晶の動作（攻撃指示等）
public class StageController : MonoBehaviour
{
    //水晶のリスト
    private List<ICrystalController> _crystalsList;

    [SerializeField]
    private Transform _crystalListTr;

    // Start is called before the first frame update
    void Start()
    {
        //全ての水晶を取得
        _crystalsList = new List<ICrystalController>();
        
        for(int i = 0 ; i < _crystalListTr.childCount ; i++){
            _crystalsList.Add(_crystalListTr.GetChild(i).gameObject.GetComponent<CrystalController>());
        }
    }




}
