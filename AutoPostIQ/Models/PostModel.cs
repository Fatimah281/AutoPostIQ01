namespace AutoPostIQ.Models
{
	public class PostModel
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public string Topic { get; set; }
		public DateTime GeneratedAt { get; set; }
		public bool IsApproved { get; set; }
		public DateTime? ScheduledDate { get; set; }
	}
}
