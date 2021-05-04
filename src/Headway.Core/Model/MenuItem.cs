﻿using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class MenuItem
    {
        public MenuItem()
        {
            Rights = new List<string>();
        }

        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string ImageClass { get; set; }
        public string Path { get; set; }
        public List<string> Rights { get; set; }
    }
}