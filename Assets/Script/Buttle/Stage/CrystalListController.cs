using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//各クリスタルに指示を出すクラス
public class CrystalListController : MonoBehaviour
{
    //水晶のリスト
    public static List<CrystalController> _crystalList;

    [SerializeField]
    private Transform _crystalListTr;

    void Awake(){
        //全てのエネミーを取得
        _crystalList = new List<CrystalController>();
        
        for(int i = 0 ; i < _crystalListTr.childCount ; i++){
            _crystalList.Add(_crystalListTr.GetChild(i).gameObject.GetComponent<CrystalController>());
        }
    }

    void Update(){
        //リフト中のクリスタルとセット中のクリスタルの効果
        for(int i = 0; i < _crystalList.Count; i++){
            if(_crystalList[i] == LiftCrystal._crystalController){
                _crystalList[i].LiftEffect();
            }else{
                _crystalList[i].SetEffect();
            }
        }
    }

    //クリスタルコントローラをリストに追加（クリスタルを機能させる）
    public static void AddCrystalList(CrystalController _crystalController){
        _crystalList.Add(_crystalController);
    }
    //クリスタルコントローラをリストから削除
    public static void RemoveCrystalList(CrystalController _crystalController){
        _crystalList.Remove(_crystalController);
    }
}
