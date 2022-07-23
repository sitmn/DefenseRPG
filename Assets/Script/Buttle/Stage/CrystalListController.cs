using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//各クリスタルに指示を出すクラス
public class CrystalListController : MonoBehaviour
{
    [SerializeField]
    private List<CrystalController> _crystalListController;
    
    void Update(){
        for(int i = 0; i < _crystalListController.Count; i++){
            _crystalListController[i].SetEffect();
        }
    }
}
