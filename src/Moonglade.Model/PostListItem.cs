﻿using System;
using System.Collections.Generic;

namespace Moonglade.Model
{
    public class PostListItem
    {
        public DateTime PubDateUtc { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public string ContentAbstract { get; set; }

        public List<TagInfo> Tags { get; set; }
    }
}