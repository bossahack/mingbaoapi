using System;
using System.Transactions;

namespace Book.Service
{
    /// ///
    public class TransactionHelper
    {
        /// ///
        public static T Run<T>(Func<T> func, TransactionScopeOption scopeOption = TransactionScopeOption.Required, TransactionOptions? transactionOptions = null)
        {
            using (TransactionScope scope = new TransactionScope(scopeOption, transactionOptions ?? CreateDefaultTransactionOptions()))
            {
                var result = func();


                scope.Complete();


                return result;
            }
        }

        /// ///
        public static void Run(Action action, TransactionScopeOption scopeOption = TransactionScopeOption.Required, TransactionOptions? transactionOptions = null)
        {
            using (TransactionScope scope = new TransactionScope(scopeOption, transactionOptions ?? CreateDefaultTransactionOptions()))
            {
                action();


                scope.Complete();


            }
        }


        private static TransactionOptions CreateDefaultTransactionOptions()
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout.Add(TimeSpan.FromSeconds(100000));
            return options;
        }
    }
}
