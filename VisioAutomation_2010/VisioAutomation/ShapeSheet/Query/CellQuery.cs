﻿using VA = VisioAutomation;
using System.Collections.Generic;
using System.Linq;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioAutomation.ShapeSheet.Query
{
    public class CellQuery : QueryBase
    {
        public CellQuery() :
            base()
        {
        }

        public QueryColumn AddColumn(SRC src)
        {
            var col = new QueryColumn(this.Columns.Count, src, null);
            this.AddColumn(col);
            return col;
        }

        public QueryColumn AddColumn(SRC src, string name)
        {
            var col = new QueryColumn(this.Columns.Count, src, name);
            this.AddColumn(col);
            return col;
        }

        public VA.ShapeSheet.Data.Table<CellData<T>> GetFormulasAndResults<T>(IVisio.Shape shape)
        {
            var qds = this._Execute<T>(shape, true, true);
            return qds.CreateMergedTable();
        }
        
        public VA.ShapeSheet.Data.Table<string> GetFormulas(IVisio.Shape shape)
        {
            var qds = this._Execute<double>(shape, true, false);
            return qds.Formulas;
        }

        public VA.ShapeSheet.Data.Table<T> GetResults<T>(IVisio.Shape shape)
        {
            var qds = this._Execute<T>(shape, false, true);
            return qds.Results;
        }

        private VA.Internal.QueryResults<T> _Execute<T>(IVisio.Shape shape, bool getformulas, bool getresults)
        {
            if (shape == null)
            {
                throw new System.ArgumentNullException("shape");
            }

            VA.ShapeSheet.ShapeSheetHelper.EnforceValidResultType(typeof(T));

            var shapeids = new[] { shape.ID };
            var groupcounts = new[] { 1 };
            int rowcount = shapeids.Count();
            
            // Build the Stream
            var srcs = this.Columns.Select(col => col.SRC).ToList();

            var stream = VA.ShapeSheet.SRC.ToStream(srcs);
            var unitcodes = getresults ? this.CreateUnitCodeArrayForRows(1) : null;
            var formulas = getformulas ? VA.ShapeSheet.ShapeSheetHelper.GetFormulasU(shape, stream) : null;
            var results = getresults ? VA.ShapeSheet.ShapeSheetHelper.GetResults<T>(shape, stream, unitcodes) : null;
            var groups = VA.ShapeSheet.Query.QueryBase.Build(shapeids, groupcounts, rowcount);
            var table = new VA.Internal.QueryResults<T>(formulas, results, shapeids, this.Columns.Count, rowcount, groups);

            return table;
        }

        public VA.ShapeSheet.Data.Table<VA.ShapeSheet.CellData<T>> GetFormulasAndResults<T>(
        IVisio.Page page,
        IList<int> shapeids)
        {
            var table = this._Execute<T>(page, shapeids, true, true);
            return table.CreateMergedTable();
        }

        public VA.ShapeSheet.Data.Table<string> GetFormulas(
            IVisio.Page page,
            IList<int> shapeids)
        {
            var table = this._Execute<double>(page, shapeids, true, false);
            return table.Formulas;
        }

        public VA.ShapeSheet.Data.Table<T> GetResults<T>(
            IVisio.Page page,
            IList<int> shapeids)
        {
            var table = this._Execute<T>(page, shapeids, false, true);
            return table.Results;
        }

        private VA.Internal.QueryResults<T> _Execute<T>(
            IVisio.Page page,
            IList<int> shapeids, bool getformulas, bool getresults)
        {
            if (page == null)
            {
                throw new System.ArgumentNullException("page");
            }

            if (shapeids == null)
            {
                throw new System.ArgumentNullException("shapeids");
            }

            VA.ShapeSheet.ShapeSheetHelper.EnforceValidResultType(typeof(T));

            var srcs = Columns.Select(i => i.SRC).ToList();         

            var groupcounts = new int[shapeids.Count];
            for (int i = 0; i < shapeids.Count; i++)
            {
                groupcounts[i] = 1;
            }

            int rowcount = shapeids.Count;
            int total_cells = rowcount * this.Columns.Count;

            // Build the Stream
            var sidsrcs = new List<VA.ShapeSheet.SIDSRC>(total_cells);
            foreach (var id in shapeids)
            {
                foreach (var src in srcs)
                {
                    var sidsrc = new VA.ShapeSheet.SIDSRC((short) id, src);
                    sidsrcs.Add(sidsrc);
                }
            }
            var stream = VA.ShapeSheet.SIDSRC.ToStream(sidsrcs);
            var unitcodes = getresults ? CreateUnitCodeArrayForRows(1) : null;
            var formulas = getformulas ? VA.ShapeSheet.ShapeSheetHelper.GetFormulasU(page, stream) : null;
            var results = getresults ? VA.ShapeSheet.ShapeSheetHelper.GetResults<T>(page, stream, unitcodes) : null;
            var groups = VA.ShapeSheet.Query.QueryBase.Build(shapeids, groupcounts, rowcount);
            var table = new VA.Internal.QueryResults<T>(formulas, results, shapeids, this.Columns.Count, rowcount, groups);

            return table;
        }
    }
}