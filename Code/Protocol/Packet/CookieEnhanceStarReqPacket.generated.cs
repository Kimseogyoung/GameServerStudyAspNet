using ProtoBuf;
using Proto;
namespace Protocol
{
	[ProtoContract]
	public partial class CookieEnhanceStarReqPacket : IReqPacket
	{
    
        [ProtoMember(1)]
        public ReqInfoPacket Info { get; set; } 
        
        [ProtoMember(2)]
        public int CookieNum { get; set; } 
        
        [ProtoMember(3)]
        public int BefStar { get; set; } 
        
        [ProtoMember(4)]
        public int AftStar { get; set; } 
        
        [ProtoMember(5)]
        public CostObjPacket CostObj { get; set; } 
        
        public string GetProtocolName() => "cookie/enhance-star";
	}
}
