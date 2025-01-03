using ProtoBuf;
using Proto;
namespace Protocol
{
	[ProtoContract]
	public partial class GameEnterResPacket : IResPacket
	{
    
        [ProtoMember(1)]
        public ResInfoPacket Info { get; set; } = new();
        
        [ProtoMember(2)]
        public PlayerPacket Player { get; set; } 
        
	}
}
