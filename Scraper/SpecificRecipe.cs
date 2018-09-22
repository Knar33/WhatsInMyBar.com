using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    public class SpecificRecipe
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
        public Title title { get; set; }
        public int author { get; set; }
        public string comment_status { get; set; }
        public string ping_status { get; set; }
        public string template { get; set; }
        public object[] meta { get; set; }
        public Description description { get; set; }
        public Caption caption { get; set; }
        public string alt_text { get; set; }
        public string media_type { get; set; }
        public string mime_type { get; set; }
        public Media_Details media_details { get; set; }
        public int post { get; set; }
        public string source_url { get; set; }
        public _Links _links { get; set; }

        public class Guid
        {
            public string rendered { get; set; }
        }

        public class Title
        {
            public string rendered { get; set; }
        }

        public class Description
        {
            public string rendered { get; set; }
        }

        public class Caption
        {
            public string rendered { get; set; }
        }

        public class Media_Details
        {
            public int width { get; set; }
            public int height { get; set; }
            public string file { get; set; }
            public Image_Meta image_meta { get; set; }
        }

        public class Image_Meta
        {
            public string aperture { get; set; }
            public string credit { get; set; }
            public string camera { get; set; }
            public string caption { get; set; }
            public string created_timestamp { get; set; }
            public string copyright { get; set; }
            public string focal_length { get; set; }
            public string iso { get; set; }
            public string shutter_speed { get; set; }
            public string title { get; set; }
            public string orientation { get; set; }
            public object[] keywords { get; set; }
        }

        public class _Links
        {
            public Self[] self { get; set; }
            public Collection[] collection { get; set; }
            public About[] about { get; set; }
            public Author[] author { get; set; }
            public Reply[] replies { get; set; }
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
    }

    

}
