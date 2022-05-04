using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamScript : MonoBehaviour
{
    [SerializeField]
    private MenuScript menuScript;

    [SerializeField]
    private GameObject teamFrame;
    [SerializeField]
    private GameObject charFrame;
    [SerializeField]
    private Button buttonPrefab;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void InstantiateButton(){
        //ローカルリストから編成キャラ数、所持キャラ数、各キャラ情報からボタンを作成し、配置。MenuのStream作成を実行
        //テーブルからキャラを全出力
        //List<CharProfileModel> charProfileModel = CharProfile.Get();
        //生成したボタンをリストへ格納
        List<Button> buttonList = new List<Button>();

        for(int i = 0; i < 20; i++){
            Button button = Instantiate(buttonPrefab, charFrame.transform.position + new Vector3(150 * i, -150 * ((i / 5) + 1),0),Quaternion.identity);
            buttonList.Add(button);
            button.gameObject.transform.parent = charFrame.transform;
        }

        

        menuScript.ButtonStreamCreate(buttonList);
    }
}
