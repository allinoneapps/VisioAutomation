﻿using System.Collections.Generic;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;
using VisioAutomation.Extensions;

namespace VisioAutomation.Format
{
    public class ShapeFormatCells : VA.ShapeSheet.CellGroups.CellGroup
    {
        public VA.ShapeSheet.CellData<int> FillBkgnd { get; set; }
        public VA.ShapeSheet.CellData<double> FillBkgndTrans { get; set; }
        public VA.ShapeSheet.CellData<int> FillForegnd { get; set; }
        public VA.ShapeSheet.CellData<double> FillForegndTrans { get; set; }
        public VA.ShapeSheet.CellData<int> FillPattern { get; set; }
        public VA.ShapeSheet.CellData<double> ShapeShdwObliqueAngle { get; set; }
        public VA.ShapeSheet.CellData<double> ShapeShdwOffsetX { get; set; }
        public VA.ShapeSheet.CellData<double> ShapeShdwOffsetY { get; set; }
        public VA.ShapeSheet.CellData<double> ShapeShdwScaleFactor { get; set; }
        public VA.ShapeSheet.CellData<int> ShapeShdwType { get; set; }
        public VA.ShapeSheet.CellData<int> ShdwBkgnd { get; set; }
        public VA.ShapeSheet.CellData<double> ShdwBkgndTrans { get; set; }
        public VA.ShapeSheet.CellData<int> ShdwForegnd { get; set; }
        public VA.ShapeSheet.CellData<double> ShdwForegndTrans { get; set; }
        public VA.ShapeSheet.CellData<int> ShdwPattern { get; set; }
        public VA.ShapeSheet.CellData<int> BeginArrow { get; set; }
        public VA.ShapeSheet.CellData<double> BeginArrowSize { get; set; }
        public VA.ShapeSheet.CellData<int> EndArrow { get; set; }
        public VA.ShapeSheet.CellData<double> EndArrowSize { get; set; }
        public VA.ShapeSheet.CellData<int> LineCap { get; set; }
        public VA.ShapeSheet.CellData<int> LineColor { get; set; }
        public VA.ShapeSheet.CellData<double> LineColorTrans { get; set; }
        public VA.ShapeSheet.CellData<int> LinePattern { get; set; }
        public VA.ShapeSheet.CellData<double> LineWeight { get; set; }
        public VA.ShapeSheet.CellData<double> Rounding { get; set; }

        public override void ApplyFormulas(ApplyFormula func)
        {
            func(ShapeSheet.SRCConstants.FillBkgnd, this.FillBkgnd.Formula);
            func(ShapeSheet.SRCConstants.FillBkgndTrans, this.FillBkgndTrans.Formula);
            func(ShapeSheet.SRCConstants.FillForegnd, this.FillForegnd.Formula);
            func(ShapeSheet.SRCConstants.FillForegndTrans, this.FillForegndTrans.Formula);
            func(ShapeSheet.SRCConstants.FillPattern, this.FillPattern.Formula);
            func(ShapeSheet.SRCConstants.ShapeShdwObliqueAngle, this.ShapeShdwObliqueAngle.Formula);
            func(ShapeSheet.SRCConstants.ShapeShdwOffsetX, this.ShapeShdwOffsetX.Formula);
            func(ShapeSheet.SRCConstants.ShapeShdwOffsetY, this.ShapeShdwOffsetY.Formula);
            func(ShapeSheet.SRCConstants.ShapeShdwScaleFactor, this.ShapeShdwScaleFactor.Formula);
            func(ShapeSheet.SRCConstants.ShapeShdwType, this.ShapeShdwType.Formula);
            func(ShapeSheet.SRCConstants.ShdwBkgnd, this.ShdwBkgnd.Formula);
            func(ShapeSheet.SRCConstants.ShdwBkgndTrans, this.ShdwBkgndTrans.Formula);
            func(ShapeSheet.SRCConstants.ShdwForegnd, this.ShdwForegnd.Formula);
            func(ShapeSheet.SRCConstants.ShdwForegndTrans, this.ShdwForegndTrans.Formula);
            func(ShapeSheet.SRCConstants.ShdwPattern, this.ShdwPattern.Formula);
            func(ShapeSheet.SRCConstants.BeginArrow, this.BeginArrow.Formula);
            func(ShapeSheet.SRCConstants.BeginArrowSize, this.BeginArrowSize.Formula);
            func(ShapeSheet.SRCConstants.EndArrow, this.EndArrow.Formula);
            func(ShapeSheet.SRCConstants.EndArrowSize, this.EndArrowSize.Formula);
            func(ShapeSheet.SRCConstants.LineCap, this.LineCap.Formula);
            func(ShapeSheet.SRCConstants.LineColor, this.LineColor.Formula);
            func(ShapeSheet.SRCConstants.LineColorTrans, this.LineColorTrans.Formula);
            func(ShapeSheet.SRCConstants.LinePattern, this.LinePattern.Formula);
            func(ShapeSheet.SRCConstants.LineWeight, this.LineWeight.Formula);
            func(ShapeSheet.SRCConstants.Rounding, this.Rounding.Formula);
        }

        private static ShapeFormatCells get_cells_from_row(ShapeFormatQuery query, VA.ShapeSheet.Data.Table<VA.ShapeSheet.CellData<double>> table, int row)
        {
            var cells = new ShapeFormatCells();
            cells.FillBkgnd = table[row,query.FillBkgnd].ToInt();
            cells.FillBkgndTrans = table[row,query.FillBkgndTrans];
            cells.FillForegnd = table[row,query.FillForegnd].ToInt();
            cells.FillForegndTrans = table[row,query.FillForegndTrans];
            cells.FillPattern = table[row,query.FillPattern].ToInt();
            cells.ShapeShdwObliqueAngle = table[row,query.ShapeShdwObliqueAngle];
            cells.ShapeShdwOffsetX = table[row,query.ShapeShdwOffsetX];
            cells.ShapeShdwOffsetY = table[row,query.ShapeShdwOffsetY];
            cells.ShapeShdwScaleFactor = table[row,query.ShapeShdwScaleFactor];
            cells.ShapeShdwType = table[row,query.ShapeShdwType].ToInt();
            cells.ShdwBkgnd = table[row,query.ShdwBkgnd].ToInt();
            cells.ShdwBkgndTrans = table[row,query.ShdwBkgndTrans];
            cells.ShdwForegnd = table[row,query.ShdwForegnd].ToInt();
            cells.ShdwForegndTrans = table[row,query.ShdwForegndTrans];
            cells.ShdwPattern = table[row,query.ShdwPattern].ToInt();
            cells.BeginArrow = table[row,query.BeginArrow].ToInt();
            cells.BeginArrowSize = table[row,query.BeginArrowSize];
            cells.EndArrow = table[row,query.EndArrow].ToInt();
            cells.EndArrowSize = table[row,query.EndArrowSize];
            cells.LineCap = table[row,query.LineCap].ToInt();
            cells.LineColor = table[row,query.LineColor].ToInt();
            cells.LineColorTrans = table[row,query.LineColorTrans];
            cells.LinePattern = table[row,query.LinePattern].ToInt();
            cells.LineWeight = table[row,query.LineWeight];
            cells.Rounding = table[row,query.Rounding];
            return cells;
        }

        public static IList<ShapeFormatCells> GetCells(IVisio.Page page, IList<int> shapeids)
        {
            var query = get_query();
            return VA.ShapeSheet.CellGroups.CellGroup.CellsFromRows(page, shapeids, query, get_cells_from_row);
        }

        public static ShapeFormatCells GetCells(IVisio.Shape shape)
        {
            var query = get_query();
            return VA.ShapeSheet.CellGroups.CellGroup.CellsFromRow(shape, query, get_cells_from_row);
        }

        private static ShapeFormatQuery m_query;
        private static ShapeFormatQuery get_query()
        {
            m_query = m_query ?? new ShapeFormatQuery();
            return m_query;
        }

        class ShapeFormatQuery : VA.ShapeSheet.Query.CellQuery
        {
            public VA.ShapeSheet.Query.QueryColumn FillBkgnd { get; set; }
            public VA.ShapeSheet.Query.QueryColumn FillBkgndTrans { get; set; }
            public VA.ShapeSheet.Query.QueryColumn FillForegnd { get; set; }
            public VA.ShapeSheet.Query.QueryColumn FillForegndTrans { get; set; }
            public VA.ShapeSheet.Query.QueryColumn FillPattern { get; set; }
            public VA.ShapeSheet.Query.QueryColumn ShapeShdwObliqueAngle { get; set; }
            public VA.ShapeSheet.Query.QueryColumn ShapeShdwOffsetX { get; set; }
            public VA.ShapeSheet.Query.QueryColumn ShapeShdwOffsetY { get; set; }
            public VA.ShapeSheet.Query.QueryColumn ShapeShdwScaleFactor { get; set; }
            public VA.ShapeSheet.Query.QueryColumn ShapeShdwType { get; set; }
            public VA.ShapeSheet.Query.QueryColumn ShdwBkgnd { get; set; }
            public VA.ShapeSheet.Query.QueryColumn ShdwBkgndTrans { get; set; }
            public VA.ShapeSheet.Query.QueryColumn ShdwForegnd { get; set; }
            public VA.ShapeSheet.Query.QueryColumn ShdwForegndTrans { get; set; }
            public VA.ShapeSheet.Query.QueryColumn ShdwPattern { get; set; }

            public VA.ShapeSheet.Query.QueryColumn BeginArrow { get; set; }
            public VA.ShapeSheet.Query.QueryColumn BeginArrowSize { get; set; }
            public VA.ShapeSheet.Query.QueryColumn EndArrow { get; set; }
            public VA.ShapeSheet.Query.QueryColumn EndArrowSize { get; set; }
            public VA.ShapeSheet.Query.QueryColumn LineColor { get; set; }
            public VA.ShapeSheet.Query.QueryColumn LineCap { get; set; }
            public VA.ShapeSheet.Query.QueryColumn LineColorTrans { get; set; }
            public VA.ShapeSheet.Query.QueryColumn LinePattern { get; set; }
            public VA.ShapeSheet.Query.QueryColumn LineWeight { get; set; }
            public VA.ShapeSheet.Query.QueryColumn Rounding { get; set; }

            public ShapeFormatQuery() :
                base()
            {
                this.FillBkgnd = this.AddColumn(VA.ShapeSheet.SRCConstants.FillBkgnd, "FillBkgnd");
                this.FillBkgndTrans = this.AddColumn(VA.ShapeSheet.SRCConstants.FillBkgndTrans, "FillBkgndTrans");
                this.FillForegnd = this.AddColumn(VA.ShapeSheet.SRCConstants.FillForegnd, "FillForegnd");
                this.FillForegndTrans = this.AddColumn(VA.ShapeSheet.SRCConstants.FillForegndTrans, "FillForegndTrans");
                this.FillPattern = this.AddColumn(VA.ShapeSheet.SRCConstants.FillPattern, "FillPattern");
                this.ShapeShdwObliqueAngle = this.AddColumn(VA.ShapeSheet.SRCConstants.ShapeShdwObliqueAngle, "ShapeShdwObliqueAngle");
                this.ShapeShdwOffsetX = this.AddColumn(VA.ShapeSheet.SRCConstants.ShapeShdwOffsetX, "ShapeShdwOffsetX");
                this.ShapeShdwOffsetY = this.AddColumn(VA.ShapeSheet.SRCConstants.ShapeShdwOffsetY, "ShapeShdwOffsetY");
                this.ShapeShdwScaleFactor = this.AddColumn(VA.ShapeSheet.SRCConstants.ShapeShdwScaleFactor, "ShapeShdwScaleFactor");
                this.ShapeShdwType = this.AddColumn(VA.ShapeSheet.SRCConstants.ShapeShdwType, "ShapeShdwType");
                this.ShdwBkgnd = this.AddColumn(VA.ShapeSheet.SRCConstants.ShdwBkgnd, "ShdwBkgnd ");
                this.ShdwBkgndTrans = this.AddColumn(VA.ShapeSheet.SRCConstants.ShdwBkgndTrans, "ShdwBkgndTrans");
                this.ShdwForegnd = this.AddColumn(VA.ShapeSheet.SRCConstants.ShdwForegnd, "ShdwForegnd ");
                this.ShdwForegndTrans = this.AddColumn(VA.ShapeSheet.SRCConstants.ShdwForegndTrans, "ShdwForegndTrans");
                this.ShdwPattern = this.AddColumn(VA.ShapeSheet.SRCConstants.ShdwPattern, "ShdwPattern");

                this.BeginArrow = this.AddColumn(VA.ShapeSheet.SRCConstants.BeginArrow, "BeginArrow");
                this.BeginArrowSize = this.AddColumn(VA.ShapeSheet.SRCConstants.BeginArrowSize, "BeginArrowSize");
                this.EndArrow = this.AddColumn(VA.ShapeSheet.SRCConstants.EndArrow, "EndArrow ");
                this.EndArrowSize = this.AddColumn(VA.ShapeSheet.SRCConstants.EndArrowSize, "EndArrowSize");
                this.LineColor = this.AddColumn(VA.ShapeSheet.SRCConstants.LineColor, "LineColor");
                this.LineCap = this.AddColumn(VA.ShapeSheet.SRCConstants.LineCap, "LineCap");
                this.LineColorTrans = this.AddColumn(VA.ShapeSheet.SRCConstants.LineColorTrans, "LineColorTrans");
                this.LinePattern = this.AddColumn(VA.ShapeSheet.SRCConstants.LinePattern, "LinePattern");
                this.LineWeight = this.AddColumn(VA.ShapeSheet.SRCConstants.LineWeight, "LineWeight");
                this.Rounding = this.AddColumn(VA.ShapeSheet.SRCConstants.Rounding, "Rounding");
            }
        }
    }
}