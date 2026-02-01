namespace NovelGame.Master.Scripts.Editor
{
    public readonly struct ScenarioArguments
    {
        public ScenarioArguments(string[] args)
        {
            _args = args;
        }

        public Argument this[int index] => new(_args[index]);

        private readonly string[] _args;

        public readonly struct Argument
        {
            public Argument(string value)
            {
                _value = value;
            }

            public static implicit operator string(Argument arg) => arg._value;
            public static implicit operator int(Argument arg) => int.Parse(arg._value);
            public static implicit operator float(Argument arg) => float.Parse(arg._value);
            
            private readonly string _value;
        }
    }
}
