using ProtoBuf;
using Proto;
namespace Protocol
{
	[ProtoContract]
	public partial class RewardObjPacket	
        {
        
                [ProtoMember(1)]
                public EObjType Type { get; set; } 
                
                [ProtoMember(2)]
                public int Num { get; set; } 
                
                [ProtoMember(3)]
                public double Amount { get; set; } 
                
	}
}