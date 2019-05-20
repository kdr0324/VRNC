using System;
using UnityEngine.Networking;

public class SpawnMessage : MessageBase
{
    public int CharacterType;

    public override void Deserialize(NetworkReader reader)
    {
        CharacterType = reader.ReadInt32();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(CharacterType);
    }
}
