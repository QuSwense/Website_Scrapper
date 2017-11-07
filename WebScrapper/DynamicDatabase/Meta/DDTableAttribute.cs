using System;

namespace DynamicDatabase.Meta
{
    public class DDTableAttribute : Attribute
    {
        public string Name { get; set; }

        public DDTableAttribute(string name)
        {
            this.Name = name;
        }
    }
}
