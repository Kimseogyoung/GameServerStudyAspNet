using ProtoBuf;
using Proto;
namespace Protocol
{
	[ProtoContract]
	public partial class KingdomConstructStructureReqPacket : IReqPacket
	{
    
        [ProtoMember(1)]
        public ReqInfoPacket Info { get; set; } 
        
        [ProtoMember(2)]
        public ulong KingdomStructureId { get; set; } 
        
        [ProtoMember(3)]
        public int KingdomItemNum { get; set; } 
        
        [ProtoMember(4)]
        public List<CostObjPacket> CostObjList { get; set; } 
        
        [ProtoMember(5)]
        public TilePosPacket StartTilePos { get; set; } 
        

        public const string NAME = "kingdom/construct-structure";
        public string GetProtocolName() => NAME;

        public KingdomConstructStructureReqPacket( ulong kingdomstructureid,  int kingdomitemnum,  List<CostObjPacket> costobjlist,  TilePosPacket starttilepos )
	    {   
         
                KingdomStructureId = kingdomstructureid; 
                 
                KingdomItemNum = kingdomitemnum; 
                 
                CostObjList = costobjlist; 
                 
                StartTilePos = starttilepos; 
                
	    }

    
        public KingdomConstructStructureReqPacket()
        {
        }
        

	}
}
