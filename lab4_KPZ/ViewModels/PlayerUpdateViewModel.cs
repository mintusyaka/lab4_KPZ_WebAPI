namespace lab4_KPZ.ViewModels
{
	public class PlayerUpdateViewModel
	{
		public int PlayerId { get; set; }
		public string Nickname { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Sex { get; set; } = null!;
		public int Score { get; set; }
	}
}
