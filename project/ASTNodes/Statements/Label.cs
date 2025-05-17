class Label : Statement 
{
    public string Value{ get; }
    public Label(string value) => Value = value;
}
