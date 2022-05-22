using Either.Example.Common;
using Either.Lib;

namespace Either.Example
{
    /// <summary>
    /// An example to demonstrate how using the either type can reduce branching when writing business logic
    /// </summary>
    public class BusinessLogicExample
    {
        private readonly ReportsBuilder _reportsBuilder;
        private readonly ReportManager _reportManager;

        public BusinessLogicExample()
        {
            _reportsBuilder = new ReportsBuilder();
            _reportManager = new ReportManager();
        }

        public void Demonstrate() {
            GenerateReports_LineByLine();
            GenerateReports_Chained();
            GenerateReports_Chained_ExtensionMethod();
        }

        public bool GenerateReports_LineByLine()
        {
            var currentReportsResult = _reportManager.GetAll();
            var buildReportsResult = currentReportsResult.MapRight(_reportsBuilder.Build).ReduceLeft(failed => new Left<Failed, List<Report>>(failed));
            var updateMultipleResult = buildReportsResult.MapRight(_reportManager.UpdateMultipleAsync).ReduceLeft(failed => new Left<Failed, bool>(failed));
            var result = updateMultipleResult.ReduceLeft(failed => false);
            return result;
        }

        public bool GenerateReports_Chained()
        {
            var result = _reportManager.GetAll()
                .MapRight(_reportsBuilder.Build)
                .ReduceLeft(failed => new Left<Failed, List<Report>>(failed))
                .MapRight(_reportManager.UpdateMultipleAsync)
                .ReduceLeft(failed => new Left<Failed, bool>(failed))
                .ReduceLeft(failed => false);
            return result;
        }

        public bool GenerateReports_Chained_ExtensionMethod()
        {
            var result = _reportManager.GetAll()
                .ChainRight(_reportsBuilder.Build)
                .ChainRight(_reportManager.UpdateMultipleAsync)
                .ReduceLeft(failed => false);
            return result;
        }

    }

    /// <summary>
    /// An example domain model object
    /// </summary>
    public class Report
    {
        public List<int> Data { get; set; }
    }

    /// <summary>
    /// An example manager for the report entity with methods for getting reports and updating multiple at once
    /// </summary>
    public class ReportManager
    {
        public Either<Failed, List<Report>> GetAll()
        {
            return new Right<Failed, List<Report>>(new List<Report>());
        }

        public Either<Failed, bool> UpdateMultipleAsync(List<Report> entities)
        {
            return new Left<Failed, bool>(new Failed());

            return new Right<Failed, bool>(true);
        }
    }

    /// <summary>
    /// An example builder for the report entity that accepts existing reports and returns updated ones
    /// </summary>
    public class ReportsBuilder
    {
        public Either<Failed, List<Report>> Build(List<Report> currentReports)
        {

            return new Right<Failed, List<Report>>(new List<Report>());
        }
    }
}
