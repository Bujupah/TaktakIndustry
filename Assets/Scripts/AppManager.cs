using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public GameObject profileAvatar;
    public GameObject profileData;

    private User _user;

    private void Start() {
        if (!PlayerPrefs.HasKey(User.KeyToken))
            SceneManager.LoadScene(0);
        InitState();
    }
    private void OnDestroy() {
        ClearDataMaybe();
    }
    private void InitState() {
        _user = new User().getUser();
        profileData.GetComponent<Text>().text = $"User: {_user.Name} \nEmail: {_user.Email}";
        StartCoroutine(LoadProfileImage(_user.Avatar));
    }

    private void ClearDataMaybe() {
        var isSetSave = PlayerPrefs.GetInt(User.KeyStore);
        if (PlayerPrefs.GetInt(User.KeyStore) == 0) {
            PlayerPrefs.DeleteAll();
        }
        PlayerPrefs.SetInt(User.KeyStore, isSetSave);
    }
    public void Disconnect() {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
        Debug.Log("Disconnected!");
    }

    IEnumerator LoadProfileImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            profileAvatar.GetComponent<Image>().material.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Debug.Log("Image loaded!");
        }
    }
}
