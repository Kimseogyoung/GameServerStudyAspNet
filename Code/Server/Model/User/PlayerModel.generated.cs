using ProtoBuf;
using Proto;

namespace WebStudyServer.Model
{
	
	public partial class PlayerModel : ModelBase
	{
    
    		
    		public ulong Id { get; set; } = default; //
        
    		
    		public ulong AccountId { get; set; } = default; //
        
    		
    		public ulong SfId { get; set; } = default; //
        
    		
    		public string ProfileName { get; set; } = default; //
        
    		
    		public int Lv { get; set; } = 1; //
        
    		
    		public ulong Flag { get; set; } = default; //
        
    		
    		public EPlayerState State { get; set; } = default; //
        
    		
    		public int Exp { get; set; } = default; //
        
    		
    		public int AccExp { get; set; } = default; //
        
    		
    		public int ProfileTitleNum { get; set; } = default; //
        
    		
    		public int ProfileIconNum { get; set; } = default; //
        
    		
    		public int ProfileFrameNum { get; set; } = default; //
        
    		
    		public int ProfileCookieNum { get; set; } = default; //
        
    		
    		public ulong GuildId { get; set; } = default; //
        
    		
    		public int KingdomExp { get; set; } = default; //
        
    		
    		public double Gold { get; set; } = default; //
        
    		
    		public double AccGold { get; set; } = default; //
        
    		
    		public double RealCash { get; set; } = default; //
        
    		
    		public double FreeCash { get; set; } = default; //
        
    		
    		public double AccRealCash { get; set; } = default; //
        
    		
    		public double AccFreeCash { get; set; } = default; //
        
    		
    		public DateTime UpdateTime { get; set; } = default; //
        
    		
    		public DateTime CreateTime { get; set; } = default; //
        
	}
}
