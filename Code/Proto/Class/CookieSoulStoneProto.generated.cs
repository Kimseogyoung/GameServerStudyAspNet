using ProtoBuf;
namespace Proto
{
	[ProtoContract]
	public partial class CookieSoulStoneProto : ProtoBase
	{
    
    		[ProtoMember(2)]
    		public int Num { get; set; }
        
    		[ProtoMember(3)]
    		public int CookieNum { get; set; }
        
	}
}
