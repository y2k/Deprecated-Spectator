using System;

namespace SQLite.Net.Attributes
{
    class TableAttribute : Attribute
    {
        private string v;

        public TableAttribute(string v)
        {
            this.v = v;
        }
    }
}