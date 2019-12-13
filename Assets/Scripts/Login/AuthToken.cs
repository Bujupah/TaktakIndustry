using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Login {
    public class AuthToken : MonoBehaviour {
        public Text username;
        public Text password;
        public GameObject isSaved;


        public InputField serverHost;
        public InputField serverPort;
        public GameObject serverSecure;

        // DEFAULT VALUES
        private string host = "192.168.1.2";
        private int port = 1880;
        private int isSecured = 0;

        private string url;

        private void Awake() {
            InitState();
        }
        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Return)) {
                Authenticate();
            }
        }

        private void InitState()
        {
            print(PlayerPrefs.HasKey(User.KeyToken) ? "Connected! " : "Disconnected! ");

            if (PlayerPrefs.HasKey(User.KeyToken))
                SceneManager.LoadSceneAsync(1);

            if (PlayerPrefs.HasKey(User.KeyStore))
                isSaved.GetComponent<Toggle>().isOn = PlayerPrefs.GetInt(User.KeyStore) == 1 ? true : false;

            SetUrl();
            serverHost.text = host;
            serverPort.text = $"{port}";
            serverSecure.GetComponent<Toggle>().isOn = isSecured == 1 ? true : false;

            Debug.Log(url);
        }
        private void Login() {
            Authenticate();
        }
        private void Authenticate() {
            if (username.text.Length>4 && password.text.Length>=4) {
                persistUser();
                StartCoroutine(GetToken(url + "/auth"));
            } else{
                Debug.Log("Please insert your login credentials!");
            }
        }
        private void SetUrl()
        {
            if (PlayerPrefs.HasKey(User.KeyServer))
                host = PlayerPrefs.GetString(User.KeyServer);

            if (PlayerPrefs.HasKey(User.KeyServerPort))
                port = PlayerPrefs.GetInt(User.KeyServerPort);

            if (PlayerPrefs.HasKey(User.KeyServerIsSecured))
                isSecured = PlayerPrefs.GetInt(User.KeyServerIsSecured);

            url = (isSecured == 1 ? "https" : "http") + "://" + host + ":" + port;
        }
        private void GetSaveLoad(string response) {

            User.createFromJson(response);

            

            SceneManager.LoadScene(1);
        } 
        private void persistUser() {
            PlayerPrefs.SetInt(User.KeyStore, isSaved.GetComponent<Toggle>().isOn ? 1 : 0);
        }

        public void SaveServer() {
            var _ip = serverHost.text;
            var _port = int.Parse(serverPort.text);
            var _isSecure = serverSecure.GetComponent<Toggle>().isOn ? 1 : 0;
            if (_ip.Split('.').Length == 4) {
                PlayerPrefs.SetString(User.KeyServer, _ip);
                PlayerPrefs.SetInt(User.KeyServerPort, _port);
                PlayerPrefs.SetInt(User.KeyServerIsSecured, _isSecure);
            }
            else
                Debug.LogWarning("Server settings are not set!");
            SetUrl();
            Debug.Log(url);
        }

        private IEnumerator GetToken(string uri) {
            WWWForm form = new WWWForm();
                    form.AddField("username", username.text);
                    form.AddField("password", password.text);
                    
            using (UnityWebRequest www = UnityWebRequest.Post(uri, form)) {
                yield return www.SendWebRequest();
                
                var response = www.downloadHandler.text
                    .Substring(1,www.downloadHandler.text.Length-2);
                
                if (www.isNetworkError || www.isHttpError) {
                    var errorMsg = response.Split(':')[1].Trim('\"');
                    print(errorMsg);
                    // TODO show Error dialog...
                }  else {
                    var token = response.Split(':')[1].Trim('\"');
                    StartCoroutine(GetUserWithToken(token));
                }
            }
        }
        private IEnumerator GetUserWithToken(string token)
        {
            WWWForm form = new WWWForm();
                form.AddField("token", token);

            using (UnityWebRequest www = UnityWebRequest.Post(url + "/login",form)) {
                yield return www.SendWebRequest();
                var response = www.downloadHandler.text;
                
                if (www.isNetworkError || www.isHttpError) {
                    var errorMsg = response.Split(':')[1].Trim('\"');
                    Debug.Log(errorMsg);
                    // TODO show Error dialog...
                }  else {
                    GetSaveLoad(response);
                }
            }
        }
    }
    
}
