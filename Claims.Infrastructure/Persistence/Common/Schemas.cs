namespace Claims.Infrastructure.Persistence.Common;

public class Schemas
{
    public const string Audit = "audit";

    public class Tables
    {
        public const string ClaimAudit = "ClaimAudits";
        public const string CoverAudit = "CoverAudits";
    }

    public class Sequences
    {
        public class LeadCodes
        {
            public const string Name = "LeadCodesSequence";
            public const int StartsAt = 1255;
            public const int IncrementBy = 1;
        }
    }
}
