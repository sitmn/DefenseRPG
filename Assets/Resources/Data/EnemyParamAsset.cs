using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParamData", menuName = "ScriptableObjects/CreateEnemyParamAsset")]
public class EnemyParamAsset : ScriptableObject{
    public List<EnemyParam> EnemyParamList = new List<EnemyParam>();

    //AttackStatusをインスタンス化
    public void SetAttackStatus(){
        for(int i = 0; i < EnemyParamList.Count; i++){
            EnemyParamList[i]._attackStatus = new AttackStatus(EnemyParamList[i]._attack, EnemyParamList[i]._attackRange, EnemyParamList[i]._effectRate, EnemyParamList[i]._effectTime);
        }
    }
}

[System.Serializable]
public class EnemyParam{
    //敵キャラ名
    [SerializeField]
    public string _enemyName;
    //最大HP
    [SerializeField]
    public List<int> _maxHp;
    //攻撃間隔
    [SerializeField]
    public List<int> _attackMaxCount;
    //攻撃力
    [SerializeField]
    public List<int> _attack;
    //攻撃範囲
    [SerializeField]
    public List<int> _attackRange;
    //効果倍率
    [SerializeField]
    public List<float> _effectRate;
    //効果時間
    [SerializeField]
    public List<int> _effectTime;
    //移動速度
    [SerializeField]
    public List<float> _moveSpeed;
    //索敵範囲
    [SerializeField]
    public List<int> _searchDestination;
    //攻撃関連のステータス
    public AttackStatus _attackStatus;
}
