using ProtoBuf;
using Proto;
namespace Protocol
{
	[ProtoContract]
	public partial class KingdomChangeItemReqPacket : IReqPacket
	{
    
        [ProtoMember(1)]
        public ReqInfoPacket Info { get; set; } 
        
        [ProtoMember(2)]
        public List<ulong> StoreKingdomItemIdList { get; set; } 
        
        [ProtoMember(3)]
        public List<ChgKingdomItemPacket> ChgKingdomItemList { get; set; } 
        
        public string GetProtocolName() => "kingdom/change-item";
	}
}
