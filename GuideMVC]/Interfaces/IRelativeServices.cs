using System.Collections.Generic;
using GuideMVC_.Models;

namespace GuideMVC_.Interfaces
{
    public interface IRelativeService
    {
        List<PersonView> GetSiblings(int personId);
        List<Person> GetParents(int childId);
        List<Person> GetChildren(int parentId);
        List<Person> GetPersonChildren(int parentId);
        List<PersonView> GetChildrenViews(int parentId);
        List<Person> GetGrandchildren(int grandparentId);
        Person GetFather(int childId);
        Person GetMother(int childId);
        Person GetSpouse(int personId);
        Marriage GetMarriage(int personId);
        List<Marriage> GetMarriages(int personId);
    }
}