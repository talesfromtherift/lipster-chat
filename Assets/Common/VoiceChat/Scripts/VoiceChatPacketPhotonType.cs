using UnityEngine;
using System;
using System.IO;
using ExitGames.Client.Photon;
using VoiceChat;

public class VoiceChatPacketPhotonType
{
	public static void Register()
	{
		PhotonPeer.RegisterType(typeof(VoiceChatPacket), (byte)'C', SerializeVoiceChatPacket, DeserializeVoiceChatPacket);
	}
	public static readonly byte[] memVoiceChatPacket = new byte[4 + 2 + 4];
	private static short SerializeVoiceChatPacket(MemoryStream outStream, object customobject)
	{
		VoiceChatPacket vcp = (VoiceChatPacket)customobject;
		int index = 0;
		lock (memVoiceChatPacket)
		{
			byte[] bytes = memVoiceChatPacket;
			Protocol.Serialize(vcp.NetworkId, bytes, ref index);
			Protocol.Serialize((Int16)vcp.Compression, bytes, ref index);
			Protocol.Serialize(vcp.Length, bytes, ref index);
			outStream.Write(bytes, 0, 4 + 2 + 4);
			outStream.Write(vcp.Data, 0, vcp.Length);
		}
		return (short)(4 + 2 + 4 + vcp.Length);
	}
	
	private static object DeserializeVoiceChatPacket(MemoryStream inStream, short length)
	{
		VoiceChatPacket vcp = new VoiceChatPacket();
		lock (memVoiceChatPacket)
		{
			inStream.Read(memVoiceChatPacket, 0, 4 + 2 + 4);
			int index = 0;
			Protocol.Deserialize(out vcp.NetworkId, memVoiceChatPacket, ref index);
			Int16 Compression;
			Protocol.Deserialize(out Compression, memVoiceChatPacket, ref index);
			vcp.Compression = (VoiceChatCompression)Compression;
			Protocol.Deserialize(out vcp.Length, memVoiceChatPacket, ref index);
			byte[] bytes = new byte[vcp.Length];
			inStream.Read(bytes, 0, vcp.Length);
			vcp.Data = bytes;
		}
		return vcp;
	}
	
}
