using ProtoBuf;
using Proto;
namespace Protocol
{
	[ProtoContract]
	public partial class PlacedKingdomItemPacket
	{
    
    		[ProtoMember(1)]
    		public ulong Id { get; set; } = default; //
        
    		[ProtoMember(2)]
    		public ulong StructureItemId { get; set; } = default; //
        
    		[ProtoMember(3)]
    		public EKingdomItemType Type { get; set; } = default; //
        
    		[ProtoMember(4)]
    		public int Num { get; set; } = default; //
        
    		[ProtoMember(5)]
    		public EPlacedKingdomItemState State { get; set; } = default; //
        
    		[ProtoMember(6)]
    		public int StartTileX { get; set; } = default; //
        
    		[ProtoMember(7)]
    		public int StartTileY { get; set; } = default; //
        
    		[ProtoMember(8)]
    		public int SizeX { get; set; } = default; //
        
    		[ProtoMember(9)]
    		public int SizeY { get; set; } = default; //
        
    		[ProtoMember(10)]
    		public int Rotation { get; set; } = default; //
        
	}
}
