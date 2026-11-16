namespace Ewallet.Mvc.Models;

public sealed class AuditLog
{
    private AuditLog() { } 

    public long Id {get; private set;}
    public string Action {get; private set;} = string.Empty;
    public string EntityType {get; private set;}= string.Empty;
    public string? EntityId {get; private set;}
    public OutcomeEnum Outcome {get; private set;}
    public string? Description {get; private set;}
    public string? CorrelationId {get; private set;}
    public DateTime CreatedAtUtc {get; private set;}

    // Foreign Key
    public long? ActorUserId {get; private set;}
    public User? ActorUser {get; private set;}
    public long? TransactionId {get; private set;}
    public Transaction? Transaction {get; private set;}

    public AuditLog(long id, string action, string entityType,string? entityId, OutcomeEnum outcome, string? description, string? correlationId, DateTime createdAtUtc)
    {
        Id=id;
        Action=action;
        EntityType=entityType;
        EntityId=entityId;
        Outcome=outcome;
        Description=description;
        CorrelationId=correlationId;
        CreatedAtUtc=createdAtUtc;
    }


    public static AuditLog Create(
        string action, string entityType, string? entityId,
        OutcomeEnum outcome, string? description,
        long? actorUserId, long? transactionId,
        string? correlationId, DateTime nowUtc)
        => new()
        {
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Outcome = outcome,
            Description = description,
            ActorUserId = actorUserId,
            TransactionId = transactionId,
            CorrelationId = correlationId,
            CreatedAtUtc = nowUtc
        };
	

}


public enum OutcomeEnum{
    SUCCESS, 
    REJECTED, 
    FAILED
}

