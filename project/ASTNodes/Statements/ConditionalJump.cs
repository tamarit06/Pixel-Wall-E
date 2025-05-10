class ConditionalJump : Statement
 {
    public string TargetLabel { get; }
    public Expression Condition { get; }
    public ConditionalJump(string label, Expression cond)
     {
        TargetLabel = label;
        Condition = cond;
    }
 }
