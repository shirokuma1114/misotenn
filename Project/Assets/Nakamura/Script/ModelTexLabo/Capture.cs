using System.IO;
using UnityEngine;

public class Capture : MonoBehaviour
{
    [SerializeField] private Camera _3dCamera;

    // 非アクティブにしたい対象(床)
    [SerializeField] private GameObject _3dObject;

    // 書き出したい背景カラー
    [SerializeField] private Color _backgroundColor;

    // 画像の保存フォルダ名
    private const string SAVE_FOLDER = "Nakamura";

    public Texture2D GetCapture()
    {
        // 後から戻せるように背景色を退避
        var bgColor = _3dCamera.backgroundColor;
        // 背景変更する
        _3dCamera.backgroundColor = _backgroundColor;
        // いらないオブジェクトを非表示に
        _3dObject.SetActive(false);
        // 再描画する
        _3dCamera.Render();

        // RenderTextureを取得する
        var targetRt = _3dCamera.targetTexture;
        var width = targetRt.width;
        var height = targetRt.height;

        // 現在使用している描画データを退避
        var currentRt = RenderTexture.active;
        // 一時的に使用するRenderTextureを生成
        var workRt = RenderTexture.GetTemporary(width, height, 0);
        // 描画対象にセット
        RenderTexture.active = workRt;

        // RenderTextureに描画する
        {
            // 背景透過用にclearで塗りつぶす(描画対象範囲が同じなら不要)
            GL.Clear(true, true, Color.clear);

            Graphics.Blit(targetRt, workRt);
        }

        // 透過書き出しするのでAlphaも含める
        var format = TextureFormat.ARGB32;
        // Texture2Dに書き出す
        var resultTexture = new Texture2D(width, height, format, false);
        resultTexture.hideFlags = HideFlags.DontSave;
        resultTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
        resultTexture.Apply();

        // 退避していたRenderTextureを戻す
        RenderTexture.active = currentRt;
        // 生成したRenderTextureを破棄
        RenderTexture.ReleaseTemporary(workRt);

        // 表示を戻す
        _3dCamera.backgroundColor = bgColor;
        _3dObject.SetActive(true);

        return resultTexture;
    }

    // uGUIのButtonから呼ぶ
    public void SaveCapture()
    {
        var tex = GetCapture();
        // 保存する画像名（同じ名前だと上書きされるので、複数取るなら連番にする）
        SaveImage("capture", tex);
    }

    // Textureを画像書き出し
    private void SaveImage(string fileName, Texture2D tex)
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            // Assetsと同階層（プロジェクトフォルダ直下）に作る
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        var path = Path.Combine(SAVE_FOLDER, fileName);
        // 透過画像を扱うので、pngで保存
        File.WriteAllBytes($"{path}.png", tex.EncodeToPNG());
    }
}