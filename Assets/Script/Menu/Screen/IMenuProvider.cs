using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IMenuProvider
{
    //ボタンストリームを作成
    void CreateButtonStream(List<Button> createButton);
    //スクリーンの動的ボタンを生成
    void CreateButton(GameObject buttonParent, List<Button> buttonList);
}
