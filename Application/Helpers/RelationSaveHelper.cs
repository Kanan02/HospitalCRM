using Application.Interfaces.IUoW;

namespace Application.Helpers
{
    public class RelationSaveHelper
    {
        public static async Task<Tuple<List<T>, List<T>>> SaveOnaToManyList<T>(List<T> newList, List<T> oldList, Func<T, List<T>, bool> func, IUoW unitOfWork) where T : class
        {
            var deleted = oldList.Where(sm => !func.Invoke(sm, newList)).ToList();
            var added = newList.Where(sm => !func.Invoke(sm, oldList)).ToList();

            unitOfWork.Repository<T>().RemoveAll(deleted);
            await unitOfWork.Repository<T>().AddAllAsync(added);

            return new Tuple<List<T>, List<T>>(deleted, added);
        }

        public static async Task<(List<T> added, List<T> deleted)> SaveOnaToManyList<T>(List<T> newList, List<T> oldList, string propName, IUoW unitOfWork, bool isSetAllProp = false) where T : class
        {
            var deleted = oldList.Where(sm => newList.All(nl => !nl.IsEqualValue(sm, propName))).ToList();
            var added = newList.Where(nl => oldList.All(sm => !sm.IsEqualValue(nl, propName))).ToList();

            if (isSetAllProp)
                oldList.ForEach(old =>
                {
                    var @new = newList.FirstOrDefault(nl => nl.IsEqualValue(old, propName));
                    if (@new != null)
                        unitOfWork.Repository<T>().Update(@new, old);
                });

            unitOfWork.Repository<T>().RemoveAll(deleted);
            await unitOfWork.Repository<T>().AddAllAsync(added);

            return (added, deleted);
        }

        public static (List<T> added, List<T> deleted) GetUpdatedList<T>(List<T> newList, List<T> oldList, string propName) where T : class
        {
            var deleted = oldList.Where(sm => newList.All(nl => !nl.IsEqualValue(sm, propName))).ToList();
            var added = newList.Where(nl => oldList.All(sm => !sm.IsEqualValue(nl, propName))).ToList();

            return (added, deleted);
        }

    }
}
