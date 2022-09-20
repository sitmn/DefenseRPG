using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ移動に関するエリア区画エフェクト用のクラス
/// </summary>
public class FieldWall : MonoBehaviour
{
    //エリア区画エフェクトの親オブジェクト
    private Transform _fieldWallParent;
    //エリアを区画するエフェクト
    private ParticleSystem[] _particleSystem = new ParticleSystem[4];
    private ParticleSystem.MainModule[] _fieldWallParticleArr = new ParticleSystem.MainModule[4];
    private ParticleSystem.EmissionModule[] _fieldWallEmissionArr = new ParticleSystem.EmissionModule[4];

    /// <summary>
    /// クラスの初期化
    /// </summary>
    public void AwakeManager(){
        _fieldWallParent = this.GetComponent<Transform>();
        
        for(int i = 0 ; i < _fieldWallParticleArr.Length; i++){
            _particleSystem[i] = _fieldWallParent.GetChild(i).GetComponent<Transform>().GetChild(0).GetComponent<ParticleSystem>();
            _fieldWallParticleArr[i] = _particleSystem[i].main;
            _fieldWallEmissionArr[i] = _particleSystem[i].emission;
        }
    }

    /// <summary>
    /// エリア区画用のエフェクトをステージ移動に併せて1マス移動させるためのメソッド
    /// </summary>
    public void MoveFieldWall(){
        _fieldWallParent.position += new Vector3(1, 0, 0);
    }

    /// <summary>
    /// ステージ移動までの時間が少なくなる毎にエリア区画エフェクトを点滅させるメソッド
    /// </summary>
    /// <param name="_stageMoveTimeRate">ステージ移動までの時間と現在経過した時間の割合</param>
    public void FlashFieldWall(float _stageMoveTimeRate){
        for(int i = 0; i < _fieldWallParticleArr.Length; i++){
            _fieldWallParticleArr[i].startLifetimeMultiplier = Mathf.Lerp(ConstManager._fieldWallMaxStartLifeTime, ConstManager._fieldWallMinStartLifeTime, _stageMoveTimeRate);
            _fieldWallEmissionArr[i].rateOverTime = 5 * Mathf.Lerp(ConstManager._fieldWallMinRateOverTime, ConstManager._fieldWallMaxRateOverTime, _stageMoveTimeRate);;
            //ステージ移動に併せてParticleをリセット
            if(_stageMoveTimeRate == 0){
                _particleSystem[i].Clear();
            }
        }
    }
}
