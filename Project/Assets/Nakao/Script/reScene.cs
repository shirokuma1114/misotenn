using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class reScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //ChangeScene
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("ti");

            if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Correct");
        }

        // �^�C�g���V�[���ŕێ����Ă���X�R�A���擾
        //int score = ti.GetScore();
    }
}
