using System;
using UnityEngine;

[Serializable]
public class User {
    public const string KeyName = "Name";
    public const string KeyUsername = "Username";
    public const string KeyAvatar = "Avatar";
    public const string KeyEmail = "Email";
    public const string KeyToken = "Token";
    public const string KeyStore = "Store";
    public const string KeyServer = "Server";
    public const string KeyServerPort = "ServerPort";
    public const string KeyServerIsSecured = "isSecured";

    public string Name;
    public string Username;
    public string Avatar;
    public string Email;
    public string Token;

    private User saveUser()
    {
        PlayerPrefs.SetString(KeyName, this.Name);
        PlayerPrefs.SetString(KeyUsername, this.Username);
        PlayerPrefs.SetString(KeyAvatar, this.Avatar);
        PlayerPrefs.SetString(KeyEmail, this.Email);
        PlayerPrefs.SetString(KeyToken, this.Token);

        return this;
    }

    public User getUser()
    {
        User user = new User
        {
            Name = PlayerPrefs.GetString(KeyName),
            Username = PlayerPrefs.GetString(KeyUsername),
            Avatar = PlayerPrefs.GetString(KeyAvatar),
            Email = PlayerPrefs.GetString(KeyEmail),
            Token = PlayerPrefs.GetString(KeyToken)
        };
        return user;
    }
    public static User createFromJson(string json)
    {
        return JsonUtility.FromJson<User>(json).saveUser();
    }

}
