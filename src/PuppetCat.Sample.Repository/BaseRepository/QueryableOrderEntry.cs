using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PuppetCat.Sample.Repository
{
    public class QueryableOrderEntry<TSource, TKey>
    {
        /// <summary>
        /// Sorting extension
        /// </summary>
        /// <param name="expression"></param>
        public QueryableOrderEntry(Expression<Func<TSource, TKey>> expression)
        {
            this.Expression = expression;
            OrderDirection = OrderDirection.ASC;
        }

        public QueryableOrderEntry(Expression<Func<TSource, TKey>> expression, OrderDirection orderDirection)
        {
            this.Expression = expression;
            OrderDirection = orderDirection;
        }

        public Expression<Func<TSource, TKey>> Expression
        {
            get;
            set;
        }

        public OrderDirection OrderDirection
        {
            get;
            set;
        }
    }
    public enum OrderDirection
    {
        ASC, DESC
    }
}
