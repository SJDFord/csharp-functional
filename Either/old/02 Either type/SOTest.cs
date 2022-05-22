using Demo.Either;

namespace Demo
{
    // Test from SO answer: https://stackoverflow.com/questions/63231450/how-to-use-the-either-type-in-c

    public static class SOTest
    {
        public static void Perform() {
            var input = 42;

            var lineByLineResponse = LineByLine(input);
            var chainedResponse = Chained(input);
        }

        public static Response LineByLine(int input) {
            var userEither = DemoFunctions.ReadUser(input);
            var productEither = userEither.MapLeft(DemoFunctions.FindProduct);
            var productDetailsEither = productEither.MapLeft(DemoFunctions.ReadTechnicalDetails).ReduceRight(error => new Right<ProductDetails, Error>(error));
            var responseEither = productDetailsEither.MapLeft(DemoFunctions.View);
            var response = responseEither.ReduceRight(DemoFunctions.ErrorView);
            return response;
        }

        public static Response Chained(int input) {
            var response = DemoFunctions.ReadUser(input)
                .MapLeft(DemoFunctions.FindProduct)
                .MapLeft(DemoFunctions.ReadTechnicalDetails).ReduceRight(error => new Right<ProductDetails, Error>(error))
                .MapLeft(DemoFunctions.View)
                .ReduceRight(DemoFunctions.ErrorView);
            return response;
        }

        public static Response ChainedExt(int input) {
            var response = DemoFunctions.ReadUser(input)
                .MapLeft(DemoFunctions.FindProduct)
                .ChainLeft(DemoFunctions.ReadTechnicalDetails)
                .MapLeft(DemoFunctions.View)
                .ReduceRight(DemoFunctions.ErrorView);
            return response;
        }
    }

    public class User { };
    public class Product { };
    public class ProductDetails { };
    public class Error { };
    public class Response { };

    static class DemoFunctions
    {
        public static Either<User, Error> ReadUser(int input)
        {
            return new Left<User, Error>(new User());
        }

        public static Product FindProduct(User user)
        {
            return new Product();
        }

        public static Either<ProductDetails, Error> ReadTechnicalDetails(Product product)
        {
            return new Left<ProductDetails, Error>(new ProductDetails());
        }

        public static Response View(ProductDetails product)
        {
            return new Response();
        }

        public static Response ErrorView(Error error)
        {
            return new Response();
        }
    }
}
