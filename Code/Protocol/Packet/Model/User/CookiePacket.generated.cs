using ProtoBuf;
using Proto;
namespace Protocol
{
	[ProtoContract]
	public partial class CookiePacket
	{
    
    		[ProtoMember(1)]
    		public int Num { get; set; } = default; //
        
    		[ProtoMember(2)]
    		public int StarExp { get; set; } = default; //
        
    		[ProtoMember(3)]
    		public int AccStarExp { get; set; } = default; //
        
    		[ProtoMember(4)]
    		public int Star { get; set; } = default; //
        
    		[ProtoMember(5)]
    		public int Flag { get; set; } = default; //
        
    		[ProtoMember(6)]
    		public ECookieState State { get; set; } = default; //
        
	}
}
