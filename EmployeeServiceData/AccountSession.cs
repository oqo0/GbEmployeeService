using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeServiceData;

[Table("AccountSessions")]
public class AccountSession
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SessionId { get; set; }
    
    [Required]
    [StringLength(384)]
    public string SessionToken { get; set; }
    
    [ForeignKey(nameof(Account))]
    public int AccountId { get; set; }
    
    [Column(TypeName = "datetime")]
    public DateTime TimeCreated { get; set; }
    
    [Column(TypeName = "datetime")]
    public DateTime TimeLastRequest { get; set; }
    
    public bool IsClosed { get; set; }
    
    [Column(TypeName = "datetime")]
    public DateTime? TimeClosed { get; set; }
    
    public virtual Account Account { get; set; }
}