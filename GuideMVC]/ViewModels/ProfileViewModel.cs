using System.Collections.Generic;
using System.Linq;
using GuideMVC_.Helpers;
using GuideMVC_.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GuideMVC_.ViewModels
{
    public class ProfileViewModel
    {
        public Person Person { get; set; }
        public Person Father => GetFather(Person.Id);
        public Person Mother => GetMother(Person.Id);
        public Person Grandfather => GetFather(GetFather(Person.Id).Id);
        public Person Grandmother => GetMother(Mother.Id);
        public List<UserRelative> Relatives { get; set; }

        public List<Person> Siblings
        {
            get
            {
                var relatives = Relatives
                    .Where(n => n.RelativeTypeId == (int) RelativeTypes.Siblings).ToList();
                var from = relatives.Where(n => n.FromUserId != Person.Id)
                    .Select(n => n.FromPerson).ToList();
                var to = relatives.Where(n => n.ToUserId != Person.Id)
                    .Select(n => n.ToPerson).ToList();
                var siblings = from.Union(to).ToList();
                return siblings;
            }
        }

        public List<Person> GetParents(int childId)
        {
            var relatives = Relatives
                .Where(n => n.FromUserId == childId || n.ToUserId == childId)
                .Where(n => n.RelativeTypeId == (int) Helpers.RelativeTypes.Parents)
                .ToList();
            var parents = relatives.Where(n => n.FromUserId != childId)
                .Select(n => n.FromPerson).ToList();
            return parents;
        }

        public List<Person> GetChildren(int parentId)
        {
            var relatives = Relatives
                .Where(n => n.FromUserId == parentId || n.ToUserId == parentId)
                .Where(n => n.RelativeTypeId == (int) Helpers.RelativeTypes.Parents)
                .ToList();
            var children = relatives.Where(n => n.ToUserId != parentId)
                .Select(n => n.ToPerson).ToList();
            return children;
        }
        
        public List<Person> GetGrandchildren(int grandparentId)
        {
            var children = GetChildren(grandparentId);
            var grandchildren = new List<Person>();
            children.ForEach(child =>
            {
                grandchildren = grandchildren.Union(GetChildren(child.Id)).ToList();
            });
            return grandchildren;
        }

        public Person GetFather(int childId)
        {
            var parents = GetParents(childId);
            return parents.FirstOrDefault(n => n.GenderId == 1) ?? new Person();
        }

        public Person GetMother(int childId)
        {
            var parents = GetParents(childId);
            return parents.FirstOrDefault(n => n.GenderId == 2) ?? new Person();
        }
    }
}