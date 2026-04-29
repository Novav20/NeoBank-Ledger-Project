namespace NeoBank.Ledger.Domain.Enums;

public enum ValidationLevel
{
    NoValidation,
    SyntaxValid,
    SchemaValid,
    MessageValid,
    RuleValid,
    MarketPracticeValid,
    BusinessProcessValid,
    CompletelyValid
}
