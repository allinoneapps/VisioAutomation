using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisioAutomation.Extensions;
using System.Linq;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;

namespace TestVisioAutomation
{
    [TestClass]
    public class MetadataTests : VisioAutomationTest
    {
        [TestMethod]
        public void CheckPersistance()
        {
            string output_path = TestCommon.Globals.Helper.GetTestMethodOutputFilename();

            if (!System.IO.Directory.Exists(output_path))
            {
                System.IO.Directory.CreateDirectory(output_path);
            }
            var db = VA.Metadata.MetadataDB.Load();
            db.Save(output_path);
        }

        [TestMethod]
        public void VerifyItemCounts()
        {
            var db = VA.Metadata.MetadataDB.Load();

            var allcells = db.Cells;

            var dupe_cell_names = TestCommon.Helper.GetDuplicates(allcells.Select(c => c.Name));
            Assert.IsTrue(dupe_cell_names.Contains("Tabs"));

            Assert.AreEqual(351, allcells.Count);

            var visio_2007_cells = allcells.Where(c => c.MinVersion.Contains("Visio2007")).ToList();
            Assert.AreEqual(342, visio_2007_cells.Count());

            TestCommon.Helper.AssertNoDuplicates(allcells.Select(c => c.ID));

            var constants = db.Constants;

            // There are 3003 known constants in the Visio 2007 PIA
            // There are 3048 known constants in the Visio 2007 PIA
            Assert.AreEqual(3348, constants.Count);

            var sections = db.Sections;

            // There are 40 known sections in the Visio PIA
            Assert.AreEqual(40, sections.Count);

            var cellvals = db.CellValues;

            // There are 397 known sections in the Visio 2010 PIA
            Assert.AreEqual(397, cellvals.Count);
        }

        [TestMethod]
        public void ValidateCellNameCodes()
        {
            var db = VA.Metadata.MetadataDB.Load();

            var cellvals = db.CellValues;
            var allcells = db.Cells;
            var visio_2007_cells = allcells.Where(c => c.MinVersion.Contains("Visio2007")).ToList();

            var va_name_to_src = VA.ShapeSheet.SRCConstants.GetSRCDictionary();
            Assert.IsTrue(va_name_to_src.Count>300);

            TestCommon.Helper.AssertNoDuplicates(visio_2007_cells.Select(i => i.NameCode));

            var db_codename_to_cell = visio_2007_cells.ToDictionary(i => i.NameCode, i => i);

            var unfound = new List<string>();

            // verify that all the fields in SRCConstants are corrected represented in the metadata
            foreach (var va_name in va_name_to_src.Keys)
            {
                if (!db_codename_to_cell.ContainsKey(va_name))
                {
                    unfound.Add(va_name);
                }
            }

            if (unfound.Count > 0)
            {
                string message = string.Format(" didn't find in DB cells " + string.Join(",", unfound));
            }

            unfound.Clear();
            // verify that all the fields in db are corrected represented in the VA srcconstants
            foreach (var db_name in db_codename_to_cell.Keys)
            {
                if (!va_name_to_src.ContainsKey(db_name))
                {
                    unfound.Add(db_name);
                }
            }

            if (unfound.Count > 0)
            {
                string message = string.Format(" didn't find in src constants " + string.Join(",", unfound));
            }


            int x = 1;
        }

        [TestMethod]
        public void CheckPIA()
        {
            var db = VA.Metadata.MetadataDB.Load();
            var db_autoenums = db.AutomationEnums;

            var pia_enums = VA.Interop.InteropHelper.GetEnums().Select(i=>i.Type);

            var db_name_to_enum = db_autoenums.ToDictionary(i => i.Name, i => i);
            foreach (var pia_enum in pia_enums)
            {
                Assert.IsTrue(db_name_to_enum.ContainsKey(pia_enum.Name));
            }

            // verify that everying in the metadatadb is int the PIA 

            foreach (var pia_enum in pia_enums)
            {
                var pia_enum_values = TestCommon.Helper.EnumToDictionary<int>(pia_enum);
                var db_enum = db.GetAutomationEnumByName(pia_enum.Name);
                foreach (string pia_value_name in pia_enum_values.Keys)
                {
                    Assert.IsTrue(db_enum.HasItem(pia_value_name));
                    Assert.AreEqual(pia_enum_values[pia_value_name], db_enum[pia_value_name]);
                }
            }


            // verify that everying in the PIA is int the metadatadb

            var name_to_pia_type = pia_enums.ToDictionary(i => i.Name, i => i);

            foreach (var md_enum  in db.AutomationEnums)
            {
                var pia_type = name_to_pia_type[md_enum.Name];
                var pia_dic = TestCommon.Helper.EnumToDictionary<int>(pia_type);
                foreach (string md_value_name in md_enum.Items.Select(i => i.Name))
                {
                    Assert.IsTrue(pia_dic.ContainsKey(md_value_name));
                    Assert.AreEqual(md_enum[md_value_name], pia_dic[md_value_name]);
                }
            }
        }
        [TestMethod]
        public void CheckV2010()
        {
            var db = VA.Metadata.MetadataDB.Load();
            var visio_2010_cells = db.Cells.Where(c => c.MinVersion.Contains("Visio2010")).ToList();
            Assert.AreEqual(6,visio_2010_cells.Count);
        }

        [TestMethod]
        public void CheckSRCConstantIndices()
        {
            var db = VA.Metadata.MetadataDB.Load();
            var all_cells = db.Cells;
            var visio_cells = all_cells.Where(c => c.MinVersion.Contains("Visio2007") || c.MinVersion.Contains("Visio2010")).ToList();
            var va_name_to_src = VA.ShapeSheet.SRCConstants.GetSRCDictionary();
            var db_name_to_cell = visio_cells.ToDictionary(c => c.NameCode, c => c);
            foreach (string name in va_name_to_src.Keys)
            {
                if (!db_name_to_cell.ContainsKey(name))
                {
                    string msg = string.Format("DB does not contain cell with namecode \"{0}\"", name);
                    Assert.Fail(msg);
                }
            }

            var sectioindexname_to_int = TestCommon.Helper.EnumToDictionary<int>(typeof(IVisio.VisSectionIndices));
            var rowindexname_to_int = TestCommon.Helper.EnumToDictionary<int>(typeof(IVisio.VisRowIndices));
            var cellindexname_to_int = TestCommon.Helper.EnumToDictionary<int>(typeof(IVisio.VisCellIndices));
            foreach (var db_cell in visio_cells)
            {
                if (!sectioindexname_to_int.ContainsKey(db_cell.SectionIndex))
                {
                    Assert.Fail(db_cell.Name);
                }
                if (!rowindexname_to_int.ContainsKey(db_cell.RowIndex))
                {
                    Assert.Fail(db_cell.Name);
                }
                if (!cellindexname_to_int.ContainsKey(db_cell.CellIndex))
                {
                    Assert.Fail(db_cell.CellIndex + " " + db_cell.Name);
                }

                int s = sectioindexname_to_int[db_cell.SectionIndex];
                int r = rowindexname_to_int[db_cell.RowIndex];
                int c = cellindexname_to_int[db_cell.CellIndex];

                if (!va_name_to_src.ContainsKey(db_cell.NameCode))
                {
                    Assert.Fail(db_cell.NameCode);
                }
            }
        }


        [TestMethod]
        public void CheckSectionIndices()
        {
            var db = VA.Metadata.MetadataDB.Load();

            // verify that each section has an sectioindex enum that is found in the database
            foreach (var section in db.Sections)
            {
                string secindex_name = section.Enum;
                int secindex_int = db.GetAutomationConstantByName(secindex_name).GetValueAsInt();
            }
        }

        [TestMethod]
        public void CheckDBCellsAgainstVACode()
        {
            var db = VA.Metadata.MetadataDB.Load();
            var all_cells = db.Cells;
            var visio_2007_cells = all_cells.Where(c => c.MinVersion.Contains("Visio2007")).ToList();

            foreach (var db_cell in visio_2007_cells)
            {
                var md_section = db.GetAutomationConstantByName(db_cell.SectionIndex);
                var md_row = db.GetAutomationConstantByName(db_cell.RowIndex);
                var md_cell = db.GetAutomationConstantByName(db_cell.CellIndex);

                var s = (short) md_section.GetValueAsInt();
                var r = (short) md_row.GetValueAsInt();
                var c = (short) md_cell.GetValueAsInt();

                if (s == (short) IVisio.VisSectionIndices.visSectionTab)
                {
                    continue;
                }

                if (s == (short) IVisio.VisSectionIndices.visSectionUser)
                {
                    continue;
                }

                if (s == (short) IVisio.VisSectionIndices.visSectionFirstComponent)
                {
                    continue;
                }

                if (s == (short) IVisio.VisSectionIndices.visSectionLayer)
                {
                    continue;
                }

                var src = new VA.ShapeSheet.SRC(s, r, c);

                // Verify that the VisioAutomation library can find this cell
                var va_cellname = VA.ShapeSheet.ShapeSheetHelper.TryGetNameFromSRC(src);
                if (va_cellname == null)
                {
                    string msg = string.Format(@" DB Cell not found in VisioAutomation: ""{0}"" ({1},{2},{3}) ",
                                               db_cell.Name,
                                               md_section.Name,
                                               md_row.Name,
                                               md_cell.Name
                        );
                    Assert.Fail(msg);
                }

                if (va_cellname != db_cell.Name)
                {
                    string msg0 = string.Format(@" ""{0}"" ({1},{2},{3}) ",
                                                db_cell.Name,
                                                md_section.Name,
                                                md_row.Name,
                                                md_cell.Name);
                    string msg = string.Format(@" DB Cell has different name than in VisioAutomation: ""{0}"" ""{1}"" ",
                                               db_cell.Name, va_cellname
                        );
                    Assert.Fail(msg + msg0);
                }
            }
            // end
        }

        [TestMethod]
        public void ExportMetadataCode()
        {
            var db = VA.Metadata.MetadataDB.Load();

            string filename = TestCommon.Globals.Helper.GetTestMethodOutputFilename(".txt");

            db.ExportCode(filename);
        }
    }
}