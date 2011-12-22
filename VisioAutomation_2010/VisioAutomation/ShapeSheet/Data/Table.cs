﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VA=VisioAutomation;

namespace VisioAutomation.ShapeSheet.Data
{

    /// <summary>
    /// Used to store the output of the QueryRows and QueryCells methods. Stores a string for the formula and a typed value (int|bool|double|string) for the result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Table<T> : IEnumerable<TableRow<T>>
    {
        private readonly T[,] _values;
        private readonly int rowcount;
        private readonly TableColumnList<T> _cols;

        public TableRowGroupList Groups { get; private set; }

        internal Table(int rows, int cols, TableRowGroupList groups, T[,] values)
        {
            this._values = values;
            this.Groups = groups;
            this.rowcount = rows;
            this._cols = new TableColumnList<T>(this, cols);
        }

        public T this[int row, int column]
        {
            get { return this._values[row, column]; }
            set { this._values[row, column] = value; }
        }

        public T this[int row, VA.ShapeSheet.Query.QueryColumn column]
        {
            get
            {
                if (column==null)
                {
                    throw new System.ArgumentNullException("column");
                }
                return this._values[row, column.Ordinal];
            }
            set { this._values[row, column.Ordinal] = value; }
        }

        public TableColumnList<T> Columns
        {
            get { return this._cols; }
        }


        public IEnumerator<TableRow<T>> GetEnumerator()
        {
            for (int i = 0; i < this.rowcount; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()     // Explicit implementation
        {                                           // keeps it hidden.
            return GetEnumerator();
        }

        public int Count
        {
            get { return this.rowcount; }
        }

        public TableRow<T> this[int index]
        {
            get { return new TableRow<T>(this,index); }
        }

    }
}