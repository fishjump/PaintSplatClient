using System;
using System.Collections.Generic;

public enum Role { red, blue, green, yellow }

[Serializable]
public struct Position
{
    public float x;
    public float y;
}

[Serializable]
public struct BattleLog
{
    public string player_id;
    public Position pos;
}

[Serializable]
public struct CreateSessionRequest
{
    public string player_id;
}

[Serializable]
public struct CreateSessionResponse
{
    public bool success;
    public string reason;
    public string session_id;
    public Role role;
}

[Serializable]
public struct JoinSessionRequest
{
    public string player_id;
    public string session_id;
}

[Serializable]
public struct JoinSessionResponse
{
    public bool success;
    public string reason;
}

[Serializable]
public struct ActInSessionRequest
{
    public string player_id;
    public string session_id;
    public Position pos;
}

[Serializable]
public struct ActInSessionResponse
{
    public bool success;
    public string reason;
}

[Serializable]
public struct GetSessionLogRequest
{
    public string session_id;
    public int from;
    public int to;
}

[Serializable]
public struct GetSessionLogResponse
{
    public bool success;
    public string reason;
    public List<BattleLog> logs;
}

[Serializable]
public struct GetSessionInfoRequest
{
    public string session_id;
}

[Serializable]
public struct GetSessioInfoResponse
{
    public bool success;
    public string reason;
    public int available_seats;
}