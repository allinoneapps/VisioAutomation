﻿using System;
using System.Collections.Generic;
using System.Linq;
using VA=VisioAutomation;

namespace VisioAutomation.Metadata.CodeGen
{
    public class CellGroup
    {
        public string Name;
        public string Parent;
        public Type DataType;
        public bool ForSection;
        public List<VA.Metadata.CodeGen.CellGroupMember> Cells;

        public CellGroup(string name)
        {
            this.Name=name;

            this.Cells = new List<CellGroupMember>();
            this.ForSection = false;
        }

        public string GenCode()
        {
            this.Parent = this.ForSection ? "CellSectionDataGroup" : "CellDataGroup";
            var sb = new System.Text.StringBuilder();
            this.Start(sb);
            sb.AppendFormat("\r\n");
            this.ApplyFunc(sb);
            sb.AppendFormat("\r\n");
            this.CellsFromRow(sb);
            sb.AppendFormat("\r\n");

            string rt_a;
            string rt_b;


            if (this.ForSection)
            {
                rt_a = string.Format("IList< List<{0}>>", this.Name);
                rt_b = string.Format("List<{0}>", this.Name);
            }
            else
            {
                rt_a = string.Format("IList<{0}>", this.Name);
                rt_b = string.Format("{0}", this.Name);
            }
            
            sb.AppendFormat("\tinternal static {0} GetCells(IVisio.Page page, IList<int> shapeids)\r\n", rt_a);
            sb.AppendFormat("\t{{\r\n");
            sb.AppendFormat("\tvar query = new ShapeFormatQuery();\r\n");
            sb.AppendFormat("\treturn {0}._GetCells(page, shapeids, query, get_cells_from_row);\r\n", this.Parent);
            sb.AppendFormat("\t}}\r\n");

            sb.AppendFormat("\tinternal static {0} GetCells(IVisio.Shape shape)\r\n", rt_b);
            sb.AppendFormat("\t{{\r\n");
            sb.AppendFormat("\tvar query = new ShapeFormatQuery();\r\n");
            sb.AppendFormat("\treturn {0}._GetCells(page, shapeids, query, get_cells_from_row);\r\n", this.Parent);
            sb.AppendFormat("\t}}\r\n");                

            this.Query(sb);
            this.End(sb);

            return sb.ToString();
        }
        //        public VA.ShapeSheet.CellData<int> FillBkgnd { get; set; }

        private void Start(System.Text.StringBuilder sb)
        {
            sb.AppendFormat("public class {0} : {1}\r\n", this.Name, this.Parent);
            sb.AppendFormat("{{\r\n");
            foreach (var cell in this.Cells)
            {
                sb.AppendFormat("\tpublic VA.ShapeSheet.CellData<{0}> {1} {{get;set;}}\r\n",cell.DataTypeName,cell.MemberName);
            }
        }

        private void End(System.Text.StringBuilder sb)
        {
            sb.AppendFormat("}}\r\n");
        }

        private void ApplyFunc(System.Text.StringBuilder sb)
        {
            if (this.ForSection)
            {
                sb.AppendFormat("\tprotected override void _Apply(VA.ShapeSheet.CellDataGroup.ApplyFormula func, short row)\r\n");
                
            }
            else
            {
                sb.AppendFormat("\tprotected override void _Apply(VA.ShapeSheet.CellDataGroup.ApplyFormula func)\r\n");
            }
            sb.AppendFormat("\t{{\r\n");
            foreach (var cell in this.Cells)
            {
                if (this.ForSection)
                {
                    sb.AppendFormat("\t\tfunc(ShapeSheet.SRCConstants.{0}.ForRow(row), this.{1}.Formula);\r\n", cell.Cell.NameCode, cell.MemberName);
                    
                }
                else
                {
                    sb.AppendFormat("\t\tfunc(ShapeSheet.SRCConstants.{0}, this.{1}.Formula);\r\n", cell.Cell.NameCode,  cell.MemberName);
                }
            }
            sb.AppendFormat("\t}}\r\n");
        }

        private void CellsFromRow(System.Text.StringBuilder sb)
        {
            sb.AppendFormat("\tprivate static ShapeFormatCells get_cells_from_row(ShapeFormatQuery query, VA.ShapeSheet.Query.QueryDataSet<double> qds, int row)\r\n");
            sb.AppendFormat("\t{{\r\n");
            sb.AppendFormat("\t\tvar cells = new {0}();;\r\n", this.Name);
            foreach (var cell in this.Cells)
            {
                string to = "To"+cell.DataTypeName.Substring(0, 1).ToUpper() + cell.DataTypeName.Substring(1);
                sb.AppendFormat("\t\tcells.{0}= qds.GetItem(row, query.{0}).{1}();\r\n", cell.MemberName,to);
            }
            sb.AppendFormat("\t}}\r\n");
        }

        public void Add(string name, VA.Metadata.Cell cell, string datatype)
        {
            var m = new VA.Metadata.CodeGen.CellGroupMember();
            m.MemberName = name;
            m.Cell = cell;
            m.DataTypeName = datatype;
            this.Cells.Add(m);
        }

        public void Add(VA.Metadata.Cell cell, string datatype)
        {
            this.Add(cell.NameCode,cell,datatype);
        }

        private void Query(System.Text.StringBuilder sb)
        {
            string Queryname = this.Name + "Query";
            sb.AppendFormat("\tclass {0}{{\r\n",Queryname);
            sb.AppendFormat("\t{{\r\n");
            foreach (var cell in this.Cells)
            {
                sb.AppendFormat("\t\tpublic VA.ShapeSheet.Query.CellQueryColumn {0} {{ get; set; }};\r\n", cell.MemberName);
            }
            sb.AppendFormat("\t\r\n");
            sb.AppendFormat("\t\tpublic {0}() : base()\r\n\t\t{{\r\n", Queryname);
            foreach (var cell in this.Cells)
            {
                sb.AppendFormat("\t\t\tthis.{0} = this.AddColumn(VA.ShapeSheet.SRCConstants.{1}, \"{2}\" );\r\n", cell.MemberName,
                                cell.Cell.NameCode, cell.MemberName);

            }
            sb.AppendFormat("\t\t}}\r\n");

            sb.AppendFormat("\t}}\r\n");
        }

    }
}
