namespace Demo.Either
{
    public abstract class Either<TLeft, TRight>
    {
        public abstract Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> mapping);

        public abstract Either<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> mapping);

        public abstract TLeft ReduceRight(Func<TRight, TLeft> mapping);

        public abstract TRight ReduceLeft(Func<TLeft, TRight> mapping);
    }
}
