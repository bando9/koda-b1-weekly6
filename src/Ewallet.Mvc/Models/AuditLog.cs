namespace Ewallet.Mvc.Models;

public sealed class AuditLog
{
    public long Id {get; private set;}
    public string Action {get; private set;} = string.Empty;
    public string EntityType {get; private set;}= string.Empty;
    public string? EntityId {get; private set;}= string.Empty;
    public OutcomeEnum Outcome {get; private set;}
    public string? Description {get; private set;}= string.Empty;
    public string? CorrelationId {get; private set;}= string.Empty;
    public DateTime CreatedAtUtc {get; private set;}

    // Foreign Key
    public long? ActorUserId {get; private set;}
    public User? ActorUser {get; private set;} = null!;
    public long? TransactionId {get; private set;}
    public Transaction? Transaction {get; private set;} = null!;
	

}


public enum OutcomeEnum{
    SUCCESS, 
    REJECTED, 
    FAILED
}