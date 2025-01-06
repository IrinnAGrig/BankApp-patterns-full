namespace BankingAppBackend.Model
{
    public class RequestModel
    {
        public required string Id { get; set; }
        public string Name { get; set; }
        public string ReceiverId { get; set; }

        public string SenderId { get; set; }
        public User Sender { get; set; }
        public decimal Amount { get; set; }
        public string Valute { get; set; }
        public string DueDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DateSent { get; set; }
        public bool Closed { get; set; }
        public char Status { get; set; }
        public string DateClosed { get; set; }
        public bool IsRead{ get; set; }
        public decimal RemainAmount { get; set; }
     }
}
