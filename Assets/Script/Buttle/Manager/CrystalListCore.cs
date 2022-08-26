using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//各クリスタルに指示を出すクラス
public class CrystalListCore : MonoBehaviour
{
    //水晶のリスト
    public static List<CrystalCore> _crystalList;

    [SerializeField]
    private Transform _crystalListTr;

    public void AwakeManager(CrystalParam _crystalParam){
        //全てのエネミーを取得
        _crystalList = new List<CrystalCore>();
        
        for(int i = 0 ; i < _crystalListTr.childCount ; i++){
            SetCrystalCoreInList(_crystalListTr.GetChild(i).gameObject.GetComponent<CrystalCore>(), _crystalParam);
        }
    }

    public void UpdateManager(){
        //リフト中のクリスタルとセット中のクリスタルの効果
        for(int i = 0; i < _crystalList.Count; i++){
            _crystalList[i].Attack();
        }
    }

    //クリスタルコントローラをリストに追加し、ステータスをセット（黒水晶）（クリスタルを機能させる）
    public static void SetCrystalCoreInList(CrystalCore _crystalCore, CrystalParam _crystalParam){
        _crystalList.Add(_crystalCore);
        _crystalCore.InitializeCore(_crystalParam);
    }
    //クリスタルコントローラをリストから削除
    public static void RemoveCrystalCoreInList(CrystalCore _crystalCore){
        _crystalList.Remove(_crystalCore);
    }
}
