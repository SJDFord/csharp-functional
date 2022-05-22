using Demo.Either;

namespace Demo
{
    public class Entity
    {
        public string Name { get; set; }
    }

    public class Report : Entity
    {
        public List<int> Data { get; set; }
    }

    public class EntityManager<T> where T : Entity
    {
        public Either<Failed, List<T>> GetAll()
        {
            return new Right<Failed, List<T>>(new List<T>());
        }
        public Either<Failed, bool> UpdateMultipleAsync(List<T> entities)
        {
            return new Left<Failed, bool>(new Failed());

            return new Right<Failed, bool>(true);
        }

    }

    public class ReportsBuilder<T> where T : Entity
    {
        public Either<Failed, List<T>> Build(List<T> currentReports)
        {

            return new Right<Failed, List<T>>(new List<T>());
        }
    }

    class ReportsGenerator<T> where T : Entity
    {
        private readonly ReportsBuilder<T> _reportsBuilder;
        private readonly EntityManager<T> _reportManager;

        public ReportsGenerator(ReportsBuilder<T> reportsBuilder, EntityManager<T> reportManager)
        {
            _reportsBuilder = reportsBuilder;
            _reportManager = reportManager;
        }

        public bool GenerateReports_LineByLine()
        {
            var currentReportsResult = _reportManager.GetAll();
            var buildReportsResult = currentReportsResult.MapRight(_reportsBuilder.Build).ReduceLeft(failed => new Left<Failed, List<T>>(failed));
            var updateMultipleResult = buildReportsResult.MapRight(_reportManager.UpdateMultipleAsync).ReduceLeft(failed => new Left<Failed, bool>(failed));
            var result = updateMultipleResult.ReduceLeft(failed => false);
            return result;
        }

        public bool GenerateReports_Chained() 
        {
            var result = _reportManager.GetAll()
                .MapRight(_reportsBuilder.Build)
                .ReduceLeft(failed => new Left<Failed, List<T>>(failed))
                .MapRight(_reportManager.UpdateMultipleAsync)
                .ReduceLeft(failed => new Left<Failed, bool>(failed))
                .ReduceLeft(failed => false);
            return result;
        }

        public bool GenerateReports_ChainedExtensionMethod()
        {
            var result = _reportManager.GetAll()
                .ChainRight(_reportsBuilder.Build)
                .ChainRight(_reportManager.UpdateMultipleAsync)
                .ReduceLeft(failed => false);
            return result;
        }
    }
}
