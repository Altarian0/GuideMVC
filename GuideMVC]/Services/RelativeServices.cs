using System.Collections.Generic;
using System.Linq;
using GuideMVC_.Helpers;
using GuideMVC_.Interfaces;
using GuideMVC_.Models;
using Microsoft.EntityFrameworkCore;

namespace GuideMVC_.Services
{
    public class RelativeService : IRelativeService
    {
        private readonly GuideDBContext _db;
        private static List<UserRelative> _relatives;


        public RelativeService(GuideDBContext db)
        {
            _db = db;
            _relatives = _db.UserRelatives
                .Include(n => n.FromPerson)
                .Include(n => n.ToPerson)
                .Include(n => n.Marriage)
                .ToList();
            ;
        }

        public List<PersonView> GetSiblings(int personId)
        {
            var relatives = _relatives
                .Where(n => n.RelativeTypeId == (int) RelativeTypes.Siblings).ToList();
            var from = relatives.Where(n => n.FromUserId != personId)
                .Select(n => n.FromPerson).ToList();
            var to = relatives.Where(n => n.ToUserId != personId)
                .Select(n => n.ToPerson).ToList();
            var siblings = from.Union(to).ToList();

            var parents = GetParents(personId);
            var parentsChildren = new List<Person>();
            parents.ForEach(parent =>
            {
                parentsChildren = parentsChildren.Union(GetPersonChildren(parent.Id)).ToList();
            });
            parentsChildren.Remove(parentsChildren.FirstOrDefault(n => n.Id == personId));
            List<PersonView> personViews = new List<PersonView>();
            parentsChildren.ForEach(n => personViews.Add(new PersonView() {Person = n, IsParentChild = true}));
            siblings.ForEach(n => personViews.Add(new PersonView() {Person = n}));
            return personViews.Distinct().ToList();
        }

        public List<Person> GetParents(int childId)
        {
            var relatives = _relatives
                .Where(n => n.FromUserId == childId || n.ToUserId == childId)
                .Where(n => n.RelativeTypeId == (int) Helpers.RelativeTypes.Parents)
                .ToList();
            var parents = relatives.Where(n => n.FromUserId != childId)
                .Select(n => n.FromPerson).ToList();
            return parents;
        }

        public List<Person> GetChildren(int parentId)
        {
            var children = GetPersonChildren(parentId);
            var spouseChildren = GetPersonChildren(GetSpouse(parentId).Id);

            return children.Union(spouseChildren).ToList();
        }

        public List<PersonView> GetChildrenViews(int parentId)
        {
            var childrenPv = new List<PersonView>();
            var spouseChildrenPv = new List<PersonView>();
            var marriageChildrenPv = new List<PersonView>();

            var spouse = GetSpouse(parentId);
            var children = GetPersonChildren(parentId);
            var spouseChildren = spouse == null ? new List<Person>() : GetPersonChildren(spouse.Id);

            children.ForEach(n => { childrenPv.Add(new PersonView() {Person = n, IsParentChild = true}); });
            spouseChildren.ForEach(n =>
            {
                spouseChildrenPv.Add(new PersonView() {Person = n, IsParentChild = false});
            });


            var marriages = GetMarriages(parentId);
            if (marriages != null)
            {
                foreach (var marriage in marriages)
                {
                    var marriageRelatives = marriage.Relatives
                        .Where(n => n.RelativeTypeId == (int) RelativeTypes.Parents);
                    var marriageChildren = marriageRelatives.Select(n => n.ToPerson).ToList();

                    marriageChildren.ForEach(n =>
                    {
                        marriageChildrenPv.Add(new PersonView() {Person = n, IsParentChild = false});
                    });
                }
            }

            var list = childrenPv.Union(spouseChildrenPv).Union(marriageChildrenPv).Distinct().ToList();
            return list;
        }

        public List<Person> GetPersonChildren(int parentId)
        {
            var relatives = _relatives
                .Where(n => n.FromUserId == parentId || n.ToUserId == parentId)
                .Where(n => n.RelativeTypeId == (int) Helpers.RelativeTypes.Parents)
                .ToList();
            var children = relatives.Where(n => n.ToUserId != parentId)
                .Select(n => n.ToPerson).ToList();
            return children;
        }

        public List<Person> GetGrandchildren(int grandparentId)
        {
            var children = GetPersonChildren(grandparentId);
            var grandchildren = new List<Person>();
            children.ForEach(child => { grandchildren = grandchildren.Union(GetPersonChildren(child.Id)).ToList(); });
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

        public Person GetSpouse(int personId)
        {
            var relative = _relatives
                .Where(n => n.FromUserId == personId || n.ToUserId == personId)
                .Where(n => n.MarriageId != null && n.Marriage.IsDivorced != true)
                .FirstOrDefault(n => n.RelativeTypeId == (int) RelativeTypes.Spouse);
            if (relative is null)
                return null;
            var spouse = relative.FromUserId == personId ? relative.ToPerson : relative.FromPerson;
            return spouse;
        }

        public Marriage GetMarriage(int personId)
        {
            var relative = _relatives
                .Where(n => n.FromUserId == personId || n.ToUserId == personId)
                .Where(n => n.MarriageId != null && n.Marriage.IsDivorced != true)
                .FirstOrDefault(n => n.RelativeTypeId == (int) RelativeTypes.Spouse);
            var marriage = relative?.Marriage;
            return marriage;
        }

        public List<Marriage> GetMarriages(int personId)
        {
            var relatives = _relatives
                .Where(n => n.FromUserId == personId || n.ToUserId == personId)
                .Where(n => n.MarriageId != null)
                .Where(n => n.RelativeTypeId == (int) RelativeTypes.Spouse).ToList();
            var marriages = relatives.Select(n => n.Marriage).ToList();
            return marriages;
        }
    }
}