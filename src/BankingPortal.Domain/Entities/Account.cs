using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace BankingPortal.Domain.Entities
{
    public class Account
    {
        public long AccountId { get; set; }
        public long CustomerId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public int AccountTypeId { get; set; }
        public decimal Balance { get; set; } = 0.00m;
        public string Status { get; set; } = "Active";
        public long BranchId { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime OpenedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedDate { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; } // Optimistic Concurrency Check

        public Customer Customer { get; set; } = null!;
        public ICollection<TransactionLedger> Transactions { get; set; } = new List<TransactionLedger>();
    }

    public class TransactionLedger
    {
        public long TransactionId { get; set; }
        public long AccountId { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public int TransactionTypeId { get; set; }
        public decimal DebitAmount { get; set; } = 0.00m;
        public decimal CreditAmount { get; set; } = 0.00m;
        public decimal BalanceAfterTransaction { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string? Narration { get; set; }
        public string Status { get; set; } = "Completed";

        public Account Account { get; set; } = null!;
    }
}
