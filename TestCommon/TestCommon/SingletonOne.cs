namespace TestCommon
{
    public class SingletonOne
    {
        public SingletonOne(IItransientOne transientOne)
        {
            TransientOne = transientOne;
        }

        public IItransientOne TransientOne { get; }
    }
}
