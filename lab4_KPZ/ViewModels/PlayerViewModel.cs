namespace lab4_KPZ.ViewModels
{
	public class PlayerViewModel
	{
		public int PlayerId { get; set; }
		public string Nickname { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Sex { get; set; } = null!;
		public DateOnly RegistrationDate { get; set; }
		public TimeOnly RegistrationTime { get; set; }
		public int Score { get; set; }
	}
}
