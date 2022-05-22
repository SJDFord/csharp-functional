namespace Either.Lib
{
    public class Left<TLeft, TRight> : Either<TLeft, TRight>
    {
        TLeft Value { get; }

        public Left(TLeft value)
        {
            Value = value;
        }

        public override Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> mapping) =>
            new Left<TNewLeft, TRight>(mapping(Value));

        public override Either<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> mapping) =>
            new Left<TLeft, TNewRight>(Value);

        public override TLeft ReduceRight(Func<TRight, TLeft> mapping) => Value;

        public override TRight ReduceLeft(Func<TLeft, TRight> mapping) => mapping(Value);
    }
}
