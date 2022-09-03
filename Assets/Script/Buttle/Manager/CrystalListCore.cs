using System.Collections.Generic;
using UnityEngine;

//各クリスタルに指示を出すクラス
public class CrystalListCore : MonoBehaviour
{
    //水晶のリスト
    public static List<CrystalCoreBase> _crystalList;

    [SerializeField]
    private Transform _crystalListTr;

    public void AwakeManager(CrystalParamAsset _crystalParamAsset){
        //全てのエネミーを取得
        _crystalList = new List<CrystalCoreBase>();
        
        for(int i = 0 ; i < _crystalListTr.childCount ; i++){
            CrystalCoreBase _crystalCore = _crystalListTr.GetChild(i).gameObject.GetComponent<CrystalCoreBase>();
            if(i == 0){
                //ShippingCrystalの初期化
                SetCrystalCoreInList(_crystalCore, _crystalParamAsset.CrystalParamList[_crystalParamAsset.CrystalParamList.Count - 1]);
            }else{
                //BlackCrystalの初期化
                SetCrystalCoreInList(_crystalCore, _crystalParamAsset.CrystalParamList[0]);
            }
            
        }
    }

    public void UpdateManager(){
        CrystalActions();
    }

    //全てのクリスタルを行動させる
    private void CrystalActions(){
        //リフト中のクリスタルとセット中のクリスタルの効果
        for(int i = 0; i < _crystalList.Count; i++){
            _crystalList[i].CrystalAction();
        }
    }

    //クリスタルコントローラをリストに追加し、ステータスをセット（黒水晶）（クリスタルを機能させる）
    public static void SetCrystalCoreInList(CrystalCoreBase _crystalCore, CrystalParam _crystalParam){
        _crystalList.Add(_crystalCore);
        _crystalCore.InitializeCore(_crystalParam);
    }
    //クリスタルコントローラをリストから削除
    public static void RemoveCrystalCoreInList(CrystalCoreBase _crystalCore){
        _crystalList.Remove(_crystalCore);
    }
}
