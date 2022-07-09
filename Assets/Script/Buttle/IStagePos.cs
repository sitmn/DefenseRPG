using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IStagePos
{
    //ステージマス管理
    //１マスに１つ
    StageObject[][] stageObjList{get; set;}

    //配列[0][0]の位置
    //エリア収縮＋エリア拡張の際に最後列だったものが最前列に置き換わるため、そのカウント用
    int startPointAnchor{get; set;}
}
