﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    public class ProtoRecipeResponse
    {
        public List<ProtoRecipe> Property1 { get; set; }
    }

    public class ProtoRecipe
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public DateTime date_gmt { get; set; }
        public Guid guid { get; set; }
        public DateTime modified { get; set; }
        public DateTime modified_gmt { get; set; }
        public string slug { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public RecipeTitle title { get; set; }
        public Excerpt excerpt { get; set; }
        public int author { get; set; }
        public int featured_media { get; set; }
        public string comment_status { get; set; }
        public string ping_status { get; set; }
        public string template { get; set; }
        public List<object> meta { get; set; }
        public List<object> categories { get; set; }
        public _Links _links { get; set; }
    }

    public class Guid
    {
        public string rendered { get; set; }
    }

    public class RecipeTitle
    {
        public string rendered { get; set; }
    }

    public class Excerpt
    {
        public string rendered { get; set; }
        public bool _protected { get; set; }
    }

    public class _Links
    {
        public List<Self> self { get; set; }
        public List<Collection> collection { get; set; }
        public List<About> about { get; set; }
        public List<Author> author { get; set; }
        public List<Reply> replies { get; set; }
        public List<WpAttachment> wpattachment { get; set; }
        public List<WpTerm> wpterm { get; set; }
        public List<Cury> curies { get; set; }
        public List<WpFeaturedmedia> wpfeaturedmedia { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Collection
    {
        public string href { get; set; }
    }

    public class About
    {
        public string href { get; set; }
    }

    public class Author
    {
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class Reply
    {
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class WpAttachment
    {
        public string href { get; set; }
    }

    public class WpTerm
    {
        public string taxonomy { get; set; }
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class Cury
    {
        public string name { get; set; }
        public string href { get; set; }
        public bool templated { get; set; }
    }

    public class WpFeaturedmedia
    {
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

}
