namespace Demo.Either
{
    public class Right<TLeft, TRight> : Either<TLeft, TRight>
    {
        TRight Value { get; }

        public Right(TRight value)
        {
            Value = value;
        }

        public override Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> mapping) =>
            new Right<TNewLeft, TRight>(Value);

        public override Either<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> mapping) =>
            new Right<TLeft, TNewRight>(mapping(Value));

        public override TLeft ReduceRight(Func<TRight, TLeft> mapping) => mapping(Value);

        public override TRight ReduceLeft(Func<TLeft, TRight> mapping) => Value;
    }
}
