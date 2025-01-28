using System;
using System.Collections.Generic;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.API.Models
{
    public class TagSelectableView
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public string Icon { get; set; }
        public TagView Tag { get; set; }
        public int OrderIndex { get; set; }
        public string ExcludedTags { get; set; }
        public bool DontAddTag { get; set; }
        public List<TagSelectableCategoryView> Categories { get; set; }
    }
    public class TagSelectableCategoryView
    {
        public int Id { get; set; }
        //public int TitleId { get; set; }
        public string Title { get; set; }
        //public int? TextId { get; set; }
        public string Text { get; set; }
        public string ExcludedTags { get; set; }
        public List<TagSelectableView> TagSelectables { get; set; }
    }
    public class TagSelectableChoiceOrderView
    {
        public int Id { get; set; }
        public int? TagId { get; set; }
        public int? RelatedChoiceId { get; set; }
        public TagView Tag { get; set; }
        public int OrderIndex { get; set; }
        public int TagSelectableCategoryId { get; set; }
        public TagSelectableCategoryView TagSelectableCategory { get; set; }
    }

}
