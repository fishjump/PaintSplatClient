using System;
using System.Collections.Generic;

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
    public string winner_id;
}

[Serializable]
public struct UploadBoardRequest
{
    public string session_id;
    public string player_id;
    public Position pos;
}

[Serializable]
public struct UploadBoardResponse
{
    public bool success;
    public string reason;
}

[Serializable]
public struct SyncBoardRequest
{
    public string session_id;
}

[Serializable]
public struct SyncBoardResponse
{
    public bool success;
    public string reason;
    public Position pos;
}