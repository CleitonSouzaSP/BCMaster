using MediatR;

namespace BC.Kernel
{
    public struct Result<TFailure, TSuccess>
    {

        public TFailure Failure
        {
            get;
            internal set;
        }

        public TSuccess Success
        {
            get;
            internal set;
        }

        public bool IsFailure
        {
            get;
        }

        public bool IsSuccess => !IsFailure;

        internal Result(TFailure failure)
        {
            IsFailure = true;
            Failure = failure;
            Success = default(TSuccess);
        }

        internal Result(TSuccess success)
        {
            IsFailure = false;
            Failure = default(TFailure);
            Success = success;
        }

        public TResult Match<TResult>(Func<TFailure, TResult> failure, Func<TSuccess, TResult> success)
        {
            if (!IsFailure)
            {
                return success(Success);
            }

            return failure(Failure);
        }

        public Unit Match(Action<TFailure> failure, Action<TSuccess> success)
        {
            return Match(Helpers.ToFunc(failure), Helpers.ToFunc(success));
        }

        public static implicit operator Result<TFailure, TSuccess>(TFailure failure)
        {
            return new Result<TFailure, TSuccess>(failure);
        }

        public static implicit operator Result<TFailure, TSuccess>(TSuccess success)
        {
            return new Result<TFailure, TSuccess>(success);
        }

        public static Result<TFailure, TSuccess> Of(TSuccess obj)
        {
            return obj;
        }

        public static Result<TFailure, TSuccess> Of(TFailure obj)
        {
            return obj;
        }

    }

    public static class Result
    {
        public static Result<Exception, TSuceess> Run<TSuceess>(this Func<TSuceess> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        public static Result<Exception, Unit> Run<TSuccess>(this Action action)
        {
            return Helpers.ToFunc(action).Run();
        }
        public static Result<Exception, TSuccess> Run<TSuccess>(this Exception ex, Exception resultValidation)
        {
            return ex;
        }
        public static Result<Exception, IQueryable<TSuccess>> AsResult<TSuccess>(this IEnumerable<TSuccess> source)
        {
            return Run(() => source.AsQueryable());
        }

        public static async Task<Result<Exception, TSuccess>> Run<TSuccess>(Func<Task<TSuccess>> func)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
