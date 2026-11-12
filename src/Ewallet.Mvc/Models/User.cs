namespace Ewallet.Mvc.Models;

public sealed class User
{
    public long Id {get; private set;}
    public string Username {get; private set;} = string.Empty;
    public string FullName {get; private set;} = string.Empty;    
    public string Email {get; private set;} = string.Empty;    
    public string Phone {get; private set;} = string.Empty;    
    public string? PasswordHash {get; private set;}    
    public UserStatusEnum Status {get; private set;}    
    public DateTime CreatedAtUtc {get; private set;}
    public DateTime? UpdatedAtUtc {get; private set;}

    // constraint wallet
    public Wallet? Wallet {get; private set;}
    public ICollection<AuditLog> AuditLogs {get; private set;} = new List<AuditLog>();

}

public enum UserStatusEnum {
    ACTIVE, 
    SUSPENDED
}