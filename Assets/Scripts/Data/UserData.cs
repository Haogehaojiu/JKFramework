using System;

[Serializable]
public class UserData
{
    public string userName;
    public int score;
    
    public UserData(string userName) => this.userName = userName;

}
