using System;

namespace DynamicDatabase.Meta
{
    public class DDColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public DDColumnAttribute(string name)
        {
            this.Name = name;
        }
    }
}
