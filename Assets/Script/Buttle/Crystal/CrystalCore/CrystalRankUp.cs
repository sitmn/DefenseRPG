using UnityEngine;

public class CrystalRankUp : MonoBehaviour
{
    //ランクアップ用カウント
    public int _rankUpCount;
    [SerializeField]
    //ランクアップまでのカウント
    public int _rankUpMaxCount;

    // Start is called before the first frame update
    void Start()
    {
        _rankUpCount = 0;
    }

    public void UpdateManager(){
        if(!RankUpCount()) return;

        //RankUp();
    }

    //◎ランクアップまでのカウント
    private bool RankUpCount(){
        bool _rankUpFlag = false;
        _rankUpCount ++;

        if(_rankUpCount >= _rankUpMaxCount){
            _rankUpCount = 0;
            
            _rankUpFlag = true;
        }

        return _rankUpFlag;
    }

    //水晶の
    private int RankUp(int _nowRank){
        return 1;
    }

}
