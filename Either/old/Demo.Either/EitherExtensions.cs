namespace Demo.Either
{
    public static class EitherExtensions
    {
        public static Either<TLeft, TNewRight> ChainRight<TLeft, TRight, TNewRight>(this Either<TLeft, TRight> either, Func<TRight, Either<TLeft, TNewRight>> mapping) {
            return either.MapRight(mapping).ReduceLeft(left => new Left<TLeft, TNewRight>(left));
        }

        public static Either<TNewLeft, TRight> ChainLeft<TLeft, TRight, TNewLeft>(this Either<TLeft, TRight> either, Func<TLeft, Either<TNewLeft, TRight>> mapping)
        {
            return either.MapLeft(mapping).ReduceRight(right => new Right<TNewLeft, TRight>(right));
        }
    }
}
