using System;
using System.Collections.Generic;

namespace UnitOfWork.UnitTestFakes
{
    public class FakeDbConnection
    {
        private readonly Dictionary<string, (object obj, Action commitHandler)> _trackedObjects =
            new Dictionary<string, (object obj, Action commitHandler)>();

        public void Commit()
        {
            foreach (var (_, commitHandler) in _trackedObjects.Values)
            {
                commitHandler();
            }
        }

        public void EnsureObjectIsTracked(string key, object originalValue, Action<object> finalCommittedValueHandler)
        {
            if (!_trackedObjects.ContainsKey(key))
            {
                _trackedObjects[key] = (originalValue, () => finalCommittedValueHandler(_trackedObjects[key].obj));
            }
        }

        public void WriteObject(string key, object newValue)
        {
            _trackedObjects[key] = (newValue, _trackedObjects[key].commitHandler);
        }

        public object ReadObject(string key)
        {
            return _trackedObjects[key].obj;
        }
    }
}