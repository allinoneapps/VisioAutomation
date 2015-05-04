using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VisioPowerShell
{
    public class CellNameDictionary<T>
    {
        private Dictionary<string, T> dic;
        private System.Text.RegularExpressions.Regex regex_cellname;
        private System.Text.RegularExpressions.Regex regex_cellname_wildcard;

        public CellNameDictionary()
        {
            this.regex_cellname = new System.Text.RegularExpressions.Regex("^[a-zA-Z]*$");
            this.regex_cellname_wildcard = new System.Text.RegularExpressions.Regex("^[a-zA-Z\\*\\?]*$");
            this.dic = new Dictionary<string, T>(System.StringComparer.OrdinalIgnoreCase);
        }

        public List<string> GetNames()
        {
            return this.CellNames.ToList();
        }

        public T this[string name]
        {
            get { return this.dic[name]; }
            set
            {
                this.CheckCellName(name);

                if (dic.ContainsKey(name))
                {
                    string msg = string.Format("CellMap already contains a cell called \"{0}\"", name);
                    throw new System.ArgumentOutOfRangeException(msg);
                }

                this.dic[name] = value;
            }
        }

        public Dictionary<string, T>.KeyCollection CellNames
        {
            get { return this.dic.Keys; }
        }

        public bool IsValidCellName(string name)
        {
            return this.regex_cellname.IsMatch(name);
        }

        public bool IsValidCellNameWildCard(string name)
        {
            return this.regex_cellname_wildcard.IsMatch(name);
        }


        public void CheckCellName(string name)
        {
            if (this.IsValidCellName(name))
            {
                return;
            }

            string msg = string.Format("Cell name \"{0}\" is not valid", name);
            throw new System.ArgumentOutOfRangeException(msg);
        }

        public void CheckCellNameWildcard(string name)
        {
            if (this.IsValidCellNameWildCard(name))
            {
                return;
            }

            string msg = string.Format("Cell name wildcard pattern \"{0}\" is not valid", name);
            throw new System.ArgumentException(msg, "name");
        }

        public IEnumerable<string> ResolveName(string cellname)
        {
            if (cellname.Contains("*") || cellname.Contains("?"))
            {
                this.CheckCellNameWildcard(cellname);

                var regex = GetRegexForWildCardPattern(cellname);

                foreach (string k in this.CellNames)
                {
                    if (regex.IsMatch(k))
                    {
                        yield return k;
                    }
                }
            }
            else
            {
                this.CheckCellName(cellname);
                if (this.dic.ContainsKey(cellname))
                {
                    // found the exact cell name, yield it
                    yield return cellname;
                }
                else
                {
                    // Coudn't find the exact cell name, yield nothing
                    yield break;
                }
            }
        }

        private static Regex GetRegexForWildCardPattern(string cellname)
        {
            string pat = "^" + System.Text.RegularExpressions.Regex.Escape(cellname)
                .Replace(@"\*", ".*").
                Replace(@"\?", ".") + "$";

            var regex = new System.Text.RegularExpressions.Regex(pat,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return regex;
        }

        public IEnumerable<string> ResolveNames(IEnumerable<string> cellnames)
        {
            foreach (var name in cellnames)
            {
                foreach (var resolved_name in this.ResolveName(name))
                {
                    yield return resolved_name;
                }
            }
        }

        public bool ContainsCell(string name)
        {
            return this.dic.ContainsKey(name);
        }

    }
}