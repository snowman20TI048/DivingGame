using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    [SerializeField]
    private Material[] skyboxMaterials;   // Skybox �p�̃}�e���A����o�^���邽�߂̔z��

    [SerializeField, Header("Skybox�ݒ�p�̎w��l�B999 �̏ꍇ�A�����_���ɂ���")]
    private int skyboxMaterialsIndex;

    /// <summary>
    /// Skybox��ύX
    /// </summary>
    public void ChangeSkybox()
    {

        // �����_���ݒ�̏ꍇ
        if (skyboxMaterialsIndex == 999)
        {

            // Skybox �������_���ȗv�f�ԍ��̃}�e���A���� Skybox �ɕύX
            RenderSettings.skybox = skyboxMaterials[RandomSelectIndexOfSkyboxMaterials()];
        }
        else
        {

            // Skybox ���w�肳�ꂽ�v�f�ԍ��̃}�e���A���� Skybox �ɕύX
            RenderSettings.skybox = skyboxMaterials[skyboxMaterialsIndex];
        }

        Debug.Log("Skybox �ύX");
    }

    /// <summary>
    /// Skybox�̃����_���ȗv�f�ԍ����擾
    /// </summary>
    /// <returns></returns>
    public int RandomSelectIndexOfSkyboxMaterials()
    {
        return Random.Range(0, skyboxMaterials.Length);
    }
}

