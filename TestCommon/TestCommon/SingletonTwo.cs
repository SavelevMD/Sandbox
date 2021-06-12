namespace TestCommon
{
    public class SingletonTwo
    {
        public SingletonTwo(IItransientOne transientOne)
        {
            TransientOne = transientOne;
        }

        public IItransientOne TransientOne { get; }
    }
}
