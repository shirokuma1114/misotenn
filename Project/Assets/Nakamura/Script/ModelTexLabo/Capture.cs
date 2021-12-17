using System.IO;
using UnityEngine;

public class Capture : MonoBehaviour
{
    [SerializeField] private Camera _3dCamera;

    // ��A�N�e�B�u�ɂ������Ώ�(��)
    [SerializeField] private GameObject _3dObject;

    // �����o�������w�i�J���[
    [SerializeField] private Color _backgroundColor;

    // �摜�̕ۑ��t�H���_��
    private const string SAVE_FOLDER = "Nakamura";

    public Texture2D GetCapture()
    {
        // �ォ��߂���悤�ɔw�i�F��ޔ�
        var bgColor = _3dCamera.backgroundColor;
        // �w�i�ύX����
        _3dCamera.backgroundColor = _backgroundColor;
        // ����Ȃ��I�u�W�F�N�g���\����
        _3dObject.SetActive(false);
        // �ĕ`�悷��
        _3dCamera.Render();

        // RenderTexture���擾����
        var targetRt = _3dCamera.targetTexture;
        var width = targetRt.width;
        var height = targetRt.height;

        // ���ݎg�p���Ă���`��f�[�^��ޔ�
        var currentRt = RenderTexture.active;
        // �ꎞ�I�Ɏg�p����RenderTexture�𐶐�
        var workRt = RenderTexture.GetTemporary(width, height, 0);
        // �`��ΏۂɃZ�b�g
        RenderTexture.active = workRt;

        // RenderTexture�ɕ`�悷��
        {
            // �w�i���ߗp��clear�œh��Ԃ�(�`��Ώ۔͈͂������Ȃ�s�v)
            GL.Clear(true, true, Color.clear);

            Graphics.Blit(targetRt, workRt);
        }

        // ���ߏ����o������̂�Alpha���܂߂�
        var format = TextureFormat.ARGB32;
        // Texture2D�ɏ����o��
        var resultTexture = new Texture2D(width, height, format, false);
        resultTexture.hideFlags = HideFlags.DontSave;
        resultTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
        resultTexture.Apply();

        // �ޔ����Ă���RenderTexture��߂�
        RenderTexture.active = currentRt;
        // ��������RenderTexture��j��
        RenderTexture.ReleaseTemporary(workRt);

        // �\����߂�
        _3dCamera.backgroundColor = bgColor;
        _3dObject.SetActive(true);

        return resultTexture;
    }

    // uGUI��Button����Ă�
    public void SaveCapture()
    {
        var tex = GetCapture();
        // �ۑ�����摜���i�������O���Ə㏑�������̂ŁA�������Ȃ�A�Ԃɂ���j
        SaveImage("capture", tex);
    }

    // Texture���摜�����o��
    private void SaveImage(string fileName, Texture2D tex)
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            // Assets�Ɠ��K�w�i�v���W�F�N�g�t�H���_�����j�ɍ��
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        var path = Path.Combine(SAVE_FOLDER, fileName);
        // ���߉摜�������̂ŁApng�ŕۑ�
        File.WriteAllBytes($"{path}.png", tex.EncodeToPNG());
    }
}