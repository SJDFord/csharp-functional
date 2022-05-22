using Either.Lib;

namespace Either.Example
{
    /// <summary>
    /// Example from https://stackoverflow.com/questions/63231450/how-to-use-the-either-type-in-c
    /// </summary>
    public class StackOverflowExample
    {
        public void Demostrate() {
            var input = 42;
            // All three methods have identical behaviour but varying levels of syntactic verbosity
            GetResponse_LineByLine(input);
            GetResponse_Chained(input);
            GetResponse_Chained_ExtensionMethods(input);
        }

        private Response GetResponse_LineByLine(int input) {
            var eitherUserOrError = ReadUser(input);
            var product = eitherUserOrError.MapLeft(FindProduct);
            var eitherProductDetailsOrError = product.MapLeft(ReadTechnicalDetails).ReduceRight(error => new Right<ProductDetails, Error>(error));
            var eitherResponseOrError = eitherProductDetailsOrError.MapLeft(View);
            var response = eitherResponseOrError.ReduceRight(ErrorView);
            return response;
        }

        private Response GetResponse_Chained(int input) {
            var response = ReadUser(input)
                .MapLeft(FindProduct)
                .MapLeft(ReadTechnicalDetails)
                .ReduceRight(error => new Right<ProductDetails, Error>(error))
                .MapLeft(View)
                .ReduceRight(ErrorView);
            return response;
        }

        private Response GetResponse_Chained_ExtensionMethods(int input) {
            var response = ReadUser(input)
                .MapLeft(FindProduct)
                .ChainLeft(ReadTechnicalDetails)
                .MapLeft(View)
                .ReduceRight(ErrorView);
            return response;
        }

        private Either<User, Error> ReadUser(int input)
        {
            return new Left<User, Error>(new User());
        }

        private Product FindProduct(User user)
        {
            return new Product();
        }

        private Either<ProductDetails, Error> ReadTechnicalDetails(Product product)
        {
            return new Left<ProductDetails, Error>(new ProductDetails());
        }

        private Response View(ProductDetails product)
        {
            return new Response();
        }

        private Response ErrorView(Error error)
        {
            return new Response();
        }
    }

    public class User { };
    public class Product { };
    public class ProductDetails { };
    public class Error { };
    public class Response { };
}